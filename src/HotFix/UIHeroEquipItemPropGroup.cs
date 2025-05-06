using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIHeroEquipItemPropGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			Transform transform = base.transform;
			for (int i = 0; i < this.m_columnCount; i++)
			{
				UIItem component = transform.Find(string.Format("Item{0}", i + 1)).gameObject.GetComponent<UIItem>();
				component.SetCountShowType(UIItem.CountShowType.ShowAll);
				component.onClick = new Action<UIItem, PropData, object>(this.InternalClickItem);
				component.Init();
				this.m_items.Add(component);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_datas.Clear();
			for (int i = 0; i < this.m_items.Count; i++)
			{
				UIItem uiitem = this.m_items[i];
				if (!(uiitem == null))
				{
					uiitem.DeInit();
				}
			}
			this.m_items.Clear();
			this.m_onClick = null;
		}

		public void SetData(int columnCount, Action<UIItem, PropData, object> onClick)
		{
			this.m_columnCount = columnCount;
			this.m_onClick = onClick;
		}

		public void RefreshData(List<PropData> datas)
		{
			for (int i = 0; i < this.m_items.Count; i++)
			{
				UIItem uiitem = this.m_items[i];
				if (!(uiitem == null))
				{
					if (i < datas.Count)
					{
						uiitem.SetActive(true);
						uiitem.SetData(datas[i]);
						uiitem.OnRefresh();
					}
					else
					{
						uiitem.SetActive(false);
					}
				}
			}
		}

		private void InternalClickItem(UIItem item, PropData data, object arg3)
		{
			Action<UIItem, PropData, object> onClick = this.m_onClick;
			if (onClick == null)
			{
				return;
			}
			onClick(item, data, arg3);
		}

		private List<PropData> m_datas = new List<PropData>();

		[SerializeField]
		private List<UIItem> m_items = new List<UIItem>();

		[SerializeField]
		private int m_columnCount = 5;

		public Action<UIItem, PropData, object> m_onClick;
	}
}
