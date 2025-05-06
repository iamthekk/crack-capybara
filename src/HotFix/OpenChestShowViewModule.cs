using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class OpenChestShowViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.pageChestAnimation.Init();
			this.pageChestAnimation.gameObject.SetActive(false);
			this.pageRewardsShow.Init();
			this.pageRewardsShow.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.openData = data as OpenChestShowViewModule.OpenData;
			if (this.openData == null)
			{
				HLog.LogError("OpenChestShowViewModule data is null");
				GameApp.View.CloseView(ViewName.OpenEquipBoxViewModule, null);
				return;
			}
			this.pageChestAnimation.gameObject.SetActive(true);
			this.pageRewardsShow.gameObject.SetActive(false);
			this.pageChestAnimation.ShowRewards(this.openData.chestType, this.openData.itemDatas, new Action(this.OnShowResultRewards));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.pageRewardsShow.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.pageChestAnimation.gameObject.SetActive(false);
			this.pageRewardsShow.gameObject.SetActive(false);
			if (this.openData != null && this.openData.onCloseCallback != null)
			{
				this.openData.onCloseCallback(this.openData.chestType);
			}
		}

		public override void OnDelete()
		{
			this.pageChestAnimation.DeInit();
			this.pageRewardsShow.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnShowResultRewards()
		{
			if (this.openData.itemDatas.Count < 1)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.pageChestAnimation.gameObject.SetActive(false);
			this.pageRewardsShow.gameObject.SetActive(true);
			this.pageRewardsShow.ShowRewards(this.openData.chestType, this.openData.itemDatas, new Action(this.OnBtnCloseClick));
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.OpenChestShowViewModule, null);
		}

		[GameTestMethod("界面", "打开开箱子奖励界面", "", 0)]
		private static void OnOpenTest()
		{
			OpenChestShowViewModule.OnOpenTestImpl(1, 10, null);
		}

		public static void OnOpenTestImpl(int chestType, int itemCount, Action<int> callback = null)
		{
			OpenChestShowViewModule.OpenData openData = new OpenChestShowViewModule.OpenData();
			openData.chestType = chestType;
			openData.itemDatas = new List<ItemData>();
			openData.onCloseCallback = callback;
			IList<Item_Item> allElements = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetAllElements();
			for (int i = 0; i < itemCount; i++)
			{
				int num = Random.Range(0, allElements.Count);
				ItemData itemData = new ItemData(allElements[num].id, 1L);
				openData.itemDatas.Add(itemData);
			}
			GameApp.View.OpenView(ViewName.OpenChestShowViewModule, openData, 1, null, null);
		}

		public OpenChestShowView_PageChestAnimation pageChestAnimation;

		public OpenCheckShowView_PageRewardsShow pageRewardsShow;

		private OpenChestShowViewModule.OpenData openData;

		public class OpenData
		{
			public int chestType;

			public List<ItemData> itemDatas;

			public Action<int> onCloseCallback;
		}
	}
}
