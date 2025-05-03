using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildManageMemberPositionChangeButton : GuildProxy.GuildProxy_BaseBehaviour
	{
		public bool IsSelect { get; private set; }

		protected override void GuildUI_OnInit()
		{
			this.Button.onClick.AddListener(new UnityAction(this.OnInternalClick));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.Button.onClick.RemoveListener(new UnityAction(this.OnInternalClick));
		}

		public void SetSelected(bool selected)
		{
			this.IsSelect = selected;
			this.ObjSelected.SetActive(selected);
		}

		public void SetIsCanChange(bool canChange)
		{
			this.IsCanChange = canChange;
			this.Button.interactable = this.IsCanChange;
		}

		private void OnInternalClick()
		{
			if (!this.IsCanChange)
			{
				return;
			}
			Action<GuildManageMemberPositionChangeButton, bool> onChange = this.OnChange;
			if (onChange == null)
			{
				return;
			}
			onChange(this, !this.IsSelect);
		}

		public CustomButton Button;

		public GameObject ObjSelected;

		public GuildPositionType Position;

		public bool IsCanChange = true;

		public Action<GuildManageMemberPositionChangeButton, bool> OnChange;
	}
}
