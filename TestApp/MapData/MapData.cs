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

		public Envelope Extend { get { return _extend; } }

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
			set { _geometries[i] = value; }
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

		public int Draw(GeometryBase geometry, Style style)
		{
			geometry.Tag = style;
			int result = _geometries.Count;
			_geometries.Add(geometry);
			_extend.ExpandToInclude(geometry);
			return result;
		}

		public int AddGeometry(GeometryBase geometry)
		{
			int result = _geometries.Count;
			_geometries.Add(geometry);
			_extend.ExpandToInclude(geometry);
			return result;
		}

		public bool RemoveGeometry(GeometryBase geometry)
		{
			return _geometries.Remove(geometry);
		}
		
		public void RemoveGeometry(int geometry)
		{
			if (geometry < 0 || geometry >= _geometries.Count)
				return;
			_geometries.RemoveAt(geometry);
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

			Style style;

			foreach (var item in _geometries)
			{
				if (item.Tag != null)
					style = (Style)item.Tag;
				else
				{
					style.Color = -1;
					style.Size = 1;
					style.Kind = 0;
				}

				switch (item.Type)
				{
					case GeometryType.Polygon:
						if (item.Tag == null)
						{
							style.Color = MapEngine.RGB(192, 128, 0);
							style.Size = 1;
							style.Kind = 7;  //HatchStyle.Percent10;
						}

						_engine.DrawPolygon((Polygon)item, style, _drawTiks);
						break;

					case GeometryType.MultiPolygon:
						if (item.Tag == null)
						{
							style.Color = MapEngine.RGB(192, 128, 0);
							style.Size = 1;
							style.Kind = 7;  //HatchStyle.Percent10;
						}

						_engine.DrawMultiPolygon((MultiPolygon)item, style, _drawTiks);
						break;

					case GeometryType.LineString:
						if (item.Tag == null)
							style.Color = MapEngine.RGB(192, 128, 255);
						_engine.DrawLineString((LineString)item, style, _drawTiks);
						break;

					case GeometryType.MultiLineString:
						if (item.Tag == null)
							style.Color = MapEngine.RGB(192, 128, 255);
						_engine.DrawMultiLineString((MultiLineString)item, style, _drawTiks);
						break;

					case GeometryType.Point:

						Point pt = (Point)item;
						if (string.IsNullOrEmpty(pt.Name))
							_engine.DrawPoint(pt, iColor: MapEngine.RGB(192, 128, 255));
						else
						{
							if (item.Tag == null)
							{
								style.Color = MapEngine.RGB(192, 192, 255);
								style.Size = 10;
							}
							_engine.DrawPointWithText(pt, pt.Name, style);
						}
						break;

					case GeometryType.MultiPoint:
						if (item.Tag == null)
						{
							style.Color = MapEngine.RGB(92, 192, 92);
							style.Size = 6;
						}
						_engine.DrawMultiPoint((MultiPoint)item, style);
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
