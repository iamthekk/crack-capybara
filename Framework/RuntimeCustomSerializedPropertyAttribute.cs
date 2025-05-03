using System;

namespace Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class RuntimeCustomSerializedPropertyAttribute : Attribute
	{
		public RuntimeCustomSerializedPropertyAttribute(string typeFullName)
		{
			this.m_typeFullName = typeFullName;
		}

		public string m_typeFullName;
	}
}
