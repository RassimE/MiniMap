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
			string distunits = "m", dirunits = "°", dirname = "Direction";
			double dist = args.Distance, dir = args.Direction;

			if (control.Units == MapUnits.Degrees)
				//Inverse(args.FromPoint, args.ToPoint, out dist, out dir);
				dirname = "Azimuth";
			else if (control.Units != MapUnits.Meters)
				dirunits = distunits = "";

			labelMeasure.Text = string.Format("Distance = {0:0.00} {1}, {2} = {3:0.00}{4}", dist, distunits, dirname, dir, dirunits);
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
