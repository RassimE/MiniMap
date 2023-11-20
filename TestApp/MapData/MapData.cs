using System.Collections;
using System.Collections.Generic;
using MiniMap;
using MiniMap.Geometry;

namespace DataFrame
{
	public class MapData : IEnumerable<GeometryBase>, IEnumerable
	{
		Envelope _extend;
		MapEngine _engine;
		List<GeometryBase> _geometries;

		bool _drawTiks;
		MapUnits _units;

		public string Name { get; set; }

		public MapUnits Units
		{
			get { return _units; }

			set
			{
				if (_units == value)
					return;

				_units = value;

				ResetViewAspect();
				_engine.RefreshGrafics();
			}
		}

		public bool DrawTiks
		{
			get { return _drawTiks; }

			set
			{
				bool resetView = value != _drawTiks;
				_drawTiks = value;

				if (resetView)
					DrawData(false);
			}
		}

		public MapData(MapEngine engine)
		{
			_extend = new Envelope();
			_geometries = new List<GeometryBase>();

			_engine = engine;
			_drawTiks = false;
		}

		public GeometryBase this[int i]
		{
			get { return _geometries[i]; }
		}

		public IEnumerator GetEnumerator()
		{
			return _geometries.GetEnumerator();
		}

		IEnumerator<GeometryBase> IEnumerable<GeometryBase>.GetEnumerator()
		{
			return _geometries.GetEnumerator();
		}

		public int GeometryCount { get { return _geometries.Count; } }

		public void Clear()
		{
			_geometries.Clear();
			_extend.SetEmpty();
		}

		public void AddGeometry(GeometryBase geometry)
		{
			_geometries.Add(geometry);
			_extend.ExpandToInclude(geometry);
		}

		public void CalckExtent()
		{
			_extend.SetEmpty();

			foreach (var geom in _geometries)
				_extend.ExpandToInclude(geom);
		}

		public void DrawData(bool resetView = true)
		{
			if (resetView)
				ResetViewAspect();

			_engine.ClearView();

			foreach (var item in _geometries)
			{
				switch (item.Type)
				{
					case GeometryType.Polygon:
						_engine.DrawPolygon((Polygon)item, MapEngine.RGB(192, 128, 0), _drawTiks);
						break;

					case GeometryType.MultiPolygon:
						_engine.DrawMultiPolygon((MultiPolygon)item, MapEngine.RGB(192, 128, 0), _drawTiks);
						break;

					case GeometryType.LineString:
						_engine.DrawLineString((LineString)item, 1, MapEngine.RGB(192, 128, 255), _drawTiks);
						break;

					case GeometryType.MultiLineString:
						_engine.DrawMultiLineString((MultiLineString)item, 1, MapEngine.RGB(192, 128, 255), _drawTiks);
						break;

					case GeometryType.Point:
						Point pt = (Point)item;
						if (string.IsNullOrEmpty(pt.Name))
							_engine.DrawPoint(pt, iColor: MapEngine.RGB(192, 128, 255));
						else
							_engine.DrawPointWithText(pt, pt.Name, iColor: MapEngine.RGB(192, 192, 255));
						break;

					case GeometryType.MultiPoint:
						_engine.DrawMultiPoint((MultiPoint)item, 6, MapEngine.RGB(92, 192, 92));
						break;
				}
			}

			_engine.RefreshGrafics();
		}

		public void ResetViewAspect()
		{
			double kX = 1.0, kY = 1.0;

			if (Units == MapUnits.Degrees)
				Utils.CalculateRatio(_extend, out kX, out kY);

			_engine.SetAspectScales(kX, kY);
			_engine.SetViewExtend(_extend);
		}

		public void ClearViewExtendStack()
		{
			_engine.ClearViewExtendStack();
		}

		public void ZoomToExtend()
		{
			_engine.SetViewExtend(_extend);
			_engine.RefreshGrafics();
		}
	}
}
