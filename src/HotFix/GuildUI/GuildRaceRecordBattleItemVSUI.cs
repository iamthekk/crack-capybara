using System;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.User;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordBattleItemVSUI : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Button_Battle.onClick.AddListener(new UnityAction(this.OnClickBattle));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button_Battle = this.Button_Battle;
			if (button_Battle == null)
			{
				return;
			}
			button_Battle.onClick.RemoveListener(new UnityAction(this.OnClickBattle));
		}

		private void OnClickBattle()
		{
			if (base.gameObject == null || this.Data == null || this.Result == null)
			{
				return;
			}
			ulong recordid = this.Result.BattleRecordID;
			if (recordid == 0UL)
			{
				return;
			}
			GuildProxy.UI.GetBattleReport(recordid, delegate(bool result, UserGetBattleReportResponse resp)
			{
				if (this.gameObject == null || !this.gameObject.activeSelf)
				{
					return;
				}
				if (result)
				{
					if (resp.Record.ReportRowId != (long)recordid)
					{
						return;
					}
					GuildProxy.UI.JumpToBattle(resp, this.Data);
				}
			});
		}

		public void SetData(GuildRaceUserVSRecord data, int index)
		{
			this.Data = data;
			this.BattleIndex = index;
			this.Result = this.Data.GetResult(index);
		}

		public void RefreshUI()
		{
			GuildRaceUserVSRecord data = this.Data;
		}

		public CustomButton Button_Battle;

		public GuildRaceUserVSRecord Data;

		public int BattleIndex;

		public GuildRaceUserVSRecordResult Result;
	}
}
