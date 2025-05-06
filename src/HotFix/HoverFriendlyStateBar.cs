using System;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class HoverFriendlyStateBar : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.FriendlyStateBar;
		}

		protected override void OnInit()
		{
			base.gameObject.SetActive(false);
			this.hpBar.OnInit();
			this.rechargeBar.Init();
			HoverStateBarData hoverStateBarData = this.hoverData as HoverStateBarData;
			if (hoverStateBarData != null)
			{
				this.hpBar.SetProgress(hoverStateBarData.hpData.current, hoverStateBarData.hpData.max);
				this.rechargeBar.SetProgress(hoverStateBarData.rechargeData.current, hoverStateBarData.rechargeData.max);
			}
			else
			{
				this.hpBar.SetValuePercent(1f);
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RefreshHP, new HandlerEvent(this.OnRefreshHP));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RefresRecharge, new HandlerEvent(this.OnRefreshRecharge));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RefreshShild, new HandlerEvent(this.OnRefreshShield));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnShowHpHUD));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RefreshHP, new HandlerEvent(this.OnRefreshHP));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RefresRecharge, new HandlerEvent(this.OnRefreshRecharge));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RefreshShild, new HandlerEvent(this.OnRefreshShield));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnShowHpHUD));
			this.hpBar.OnDeInit();
			this.rechargeBar.DeInit();
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaledDeltaTime)
		{
			this.RefreshTargetPos(base.target.transform.position);
			if (this.hpBar != null)
			{
				this.hpBar.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.rechargeBar != null)
			{
				this.rechargeBar.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		private void OnRefreshHP(object sender, int id, BaseEventArgs args)
		{
			EventArgsRefreshHP eventArgsRefreshHP = args as EventArgsRefreshHP;
			if (eventArgsRefreshHP == null)
			{
				return;
			}
			if (base.ownerId != eventArgsRefreshHP.memberInstanceId)
			{
				return;
			}
			if (this.hpBar == null)
			{
				return;
			}
			this.hpBar.SetProgress(eventArgsRefreshHP.current, eventArgsRefreshHP.max);
		}

		private void OnRefreshRecharge(object sender, int id, BaseEventArgs args)
		{
			EventArgsRefreshRecharge eventArgsRefreshRecharge = args as EventArgsRefreshRecharge;
			if (eventArgsRefreshRecharge == null)
			{
				return;
			}
			if (base.ownerId != eventArgsRefreshRecharge.memberInstanceId)
			{
				return;
			}
			if (this.rechargeBar == null)
			{
				return;
			}
			this.rechargeBar.SetProgress(eventArgsRefreshRecharge.current, eventArgsRefreshRecharge.max);
		}

		private void OnRefreshShield(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshShield eventArgsRefreshShield = args as EventArgsRefreshShield;
			if (eventArgsRefreshShield == null)
			{
				return;
			}
			if (base.ownerId != eventArgsRefreshShield.memberInstanceId)
			{
				return;
			}
			if (this.hpBar == null)
			{
				return;
			}
			this.hpBar.SetShield(eventArgsRefreshShield.current);
		}

		private void OnShowHpHUD(object sender, int type, BaseEventArgs args)
		{
			EventArgsShowHpHUD eventArgsShowHpHUD = args as EventArgsShowHpHUD;
			if (eventArgsShowHpHUD == null)
			{
				return;
			}
			if (base.ownerId != eventArgsShowHpHUD.instanceId)
			{
				return;
			}
			base.gameObject.SetActive(eventArgsShowHpHUD.isShow);
		}

		public UIProgressShieldBar hpBar;

		public UIProgressBar rechargeBar;
	}
}
