using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIOneButton4MiniGame : CustomBehaviour
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
			this.textUp.gameObject.SetActiveSafe(false);
			this.textDown.gameObject.SetActiveSafe(true);
			this.animator.Play("DownIdle");
			this.maskAnimator.Play("DownIdle");
			if (this.loadingObj)
			{
				this.loadingObj.SetActiveSafe(true);
			}
			this.isClicked = false;
		}

		private void SetButtonUp()
		{
			this.textUp.gameObject.SetActiveSafe(true);
			this.textDown.gameObject.SetActiveSafe(false);
			this.animator.Play("UpIdle");
			this.maskAnimator.Play("UpIdle");
			if (this.loadingObj)
			{
				this.loadingObj.SetActiveSafe(false);
			}
			this.isClicked = false;
		}

		public void SetTextUp(string info)
		{
			this.textUp.text = info;
		}

		public void SetTextDown(string info)
		{
			this.textDown.text = info;
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

		private void OnButtonDown()
		{
			if (this.isLocked)
			{
				return;
			}
			if (this.isClicked)
			{
				return;
			}
			this.animator.Play("PressDown");
			this.maskAnimator.Play("PressDown");
		}

		private void OnButtonUp()
		{
			if (this.isLocked)
			{
				return;
			}
			if (this.isClicked)
			{
				return;
			}
			this.animator.Play("PressUp");
			this.maskAnimator.Play("PressUp");
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
