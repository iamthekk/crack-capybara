using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIHeroEquipItemGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				this.m_equipItems[i].m_onClick = this.m_onClick;
				this.m_equipItems[i].Init();
				this.m_equipItems[i].SetRedType(120);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_equipDatas.Clear();
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				UIHeroEquipItem uiheroEquipItem = this.m_equipItems[i];
				if (!(uiheroEquipItem == null))
				{
					uiheroEquipItem.DeInit();
				}
			}
			this.m_equipItems.Clear();
			this.m_onClick = null;
		}

		public void SetData(int columnCount, Action<UIHeroEquipItem> onClick)
		{
			this.m_columnCount = columnCount;
			this.m_onClick = onClick;
		}

		public void RefreshData(List<UIHeroEquipItemGroup.EquipDataItem> equipDatas, float starTime, float during, bool needAnimationInterval)
		{
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				UIHeroEquipItem uiheroEquipItem = this.m_equipItems[i];
				if (!(uiheroEquipItem == null))
				{
					if (i < equipDatas.Count)
					{
						UIHeroEquipItemGroup.EquipDataItem equipDataItem = equipDatas[i];
						uiheroEquipItem.SetActive(true);
						uiheroEquipItem.RefreshData(equipDataItem.m_equipData);
						uiheroEquipItem.SetRedNodeActive(equipDataItem.m_isNeedEquipRedTip);
						MonoAnimBase component = uiheroEquipItem.GetComponent<MonoAnimBase>();
						if (needAnimationInterval)
						{
							starTime += (float)i * 0.02f;
						}
						component.PlayShowAnimation(starTime, during);
					}
					else
					{
						uiheroEquipItem.SetActive(false);
						uiheroEquipItem.RefreshData(null);
					}
				}
			}
		}

		public UIHeroEquipItem GetItemByIndex(int index)
		{
			if (index < this.m_equipItems.Count)
			{
				return this.m_equipItems[index];
			}
			return null;
		}

		private List<EquipData> m_equipDatas = new List<EquipData>();

		[SerializeField]
		private List<UIHeroEquipItem> m_equipItems = new List<UIHeroEquipItem>();

		[SerializeField]
		private int m_columnCount = 5;

		public Action<UIHeroEquipItem> m_onClick;

		public class EquipDataItem
		{
			public EquipData m_equipData;

			public bool m_isNeedEquipRedTip;
		}
	}
}
