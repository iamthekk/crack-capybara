using System;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_ApplyJoin
	{
		public void Init()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.RegisterEvent(24, new GuildHandlerEvent(this.OnApplyJoinCount));
			}
		}

		public void DeInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.UnRegisterEvent(24, new GuildHandlerEvent(this.OnApplyJoinCount));
			}
		}

		private void OnApplyJoinCount(int type, GuildBaseEvent eventArgs)
		{
			if (eventArgs is GuildEvent_ApplyJoinCount)
			{
				GuildProxy.RedPoint.CalcRedPoint("Guild.Hall.ApplyJoin", true);
			}
		}
	}
}
