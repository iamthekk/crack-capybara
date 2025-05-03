using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public class DifferentRandom
	{
		public DifferentRandom(int seed)
		{
			this.systemRandom = new XRandom(seed);
		}

		public void Init(int[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				this.m_listStart.Add(list[i]);
			}
		}

		public void Init(List<int> list)
		{
			this.m_listStart.AddRange(list);
		}

		public void Add(int id)
		{
			this.m_listStart.Add(id);
		}

		public int GetRandom()
		{
			if (this.m_list.Count == 0)
			{
				this.m_list.AddRange(this.m_listStart);
			}
			int num = this.systemRandom.Range(0, this.m_list.Count);
			int num2 = this.m_list[num];
			this.m_list.RemoveAt(num);
			return num2;
		}

		protected List<int> m_list = new List<int>();

		protected List<int> m_listStart = new List<int>();

		private readonly XRandom systemRandom;
	}
}
