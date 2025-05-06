using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class CardFlippingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.textSubTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("card_flipping_tip");
		}

		public override void OnOpen(object data)
		{
			this.inAutoFlipping = false;
			this.isFlippingEnd = false;
			this.historyCounter.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.historyCounter.Add(i, 0);
			}
			for (int j = 0; j < this.cardItems.Count; j++)
			{
				this.cardItems[j].Init();
				this.cardItems[j].onItemClickCallback = new Action<CardFlippingCardItem>(this.OnCardItemClick);
				this.remainCards.Add(this.cardItems[j]);
			}
			this.openData = data as CardFlippingViewModule.OpenData;
			if (this.openData == null)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.xRandom = new XRandom(this.openData.randSeed);
			this.cfg = GameApp.Table.GetManager().GetChapterMiniGame_cardFlippingBaseModelInstance().GetElementById(this.openData.cardFlippingId);
			for (int k = 0; k < this.rewardPreviewGroupList.Count; k++)
			{
				this.rewardPreviewGroupList[k].Init();
				this.rewardPreviewGroupList[k].SetData(this.cfg);
			}
			this.GenerateEndResult();
			this.coroutinePlayCardBackEffect = base.StartCoroutine(this.PlayCardBackEffect());
			this.coroutineAutoStartFlippingTrigger = base.StartCoroutine(this.AutoStartFlippingTrigger());
		}

		private IEnumerator AutoStartFlippingTrigger()
		{
			if (this.openData.isInSweep)
			{
				yield return new WaitForSeconds(this.openData.autoStartDelay);
				if (this.remainCards.Count == this.cardItems.Count)
				{
					this.inAutoFlipping = true;
				}
				while (this.inAutoFlipping && !this.isFlippingEnd)
				{
					int num = Random.Range(0, this.remainCards.Count);
					this.OnCardItemClickImpl(this.remainCards[num], true);
					yield return new WaitForSeconds(1f);
				}
			}
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.coroutinePlayCardBackEffect != null)
			{
				base.StopCoroutine(this.coroutinePlayCardBackEffect);
				this.coroutinePlayCardBackEffect = null;
			}
			if (this.coroutineAutoStartFlippingTrigger != null)
			{
				base.StopCoroutine(this.coroutineAutoStartFlippingTrigger);
				this.coroutineAutoStartFlippingTrigger = null;
			}
			for (int i = 0; i < this.cardItems.Count; i++)
			{
				this.cardItems[i].DeInit();
			}
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void GenerateEndResult()
		{
			this.endResult = RandUtils.GetWeightedRandomSelection(new List<int>(this.cfg.weights), this.xRandom);
		}

		private int GetOpenRewardIndex()
		{
			int num = Random.Range(0, this.cfg.weights.Length);
			if (num != this.endResult && this.historyCounter[num] >= 2)
			{
				num = this.endResult;
			}
			Dictionary<int, int> dictionary = this.historyCounter;
			int num2 = num;
			int num3 = dictionary[num2];
			dictionary[num2] = num3 + 1;
			return num;
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.CardFlippingViewModule, null);
			EventCloseMiniGameUI eventCloseMiniGameUI = new EventCloseMiniGameUI();
			eventCloseMiniGameUI.SetData(MiniGameType.CardFlipping, this.historyList);
			GameApp.Event.DispatchNow(this, 375, eventCloseMiniGameUI);
		}

		private void OnCardItemClick(CardFlippingCardItem item)
		{
			this.OnCardItemClickImpl(item, false);
		}

		private void OnCardItemClickImpl(CardFlippingCardItem item, bool isAutoClick = false)
		{
			if (this.isFlippingEnd)
			{
				return;
			}
			if (item == null)
			{
				return;
			}
			if (item.isOpened)
			{
				return;
			}
			if (!isAutoClick)
			{
				this.inAutoFlipping = false;
			}
			this.remainCards.Remove(item);
			int openRewardIndex = this.GetOpenRewardIndex();
			int result = openRewardIndex;
			int count = this.historyCounter[openRewardIndex];
			MiniGameResult miniGameResult = MiniGameResult.None;
			if (count >= 3)
			{
				this.isFlippingEnd = true;
				this.remainCards.Clear();
				switch (result)
				{
				case 0:
					miniGameResult = MiniGameResult.GearOne;
					break;
				case 1:
					miniGameResult = MiniGameResult.GearTwo;
					break;
				case 2:
					miniGameResult = MiniGameResult.GearThree;
					break;
				}
			}
			bool flag = this.isFlippingEnd;
			item.PlayFlipping(result, this.cfg, flag);
			DelayCall.Instance.CallOnce(400, delegate
			{
				if (this.gameObject == null)
				{
					return;
				}
				for (int i = 0; i < this.rewardPreviewGroupList.Count; i++)
				{
					if (this.rewardPreviewGroupList[i].rewardType == (CardFlippingViewModule.CardFlippingRewardType)result)
					{
						this.rewardPreviewGroupList[i].PlayRewardAnimation(result, count);
					}
				}
			});
			if (flag)
			{
				DelayCall.Instance.CallOnce(500, delegate
				{
					if (this == null || this.gameObject == null)
					{
						return;
					}
					for (int j = 0; j < this.cardItems.Count; j++)
					{
						if (this.cardItems[j] != null && this.cardItems[j].isOpened && this.cardItems[j].result == result)
						{
							this.cardItems[j].PlayEndRewardAnimation();
						}
					}
					for (int k = 0; k < this.rewardPreviewGroupList.Count; k++)
					{
						if (this.rewardPreviewGroupList[k].rewardType == (CardFlippingViewModule.CardFlippingRewardType)result)
						{
							this.rewardPreviewGroupList[k].PlayEndRewardAnimation();
						}
					}
				});
				DelayCall.Instance.CallOnce(1500, delegate
				{
					if (this == null || this.gameObject == null)
					{
						return;
					}
					string[] rewards = item.rewards;
					if (rewards[0] == "1")
					{
						NodeAttParam nodeAttParam = MiniGameUtils.GetNodeAttParam(rewards, ChapterDropSource.CardFlipping, this.openData.rewardRate);
						GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
						Singleton<GameEventController>.Instance.MergerAttribute(nodeAttParam);
						GameEventMiniGameData gameEventMiniGameData = new GameEventMiniGameData();
						gameEventMiniGameData.SetData(MiniGameType.CardFlipping, nodeAttParam, item.cfg.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData);
						RewardAttributeViewModule.OpenData openData = new RewardAttributeViewModule.OpenData();
						openData.nodeAttParam = nodeAttParam;
						openData.viewCloseCallback = new Action(this.OnBtnCloseClick);
						openData.isInSweep = this.openData.isInSweep;
						openData.autoCloseTime = 1f;
						GameApp.View.OpenView(ViewName.RewardAttributeViewModule, openData, 1, null, null);
						return;
					}
					if (rewards[0] == "2")
					{
						GameEventSkillBuildData skillBuildData = MiniGameUtils.GetSkillBuildData(rewards, this.openData.randSeed);
						GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { skillBuildData }, true);
						GameEventMiniGameData gameEventMiniGameData2 = new GameEventMiniGameData();
						gameEventMiniGameData2.SetData(MiniGameType.CardFlipping, skillBuildData, item.cfg.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData2);
						GameEventSlotSkillViewModule.OpenData openData2 = new GameEventSlotSkillViewModule.OpenData();
						openData2.skillBuild = skillBuildData;
						openData2.viewCloseCallback = new Action(this.OnBtnCloseClick);
						openData2.seed = this.openData.randSeed;
						GameApp.View.OpenView(ViewName.GameEventSlotSkillViewModule, openData2, 1, null, null);
						return;
					}
					if (rewards[0] == "3")
					{
						NodeItemParam nodeItemParam = MiniGameUtils.GetNodeItemParam(rewards, NodeItemType.Item, ChapterDropSource.CardFlipping, this.openData.rewardRate);
						GameTGATools.Ins.AddStageClickTempItem(new List<NodeItemParam> { nodeItemParam }, true);
						Singleton<GameEventController>.Instance.AddDrop(nodeItemParam);
						GameEventMiniGameData gameEventMiniGameData3 = new GameEventMiniGameData();
						gameEventMiniGameData3.SetData(MiniGameType.CardFlipping, nodeItemParam, item.cfg.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData3);
						RewardCommonData rewardCommonData = new RewardCommonData();
						rewardCommonData.list = new List<ItemData> { nodeItemParam.ToItemData() };
						rewardCommonData.OnClose = new Action(this.OnBtnCloseClick);
						rewardCommonData.m_isFly = false;
						rewardCommonData.m_isInSweep = this.openData.isInSweep;
						rewardCommonData.m_autoCloseTime = 1f;
						GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
						return;
					}
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("turntable_reward_empty"));
					GameEventMiniGameData gameEventMiniGameData4 = new GameEventMiniGameData();
					gameEventMiniGameData4.SetData(MiniGameType.CardFlipping, null, item.cfg.id, MiniGameResult.None);
					this.historyList.Add(gameEventMiniGameData4);
				});
			}
		}

		private IEnumerator PlayCardBackEffect()
		{
			while (this.remainCards.Count > 0)
			{
				float num = Random.Range(3f, 5f);
				yield return new WaitForSeconds(num);
				if (this.remainCards.Count == 0)
				{
					break;
				}
				int num2 = Random.Range(0, this.remainCards.Count);
				this.remainCards[num2].PlayBackEffect();
			}
			yield break;
		}

		[GameTestMethod("事件小游戏", "翻牌子", "", 101)]
		private static void OpenCardFlipping()
		{
			CardFlippingViewModule.OpenData openData = new CardFlippingViewModule.OpenData();
			openData.cardFlippingId = 1;
			openData.randSeed = Random.Range(1, 10000);
			openData.rewardRate = 1;
			openData.isInSweep = true;
			openData.autoStartDelay = 1f;
			GameApp.View.OpenView(ViewName.CardFlippingViewModule, openData, 1, null, null);
		}

		public RectTransform content;

		public CustomText textSubTitle;

		public List<CardFlippingRewardPreviewGroup> rewardPreviewGroupList = new List<CardFlippingRewardPreviewGroup>();

		public List<CardFlippingCardItem> cardItems = new List<CardFlippingCardItem>();

		private ChapterMiniGame_cardFlippingBase cfg;

		private int endResult;

		private Dictionary<int, int> historyCounter = new Dictionary<int, int>();

		private List<GameEventMiniGameData> historyList = new List<GameEventMiniGameData>();

		private bool isFlippingEnd;

		private List<CardFlippingCardItem> remainCards = new List<CardFlippingCardItem>();

		private CardFlippingViewModule.OpenData openData;

		private XRandom xRandom;

		private bool inAutoFlipping;

		private Coroutine coroutinePlayCardBackEffect;

		private Coroutine coroutineAutoStartFlippingTrigger;

		public enum CardFlippingRewardType
		{
			SmallReward,
			MiddleReward,
			BigReward
		}

		public class OpenData
		{
			public int cardFlippingId;

			public int randSeed = 100;

			public int rewardRate = 1;

			public bool isInSweep;

			public float autoStartDelay = 1f;
		}
	}
}
