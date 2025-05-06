using System;

namespace HotFix
{
	public class PopCommonData
	{
		public string m_title = "";

		public string m_content = "";

		public string m_sureContent = "";

		public string m_cancelContent = "";

		public bool m_isShowClose = true;

		public bool m_isShowCancel;

		public bool m_isShowSure = true;

		public Action m_onSure;

		public Action m_onClose;

		public Action m_onCancel;

		public bool m_swappingButton;
	}
}
