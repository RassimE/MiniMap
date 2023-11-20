#if haveProperties
using System.Collections.Generic;
#endif

namespace MiniMap.Geometry
{
#if haveProperties
	public struct Property
	{
		public string Name;
		public object Value;

		public override string ToString()
		{
			return string.Format("{0}: {1}", Name, Value);
		}
	}
#endif

	public abstract class GeometryBase
	{
		protected GeometryType _type;

		public GeometryType Type => _type;
		public string Name { get; set; }

		public abstract Envelope GetExtend();

		public abstract void Assign(GeometryBase source);

		public abstract GeometryBase Clone();

#if haveProperties
		public List<Property> Propertyes = new List<Property>();
#endif
	}
}
