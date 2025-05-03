using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_UserSetPowerItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public GuildPositionType GuildPosition { get; set; }

		protected override void GuildUI_OnInit()
		{
			this.ButtonSet.onClick.AddListener(new UnityAction(this.OnClickSetPosition));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.ButtonSet.onClick.RemoveListener(new UnityAction(this.OnClickSetPosition));
		}

		private void OnClickSetPosition()
		{
			Action<GuildInfoPopUI_UserSetPowerItem> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void Sel(bool sel)
		{
		}

		public void SetGray(bool gray)
		{
			this.Mask.gameObject.SetActiveSafe(gray);
			this.ButtonSet.enabled = !gray;
		}

		public CustomButton ButtonSet;

		public Text Text;

		private bool _clickable = true;

		public CustomImage Mask;

		public Action<GuildInfoPopUI_UserSetPowerItem> OnClick;
	}
}
