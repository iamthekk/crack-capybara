using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TextTipNode : CustomBehaviour
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
		}

		private void OnAnimatorListen(GameObject gameObject, string eventParameter)
		{
			if (string.Equals(eventParameter, "End"))
			{
				Action<TextTipNode> onFinished = this.m_onFinished;
				if (onFinished == null)
				{
					return;
				}
				onFinished(this);
			}
		}

		public AnimatorListen m_listen;

		public CustomText m_text;

		public Action<TextTipNode> m_onFinished;
	}
}
