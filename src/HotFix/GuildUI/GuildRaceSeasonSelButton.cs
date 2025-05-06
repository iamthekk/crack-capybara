using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceSeasonSelButton : GuildProxy.GuildProxy_BaseBehaviour
	{
		public bool Selected { get; private set; }

		protected override void GuildUI_OnInit()
		{
			this.Image_Sel.SetActive(false);
			this.Button.onClick.AddListener(new UnityAction(this.OnClickSelectThis));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button = this.Button;
			if (button == null)
			{
				return;
			}
			button.onClick.RemoveListener(new UnityAction(this.OnClickSelectThis));
		}

		private void OnClickSelectThis()
		{
			Action<GuildRaceSeasonSelButton> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void SetSelect(bool sel)
		{
			this.Selected = sel;
			this.Image_Sel.SetActive(sel);
		}

		public GameObject Image_Sel;

		public CustomButton Button;

		[HideInInspector]
		public int Index;

		public Action<GuildRaceSeasonSelButton> OnClick;
	}
}
