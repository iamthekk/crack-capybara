using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class LoadPool<T> where T : Object
	{
		public async Task Loads(List<string> paths)
		{
			List<Task> list = new List<Task>(paths.Count);
			for (int i = 0; i < paths.Count; i++)
			{
				string text = paths[i];
				list.Add(this.OnLoad(text));
			}
			await Task.WhenAll(list);
		}

		public async Task Load(string path)
		{
			await this.OnLoad(path);
		}

		private Task OnLoad(string path)
		{
			LoadPool<T>.<OnLoad>d__3 <OnLoad>d__;
			<OnLoad>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnLoad>d__.<>4__this = this;
			<OnLoad>d__.path = path;
			<OnLoad>d__.<>1__state = -1;
			<OnLoad>d__.<>t__builder.Start<LoadPool<T>.<OnLoad>d__3>(ref <OnLoad>d__);
			return <OnLoad>d__.<>t__builder.Task;
		}

		public void UnLoadAll()
		{
			foreach (KeyValuePair<string, T> keyValuePair in this.m_pools)
			{
				if (!(keyValuePair.Value == null))
				{
					GameApp.Resources.Release<T>(keyValuePair.Value);
				}
			}
			this.m_pools.Clear();
		}

		public void UnLoads(List<string> paths)
		{
			for (int i = 0; i < paths.Count; i++)
			{
				string text = paths[i];
				this.UnLoad(text);
			}
		}

		public void UnLoad(string path)
		{
			T t;
			if (!this.m_pools.TryGetValue(path, out t))
			{
				return;
			}
			if (t == null)
			{
				return;
			}
			GameApp.Resources.Release<T>(t);
			t = default(T);
			this.m_pools[path] = default(T);
			this.m_pools.Remove(path);
		}

		public T Get(string path)
		{
			T t;
			this.m_pools.TryGetValue(path, out t);
			return t;
		}

		private Dictionary<string, T> m_pools = new Dictionary<string, T>();
	}
}
