﻿using System;
using System.Reflection;

namespace Google.Protobuf.Compatibility
{
	internal static class PropertyInfoExtensions
	{
		internal static MethodInfo GetGetMethod(this PropertyInfo target)
		{
			MethodInfo getMethod = target.GetGetMethod();
			if (!(getMethod != null) || !getMethod.IsPublic)
			{
				return null;
			}
			return getMethod;
		}

		internal static MethodInfo GetSetMethod(this PropertyInfo target)
		{
			MethodInfo setMethod = target.GetSetMethod();
			if (!(setMethod != null) || !setMethod.IsPublic)
			{
				return null;
			}
			return setMethod;
		}
	}
}
