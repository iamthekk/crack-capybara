using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class HeroLevelupAttributeController : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void PlayAnimator(string trigger)
		{
			if (this.m_animator == null)
			{
				return;
			}
			this.m_animator.SetTrigger(trigger);
		}

		public void SetNameTxt(string info)
		{
			if (this.m_name == null)
			{
				HLog.LogError("HeroLevelupAttributeController SetNameTxt info = " + info + "  m_name is null");
				return;
			}
			this.m_name.text = info;
		}

		public void SetFromTxt(string info)
		{
			if (this.m_fromTxt == null)
			{
				return;
			}
			this.m_fromTxt.text = info;
		}

		public void SetToTxt(string info)
		{
			if (this.m_toTxt == null)
			{
				return;
			}
			this.m_toTxt.text = info;
		}

		public Animator m_animator;

		public CustomText m_name;

		public CustomText m_fromTxt;

		public CustomText m_toTxt;
	}
}
