using MiniMap.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MiniMap
{
	public struct Style
	{
		public int Color;
		public int Size;
		public int Kind;
	}

	public class MapEngine	//View
	{
		/// <summary>
		/// initial map extend
		/// </summary>
		const double fieldPolySpan = 10000;

		private struct DrawingParams
		{
			public GeometryBase geometry;
			public GeometryType type;
			public Geometry.Point textPosition;
			public float textDirection;
			public bool drawVerts;
			public string text;

			public Style style;
		}

		#region variables
		private readonly MapControl _mapControl;
		private readonly Dictionary<int, DrawingParams> _drawedItems;

		private readonly PictureBox _screen;
		private Bitmap _bmp;
		private Graphics _gr;

		private double _xoffset;
		private double _yoffset;

		private readonly Envelope _fullExtend;
		private readonly Envelope _currentExtend;
		private List<Envelope> _viewStack;
		private int _currentView;

		private int _drawedCount;
		private bool _drawingVertex;
		#endregion

		#region properties
		public double xScale { get; set; }

		public double yScale { get; set; }

		public double Scale { get; private set; }
		#endregion

		#region ctor
		public MapEngine(MapControl control, PictureBox pictureBox)
		{
			_mapControl = control;
			_fullExtend = new Envelope();
			_currentExtend = new Envelope
			{
				ptMin = new Geometry.Point(-fieldPolySpan, -fieldPolySpan),
				ptMax = new Geometry.Point(fieldPolySpan, fieldPolySpan)
			};

			_screen = pictureBox;
			_currentView = -1;
			_viewStack = new List<Envelope>();
			_drawedItems = new Dictionary<int, DrawingParams>();
			_drawedCount = 1;
			_drawingVertex = false;

			Scale = xScale = yScale = 1.0;
			_bmp = new Bitmap(pictureBox.ClientRectangle.Width, pictureBox.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			_gr = Graphics.FromImage(_bmp);
			_gr.FillRectangle(Brushes.White, 0, 0, _bmp.Width, _bmp.Height);
		}
		#endregion

		#region Routines
		static Random rnd = new Random(DateTime.Now.Millisecond);

		public static int RandomColor()
		{
			int r = rnd.Next(256);
			int g = rnd.Next(256);
			int b = rnd.Next(256);

			return (b << 16) | (g << 8) | r;
		}

		public static int RGB(int r, int g, int b)
		{
			r &= 0xff;
			g &= 0xff;
			b &= 0xff;
			return (b << 0x10) | (g << 8) | r;
		}

		public static int InvRGB(Color color)
		{
			int r = 0xff - (color.R & 0xff);
			int g = 0xff - (color.G & 0xff);
			int b = 0xff - (color.B & 0xff);
			return (b << 0x10) | (g << 8) | r;
		}

		public static float RadToDeg(float x)
		{
			return (float)(x * 180.0 / Math.PI);
		}
		#endregion

		#region draw

		public int DrawPoint(Geometry.Point pt, int Size = 8, int iColor = -1)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = pt,
				textPosition = pt,
				type = GeometryType.Point,
				drawVerts = false,
				text = null,
				style = new Style { Color = iColor, Size = Size, Kind = 0 }
			};

			DrawPoint(draw);

			if (!_drawingVertex)
				_drawedItems.Add(_drawedCount++, draw);

			return _drawedCount;
		}

		public int DrawPoint(Geometry.Point pt, Style style)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = pt,
				textPosition = pt,
				type = GeometryType.Point,
				drawVerts = false,
				text = null,
				style = style
			};

			DrawPoint(draw);

			if (!_drawingVertex)
				_drawedItems.Add(_drawedCount++, draw);

			return _drawedCount;
		}

		public int DrawPointWithText(Geometry.Point pt, string text, int Size = 10, int iColor = -1)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = pt,
				textPosition = pt,
				type = GeometryType.Point,
				drawVerts = false,
				text = text,
				style = new Style { Color = iColor, Size = Size }
			};

			DrawPoint(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawPointWithText(Geometry.Point pt, string text, Style style)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = pt,
				textPosition = pt,
				type = GeometryType.Point,
				drawVerts = false,
				text = text,
				style = style
			};

			DrawPoint(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawPoint(DrawingParams draw)
		{
			Brush brush;
			if (_gr == null)
				_gr = Graphics.FromImage(_bmp);

			if (draw.style.Color < 0)
			{
				Random random = new Random();
				brush = new SolidBrush(Color.FromArgb(random.Next(0x100), random.Next(0x100), random.Next(0x100)));
			}
			else
				brush = new SolidBrush(Color.FromArgb(draw.style.Color & 0xff, (draw.style.Color >> 8) & 0xff, (draw.style.Color >> 0x10) & 0xff));

			Geometry.Point pt = (Geometry.Point)draw.geometry;
			_fullExtend.ExpandToInclude(pt);

			PointF point = ProjectToScr(pt);
			if (point.X < draw.style.Size || point.Y < draw.style.Size)
				return;

			if (point.X + draw.style.Size > _bmp.Width || point.Y + draw.style.Size > _bmp.Height)
				return;

			_gr.FillEllipse(brush, point.X - (0.5f * draw.style.Size), point.Y - (0.5f * draw.style.Size), draw.style.Size, draw.style.Size);

			if (!string.IsNullOrEmpty(draw.text))
			{
				Font font = new Font("Arial", draw.style.Size + 2f);
				point.X += 0.5f * draw.style.Size;
				point.Y -= 0.5f * draw.style.Size;
				_gr.DrawString(draw.text, font, Brushes.Black, point);
			}
		}

		public int DrawMultiPoint(MultiPoint mpt, int Size = 8, int iColor = -1)
		{
			double x = 0, y = 0;
			foreach (var pt in mpt)
			{
				x += pt.X;
				y += pt.Y;
			}
			Geometry.Point textPos = new Geometry.Point(x, y);

			DrawingParams draw = new DrawingParams
			{
				geometry = mpt,
				textPosition = textPos,
				type = GeometryType.MultiPoint,
				drawVerts = false,
				text = null,
				style = new Style { Color = iColor, Size = Size }
			};

			DrawMultiPoint(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawMultiPoint(MultiPoint mpt, Style style)
		{
			double x = 0, y = 0;
			foreach (var pt in mpt)
			{
				x += pt.X;
				y += pt.Y;
			}
			Geometry.Point textPos = new Geometry.Point(x, y);

			DrawingParams draw = new DrawingParams
			{
				geometry = mpt,
				textPosition = textPos,
				type = GeometryType.MultiPoint,
				drawVerts = false,
				text = null,
				style = style
			};

			DrawMultiPoint(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawMultiPoint(DrawingParams draw)
		{
			Brush brush;
			if (_gr == null)
				_gr = Graphics.FromImage(_bmp);

			if (draw.style.Color < 0)
			{
				Random random = new Random();
				brush = new SolidBrush(Color.FromArgb(random.Next(0x100), random.Next(0x100), random.Next(0x100)));
			}
			else
				brush = new SolidBrush(Color.FromArgb(draw.style.Color & 0xff, (draw.style.Color >> 8) & 0xff, (draw.style.Color >> 0x10) & 0xff));

			MultiPoint mpt = (MultiPoint)draw.geometry;

			foreach (var pt in mpt)
			{
				_fullExtend.ExpandToInclude(pt);

				PointF point = ProjectToScr(pt);
				if (point.X < draw.style.Size || point.Y < draw.style.Size)
					continue;

				if (point.X + draw.style.Size > _bmp.Width || point.Y + draw.style.Size > _bmp.Height)
					continue;

				_gr.FillEllipse(brush, point.X - (0.5f * draw.style.Size), point.Y - (0.5f * draw.style.Size), draw.style.Size, draw.style.Size);
			}
		}

		public int DrawLineString(LineString lineStr, int width = 1, int color = -1, bool markVerts = false)
		{
			double ltp = 0.5 * lineStr.Length;

			Geometry.Point textPos = new Geometry.Point();
			float dir = 0;
			Geometry.Point pt0 = lineStr[0], pt1;

			for (int i = 1; i < lineStr.Count; i++)
			{
				pt1 = lineStr[i];
				double dx = pt1.X - pt0.X;
				double dy = pt1.Y - pt0.Y;
				double lc = Math.Sqrt(dx * dx + dy * dy);
				if (lc > ltp)
				{
					double k = ltp / lc;
					textPos.X = pt0.X + k * dx;
					textPos.Y = pt0.Y + k * dy;

					dir = RadToDeg((float)Math.Atan2(-dy, dx));
					break;
				}
				pt0 = pt1;
				ltp -= lc;
			}

			DrawingParams draw = new DrawingParams
			{
				geometry = lineStr,
				textPosition = textPos,
				textDirection = dir,
				type = GeometryType.LineString,
				drawVerts = markVerts,
				text = lineStr.Name,
				style = new Style
				{
					Color = color,
					Size = width
				}
			};
			DrawLineString(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawLineString(LineString lineStr, Style style, bool markVerts = false)
		{
			double ltp = 0.5 * lineStr.Length;

			Geometry.Point textPos = new Geometry.Point();
			float dir = 0;
			Geometry.Point pt0 = lineStr[0], pt1;

			for (int i = 1; i < lineStr.Count; i++)
			{
				pt1 = lineStr[i];
				double dx = pt1.X - pt0.X;
				double dy = pt1.Y - pt0.Y;
				double lc = Math.Sqrt(dx * dx + dy * dy);
				if (lc > ltp)
				{
					double k = ltp / lc;
					textPos.X = pt0.X + k * dx;
					textPos.Y = pt0.Y + k * dy;

					dir = RadToDeg((float)Math.Atan2(-dy, dx));
					break;
				}
				pt0 = pt1;
				ltp -= lc;
			}

			DrawingParams draw = new DrawingParams
			{
				geometry = lineStr,
				textPosition = textPos,
				textDirection = dir,
				type = GeometryType.LineString,
				drawVerts = markVerts,
				text = lineStr.Name,
				style = style
			};
			DrawLineString(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawLineString(DrawingParams draw, bool isGridPart = false)
		{
			if (_gr == null)
				_gr = Graphics.FromImage(_bmp);

			Pen pen;

			if (draw.style.Color < 0)
			{
				Random random = new Random();
				pen = new Pen(Color.FromArgb(random.Next(0x100), random.Next(0x100), random.Next(0x100)), draw.style.Size);
			}
			else
				pen = new Pen(Color.FromArgb(draw.style.Color & 0xff, (draw.style.Color >> 8) & 0xff, (draw.style.Color >> 0x10) & 0xff), draw.style.Size);

			pen.DashStyle = (DashStyle)draw.style.Kind;
			pen.Width = draw.style.Size;

			LineString lineStr = (LineString)draw.geometry;

			int iColor = InvRGB(pen.Color);

			_drawingVertex = true;

			if (draw.drawVerts)
				DrawPoint(lineStr[0], 8, iColor);

			if (!isGridPart)
				_fullExtend.ExpandToInclude(lineStr[0]);

			PointF tf0 = ProjectToScr(lineStr[0]);

			for (int i = 1; i < lineStr.Count; i++)
			{
				if (!isGridPart)
					_fullExtend.ExpandToInclude(lineStr[i]);

				PointF tf1 = ProjectToScr(lineStr[i]);

				// TO DO: Apply Cohen–Sutherland clipping here
				_gr.DrawLine(pen, tf0, tf1);

				if (draw.drawVerts)
					DrawPoint(lineStr[i], 8, iColor);

				tf0 = tf1;
			}

#if _DEBUG
			if (!string.IsNullOrEmpty(draw.text))
			{
				PointF point = ProjectToScr(draw.TextPosition);

				Font font = new Font("Arial", (float)(7 + 2));
				point.X += 0.5f * draw.size;
				point.Y -= 0.5f * draw.size;
				_gr.TranslateTransform(point.X, point.Y);
				_gr.RotateTransform(draw.TextDirection);
				_gr.TranslateTransform(-point.X, -point.Y);

				//_gr.FillEllipse(Brushes.Black, point.X - 3, point.Y - 3, 6, 6);
				_gr.DrawString(draw.text, font, Brushes.Black, point);
				_gr.ResetTransform();
			}
#endif

			_drawingVertex = false;
		}

		public int DrawMultiLineString(MultiLineString polyline, int width = 1, int color = -1, bool markVerts = false)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = polyline,
				type = GeometryType.MultiLineString,
				drawVerts = markVerts,
				text = null,
				style = new Style { Color = color, Size = width }
			};

			DrawMultiLineString(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawMultiLineString(MultiLineString polyline, Style style, bool markVerts = false)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = polyline,
				type = GeometryType.MultiLineString,
				drawVerts = markVerts,
				text = null,
				style = style
			};

			DrawMultiLineString(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawMultiLineString(DrawingParams draw)
		{
			DrawingParams draw1 = new DrawingParams
			{
				style = draw.style,
				drawVerts = draw.drawVerts,
				text = null
			};

			foreach (LineString str in (MultiLineString)draw.geometry)
			{
				draw1.geometry = str;
				DrawLineString(draw1);
			}
		}

		public int DrawPolygon(Polygon pgon, int color = -1, bool markVerts = false)
		{
			if (pgon.ExteriorRing.Count == 0)
				return -1;
			DrawingParams draw = new DrawingParams
			{
				geometry = pgon,
				type = GeometryType.Polygon,
				drawVerts = markVerts,
				text = null,
				style = new Style { Color = color, Size = 0 }
			};
			DrawPolygon(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawPolygon(Polygon pgon, Style style, bool markVerts = false)
		{
			DrawingParams draw = new DrawingParams
			{
				geometry = pgon,
				type = GeometryType.Polygon,
				drawVerts = markVerts,
				text = null,
				style = style
			};
			DrawPolygon(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawPolygon(DrawingParams draw)
		{
			if (_gr == null)
				_gr = Graphics.FromImage(_bmp);

			//Brush brush = new HatchBrush(HatchStyle.Percent80, Color.White);
			//Pen pen = new Pen(Color.Peru, 2f);

			Brush brush = new HatchBrush((HatchStyle)draw.style.Kind, Color.FromArgb(draw.style.Color | (255 << 24)), Color.White);
			Pen pen = new Pen(Color.FromArgb(draw.style.Color | (255 << 24)), 2f);
			pen.Width = draw.style.Size;

			GraphicsPath path = new GraphicsPath();
			Polygon pgon = (Polygon)draw.geometry;

			for (int i = 0; i <= pgon.InteriorRingList.Count; i++)
			{
				PointF[] tfArray;
				if (i == 0)
				{
					tfArray = new PointF[pgon.ExteriorRing.Count];
					for (int j = 0; j < pgon.ExteriorRing.Count; j++)
					{
						_fullExtend.ExpandToInclude(pgon.ExteriorRing[j]);
						tfArray[j] = ProjectToScr(pgon.ExteriorRing[j]);
					}
				}
				else
				{
					tfArray = new PointF[pgon.InteriorRingList[i - 1].Count];
					for (int k = 0; k < pgon.InteriorRingList[i - 1].Count; k++)
					{
						_fullExtend.ExpandToInclude(pgon.InteriorRingList[i - 1][k]);
						tfArray[k] = ProjectToScr(pgon.InteriorRingList[i - 1][k]);
					}
				}
				//TO DO: Apply Sutherland–Hodgman clipping here
				path.AddPolygon(tfArray);
			}

			_gr.FillPath(brush, path);
			_gr.DrawPath(pen, path);

			if (draw.drawVerts)
			{
				_drawingVertex = true;

				int iColor = InvRGB(pen.Color);
				for (int m = 0; m <= pgon.InteriorRingList.Count; m++)
				{
					if (m == 0)
						for (int n = 0; n < pgon.ExteriorRing.Count; n++)
							DrawPoint(pgon.ExteriorRing[n], 8, iColor);
					else
						for (int num7 = 0; num7 < pgon.InteriorRingList[m - 1].Count; num7++)
							DrawPoint(pgon.InteriorRingList[m - 1][num7], 8, iColor);
				}

				_drawingVertex = false;
			}
		}

		public int DrawMultiPolygon(MultiPolygon mpgon, int color = -1, bool markVerts = false)
		{
			DrawingParams draw = new DrawingParams { geometry = mpgon, type = GeometryType.MultiPolygon, style= new Style { Color = color, Size = 0 }, drawVerts = markVerts, text = null };
			DrawMultiPolygon(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		public int DrawMultiPolygon(MultiPolygon mpgon, Style style, bool markVerts = false)
		{
			DrawingParams draw = new DrawingParams { geometry = mpgon, type = GeometryType.MultiPolygon, style = style, drawVerts = markVerts, text = null };
			DrawMultiPolygon(draw);

			_drawedItems.Add(_drawedCount, draw);
			return _drawedCount++;
		}

		private void DrawMultiPolygon(DrawingParams draw)
		{
			DrawingParams draw1 = new DrawingParams { style = draw.style, drawVerts = draw.drawVerts, text = null };

			foreach (Polygon pgon in (MultiPolygon)draw.geometry)
			{
				draw1.geometry = pgon;
				DrawPolygon(draw1);
			}
		}

		private void DrawGrid()
		{
			const double lineDir = 0.5 * Math.PI;

			if (!_mapControl.DrawGrid || _mapControl.Step == 0)
				return;

			double step, dX, dY;

			if (_mapControl.Step > 0)
				step = _mapControl.Step;
			else
			{
				dX = _fullExtend.ptMax.X - _fullExtend.ptMin.X;
				dY = _fullExtend.ptMax.Y - _fullExtend.ptMin.Y;

				double dist = Math.Min(dX, dY);
				step = Math.Pow(10, Math.Floor(Math.Log10(dist)));
			}

			Geometry.Point ptCenter = new Geometry.Point(0.0, 0.0);
			Geometry.Point ptLocalMin = PrjToLocal(ptCenter, lineDir, _fullExtend.ptMin);
			Geometry.Point ptLocalMax = PrjToLocal(ptCenter, lineDir, _fullExtend.ptMax);

			if (ptLocalMin.X > ptLocalMax.X)
			{
				double fTmp = ptLocalMin.X;
				ptLocalMin.X = ptLocalMax.X;
				ptLocalMax.X = fTmp;
			}

			if (ptLocalMin.Y > ptLocalMax.Y)
			{
				double fTmp = ptLocalMin.Y;
				ptLocalMin.Y = ptLocalMax.Y;
				ptLocalMax.Y = fTmp;
			}

			double xLo = Math.Floor(ptLocalMin.X / step) * step;
			double xHi = Math.Ceiling(ptLocalMax.X / step) * step;

			double yLo = Math.Floor(ptLocalMin.Y / step) * step;
			double yHi = Math.Ceiling(ptLocalMax.Y / step) * step;

			LineString lineStr;
			int iColor = RGB(0xc4, 0xc4, 0xc4);

			dX = xHi - xLo;
			int xCnt = ((int)Math.Round(dX / step)) + 1;
			for (int i = 0; i < xCnt; i++)
			{
				lineStr = new LineString();

				lineStr.Add(LocalToPrj(ptCenter, lineDir, xLo + step * i, yLo));
				lineStr.Add(LocalToPrj(ptCenter, lineDir, xLo + step * i, yHi));

				DrawingParams draw = new DrawingParams { geometry = lineStr, textPosition = null, textDirection = 0, type = GeometryType.LineString, style = new Style { Color = iColor, Size = 1 }, drawVerts = false, text = lineStr.Name };
				DrawLineString(draw, true);
			}

			dY = yHi - yLo;
			int yCnt = ((int)Math.Round(dY / step)) + 1;
			for (int j = 0; j < yCnt; j++)
			{
				lineStr = new LineString();
				lineStr.Add(LocalToPrj(ptCenter, lineDir, xLo, yLo + j * step));
				lineStr.Add(LocalToPrj(ptCenter, lineDir, xHi, yLo + j * step));

				DrawingParams draw = new DrawingParams { geometry = lineStr, textPosition = null, textDirection = 0, type = GeometryType.LineString, style = new Style { Color = iColor, Size = 1 }, drawVerts = false, text = lineStr.Name };
				DrawLineString(draw, true);
			}
		}

		public void SafeDeleteGraphic(int i)
		{
			if (_drawedItems.ContainsKey(i))
			{
				_drawedItems.Remove(i);
				RefreshGrafics();
			}
		}

		private void DrawFromParams(DrawingParams drw)
		{
			switch (drw.type)
			{
				case GeometryType.Point:
					DrawPoint(drw);
					break;

				case GeometryType.MultiPoint:
					DrawMultiPoint(drw);
					break;

				case GeometryType.LineString:
					DrawLineString(drw);
					break;

				case GeometryType.MultiLineString:
					DrawMultiLineString(drw);
					break;

				case GeometryType.Polygon:
					DrawPolygon(drw);
					break;

				case GeometryType.MultiPolygon:
					DrawMultiPolygon(drw);
					break;
			}
		}
		#endregion

		#region screen
		public void RecreateBitmap()
		{
			if (_gr != null)
				_gr.Dispose();

			if (_bmp != null)
				_bmp.Dispose();

			if (_screen.ClientRectangle.Width == 0 || _screen.ClientRectangle.Height == 0)
				return;

			_bmp = new Bitmap(_screen.ClientRectangle.Width, _screen.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			ZoomToExtend();
			RefreshGrafics();
		}

		public void ClearView()
		{
			if (_gr != null)
				_gr.Dispose();

			_fullExtend.SetEmpty();
			_drawedItems.Clear();
			_gr = Graphics.FromImage(_bmp);
			_gr.FillRectangle(Brushes.White, 0, 0, _bmp.Width, _bmp.Height);

			RefreshScreen();
		}

		public void RefreshGrafics()
		{
			if (_gr != null)
				_gr.Dispose();

			_gr = Graphics.FromImage(_bmp);
			_gr.FillRectangle(Brushes.White, 0, 0, _bmp.Width, _bmp.Height);
			_fullExtend.SetEmpty();

			foreach (DrawingParams drw in _drawedItems.Values)
				DrawFromParams(drw);

			if (_mapControl.DrawGrid)
				DrawGrid();

			RefreshScreen();
		}

		public void RefreshScreen()
		{
			if (_gr != null)
				_screen.Image = _bmp;
			_screen.Refresh();
		}

		public Bitmap GetBitmap()
		{
			Bitmap result = new Bitmap(_bmp);
			return result;
		}

		public void SetBitmap(Bitmap bitmp)
		{
			if (_gr != null)
			{
				_gr.Dispose();
				_gr = null;
			}
			_bmp = bitmp;
		}

		#endregion

		#region projection
		public Geometry.Point ProjectToMap(Geometry.Point pt)
		{
			double x = _currentExtend.ptMin.X + (pt.X - _xoffset) / (xScale * Scale);
			double y = _currentExtend.ptMax.Y - (pt.Y - _yoffset) / (yScale * Scale);

			return new Geometry.Point(x, y);
		}

		public PointF ProjectToScr(Geometry.Point pt)
		{
			float x = (float)(_xoffset + ((pt.X - _currentExtend.ptMin.X) * xScale * Scale));
			float y = (float)(_yoffset + ((_currentExtend.ptMax.Y - pt.Y) * yScale * Scale));

			return new PointF(x, y);
		}

		public static Geometry.Point LocalToPrj(Geometry.Point center, double dirInRadian, double x, double y = 0.0)
		{
			double sinA = Math.Sin(dirInRadian);
			double cosA = Math.Cos(dirInRadian);

			double newX = center.X + x * cosA - y * sinA;
			double newY = center.Y + x * sinA + y * cosA;
			return new Geometry.Point(newX, newY);
		}

		public static Geometry.Point PrjToLocal(Geometry.Point center, double dirInRadian, Geometry.Point ptPrj)
		{
			double sinA = Math.Sin(dirInRadian);
			double cosA = Math.Cos(dirInRadian);

			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			double newX = dY * sinA + dX * cosA;
			double newY = dY * cosA - dX * sinA;
			return new Geometry.Point(newX, newY);
		}

		public bool HavePrevExtend()
		{
			return _currentView > 0;
		}

		public bool HaveNextExtend()
		{
			return _currentView < _viewStack.Count - 1;
		}

		public void ClearViewExtendStack()
		{
			if (_viewStack.Count > 0)
				_currentExtend.Assign(_viewStack[0]);
			_viewStack.Clear();
			_currentView = -1;
		}

		public void SetViewExtend(Envelope extend = null)
		{
			if (extend != null)
			{
				if (extend.IsEmpty || _currentExtend.Equals(extend))
					return;

				_currentExtend.Assign(extend);
			}

			_viewStack.RemoveRange(_currentView + 1, _viewStack.Count - _currentView - 1);
			_currentView = _viewStack.Count;
			_viewStack.Add((Envelope)_currentExtend.Clone());

			ZoomToExtend();
			ExtendChanged?.Invoke(this, new EventArgs());
		}

		public bool SetNextExtend()
		{
			if (_currentView < _viewStack.Count - 1)
			{
				_currentView++;
				_currentExtend.Assign(_viewStack[_currentView]);

				ZoomToExtend();
			}

			ExtendChanged?.Invoke(this, new EventArgs());
			return _currentView < _viewStack.Count - 1;
		}

		public bool SetPrevExtend()
		{
			if (_currentView > 0)
			{
				_currentView--;
				_currentExtend.Assign(_viewStack[_currentView]);

				ZoomToExtend();
			}

			ExtendChanged?.Invoke(this, new EventArgs());
			return _currentView > 0;
		}


		public Envelope GetExtend()
		{
			return _currentExtend;
		}

		public Envelope GetFullExtend()
		{
			return _fullExtend;
		}

		public void SetAspectScales(double xScale_, double yScale_)
		{
			xScale = xScale_;
			yScale = yScale_;
		}

		public void ShiftExtend(double x, double y)
		{
			_currentExtend.ShiftOffset(-x / (xScale * Scale), y / (yScale * Scale));
		}

		private void ZoomToExtend()
		{
			double kx = (0.9 * _bmp.Width) / (_currentExtend.Width * xScale);
			double ky = (0.9 * _bmp.Height) / (_currentExtend.Height * yScale);

			double oldScale = Scale;
			Scale = Math.Min(kx, ky);
			if (Scale > 1000000)
				Scale = 1000000;

			if (Scale < 1e-5)
				Scale = 1e-5;

			_xoffset = 0.5 * (_bmp.Width - (_currentExtend.Width * xScale * Scale));
			_yoffset = 0.5 * (_bmp.Height - (_currentExtend.Height * yScale * Scale));

			//RefreshGrafics();

			if (oldScale != Scale)
				ScaleChanged?.Invoke(this, new EventArgs());
		}

		public event EventHandler ScaleChanged;
		public event EventHandler ExtendChanged;
		#endregion
	}
}
