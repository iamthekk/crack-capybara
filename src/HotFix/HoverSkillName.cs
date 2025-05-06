using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverSkillName : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.SkillName;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position);
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			HoverLongData hoverLongData = this.hoverData as HoverLongData;
			if (hoverLongData == null)
			{
				return;
			}
			int num = (int)hoverLongData.param;
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(num);
			if (elementById == null)
			{
				HLog.LogError(string.Format("skillId:{0} is not exist in Table", num));
				return;
			}
			this.text.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.img.enabled = false;
			if (!string.IsNullOrEmpty(elementById.icon))
			{
				this.img.onFinished.RemoveAllListeners();
				this.img.onFinished.AddListener(delegate(string atlasPath, string spriteName)
				{
					this.img.enabled = true;
				});
				this.img.SetImage(elementById.iconAtlasID, elementById.icon);
			}
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

		public CustomImage img;

		public CustomText text;

		public Animator animator;

		public AnimatorListen listen;
	}
}
