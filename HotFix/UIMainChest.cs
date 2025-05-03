using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Collection;
using Proto.Common;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainChest : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			try
			{
				this.chestDataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
				this.prefabChestItem.gameObject.SetActive(false);
				this.currencyCtrl.Init();
				this.currencyCtrl.SetStyle(EModuleId.UIMainChest, new List<int> { 1, 2, 9 });
				this.spineChest.Initialize(true);
				this.scoreNode.Init();
				this.btnOpen.m_onClick = new Action(this.OnBtnOpenClick);
				this.btnArrowLeft.m_onClick = delegate
				{
					this.IndexChange(-1);
					this.UpdateView();
				};
				this.btnArrowRight.m_onClick = delegate
				{
					this.IndexChange(1);
					this.UpdateView();
				};
				IList<ChestList_ChestReward> allElements = GameApp.Table.GetManager().GetChestList_ChestRewardModelInstance().GetAllElements();
				this.scrollRect.horizontal = allElements.Count > 5;
				for (int i = 0; i < allElements.Count; i++)
				{
					UIMainChest_ChestItem uimainChest_ChestItem = Object.Instantiate<UIMainChest_ChestItem>(this.prefabChestItem, this.prefabChestItem.transform.parent, false);
					this.chestItemList.Add(uimainChest_ChestItem);
					this.chestItemList[i].gameObject.SetActive(true);
					this.chestItemList[i].Init();
					this.chestItemList[i].SetCallback(new Action<UIMainChest_ChestItem>(this.OnChestItemClick));
					this.chestItemList[i].SetData(allElements[i]);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		protected override void OnDeInit()
		{
			this.btnOpen.m_onClick = null;
			this.currencyCtrl.DeInit();
			for (int i = 0; i < this.chestItemList.Count; i++)
			{
				this.chestItemList[i].DeInit();
			}
			this.chestItemList.Clear();
			this.scoreNode.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Chest_ChestChange, new HandlerEvent(this.OnChestChangeHandler));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Chest_ScoreRewardChange, new HandlerEvent(this.OnChestScoreRewardChangeHandler));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
			this.IndexChange(0);
			this.UpdateView();
			this.scoreNode.UpdateProgress(false);
			this.animator.Play("Show");
			this.currencyCtrl.SetFlyPosition();
		}

		protected override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Chest_ChestChange, new HandlerEvent(this.OnChestChangeHandler));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Chest_ScoreRewardChange, new HandlerEvent(this.OnChestScoreRewardChangeHandler));
		}

		public override void OnLanguageChange()
		{
		}

		public void IndexChange(int changeValue)
		{
			if (changeValue > 0)
			{
				if (this.mCurIndex >= this.chestItemList.Count - 1)
				{
					this.mCurIndex = this.chestItemList.Count - 1;
				}
				else
				{
					this.mCurIndex++;
				}
			}
			else if (changeValue < 0)
			{
				if (this.mCurIndex <= 0)
				{
					this.mCurIndex = 0;
				}
				else
				{
					this.mCurIndex--;
				}
			}
			this.curChestItem = this.chestItemList[this.mCurIndex];
		}

		private void UpdateView()
		{
			this.UpdateChestInfo();
			this.UpdateBtnArrow();
			this.UpdateFlagView();
			this.UpdateChestSelect();
		}

		private void UpdateBtnArrow()
		{
			if (this.mCurIndex <= 0)
			{
				this.btnArrowLeft.gameObject.SetActive(false);
				this.btnArrowRight.gameObject.SetActive(true);
				return;
			}
			if (this.mCurIndex >= this.chestItemList.Count - 1)
			{
				this.btnArrowLeft.gameObject.SetActive(true);
				this.btnArrowRight.gameObject.SetActive(false);
				return;
			}
			this.btnArrowLeft.gameObject.SetActive(true);
			this.btnArrowRight.gameObject.SetActive(true);
		}

		private void UpdateFlagView()
		{
			try
			{
				this.imgFlag.sprite = this.flagSprites[this.curChestItem.chestType - 1];
				this.imgFlagSticker.sprite = this.flagStickers[this.curChestItem.chestType - 1];
				this.txtFlagTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.curChestItem.cfg.name);
				this.txtFlagDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.curChestItem.cfg.desc);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		private void UpdateChestSelect()
		{
			for (int i = 0; i < this.chestItemList.Count; i++)
			{
				this.chestItemList[i].SetSelect(i == this.mCurIndex);
			}
		}

		private void UpdateChestInfo()
		{
			this.UpdateOpenBet();
			string boxSkinName = MainChestType.GetBoxSkinName(this.curChestItem.cfg.chestType);
			this.spineChest.Skeleton.SetSkin(boxSkinName);
			this.spineChest.Skeleton.SetSlotsToSetupPose();
			this.spineChest.AnimationState.SetAnimation(0, "Appear", false);
			this.spineChest.AnimationState.AddAnimation(0, "Idle", true, 0f);
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.curChestItem.cfg.itemId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("itemId:{0} not found in item config", this.curChestItem.cfg.itemId));
			}
			this.imgChest.SetImage(elementById.atlasID, elementById.icon);
			if (this.openBet == 0)
			{
				this.txtBtnOpen.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_open_0");
				this.btnOpen.GetComponent<UIGrays>().SetUIGray();
			}
			else
			{
				this.txtBtnOpen.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_open_bet", new object[] { this.openBet });
				this.btnOpen.GetComponent<UIGrays>().Recovery();
			}
			for (int i = 0; i < this.chestItemList.Count; i++)
			{
				this.chestItemList[i].UpdateView();
			}
		}

		private void UpdateOpenBet()
		{
			int itemId = this.curChestItem.cfg.itemId;
			int chestType = this.curChestItem.cfg.chestType;
			long chestCount = this.chestDataModule.GetChestCount(itemId);
			if (chestCount <= 0L)
			{
				this.openBet = 0;
				return;
			}
			if (chestCount < 10L)
			{
				this.openBet = (int)chestCount;
				return;
			}
			if (chestCount < 100L)
			{
				this.openBet = 10;
				return;
			}
			this.openBet = 50;
		}

		private void OnBtnOpenClick()
		{
			if (this.openBet == 0)
			{
				return;
			}
			int ScoreChestType = this.curChestItem.cfg.chestType;
			ItemDto itemDataByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataByid((ulong)((long)this.curChestItem.cfg.itemId));
			if (itemDataByid == null)
			{
				HLog.LogError("itemDto is null");
				return;
			}
			long preScore = this.chestDataModule.GetCurScore();
			NetworkUtils.Chest.ChestUseRequest(itemDataByid.RowId, this.openBet, delegate(bool isOk, ChestUseResponse response)
			{
				if (isOk)
				{
					RepeatedField<RewardDto> reward = response.CommonData.Reward;
					if (reward.Count > 0)
					{
						OpenChestShowViewModule.OpenData openData = new OpenChestShowViewModule.OpenData();
						openData.chestType = ScoreChestType;
						openData.itemDatas = reward.ToItemDatas();
						openData.onCloseCallback = new Action<int>(this.PlayScoreAnimation);
						GameApp.View.OpenView(ViewName.OpenChestShowViewModule, openData, 1, null, null);
						GameApp.SDK.Analyze.Track_TreasureOpen(ScoreChestType, this.openBet, response.CommonData.Reward);
						long curScore = this.chestDataModule.GetCurScore();
						GameApp.SDK.Analyze.Track_TreasurePoint_Collect(curScore - preScore);
						return;
					}
					HLog.LogError("main chest rewardList count is 0");
				}
			});
		}

		private void PlayScoreAnimation(int scoreChestType)
		{
			this.UpdateChestInfo();
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			this.PlayScoreAnimationImpl(scoreChestType);
		}

		private void PlayScoreAnimationImpl(int scoreChestType)
		{
			ChestList_ChestReward elementById = GameApp.Table.GetManager().GetChestList_ChestRewardModelInstance().GetElementById(scoreChestType);
			if (elementById == null)
			{
				HLog.LogError(string.Format("scoreChestType:{0} not found in chest config", scoreChestType));
				return;
			}
			if (elementById.openPoint <= 0)
			{
				return;
			}
			this.scoreEffectImgs.Clear();
			for (int i = 0; i < 5; i++)
			{
				CustomImage customImage = Object.Instantiate<CustomImage>(this.scoreNode.imgScoreIcon, this.scoreEffectNode, false);
				customImage.name = "imgTest" + i.ToString();
				customImage.transform.position = this.chestCenterPoint.position;
				customImage.gameObject.SetActive(false);
				this.scoreEffectImgs.Add(customImage);
			}
			float endX = this.scoreNode.imgScoreIcon.transform.position.x;
			float endY = this.scoreNode.imgScoreIcon.transform.position.y;
			for (int j = 0; j < this.scoreEffectImgs.Count; j++)
			{
				int num = j;
				CustomImage imgScore = this.scoreEffectImgs[num];
				int num2 = num * 100;
				DelayCall.Instance.CallOnce(num2, delegate
				{
					if (imgScore == null)
					{
						return;
					}
					imgScore.gameObject.SetActive(true);
					TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMoveX(imgScore.transform, endX, 0.4f, false), this.easeType);
					ShortcutExtensions.DOMoveY(imgScore.transform, endY, 0.4f, false);
				});
				DelayCall.Instance.CallOnce(num2 + 400, delegate
				{
					if (imgScore == null)
					{
						return;
					}
					if (imgScore != null && imgScore.gameObject != null)
					{
						Object.Destroy(imgScore.gameObject);
					}
				});
			}
			DelayCall.Instance.CallOnce(this.scoreEffectImgs.Count * 100, delegate
			{
				if (this.scoreNode == null)
				{
					return;
				}
				this.scoreNode.UpdateProgress(true);
			});
		}

		private void OnChestItemClick(UIMainChest_ChestItem chestItem)
		{
			if (this.chestItemList.IndexOf(chestItem) == this.mCurIndex)
			{
				return;
			}
			this.mCurIndex = this.chestItemList.IndexOf(chestItem);
			this.curChestItem = this.chestItemList[this.mCurIndex];
			this.UpdateView();
		}

		private void OnChestChangeHandler(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateChestInfo();
		}

		private void OnChestScoreRewardChangeHandler(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.scoreNode != null)
			{
				this.scoreNode.UpdateProgress(false);
			}
		}

		public Animator animator;

		public ModuleCurrencyCtrl currencyCtrl;

		public SkeletonGraphic spineChest;

		public List<Sprite> flagSprites = new List<Sprite>();

		public List<Sprite> flagStickers = new List<Sprite>();

		public CustomImage imgFlag;

		public CustomImage imgFlagSticker;

		public CustomText txtFlagTitle;

		public CustomText txtFlagDesc;

		public UIMainChest_ChestItem prefabChestItem;

		public ScrollRect scrollRect;

		public CustomButton btnArrowLeft;

		public CustomButton btnArrowRight;

		public UIMainChest_ScoreNode scoreNode;

		public CustomButton btnOpen;

		public CustomText txtBtnOpen;

		public CustomImage imgChest;

		public Transform chestCenterPoint;

		public Transform scoreEffectNode;

		private List<UIMainChest_ChestItem> chestItemList = new List<UIMainChest_ChestItem>();

		private int mCurIndex;

		private UIMainChest_ChestItem curChestItem;

		private int openBet = 1;

		private ChestDataModule chestDataModule;

		private int addScore;

		private List<CustomImage> scoreEffectImgs = new List<CustomImage>();

		public Ease easeType = 27;
	}
}
