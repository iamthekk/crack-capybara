using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class CustomButtonForCustomLanguageAndItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_button = base.gameObject.GetComponent<CustomButton>();
			this.m_button.onClick.AddListener(new UnityAction(this.InternalOnClick));
		}

		protected override void OnDeInit()
		{
			if (this.m_button != null)
			{
				this.m_button.onClick.RemoveListener(new UnityAction(this.InternalOnClick));
			}
		}

		public void SetOnClick(Action onClick)
		{
			this.m_onClick = onClick;
		}

		public void SetCount(int count)
		{
			if (this.m_itemTxt == null)
			{
				return;
			}
			this.m_itemTxt.text = count.ToString();
		}

		public void SetCount(string info)
		{
			if (this.m_itemTxt == null)
			{
				return;
			}
			this.m_itemTxt.text = info;
		}

		public void RefreshLayerout()
		{
			if (this.itemParent == null)
			{
				return;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.itemParent);
		}

		private void InternalOnClick()
		{
			Action onClick = this.m_onClick;
			if (onClick == null)
			{
				return;
			}
			onClick();
		}

		[SerializeField]
		private RectTransform itemParent;

		[SerializeField]
		private CustomText m_itemTxt;

		private CustomButton m_button;

		public Action m_onClick;
	}
}
