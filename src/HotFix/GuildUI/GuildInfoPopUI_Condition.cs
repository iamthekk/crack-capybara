using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_Condition : GuildInfoPopUI_Base
	{
		public override void RefreshUI()
		{
			base.RefreshUI();
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			this.Text_Add.text = guildData.GetJoinTypeString();
			this.Text_Condition.text = GuildProxy.Language.GetInfoByID1("400101", guildData.LevelNeed);
		}

		[SerializeField]
		private CustomText Text_Add;

		[SerializeField]
		private CustomText Text_Condition;
	}
}
