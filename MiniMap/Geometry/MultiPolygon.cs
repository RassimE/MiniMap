using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniMap.Geometry
{
	public class MultiPolygon : GeometryBase, IEnumerable<Polygon>, IEnumerable
	{
		public List<Polygon> _polygons = new List<Polygon>();

		public MultiPolygon()
		{
			_type = GeometryType.MultiPolygon;
		}

		public void Add(Polygon poly)
		{
			_polygons.Add(poly);
		}

		public void Clear()
		{
			_polygons.Clear();
		}

		public IEnumerator<Polygon> GetEnumerator()
		{
			return _polygons.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _polygons.GetEnumerator();
		}

		public override Envelope GetExtend()
		{
			Envelope result = new Envelope();
			foreach (var pg in _polygons)
				result.ExpandToInclude(pg);
			return result;
		}

		public override void Assign(GeometryBase source)
		{
			foreach (Polygon pgn in ((MultiPolygon)source)._polygons)
				_polygons.Add((Polygon)pgn.Clone());
		}

		public override GeometryBase Clone()
		{
			throw new NotImplementedException();
		}

		public int Count { get { return _polygons.Count; } }

		public Polygon this[int i]
		{
			get { return _polygons[i]; }
			set { _polygons[i] = value; }
		}
	}
}
