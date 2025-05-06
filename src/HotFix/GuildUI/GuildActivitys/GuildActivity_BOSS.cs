using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI.GuildActivitys
{
	public class GuildActivity_BOSS : GuildActivityBase
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.Obj_Apply.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
		}

		public override void RefreshUIOnOpen()
		{
		}

		protected override void OnClickThis()
		{
			base.OnClickThis();
			GuildProxy.UI.OpenUIGuildBoss(null, null, delegate(GameObject obj)
			{
				GuildProxy.UI.CloseGuildActivity(null);
			});
		}

		public GameObject Obj_Apply;

		public CustomText Text_Dan;

		public CustomText Text_Time;

		public CustomText Text_ActivityState;
	}
}
