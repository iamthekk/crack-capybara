using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class ChainPacksPushInfo
	{
		public ChainPacksPushInfo(ChainActvDto actvDto)
		{
			this.SetData(actvDto);
		}

		public void SetData(ChainActvDto actvDto)
		{
			this.ActvDto = actvDto;
			this.ChainPushCfg = GameApp.Table.GetManager().GetChainPacks_PushChainActv(actvDto.Id);
			if (this.ChainPushCfg != null)
			{
				IList<ChainPacks_ChainPacks> chainPacks_ChainPacksElements = GameApp.Table.GetManager().GetChainPacks_ChainPacksElements();
				for (int i = 0; i < chainPacks_ChainPacksElements.Count; i++)
				{
					if (chainPacks_ChainPacksElements[i].group == this.ChainPushCfg.groupID)
					{
						this.CfgList.Add(chainPacks_ChainPacksElements[i]);
					}
				}
				int num = 0;
				for (int j = 0; j < actvDto.GetRewardIds.Count; j++)
				{
					if (actvDto.GetRewardIds[j] > num)
					{
						num = actvDto.GetRewardIds[j];
					}
				}
				for (int k = 0; k < this.CfgList.Count; k++)
				{
					if (this.CfgList[k].id == num)
					{
						this.PickIndex = k;
						return;
					}
				}
			}
		}

		public bool IsCanShow()
		{
			return DxxTools.Time.ServerTimestamp <= (long)this.ActvDto.EndTime && this.PickIndex < this.CfgList.Count - 1;
		}

		public bool IsPop;

		public ChainActvDto ActvDto;

		public int PickIndex = -1;

		public ChainPacks_PushChainActv ChainPushCfg;

		public List<ChainPacks_ChainPacks> CfgList = new List<ChainPacks_ChainPacks>();
	}
}
