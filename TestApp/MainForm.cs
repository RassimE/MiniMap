using System;
using System.IO;
using System.Windows.Forms;
using DataFrame;
using MiniMap;
using MiniMap.Geometry;

namespace TestApp
{
	public partial class MainForm : Form
	{
		private readonly MapEngine engine;
		private readonly MapData vectorData;
		
		public MainForm()
		{
			InitializeComponent();

			engine = mapControl1.MapGraphics;
			vectorData = new MapData(engine);
			GeoTxtSerializer.Owner = this;

			vectorData.Units = MapUnits.Meters;
			Point ptTHR = new Point(0, 0, 9) { Name = "ptTHR" };
			vectorData.AddGeometry(ptTHR);

			const int n = 10;
			const double dDir = 2.0 * Math.PI / n;
			const double fieldPolySpan = 10000;

			Random rnd = new Random();

			for (int i = 0; i < n; i++)
			{
				double dir = i * dDir;
				double x = fieldPolySpan - 100.0 - 100.0 * rnd.NextDouble();
				double y = fieldPolySpan - 100.0 - 100.0 * rnd.NextDouble();

				Point ptPrj = Utils.LocalToPrj(ptTHR, dir, x, y);
				ptPrj.Name = "DP_" + (i < 10 ? "0" : "") + i.ToString();

				vectorData.AddGeometry(ptPrj);
			}

			vectorData.CalckExtent();
			vectorData.DrawData();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openMapData.ShowDialog() != DialogResult.OK)
				return;

			try
			{
				int FilterIndex = 1;
				string ext = Path.GetExtension(openMapData.FileName).ToLower();
				switch (ext)
				{
					case ".txt":
						FilterIndex = 1;
						break;
					case ".geojson":
					case ".json":
						FilterIndex = 2;
						break;
					default:
						break;
				}

				if (FilterIndex == 1)
					vectorData.LoadFromFile(openMapData.FileName);
				else if (FilterIndex == 2)
					vectorData.LoadGeoJson(openMapData.FileName);

				mapControl1.Units = vectorData.Units;
				vectorData.DrawData();

				saveMapData.FileName = Path.ChangeExtension(openMapData.FileName, null);
				Text = "Test Application [" + Path.GetFileName(openMapData.FileName) + "]";
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveMapData.ShowDialog() != DialogResult.OK)
				return;

			try
			{
				int FilterIndex = 1;
				string ext = Path.GetExtension(saveMapData.FileName).ToLower();
				switch (ext)
				{
					case ".txt":
						FilterIndex = 1;
						break;
					case ".geojson":
					case ".json":
						FilterIndex = 2;
						break;
					default:
						break;
				}

				if (FilterIndex == 1)
					vectorData.SaveToTxtFile(saveMapData.FileName);
				else if (FilterIndex == 2)
					vectorData.SaveToGeoJsonFile(saveMapData.FileName);

				openMapData.FileName = Path.ChangeExtension(saveMapData.FileName, null);
				Text = "Test Application [" + Path.GetFileName(saveMapData.FileName) + "]";
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void saveAsBitmapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveAsBitmap.ShowDialog() != DialogResult.OK)
				return;

			System.Drawing.Bitmap bmp = engine.GetBitmap();

			try
			{
				string ext = Path.GetExtension(saveMapData.FileName).ToLower();
				switch (ext)
				{
					case ".gif":
						bmp.Save(saveAsBitmap.FileName, System.Drawing.Imaging.ImageFormat.Gif);
						break;
					case ".bmp":
						bmp.Save(saveAsBitmap.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
						break;
					case ".png":
						bmp.Save(saveAsBitmap.FileName, System.Drawing.Imaging.ImageFormat.Png);
						break;
					case ".jpg":
					case ".jpeg":
						bmp.Save(saveAsBitmap.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
						break;
					default:
						bmp.Save(saveAsBitmap.FileName);
						break;
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void drawGridToolStripMenuItem_Click(object sender, EventArgs e)
		{
			drawGridToolStripMenuItem.Checked = !drawGridToolStripMenuItem.Checked;
			mapControl1.DrawGrid = drawGridToolStripMenuItem.Checked;
		}

		private void zoomToExtendToolStripMenuItem_Click(object sender, EventArgs e)
		{
			vectorData.ZoomToExtend();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this, "MiniMap test application", "Test application", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mapControl1_ScaleChanged(object sender, EventArgs e)
		{
			if (mapControl1.Units == MapUnits.Degrees)
			{
				scaleToolStripStatusLabel.Text = "-:-";
				return;
			}

			double scale = engine.Scale * 0.00026;
			string scaleText;

			if (scale < 1)
			{
				int a = (int)Math.Round(1.0 / scale);
				if (a == 0)
					a = 1;
				scaleText = "1 : " + a;
			}
			else
			{
				int a = (int)Math.Round(scale);
				if (a == 0)
					a = 1;
				scaleText = a + " : 1";
			}

			scaleToolStripStatusLabel.Text = scaleText;
		}
	}
}
