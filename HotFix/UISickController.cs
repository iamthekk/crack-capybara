using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISickController : CustomBehaviour
	{
		protected override void OnInit()
		{
			base.gameObject.SetActiveSafe(false);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Sick, new HandlerEvent(this.OnEventSick));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Sick, new HandlerEvent(this.OnEventSick));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
		}

		private void OnEventSick(object sender, int type, BaseEventArgs args)
		{
			EventArgSick eventArgSick = args as EventArgSick;
			if (eventArgSick != null && eventArgSick.data != null)
			{
				int num;
				if (int.TryParse(eventArgSick.data.param, out num))
				{
					this.sick = (EventSick)num;
				}
				this.endStage = eventArgSick.data.endStage;
				this.imageSick.sprite = this.spriteRegister.GetSprite(eventArgSick.data.icon);
				this.textSick.text = Singleton<LanguageManager>.Instance.GetInfoByID(eventArgSick.data.languageId);
				base.gameObject.SetActiveSafe(true);
				this.RefreshDay(eventArgSick.data.lifetime);
			}
		}

		private void OnEventEnterNewStage(object sender, int type, BaseEventArgs args)
		{
			EventArgsInt eventArgsInt = args as EventArgsInt;
			if (eventArgsInt != null)
			{
				int num = this.endStage - eventArgsInt.Value;
				this.RefreshDay(num);
				if (num < 0)
				{
					base.gameObject.SetActiveSafe(false);
				}
			}
		}

		private void RefreshDay(int day)
		{
			this.textDay.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_157", new object[] { day });
		}

		public CustomImage imageSick;

		public CustomText textDay;

		public CustomText textSick;

		public SpriteRegister spriteRegister;

		private EventSick sick;

		private int endStage;

		private int currentDay;
	}
}
