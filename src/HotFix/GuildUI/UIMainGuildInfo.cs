using System;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIMainGuildInfo : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.joinGuildCtrl.Init();
			this.buttonClose.onClick.AddListener(new UnityAction(this.Close));
		}

		protected override void OnViewOpen(object data)
		{
			this.joinGuildCtrl.Show();
		}

		protected override void OnViewClose()
		{
			this.joinGuildCtrl.Close();
		}

		protected override void OnViewDelete()
		{
			if (this.buttonClose != null)
			{
				this.buttonClose.onClick.RemoveListener(new UnityAction(this.Close));
			}
			this.joinGuildCtrl.DeInit();
		}

		protected override void OnViewRegisterEvents(EventSystemManager manager)
		{
		}

		protected override void OnViewUnRegisterEvents(EventSystemManager manager)
		{
		}

		public void SetShowTag(UIMainGuildInfo.ShowTag showTag)
		{
		}

		public void SetShow(UIMainGuildInfo.ShowTag showTag)
		{
		}

		public void Close()
		{
			GuildProxy.UI.CloseMainGuildInfo();
		}

		[SerializeField]
		private UIMainGuildJoinGuildCtrl joinGuildCtrl;

		[SerializeField]
		private CustomButton buttonClose;

		public enum ShowTag
		{
			JoinGuild,
			CreateGuild
		}
	}
}
