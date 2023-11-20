using System;
using System.Windows.Forms;

namespace MiniMap
{
	public partial class MapToolStrip : ToolStrip
	{
		#region ctor
		public MapToolStrip()
		{
			InitializeComponent();
			_mapControl = null;

			btnZoomToPrev.Enabled = false;
			btnZoomToNext.Enabled = false;
		}
		#endregion

		#region view extend manipulations
		private void btnZoomToAll_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			_mapGraphics.SetViewExtend(_mapControl.FullExtend);
			_mapGraphics.RefreshGrafics();
		}

		private void btnZoomToPrev_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			_mapGraphics.SetPrevExtend();
			btnZoomToPrev.Enabled = _mapGraphics.HavePrevExtend();
			btnZoomToNext.Enabled = _mapGraphics.HaveNextExtend();

			_mapGraphics.RefreshGrafics();
		}

		private void btnZoomToNext_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			_mapGraphics.SetNextExtend();
			btnZoomToPrev.Enabled = _mapGraphics.HavePrevExtend();
			btnZoomToNext.Enabled = _mapGraphics.HaveNextExtend();

			_mapGraphics.RefreshGrafics();
		}
		#endregion

		#region map functions
		private void panToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			modeToolStripSplitButton.Image = panToolStripMenuItem.Image;
			modeToolStripSplitButton.Text = "Mode - Pan";
			_mapControl.Mode = ViewerMode.Pan;
			_mapControl.Cursor = new Cursor(Properties.Resources.HandFlat.Handle);
		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			modeToolStripSplitButton.Image = zoomInToolStripMenuItem.Image;
			modeToolStripSplitButton.Text = "Mode - Zoom In";

			_mapControl.Mode = ViewerMode.ZoomIn;
			_mapControl.Cursor = new Cursor(Properties.Resources.ZoomIn.Handle);
		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			modeToolStripSplitButton.Image = zoomOutToolStripMenuItem.Image;
			modeToolStripSplitButton.Text = "Mode - Zoom Out";

			_mapControl.Mode = ViewerMode.ZoomOut;
			_mapControl.Cursor = new Cursor(Properties.Resources.ZoomOut.Handle);
		}

		private void measureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			modeToolStripSplitButton.Image = measureToolStripMenuItem.Image;
			modeToolStripSplitButton.Text = "Mode - Measure";

			_mapControl.Mode = ViewerMode.Measure;
			_mapControl.Cursor = Cursors.Cross;
		}

		private void noneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			modeToolStripSplitButton.Image = noneToolStripMenuItem.Image;
			modeToolStripSplitButton.Text = "Mode - None";

			_mapControl.Mode = ViewerMode.None;
			_mapControl.Cursor = Cursors.NoMove2D;
		}
		#endregion

		#region properties and events
		MapEngine _mapGraphics;
		MapControl _mapControl;

		public MapControl mapControl
		{
			get { return _mapControl; }

			set
			{
				if (_mapControl != null)
				{
					_mapControl.ExtendChanged -= mapExtendChanged;
					_mapControl.ModeChanged -= mapModeChanged;
				}

				_mapControl = value;
				_mapGraphics = null;

				if (value != null)
				{
					_mapControl.ExtendChanged += mapExtendChanged;
					_mapControl.ModeChanged += mapModeChanged;

					_mapGraphics = _mapControl.MapGraphics;

					switch (_mapControl.Mode)
					{
						case ViewerMode.None:
							_mapControl.Cursor = Cursors.NoMove2D;
							break;

						case ViewerMode.Measure:
							_mapControl.Cursor = Cursors.Cross;
							break;

						case ViewerMode.Pan:
							_mapControl.Cursor = new Cursor(Properties.Resources.HandFlat.Handle);
							break;

						case ViewerMode.ZoomIn:
							_mapControl.Cursor = new Cursor(Properties.Resources.ZoomIn.Handle);
							break;

						case ViewerMode.ZoomOut:
							_mapControl.Cursor = new Cursor(Properties.Resources.ZoomOut.Handle);
							break;
					}
				}
			}
		}

		private void mapExtendChanged(object sender, EventArgs e)
		{
			btnZoomToPrev.Enabled = _mapGraphics.HavePrevExtend();
			btnZoomToNext.Enabled = _mapGraphics.HaveNextExtend();
		}

		private void mapModeChanged(object sender, EventArgs e)
		{
			if (_mapControl == null)
				return;

			switch (_mapControl.Mode)
			{
				case ViewerMode.None:
					noneToolStripMenuItem_Click(null, null);
					break;

				case ViewerMode.Measure:
					measureToolStripMenuItem_Click(null, null);
					break;

				case ViewerMode.Pan:
					panToolStripMenuItem_Click(null, null);
					break;

				case ViewerMode.ZoomIn:
					zoomInToolStripMenuItem_Click(null, null);	
					break;

				case ViewerMode.ZoomOut:
					zoomOutToolStripMenuItem_Click(null, null);
					break;
			}
		}

		#endregion
	}
}
