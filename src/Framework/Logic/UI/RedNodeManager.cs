using System;
using System.Collections.Generic;

namespace Framework.Logic.UI
{
	public class RedNodeManager : Singleton<RedNodeManager>
	{
		public void Add(RedNodeOneCtrl ctrl)
		{
			if (ctrl == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(ctrl.Key))
			{
				return;
			}
			List<RedNodeOneCtrl> list;
			this.m_ctrls.TryGetValue(ctrl.Key, out list);
			if (list == null)
			{
				list = new List<RedNodeOneCtrl>();
			}
			list.Add(ctrl);
			this.m_ctrls[ctrl.Key] = list;
			if (this.m_onAdd != null)
			{
				this.m_onAdd(ctrl);
			}
		}

		public void Remove(RedNodeOneCtrl ctrl)
		{
			if (ctrl == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(ctrl.Key))
			{
				return;
			}
			List<RedNodeOneCtrl> list;
			this.m_ctrls.TryGetValue(ctrl.Key, out list);
			if (list == null)
			{
				return;
			}
			list.Remove(ctrl);
			this.m_ctrls[ctrl.Key] = list;
			if (this.m_onRemove != null)
			{
				this.m_onRemove(ctrl);
			}
		}

		public void OnRefresh(string key, RedNodeListenData listenData)
		{
			if (listenData == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			List<RedNodeOneCtrl> list;
			this.m_ctrls.TryGetValue(key, out list);
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				RedNodeOneCtrl redNodeOneCtrl = list[i];
				if (!(redNodeOneCtrl == null))
				{
					redNodeOneCtrl.OnRefresh(listenData);
				}
			}
		}

		public void OnRefresh(RedNodeOneCtrl redNode, RedNodeListenData listenData)
		{
			if (redNode == null)
			{
				return;
			}
			if (listenData == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(redNode.Key))
			{
				return;
			}
			redNode.OnRefresh(listenData);
		}

		private Dictionary<string, List<RedNodeOneCtrl>> m_ctrls = new Dictionary<string, List<RedNodeOneCtrl>>();

		public Action<RedNodeOneCtrl> m_onAdd;

		public Action<RedNodeOneCtrl> m_onRemove;
	}
}
