using System;
using Framework.Platfrom;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class TextAlphaPingPong : MonoBehaviour
	{
		private void hideText()
		{
			if (this.Text_Content)
			{
				Color color = this.Text_Content.color;
				this.Text_Content.color = new Color(color.r, color.g, color.b, 0f);
			}
		}

		private void OnEnable()
		{
			if (this.m_child != null)
			{
				this.m_child.SetActive(true);
			}
			this.hideText();
			this.onShow();
		}

		private async void onShow()
		{
			await TaskExpand.Yield();
			this.m_idleAni.enabled = true;
		}

		public Text Text_Content;

		public Animator m_idleAni;

		public GameObject m_child;
	}
}
