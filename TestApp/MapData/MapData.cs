using System;
using System.Collections;
using System.Collections.Generic;
using DataFrame;
using MiniMap;
using MiniMap.Geometry;

namespace DataFrame
{
    public struct Style
    {
        public int Color;
        public int Size;
        public int Kind;
    }

    public class MapData : IEnumerable<GeometryBase>, IEnumerable
    {
        private readonly Random _random = new Random();
        private readonly Style _defaultStyle = new()
        {
            Color = MapEngine.RGB(0, 0, 0),
            Size = 1,
            Kind = 0
        };

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

            foreach (var item in _geometries)
            {
                Style style = item.Tag switch
                {
                    "Default" => _defaultStyle,
                    Style tagStyle => tagStyle,
                    _ => GenerateRandomStyle()
                };

                switch (item.Type)
                {
                    case GeometryType.Polygon:
                        ApplyDefaultStyleIfTagIsNull(item, ref style);
                        _engine.DrawPolygon((Polygon)item, style.Color, _drawTiks);
                        break;

                    case GeometryType.MultiPolygon:
                        _engine.DrawMultiPolygon((MultiPolygon)item, style.Color, _drawTiks);
                        break;

                    case GeometryType.LineString:
                        _engine.DrawLineString((LineString)item, 1, style.Color, _drawTiks);
                        break;

                    case GeometryType.MultiLineString:
                        _engine.DrawMultiLineString((MultiLineString)item, 1, style.Color, _drawTiks);
                        break;

                    case GeometryType.Point:
                        var pt = (Point)item;
                        if (string.IsNullOrEmpty(pt.Name))
                            _engine.DrawPoint(pt, iColor: MapEngine.RGB(192, 128, 255));
                        else
                            _engine.DrawPointWithText(pt, pt.Name, iColor: style.Color);
                        break;

                    case GeometryType.MultiPoint:
                        _engine.DrawMultiPoint((MultiPoint)item, 6, style.Color);
                        break;
                }
            }

            _engine.RefreshGrafics();
        }

        private Style GenerateRandomStyle()
        {
            int r = _random.Next(0, 256);
            int g = _random.Next(0, 256);
            int b = _random.Next(0, 256);

            return new Style
            {
                Color = MapEngine.RGB(r, g, b),
                Size = 1,
                Kind = 0
            };
        }

        private void ApplyDefaultStyleIfTagIsNull(GeometryBase item, ref Style style)
        {
            if (item.Tag == null)
            {
                style.Size = 1;
                style.Kind = 0;
            }
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