using System;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class MainGuild_JoinedCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnShow()
		{
			base.gameObject.SetActive(true);
			if (this.guildBuilding != null)
			{
				this.guildBuilding.Show();
				return;
			}
			this.CreateNode();
		}

		protected override void GuildUI_OnClose()
		{
			if (this.guildBuilding != null)
			{
				this.guildBuilding.Close();
			}
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.guildBuilding != null)
			{
				this.guildBuilding.DeInit();
				Object.Destroy(this.guildBuilding);
			}
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.guildBuilding != null)
			{
				this.guildBuilding.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void SetPreload(GuildProxy.GuildPreLoad preload)
		{
			this.mPreload = preload;
		}

		private void CreateNode()
		{
			if (this.mPreload == null)
			{
				return;
			}
			GameObject gameObject = this.mPreload.CreateGameObject("UI_GuildBuilding", base.gameObject.transform);
			if (gameObject == null)
			{
				return;
			}
			gameObject.transform.SetParentNormal(base.gameObject, false);
			this.guildBuilding = gameObject.GetComponent<UIGuildBuilding>();
			this.guildBuilding.Init();
			this.guildBuilding.Show();
		}

		public bool OnClickCloseHook()
		{
			return true;
		}

		private UIGuildBuilding guildBuilding;

		private GuildProxy.GuildPreLoad mPreload;
	}
}
