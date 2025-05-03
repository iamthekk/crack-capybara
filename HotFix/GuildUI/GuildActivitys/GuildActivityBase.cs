using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI.GuildActivitys
{
	public class GuildActivityBase : GuildProxy.GuildProxy_BaseBehaviour
	{
		public RectTransform RTF
		{
			get
			{
				if (this.mRTF == null)
				{
					this.mRTF = base.gameObject.transform as RectTransform;
				}
				return this.mRTF;
			}
		}

		public CanvasGroup CanvasGroup
		{
			get
			{
				if (this.mCanvasGroup == null)
				{
					this.mCanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
				}
				return this.mCanvasGroup;
			}
		}

		protected override void GuildUI_OnInit()
		{
			if (this.Button != null)
			{
				this.Button.onClick.AddListener(new UnityAction(this.OnClickThis));
			}
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.Button != null)
			{
				this.Button.onClick.RemoveListener(new UnityAction(this.OnClickThis));
			}
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public virtual void RefreshUIOnOpen()
		{
		}

		protected virtual void OnClickThis()
		{
			if (this.RedNode != null && !string.IsNullOrEmpty(this.RedNode.Key))
			{
				GuildProxy.RedPoint.ClickRedPoint(this.RedNode.Key);
			}
		}

		public RedNodeOneCtrl RedNode;

		public CustomButton Button;

		private RectTransform mRTF;

		private CanvasGroup mCanvasGroup;
	}
}
