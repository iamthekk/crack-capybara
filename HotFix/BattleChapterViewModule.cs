using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleChapterViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.m_BtnJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
			this.m_BtnSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvE);
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
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		[SerializeField]
		private CustomButton m_BtnJump;

		[SerializeField]
		private UISpeedButtonCtrl m_BtnSpeedUp;
	}
}
