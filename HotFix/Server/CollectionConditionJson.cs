using System;

namespace Server
{
	[Serializable]
	public class CollectionConditionJson
	{
		public long Cost;

		public string Attribute;

		public FP Value;

		public int Limit;
	}
}
