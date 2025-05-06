using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainUserVS_VSUI : GuildProxy.GuildProxy_BaseBehaviour
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
			if (this.Data == null || this.Data.IsEmptyRecord)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400449));
				return;
			}
			GuildRaceRecordBattleViewModule.OpenData openData = new GuildRaceRecordBattleViewModule.OpenData();
			openData.Record = this.Data;
			GameApp.View.OpenView(ViewName.GuildRaceRecordBattleViewModule, openData, 1, null, null);
		}

		public void SetData(GuildRaceUserVSRecord data)
		{
			this.Data = data;
		}

		public void RefreshUI()
		{
			GuildRaceUserVSRecord data = this.Data;
		}

		public CustomButton Button_Battle;

		public GuildRaceUserVSRecord Data;
	}
}
