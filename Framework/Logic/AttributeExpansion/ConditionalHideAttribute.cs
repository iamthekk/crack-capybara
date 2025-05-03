using System;
using UnityEngine;

namespace Framework.Logic.AttributeExpansion
{
	[AttributeUsage(AttributeTargets.All, Inherited = true)]
	public class ConditionalHideAttribute : PropertyAttribute
	{
		public ConditionalHideAttribute(string conditionalSourceField)
		{
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = false;
			this.Inverse = false;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
		{
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.Inverse = false;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
		{
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.Inverse = inverse;
		}

		public string ConditionalSourceField = "";

		public string ConditionalSourceField2 = "";

		public bool HideInInspector;

		public bool Inverse;
	}
}
