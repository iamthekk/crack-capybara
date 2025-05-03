using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverBuffName : BaseHover
	{
		public bool IsPlayed { get; private set; }

		public override HoverType GetHoverType()
		{
			return HoverType.BuffName;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 1.5f, 0f));
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			this.data = this.hoverData as HoverStringData;
			if (this.data == null)
			{
				return;
			}
			this.text.text = "";
			this.IsPlayed = false;
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

		public void Play()
		{
			this.text.text = this.data.strParam;
			if (this.data.memberCamp == MemberCamp.Friendly)
			{
				if (this.animator != null)
				{
					this.animator.SetTrigger("Run");
				}
			}
			else if (this.animator != null)
			{
				this.animator.SetTrigger("EnemyRun");
			}
			this.IsPlayed = true;
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

		private HoverStringData data;

		private const float OffsetY = 1.5f;
	}
}
