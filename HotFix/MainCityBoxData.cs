using System;
using Framework;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class MainCityBoxData
	{
		public MainCityBoxData(ulong rowID, int quality)
		{
			this.m_rowID = rowID;
			this.m_quality = quality;
		}

		public ulong m_rowID;

		public int m_quality;
	}
}
