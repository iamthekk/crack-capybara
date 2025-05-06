using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIOneButtonCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
			this.button.onDown = new Action(this.OnButtonDown);
			this.button.onUp = new Action(this.OnButtonUp);
			this.SetButtonUp();
			this.textDown.gameObject.SetActiveSafe(false);
			this.isLocked = false;
			this.maskAnimator.Play("UpIdle");
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			this.isLocked = false;
		}

		public void SetData(Action onClick)
		{
			this.onClickButton = onClick;
		}

		public void SetLock(bool isLock)
		{
			if (this.isLocked == isLock)
			{
				return;
			}
			this.isLocked = isLock;
			if (this.isLocked)
			{
				this.SetButtonDown();
				return;
			}
			this.SetButtonUp();
		}

		private void SetButtonDown()
		{
			this.PlayerAnim("DownIdle");
			this.ShowLoading(true);
			this.isClicked = false;
		}

		private void SetButtonUp()
		{
			this.PlayerAnim("UpIdle");
			this.ShowLoading(false);
			this.isClicked = false;
		}

		public void ShowLoading(bool isShow)
		{
			if (this.loadingObj)
			{
				this.loadingObj.SetActiveSafe(isShow);
			}
		}

		public void SetText(string info)
		{
			this.textUp.text = info;
			this.textDown.text = info;
		}

		public void SetText(string infoUp, string infoDown)
		{
			this.textUp.text = infoUp;
			this.textDown.text = infoDown;
		}

		private void OnClickButton()
		{
			if (this.isLocked)
			{
				return;
			}
			this.isClicked = true;
			Action action = this.onClickButton;
			if (action == null)
			{
				return;
			}
			action();
		}

		protected virtual void OnButtonDown()
		{
			if (this.isLocked)
			{
				return;
			}
			if (this.isClicked)
			{
				return;
			}
			this.PlayerAnim("PressDown");
		}

		protected virtual void OnButtonUp()
		{
			if (this.isLocked)
			{
				return;
			}
			if (this.isClicked)
			{
				return;
			}
			this.PlayerAnim("PressUp");
		}

		private void PlayerAnim(string aniName)
		{
			this.animator.StopPlayback();
			this.maskAnimator.StopPlayback();
			this.animator.Play(aniName, -1, 0f);
			this.maskAnimator.Play(aniName, -1, 0f);
		}

		public bool IsUnLock()
		{
			return !this.isLocked;
		}

		public CustomButton button;

		public CustomText textUp;

		public CustomText textDown;

		public GameObject mask;

		public Animator animator;

		public GameObject loadingObj;

		public Animator maskAnimator;

		protected Action onClickButton;

		protected bool isLocked;

		private bool isClicked;

		private const string DownIdle = "DownIdle";

		private const string UpIdle = "UpIdle";

		private const string PressDown = "PressDown";

		private const string PressUp = "PressUp";
	}
}
