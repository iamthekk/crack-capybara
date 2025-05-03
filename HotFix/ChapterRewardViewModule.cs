using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterRewardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.chapterRewardObj.SetActiveSafe(false);
			this.rewardObj.SetActiveSafe(false);
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnGetReward));
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnBack));
			this.chapterDataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.customScroll.copyItem = this.chapterRewardObj;
			this.customScroll.mScrollChild = this.child.transform;
			this.customScroll.OnUpdateOne = new Action<int, CustomScrollItem>(this.UpdateOne);
			this.customScroll.OnScrollEnd = new Action<int, CustomScrollItem>(this.OnScrollEnd);
			this.customScroll.SetShowWidthScale(1f);
			this.customScroll.maxScale = 1f;
			this.customScroll.minScale = 0.56f;
			this.customScroll.DragDisableForce = true;
			this.chapterRewardList = this.chapterDataModule.GetAllChapterRewardDataList();
			this.customScroll.Init(this.chapterRewardList.Count);
			List<int> list = new List<int>();
			list.Add(9);
			list.Add(2);
			list.Add(1);
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.ChapterReward, list);
		}

		public override void OnOpen(object data)
		{
			this.Refresh(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.currencyCtrl.DeInit();
			this.dic.Clear();
			this.rewardItemList.Clear();
			this.chapterRewardList = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Refresh(bool playAni)
		{
			this.chapterRewardData = this.chapterDataModule.GetShowReward();
			if (this.chapterRewardData == null)
			{
				return;
			}
			this.showIndex = this.chapterRewardList.IndexOf(this.chapterRewardData);
			this.customScroll.GotoInt(this.showIndex, playAni);
			Vector2 sizeDelta = this.rewardBgTrans.sizeDelta;
			this.textDes.text = (this.chapterRewardData.IsNeedPass ? Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Pass", new object[] { this.chapterRewardData.chapterId }) : Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Survived", new object[] { this.chapterRewardData.stage }));
			if (this.chapterRewardData.state == ChapterRewardData.ChapterRewardState.Lock)
			{
				this.textLockTip.text = (this.chapterRewardData.IsNeedPass ? Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_PassTip", new object[] { this.chapterRewardData.chapterId }) : Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_SurviveTip", new object[]
				{
					this.chapterRewardData.chapterId,
					this.chapterRewardData.stage
				}));
				this.lockObj.SetActiveSafe(true);
			}
			else if (this.chapterRewardData.state == ChapterRewardData.ChapterRewardState.Finish)
			{
				this.textLockTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_RewardFinish");
				this.lockObj.SetActiveSafe(true);
			}
			else
			{
				this.textLockTip.text = "";
				this.lockObj.SetActiveSafe(false);
			}
			this.buttonGet.gameObject.SetActiveSafe(this.chapterRewardData.state == ChapterRewardData.ChapterRewardState.CanGet);
			this.RefreshRewardItem();
		}

		private void RefreshRewardItem()
		{
			List<PropData> rewardDataList = this.chapterRewardData.GetRewardDataList();
			for (int i = 0; i < this.rewardItemList.Count; i++)
			{
				this.rewardItemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < rewardDataList.Count; j++)
			{
				UIItem uiitem;
				if (j < this.rewardItemList.Count)
				{
					uiitem = this.rewardItemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.rewardObj);
					gameObject.SetParentNormal(this.rewardParent, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.rewardItemList.Add(uiitem);
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(rewardDataList[j]);
				uiitem.OnRefresh();
			}
		}

		private UIChapterRewardItem GetUIItem(int instanceId)
		{
			UIChapterRewardItem uichapterRewardItem;
			if (this.dic.TryGetValue(instanceId, out uichapterRewardItem))
			{
				return uichapterRewardItem;
			}
			return null;
		}

		private UIChapterRewardItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIChapterRewardItem uichapterRewardItem = this.GetUIItem(instanceID);
			if (uichapterRewardItem == null)
			{
				uichapterRewardItem = obj.GetComponent<UIChapterRewardItem>();
				uichapterRewardItem.Init();
				this.dic.Add(instanceID, uichapterRewardItem);
				return uichapterRewardItem;
			}
			return uichapterRewardItem;
		}

		private void UpdateOne(int index, CustomScrollItem one)
		{
			if (index < 0 || index >= this.chapterRewardList.Count)
			{
				return;
			}
			int instanceID = one.gameObject.GetInstanceID();
			UIChapterRewardItem uichapterRewardItem = this.GetUIItem(instanceID);
			if (uichapterRewardItem == null)
			{
				uichapterRewardItem = this.AddUIItem(one.gameObject);
			}
			uichapterRewardItem.Refresh(this.chapterRewardList[index]);
		}

		private void OnGetReward()
		{
			NetworkUtils.Chapter.DoGetWaveRewardRequest(this.chapterRewardData.chapterId, this.chapterRewardData.stage, delegate(bool result, GetWaveRewardResponse resp)
			{
				if (result && resp != null)
				{
					if (resp.CommonData.Reward.Count > 0)
					{
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					}
					this.Refresh(true);
				}
			});
		}

		private void OnBack()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainCity_Refresh, null);
			GameApp.View.CloseView(ViewName.ChapterRewardViewModule, null);
		}

		private void OnScrollEnd(int arg1, CustomScrollItem arg2)
		{
			foreach (UIChapterRewardItem uichapterRewardItem in this.dic.Values)
			{
				if (uichapterRewardItem.gameObject.GetInstanceID().Equals(arg2.gameObject.GetInstanceID()))
				{
					uichapterRewardItem.SetMask(false);
				}
				else
				{
					uichapterRewardItem.SetMask(true);
				}
			}
		}

		public CustomText textDes;

		public CustomText textLockTip;

		public CustomButton buttonGet;

		public CustomButton buttonBack;

		public CustomScroll customScroll;

		public GameObject child;

		public GameObject chapterRewardObj;

		public GameObject rewardParent;

		public GameObject rewardObj;

		public RectTransform rewardBgTrans;

		public ModuleCurrencyCtrl currencyCtrl;

		public GameObject lockObj;

		private ChapterDataModule chapterDataModule;

		private ChapterRewardData chapterRewardData;

		private int showIndex;

		private Dictionary<int, UIChapterRewardItem> dic = new Dictionary<int, UIChapterRewardItem>();

		private List<UIItem> rewardItemList = new List<UIItem>();

		private List<ChapterRewardData> chapterRewardList;
	}
}
