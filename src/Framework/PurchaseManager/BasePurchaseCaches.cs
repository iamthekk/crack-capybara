using System;
using Framework.Logic;
using Newtonsoft.Json;

namespace Framework.PurchaseManager
{
	public abstract class BasePurchaseCaches
	{
		public void OnInit()
		{
			string @string = Utility.PlayerPrefs.GetString("PUCHASECACHE_Key", string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				this.m_data = new CachesData();
			}
			else
			{
				this.m_data = JsonConvert.DeserializeObject<CachesData>(@string);
			}
			this.m_isChecking = false;
		}

		public void OnDeInit()
		{
			this.m_data = null;
			this.m_isChecking = false;
		}

		public virtual void Add(ProductMessageData data)
		{
			if (this.m_data == null)
			{
				return;
			}
			if (data == null)
			{
				return;
			}
			this.m_data.m_datas.Add(data);
			this.Save();
		}

		public virtual void Remove(ProductMessageData data)
		{
			if (this.m_data == null)
			{
				return;
			}
			if (data == null)
			{
				return;
			}
			this.m_data.m_datas.Remove(data);
			GameApp.Purchase.Manager.RemovePurchaseLinkData(data.m_productID, data.m_timestamp);
			this.Save();
		}

		public virtual void Save()
		{
			if (this.m_data == null)
			{
				return;
			}
			if (this.m_data.m_datas == null)
			{
				return;
			}
			string text = JsonConvert.SerializeObject(this.m_data);
			Utility.PlayerPrefs.SetString("PUCHASECACHE_Key", text);
			Utility.PlayerPrefs.Save();
		}

		public void CheckCaches(Action finished)
		{
			if (this.m_data == null || this.m_data.m_datas == null || this.m_isChecking)
			{
				return;
			}
			this.m_isChecking = true;
			this.OnCheckNext(finished, 0);
		}

		protected abstract void OnCheckNext(Action finished, int index);

		public CachesData m_data;

		protected bool m_isChecking;
	}
}
