using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI.GuildLevelInfoUI
{
	public class GuildLevelInfo_NextLevel : GuildLevelInfo_Base
	{
		private int MaxLevel
		{
			get
			{
				return GuildProxy.Table.GetGuildLevelTableAll().Count;
			}
		}

		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.prefab.SetActive(false);
			this.buttonLevelUP.onClick.AddListener(new UnityAction(this.OnClickLevelUP));
			this.objMaxTips.SetActive(false);
			this.objAutoTip.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton customButton = this.buttonLevelUP;
			if (customButton != null)
			{
				customButton.onClick.RemoveListener(new UnityAction(this.OnClickLevelUP));
			}
			for (int i = 0; i < this.needItemList.Count; i++)
			{
				this.needItemList[i].DeInit();
				Object.Destroy(this.needItemList[i].gameObject);
			}
			this.needItemList.Clear();
			base.GuildUI_OnUnInit();
		}

		public override void RefreshUI()
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			if (guildData.GuildLevel >= this.MaxLevel)
			{
				this.objMaxTips.SetActive(true);
				this.objAutoTip.SetActive(false);
				this.rtfParent.gameObject.SetActive(false);
				this.buttonLevelUP.gameObject.SetActive(false);
				return;
			}
			this.objMaxTips.SetActive(false);
			this.rtfParent.gameObject.SetActive(true);
			bool flag = guildData.GuildLevel >= this.MaxLevel;
			bool flag2 = base.SDK.Permission.HasPermission(GuildPermissionKind.LevelUp, null);
			bool flag3 = guildData.IsCanLevelUp();
			bool flag4 = false;
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(113);
			if (guildConstTable != null)
			{
				flag4 = guildConstTable.TypeInt == 1;
			}
			this.buttonLevelUP.gameObject.SetActiveSafe(flag2 && !flag4 && flag3 && !flag);
			this.objAutoTip.SetActive(flag4);
			this.textLevel.text = (base.SDK.GuildInfo.GuildData.GuildLevel + 1).ToString();
			for (int i = 0; i < this.needItemList.Count; i++)
			{
				this.needItemList[i].gameObject.SetActiveSafe(false);
			}
			List<GuildShareDataEx.LevelInfoData> levelUpNeedList = base.SDK.GuildInfo.GuildData.GetLevelUpNeedList();
			for (int j = 0; j < levelUpNeedList.Count; j++)
			{
				UIGuildLevelInfoItem uiguildLevelInfoItem;
				if (j < this.needItemList.Count)
				{
					uiguildLevelInfoItem = this.needItemList[j];
				}
				else
				{
					uiguildLevelInfoItem = Object.Instantiate<GameObject>(this.prefab, this.rtfParent).GetComponent<UIGuildLevelInfoItem>();
					uiguildLevelInfoItem.Init();
					this.needItemList.Add(uiguildLevelInfoItem);
				}
				uiguildLevelInfoItem.gameObject.SetActiveSafe(true);
				uiguildLevelInfoItem.Refresh(levelUpNeedList[j]);
			}
		}

		private void OnClickLevelUP()
		{
			Action onClickToLevelUP = this.OnClickToLevelUP;
			if (onClickToLevelUP == null)
			{
				return;
			}
			onClickToLevelUP();
		}

		public CustomText textLevel;

		public RectTransform rtfParent;

		public GameObject prefab;

		public GameObject objMaxTips;

		public GameObject objAutoTip;

		public CustomButton buttonLevelUP;

		private List<UIGuildLevelInfoItem> needItemList = new List<UIGuildLevelInfoItem>();

		public Action OnClickToLevelUP;
	}
}
