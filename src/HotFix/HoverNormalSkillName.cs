using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverNormalSkillName : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.NormalSkillName;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position);
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			HoverStringData hoverStringData = this.hoverData as HoverStringData;
			if (hoverStringData == null)
			{
				return;
			}
			this.text.text = hoverStringData.strParam;
			if (hoverStringData.memberCamp == MemberCamp.Friendly)
			{
				if (this.animator != null)
				{
					this.animator.SetTrigger("Run");
					return;
				}
			}
			else if (this.animator != null)
			{
				this.animator.SetTrigger("EnemyRun");
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
	}
}
