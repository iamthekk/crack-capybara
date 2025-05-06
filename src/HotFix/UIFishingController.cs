using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;

namespace HotFix
{
	public class UIFishingController : CustomBehaviour
	{
		public bool isFishing { get; private set; }

		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Enter, new HandlerEvent(this.OnEventFishingEnter));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Sucess, new HandlerEvent(this.OnEventFishingSuccess));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Enter, new HandlerEvent(this.OnEventFishingEnter));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Sucess, new HandlerEvent(this.OnEventFishingSuccess));
		}

		private void OnEventFishingEnter(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = new EventArgsBool();
			eventArgsBool.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowEventLoading, eventArgsBool);
			this.isFishing = true;
			EventArgsString eventArgsString = new EventArgsString();
			eventArgsString.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_160"));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowEventTipInfo, eventArgsString);
		}

		private void OnEventFishingSuccess(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = new EventArgsBool();
			eventArgsBool.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowEventLoading, eventArgsBool);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseFishingResult, null);
			this.isFishing = false;
		}

		private int npcId;
	}
}
