using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshMainCityBoxData : BaseEventArgs
	{
		public void SetData(RepeatedField<CityChestDto> chestDatas, ulong startChestHangeTime, ulong refreshChestHangTime)
		{
			this.Clear();
			this.m_chestDatas = chestDatas;
			this.m_startChestHangeTime = startChestHangeTime;
			this.m_refreshChestHangTime = refreshChestHangTime;
		}

		public void SetData(RepeatedField<ulong> removeRowIDs, ulong refreshChestHangTime, int chestIntegral)
		{
			this.Clear();
			this.m_removeRowIDs = removeRowIDs;
			this.m_refreshChestHangTime = refreshChestHangTime;
			this.m_chestIntegral = chestIntegral;
		}

		public void SetData(ulong refreshChestHangTime, int chestIntegral)
		{
			this.Clear();
			this.m_refreshChestHangTime = refreshChestHangTime;
			this.m_chestIntegral = chestIntegral;
		}

		public override void Clear()
		{
			this.m_chestDatas = null;
			this.m_removeRowIDs = null;
			this.m_startChestHangeTime = 0UL;
			this.m_refreshChestHangTime = 0UL;
			this.m_chestIntegral = -1;
		}

		public RepeatedField<CityChestDto> m_chestDatas;

		public ulong m_startChestHangeTime;

		public ulong m_refreshChestHangTime;

		public int m_chestIntegral = -1;

		public RepeatedField<ulong> m_removeRowIDs;
	}
}
