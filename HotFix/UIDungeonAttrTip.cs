using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIDungeonAttrTip : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(string langId)
		{
			this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID(langId);
		}

		public CustomText textTip;
	}
}
