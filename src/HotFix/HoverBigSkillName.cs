using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverBigSkillName : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.BigSkillName;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 1.5f, 0f));
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			HoverStringData hoverStringData = this.hoverData as HoverStringData;
			if (hoverStringData == null)
			{
				return;
			}
			this.text.text = hoverStringData.strParam;
			if (this.animator != null)
			{
				this.animator.SetTrigger("Run");
			}
		}

		protected override void OnDeInit()
		{
			if (this.listen != null)
			{
				this.listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaleDeltaTime)
		{
		}

		private void OnAnimatorListen(GameObject obj, string arg)
		{
			if (arg == "End")
			{
				base.RemoveHover();
			}
		}

		public CustomText text;

		public Animator animator;

		public AnimatorListen listen;

		private const float OffsetY = 1.5f;
	}
}
