using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleCrossArenaViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_leftHpSlider.OnInit();
			this.m_rightHpSlider.OnInit();
		}

		public override void OnOpen(object data)
		{
			this.HideRoundText();
			this.m_battleCrossArenaDataModule = GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule);
			if (this.m_leftAvatar != null)
			{
				this.m_leftAvatar.Init();
				this.m_leftAvatar.SetEnableButton(false);
				this.m_leftAvatar.RefreshData(this.m_battleCrossArenaDataModule.Record.OwnerUser.Avatar, this.m_battleCrossArenaDataModule.Record.OwnerUser.AvatarFrame);
			}
			if (this.m_leftNickName != null)
			{
				this.m_leftNickName.text = (string.IsNullOrEmpty(this.m_battleCrossArenaDataModule.Record.OwnerUser.NickName) ? DxxTools.GetDefaultNick(this.m_battleCrossArenaDataModule.Record.OwnerUser.UserId) : this.m_battleCrossArenaDataModule.Record.OwnerUser.NickName);
			}
			if (this.m_leftPower != null)
			{
				long power = this.m_battleCrossArenaDataModule.Record.OwnerUser.Power;
				this.m_leftPower.text = DxxTools.FormatNumber(power);
			}
			if (this.m_rightAvatar != null)
			{
				this.m_rightAvatar.Init();
				this.m_rightAvatar.SetEnableButton(false);
				this.m_rightAvatar.RefreshData(this.m_battleCrossArenaDataModule.Record.OtherUser.Avatar, this.m_battleCrossArenaDataModule.Record.OtherUser.AvatarFrame);
			}
			if (this.m_rightNickName != null)
			{
				this.m_rightNickName.text = (string.IsNullOrEmpty(this.m_battleCrossArenaDataModule.Record.OtherUser.NickName) ? DxxTools.GetDefaultNick(this.m_battleCrossArenaDataModule.Record.OtherUser.UserId) : this.m_battleCrossArenaDataModule.Record.OtherUser.NickName);
			}
			if (this.m_rightPower != null)
			{
				long power2 = this.m_battleCrossArenaDataModule.Record.OtherUser.Power;
				this.m_rightPower.text = DxxTools.FormatNumber(power2);
			}
			this.UpdateSelfTotalHp(1, 1, this.m_leftHpSlider);
			this.UpdateSelfTotalHp(1, 1, this.m_rightHpSlider);
			this.m_BtnJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
			this.m_BtnSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvP);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_leftHpSlider.OnUpdate(deltaTime, unscaledDeltaTime);
			this.m_rightHpSlider.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.m_BtnJump.onClick.RemoveListener(new UnityAction(this.OnClickBtnJump));
			if (this.m_leftAvatar != null)
			{
				this.m_leftAvatar.DeInit();
			}
			if (this.m_rightAvatar != null)
			{
				this.m_rightAvatar.DeInit();
			}
			this.m_battleCrossArenaDataModule = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_CrossArenaHpInfo, new HandlerEvent(this.OnCrossArenaHpInfo));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_CrossArenaHpInfo, new HandlerEvent(this.OnCrossArenaHpInfo));
		}

		private void OnRoundStartHandler(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			if (this.m_txtRound == null)
			{
				return;
			}
			this.m_txtRound.transform.parent.gameObject.SetActiveSafe(true);
			this.m_txtRound.text = string.Format("{0}/{1}", eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound);
		}

		private void OnCrossArenaHpInfo(object sender, int type, BaseEventArgs args)
		{
			EventArgCrossArenaHpInfo eventArgCrossArenaHpInfo = args as EventArgCrossArenaHpInfo;
			if (eventArgCrossArenaHpInfo == null)
			{
				return;
			}
			if (this.lastSelfCampHp > 0 && (int)eventArgCrossArenaHpInfo.m_curHpAllFriendly < this.lastSelfCampHp && this.m_leftAvatarAnimator != null)
			{
				this.m_leftAvatarAnimator.Play("shake");
			}
			if (this.lastEnemyCampHp > 0 && (int)eventArgCrossArenaHpInfo.m_curHpAllEnemy < this.lastEnemyCampHp && this.m_rightAvatarAnimator != null)
			{
				this.m_rightAvatarAnimator.Play("shake");
			}
			this.UpdateSelfTotalHp((int)eventArgCrossArenaHpInfo.m_curHpAllFriendly, (int)eventArgCrossArenaHpInfo.m_cmaxHpAllFriendly, this.m_leftHpSlider);
			this.UpdateSelfTotalHp((int)eventArgCrossArenaHpInfo.m_curHpAllEnemy, (int)eventArgCrossArenaHpInfo.m_maxHpAllEnemy, this.m_rightHpSlider);
			this.lastSelfCampHp = (int)eventArgCrossArenaHpInfo.m_curHpAllFriendly;
			this.lastEnemyCampHp = (int)eventArgCrossArenaHpInfo.m_curHpAllEnemy;
		}

		private void UpdateSelfTotalHp(int curHp, int maxHp, UIProgressShieldBar slider)
		{
			if (slider != null)
			{
				slider.SetProgress((long)curHp, (long)maxHp);
			}
		}

		public void HideRoundText()
		{
			this.m_txtRound.transform.parent.gameObject.SetActiveSafe(false);
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		[SerializeField]
		private CustomButton m_BtnJump;

		[SerializeField]
		private UISpeedButtonCtrl m_BtnSpeedUp;

		public RectTransform m_leftAvatarGroup;

		public UIAvatarCtrl m_leftAvatar;

		public Animator m_leftAvatarAnimator;

		public CustomText m_leftNickName;

		public CustomText m_leftPower;

		public UIProgressShieldBar m_leftHpSlider;

		public RectTransform m_rightAvatarGroup;

		public UIAvatarCtrl m_rightAvatar;

		public Animator m_rightAvatarAnimator;

		public CustomText m_rightNickName;

		public CustomText m_rightPower;

		public UIProgressShieldBar m_rightHpSlider;

		public CustomText m_txtRound;

		private BattleCrossArenaDataModule m_battleCrossArenaDataModule;

		private int lastSelfCampHp;

		private int lastEnemyCampHp;
	}
}
