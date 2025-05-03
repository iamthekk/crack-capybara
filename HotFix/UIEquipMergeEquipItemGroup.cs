using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIEquipMergeEquipItemGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				this.m_equipItems[i].Init();
				this.m_equipItems[i].SetRedType(110);
				this.m_equipItems[i].m_onClick = this.m_onClick;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				UIHeroEquipItem uiheroEquipItem = this.m_equipItems[i];
				if (!(uiheroEquipItem == null))
				{
					uiheroEquipItem.DeInit();
				}
			}
			this.m_data = null;
			this.m_equipItems.Clear();
			this.m_onClick = null;
		}

		public void SetData(int columnCount, Action<UIHeroEquipItem> onClick)
		{
			this.m_columnCount = columnCount;
			this.m_onClick = onClick;
		}

		public void RefreshData(EquipMergeViewModule.NodeData data, float starTime = 0f, float during = 0f, bool needAnimationInterval = false)
		{
			this.m_data = data;
			for (int i = 0; i < this.m_equipItems.Count; i++)
			{
				UIHeroEquipItem uiheroEquipItem = this.m_equipItems[i];
				if (!(uiheroEquipItem == null))
				{
					if (i < this.m_data.m_equipDatas.Count)
					{
						UIEquipMergeEquipItemGroup.NodeItemData nodeItemData = this.m_data.m_equipDatas[i];
						uiheroEquipItem.SetActive(true);
						uiheroEquipItem.SetButtonEnable(!nodeItemData.m_lockActive);
						uiheroEquipItem.RefreshData(nodeItemData.m_equipData);
						uiheroEquipItem.SetMaskActive(nodeItemData.m_lockActive || nodeItemData.m_tickActive);
						uiheroEquipItem.SetTickActive(nodeItemData.m_tickActive);
						uiheroEquipItem.SetPutOnActive(nodeItemData.m_putonActive);
						uiheroEquipItem.SetLockActive(nodeItemData.m_lockActive);
						uiheroEquipItem.SetRedNodeActive(nodeItemData.m_isCanMerge);
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

		[SerializeField]
		private List<UIHeroEquipItem> m_equipItems = new List<UIHeroEquipItem>();

		[SerializeField]
		private int m_columnCount = 5;

		public Action<UIHeroEquipItem> m_onClick;

		public EquipMergeViewModule.NodeData m_data;

		public class NodeItemData
		{
			public EquipData m_equipData;

			public bool m_lockActive;

			public bool m_tickActive;

			public bool m_putonActive;

			public bool m_isCanMerge;
		}
	}
}
