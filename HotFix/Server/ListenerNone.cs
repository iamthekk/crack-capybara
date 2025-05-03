using System;
using System.Collections.Generic;

namespace Server
{
	public class ListenerNone
	{
		public void AddListen(ListenerNone.Listener action)
		{
			if (action == null)
			{
				return;
			}
			this.m_listeners.Add(action);
		}

		public void RemoveListen(ListenerNone.Listener action)
		{
			if (action == null)
			{
				return;
			}
			this.m_listeners.Remove(action);
		}

		public void RemoveAllListen()
		{
			this.m_listeners.Clear();
		}

		public void Invoke()
		{
			for (int i = 0; i < this.m_listeners.Count; i++)
			{
				ListenerNone.Listener listener = this.m_listeners[i];
				if (listener != null)
				{
					listener();
				}
			}
		}

		private List<ListenerNone.Listener> m_listeners = new List<ListenerNone.Listener>();

		public delegate void Listener();
	}
}
