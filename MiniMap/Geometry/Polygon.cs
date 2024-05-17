using System.Collections.Generic;

namespace MiniMap.Geometry
{
	public class Polygon : GeometryBase
	{
		public Ring ExteriorRing = null;
		public List<Ring> InteriorRingList = new List<Ring>();

		public Polygon()
		{
			_type = GeometryType.Polygon;
		}

		public IEnumerator<Ring> GetEnumerator()
		{
			return InteriorRingList.GetEnumerator();
		}

		public override Envelope GetExtend()
		{
			Envelope result = new Envelope();
			result.ExpandToInclude(ExteriorRing);
			//foreach (var ir in InteriorRingList)
			//	result.ExpandToInclude(ir);
			return result;
		}

		public override void Assign(GeometryBase source)
		{
			ExteriorRing = new Ring();
			ExteriorRing.Assign(((Polygon)source).ExteriorRing);

			InteriorRingList.Clear();
			foreach (Ring rg in ((Polygon)source).InteriorRingList)
				InteriorRingList.Add((Ring)rg.Clone());
		}

		public override GeometryBase Clone()
		{
			Polygon result = new Polygon();
			result.Assign(this);
			return result;
		}

		internal Point GetCenter()
		{
			double xSum = 0.0, ySum = 0.0;
			int n = 0;

			for (int i = 0; i <= InteriorRingList.Count; i++)
			{
				Ring ring = i == 0 ? ExteriorRing : InteriorRingList[i - 1];
				//Point pt = ring.CenterOfMass;

				foreach (Point pt in ring)
				{
					xSum += pt.X;
					ySum += pt.Y;
				}

				n += ring.Count;
			}

			return new Point(xSum / n, ySum / n);
		}
	}
}
