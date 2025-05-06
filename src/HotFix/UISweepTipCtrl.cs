using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UISweepTipCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mIsAutoHide)
			{
				this.timer += deltaTime;
				if (this.timer >= 2f)
				{
					this.mIsAutoHide = false;
					this.timer = 0f;
					this.HideAni();
				}
			}
		}

		public void SetData(int rate, bool isMax)
		{
			if (isMax)
			{
				this.imageTitle.sprite = this.spriteRegister.GetSprite("purple");
			}
			else
			{
				this.imageTitle.sprite = this.spriteRegister.GetSprite("yellow");
			}
			this.textRate.text = string.Format("x{0}", rate);
		}

		public void SetShow(bool isShow)
		{
			this.child.SetActive(isShow);
			this.ShowAni(true);
		}

		public void ShowAni(bool isAutoHide)
		{
			this.timer = 0f;
			this.mIsAutoHide = isAutoHide;
			this.animator.Play("Show");
		}

		public void HideAni()
		{
			this.animator.Play("Hide");
		}

		public CustomImage imageTitle;

		public CustomText textRate;

		public SpriteRegister spriteRegister;

		public GameObject child;

		public Animator animator;

		private bool mIsAutoHide;

		private float timer;
	}
}
