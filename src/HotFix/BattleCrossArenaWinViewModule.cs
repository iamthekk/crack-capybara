using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleCrossArenaWinViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.avatarCtrlSelf.Init();
			this.avatarCtrlEnemy.Init();
		}

		public override void OnOpen(object data)
		{
			this.winEffect.SetActive(false);
			BattleCrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule);
			if (this.txtArenaName != null)
			{
				this.txtArenaName.text = CrossArenaDataModule.GetCrossArenaDanName(dataModule.Record.Dan);
			}
			if (this.avatarCtrlSelf != null)
			{
				this.avatarCtrlSelf.SetEnableButton(false);
				this.avatarCtrlSelf.RefreshData(dataModule.Record.OwnerUser.Avatar, dataModule.Record.OwnerUser.AvatarFrame);
			}
			if (this.txtSelfName != null)
			{
				this.txtSelfName.text = (string.IsNullOrEmpty(dataModule.Record.OwnerUser.NickName) ? DxxTools.GetDefaultNick(dataModule.Record.OwnerUser.UserId) : dataModule.Record.OwnerUser.NickName);
			}
			if (this.txtSelfScore != null)
			{
				int ownerChangeScore = dataModule.ownerChangeScore;
				if (ownerChangeScore > 0)
				{
					this.txtSelfScore.text = "+" + ownerChangeScore.ToString();
				}
				else
				{
					this.txtSelfScore.text = ownerChangeScore.ToString();
				}
			}
			if (this.avatarCtrlEnemy != null)
			{
				this.avatarCtrlEnemy.SetEnableButton(false);
				this.avatarCtrlEnemy.RefreshData(dataModule.Record.OtherUser.Avatar, dataModule.Record.OtherUser.AvatarFrame);
			}
			if (this.txtEnemyName != null)
			{
				this.txtEnemyName.text = (string.IsNullOrEmpty(dataModule.Record.OtherUser.NickName) ? DxxTools.GetDefaultNick(dataModule.Record.OtherUser.UserId) : dataModule.Record.OtherUser.NickName);
			}
			if (this.txtEnemyScore != null)
			{
				int targetChangeScore = dataModule.targetChangeScore;
				if (targetChangeScore > 0)
				{
					this.txtEnemyScore.text = "+" + targetChangeScore.ToString();
				}
				else
				{
					this.txtEnemyScore.text = targetChangeScore.ToString();
				}
			}
			base.StartCoroutine("PlayResultAni");
		}

		private IEnumerator PlayResultAni()
		{
			float duration = this.spineWin.SkeletonData.FindAnimation("ShengLi_1").Duration;
			this.spineWin.AnimationState.SetAnimation(0, "ShengLi_1", false);
			yield return new WaitForSeconds(duration);
			this.winEffect.SetActiveSafe(true);
			this.spineWin.AnimationState.SetAnimation(0, "ShengLi_2", true);
			this.PlayUIShow();
			yield break;
		}

		private void PlayUIShow()
		{
			if (this.AnimatorView != null)
			{
				this.AnimatorView.Play("open");
			}
		}

		public override void OnClose()
		{
			this.avatarCtrlSelf.DeInit();
			this.avatarCtrlEnemy.DeInit();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.BtnClose.onClick.AddListener(new UnityAction(this.OnBtnCloseHandler));
			this.btnDetails.m_onClick = new Action(this.OnBtnClickDetails);
			this.btnPlayback.m_onClick = new Action(this.OnBtnClickPlayback);
			this.btnConfirm.m_onClick = new Action(this.OnBtnCloseHandler);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			Button btnClose = this.BtnClose;
			if (btnClose != null)
			{
				Button.ButtonClickedEvent onClick = btnClose.onClick;
				if (onClick != null)
				{
					onClick.RemoveAllListeners();
				}
			}
			this.btnDetails.m_onClick = null;
			this.btnPlayback.m_onClick = null;
			this.btnConfirm.m_onClick = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnBtnClickDetails()
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("function_lock_locktip"));
		}

		private void OnBtnClickPlayback()
		{
			this.UnRegisterEvents(GameApp.Event);
			BattleCrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule);
			EventArgsBattleCrossArenaEnter instance = Singleton<EventArgsBattleCrossArenaEnter>.Instance;
			instance.SetData(dataModule.Record, true);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleCrossArena_BattleCrossArenaEnter, instance);
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					if (GameApp.View.IsOpened(ViewName.CrossArenaChallengeViewModule))
					{
						GameApp.View.CloseView(ViewName.CrossArenaChallengeViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.CrossArenaViewModule))
					{
						GameApp.View.CloseView(ViewName.CrossArenaViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.BattleCrossArenaWinViewModule))
					{
						GameApp.View.CloseView(ViewName.BattleCrossArenaWinViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.BattleCrossArenaLoseViewModule))
					{
						GameApp.View.CloseView(ViewName.BattleCrossArenaLoseViewModule, null);
					}
					EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
					instance2.SetData(GameModel.CrossArena, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
					GameApp.State.ActiveState(StateName.BattleCrossArenaState);
				});
			});
		}

		private void OnBtnCloseHandler()
		{
			this.UnRegisterEvents(GameApp.Event);
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					GameApp.View.CloseView(ViewName.BattleCrossArenaWinViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		public GameObject winEffect;

		public SkeletonGraphic spineWin;

		public Animator AnimatorView;

		[SerializeField]
		private Button BtnClose;

		public CustomText txtArenaName;

		public CustomText txtSelfName;

		public CustomText txtSelfScore;

		public CustomText txtEnemyName;

		public CustomText txtEnemyScore;

		public UIAvatarCtrl avatarCtrlSelf;

		public UIAvatarCtrl avatarCtrlEnemy;

		public CustomButton btnDetails;

		public CustomButton btnPlayback;

		public CustomButton btnConfirm;
	}
}
