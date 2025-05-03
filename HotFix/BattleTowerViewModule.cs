using System;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleTowerViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_BtnJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
			this.start.SetActiveSafe(false);
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
		}

		public override void OnOpen(object data)
		{
			this.buttonTrans.anchoredPosition = new Vector2(500f, this.buttonTrans.anchoredPosition.y);
			this.Refresh();
			this.m_BtnSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvE);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.m_BtnJump.onClick.RemoveListener(new UnityAction(this.OnClickBtnJump));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnBattleStart));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnBattleStart));
		}

		private void Refresh()
		{
			TowerChallenge_Tower curTowerConfig = this.towerDataModule.CurTowerConfig;
			int num = this.towerDataModule.FightLevelIndex + 1;
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(curTowerConfig.name);
			this.textTowerName.text = infoByID;
			this.textStartName.text = infoByID;
			this.textTowerStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_stage", new object[] { num });
			this.textStartStage.text = this.textTowerStage.text;
			this.textRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitowerbattle_round", new object[] { 0, 15 });
		}

		public void PlayOpenAni(Action aniFinish)
		{
			if (this.animator)
			{
				float num = 0f;
				AnimationClip animationClip = this.animator.runtimeAnimatorController.animationClips.ToList<AnimationClip>().FirstOrDefault((AnimationClip ani) => ani.name == "UIMainTower_Show");
				if (animationClip != null)
				{
					num = animationClip.length;
				}
				this.animator.Play("UIMainTower_Show", 0, 0f);
				DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
				{
					Action aniFinish3 = aniFinish;
					if (aniFinish3 == null)
					{
						return;
					}
					aniFinish3();
				});
				return;
			}
			Action aniFinish2 = aniFinish;
			if (aniFinish2 == null)
			{
				return;
			}
			aniFinish2();
		}

		private void ShowButtons()
		{
			this.buttonTrans.anchoredPosition = new Vector2(500f, this.buttonTrans.anchoredPosition.y);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions46.DOAnchorPosX(this.buttonTrans, -10f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.buttonTrans, 0f, 0.1f, false));
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		private void OnRoundStartHandler(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			this.textRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitowerbattle_round", new object[] { eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound });
		}

		private void OnBattleStart(object sender, int type, BaseEventArgs args)
		{
			this.ShowButtons();
		}

		[SerializeField]
		private CustomButton m_BtnJump;

		[SerializeField]
		private UISpeedButtonCtrl m_BtnSpeedUp;

		[SerializeField]
		private CustomText textTowerName;

		[SerializeField]
		private CustomText textTowerStage;

		[SerializeField]
		private CustomText textRound;

		[SerializeField]
		private CustomText textStartName;

		[SerializeField]
		private CustomText textStartStage;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private RectTransform buttonTrans;

		[SerializeField]
		private GameObject start;

		private TowerDataModule towerDataModule;

		private const float BUTTON_MOVE_X = 500f;
	}
}
