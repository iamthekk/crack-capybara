using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI
{
	public class CustomChooseButtonGroup : MonoBehaviour
	{
		public CustomChooseButton CurrentButton
		{
			get
			{
				for (int i = 0; i < this.Buttons.Count; i++)
				{
					CustomChooseButton customChooseButton = this.Buttons[i];
					if (!(customChooseButton == null) && customChooseButton.isActiveAndEnabled && customChooseButton.IsSelected)
					{
						return customChooseButton;
					}
				}
				return null;
			}
		}

		public string CurrentButtonName
		{
			get
			{
				for (int i = 0; i < this.Buttons.Count; i++)
				{
					CustomChooseButton customChooseButton = this.Buttons[i];
					if (!(customChooseButton == null) && customChooseButton.isActiveAndEnabled && customChooseButton.IsSelected)
					{
						return customChooseButton.name;
					}
				}
				return "";
			}
		}

		private void Awake()
		{
			this.CollectChildButtons();
		}

		public void CollectChildButtons()
		{
			this.Buttons.Clear();
			base.GetComponentsInChildren<CustomChooseButton>(false, this.Buttons);
			for (int i = 0; i < this.Buttons.Count; i++)
			{
				CustomChooseButton customChooseButton = this.Buttons[i];
				if (!(customChooseButton == null))
				{
					customChooseButton.OnClickButton = new Action<CustomChooseButton>(this.OnClickSwitchButton);
					customChooseButton.EnableOnClickButton();
				}
			}
		}

		private void OnClickSwitchButton(CustomChooseButton button)
		{
			if (this.Buttons == null)
			{
				return;
			}
			for (int i = 0; i < this.Buttons.Count; i++)
			{
				CustomChooseButton customChooseButton = this.Buttons[i];
				if (!(customChooseButton == null))
				{
					customChooseButton.SetSelect(customChooseButton == button);
				}
			}
			Action<CustomChooseButton> onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch(button);
		}

		public void ChooseButton(CustomChooseButton button)
		{
			this.OnClickSwitchButton(button);
		}

		public void ChooseButtonName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			for (int i = 0; i < this.Buttons.Count; i++)
			{
				CustomChooseButton customChooseButton = this.Buttons[i];
				if (!(customChooseButton == null) && customChooseButton.name == name)
				{
					this.OnClickSwitchButton(customChooseButton);
					return;
				}
			}
		}

		public List<CustomChooseButton> Buttons = new List<CustomChooseButton>();

		public Action<CustomChooseButton> OnSwitch;
	}
}
