using System.Collections;
using System.Collections.Generic;

namespace MiniMap.Geometry
{
	public class MultiLineString : GeometryBase, IEnumerable<LineString>, IEnumerable
	{
		public List<LineString> _LineStrings = new List<LineString>();

		public MultiLineString()
		{
			_type = GeometryType.MultiLineString;
		}

		public void Add(LineString lineString)
		{
			_LineStrings.Add(lineString);
		}

		public void Clear()
		{
			_LineStrings.Clear();
		}

		public IEnumerator<LineString> GetEnumerator()
		{
			return _LineStrings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _LineStrings.GetEnumerator();
		}

		public override Envelope GetExtend()
		{
			Envelope result = new Envelope();
			foreach (var ls in _LineStrings)
				result.ExpandToInclude(ls);
			return result;
		}

		public override void Assign(GeometryBase source)
		{
			_LineStrings.Clear();
			if (!(source is MultiLineString src))
				return;

			foreach (LineString lns in src)
				this.Add((LineString)lns.Clone());
		}

		public override GeometryBase Clone()
		{
			MultiLineString res = new MultiLineString();
			res.Assign(this);
			return res;
		}

		public int Count { get { return _LineStrings.Count; } }

		public double Length
		{
			get
			{
				double l = 0;
				foreach (var ls in _LineStrings)
					l += ls.Length;
				return l;
			}
		}

		public LineString this[int i]
		{
			get { return _LineStrings[i]; }
			set { _LineStrings[i] = value; }
		}
	}
}
