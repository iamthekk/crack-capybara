using System;
using Framework;

namespace HotFix
{
	public class UIMainCity_GuildNode : UIBaseMainCityNode
	{
		public override int FunctionOpenID
		{
			get
			{
				return 24;
			}
		}

		public override MainCityName Name
		{
			get
			{
				return MainCityName.Guild;
			}
		}

		public override string RedName
		{
			get
			{
				return "Main.Guild";
			}
		}

		public override int NameLanguageID
		{
			get
			{
				return 4046;
			}
		}

		public override string NameLanguageIDStr
		{
			get
			{
				return "4046";
			}
		}

		protected override void OnClickUnlockBt()
		{
			GameApp.View.OpenView(ViewName.MainGuildViewModule, null, 1, null, null);
		}
	}
}
