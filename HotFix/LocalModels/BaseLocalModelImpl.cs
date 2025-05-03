using System;
using System.Collections.Generic;
using System.Net;

namespace LocalModels
{
	public abstract class BaseLocalModelImpl<T, Key> where T : BaseLocalBean
	{
		protected abstract IBeanBuilder GetBuilder();

		protected abstract Key GetBeanKey(T bean);

		protected virtual void ArrangeElements()
		{
			foreach (T t in this.beans)
			{
				Key beanKey = this.GetBeanKey(t);
				this.beansMap[beanKey] = t;
			}
		}

		public void Initialise(string name, byte[] bytes)
		{
			this.fileName = name;
			this.fileBytes = bytes;
			this.ReadFromFile();
			this.ArrangeElements();
		}

		public void DeInitialise()
		{
			this.fileName = null;
			this.fileBytes = null;
			this.beans.Clear();
			this.beansMap.Clear();
		}

		public IList<T> GetAllElement()
		{
			return this.beans;
		}

		public T GetElementById(Key key)
		{
			T t = default(T);
			if (this.beansMap.TryGetValue(key, out t))
			{
				return t;
			}
			if (!string.IsNullOrEmpty(this.fileName))
			{
				this.fileName.Equals("LanguageCN_languagetable");
			}
			return default(T);
		}

		protected bool ReadFromFile()
		{
			IBeanBuilder builder = this.GetBuilder();
			try
			{
				byte[] array = this.fileBytes;
				if (array == null)
				{
					HLog.LogError("Config file:" + this.fileName + " not Find");
					return false;
				}
				int elementCount = this.GetElementCount(array);
				int num = 4;
				for (int i = 0; i < elementCount; i++)
				{
					T t = (T)((object)builder.createBean());
					num = t.readFromBytes(array, num);
					if (num < 0)
					{
						HLog.LogError("read resource file failed :: " + this.fileName);
						break;
					}
					this.beans.Add(t);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				return false;
			}
			return true;
		}

		protected int GetElementCount(byte[] raws)
		{
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(new byte[]
			{
				raws[0],
				raws[1],
				raws[2],
				raws[3]
			}, 0));
		}

		private string fileName;

		private byte[] fileBytes;

		private IList<T> beans = new List<T>();

		private Dictionary<Key, T> beansMap = new Dictionary<Key, T>();
	}
}
