namespace MiniMap.Geometry
{
	public class Ring : MultiPoint
	{
		public Ring() : base()
		{
			_type = GeometryType.Ring;
		}

		public void Assign(Ring another)
		{
			_points.Clear();
			_points.AddRange(another._points);
		}

		//public double Area { }
	}
}
