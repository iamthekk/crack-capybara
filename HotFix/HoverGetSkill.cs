using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverGetSkill : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.GetSkill;
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
			HoverSkillBuildData hoverSkillBuildData = this.hoverData as HoverSkillBuildData;
			if (hoverSkillBuildData == null)
			{
				return;
			}
			this.text.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(153, new object[] { hoverSkillBuildData.data.skillName });
			this.item.Init();
			this.item.Refresh(hoverSkillBuildData.data);
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

		public UIGameEventSkillIconItem item;

		public Animator animator;

		public AnimatorListen listen;

		private const float OffsetY = 0.5f;
	}
}
