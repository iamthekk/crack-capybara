using System;

namespace HotFix
{
	public class OtherMainCity_DefaultNode : BaseOtherMainCityNode
	{
		protected override void OnInit()
		{
			base.OnInit();
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.gameObject.SetActive(false);
			}
			if (this.m_lockBt != null)
			{
				this.m_lockBt.enabled = false;
			}
			if (this.m_unlockBt != null)
			{
				this.m_unlockBt.enabled = false;
			}
		}

		protected override void OnClickUnlockBt()
		{
		}

		protected override void OnClickLockBt()
		{
		}
	}
}
