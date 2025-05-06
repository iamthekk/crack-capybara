using System;
using System.Collections.Generic;

namespace HotFix
{
	public sealed class Bundle
	{
		public static Bundle Create()
		{
			return new Bundle();
		}

		public Bundle SetData<T>(string key, T value)
		{
			if (this.HasData(key))
			{
				this.mDict[key] = value;
			}
			else
			{
				this.mDict.Add(key, value);
			}
			return this;
		}

		public object GetData(string key)
		{
			if (this.HasData(key))
			{
				return this.mDict[key];
			}
			return null;
		}

		public T GetData<T>(string key)
		{
			if (this.HasData(key))
			{
				return (T)((object)this.mDict[key]);
			}
			return default(T);
		}

		public bool HasData(string key)
		{
			return this.mDict.ContainsKey(key);
		}

		private Dictionary<string, object> mDict = new Dictionary<string, object>();
	}
}
