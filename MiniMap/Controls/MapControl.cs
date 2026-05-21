using System;
using System.ComponentModel;
using System.Windows.Forms;
using MiniMap.Geometry;

namespace MiniMap
{
	public partial class MapControl : UserControl
	{
		#region MapData
		private readonly Point _Md, _Mu, _Mc;
		private bool _isDown;
		#endregion

		#region ctor
		public MapControl()
		{
			InitializeComponent();

			_step = -1.0;
			_drawGrid = false;

			MapGraphics = new MapEngine(this, MapPicture);
			MapGraphics.ScaleChanged += new EventHandler(OnScaleChanged);
			MapGraphics.ExtendChanged += new EventHandler(OnExtendChanged);

			_isDown = false;
			_Md = new Point();
			_Mu = new Point();
			_Mc = new Point();

			Mode = ViewerMode.Pan;
		}
		#endregion

		#region utils
		public static void Inverse(Point Pt0, Point Pt1, out double distance, out double azimuth)
		{
			const double AWG = 6378137.0;
			const double FWG = 1.0 / 298.257223563;
			const double BWG = AWG * (1.0 - FWG);
			const double DegToRadValue = Math.PI / 180.0;
			const double epsilon = 1e-12;

			double u1 = Math.Atan((1 - FWG) * Math.Tan(Pt0.Y * DegToRadValue));
			double u2 = Math.Atan((1 - FWG) * Math.Tan(Pt1.Y * DegToRadValue));
			double L = (Pt1.X - Pt0.X) * DegToRadValue;

			double sinU1 = Math.Sin(u1), cosU1 = Math.Cos(u1);
			double sinU2 = Math.Sin(u2), cosU2 = Math.Cos(u2);

			double lambda = L;
			double lambdaP;
			double sinSigma, cosSigma, sigma, sinAlpha, cosSqAlpha, cos2SigmaM;
			int iterations = 0;

			do
			{
				double sinLambda = Math.Sin(lambda), cosLambda = Math.Cos(lambda);
				sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
								   (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));

				if (sinSigma == 0)
				{
					distance = azimuth = 0;
					return;
				}

				cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
				sigma = Math.Atan2(sinSigma, cosSigma);
				sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
				cosSqAlpha = 1 - sinAlpha * sinAlpha;

				cos2SigmaM = cosSqAlpha == 0 ? 0 : cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;

				double C = FWG / 16 * cosSqAlpha * (4 + FWG * (4 - 3 * cosSqAlpha));
				lambdaP = lambda;
				lambda = L + (1 - C) * FWG * sinAlpha *
						 (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

			} while (Math.Abs(lambda - lambdaP) > epsilon && ++iterations < 200);

			double uSq = cosSqAlpha * (AWG * AWG - BWG * BWG) / (BWG * BWG);
			// Коэффициенты Карни/Бесселя 4-го порядка
			double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
			double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

			double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
								B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));

			distance = BWG * A * (sigma - deltaSigma);
			double alpha1 = Math.Atan2(cosU2 * Math.Sin(lambda), cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(lambda));

			azimuth = (alpha1 / DegToRadValue + 360.0) % 360.0;
			return;
		}

		#endregion

		#region mouse and control events
		private void MapPicture_MouseDown(object sender, MouseEventArgs e)
		{
			Point ptp = new Point(e.X, e.Y);
			MouseCurrentCoord = MapGraphics.ProjectToMap(ptp);
			MouseDownCoord = (Point)MouseCurrentCoord.Clone();

			if (e.Button == MouseButtons.Left && !_isDown)
			{
				_isDown = true;
				_Mc.X = _Md.X = e.X;
				_Mc.Y = _Md.Y = e.Y;
			}

			var onMouseDown = this.GetType().GetMethod("OnMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			onMouseDown?.Invoke(this, new object[] { e });
		}

		private void MapPicture_MouseMove(object sender, MouseEventArgs e)
		{
			var OnMouseMove = this.GetType().GetMethod("OnMouseMove", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			Point ptp = new Point(e.X, e.Y);
			MouseCurrentCoord = MapGraphics.ProjectToMap(ptp);

			if (e.Button != MouseButtons.Left || Mode == ViewerMode.None)
			{
				OnMouseMove?.Invoke(this, new object[] { e });
				return;
			}

			if (_isDown)
			{
				Point mp = (Point)_Mc.Clone();

				if (Mode == ViewerMode.Pan)
				{
					MapGraphics.ShiftExtend(e.X - _Md.X, e.Y - _Md.Y);
					MapGraphics.RefreshGrafics();
					_Md.X = e.X;
					_Md.Y = e.Y;
					MouseDownCoord = (Point)MouseCurrentCoord.Clone();
				}
				else if (Mode == ViewerMode.ZoomIn || Mode == ViewerMode.ZoomOut)
				{
					if (mp.X != e.X && mp.Y != e.Y)
					{
						_Mc.X = e.X;
						_Mc.Y = e.Y;

						int x0 = (int)Math.Min(_Md.X, mp.X);
						int y0 = (int)Math.Min(_Md.Y, mp.Y);

						int x1 = (int)Math.Max(_Md.X, mp.X);
						int y1 = (int)Math.Max(_Md.Y, mp.Y);

						IntPtr hdc = XorDrawing.BeginDraw(MapPicture.Handle, out int oldRop, out IntPtr oldpen);
						XorDrawing.Rectangle(hdc, x0, y0, x1, y1);

						x0 = (int)Math.Min(_Md.X, _Mc.X);
						y0 = (int)Math.Min(_Md.Y, _Mc.Y);

						x1 = (int)Math.Max(_Md.X, _Mc.X);
						y1 = (int)Math.Max(_Md.Y, _Mc.Y);

						XorDrawing.Rectangle(hdc, x0, y0, x1, y1);
						XorDrawing.FinishDraw(MapPicture.Handle, hdc, oldRop, oldpen);
					}
				}
				else if (Mode == ViewerMode.Measure)
				{
					if (mp.X != e.X && mp.Y != e.Y)
					{
						_Mc.X = e.X;
						_Mc.Y = e.Y;

						int x0 = (int)_Md.X;
						int y0 = (int)_Md.Y;

						int x1 = (int)mp.X;
						int y1 = (int)mp.Y;

						IntPtr hdc = XorDrawing.BeginDraw(MapPicture.Handle, out int oldRop, out IntPtr oldpen);
						XorDrawing.MoveToEx(hdc, x0, y0, IntPtr.Zero);
						XorDrawing.LineTo(hdc, x1, y1);

						x1 = (int)_Mc.X;
						y1 = (int)_Mc.Y;

						XorDrawing.MoveToEx(hdc, x0, y0, IntPtr.Zero);
						XorDrawing.LineTo(hdc, x1, y1);

						XorDrawing.FinishDraw(MapPicture.Handle, hdc, oldRop, oldpen);
					}

					MeasureEvent?.Invoke(this, new MapEventArgs(this, MouseDownCoord, MouseCurrentCoord));
				}
			}

			OnMouseMove?.Invoke(this, new object[] { e });
		}

		private void MapPicture_MouseUp(object sender, MouseEventArgs e)
		{
			var OnMouseUp = this.GetType().GetMethod("OnMouseUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			Point ptp = new Point(e.X, e.Y);
			MouseCurrentCoord = MapGraphics.ProjectToMap(ptp);

			if (e.Button == MouseButtons.Left)
			{
				_isDown = false;
				_Mu.X = e.X;
				_Mu.Y = e.Y;
			}

			MouseUpCoord = (Point)MouseCurrentCoord.Clone();

			if (Mode == ViewerMode.ZoomIn || Mode == ViewerMode.ZoomOut)
			{
				int a = (int)Math.Abs(_Md.X - _Mu.X);
				int b = (int)Math.Abs(_Md.Y - _Mu.Y);

				if (a == 0 || b == 0)
				{
					OnMouseUp?.Invoke(this, new object[] { e });
					return;
				}

				Envelope extend = new Envelope();

				extend.ExpandToInclude(MouseDownCoord);
				extend.ExpandToInclude(MouseUpCoord);

				if (Mode == ViewerMode.ZoomOut)
				{
					Envelope graphicsExtend = MapGraphics.GetExtend();

					double dx = graphicsExtend.Width / extend.Width * (graphicsExtend.Width - extend.Width);
					double dy = graphicsExtend.Height / extend.Height * (graphicsExtend.Height - extend.Height);

					double MinX = extend.ptMin.X - dx;
					double MinY = extend.ptMin.Y - dy;

					double MaxX = extend.ptMax.X + dx;
					double MaxY = extend.ptMax.Y + dy;

					extend.SetEmpty();
					extend.ExpandToInclude(MinX, MinY);
					extend.ExpandToInclude(MaxX, MaxY);
				}

				MapGraphics.SetViewExtend(extend);
				MapGraphics.RefreshGrafics();
			}
			else if (Mode == ViewerMode.Pan)
				MapGraphics.SetViewExtend();
			else if (Mode == ViewerMode.Measure)
				MapGraphics.RefreshGrafics();

			OnMouseUp?.Invoke(this, new object[] { e });
		}

		private void MapPicture_MouseWheel(object sender, MouseEventArgs e)
		{
			const double scale_per_delta = 1.0 / 120.0;

			double scale = (e.Delta > 0 ? 0.75 : -1.25) * e.Delta * scale_per_delta;

			// calculate the new extend.
			Envelope graphicsExtend = MapGraphics.GetExtend();

			double MinX = MouseCurrentCoord.X + scale * (graphicsExtend.ptMin.X - MouseCurrentCoord.X);
			double MinY = MouseCurrentCoord.Y + scale * (graphicsExtend.ptMin.Y - MouseCurrentCoord.Y);

			double MaxX = MouseCurrentCoord.X + scale * (graphicsExtend.ptMax.X - MouseCurrentCoord.X);
			double MaxY = MouseCurrentCoord.Y + scale * (graphicsExtend.ptMax.Y - MouseCurrentCoord.Y);
			//============================================================================================

			Envelope extend = new Envelope();

			extend.ExpandToInclude(MinX, MinY);
			extend.ExpandToInclude(MaxX, MaxY);

			MapGraphics.SetViewExtend(extend);
			MapGraphics.RefreshGrafics();

			var OnMouseWheel = this.GetType().GetMethod("OnMouseWheel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			OnMouseWheel?.Invoke(this, new object[] { e });
		}

		private void MapPicture_SizeChanged(object sender, EventArgs e)
		{
			MapGraphics.RecreateBitmap();
		}
		#endregion

		#region events

		void OnScaleChanged(object sender, EventArgs e)
		{
			ScaleChanged?.Invoke(this, new EventArgs());
		}

		void OnExtendChanged(object sender, EventArgs e)
		{
			ExtendChanged?.Invoke(this, new EventArgs());
		}

		//===============================================
		/// <summary>
		/// Occurs when the user measures the distance and / or angle between two points.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public event MeasureEventHandler MeasureEvent;

		/// <summary>
		/// Occurs when map scale changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public event EventHandler ScaleChanged;

		/// <summary>
		/// Occurs when visible map extend changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public event EventHandler ExtendChanged;

		/// <summary>
		/// Occurs when viewer mode changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public event EventHandler ModeChanged;

		/// <summary>
		/// Occurs when map units changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public event EventHandler MapUnitsChanged;

		#endregion

		#region properties
		ViewerMode _mode;
		public ViewerMode Mode
		{
			get { return _mode; }

			set
			{
				if (_mode == value)
					return;
				_mode = value;
				ModeChanged?.Invoke(this, new EventArgs());
			}
		}

		private bool _drawGrid;
		public bool DrawGrid
		{
			get { return _drawGrid; }

			set
			{
				if (_drawGrid != value)
				{
					_drawGrid = value;
					MapGraphics.RefreshGrafics();
				}
			}
		}

		double _step;
		public double Step
		{
			get { return _step; }
			set
			{
				if (_step != value)
				{
					_step = value;
					if (_drawGrid)
						MapGraphics.RefreshGrafics();
				}
			}
		}

		[AmbientValue("")]
		public override Cursor Cursor
		{
			get { return MapPicture.Cursor; }
			set { MapPicture.Cursor = value; }
		}

		[Browsable(false)]
		public MapEngine MapGraphics { get; }

		[Browsable(false)]
		public Point MouseDownCoord { get; private set; }

		[Browsable(false)]
		public Point MouseUpCoord { get; private set; }

		[Browsable(false)]
		public Point MouseCurrentCoord { get; private set; }

		MapUnits _units;
		public MapUnits Units
		{
			get { return _units; }

			set
			{
				_units = value;
				MapUnitsChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		[Browsable(false)]
		public Envelope FullExtend { get { return MapGraphics.GetFullExtend(); } }
		#endregion
	}

	public class MapEventArgs
	{
		public Point FromPoint { get; }
		public Point ToPoint { get; }
		public double Distance { get; }
		public double Direction { get; }


		public MapEventArgs(MapControl controol, Point pt0, Point pt1)
		{
			FromPoint = (Point)pt0.Clone();
			ToPoint = (Point)pt1.Clone();
			if (controol.Units == MapUnits.Degrees)
			{
				MapControl.Inverse(pt0, pt1, out double distance, out double azimuth);
				Distance = distance;
				Direction = azimuth;
			}
			else
			{

				double dx = pt1.X - pt0.X;
				double dy = pt1.Y - pt0.Y;
				Distance = Math.Sqrt(dx * dx + dy * dy);
				Direction = ((Math.Atan2(dy, dx) * 180.0 / Math.PI) + 360.0) % 360.0;
			}
		}
	}

	public enum ViewerMode : int
	{
		None,
		Measure,
		Pan,
		ZoomIn,
		ZoomOut,
		Edit,
		Info,
		Select,
		SelectBox,
		Super
	}

	public enum MapUnits
	{
		None,	//Unknown
		Meters,
		Degrees
	}

	/// <summary>
	/// Represents the method that will handle the measure event
	///
	/// Parameters:
	///   sender<param name="sender"></param>:
	///     The source of the event.
	///
	///   args<param name="args"></param>:
	///     A MapEventArgs that contains the event data.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void MeasureEventHandler(object sender, MapEventArgs args);
}
