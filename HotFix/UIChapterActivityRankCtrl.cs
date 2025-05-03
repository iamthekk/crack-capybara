using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Chapter;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIChapterActivityRankCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonCollect.onClick.AddListener(new UnityAction(this.OnClickCollect));
			this.ShowNetLoading(false);
			this.selfRankItem.Init();
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			this.buttonCollect.onClick.RemoveListener(new UnityAction(this.OnClickCollect));
			this.selfRankItem.DeInit();
			foreach (UIChapterActivityRankItem uichapterActivityRankItem in this.itemDic.Values)
			{
				uichapterActivityRankItem.DeInit();
			}
			this.itemDic.Clear();
		}

		public void SetData(ChapterActivity_RankActivity activity, ChapterActRankDto self, ulong rewardActRowId)
		{
			this.rankActivity = activity;
			this.rewardRowId = rewardActRowId;
			if (rewardActRowId > 0UL)
			{
				this.child.anchoredPosition = new Vector2(this.child.anchoredPosition.x, 168f);
			}
			else
			{
				this.child.anchoredPosition = new Vector2(this.child.anchoredPosition.x, 0f);
			}
			this.buttonCollect.gameObject.SetActiveSafe(rewardActRowId > 0UL);
			this.RefreshSelf(self);
			this.loopListView.SetListItemCount(0, true);
			this.loopListView.UpdateListView();
		}

		private void RefreshSelf(ChapterActRankDto self)
		{
			if (self == null)
			{
				return;
			}
			this.atlas = GameApp.Table.GetAtlasPath(this.rankActivity.atlasID);
			this.icon = this.rankActivity.itemIcon;
			this.selfRankItem.SetData(self, this.atlas, this.icon, this.rankActivity.rankID);
			this.selfRankItem.SetSelfBG();
		}

		public void RefreshRankList(RepeatedField<ChapterActRankDto> list, ChapterActRankDto self)
		{
			if (list == null || self == null)
			{
				return;
			}
			this.rankList = list;
			this.RefreshSelf(self);
			this.loopListView.SetListItemCount(this.rankList.Count, true);
			this.loopListView.UpdateListView();
			this.PlayAnimation();
		}

		private void OnClickCollect()
		{
			NetworkUtils.Chapter.DoChapterRankRewardRequest(delegate(bool result, ChapterRankRewardResponse response)
			{
				ChapterActivityData activeActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActiveActivityData(ChapterActivityKind.Rank);
				if (activeActivityData != null)
				{
					string text = GameTGATools.GetSourceName(11018) + Singleton<LanguageManager>.Instance.GetInfoByID(2, this.rankActivity.name);
					if (response.CommonData != null)
					{
						GameApp.SDK.Analyze.Track_Get(text, response.CommonData.Reward, null, null, null, null);
					}
				}
				GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).CollectReward(this.rewardRowId);
				if (response.CommonData != null && response.CommonData.Reward.Count > 0)
				{
					DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, new Action(this.CollectClose), true);
					return;
				}
				this.CollectClose();
			});
		}

		private void CollectClose()
		{
			GameApp.View.CloseView(ViewName.ChapterActivityRankViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_CloseCheckMain, null);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.rankList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIChapterActivityRankItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIChapterActivityRankItem uichapterActivityRankItem = this.GetUIItem(instanceID);
			if (uichapterActivityRankItem == null)
			{
				uichapterActivityRankItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			uichapterActivityRankItem.SetData(this.rankList[index], this.atlas, this.icon, this.rankActivity.rankID);
			return loopListViewItem;
		}

		private UIChapterActivityRankItem GetUIItem(int instanceId)
		{
			UIChapterActivityRankItem uichapterActivityRankItem;
			if (this.itemDic.TryGetValue(instanceId, out uichapterActivityRankItem))
			{
				return uichapterActivityRankItem;
			}
			return null;
		}

		private UIChapterActivityRankItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIChapterActivityRankItem uichapterActivityRankItem = this.GetUIItem(instanceID);
			if (uichapterActivityRankItem == null)
			{
				uichapterActivityRankItem = obj.GetComponent<UIChapterActivityRankItem>();
				uichapterActivityRankItem.Init();
				this.itemDic.Add(instanceID, uichapterActivityRankItem);
				return uichapterActivityRankItem;
			}
			return uichapterActivityRankItem;
		}

		private void PlayAnimation()
		{
			for (int i = 0; i < this.loopListView.ShownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = this.loopListView.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null))
				{
					RectTransform cachedRectTransform = shownItemByIndex.CachedRectTransform;
					cachedRectTransform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(cachedRectTransform, 1f, 0.2f), (float)i * 0.05f));
				}
			}
		}

		public void ShowNetLoading(bool isShow)
		{
			this.netLoadingObj.SetActiveSafe(isShow);
		}

		public RectTransform child;

		public LoopListView2 loopListView;

		public UIChapterActivityRankItem selfRankItem;

		public CustomButton buttonCollect;

		public GameObject netLoadingObj;

		private Dictionary<int, UIChapterActivityRankItem> itemDic = new Dictionary<int, UIChapterActivityRankItem>();

		private RepeatedField<ChapterActRankDto> rankList;

		private ChapterActivity_RankActivity rankActivity;

		private ulong rewardRowId;

		private string atlas;

		private string icon;
	}
}
