using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPTabBase<T> : CustomBehaviour where T : Enum
	{
		public T TabType { get; private set; }

		protected override void OnInit()
		{
			this.tabButton.onClick.AddListener(new UnityAction(this.OnClickNodeButton));
		}

		protected override void OnDeInit()
		{
			this.tabButton.onClick.RemoveListener(new UnityAction(this.OnClickNodeButton));
		}

		private void OnClickNodeButton()
		{
			Action<T> action = this.onClick;
			if (action == null)
			{
				return;
			}
			action(this.TabType);
		}

		public void SetData(bool initSelect, Action<T> onClickVul)
		{
			this.onClick = onClickVul;
			this.DoSelect(initSelect);
		}

		public void SetSelect(bool isSelectVal)
		{
			if (this.isSelect == isSelectVal)
			{
				return;
			}
			this.DoSelect(isSelectVal);
		}

		protected void DoSelect(bool isSelectVal)
		{
			this.isSelect = isSelectVal;
			if (isSelectVal)
			{
				this.OnSelect();
				return;
			}
			this.OnUnSelect();
		}

		protected virtual void OnSelect()
		{
		}

		protected virtual void OnUnSelect()
		{
		}

		[SerializeField]
		private CustomButton tabButton;

		private Action<T> onClick;

		private bool isSelect;
	}
}
