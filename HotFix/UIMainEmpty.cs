using System;
using Framework;

namespace HotFix
{
	public class UIMainEmpty : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
		}

		protected override void OnHide()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnLanguageChange()
		{
		}
	}
}
