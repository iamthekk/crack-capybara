using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UICostCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (this.m_prefab != null)
			{
				this.m_prefab.SetActive(false);
			}
		}

		protected override void OnDeInit()
		{
			this.DestroyUI();
			if (this.m_costDatas != null)
			{
				this.m_costDatas.Clear();
			}
			this.m_costDatas = null;
			this.m_propDataModule = null;
		}

		public void RefreshUI(List<ItemData> costDatas)
		{
			this.m_costDatas = costDatas;
			this.DestroyUI();
			this.CreateUI();
		}

		private void CreateUI()
		{
			if (this.m_costDatas == null)
			{
				return;
			}
			for (int i = 0; i < this.m_costDatas.Count; i++)
			{
				ItemData itemData = this.m_costDatas[i];
				if (itemData != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_prefab);
					gameObject.SetParentNormal(this.m_parent, false);
					UICostNode component = gameObject.GetComponent<UICostNode>();
					component.Init();
					component.SetActive(true);
					component.SetData(itemData, this.m_propDataModule.GetItemDataCountByid((ulong)((long)itemData.ID)));
					this.m_costItems[component.GetObjectInstanceID()] = component;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_parent);
		}

		private void DestroyUI()
		{
			if (this.m_costItems == null)
			{
				return;
			}
			foreach (KeyValuePair<int, UICostNode> keyValuePair in this.m_costItems)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					if (!(keyValuePair.Value.gameObject == null))
					{
						Object.Destroy(keyValuePair.Value.gameObject);
					}
				}
			}
			this.m_costItems.Clear();
		}

		public RectTransform m_parent;

		public GameObject m_prefab;

		private List<ItemData> m_costDatas;

		[SerializeField]
		private Dictionary<int, UICostNode> m_costItems = new Dictionary<int, UICostNode>();

		private PropDataModule m_propDataModule;
	}
}
