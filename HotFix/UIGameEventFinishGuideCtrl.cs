using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIGameEventFinishGuideCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.SetShow(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void CheckShow()
		{
			base.gameObject.SetActiveSafe(false);
		}

		public void SetShow(bool isShow)
		{
			base.gameObject.SetActiveSafe(isShow);
		}

		public Animator animator;
	}
}
