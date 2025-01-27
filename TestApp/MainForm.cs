using Aran.Geometries;
using Aran.Geometries.NtsOperators;
using Aran.Geometries.Operators;
using DataFrame;
using MiniMap;
using MiniMap.Geometry;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.Mapper;
using Point = MiniMap.Geometry.Point;

namespace TestApp
{
    public partial class MainForm : Form
    {
        private readonly IGeometryOperators _geometryOperators;
        private readonly string _baseDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "risk", "WorldGeoJson");
        private readonly MapEngine engine;
        private readonly MapData vectorData;

        public bool DebugMode { get; set; } = true;

        public MainForm()
        {
            InitializeComponent();

            _geometryOperators = new NtsGeometryOperators(); // new

            engine = mapControl1.MapGraphics;
            vectorData = new MapData(engine);
            GeoTxtSerializer.Owner = this;

            vectorData.Units = MapUnits.Meters;
            openMapData.Filter = "GeoJSON Files|*.geojson;*.json|Text Files|*.txt";
            openMapData.Multiselect = true;

            if (DebugMode)
            {
                AddDefaultDebugValues();
            }
        }

        private void AddDefaultDebugValues()
        {
            Point ptTHR = new Point(0, 0, 9) { Name = "ptTHR", Tag = "Default" };
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
                ptPrj.Tag = "Default";

                vectorData.AddGeometry(ptPrj);
            }

            vectorData.CalckExtent();
            vectorData.DrawData();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<GeometryBase> geometries = new List<GeometryBase>();

            if (openMapData.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                foreach (var fileName in openMapData.FileNames)
                {
                    string ext = Path.GetExtension(fileName);
                    switch (ext)
                    {
                        case ".txt":
                            vectorData.LoadFromFile(fileName);
                            break;

                        case ".geojson":
                        case ".json":
                            if (Path.GetFileName(fileName).StartsWith("predefined"))
                            {
                                MakeFeatureCollection(fileName);
                            }

                            GeometryBase geometryBase = vectorData.LoadGeoJson(fileName);
                            Aran.Geometries.Geometry loadedAranGeom = GeomMapper.Map(geometryBase);

                            //if (loadedAranGeom is Aran.Geometries.MultiPolygon multiPolygon)
                            //{
                            //    multiPolygon[0] = RemoveAgnails(multiPolygon[0], close: true);
                            //}

                            //simplifiedGeom = _geometryOperators.SimplifyGeometry(loadedAranGeom, 1000);
                            vectorData.AddGeometry(GeomMapper.Map(loadedAranGeom));
                            break;

                        default:
                            break;
                    }
                }

                // to display points of polygon
                //Aran.Geometries.Geometry aranGeom = GeomMapper.Map(geometries[0]);
                //aranGeom = _geometryOperators.SimplifyGeometry(aranGeom, 1000);

                if (openMapData.FileNames.Length == 2) // to check serialized aran geometries from NTS operations.
                {
                    Aran.Geometries.Geometry geometry_1 = GeomMapper.Map(geometries[0]);
                    Aran.Geometries.Geometry geometry_2 = GeomMapper.Map(geometries[1]);

                    geometry_1 = _geometryOperators.SimplifyGeometry(geometry_1);
                    geometry_2 = _geometryOperators.SimplifyGeometry(geometry_2);

                    //Aran.Geometries.Geometry unionedGeometry = _geometryOperators.UnionGeometry(geometry_1, geometry_2);
                    //GeometryBase minimapGeometry = GeomMapper.Map(unionedGeometry);

                    vectorData.AddGeometry(GeomMapper.Map(geometry_1));
                    vectorData.AddGeometry(GeomMapper.Map(geometry_2));
                }

                mapControl1.Units = vectorData.Units;
                vectorData.DrawData();

                string combinedFilename = string.Join(" | ", openMapData.FileNames.Select(Path.GetFileName));
                saveMapData.FileName = Path.ChangeExtension(combinedFilename, null);
                Text = "Test Application [" + Path.GetFileName(combinedFilename) + "]";
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private static Aran.Geometries.Polygon RemoveAgnails(Aran.Geometries.Polygon polygon, bool close = false)
        {
            var polygonCloned = (Aran.Geometries.Polygon)polygon.Clone();

            for (int i = 0; i < polygonCloned.InteriorRingList.Count; i++)
                polygonCloned.InteriorRingList[i] = RemoveAgnails(polygonCloned.InteriorRingList[i], close);

            polygonCloned.ExteriorRing = RemoveAgnails(polygonCloned.ExteriorRing, close);

            return polygonCloned;
        }

        private static Aran.Geometries.Ring RemoveAgnails(Aran.Geometries.Ring ring, bool close = false)
        {
            const double epsilon = 0.00001;

            var result = new Aran.Geometries.Ring();
            var pointsCount = ring.Count;
            if (pointsCount < 3)
                return result;

            var dx = ring[0].X - ring[pointsCount - 1].X;
            var dy = ring[0].Y - ring[pointsCount - 1].Y;

            if (dx * dx + dy * dy < epsilon * epsilon)
                pointsCount--;

            if (pointsCount < 3)
                return result;

            var i = pointsCount - 2;
            var j = pointsCount - 1;

            for (int k = 0; k < pointsCount; k++)
            {
                var dx1 = ring[k].X - ring[j].X;
                var dy1 = ring[k].Y - ring[j].Y;

                var dxx = dx1 * dx1;
                var dyy = dy1 * dy1;
                var dl1 = dxx + dyy;

                if (dl1 < epsilon)
                    continue;

                var dx0 = ring[j].X - ring[i].X;
                var dy0 = ring[j].Y - ring[i].Y;
                var dl0 = dx0 * dx0 + dy0 * dy0;

                if (dl0 >= epsilon)
                {
                    if (dy0 * dy0 > 0.0)
                    {
                        if (dyy == 0.0)
                            result.Add(ring[j]);
                        else if (Math.Abs(Math.Abs(dx0 / dy0) - Math.Abs(dx1 / dy1)) >= epsilon)
                            result.Add(ring[j]);
                    }
                    else if (dx0 * dx0 > 0.0)
                    {
                        if (dxx == 0.0)
                            result.Add(ring[j]);
                        else if (Math.Abs(Math.Abs(dy0 / dx0) - Math.Abs(dy1 / dx1)) >= epsilon)
                            result.Add(ring[j]);
                    }
                    else
                        result.Add(ring[j]);
                }

                i = j;
                j = k;
            }

            if (close && result.Count > 2)
                result.Add(result[0]);

            return result;
        }

        private void MakeFeatureCollection(string fileName)
        {
            string featuresContent = File.ReadAllText(Path.Combine(_baseDirectoryPath, "baseFeatureCollection.geo.json"));
            string geometryContent = string.Empty;

            using (FileStream geometryStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            using (TextReader reader = new StreamReader(geometryStream))
            {
                geometryContent = reader.ReadToEnd();
                if (geometryContent.Contains("FeatureCollection"))
                {
                    return;
                }
            }

            string updatedFeatureCollection = featuresContent.Replace("\"replaceHere\": \"\"", geometryContent.Trim(new char[] { '{', '}', '\r', '\n' }));
            File.WriteAllText(fileName, updatedFeatureCollection);
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
                string ext = Path.GetExtension(saveAsBitmap.FileName).ToLower();
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
            if (engine == null)
                return;

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
