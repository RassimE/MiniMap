using System.Collections.Generic;

namespace MiniMap.Geometry
{
	public class MultiPoint : GeometryBase
	{
		protected readonly List<Point> _points = new List<Point>();

		public MultiPoint()
		{
			_type = GeometryType.MultiPoint;
		}

		public void Add(Point pt)
		{
			_points.Add(pt);
		}

		public void Clear()
		{
			_points.Clear();
		}

		public IEnumerator<Point> GetEnumerator()
		{
			return _points.GetEnumerator();
		}

		public override Envelope GetExtend()
		{
			Envelope result = new Envelope();
			foreach (var pt in _points)
				result.ExpandToInclude(pt);
			return result;
		}

		public void Assign(MultiPoint another)
		{
			_points.Clear();
			_points.AddRange(another._points);
		}

		public override void Assign(GeometryBase source)
		{
			Assign((MultiPoint)source);
		}

		public override GeometryBase Clone()
		{
			MultiPoint result = new MultiPoint();
			result.Assign(this);
			return result;
		}

		public int Count { get { return _points.Count; } }

		public Point CenterOfMass
		{
			get
			{
				if (_points.Count == 0)
					return new Point();

				double sumX = 0, sumY = 0;

				foreach (var pt in _points)
				{
					sumX += pt.X;
					sumY += pt.Y;
				}

				return new Point(sumX / _points.Count, sumY / _points.Count);
			}
		}

		public void AddMultiPoint(MultiPoint ls)
		{
			_points.AddRange(ls._points);
		}

		public void AddReverse(MultiPoint ls)
		{
			//ls._points.Reverse();
			//_points.AddRange(ls._points);
			//ls._points.Reverse();
			for (int i = ls.Count - 1; i >= 0; i--)
				_points.Add(ls[i]);
		}

		public Point this[int i]
		{
			get { return _points[i]; }
			set { _points[i] = value; }
		}
	}
}
