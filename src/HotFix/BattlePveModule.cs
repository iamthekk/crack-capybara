using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattlePveModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.textRound.text = "";
			this.m_BtnJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
			this.m_BtnSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.Editor);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_BtnJump.onClick.RemoveListener(new UnityAction(this.OnClickBtnJump));
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
		}

		private void OnRoundStartHandler(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			this.textRound.text = string.Format("{0}/{1}", eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound);
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		[SerializeField]
		private CustomText textRound;

		[SerializeField]
		private CustomButton m_BtnJump;

		[SerializeField]
		private UISpeedButtonCtrl m_BtnSpeedUp;
	}
}
