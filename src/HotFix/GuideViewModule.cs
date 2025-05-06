using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Platfrom;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class GuideViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			if (this.ObjStyles != null)
			{
				this.ObjStyles.SetActive(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.OnInitByOpen();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.CurAdditional != null)
			{
				this.CurAdditional.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			DOTween.Kill(this, false);
		}

		public override void OnDelete()
		{
			if (this.Additional != null)
			{
				this.Additional.DeInit();
			}
			if (this.CurAdditional != null)
			{
				this.CurAdditional.DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guide_RefreshUI, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guide_HangGuideUI, new HandlerEvent(this.OnHangGuideUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guide_CloseGuideUI, new HandlerEvent(this.OnCloseGuideUI));
			GameApp.Event.RegisterEvent(LocalMessageName.Guide_Special_CompleteStep, new HandlerEvent(this.OnSpecialCompleteStep));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guide_RefreshUI, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guide_HangGuideUI, new HandlerEvent(this.OnHangGuideUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guide_CloseGuideUI, new HandlerEvent(this.OnCloseGuideUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.Guide_Special_CompleteStep, new HandlerEvent(this.OnSpecialCompleteStep));
		}

		private void OnCloseGuideUI(object sender, int type, BaseEventArgs eventArgs)
		{
			this.CloseSelf();
		}

		private void OnHangGuideUI(object sender, int type, BaseEventArgs eventArgs)
		{
			this.CloseSelf();
		}

		private void OnRefreshUI(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnInitByOpen();
		}

		private void OnSpecialCompleteStep(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.CurAdditional != null)
			{
				this.CurAdditional.OnSkipGuide = null;
			}
			this.OnCloseThis();
		}

		private async void OnInitByOpen()
		{
			if (!this.mInitializing)
			{
				this.mInitializing = true;
				if (this.CurAdditional != null)
				{
					Object.Destroy(this.CurAdditional);
					this.CurAdditional.DeInit();
					this.CurAdditional = null;
				}
				this.mCurGuide = null;
				GuideData guide = GuideController.Instance.CurrentGuide;
				if (guide == null)
				{
					HLog.LogError("[新手引导]当前没有引导，但是打开了界面！！！");
					this.OnCloseThis();
				}
				else
				{
					await TaskExpand.Delay(1);
					this.mCurGuide = guide;
					if (this.mCurGuide.DelayMSec > 0)
					{
						bool showGuideLog = GuideController.ShowGuideLog;
						await TaskExpand.Delay(this.mCurGuide.DelayMSec);
					}
					int tryfindcount = 3;
					while (tryfindcount > 0)
					{
						tryfindcount--;
						if (this.mCurGuide == null)
						{
							this.OnCloseThis();
							return;
						}
						this.mRTFTarget = GuideController.Instance.GetTarget(this.mCurGuide.GuideTargetName) as RectTransform;
						if (!(this.mRTFTarget == null))
						{
							break;
						}
						if (tryfindcount <= 0)
						{
							HLog.LogError(string.Format("[新手引导]未找到引导目标 引导被挂起 ID={0} 请检查目标类<color=#FF00FF>{1}</color> !", guide.ID, guide.GuideTargetName));
							if (this.mCurGuide != null)
							{
								GuideController.Instance.HangCurGuide(this.mCurGuide);
								this.mCurGuide = null;
							}
							this.OnCloseThis();
							return;
						}
						await TaskExpand.Delay(500);
					}
					if (this.mRTFTarget == null)
					{
						HLog.LogError(string.Format("[新手引导]未定义的引导目标 引导被挂起 ID={0} 请检查目标<color=#FF00FF>{1}</color> !", guide.ID, guide.GuideTargetName));
						if (this.mCurGuide != null)
						{
							GuideController.Instance.HangCurGuide(this.mCurGuide);
							this.mCurGuide = null;
						}
						this.OnCloseThis();
					}
					else if (!this.mRTFTarget.gameObject.activeSelf)
					{
						HLog.LogError(string.Format("[新手引导]引导目标 不可见 引导被挂起 ID={0} 请检查目标<color=#FF00FF>{1}</color> !", guide.ID, guide.GuideTargetName));
						if (this.mCurGuide != null)
						{
							GuideController.Instance.HangCurGuide(this.mCurGuide);
							this.mCurGuide = null;
						}
						this.OnCloseThis();
					}
					else
					{
						Transform transform = base.transform;
						if (transform == null)
						{
							HLog.LogError(string.Format("[新手引导]引导面板已关闭  ID={0} 请检查目标<color=#FF00FF>{1}</color> !", guide.ID, guide.GuideTargetName));
							if (this.mCurGuide != null)
							{
								GuideController.Instance.HangCurGuide(this.mCurGuide);
								this.mCurGuide = null;
							}
							this.mInitializing = false;
						}
						else
						{
							BaseViewModule targetView = this.GetTargetView(this.mRTFTarget);
							if (targetView != null)
							{
								if (guide.SettedViewOrder != -1)
								{
									transform = base.transform;
									Canvas component = base.GetComponent<Canvas>();
									if (component)
									{
										component.sortingOrder = guide.SettedViewOrder;
									}
								}
								else
								{
									transform = targetView.transform;
								}
							}
							else
							{
								bool showGuideLog2 = GuideController.ShowGuideLog;
							}
							this.CurAdditional = Object.Instantiate<UIGuideAdditionalCtrl>(this.Additional, transform);
							this.CurAdditional.name = "Guide_Additional";
							this.CurAdditional.Init();
							this.CurAdditional.SetGuideData(guide);
							this.CurAdditional.SetGuideTarget(this.mRTFTarget);
							this.CurAdditional.OnSkipGuide = new Action<UIGuideAdditionalCtrl, GuideSkipKind>(this.OnSkipGuideByAdditional);
							this.CurAdditional.RefreshUI();
							List<GuideStyleData> styles = guide.Styles;
							Transform styleNode = this.CurAdditional.StyleNode;
							bool flag = false;
							if (styles != null)
							{
								for (int i = 0; i < styles.Count; i++)
								{
									GuideStyleData guideStyleData = styles[i];
									if (guideStyleData != null && guideStyleData.StyleKind != GuideStyleKind.None)
									{
										switch (guideStyleData.StyleKind)
										{
										case GuideStyleKind.Arrow:
										{
											GameObject gameObject = Object.Instantiate<GameObject>(this.StyleArrow, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleNormal().SetStyle(guideStyleData, gameObject));
											break;
										}
										case GuideStyleKind.Hand:
										{
											GameObject gameObject2 = Object.Instantiate<GameObject>(this.StyleHand, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleNormal().SetStyle(guideStyleData, gameObject2));
											break;
										}
										case GuideStyleKind.Circle:
										{
											GameObject gameObject3 = Object.Instantiate<GameObject>(this.StyleCircel, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleNormal().SetStyle(guideStyleData, gameObject3));
											break;
										}
										case GuideStyleKind.Tips:
										{
											GameObject gameObject4 = Object.Instantiate<GameObject>(this.TFTips.gameObject, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleTips().SetStyle(guideStyleData, gameObject4));
											break;
										}
										case GuideStyleKind.GrayMask:
											flag = true;
											break;
										case GuideStyleKind.BattleHand:
										{
											GameObject gameObject5 = Object.Instantiate<GameObject>(this.BattleHand, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleBattleHand().SetStyle(guideStyleData, gameObject5));
											break;
										}
										case GuideStyleKind.BattleTips:
										{
											GameObject gameObject6 = Object.Instantiate<GameObject>(this.BattleTips, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleBattleTips().SetStyle(guideStyleData, gameObject6));
											break;
										}
										case GuideStyleKind.HandDrag:
										{
											GameObject gameObject7 = Object.Instantiate<GameObject>(this.StyleHand, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleNormal().SetStyle(guideStyleData, gameObject7));
											break;
										}
										case GuideStyleKind.HandDragOffset:
										{
											GameObject gameObject8 = Object.Instantiate<GameObject>(this.StyleHand, styleNode);
											this.CurAdditional.AddStyleUI(new UIGuideStyleNormal().SetStyle(guideStyleData, gameObject8));
											break;
										}
										}
									}
								}
							}
							this.CurAdditional.SetShowGrayMask(flag);
							this.mRTFTargetDragEnd = null;
							this.OnCheckTargetMove = null;
							this.TargetStartPos = ((this.mRTFTarget != null) ? this.mRTFTarget.position : Vector3.zero);
							EventArgsLong eventArgsLong = new EventArgsLong();
							eventArgsLong.SetData((long)guide.ID);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_GuideUIActive, eventArgsLong);
							bool showGuideLog3 = GuideController.ShowGuideLog;
							this.CurAdditional.ForceUpdatePosition();
							GameApp.SDK.Analyze.Track_Guide(guide.ID, 0);
							this.mInitializing = false;
						}
					}
				}
			}
		}

		private void OnSkipGuideByAdditional(UIGuideAdditionalCtrl ui, GuideSkipKind kind)
		{
			if (ui != null)
			{
				ui.OnSkipGuide = null;
				ui.ClearBind();
				if (ui == this.CurAdditional)
				{
					this.CurAdditional = null;
				}
				Object.Destroy(ui.gameObject);
			}
			GuideData curGuide = ui.CurGuide;
			if (curGuide == null)
			{
				if (this.mCurGuide == null)
				{
					this.OnCloseThis();
				}
				return;
			}
			if (kind <= GuideSkipKind.TargetLost)
			{
				if (kind == GuideSkipKind.TargetOutOfScreen)
				{
					GuideController.Instance.HangCurGuide(curGuide);
					goto IL_00E6;
				}
				if (kind == GuideSkipKind.TargetLost)
				{
					GuideController.Instance.HangCurGuide(curGuide);
					goto IL_00E6;
				}
			}
			else
			{
				if (kind == GuideSkipKind.UserSkip)
				{
					EventArgGuideOver eventArgGuideOver = new EventArgGuideOver();
					eventArgGuideOver.SetData(curGuide.Group, curGuide.ID, true);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_GuideOver, eventArgGuideOver);
					goto IL_00E6;
				}
				if (kind == GuideSkipKind.AdditionalUIDestroy)
				{
					GuideController.Instance.HangCurGuide(curGuide);
					goto IL_00E6;
				}
			}
			EventArgGuideOver eventArgGuideOver2 = new EventArgGuideOver();
			eventArgGuideOver2.SetData(curGuide.Group, curGuide.ID, false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_GuideOver, eventArgGuideOver2);
			IL_00E6:
			if (curGuide == this.mCurGuide && GuideController.Instance.IsGuideGroupHang(this.mCurGuide.Group))
			{
				this.mCurGuide = null;
				this.OnCloseThis();
			}
		}

		private BaseViewModule GetTargetView(Transform targettf)
		{
			if (targettf == null)
			{
				return null;
			}
			Stack<Transform> stack = new Stack<Transform>();
			Transform transform = targettf;
			stack.Push(transform);
			while (transform.parent != null)
			{
				transform = transform.parent;
				stack.Push(transform);
			}
			BaseViewModule baseViewModule = null;
			while (baseViewModule == null)
			{
				transform = stack.Pop();
				baseViewModule = transform.GetComponent<BaseViewModule>();
				if (stack.Count <= 0)
				{
					break;
				}
			}
			return baseViewModule;
		}

		private Vector3 GetDragStartPos()
		{
			if (this.mRTFTarget != null)
			{
				return this.mRTFTarget.position;
			}
			return Vector3.zero;
		}

		private Vector3 GetDragEndPos()
		{
			if (this.mRTFTargetDragEnd != null)
			{
				return this.mRTFTargetDragEnd.position;
			}
			return this.GetDragStartPos();
		}

		private void OnCloseThis()
		{
			this.mInitializing = false;
			if (GameApp.View.IsOpened(ViewName.GuideViewModule))
			{
				GameApp.View.CloseView(ViewName.GuideViewModule, null);
			}
			if (this.mCurGuide != null)
			{
				EventArgGuideOver eventArgGuideOver = new EventArgGuideOver();
				eventArgGuideOver.SetData(this.mCurGuide.Group, this.mCurGuide.ID, false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_GuideOver, eventArgGuideOver);
			}
		}

		private void CloseSelf()
		{
			this.mInitializing = false;
			if (GameApp.View.IsOpened(ViewName.GuideViewModule))
			{
				GameApp.View.CloseView(ViewName.GuideViewModule, null);
			}
		}

		public GameObject ObjStyles;

		public GameObject StyleArrow;

		public GameObject StyleHand;

		public GameObject StyleCircel;

		public GameObject BattleHand;

		public GameObject BattleTips;

		public UIGuideTipsCtrl TFTips;

		public UIGuideAdditionalCtrl Additional;

		public UIGuideAdditionalCtrl CurAdditional;

		private BaseViewModule mViewTarget;

		private RectTransform mRTFTarget;

		private RectTransform mRTFTargetDragEnd;

		private Vector3 mDragOffset;

		private Dictionary<int, GameObject> m_guideStyles = new Dictionary<int, GameObject>();

		private GuideData mCurGuide;

		private Vector3 TargetStartPos;

		private Action OnCheckTargetMove;

		private bool mInitializing;
	}
}
