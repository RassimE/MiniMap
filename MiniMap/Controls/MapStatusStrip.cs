using MiniMap.Geometry;
using System;
using System.Windows.Forms;

namespace MiniMap
{
	public partial class MapStatusStrip : StatusStrip
	{
		#region ctor
		public MapStatusStrip()
		{
			InitializeComponent();
			_mapControl = null;
			_userFields = false;
		}
		#endregion

		#region routines
		private static double GeoDistance(Point Pnt0, Point Pnt1)
		{
			const double DegToRadValue = Math.PI / 180.0;
			const double r00 = 6356824.05;
			const double r45 = 6374541.883;
			const double r90 = 6399592.574;

			const double d2 = 4.0 * (r90 - 2.0 * r45 + r00);
			const double d1 = r90 - r00;

			const double InvPI = 1.0 / Math.PI;

			double lon1r = Pnt0.X * DegToRadValue;
			double lat1r = Pnt0.Y * DegToRadValue;

			double lon2r = Pnt1.X * DegToRadValue;
			double lat2r = Pnt1.Y * DegToRadValue;

			double CosL = Math.Sin(lat1r) * Math.Sin(lat2r) + Math.Cos(lat1r) * Math.Cos(lat2r) * Math.Cos(lon2r - lon1r);
			//CosL = CosL > 1.0 ? 1.0 : CosL;
			//CosL = CosL < -1.0 ? -1.0 : CosL;

			double x = InvPI * Math.Abs(lat1r + lat2r);
			double dx = x - 0.5;
			double Rm = r45 + dx * (d1 + 0.5 * dx * d2);

			return Rm * Math.Acos(CosL);
		}
		#endregion

		#region mouse events
		private void mapControl_MouseMove(object sender, MouseEventArgs e)
		{
			MapControl control = (MapControl)sender;

			string units = control.Units == MapUnits.Degrees ? "°" : control.Units == MapUnits.Meters ? "m" : "";
			Point pt = control.MouseCurrentCoord;

			labelPrjX.Text = string.Format("PrjX = {0:0.00} {1}", pt.X, units);
			labelPrjY.Text = string.Format("PrjY = {0:0.00} {1}", pt.Y, units);

			labelScreenX.Text = string.Format("X = {0:0.00} {1}", e.X, "px");
			labelScreenY.Text = string.Format("Y = {0:0.00} {1}", e.Y, "px");
		}

		private void mapControl_MeasureEvent(object sender, MapEventArgs args)
		{
			MapControl control = (MapControl)sender;
			string units = "m";
			double dist = args.Distance;

			if (control.Units == MapUnits.Degrees)
				dist = GeoDistance(args.FromPoint, args.ToPoint);
			else if (control.Units != MapUnits.Meters)
				units = "";

			labelMeasure.Text = string.Format("Distance = {0:0.00} {1}", dist, units);
		}
		#endregion

		#region properties
		MapControl _mapControl;
		public MapControl mapControl
		{
			get { return _mapControl; }

			set
			{
				if (value == _mapControl)
					return;

				if (_mapControl != null)
				{
					_mapControl.MouseMove -= mapControl_MouseMove;
					_mapControl.MeasureEvent -= mapControl_MeasureEvent;
				}

				_mapControl = value;

				if (value != null)
				{
					_mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);
					_mapControl.MeasureEvent += new MeasureEventHandler(mapControl_MeasureEvent);
				}
			}
		}

		bool _userFields;
		public bool UserFields
		{
			get { return _userFields; }

			set
			{
				if (value == _userFields)
					return;

				_userFields = value;
				if (_userFields)
				{
					Items.Insert(4, labelUserX);
					Items.Insert(5, labelUserY);
				}
				else
				{
					Items.Remove(labelUserX);
					Items.Remove(labelUserY);
				}
			}
		}

		public string UserField1
		{
			set { labelUserX.Text = value; }
		}

		public string UserField2
		{
			set { labelUserY.Text = value; }
		}
		#endregion
	}
}
