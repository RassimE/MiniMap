using System;

namespace MiniMap.Geometry
{
	public class LineString : MultiPoint
	{
		public LineString() : base()
		{
			base._type = GeometryType.LineString;
		}

		public void Assign(LineString another)
		{
			_points.Clear();
			_points.AddRange(another._points);
		}

		public double Length
		{
			get
			{
				double l = 0;

				for (int i = 1; i < _points.Count; i++)
				{
					Point pt0 = _points[i - 1];
					Point pt1 = _points[i];
					double dx = pt1.X - pt0.X;
					double dy = pt1.Y - pt0.Y;
					l += Math.Sqrt(dx * dx + dy * dy);
				}

				return l;
			}
		}

		public Point PointOnLine(double atBegin)
		{
			if (atBegin <= 0)
				return _points[0];
			if (atBegin >= Length)
				return _points[_points.Count - 1];

			double l = 0;

			for (int i = 1; i < _points.Count; i++)
			{
				Point pt0 = _points[i - 1];
				Point pt1 = _points[i];
				double dx = pt1.X - pt0.X;
				double dy = pt1.Y - pt0.Y;
				double l0 = Math.Sqrt(dx * dx + dy * dy);

				if (l + l0 >= atBegin)
				{
					double d = atBegin - l;
					double k = d / l0;
					return new Point(pt0.X + dx * k, pt0.Y + dy * k);
				}

				l += l0;
			}

			return null;
		}

	}
}
