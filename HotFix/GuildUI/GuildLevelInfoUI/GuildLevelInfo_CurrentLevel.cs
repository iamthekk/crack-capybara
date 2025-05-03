using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI.GuildLevelInfoUI
{
	public class GuildLevelInfo_CurrentLevel : GuildLevelInfo_Base
	{
		public int CurrentLevel
		{
			get
			{
				return this.currentLevel;
			}
		}

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
			this.buttonLeft.onClick.AddListener(new UnityAction(this.SwitchLastLevel));
			this.buttonRight.onClick.AddListener(new UnityAction(this.SwitchNextLevel));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton customButton = this.buttonLeft;
			if (customButton != null)
			{
				customButton.onClick.RemoveListener(new UnityAction(this.SwitchLastLevel));
			}
			CustomButton customButton2 = this.buttonRight;
			if (customButton2 != null)
			{
				customButton2.onClick.RemoveListener(new UnityAction(this.SwitchNextLevel));
			}
			for (int i = 0; i < this.detailItemList.Count; i++)
			{
				this.detailItemList[i].DeInit();
				Object.Destroy(this.detailItemList[i].gameObject);
			}
			this.detailItemList.Clear();
			base.GuildUI_OnUnInit();
		}

		public override void RefreshUI()
		{
			this.textLevel.text = this.currentLevel.ToString();
			int maxLevel = this.MaxLevel;
			this.buttonRight.gameObject.SetActiveSafe(this.currentLevel < maxLevel);
			this.buttonLeft.gameObject.SetActiveSafe(this.currentLevel > 1);
			List<GuildShareDataEx.LevelChangeData> levelDetailList = GuildUITool.GetLevelDetailList(this.currentLevel);
			for (int i = 0; i < this.detailItemList.Count; i++)
			{
				this.detailItemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < levelDetailList.Count; j++)
			{
				UIGuildLevelDetailItem uiguildLevelDetailItem;
				if (j < this.detailItemList.Count)
				{
					uiguildLevelDetailItem = this.detailItemList[j];
				}
				else
				{
					uiguildLevelDetailItem = Object.Instantiate<GameObject>(this.prefab, this.rtfParent).GetComponent<UIGuildLevelDetailItem>();
					uiguildLevelDetailItem.Init();
					this.detailItemList.Add(uiguildLevelDetailItem);
					uiguildLevelDetailItem.ResetAni();
				}
				uiguildLevelDetailItem.gameObject.SetActiveSafe(true);
				uiguildLevelDetailItem.Refresh(levelDetailList[j]);
				uiguildLevelDetailItem.ClearAni();
			}
		}

		public void SetCurrentLevel(int level)
		{
			this.currentLevel = level;
		}

		private void SwitchLastLevel()
		{
			if (this.currentLevel > 1)
			{
				this.currentLevel--;
				this.RefreshUI();
			}
		}

		private void SwitchNextLevel()
		{
			if (this.currentLevel < this.MaxLevel)
			{
				this.currentLevel++;
				this.RefreshUI();
			}
		}

		public CustomText textLevel;

		public RectTransform rtfParent;

		public GameObject prefab;

		public CustomButton buttonLeft;

		public CustomButton buttonRight;

		private List<UIGuildLevelDetailItem> detailItemList = new List<UIGuildLevelDetailItem>();

		private int currentLevel = 1;
	}
}
