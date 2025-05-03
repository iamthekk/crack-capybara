using System;

namespace Server
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class MemberAttributeInfo : Attribute
	{
		public string AttributeKey { get; }

		public string Description { get; set; }

		public int Priority { get; }

		public MemberAttributeInfo(string attributeKey, string description = "", int priority = 0)
		{
			this.AttributeKey = attributeKey;
			this.Description = description;
			this.Priority = priority;
		}
	}
}
