using System;

namespace Server
{
	public static class TimeOverlayTypeEx
	{
		public static int Math(this TimeOverlayType timeOverlayType, int source, int current)
		{
			int num;
			switch (timeOverlayType)
			{
			case TimeOverlayType.Equals:
				num = current;
				break;
			case TimeOverlayType.Add:
				num = source + current;
				break;
			case TimeOverlayType.Max:
				num = MathTools.Max(source, current);
				break;
			case TimeOverlayType.Min:
				num = MathTools.Min(source, current);
				break;
			default:
				throw new ArgumentOutOfRangeException("timeOverlayType", timeOverlayType, null);
			}
			return num;
		}
	}
}
