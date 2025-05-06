using System;
using Framework.Platfrom;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class TapToCloseCtrl : MonoBehaviour
	{
		private void Awake()
		{
			if (this.Button_Close != null)
			{
				this.Button_Close.onClick.AddListener(new UnityAction(this.InternalClick));
			}
			this.OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		private void InternalClick()
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		private void OnEnable()
		{
			this.onShow();
		}

		private async void onShow()
		{
			if (this.m_idleAni)
			{
				await TaskExpand.Yield();
				await TaskExpand.Yield();
				if (this.m_idleAni != null)
				{
					this.m_idleAni.enabled = true;
				}
			}
		}

		public void Show(bool value)
		{
			this.child.SetActive(value);
		}

		public GameObject child;

		public CustomButton Button_Close;

		public Animator m_idleAni;

		public Text Text_Content;

		public Action OnClose;
	}
}
