using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix
{
	public class AttributeDetailedNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshData(AttributeDetailedViewModule.Data data, bool isShowBg)
		{
			if (data == null)
			{
				return;
			}
			if (this.m_bg != null)
			{
				this.m_bg.gameObject.SetActive(isShowBg);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = data.m_name;
			}
			if (this.m_toTxt != null)
			{
				this.m_toTxt.text = data.m_to;
			}
		}

		public Image m_bg;

		public CustomText m_nameTxt;

		public CustomText m_toTxt;
	}
}
