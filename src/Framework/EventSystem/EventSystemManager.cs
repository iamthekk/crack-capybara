using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.EventSystem
{
	public class EventSystemManager : MonoBehaviour, IModuleManager
	{
		public void Init()
		{
			if (this.isInited)
			{
				return;
			}
			this.isInited = true;
		}

		public void RegisterEvent(int type, HandlerEvent handle)
		{
			List<HandlerEvent> list;
			if (!this.m_handles.TryGetValue(type, out list))
			{
				list = new List<HandlerEvent>();
				this.m_handles.Add(type, list);
			}
			if (!list.Contains(handle))
			{
				list.Add(handle);
			}
		}

		public void UnRegisterEvent(int type, HandlerEvent handle)
		{
			List<HandlerEvent> list;
			if (this.m_handles.TryGetValue(type, out list))
			{
				list.Remove(handle);
				if (list.Count == 0)
				{
					this.m_handles.Remove(type);
				}
			}
		}

		public void UnRegisterAllEvent()
		{
			this.m_handles.Clear();
			this.m_dispatchDatas.Clear();
		}

		public void DispatchNow(object sender, int type, BaseEventArgs eventArgs = null)
		{
			List<HandlerEvent> list;
			if (this.m_handles.TryGetValue(type, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					HandlerEvent handlerEvent = list[i];
					if (handlerEvent != null)
					{
						handlerEvent(sender, type, eventArgs);
					}
				}
			}
		}

		public void Dispatch(object sender, int type, BaseEventArgs eventArgs = null)
		{
			this.m_dispatchDatas.Add(new DispatchData
			{
				m_sender = sender,
				m_type = type,
				m_eventArgs = eventArgs
			});
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_dispatchDatas.Count > 0)
			{
				List<DispatchData> dispatchDatas = this.m_dispatchDatas;
				int count = dispatchDatas.Count;
				for (int i = 0; i < count; i++)
				{
					DispatchData dispatchData = dispatchDatas[i];
					this.DispatchNow(dispatchData.m_sender, dispatchData.m_type, dispatchData.m_eventArgs);
				}
				this.m_dispatchDatas.Clear();
			}
		}

		private Dictionary<int, List<HandlerEvent>> m_handles = new Dictionary<int, List<HandlerEvent>>();

		private List<DispatchData> m_dispatchDatas = new List<DispatchData>();

		private bool isInited;
	}
}
