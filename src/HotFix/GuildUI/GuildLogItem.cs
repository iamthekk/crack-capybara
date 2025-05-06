using System;
using System.Linq;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildLogItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		protected override void GuildUI_OnShow()
		{
		}

		public void RefreshData(GuildLogViewModule.GuildLogData data, DateTime lastDate)
		{
			Guild_guilddairy guild_guilddairy = GameApp.Table.GetManager().GetGuild_guilddairy(data.logType);
			string text;
			if (data.logType == 3)
			{
				string[] array = data.param.ToArray<string>();
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("guild_position_" + array[1]);
				string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("guild_position_" + array[2]);
				string[] array2 = new string[]
				{
					array[0],
					infoByID,
					infoByID2
				};
				LanguageManager instance = Singleton<LanguageManager>.Instance;
				string language = guild_guilddairy.language;
				object[] array3 = array2;
				text = instance.GetInfoByID(language, array3);
			}
			else
			{
				LanguageManager instance2 = Singleton<LanguageManager>.Instance;
				string language2 = guild_guilddairy.language;
				object[] array3 = data.param.ToArray<string>();
				text = instance2.GetInfoByID(language2, array3);
			}
			text = "[" + data.serverName + "] " + text;
			this.TextContent.text = text;
			this.TextHM.text = string.Format("{0:D2}:{1:D2}", data.time.Hour, data.time.Minute);
			if (data.time.Day == lastDate.Day && data.time.Month == lastDate.Month && data.time.Year == lastDate.Year)
			{
				this.TimeObj.SetActive(false);
			}
			else
			{
				this.TimeObj.SetActive(true);
				this.TextMD.text = string.Format("{0:D2}.{1:D2}", data.time.Month, data.time.Day);
			}
			this.textFilter.SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.contentTrans);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.parentTrans);
		}

		protected override void GuildUI_OnClose()
		{
		}

		public GameObject TimeObj;

		public CustomText TextMD;

		public CustomText TextHM;

		public CustomText TextContent;

		public ContentSizeFitter textFilter;

		public RectTransform parentTrans;

		public RectTransform contentTrans;
	}
}
