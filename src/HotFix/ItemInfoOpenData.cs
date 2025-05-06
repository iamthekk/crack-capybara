using System;
using Server;

namespace HotFix
{
	public class ItemInfoOpenData
	{
		public override string ToString()
		{
			return string.Format("EquipInfoData propData:{0} openType:{1}", this.m_propData.ToString(), this.m_openDataType);
		}

		public PropData m_propData;

		public SMemberData m_memberData;

		public ItemInfoOpenDataType m_openDataType;

		public OnItemInfoMathVolume m_onItemInfoMathVolume;
	}
}
