namespace MiniMap.Geometry
{
	public class Envelope : GeometryBase
	{
		public Point ptMax;
		public Point ptMin;

		public Envelope()
		{
			ptMax = new Point();
			ptMin = new Point();
			_type = GeometryType.Extend;
			SetEmpty();
		}

		public void SetEmpty()
		{
			ptMin.X = ptMin.Y = double.MaxValue;
			ptMax.X = ptMax.Y = double.MinValue;
		}

		public bool IsEmpty => ptMax.X < ptMin.X;

		public void ExpandToInclude(double x, double y)
		{
			if (IsEmpty)
			{
				ptMin.X = x;
				ptMax.X = x;
				ptMin.Y = y;
				ptMax.Y = y;
				return;
			}

			if (x < ptMin.X)
				ptMin.X = x;

			if (x > ptMax.X)
				ptMax.X = x;

			if (y < ptMin.Y)
				ptMin.Y = y;

			if (y > ptMax.Y)
				ptMax.Y = y;
		}

		public void ExpandToInclude(Point pt)
		{
			if (IsEmpty)
			{
				ptMin.X = pt.X;
				ptMax.X = pt.X;
				ptMin.Y = pt.Y;
				ptMax.Y = pt.Y;
				return;
			}

			if (pt.X < ptMin.X)
				ptMin.X = pt.X;

			if (pt.X > ptMax.X)
				ptMax.X = pt.X;

			if (pt.Y < ptMin.Y)
				ptMin.Y = pt.Y;

			if (pt.Y > ptMax.Y)
				ptMax.Y = pt.Y;
		}

		public void ExpandToInclude(Envelope src)
		{
			if (IsEmpty)
			{
				ptMin = (Point)src.ptMin.Clone();
				ptMax = (Point)src.ptMax.Clone();

				return;
			}

			if (src.ptMin.X < ptMin.X)
				ptMin.X = src.ptMin.X;

			if (src.ptMax.X > ptMax.X)
				ptMax.X = src.ptMax.X;

			if (src.ptMin.Y < ptMin.Y)
				ptMin.Y = src.ptMin.Y;

			if (src.ptMax.Y > ptMax.Y)
				ptMax.Y = src.ptMax.Y;
		}

		public void ExpandToInclude(GeometryBase src)
		{
			ExpandToInclude(src.GetExtend());
		}

		public override Envelope GetExtend()
		{
			return this;
		}

		public void Assign(Envelope another)
		{
			ptMin.Assign(another.ptMin);
			ptMax.Assign(another.ptMax);
		}

		public override void Assign(GeometryBase source)
		{
			Assign((Envelope)source);
		}

		public override GeometryBase Clone()
		{
			Envelope result = new Envelope();
			result.Assign(this);
			return result;
		}

		//public void ScaleX(double xScale)
		//{
		//	ptMin.X *= xScale;
		//	ptMax.X *= xScale;
		//}

		//public void ScaleY(double yScale)
		//{
		//	ptMin.Y *= yScale;
		//	ptMax.Y *= yScale;
		//}

		public void ShiftOffset(double x, double y)
		{
			ptMin.X += x; ptMax.X += x;
			ptMin.Y += y; ptMax.Y += y;
		}

		public void Scale(double xScale, double yScale)
		{
			ptMin.X *= xScale; ptMax.X *= xScale;
			ptMin.Y *= yScale; ptMax.Y *= yScale;
		}

		public double Height { get { return ptMax.Y - ptMin.Y; } }
		public double Width { get { return ptMax.X - ptMin.X; } }

		public override string ToString()
		{
			return string.Format("lX:{0} tY:{1} rX:{2} bY:{3}", ptMin.X, ptMin.Y, ptMax.X, ptMax.Y);
		}
	}
}
