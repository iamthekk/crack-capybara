using System;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverPlusHp : BaseHover
	{
		protected override void OnDeInit()
		{
			if (this.listen != null)
			{
				this.listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public override HoverType GetHoverType()
		{
			return HoverType.PlusHp;
		}

		protected override void OnInit()
		{
			base.RefreshTargetPositionLessHp(base.target.transform.position);
			if (this.listen != null)
			{
				this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			if (this.animator != null)
			{
				this.animator.SetTrigger("Run");
			}
			HoverLongData hoverLongData = this.hoverData as HoverLongData;
			if (hoverLongData == null)
			{
				return;
			}
			string text = DxxTools.FormatNumber(Utility.Math.Abs(hoverLongData.param));
			this.text.text = "+" + text;
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
