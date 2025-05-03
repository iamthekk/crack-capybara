using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Chapter;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterActivityWheelViewModule : BaseViewModule
	{
		private int RealCostScore
		{
			get
			{
				return this.costScoreBase * this.selectRate;
			}
		}

		public override void OnCreate(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			this.btnStartSpin.Init();
			this.wheelSymbolItem.gameObject.SetActive(false);
			this.pointerCollider.OnPointerColliderEnter = new Action(this.OnPointerCollide);
			this.progressCtrl.Init();
			this.freeTimeObj.SetActiveSafe(false);
			this.fxBig.gameObject.SetActiveSafe(false);
			this.fxSmall.gameObject.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			if (this.dataModule.WheelInfo == null)
			{
				this.OnClickBack();
				return;
			}
			this.turnBaseCfg = GameApp.Table.GetManager().GetChapterActivity_ActvTurntableBase(this.dataModule.WheelInfo.ActiveId);
			if (this.turnBaseCfg == null)
			{
				this.OnClickBack();
				return;
			}
			this.isSpinning = false;
			this.isActChange = false;
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickBack));
			this.buttonRateSub.onClick.AddListener(new UnityAction(this.OnClickRateSub));
			this.buttonRateAdd.onClick.AddListener(new UnityAction(this.OnClickRateAdd));
			this.buttonRateMax.onClick.AddListener(new UnityAction(this.OnClickRateMax));
			this.btnStartSpin.button.onClick.AddListener(new UnityAction(this.OnBtnStartSpinClick));
			this.btnStartSpin.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("fortune_wheel_rotate"));
			this.btnStartSpin.SetLock(false);
			this.costObj.SetActiveSafe(true);
			this.progressCtrl.SetLock(false);
			this.fxBig.gameObject.SetActiveSafe(false);
			this.fxSmall.gameObject.SetActiveSafe(false);
			this.rateList = GameConfig.Chapter_Activity_Wheel_Rates;
			this.costScoreBase = this.turnBaseCfg.cost;
			this.selectRate = this.dataModule.WheelInfo.FreeRate;
			if (this.selectRate < 1)
			{
				this.selectRate = 1;
			}
			else
			{
				int num = this.selectRate;
				List<int> list = this.rateList;
				if (num > list[list.Count - 1])
				{
					List<int> list2 = this.rateList;
					this.selectRate = list2[list2.Count - 1];
				}
			}
			GameApp.Sound.PlayClip(260018, 1f);
			this.EnableNailsCollider(false);
			this.InitWheel();
			this.Refresh();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.sweepTipCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.dataModule == null)
			{
				return;
			}
			if (this.dataModule.WheelInfo == null)
			{
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.dataModule.WheelInfo.EndTime - serverTimestamp;
			if (num > 0L)
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(num);
				return;
			}
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
		}

		public override void OnClose()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickBack));
			this.buttonRateSub.onClick.RemoveListener(new UnityAction(this.OnClickRateSub));
			this.buttonRateAdd.onClick.RemoveListener(new UnityAction(this.OnClickRateAdd));
			this.buttonRateMax.onClick.RemoveListener(new UnityAction(this.OnClickRateMax));
			this.btnStartSpin.button.onClick.RemoveListener(new UnityAction(this.OnBtnStartSpinClick));
		}

		public override void OnDelete()
		{
			this.pointerCollider.Clear();
			this.progressCtrl.DeInit();
			this.sequencePool.Clear(false);
			this.itemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(484, new HandlerEvent(this.OnEventActChange));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(484, new HandlerEvent(this.OnEventActChange));
		}

		private void Refresh()
		{
			this.RefreshProgress();
			this.RefreshRate();
		}

		private void RefreshProgress()
		{
			this.progressCtrl.SetData(this.dataModule.WheelInfo.RewardScore, this.dataModule.uiDataList, new Action(this.OnProgressAniFinish));
		}

		private void RefreshRate()
		{
			if (this.dataModule.WheelInfo == null)
			{
				return;
			}
			int num = this.selectRate;
			List<int> list = this.rateList;
			bool flag = num == list[list.Count - 1];
			this.textHave.text = this.dataModule.WheelInfo.Score.ToString();
			this.textRate.text = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_rate", new object[] { this.selectRate });
			this.textCost.text = ((this.dataModule.WheelInfo.FreeNum > 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_free") : string.Format("-{0}", this.RealCostScore));
			this.sweepTipCtrl.SetData(this.selectRate, flag);
			if (this.dataModule.WheelInfo.Score < this.RealCostScore && this.dataModule.WheelInfo.FreeNum <= 0)
			{
				this.textCost.color = Color.red;
			}
			else
			{
				this.textCost.color = Color.white;
			}
			int num2 = this.rateList.IndexOf(this.selectRate);
			if (num2 == 0)
			{
				this.grayRateSub.SetUIGray();
				this.grayRateAdd.Recovery();
				this.grayRateMax.Recovery();
			}
			else if (num2 == this.rateList.Count - 1)
			{
				this.grayRateSub.Recovery();
				this.grayRateAdd.SetUIGray();
				this.grayRateMax.SetUIGray();
			}
			else
			{
				this.grayRateSub.Recovery();
				this.grayRateAdd.Recovery();
				this.grayRateMax.Recovery();
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].RefreshRate(this.selectRate);
			}
		}

		private void EnableNailsCollider(bool enable)
		{
			Collider2D[] componentsInChildren = this.nodeNails.GetComponentsInChildren<Collider2D>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = enable;
				}
			}
		}

		private void OnBtnStartSpinClick()
		{
			if (this.isSpinning)
			{
				return;
			}
			this.btnStartSpin.SetLock(false);
			this.costObj.SetActiveSafe(true);
			if (this.dataModule.WheelInfo.FreeNum == 0 && this.dataModule.WheelInfo.Score < this.RealCostScore)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("activity_no_score");
				ChapterDataModule chapterDataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
				int previousId = chapterDataModule.GetPreviousChapterID();
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Sweep, false) && previousId > 0)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_goto");
					string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_cancel");
					DxxTools.UI.OpenPopCommon(infoByID, delegate(int result)
					{
						if (result == 1)
						{
							this.OnClickBack();
							ChapterSweepViewModule.OpenData openData = new ChapterSweepViewModule.OpenData();
							openData.chapterId = previousId;
							openData.isRecord = false;
							GameApp.View.OpenView(ViewName.ChapterSweepViewModule, openData, 1, null, null);
						}
					}, "", infoByID2, infoByID3, true, 2);
					return;
				}
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			else
			{
				if (this.dataModule.IsAllFinish())
				{
					string infoByID4 = Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_finish");
					GameApp.View.ShowStringTip(infoByID4);
					return;
				}
				this.oldRewardScore = this.dataModule.WheelInfo.RewardScore;
				NetworkUtils.Chapter.DoChapterWheelSpineRequest(this.selectRate, delegate(bool isOk, ChapterWheelSpineResponse resp)
				{
					if (isOk)
					{
						for (int i = 0; i < this.dataList.Count; i++)
						{
							if ((long)this.dataList[i].cfg.id == resp.RewardId)
							{
								this.randResultIndex = i;
								break;
							}
						}
						this.newRewardScore = resp.RewardScore;
						this.showRewards = resp.CommonData.Reward;
						this.RefreshRate();
						this.EnablePointer(true);
						this.btnStartSpin.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_spinning"));
						this.btnStartSpin.SetLock(true);
						this.costObj.SetActiveSafe(false);
						this.isSpinning = true;
						this.progressCtrl.SetLock(this.isSpinning);
						this.isWaitSendReward = true;
						this.CalcResult();
						this.EnableNailsCollider(true);
						this.wheelAnimator.Play("SpinStart", 0, 0f);
						float animationLength = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, "SpinStart");
						base.StartCoroutine(this.ActiveWheelAutoSpin(animationLength));
					}
				});
				return;
			}
		}

		private IEnumerator ActiveWheelAutoSpin(float duration)
		{
			yield return new WaitForSeconds(duration);
			float timer = 0f;
			this.wheelAnimator.Play("SpinLoop", 0, 0f);
			this.wheelRect.transform.localRotation = Quaternion.Euler(0f, 0f, this.randAngle);
			float loopDuration = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, "SpinLoop");
			float randomTime = Random.Range(0.5f, 1f);
			yield return new WaitForSeconds(loopDuration);
			timer += loopDuration;
			while (timer < randomTime)
			{
				timer += Time.deltaTime;
				yield return 0;
			}
			this.endAnimName = ChapterActivityWheelViewModule.EndAnimationsNames;
			this.wheelAnimator.Play(this.endAnimName, 0, 0f);
			float animationLength = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, this.endAnimName);
			yield return new WaitForSeconds(animationLength);
			this.EnableNailsCollider(false);
			this.EnablePointer(false);
			if (this.isWaitSendReward)
			{
				GameApp.Sound.PlayClip(260020, 1f);
				this.btnStartSpin.SetLock(false);
				this.costObj.SetActiveSafe(true);
				this.SendReward();
			}
			yield break;
		}

		private void SendReward()
		{
			this.isWaitSendReward = false;
			this.EnablePointer(false);
			Sequence sequence = this.sequencePool.Get();
			if (this.randResultIndex < this.dataList.Count && this.dataList[this.randResultIndex].cfg.again > 0)
			{
				GameApp.Sound.PlayClip(644, 1f);
				this.fxBig.gameObject.SetActiveSafe(true);
				this.fxBig.Play();
				this.freeTimeObj.transform.localScale = Vector3.zero;
				this.freeTimeObj.SetActiveSafe(true);
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.freeTimeObj.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.freeTimeObj.transform, 1f, 0.05f));
				TweenSettingsExtensions.AppendInterval(sequence, 2f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (base.gameObject != null)
					{
						this.freeTimeObj.SetActiveSafe(false);
						this.fxBig.gameObject.SetActiveSafe(false);
						this.progressCtrl.PlayAni(this.oldRewardScore, this.newRewardScore);
					}
				});
				return;
			}
			GameApp.Sound.PlayClip(643, 1f);
			this.fxSmall.gameObject.SetActiveSafe(true);
			this.fxSmall.Play();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (base.gameObject != null)
				{
					this.progressCtrl.PlayAni(this.oldRewardScore, this.newRewardScore);
				}
			});
		}

		private void OnProgressAniFinish()
		{
			this.isSpinning = false;
			this.progressCtrl.SetLock(this.isSpinning);
			if (base.gameObject.activeSelf && this.showRewards != null && this.showRewards.Count > 0)
			{
				DxxTools.UI.OpenRewardCommon(this.showRewards, new Action(this.CloseReward), true);
			}
		}

		private void CalcResult()
		{
			this.rewardData = null;
			this.rewardData = this.dataList[this.randResultIndex];
			float num = this.rewardData.offsetAngle + 1f;
			float num2 = this.rewardData.offsetAngle + this.dataList[this.randResultIndex].areaAngle - 1f;
			this.randAngle = Random.Range(num, num2);
		}

		private void InitWheel()
		{
			this.weightList.Clear();
			this.EnablePointer(false);
			this.defaultWheelOffsetAngle = this.turnBaseCfg.offsetAngle;
			this.wheelRect.transform.localRotation = Quaternion.Euler(0f, 0f, this.defaultWheelOffsetAngle);
			List<int> list = new List<int>(this.turnBaseCfg.rewards);
			this.dataList = new List<TurntableSymbolData>();
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				ChapterActivity_ActvTurntableDetail chapterActivity_ActvTurntableDetail = GameApp.Table.GetManager().GetChapterActivity_ActvTurntableDetail(list[i]);
				num += (float)chapterActivity_ActvTurntableDetail.weight;
				this.weightList.Add(chapterActivity_ActvTurntableDetail.weight);
				num2 += (float)chapterActivity_ActvTurntableDetail.showWeight;
				TurntableSymbolData turntableSymbolData = new TurntableSymbolData(chapterActivity_ActvTurntableDetail, i);
				this.dataList.Add(turntableSymbolData);
			}
			if (this.dataList.Count != this.itemList.Count)
			{
				for (int j = 0; j < this.itemList.Count; j++)
				{
					this.itemList[j].gameObject.SetActiveSafe(false);
				}
			}
			float num3 = 0f;
			for (int k = 0; k < this.dataList.Count; k++)
			{
				TurntableSymbolData turntableSymbolData2 = this.dataList[k];
				turntableSymbolData2.Calc(num, num2, num3);
				UIWheelSymbolItem uiwheelSymbolItem;
				if (k < this.itemList.Count)
				{
					uiwheelSymbolItem = this.itemList[k];
				}
				else
				{
					uiwheelSymbolItem = Object.Instantiate<UIWheelSymbolItem>(this.wheelSymbolItem, this.wheelSymbolItem.transform.parent).GetComponent<UIWheelSymbolItem>();
				}
				uiwheelSymbolItem.gameObject.SetActive(true);
				Transform transform = uiwheelSymbolItem.transform;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.Euler(0f, 0f, -num3);
				uiwheelSymbolItem.SetData(turntableSymbolData2);
				this.itemList.Add(uiwheelSymbolItem);
				num3 += turntableSymbolData2.areaAngle;
			}
		}

		private void EnablePointer(bool enable)
		{
			this.pointerCollider.SetColliderEnable(enable);
		}

		private void OnPointerCollide()
		{
			GameApp.Sound.PlayClip(260019, 1f);
		}

		private void OnClickBack()
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.isSpinning)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.ChapterActivityWheelViewModule, null);
		}

		private void OnClickRateSub()
		{
			if (this.isSpinning)
			{
				return;
			}
			if (this.IsFree(true))
			{
				return;
			}
			int num = this.rateList.IndexOf(this.selectRate);
			num--;
			if (num < 0)
			{
				num = 0;
			}
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnClickRateAdd()
		{
			if (this.isSpinning)
			{
				return;
			}
			if (this.IsFree(true))
			{
				return;
			}
			int num = this.rateList.IndexOf(this.selectRate);
			num++;
			if (num >= this.rateList.Count)
			{
				num = this.rateList.Count - 1;
			}
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnClickRateMax()
		{
			if (this.isSpinning)
			{
				return;
			}
			if (this.IsFree(true))
			{
				return;
			}
			int num = this.rateList.Count - 1;
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnEventActChange(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.isSpinning)
			{
				this.isActChange = true;
				return;
			}
			this.ActChange();
		}

		private void ActChange()
		{
			this.OnClickBack();
			if (GameApp.View.IsOpened(ViewName.ChapterActivityWheelPreviewViewModule))
			{
				GameApp.View.CloseView(ViewName.ChapterActivityWheelPreviewViewModule, null);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_change");
			GameApp.View.ShowStringTip(infoByID);
		}

		private void CloseReward()
		{
			if (this.isActChange)
			{
				this.ActChange();
			}
		}

		private bool IsFree(bool isShowTip)
		{
			if (this.dataModule.WheelInfo != null && this.dataModule.WheelInfo.FreeNum > 0)
			{
				if (isShowTip)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_free_tip"));
				}
				return true;
			}
			return false;
		}

		[Header("转盘")]
		public UIWheelSymbolItem wheelSymbolItem;

		public Animator wheelAnimator;

		public RectTransform wheelRect;

		public Transform nodeNails;

		public PointerCollider pointerCollider;

		[Header("功能")]
		public UIWheelProgressCtrl progressCtrl;

		public UIOneButtonCtrl btnStartSpin;

		public CustomButton buttonClose;

		public UISweepTipCtrl sweepTipCtrl;

		public CustomButton buttonRateSub;

		public CustomButton buttonRateAdd;

		public CustomButton buttonRateMax;

		public UIGray grayRateSub;

		public UIGray grayRateAdd;

		public UIGray grayRateMax;

		public CustomText textCost;

		public CustomText textRate;

		public CustomText textHave;

		public GameObject costObj;

		public CustomText textTime;

		[Header("特效")]
		public GameObject freeTimeObj;

		public ParticleSystem fxBig;

		public ParticleSystem fxSmall;

		private static readonly string EndAnimationsNames = "SpinEnd1";

		private float defaultWheelOffsetAngle;

		private bool isSpinning;

		private bool isWaitSendReward;

		private string endAnimName;

		private ChapterActivityWheelDataModule dataModule;

		private ChapterActivity_ActvTurntableBase turnBaseCfg;

		private List<int> rateList;

		private int selectRate = 1;

		private int costScoreBase;

		private RepeatedField<RewardDto> showRewards;

		private int oldRewardScore;

		private int newRewardScore;

		private bool isActChange;

		private SequencePool sequencePool = new SequencePool();

		private TurntableSymbolData rewardData;

		private float randAngle;

		private int randResultIndex;

		private List<TurntableSymbolData> dataList = new List<TurntableSymbolData>();

		private List<UIWheelSymbolItem> itemList = new List<UIWheelSymbolItem>();

		private List<int> weightList = new List<int>();
	}
}
