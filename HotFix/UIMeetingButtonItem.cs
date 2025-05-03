using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMeetingButtonItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		}

		public void SetData(int index, Action<int> onSelect)
		{
			this.mIndex = index;
			this.OnSelect = onSelect;
		}

		public void SetRedPoint(bool isRedPoint)
		{
			this.redNode.SetActiveSafe(isRedPoint);
		}

		public void SetSelect(int selectIndex)
		{
			this.button.SetSelect(this.mIndex == selectIndex);
		}

		public void SetPrice(string price)
		{
			this.textPrice.text = price;
		}

		private void OnClickButton()
		{
			Action<int> onSelect = this.OnSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.mIndex);
		}

		public CustomChooseButton button;

		public CustomText textPrice;

		public GameObject redNode;

		private int mIndex;

		private Action<int> OnSelect;
	}
}
