using System;
using UnityEngine;

namespace HotFix
{
	public class UIGuideStyleBattleHand : UIGuideStyleNormal
	{
		public override UIGuideStyleNormal SetStyle(GuideStyleData styledata, GameObject obj)
		{
			base.SetStyle(styledata, obj);
			this.Animator = this.ObjStyle.GetComponent<Animator>();
			if (this.Animator != null)
			{
				this.Animator.Play("GuideHand");
			}
			return this;
		}

		protected override void InitArgs()
		{
			this.mStyleOffset = Vector2.zero;
			string arg = this.StyleData.GetArg(1);
			string arg2 = this.StyleData.GetArg(2);
			if (!string.IsNullOrEmpty(arg) || !string.IsNullOrEmpty(arg2))
			{
				if (!float.TryParse(arg, out this.mStyleOffset.x))
				{
					this.mStyleOffset.x = 0f;
				}
				if (!float.TryParse(arg2, out this.mStyleOffset.y))
				{
					this.mStyleOffset.y = 0f;
				}
			}
		}

		public override void OnTargetAreaChange(Rect area)
		{
			Vector2 vector;
			vector..ctor(0f, 50f);
			this.RTFStyle.anchoredPosition = vector + this.mStyleOffset;
		}

		public Animator Animator;
	}
}
