using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.Serialization;

namespace HotFix
{
	public class UIEquipMergePropItemGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.m_propItems.Count; i++)
			{
				this.m_propItems[i].Init();
				this.m_propItems[i].onClick = this.m_onClick;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.m_propItems.Count; i++)
			{
				UIItem uiitem = this.m_propItems[i];
				if (!(uiitem == null))
				{
					uiitem.DeInit();
				}
			}
			this.m_data = null;
			this.m_propItems.Clear();
			this.m_onClick = null;
		}

		public void SetData(int columnCount, Action<UIItem, PropData, object> onClick)
		{
			this.m_columnCount = columnCount;
			this.m_onClick = onClick;
		}

		public void RefreshData(EquipMergeViewModule.NodeData data, float starTime = 0f, float during = 0f, bool needAnimationInterval = false)
		{
			this.m_data = data;
			for (int i = 0; i < this.m_propItems.Count; i++)
			{
				UIItem uiitem = this.m_propItems[i];
				if (!(uiitem == null))
				{
					if (i < this.m_data.m_propDatas.Count)
					{
						UIEquipMergePropItemGroup.NodePropData nodePropData = this.m_data.m_propDatas[i];
						uiitem.SetActive(true);
						uiitem.SetEnableButton(!nodePropData.m_lockActive);
						uiitem.SetData(nodePropData.m_propData);
						uiitem.OnRefresh();
						uiitem.SetMaskActive(nodePropData.m_lockActive || nodePropData.m_tickActive);
						uiitem.SetTickActive(nodePropData.m_tickActive);
						uiitem.SetLockActive(nodePropData.m_lockActive);
						MonoAnimBase component = uiitem.GetComponent<MonoAnimBase>();
						if (needAnimationInterval)
						{
							starTime += (float)i * 0.02f;
						}
						component.PlayShowAnimation(starTime, during);
					}
					else
					{
						uiitem.SetActive(false);
					}
				}
			}
		}

		[FormerlySerializedAs("m_equipItems")]
		[SerializeField]
		private List<UIItem> m_propItems = new List<UIItem>();

		[SerializeField]
		private int m_columnCount = 5;

		public Action<UIItem, PropData, object> m_onClick;

		public EquipMergeViewModule.NodeData m_data;

		public class NodePropData
		{
			public PropData m_propData;

			public bool m_lockActive;

			public bool m_tickActive;
		}
	}
}
