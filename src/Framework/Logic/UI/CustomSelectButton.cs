using System;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Logic.UI
{
	public class CustomSelectButton : CustomBehaviour
	{
		public bool IsSelected
		{
			get
			{
				return this.isSelect;
			}
		}

		protected override void OnInit()
		{
			if (this.isInited)
			{
				return;
			}
			this.isInited = true;
			if (this.button != null)
			{
				this.button.onClick.AddListener(new UnityAction(this.OnClickNodeButton));
			}
		}

		protected override void OnDeInit()
		{
			if (!this.isInited)
			{
				return;
			}
			if (this.button != null)
			{
				this.button.onClick.RemoveListener(new UnityAction(this.OnClickNodeButton));
			}
			this.onClick = null;
			this.onSelect = null;
			this.isInited = false;
		}

		private void OnClickNodeButton()
		{
			Action<CustomSelectButton> action = this.onClick;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void InitAndSetData(bool initSelect, Action<CustomSelectButton> onClickFunc = null, Action<CustomSelectButton> onSelectFunc = null)
		{
			base.Init();
			this.onClick = onClickFunc;
			this.onSelect = onSelectFunc;
			this.DoSelect(initSelect);
		}

		public void SetSelect(bool isSelectVal, bool force = false)
		{
			if (this.isSelect == isSelectVal && !force)
			{
				return;
			}
			this.DoSelect(isSelectVal);
		}

		private void DoSelect(bool isSelectVal)
		{
			this.isSelect = isSelectVal;
			if (isSelectVal)
			{
				this.OnSelect();
			}
			else
			{
				this.OnUnSelect();
			}
			Action<CustomSelectButton> action = this.onSelect;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		protected virtual void OnSelect()
		{
			this.UpdateView(true);
		}

		protected virtual void OnUnSelect()
		{
			this.UpdateView(false);
		}

		private void UpdateView(bool isSelect)
		{
			if (this.imgSelectBg != null)
			{
				if (this.selectMode == CustomSelectButton.SelectMode.Active)
				{
					this.imgSelectBg.gameObject.SetActive(isSelect);
				}
				else
				{
					this.imgSelectBg.enabled = isSelect;
				}
			}
			if (this.imgUnSelectBg != null)
			{
				if (this.selectMode == CustomSelectButton.SelectMode.Active)
				{
					this.imgUnSelectBg.gameObject.SetActive(!isSelect);
				}
				else
				{
					this.imgUnSelectBg.enabled = !isSelect;
				}
			}
			if (this.txtSelect != null)
			{
				if (this.selectMode == CustomSelectButton.SelectMode.Active)
				{
					this.txtSelect.gameObject.SetActive(isSelect);
				}
				else
				{
					this.txtSelect.enabled = isSelect;
				}
			}
			if (this.txtUnSelect != null)
			{
				if (this.selectMode == CustomSelectButton.SelectMode.Active)
				{
					this.txtUnSelect.gameObject.SetActive(!isSelect);
					return;
				}
				this.txtUnSelect.enabled = !isSelect;
			}
		}

		public CustomSelectButton.SelectMode selectMode = CustomSelectButton.SelectMode.Enable;

		public CustomButton button;

		public CustomImage imgSelectBg;

		public CustomImage imgUnSelectBg;

		public CustomLanguageText txtSelect;

		public CustomLanguageText txtUnSelect;

		private Action<CustomSelectButton> onClick;

		private Action<CustomSelectButton> onSelect;

		[SerializeField]
		private bool isSelect;

		private bool isInited;

		public enum SelectMode
		{
			Active,
			Enable
		}
	}
}
