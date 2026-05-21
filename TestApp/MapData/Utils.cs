using System;
using MiniMap.Geometry;

namespace DataFrame
{
	static class Utils
	{
		public static Point LocalToPrj(Point center, double dirInRadian, double x, double y = 0.0)
		{
			double sinA = Math.Sin(dirInRadian);
			double cosA = Math.Cos(dirInRadian);

			double newX = center.X + x * cosA - y * sinA;
			double newY = center.Y + x * sinA + y * cosA;
			return new Point(newX, newY);
		}

		public static Point PrjToLocal(Point center, double dirInRadian, Point ptPrj)
		{
			double sinA = Math.Sin(dirInRadian);
			double cosA = Math.Cos(dirInRadian);

			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			double newX = dY * sinA + dX * cosA;
			double newY = dY * cosA - dX * sinA;
			return new Point(newX, newY);
		}

		private static double GeoDistance(Point Pt0, Point Pt1)
		{
			const double DegToRadValue = Math.PI / 180.0;
			const double r00 = 6356824.05;
			const double r45 = 6374541.883;
			const double r90 = 6399592.574;

			const double d2 = 4.0 * (r90 - 2.0 * r45 + r00);
			const double d1 = r90 - r00;

			const double InvPI = 1.0 / Math.PI;

			double lon1r = Pt0.X * DegToRadValue;
			double lat1r = Pt0.Y * DegToRadValue;

			double lon2r = Pt1.X * DegToRadValue;
			double lat2r = Pt1.Y * DegToRadValue;

			double CosL = Math.Sin(lat1r) * Math.Sin(lat2r) + Math.Cos(lat1r) * Math.Cos(lat2r) * Math.Cos(lon2r - lon1r);
			//CosL = CosL > 1.0 ? 1.0 : CosL;
			//CosL = CosL < -1.0 ? -1.0 : CosL;

			double x = InvPI * Math.Abs(lat1r + lat2r);
			double dx = x - 0.5;
			double Rm = r45 + dx * (d1 + 0.5 * dx * d2);

			return Rm * Math.Acos(CosL);
		}

		public static void CalculateRatio(Envelope extend, out double kx, out double ky)
		{
			Point ptMdl = new Point(0.5 * (extend.ptMin.X + extend.ptMax.X), 0.5 * (extend.ptMin.Y + extend.ptMax.Y));
			Point ptL = new Point(ptMdl.X - 0.01, ptMdl.Y);
			Point ptR = new Point(ptMdl.X + 0.01, ptMdl.Y);

			Point ptT = new Point(ptMdl.X, ptMdl.Y - 0.01);
			Point ptB = new Point(ptMdl.X, ptMdl.Y + 0.01);

			double dx = GeoDistance(ptL, ptR);
			double dy = GeoDistance(ptT, ptB);

			if (dx < dy)
			{
				ky = 1.0;
				kx = dx / dy;
			}
			else
			{
				kx = 1.0;
				ky = dy / dx;
			}
		}
	}
}
