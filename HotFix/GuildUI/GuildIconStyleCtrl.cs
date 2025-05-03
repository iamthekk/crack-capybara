using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildIconStyleCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.GuildIcon.Init();
			this.Button.onClick.AddListener(new UnityAction(this.OnSelectThis));
			this.ObjSelect.SetActive(false);
			this.ObjSelectFront.SetActive(false);
		}

		private void OnSelectThis()
		{
			Action<GuildIconStyleCtrl, int> onSelect = this.OnSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this, this._styleId);
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.Button != null)
			{
				this.Button.onClick.RemoveListener(new UnityAction(this.OnSelectThis));
			}
			if (this.GuildIcon != null)
			{
				this.GuildIcon.DeInit();
			}
		}

		public void SetData(int iconId)
		{
			this._styleId = iconId;
			this.GuildIcon.SetIcon(iconId);
			this.SetSelect(false);
		}

		public void SetSelect(bool value)
		{
			this.ObjSelect.SetActive(value);
			this.ObjSelectFront.SetActive(value);
		}

		public void RefreshSelect(int styleId)
		{
			bool flag = this._styleId == styleId;
			this.SetSelect(flag);
		}

		public UIGuildIcon GuildIcon;

		public CustomButton Button;

		public GameObject ObjSelect;

		public GameObject ObjSelectFront;

		private int _styleId;

		public Action<GuildIconStyleCtrl, int> OnSelect;
	}
}
