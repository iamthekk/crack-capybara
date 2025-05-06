using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverEmoticons : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.Emoticons;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 0.5f, 0f));
			if (this.listen != null)
			{
				this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			if (this.animator != null)
			{
				this.animator.SetTrigger("Run");
			}
			HoverStringData hoverStringData = this.hoverData as HoverStringData;
			if (hoverStringData == null)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(103);
			this.icon.SetImage(atlasPath, hoverStringData.strParam);
		}

		protected override void OnDeInit()
		{
			if (this.listen != null)
			{
				this.listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnAnimatorListen(GameObject obj, string arg)
		{
			if (arg == "End")
			{
				base.RemoveHover();
			}
		}

		public CustomImage icon;

		public Animator animator;

		public AnimatorListen listen;

		private const float OffsetY = 0.5f;
	}
}
