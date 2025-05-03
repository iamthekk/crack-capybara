using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIBonusGameButtonCtrl : CustomBehaviour
	{
		public bool IsShowBigWin { get; private set; }

		public bool IsShowMinorWin { get; private set; }

		public bool IsShowBonus
		{
			get
			{
				return this.IsShowBigWin || this.IsShowMinorWin;
			}
		}

		protected override void OnInit()
		{
			this.clickMask.SetActiveSafe(false);
			this.effectObj.SetActiveSafe(false);
			this.buttonBigWin.onClick.AddListener(new UnityAction(this.OnClickBigWin));
			this.buttonMinorWin.onClick.AddListener(new UnityAction(this.OnClickMinorWin));
			this.effectItem.Init();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSingleSlot, new HandlerEvent(this.OnEventCloseSingleSlot));
		}

		protected override void OnDeInit()
		{
			this.buttonBigWin.onClick.RemoveListener(new UnityAction(this.OnClickBigWin));
			this.buttonMinorWin.onClick.RemoveListener(new UnityAction(this.OnClickMinorWin));
			this.effectItem.DeInit();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSingleSlot, new HandlerEvent(this.OnEventCloseSingleSlot));
			this.sequencePool.Clear(false);
		}

		public void SetData(int bigWinLimit, int freePlayNum, int minorWinLimit, int minorWinGameId, int seed, int oldBgm, Action onCloseBonus)
		{
			this.BigWinLimit = bigWinLimit;
			this.BigWinGameFreeNum = freePlayNum;
			this.MinorWinLimit = minorWinLimit;
			this.MinorWinGameId = minorWinGameId;
			this.OldBGM = oldBgm;
			this.OnCloseBonus = onCloseBonus;
			this.xRandom = new XRandom(seed);
			this.GetBigWinNum = this.GetBigWinProgress();
			this.GetMinorWinNum = this.GetMinerWinProgress();
			this.textBigProgress.text = string.Format("{0}/{1}", this.GetBigWinNum, this.BigWinLimit);
			this.textMinorProgress.text = string.Format("{0}/{1}", this.GetMinorWinNum, this.MinorWinLimit);
			if (bigWinLimit == 0)
			{
				this.buttonBigWin.gameObject.SetActiveSafe(false);
			}
			if (minorWinLimit == 0)
			{
				this.buttonMinorWin.gameObject.SetActiveSafe(false);
			}
			if (this.GetBigWinNum >= this.BigWinLimit)
			{
				this.DoGetBigWin();
				return;
			}
			if (this.GetMinorWinNum >= this.MinorWinLimit)
			{
				this.DoGetMinorWin();
			}
		}

		public void DoGetBigWin()
		{
			int getBigWinNum = this.GetBigWinNum;
			this.GetBigWinNum = this.GetBigWinProgress();
			this.textBigProgress.text = string.Format("{0}/{1}", this.GetBigWinNum, this.BigWinLimit);
			if (getBigWinNum < this.GetBigWinNum)
			{
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(this.sequencePool.Get(), ShortcutExtensions.DOScale(this.textBigProgress.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.textBigProgress.transform, 1f, 0.05f));
			}
			if (this.GetBigWinNum > 0 && this.BigWinLimit > 0 && this.GetBigWinNum >= this.BigWinLimit)
			{
				this.IsShowBigWin = true;
				int seed = this.xRandom.NextInt();
				this.clickMask.SetActiveSafe(true);
				GameApp.Sound.PlayClip(123, 1f);
				float num = this.ShowBonusEffect();
				DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
				{
					this.ShowBigWin(seed, true);
					Singleton<GameEventController>.Instance.PlayerData.PlayBigBonusGame();
					this.effectObj.SetActiveSafe(false);
				});
			}
		}

		public void DoGetMinorWin()
		{
			int getMinorWinNum = this.GetMinorWinNum;
			this.GetMinorWinNum = this.GetMinerWinProgress();
			this.textMinorProgress.text = string.Format("{0}/{1}", this.GetMinorWinNum, this.MinorWinLimit);
			if (getMinorWinNum < this.GetMinorWinNum)
			{
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(this.sequencePool.Get(), ShortcutExtensions.DOScale(this.textMinorProgress.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.textMinorProgress.transform, 1f, 0.05f));
			}
			if (this.GetMinorWinNum > 0 && this.MinorWinLimit > 0 && this.GetMinorWinNum >= this.MinorWinLimit)
			{
				this.IsShowMinorWin = true;
				int seed = this.xRandom.NextInt();
				this.clickMask.SetActiveSafe(true);
				GameApp.Sound.PlayClip(123, 1f);
				float num = this.ShowBonusEffect();
				DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
				{
					this.ShowMinorWin(seed);
					Singleton<GameEventController>.Instance.PlayerData.PlayMinorBonusGame();
					this.clickMask.SetActiveSafe(false);
					this.RefreshProgress();
					this.effectObj.SetActiveSafe(false);
				});
			}
		}

		private void OnEventCloseSingleSlot(object sender, int type, BaseEventArgs eventArgs)
		{
			this.clickMask.SetActiveSafe(false);
			this.RefreshProgress();
		}

		private float ShowBonusEffect()
		{
			this.effectObj.SetActiveSafe(true);
			string text = "Birth";
			float animationDuration = this.effectItem.GetAnimationDuration(text);
			this.textAni.transform.localScale = Vector3.zero;
			this.textCanvasGroup.alpha = 1f;
			this.effectItem.PlayAnimation(text, false);
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textAni.transform.localScale = Vector3.one * 3f;
			});
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textAni, Vector3.one * 0.9f, 0.1f)), ShortcutExtensions.DOScale(this.textAni, Vector3.one, 0.05f));
			TweenSettingsExtensions.AppendInterval(sequence, 0.7f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textAni, Vector3.one * 0.7f, 0.1f));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.textCanvasGroup, 0f, 0.1f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textAni.transform.localScale = Vector3.zero;
			});
			return animationDuration;
		}

		private void OnClickBigWin()
		{
			this.ShowBigWin(0, false);
		}

		private void OnClickMinorWin()
		{
			this.ShowMinorWin(0);
		}

		private void ShowBigWin(int seed, bool isChangeBgm)
		{
			SingleSlotViewModule.OpenData openData = new SingleSlotViewModule.OpenData();
			openData.randSeed = seed;
			openData.currentWin = this.GetBigWinNum;
			openData.needWin = this.BigWinLimit;
			openData.freePlayNum = this.BigWinGameFreeNum;
			openData.isChangeBgm = isChangeBgm;
			openData.oldBgm = this.OldBGM;
			openData.onCloseUI = new Action(this.OnCloseBonusUI);
			GameApp.View.OpenView(ViewName.SingleSlotViewModule, openData, 1, null, null);
		}

		private void ShowMinorWin(int seed)
		{
			FortuneWheelViewModule.OpenData openData = new FortuneWheelViewModule.OpenData();
			openData.turntableId = this.MinorWinGameId;
			openData.randSeed = seed;
			openData.rewardRate = 1;
			openData.isBonusGame = true;
			openData.currentMinor = this.GetMinorWinNum;
			openData.needMinor = this.MinorWinLimit;
			openData.onCloseUI = new Action(this.OnCloseBonusUI);
			GameApp.View.OpenView(ViewName.FortuneWheelViewModule, openData, 1, null, null);
		}

		public Transform GetBigWinFlyPosition()
		{
			return this.bigFlyPos;
		}

		public Transform GetMinorWinFlyPosition()
		{
			return this.smallFlyPos;
		}

		public void ShowButtons()
		{
			if (this.isShowBtn)
			{
				return;
			}
			this.isShowBtn = true;
			if (this.animator)
			{
				this.animator.Play("show");
			}
		}

		public void HideButtons()
		{
			if (!this.isShowBtn)
			{
				return;
			}
			this.isShowBtn = false;
			if (this.animator)
			{
				this.animator.Play("hide");
			}
		}

		private void RefreshProgress()
		{
			this.GetBigWinNum = this.GetBigWinProgress();
			this.GetMinorWinNum = this.GetMinerWinProgress();
			this.textBigProgress.text = string.Format("{0}/{1}", this.GetBigWinNum, this.BigWinLimit);
			this.textMinorProgress.text = string.Format("{0}/{1}", this.GetMinorWinNum, this.MinorWinLimit);
		}

		private int GetBigWinProgress()
		{
			int num = Singleton<GameEventController>.Instance.GetEventSizeTypeNum(EventSizeType.BigWin) - Singleton<GameEventController>.Instance.PlayerData.PlayBigBonusCount * this.BigWinLimit;
			return (num < 0) ? 0 : num;
		}

		private int GetMinerWinProgress()
		{
			int num = Singleton<GameEventController>.Instance.GetEventSizeTypeNum(EventSizeType.MinorWin) - Singleton<GameEventController>.Instance.PlayerData.PlayMinorBonusCount * this.MinorWinLimit;
			return (num < 0) ? 0 : num;
		}

		private void OnCloseBonusUI()
		{
			this.IsShowBigWin = false;
			this.IsShowMinorWin = false;
			Action onCloseBonus = this.OnCloseBonus;
			if (onCloseBonus == null)
			{
				return;
			}
			onCloseBonus();
		}

		[GameTestMethod("测试", "测试动画", "", 410)]
		private static void OpenGameEventDemon()
		{
			GameApp.View.GetViewModule(ViewName.GameEventViewModule).eventController.bonusCtrl.ShowBonusEffect();
		}

		public CustomButton buttonBigWin;

		public CustomButton buttonMinorWin;

		public CustomText textBigProgress;

		public CustomText textMinorProgress;

		public Transform bigFlyPos;

		public Transform smallFlyPos;

		public Animator animator;

		public GameObject clickMask;

		public GameObject effectObj;

		public UISpineModelItem effectItem;

		public Transform textAni;

		public CanvasGroup textCanvasGroup;

		private int BigWinLimit;

		private int BigWinGameFreeNum;

		private int MinorWinLimit;

		private int MinorWinGameId;

		private int GetBigWinNum;

		private int GetMinorWinNum;

		private int OldBGM;

		private XRandom xRandom;

		private bool isShowBtn = true;

		private SequencePool sequencePool = new SequencePool();

		private Action OnCloseBonus;
	}
}
