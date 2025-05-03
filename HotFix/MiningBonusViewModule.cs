using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class MiningBonusViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.copyItem.SetActiveSafe(false);
			this.clickMask.SetActiveSafe(false);
			this.freeTimesObj.SetActiveSafe(false);
			this.endTipObj.SetActiveSafe(false);
			this.progressAniObj.SetActiveSafe(false);
			if (this.fxSuccess)
			{
				this.fxSuccess.gameObject.SetActiveSafe(false);
			}
			this.copyFxReward.SetActiveSafe(false);
			this.rewardNodeCtrl.Init();
			this.tanHuangItem.Init();
			this.rateTipCtr.Init();
			this.buttonBuy.Init();
			this.buttonBuy.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("uimining_transport"));
			this.buttonBuy.SetData(new Action(this.OnClickBuy));
			this.buttonHelp.onClick.AddListener(new UnityAction(this.OnClickHelp));
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickBack));
			this.buttonRate.onClick.AddListener(new UnityAction(this.OnClickRate));
			this.tanHuangItem.PlayAnimation("Idle_01", true);
		}

		public override void OnOpen(object data)
		{
			this.modelItem.Init();
			this.modelItem.OnShow();
			this.modelItem.ShowSelfPlayerModel("UIMiningBonus", false);
			this.tipsObj.SetActiveSafe(true);
			this.infoObj.SetActiveSafe(true);
			if (this.miningDataModule.MiningDraw != null)
			{
				this.currentRate = this.miningDataModule.MiningDraw.Rate;
				this.RefreshBonusProgress(0, this.miningDataModule.MiningDraw.Progress);
			}
			this.Refresh();
			this.PlayerOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.rateTipCtr.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.minerItems.Count; i++)
			{
				UIMinerItem uiminerItem = this.minerItems[i];
				if (uiminerItem)
				{
					uiminerItem.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
			this.modelItem.OnHide(false);
			this.modelItem.DeInit();
		}

		public override void OnDelete()
		{
			this.buttonHelp.onClick.RemoveListener(new UnityAction(this.OnClickHelp));
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickBack));
			this.buttonRate.onClick.RemoveListener(new UnityAction(this.OnClickRate));
			this.rewardNodeCtrl.DeInit();
			this.tanHuangItem.DeInit();
			this.buttonBuy.DeInit();
			this.rateTipCtr.DeInit();
			this.showRewardDataList.Clear();
			this.getRewardDataList.Clear();
			this.allRewardDataList.Clear();
			for (int i = 0; i < this.minerItems.Count; i++)
			{
				if (this.minerItems[i])
				{
					this.minerItems[i].DeInit();
				}
			}
			this.minerItems.Clear();
			this.sequencePool.Clear(false);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			MiningDrawDto miningDraw = this.miningDataModule.MiningDraw;
			if (miningDraw == null)
			{
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_tips_1", new object[] { miningDraw.Progress });
			string text = infoByID;
			if (miningDraw.FreeTimes > 0)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_bonus_continue");
			}
			this.RefreshTips(text, infoByID);
			this.rateObj.SetActiveSafe(miningDraw.FreeTimes <= 0);
			if (miningDraw.FreeTimes <= 0)
			{
				this.rateTipCtr.SetShow(false);
			}
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(GameConfig.Mining_Draw_ItemId);
			if (item_Item != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
				this.imageCost.SetImage(atlasPath, item_Item.icon);
			}
			else
			{
				HLog.LogError(string.Format("Table Item_item not found id={0}", GameConfig.Mining_Draw_ItemId));
			}
			this.RefreshRate();
		}

		private void RefreshTips(string tips, string info)
		{
			this.textTips.text = tips;
			this.textInfo.text = info;
		}

		private void RefreshBonusProgress(int cur, int limit)
		{
			this.textBonus.text = string.Format("{0}/{1}", cur, limit);
		}

		private void RefreshRate()
		{
			int maxRate = this.GetMaxRate();
			this.textRate.text = string.Format("x{0}", this.currentRate);
			this.rateTipCtr.SetData(this.currentRate, this.currentRate == maxRate);
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)GameConfig.Mining_Draw_ItemId));
			int num = GameConfig.Mining_Draw_Cost * this.currentRate;
			int freeTimes = this.miningDataModule.MiningDraw.FreeTimes;
			this.textCost.text = ((freeTimes > 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("uimining_free") : string.Format("{0}/{1}", itemDataCountByid, num));
			if (freeTimes > 0 || itemDataCountByid >= (long)num)
			{
				this.textCost.color = Color.white;
			}
			else
			{
				this.textCost.color = Color.red;
			}
			this.imageRateBg.sprite = ((this.currentRate == maxRate) ? this.spriteRegister.GetSprite("purple") : this.spriteRegister.GetSprite("yellow"));
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.costRT);
		}

		private int GetMaxRate()
		{
			int num = (int)(this.propDataModule.GetItemDataCountByid((ulong)((long)GameConfig.Mining_Draw_ItemId)) / (long)GameConfig.Mining_Draw_Cost);
			if (num > GameConfig.Mining_Max_Rate)
			{
				num = GameConfig.Mining_Max_Rate;
			}
			return num;
		}

		private void PlayBonusAnimation()
		{
			GameApp.Sound.PlayBGM(640, 1f);
			this.buttonBuy.SetLock(true);
			this.infoObj.SetActiveSafe(false);
			this.freeTimesObj.SetActiveSafe(false);
			this.clickMask.SetActiveSafe(true);
			this.RefreshBonusProgress(0, this.freeProgressLimit);
			for (int i = 0; i < this.showRewardDataList.Count; i++)
			{
				UIMinerItem uiminerItem;
				if (i < this.minerItems.Count)
				{
					uiminerItem = this.minerItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.minerParent, false);
					uiminerItem = gameObject.GetComponent<UIMinerItem>();
					uiminerItem.Init();
					this.minerItems.Add(uiminerItem);
				}
				uiminerItem.gameObject.SetActive(true);
				uiminerItem.SetData(this.showRewardDataList[i], i, this.getRewardDataList.Contains(this.showRewardDataList[i]));
			}
			this.rewardNodeCtrl.SetData(this.getRewardDataList);
			this.finishCount = 0;
			this.successCount = 0;
			this.sequencePool.Get();
			for (int j = 0; j < this.minerItems.Count; j++)
			{
				this.minerItems[j].PlayAni(this.moveFailTrans.anchoredPosition.x, this.MoveEffectTrans.anchoredPosition.x, new Action<int, bool>(this.MinerFinish));
			}
		}

		private void MinerFinish(int index, bool isSuccess)
		{
			if (isSuccess)
			{
				this.successCount++;
				GameApp.Sound.PlayClip(643, 1f);
				GameObject obj = Object.Instantiate<GameObject>(this.copyFxReward);
				obj.SetParentNormal(this.copyFxReward.transform.parent, false);
				obj.transform.position = this.copyFxReward.transform.position;
				obj.SetActiveSafe(true);
				DelayCall.Instance.CallOnce(1000, delegate
				{
					if (obj)
					{
						Object.Destroy(obj);
					}
				});
				this.rewardNodeCtrl.ShowReward();
				this.RefreshBonusProgress(this.successCount, this.freeProgressLimit);
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.progressObj.transform, 1.2f, 0.15f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.progressAniObj.SetActiveSafe(true);
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.progressObj.transform, 1f, 0.05f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.progressAniObj.SetActiveSafe(false);
				});
				if (this.successCount == this.freeProgressLimit && this.miningDataModule.MiningDraw.FreeTimes > 0)
				{
					if (this.fxSuccess)
					{
						GameApp.Sound.PlayClip(644, 1f);
						this.fxSuccess.gameObject.SetActiveSafe(true);
						this.fxSuccess.Play();
					}
					this.textFreeTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_tips_2", new object[] { this.freeProgressLimit });
					this.freeTimesObj.transform.localScale = Vector3.zero;
					this.freeTimesObj.SetActiveSafe(true);
					Sequence sequence2 = this.sequencePool.Get();
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence2, ShortcutExtensions.DOScale(this.freeTimesObj.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.freeTimesObj.transform, 1f, 0.05f));
					TweenSettingsExtensions.AppendInterval(sequence2, 2f);
					TweenSettingsExtensions.AppendCallback(sequence2, delegate
					{
						this.freeTimesObj.SetActiveSafe(false);
						if (this.fxSuccess)
						{
							this.fxSuccess.gameObject.SetActiveSafe(false);
						}
					});
				}
			}
			else
			{
				GameApp.Sound.PlayClip(641, 1f);
				this.tanHuangItem.PlayAnimation("Play", false);
				this.tanHuangItem.AddAnimation("Idle_01", true, 0f);
			}
			this.finishCount++;
			if (this.finishCount == this.showRewardDataList.Count)
			{
				GameApp.Sound.PlayBGM(1, 1f);
				if (this.successCount < this.freeProgressLimit)
				{
					this.endTipObj.SetActiveSafe(true);
					this.endTipObj.transform.localScale = Vector3.zero;
					Sequence sequence3 = this.sequencePool.Get();
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence3, ShortcutExtensions.DOScale(this.endTipObj.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.endTipObj.transform, 1f, 0.05f));
					TweenSettingsExtensions.AppendInterval(sequence3, 1f);
					TweenSettingsExtensions.AppendCallback(sequence3, delegate
					{
						this.endTipObj.SetActiveSafe(false);
						if (this.Reward != null && this.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(this.Reward, new Action(this.ResetBonus), false);
							return;
						}
						this.ResetBonus();
					});
					return;
				}
				this.Refresh();
				Sequence sequence4 = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence4, 1f);
				TweenSettingsExtensions.AppendCallback(sequence4, delegate
				{
					DxxTools.UI.OpenRewardCommon(this.Reward, delegate
					{
						if (this.miningDataModule.MiningDraw.FreeTimes == 0)
						{
							this.ResetBonus();
							return;
						}
						this.RefreshBonusProgress(0, this.freeProgressLimit + 1);
						this.rewardNodeCtrl.ResetReward();
						this.infoObj.SetActiveSafe(true);
						this.buttonBuy.SetLock(false);
						this.clickMask.SetActiveSafe(false);
					}, false);
				});
			}
		}

		private void ResetBonus()
		{
			this.buttonBuy.SetLock(false);
			this.clickMask.SetActiveSafe(false);
			this.allRewardDataList.Clear();
			this.rewardNodeCtrl.ResetReward();
			this.infoObj.SetActiveSafe(true);
			this.RefreshBonusProgress(0, this.miningDataModule.MiningDraw.Progress);
			for (int i = 0; i < this.minerItems.Count; i++)
			{
				this.minerItems[i].gameObject.SetActiveSafe(false);
			}
			this.Refresh();
		}

		private void PlayerOpenAni()
		{
			this.titleRT.anchoredPosition = new Vector2(this.titleRT.anchoredPosition.x, 500f);
			this.bottomRT.anchoredPosition = new Vector2(this.bottomRT.anchoredPosition.x, -500f);
			float num = 0.2f;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.titleRT, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.bottomRT, 0f, num, false));
		}

		private void OnClickBuy()
		{
			if (this.currentRate <= 0)
			{
				return;
			}
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)GameConfig.Mining_Draw_ItemId));
			int num = GameConfig.Mining_Draw_Cost * this.currentRate;
			if (this.miningDataModule.MiningDraw.FreeTimes > 0 || itemDataCountByid >= (long)num)
			{
				this.freeProgressLimit = this.miningDataModule.MiningDraw.Progress;
				NetworkUtils.Mining.DoBonusDrawRequest(this.currentRate, delegate(bool result, BounDrawResponse response)
				{
					if (result && response != null)
					{
						this.Reward = response.CommonData.Reward;
						this.getRewardDataList.Clear();
						this.showRewardDataList = response.ShowRewards.ToItemDatas();
						for (int i = 0; i < response.RewardsIndex.Count; i++)
						{
							int num2 = response.RewardsIndex[i];
							if (num2 < this.showRewardDataList.Count)
							{
								this.getRewardDataList.Add(this.showRewardDataList[num2]);
								this.allRewardDataList.Add(this.showRewardDataList[num2]);
							}
						}
						this.PlayBonusAnimation();
					}
				});
				return;
			}
			GameApp.View.ShowItemNotEnoughTip(GameConfig.Mining_Draw_ItemId, true);
		}

		private void OnClickHelp()
		{
			GameApp.View.OpenView(ViewName.MiningBonusRateViewModule, null, 1, null, null);
		}

		private void OnClickBack()
		{
			GameApp.View.CloseView(ViewName.MiningBonusViewModule, null);
		}

		private void OnClickRate()
		{
			int maxRate = this.GetMaxRate();
			if (this.currentRate < maxRate)
			{
				this.currentRate++;
			}
			else
			{
				this.currentRate = 1;
			}
			this.RefreshRate();
			this.rateTipCtr.SetShow(true);
		}

		[GameTestMethod("挖矿", "幸运矿车", "", 502)]
		private static void OpenMiningBonus()
		{
			GameApp.View.OpenView(ViewName.MiningBonusViewModule, null, 1, null, null);
		}

		[Header("顶部主角和提示")]
		public UIModelItem modelItem;

		public GameObject tipsObj;

		public CustomText textTips;

		[Header("矿工和提示")]
		public GameObject freeTimesObj;

		public CustomText textFreeTime;

		public GameObject minerParent;

		public GameObject copyItem;

		public RectTransform MoveEffectTrans;

		public RectTransform moveFailTrans;

		public GameObject endTipObj;

		public ParticleSystem fxSuccess;

		public UISpineModelItem tanHuangItem;

		public GameObject copyFxReward;

		[Header("奖励栏")]
		public GameObject progressObj;

		public CustomText textBonus;

		public GameObject progressAniObj;

		public GameObject infoObj;

		public CustomText textInfo;

		public CustomButton buttonHelp;

		public UIMiningBonusRewardCtrl rewardNodeCtrl;

		[Header("底部功能栏")]
		public CustomButton buttonBack;

		public UIOneButtonCtrl buttonBuy;

		public CustomImage imageCost;

		public CustomText textCost;

		public CustomButton buttonRate;

		public CustomText textRate;

		public GameObject rateObj;

		public UISweepTipCtrl rateTipCtr;

		public CustomImage imageRateBg;

		public SpriteRegister spriteRegister;

		public RectTransform costRT;

		[Header("其他")]
		public GameObject clickMask;

		public RectTransform titleRT;

		public RectTransform bottomRT;

		private MiningDataModule miningDataModule;

		private PropDataModule propDataModule;

		private int currentRate;

		private int successCount;

		private int finishCount;

		private int freeProgressLimit;

		private List<ItemData> showRewardDataList = new List<ItemData>();

		private List<ItemData> getRewardDataList = new List<ItemData>();

		private List<ItemData> allRewardDataList = new List<ItemData>();

		private List<UIMinerItem> minerItems = new List<UIMinerItem>();

		private RepeatedField<RewardDto> Reward;

		private SequencePool sequencePool = new SequencePool();
	}
}
