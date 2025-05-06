using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UICheckBoxCtrl : CustomBehaviour
	{
		public bool IsSelect
		{
			get
			{
				return this.mIsSelect;
			}
		}

		protected override void OnInit()
		{
			this.buttonCheckBox.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		protected override void OnDeInit()
		{
			this.buttonCheckBox.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		}

		public void SetData(Action<bool> onClick)
		{
			this.mOnClick = onClick;
		}

		public void SetSelect(bool isSelect)
		{
			this.mIsSelect = isSelect;
			this.checkmark.SetActiveSafe(this.mIsSelect);
		}

		private void OnClickButton()
		{
			this.SetSelect(!this.mIsSelect);
			Action<bool> action = this.mOnClick;
			if (action == null)
			{
				return;
			}
			action(this.mIsSelect);
		}

		public CustomButton buttonCheckBox;

		public GameObject checkmark;

		private bool mIsSelect;

		private Action<bool> mOnClick;
	}
}
