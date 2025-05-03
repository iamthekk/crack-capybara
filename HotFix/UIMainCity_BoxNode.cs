using System;
using Framework;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIMainCity_BoxNode : UIBaseMainCityNode
	{
		public override int FunctionOpenID
		{
			get
			{
				return 1051;
			}
		}

		public override MainCityName Name
		{
			get
			{
				return MainCityName.Box;
			}
		}

		public override string RedName
		{
			get
			{
				return "Main.Box";
			}
		}

		public override int NameLanguageID
		{
			get
			{
				return 6403;
			}
		}

		public override string NameLanguageIDStr
		{
			get
			{
				return "";
			}
		}

		protected override void OnRedPointChange(RedNodeListenData obj)
		{
			if (this.m_redPoint == null)
			{
				return;
			}
			if (obj.m_count <= 1)
			{
				this.m_redPoint.SetType(240);
			}
			else
			{
				this.m_redPoint.SetType(100);
			}
			this.m_redPoint.Value = obj.m_count;
		}

		protected override void OnClickUnlockBt()
		{
			GameApp.View.OpenView(ViewName.MainCityBoxViewModule, null, 1, null, null);
		}
	}
}
