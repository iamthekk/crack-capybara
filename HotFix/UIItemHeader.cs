using System;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIItemHeader : UIItemBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetHeaderValue(string info)
		{
			if (this.m_headerTxt != null)
			{
				this.m_headerTxt.text = info;
			}
		}

		public void SetHeaderValue(long time)
		{
			if (this.m_headerTxt != null)
			{
				long num = time / 60L / 60L;
				this.m_headerTxt.text = Singleton<LanguageManager>.Instance.GetHours((int)num);
			}
		}

		public CustomText m_headerTxt;
	}
}
