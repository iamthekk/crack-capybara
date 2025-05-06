using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIGuideAdditionalCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ButtonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.mObjStyleNode = this.StyleNode.gameObject;
		}

		protected override void OnDeInit()
		{
			this.ButtonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.UnLockTargetScroll();
			if (this.OnSkipGuide != null)
			{
				this.OnSkipGuide(this, GuideSkipKind.AdditionalUIDestroy);
				this.OnSkipGuide = null;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mSecOfAreaOutView >= 0.0)
			{
				this.mSecOfAreaOutView += (double)Time.deltaTime;
				if (this.AreaMask != null && !this.AreaMask.IsAreaOutOfView)
				{
					this.mSecOfAreaOutView = -999999.0;
					this.mObjStyleNode.SetActive(true);
				}
				if (this.mSecOfAreaOutView >= 0.2)
				{
					Action<UIGuideAdditionalCtrl, GuideSkipKind> onSkipGuide = this.OnSkipGuide;
					if (onSkipGuide == null)
					{
						return;
					}
					onSkipGuide(this, GuideSkipKind.TargetOutOfScreen);
				}
			}
		}

		public void OnClickClose()
		{
			Action<UIGuideAdditionalCtrl, GuideSkipKind> onSkipGuide = this.OnSkipGuide;
			if (onSkipGuide == null)
			{
				return;
			}
			onSkipGuide(this, GuideSkipKind.UserSkip);
		}

		public void SetGuideData(GuideData data)
		{
			this.CurGuide = data;
		}

		public void SetGuideTarget(Transform target)
		{
			this.Target = target;
			this.TargetEventBind();
		}

		private void TargetEventBind()
		{
			if (this.Target == null)
			{
				this.ClearBind();
				return;
			}
			List<GuideCompleteData> completeList = this.CurGuide.CompleteList;
			bool flag = false;
			for (int i = 0; i < completeList.Count; i++)
			{
				GuideCompleteData guideCompleteData = completeList[i];
				if (guideCompleteData != null)
				{
					GuideCompleteKind guideKind = guideCompleteData.GuideKind;
					if (guideKind <= GuideCompleteKind.ViewClose)
					{
						switch (guideKind)
						{
						case GuideCompleteKind.ButtonClick:
							flag = true;
							break;
						case GuideCompleteKind.ButtonDown:
							flag = true;
							break;
						case GuideCompleteKind.ButtonUp:
							flag = true;
							break;
						default:
							if (guideKind != GuideCompleteKind.ViewClose)
							{
							}
							break;
						}
					}
					else if (guideKind != GuideCompleteKind.ViewOpen && guideKind != GuideCompleteKind.TargetPositionChange && guideKind != GuideCompleteKind.SpecialString)
					{
					}
				}
			}
			if (flag)
			{
				this.mTargetBtnCtrl = this.Target.GetComponent<CustomButton>();
				this.mTargetBtn = this.Target.GetComponent<Button>();
				this.BindTargetClick();
			}
		}

		public void AddStyleUI(UIGuideStyleNormal styleui)
		{
			this.StyleUIList.Add(styleui);
		}

		private void BindTargetClick()
		{
			if (this.CurGuide.CompleteList != null)
			{
				List<GuideCompleteData> completeList = this.CurGuide.CompleteList;
				for (int i = 0; i < completeList.Count; i++)
				{
					switch (completeList[i].GuideKind)
					{
					case GuideCompleteKind.ButtonClick:
						if (this.mTargetBtnCtrl != null)
						{
							this.mTargetBtnCtrl.onClick.RemoveListener(new UnityAction(this.OnTargetClick));
							this.mTargetBtnCtrl.onClick.AddListener(new UnityAction(this.OnTargetClick));
						}
						if (this.mTargetBtn != null)
						{
							this.mTargetBtn.onClick.RemoveListener(new UnityAction(this.OnTargetClick));
							this.mTargetBtn.onClick.AddListener(new UnityAction(this.OnTargetClick));
						}
						break;
					case GuideCompleteKind.ButtonDown:
						if (this.mTargetBtnCtrl != null)
						{
							CustomButton customButton = this.mTargetBtnCtrl;
							customButton.onDown = (Action)Delegate.Remove(customButton.onDown, new Action(this.OnTargetClick));
							CustomButton customButton2 = this.mTargetBtnCtrl;
							customButton2.onDown = (Action)Delegate.Combine(customButton2.onDown, new Action(this.OnTargetClick));
						}
						break;
					case GuideCompleteKind.ButtonUp:
						if (this.mTargetBtnCtrl != null)
						{
							CustomButton customButton3 = this.mTargetBtnCtrl;
							customButton3.onUp = (Action)Delegate.Remove(customButton3.onUp, new Action(this.OnTargetClick));
							CustomButton customButton4 = this.mTargetBtnCtrl;
							customButton4.onUp = (Action)Delegate.Combine(customButton4.onUp, new Action(this.OnTargetClick));
						}
						break;
					}
				}
			}
		}

		private void OnTargetClick()
		{
			GuideData curGuide = this.CurGuide;
			Action<UIGuideAdditionalCtrl, GuideSkipKind> onSkipGuide = this.OnSkipGuide;
			if (onSkipGuide == null)
			{
				return;
			}
			onSkipGuide(this, GuideSkipKind.GuideComplete);
		}

		public void ClearBind()
		{
			this.UnBindTargetClick();
			this.mTargetBtn = null;
			this.mTargetBtnCtrl = null;
		}

		private void BindTargetMove()
		{
			if (this.CurGuide.CompleteList != null)
			{
				List<GuideCompleteData> completeList = this.CurGuide.CompleteList;
				bool flag = false;
				for (int i = 0; i < completeList.Count; i++)
				{
					if (completeList[i].GuideKind == GuideCompleteKind.TargetPositionChange)
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
			}
		}

		private void BindFullScreenClick()
		{
		}

		private void UnBindTargetClick()
		{
			if (this.mTargetBtnCtrl != null)
			{
				this.mTargetBtnCtrl.onClick.RemoveListener(new UnityAction(this.OnTargetClick));
				CustomButton customButton = this.mTargetBtnCtrl;
				customButton.onDown = (Action)Delegate.Remove(customButton.onDown, new Action(this.OnTargetClick));
				CustomButton customButton2 = this.mTargetBtnCtrl;
				customButton2.onUp = (Action)Delegate.Remove(customButton2.onUp, new Action(this.OnTargetClick));
			}
			if (this.mTargetBtn != null)
			{
				this.mTargetBtn.onClick.RemoveListener(new UnityAction(this.OnTargetClick));
			}
		}

		public void RefreshUI()
		{
			if (this.CurGuide == null || this.Target == null)
			{
				return;
			}
			this.mSecOfAreaOutView = -999999.0;
			this.AreaMask.TargetArea = this.Target as RectTransform;
			this.AreaMask.CalcClickRect();
			this.AreaMask.OnHideInHierarchy = new Action(this.OnHideInHierarchy);
			this.AreaMask.Image.raycastTarget = this.CurGuide.GuideFullScreen;
			this.AreaMask.OnTargetPosChange = new Action(this.OnTargetPosChange);
			this.AreaMask.OnAreaOutOfView = new Action(this.OnAreaOutOfView);
			if (this.CurGuide.AutoLockScroll)
			{
				this.LockTargetScroll();
			}
			base.gameObject.SetActive(true);
			this.HoldMask.SetActive(false);
		}

		private void OnTargetPosChange()
		{
			for (int i = 0; i < this.StyleUIList.Count; i++)
			{
				this.StyleUIList[i].OnTargetAreaChange(this.AreaMask.AreaRect);
			}
			if (!this.AreaMask.IsAreaOutOfView)
			{
				this.mObjStyleNode.SetActive(true);
			}
		}

		public void ForceUpdatePosition()
		{
			this.AreaMask.CalcClickRect();
		}

		private void OnHideInHierarchy()
		{
			if (this.Target != null)
			{
				if (this.AreaMask != null)
				{
					this.AreaMask.OnHideInHierarchy = null;
				}
				if (this.OnSkipGuide != null)
				{
					this.OnSkipGuide(this, GuideSkipKind.TargetLost);
					return;
				}
			}
			else if (this.OnSkipGuide != null)
			{
				this.OnSkipGuide(this, GuideSkipKind.TargetLost);
			}
		}

		private void OnAreaOutOfView()
		{
			if (this.mSecOfAreaOutView < 0.0)
			{
				this.mSecOfAreaOutView = 0.0;
				this.StyleNode.gameObject.SetActive(false);
			}
		}

		private void LockTargetScroll()
		{
			if (this.mLockedScroll != null)
			{
				this.UnLockTargetScroll();
			}
			if (this.Target != null)
			{
				this.mLockedScroll = this.Target.GetComponentsInParent<ScrollRect>();
				if (this.mLockedScroll != null)
				{
					for (int i = 0; i < this.mLockedScroll.Length; i++)
					{
						ScrollRect scrollRect = this.mLockedScroll[i];
						if (scrollRect != null)
						{
							scrollRect.enabled = false;
						}
					}
				}
			}
		}

		private void UnLockTargetScroll()
		{
			if (this.mLockedScroll != null)
			{
				for (int i = 0; i < this.mLockedScroll.Length; i++)
				{
					ScrollRect scrollRect = this.mLockedScroll[i];
					if (scrollRect != null)
					{
						scrollRect.enabled = true;
					}
				}
			}
			this.mLockedScroll = null;
		}

		public void SetShowGrayMask(bool showgraymask)
		{
			Color color = (showgraymask ? new Color(0f, 0f, 0f, 0.0001f) : new Color(0f, 0f, 0f, 0f));
			this.AreaMask.Image.color = color;
		}

		public CustomButton ButtonClose;

		public ImageAreaMask AreaMask;

		public GameObject HoldMask;

		public Transform StyleNode;

		private GameObject mObjStyleNode;

		public List<UIGuideStyleNormal> StyleUIList = new List<UIGuideStyleNormal>();

		public Action<UIGuideAdditionalCtrl, GuideSkipKind> OnSkipGuide;

		public GuideData CurGuide;

		public Transform Target;

		private Button mTargetBtn;

		private CustomButton mTargetBtnCtrl;

		private double mSecOfAreaOutView;

		public const double SecOfAreaOutView_Max = 0.2;

		private ScrollRect[] mLockedScroll;
	}
}
