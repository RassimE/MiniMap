using MiniMap.Geometry;
using System;

namespace TestApp.Mapper;
public class GeomMapper
{
    public static Aran.Geometries.Geometry Map(GeometryBase geometry)
    {
        Aran.Geometries.Geometry aranGeometry = geometry.Type switch
        {
            GeometryType.Point => Map2Point(geometry as Point),
            GeometryType.MultiPoint => Map2MultiPoint(geometry as MultiPoint),
            GeometryType.LineString => Map2LineString(geometry as LineString),
            GeometryType.MultiLineString => Map2MultiLineString(geometry as MultiLineString),
            GeometryType.Polygon => Map2Polygon(geometry as Polygon),
            GeometryType.MultiPolygon => Map2MultiPolygon(geometry as MultiPolygon),
            GeometryType.Ring => Map2Ring(geometry as Ring),
            GeometryType.Extend => Map2Extend(geometry as Envelope),
            _ => throw new NotSupportedException()
        };

        return aranGeometry;
    }

    private static Aran.Geometries.Point Map2Point(Point point) => new()
    {
        X = point.X,
        Y = point.Y,
        Z = point.Z,
        M = point.M,
    };

    private static Aran.Geometries.MultiPoint Map2MultiPoint(MultiPoint multiPoint)
    {
        var aranMultiPoint = new Aran.Geometries.MultiPoint();
        foreach (Point point in multiPoint)
        {
            Aran.Geometries.Point aranPoint = Map2Point(point);
            aranMultiPoint.Add(aranPoint);
        }

        return aranMultiPoint;
    }

    private static Aran.Geometries.LineString Map2LineString(LineString lineString)
    {
        var aranLineString = new Aran.Geometries.LineString();
        foreach (Point point in lineString)
        {
            Aran.Geometries.Point aranPoint = Map2Point(point);
            aranLineString.Add(aranPoint);
        }

        return aranLineString;
    }

    private static Aran.Geometries.MultiLineString Map2MultiLineString(MultiLineString multiLineString)
    {
        var aranMultiLineString = new Aran.Geometries.MultiLineString();
        foreach (LineString lineString in multiLineString)
        {
            var aranLineString = Map2LineString(lineString);
            aranMultiLineString.Add(aranLineString);
        }

        return aranMultiLineString;
    }

    private static Aran.Geometries.Polygon Map2Polygon(Polygon polygon)
    {
        var aranPolygon = new Aran.Geometries.Polygon { ExteriorRing = Map2Ring(polygon.ExteriorRing) };
        polygon.InteriorRingList.ForEach((Ring interiorRing) =>
        {
            aranPolygon.InteriorRingList.Add(Map2Ring(interiorRing));
        });

        return aranPolygon;
    }

    private static Aran.Geometries.MultiPolygon Map2MultiPolygon(MultiPolygon multiPolygon)
    {
        var aranMultiPolygon = new Aran.Geometries.MultiPolygon();
        foreach (Polygon polygon in multiPolygon)
        {
            var aranPolygon = Map2Polygon(polygon);
            aranMultiPolygon.Add(aranPolygon);
        }

        return aranMultiPolygon;
    }

    private static Aran.Geometries.Ring Map2Ring(Ring ring)
    {
        var aranRing = new Aran.Geometries.Ring();
        foreach (Point point in ring)
        {
            Aran.Geometries.Point aranPoint = Map2Point(point);
            aranRing.Add(aranPoint);
        }

        return aranRing;
    }

    private static Aran.Geometries.Box Map2Extend(Envelope envelope)
    {
        var aranBox = new Aran.Geometries.Box();
        (aranBox.XMax, aranBox.YMax) = (envelope.ptMax.X, envelope.ptMax.Y);
        (aranBox.XMin, aranBox.YMin) = (envelope.ptMin.X, envelope.ptMin.Y);

        return aranBox;
    }

    // aran geometry multipoint is ienumerable, nott icollection
    //private static T MapGeometryCollection<T>(IEnumerator<Point> points, T collection)
    //    where T : ICollection<Aran.Geometries.Point>
    //{
    //    while (points.MoveNext())
    //    {
    //        Aran.Geometries.Point aranPoint = Map2Point(points.Current);
    //        collection.Add(aranPoint);
    //    }

    //    return collection;
    //}

    // ---

    public static GeometryBase Map(Aran.Geometries.Geometry aranGeometry)
    {
        GeometryBase geometry = aranGeometry.Type switch
        {
            Aran.Geometries.GeometryType.Point => Map2Point(aranGeometry as Aran.Geometries.Point),
            Aran.Geometries.GeometryType.MultiPoint => Map2MultiPoint(aranGeometry as Aran.Geometries.MultiPoint),
            Aran.Geometries.GeometryType.LineString => Map2LineString(aranGeometry as Aran.Geometries.LineString),
            Aran.Geometries.GeometryType.MultiLineString => Map2MultiLineString(aranGeometry as Aran.Geometries.MultiLineString),
            Aran.Geometries.GeometryType.Polygon => Map2Polygon(aranGeometry as Aran.Geometries.Polygon),
            Aran.Geometries.GeometryType.MultiPolygon => Map2MultiPolygon(aranGeometry as Aran.Geometries.MultiPolygon),
            Aran.Geometries.GeometryType.Ring => Map2Ring(aranGeometry as Aran.Geometries.Ring),
            Aran.Geometries.GeometryType.SpecialGeometry => Map2SpecialGeometry(aranGeometry as Aran.Geometries.SpecialGeometry),
            _ => throw new NotSupportedException()
        };

        return geometry;
    }

    private static Point Map2Point(Aran.Geometries.Point aranPoint) => new()
    {
        X = aranPoint.X,
        Y = aranPoint.Y,
        Z = aranPoint.Z,
        M = aranPoint.M,
    };

    private static MultiPoint Map2MultiPoint(Aran.Geometries.MultiPoint aranMultiPoint)
    {
        var multiPoint = new MultiPoint();
        foreach (Aran.Geometries.Point aranPoint in aranMultiPoint)
        {
            var point = Map2Point(aranPoint);
            multiPoint.Add(point);
        }

        return multiPoint;
    }

    private static LineString Map2LineString(Aran.Geometries.LineString aranLineString)
    {
        var lineString = new LineString();
        foreach (Aran.Geometries.Point aranPoint in aranLineString)
        {
            var point = Map2Point(aranPoint);
            lineString.Add(point);
        }

        return lineString;
    }

    private static MultiLineString Map2MultiLineString(Aran.Geometries.MultiLineString aranMultiLineString)
    {
        var multiLineString = new MultiLineString();
        foreach (Aran.Geometries.LineString aranLineString in aranMultiLineString)
        {
            var lineString = Map2LineString(aranLineString);
            multiLineString.Add(lineString);
        }

        return multiLineString;
    }

    private static Polygon Map2Polygon(Aran.Geometries.Polygon aranPolygon)
    {
        var polygon = new Polygon { ExteriorRing = Map2Ring(aranPolygon.ExteriorRing) };
        foreach (Aran.Geometries.Ring interiorRing in aranPolygon.InteriorRingList)
        {
            var aranRing = Map2Ring(interiorRing);
            polygon.InteriorRingList.Add(aranRing);
        };

        return polygon;
    }

    private static MultiPolygon Map2MultiPolygon(Aran.Geometries.MultiPolygon aranMultiPolygon)
    {
        var multiPolygon = new MultiPolygon();
        foreach (Aran.Geometries.Polygon aranPolygon in aranMultiPolygon)
        {
            var polygon = Map2Polygon(aranPolygon);
            multiPolygon.Add(polygon);
        }

        return multiPolygon;
    }

    private static Ring Map2Ring(Aran.Geometries.Ring aranRing)
    {
        var ring = new Ring();
        foreach (Aran.Geometries.Point aranPoint in aranRing)
        {
            var point = Map2Point(aranPoint);
            ring.Add(point);
        }

        return ring;
    }

    private static Envelope Map2SpecialGeometry(Aran.Geometries.SpecialGeometry specialGeometry)
    {
        return specialGeometry.SpecGeomType switch
        {
            Aran.Geometries.SpecialGeometryType.Box => Map2Extend(specialGeometry as Aran.Geometries.Box),
            Aran.Geometries.SpecialGeometryType.Circle => throw new NotImplementedException(),
            _ => throw new NotSupportedException()
        };
    }

    private static Envelope Map2Extend(Aran.Geometries.Box aranBox)
    {
        var envelope = new Envelope
        {
            ptMax = new Point(aranBox.XMax, aranBox.YMax),
            ptMin = new Point(aranBox.XMin, aranBox.YMin)
        };

        return envelope;
    }
}