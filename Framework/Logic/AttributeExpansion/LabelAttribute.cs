using System;
using UnityEngine;

namespace Framework.Logic.AttributeExpansion
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class LabelAttribute : PropertyAttribute
	{
	}
}
