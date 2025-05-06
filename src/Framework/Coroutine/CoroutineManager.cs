using System;
using System.Collections;
using UnityEngine;

namespace Framework.Coroutine
{
	public class CoroutineManager : MonoBehaviour
	{
		public void AddTask(int type, IEnumerator routine)
		{
			this.m_agents[type].AddTask(routine);
		}

		public void RemoveTask(int type, IEnumerator routine)
		{
			this.m_agents[type].RemoveTask(routine);
		}

		public void RemoveAllTask(int type)
		{
			this.m_agents[type].RemoveAllTask();
		}

		public void RemoveAllTask()
		{
			for (int i = 0; i < this.m_agents.Length; i++)
			{
				this.m_agents[i].RemoveAllTask();
			}
		}

		public CoroutionAgent[] m_agents = new CoroutionAgent[3];
	}
}
