namespace MiniMap.Geometry
{
	public class Point : GeometryBase
	{
		public double X;
		public double Y;
		public double Z;
		public double M;

		public Point()
		{
			X = Y = Z = M = double.NaN;
			_type = GeometryType.Point;
		}

		public Point(double x, double y)
		{
			X = x;
			Y = y;
			Z = M = double.NaN;
			_type = GeometryType.Point;
		}

		public Point(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
			M = double.NaN;
			_type = GeometryType.Point;
		}

		public override Envelope GetExtend()
		{
			Envelope result = new Envelope();
			result.ExpandToInclude(this);
			return result;
		}

		public void Assign(Point another)
		{
			X = another.X;
			Y = another.Y;
			Z = another.Z;
			M = another.M;
		}

		public override void Assign(GeometryBase source)
		{
			Assign((Point)source);
		}

		public override GeometryBase Clone()
		{
			Point point = new Point();
			point.Assign(this);
			return point;
		}
	}
}
