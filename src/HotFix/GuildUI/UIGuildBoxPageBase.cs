using System;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public abstract class UIGuildBoxPageBase : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.gotoButton.onClick.AddListener(new UnityAction(this.OnClickGotoButton));
			this.OnInit();
		}

		protected new abstract void OnInit();

		protected abstract void OnClickGotoButton();

		protected override void GuildUI_OnUnInit()
		{
			this.gotoButton.onClick.RemoveListener(new UnityAction(this.OnClickGotoButton));
			this.OnDeInit();
		}

		protected new abstract void OnDeInit();

		public CustomLanguageText desText;

		public CustomButton gotoButton;

		public LoopListView2 loopListView2;
	}
}
