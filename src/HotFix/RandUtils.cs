using System;
using System.Collections.Generic;
using System.Linq;
using Server;

namespace HotFix
{
	public static class RandUtils
	{
		public static int GetWeightedRandomSelection(List<int> weights, XRandom xRandom)
		{
			int num = 0;
			int num2 = weights.Sum();
			int num3 = xRandom.Range(0, num2);
			for (int i = 0; i < weights.Count; i++)
			{
				num3 -= weights[i];
				num = i;
				if (num3 <= 0)
				{
					break;
				}
			}
			return num;
		}
	}
}
