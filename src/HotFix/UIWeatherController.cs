using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIWeatherController : CustomBehaviour
	{
		protected override void OnInit()
		{
			base.gameObject.SetActiveSafe(false);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Weather, new HandlerEvent(this.OnEventWeather));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Weather, new HandlerEvent(this.OnEventWeather));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
		}

		private void OnEventWeather(object sender, int type, BaseEventArgs args)
		{
			EventArgWeather eventArgWeather = args as EventArgWeather;
			if (eventArgWeather != null && eventArgWeather.data != null)
			{
				ValueTuple<int, bool, bool>? valueTuple = GameEventItemData.ToWeatherData(eventArgWeather.data.param);
				if (valueTuple != null)
				{
					this.currentWeather = (EventWeather)valueTuple.Value.Item1;
				}
				this.endStage = eventArgWeather.data.endStage;
				this.imageWeather.sprite = this.spriteRegister.GetSprite(eventArgWeather.data.icon);
				this.textWeather.text = Singleton<LanguageManager>.Instance.GetInfoByID(eventArgWeather.data.languageId);
				base.gameObject.SetActiveSafe(true);
				this.RefreshDay(eventArgWeather.data.lifetime);
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

		public CustomImage imageWeather;

		public CustomText textDay;

		public CustomText textWeather;

		public SpriteRegister spriteRegister;

		private EventWeather currentWeather;

		private int endStage;
	}
}
