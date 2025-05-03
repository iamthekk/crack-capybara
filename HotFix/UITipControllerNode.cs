using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UITipControllerNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_listen != null)
			{
				this.m_listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_listen != null)
			{
				this.m_listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public void SetInfo(string info)
		{
			if (this.m_text == null)
			{
				return;
			}
			this.m_text.text = info;
			LayoutRebuilder.MarkLayoutForRebuild(this.m_text.rectTransform);
			if (this.m_root == null)
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_root);
		}

		private void OnAnimatorListen(GameObject gameObject, string eventParameter)
		{
			if (string.Equals(eventParameter, "End") && this.m_onFinished != null)
			{
				this.m_onFinished(this);
			}
		}

		[SerializeField]
		private AnimatorListen m_listen;

		[SerializeField]
		private CustomText m_text;

		[SerializeField]
		private RectTransform m_root;

		public Action<UITipControllerNode> m_onFinished;
	}
}
