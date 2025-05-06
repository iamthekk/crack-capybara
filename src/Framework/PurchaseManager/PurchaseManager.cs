using System;
using UnityEngine;

namespace Framework.PurchaseManager
{
	public class PurchaseManager : MonoBehaviour
	{
		public bool IsEnable
		{
			get
			{
				return this.m_isEnable;
			}
		}

		public IPurchaseManager Manager
		{
			get
			{
				return this.m_manager;
			}
		}

		public BasePurchaseCaches Caches
		{
			get
			{
				return this.m_caches;
			}
		}

		public void SetManager(IPurchaseManager purchaseManager)
		{
			this.m_manager = purchaseManager;
		}

		public void SetCaches(BasePurchaseCaches caches)
		{
			this.m_caches = caches;
		}

		public void OnInit()
		{
			if (this.m_caches != null)
			{
				this.m_caches.OnInit();
			}
			if (this.m_manager != null)
			{
				this.m_manager.OnInit();
			}
		}

		public void OnDeInit()
		{
			if (this.m_caches != null)
			{
				this.m_caches.OnDeInit();
			}
			if (this.m_manager != null)
			{
				this.m_manager.OnDeInit();
			}
			this.m_manager = null;
			this.m_caches = null;
		}

		public string GetPriceForProductID(string productID)
		{
			if (this.m_manager == null)
			{
				return string.Empty;
			}
			return this.m_manager.GetPriceForProductID(productID);
		}

		[SerializeField]
		private bool m_isEnable;

		private IPurchaseManager m_manager;

		private BasePurchaseCaches m_caches;
	}
}
