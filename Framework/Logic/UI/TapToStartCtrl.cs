using System;
using Framework.Platfrom;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class TapToStartCtrl : MonoBehaviour
	{
		private void Awake()
		{
			if (this.Button != null)
			{
				this.Button.onClick.AddListener(new UnityAction(this.InternalClick));
			}
			this.OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		private void InternalClick()
		{
			if (this.OnClick != null)
			{
				this.OnClick();
			}
		}

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
			this.hideText();
			this.onShow();
		}

		private async void onShow()
		{
			await TaskExpand.Yield();
			await TaskExpand.Yield();
			this.m_idleAni.enabled = true;
		}

		public void Show(bool value)
		{
			this.child.SetActive(value);
		}

		public GameObject child;

		public CustomButton Button;

		public Animator m_idleAni;

		public Text Text_Content;

		public Action OnClick;
	}
}
