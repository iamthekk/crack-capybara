using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISettingLanguageCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_bt.m_onClick = new Action(this.OnClickBt);
		}

		protected override void OnDeInit()
		{
			this.m_bt.onClick.RemoveAllListeners();
			this.m_bt = null;
		}

		private void OnClickBt()
		{
			GameApp.View.OpenView(ViewName.LanguageChooseViewModule, null, 1, null, null);
		}

		public CustomButton m_bt;
	}
}
