using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class HoverBattleText : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.BattleText;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 0.2f, 0f));
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			HoverTextData hoverTextData = this.hoverData as HoverTextData;
			if (hoverTextData == null)
			{
				return;
			}
			if (hoverTextData.hoverTxetType == EHoverTextType.Default)
			{
				this.img.enabled = false;
				this.text.text = Singleton<LanguageManager>.Instance.GetInfoByID(hoverTextData.textId);
			}
			else
			{
				ArtHover_hoverText elementById = GameApp.Table.GetManager().GetArtHover_hoverTextModelInstance().GetElementById((int)hoverTextData.hoverTxetType);
				if (elementById == null)
				{
					HLog.LogError(string.Format("hoverText id:{0} is not exist", (int)hoverTextData.hoverTxetType));
					return;
				}
				this.text.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.textId);
				if (string.IsNullOrEmpty(elementById.iconName))
				{
					this.img.enabled = false;
				}
				else
				{
					this.img.enabled = true;
					this.img.SetImage(elementById.atlasId, elementById.iconName);
					LayoutRebuilder.ForceRebuildLayoutImmediate(this.text.transform.parent as RectTransform);
				}
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

		private const float OffsetY = 0.2f;
	}
}
