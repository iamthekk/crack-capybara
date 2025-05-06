using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(1)]
	public class GuildEventModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 1;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			return true;
		}

		public void UnInit()
		{
		}

		public void RegisterEvent(int type, GuildHandlerEvent handle)
		{
			List<GuildHandlerEvent> list;
			if (this.m_handles.TryGetValue(type, out list))
			{
				list.Add(handle);
				return;
			}
			if (list == null)
			{
				list = new List<GuildHandlerEvent>();
			}
			list.Add(handle);
			this.m_handles.Add(type, list);
		}

		public void UnRegisterEvent(int type, GuildHandlerEvent handle)
		{
			List<GuildHandlerEvent> list = null;
			if (this.m_handles.TryGetValue(type, out list) && list.Contains(handle))
			{
				list.Remove(handle);
			}
		}

		public void DispatchNow(int type, GuildBaseEvent eventArgs = null)
		{
			List<GuildHandlerEvent> list = null;
			if (!this.m_handles.TryGetValue(type, out list))
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					list[i](type, eventArgs);
				}
			}
		}

		private Dictionary<int, List<GuildHandlerEvent>> m_handles = new Dictionary<int, List<GuildHandlerEvent>>();
	}
}
