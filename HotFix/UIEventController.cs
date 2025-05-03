using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HotFix
{
	public class UIEventController : CustomBehaviour
	{
		protected override void OnInit()
		{
			RectTransform component = this.scrollRect.GetComponent<RectTransform>();
			this.initY = component.sizeDelta.y;
			if (Utility.UI.ScreenRatio > Utility.UI.DesignRatio)
			{
				float num = (Utility.UI.ScreenRatio - Utility.UI.DesignRatio + 1f) * this.initY + 20f;
				component.sizeDelta = new Vector2(component.sizeDelta.x, num);
			}
			this.buttonsCtrl.Init();
			this.bonusCtrl.Init();
			this.textEventTipColor = this.textEventTip.color;
			this.eventItemObj.SetActiveSafe(false);
			this.eventLoading.SetActiveSafe(false);
			this.imageLoading.fillAmount = 0f;
			this.imageLoadingBigWin.fillAmount = 0f;
			this.height = 0f;
			this.m_currentEvent = null;
			this.viewModule = GameApp.View.GetViewModule(ViewName.GameEventViewModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEventLoading, new HandlerEvent(this.OnEventShowLoading));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEventTipInfo, new HandlerEvent(this.OnEventShowEventTipInfo));
			ScrollRectBase scrollRectBase = this.scrollRect;
			scrollRectBase.BeginDrag = (Action<PointerEventData>)Delegate.Combine(scrollRectBase.BeginDrag, new Action<PointerEventData>(this.OnDragStart));
			ScrollRectBase scrollRectBase2 = this.scrollRect;
			scrollRectBase2.EndDrag = (Action<PointerEventData>)Delegate.Combine(scrollRectBase2.EndDrag, new Action<PointerEventData>(this.OnDragEnd));
			this.imagFly.gameObject.SetActiveSafe(false);
			this.nextDayAniListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnWinAniFinish));
			LoopListViewInitParam loopListViewInitParam = LoopListViewInitParam.CopyDefaultInitParam();
			loopListViewInitParam.mSmoothDumpRate = 5f;
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), loopListViewInitParam);
			this.loopListView.ScrollRect.horizontal = false;
			this.loopListView.ScrollRect.vertical = true;
			this.contentRectTrans = this.loopListView.ScrollRect.content.GetComponent<RectTransform>();
			this.scrollHeight = this.loopListView.ScrollRect.GetComponent<RectTransform>().sizeDelta.y;
			this.scrollSeq = this.sequencePool.Get();
			for (int i = 0; i < this.helpers.Length; i++)
			{
				this.helpers[i].Init();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.buttonsCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.isScrollUpdate)
			{
				this.time += deltaTime;
				float num = Utility.Math.Clamp01(this.time / 0.2f);
				this.scrollRect.verticalNormalizedPosition = this.scrollStart * (1f - num);
				if (num >= 1f)
				{
					this.isScrollUpdate = false;
					this.scrollRect.vertical = true;
				}
			}
		}

		protected override void OnDeInit()
		{
			this.buttonsCtrl.DeInit();
			this.bonusCtrl.DeInit();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEventLoading, new HandlerEvent(this.OnEventShowLoading));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEventTipInfo, new HandlerEvent(this.OnEventShowEventTipInfo));
			ScrollRectBase scrollRectBase = this.scrollRect;
			scrollRectBase.BeginDrag = (Action<PointerEventData>)Delegate.Remove(scrollRectBase.BeginDrag, new Action<PointerEventData>(this.OnDragStart));
			ScrollRectBase scrollRectBase2 = this.scrollRect;
			scrollRectBase2.EndDrag = (Action<PointerEventData>)Delegate.Remove(scrollRectBase2.EndDrag, new Action<PointerEventData>(this.OnDragEnd));
			this.nextDayAniListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnWinAniFinish));
			for (int i = 0; i < this.cacheFlyList.Count; i++)
			{
				GameObject gameObject = this.cacheFlyList[i];
				if (gameObject)
				{
					Object.Destroy(gameObject);
				}
			}
			this.cacheFlyList.Clear();
			this.uiDataList.Clear();
			this.itemDic.Clear();
			for (int j = 0; j < this.helpers.Length; j++)
			{
				this.helpers[j].DeInit();
			}
		}

		public void EventStart()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			ChapterData currentChapter = dataModule.CurrentChapter;
			this.bonusCtrl.SetData(currentChapter.BigBonus, currentChapter.BigBonusPlayCount, currentChapter.MinorBonus, currentChapter.MinorGameId, dataModule.RandomSeed, currentChapter.defaultBgm, new Action(this.OnCloseBonus));
			this.isShowBigBonus = currentChapter.BigBonus > 0;
			this.isShowMinorBonus = currentChapter.MinorBonus > 0;
		}

		public void AddEvent(GameEventUIData uiData)
		{
			if (this.bonusCtrl.IsShowBonus)
			{
				this.cacheUIList.Add(uiData);
				return;
			}
			this.currentUIData = uiData;
			if (this.m_currentEvent != null)
			{
				this.m_currentEvent.CloseInfo();
			}
			this.uiDataList.Add(uiData);
			this.loopListView.SetListItemCount(this.uiDataList.Count, false);
			if (this.previousIndex > 0 && this.loopListView.GetShownItemByIndex(this.previousIndex) == null)
			{
				this.AddEventAni();
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.uiDataList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGameEventItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGameEventItem uigameEventItem = this.GetUIItem(instanceID);
			if (uigameEventItem == null)
			{
				uigameEventItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			bool flag = this.previousIndex < index;
			uigameEventItem.Refresh(this.uiDataList[index], this, index, flag);
			if (this.previousIndex < index)
			{
				this.previousIndex = index;
				this.m_currentEvent = uigameEventItem;
				this.loopListView.OnItemSizeChanged(index);
				float num = this.m_currentEvent.GetHeight();
				this.totalHeight = this.loopListView.ScrollRect.content.sizeDelta.y;
				this.totalHeight += num + loopListViewItem.Padding;
				this.AddEventAni();
				uigameEventItem.PlayAni();
			}
			return loopListViewItem;
		}

		private UIGameEventItem GetUIItem(int instanceId)
		{
			UIGameEventItem uigameEventItem;
			if (this.itemDic.TryGetValue(instanceId, out uigameEventItem))
			{
				return uigameEventItem;
			}
			return null;
		}

		private UIGameEventItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIGameEventItem uigameEventItem = this.GetUIItem(instanceID);
			if (uigameEventItem == null)
			{
				uigameEventItem = obj.GetComponent<UIGameEventItem>();
				uigameEventItem.Init();
				this.itemDic.Add(instanceID, uigameEventItem);
				return uigameEventItem;
			}
			return uigameEventItem;
		}

		public UIGameEventItem GetCurrentItem()
		{
			return this.m_currentEvent;
		}

		private void AddEventAni()
		{
			float num = this.totalHeight - this.scrollHeight;
			if (num < 0f)
			{
				num = 0f;
			}
			TweenExtensions.Kill(this.scrollSeq, true);
			TweenSettingsExtensions.Append(this.scrollSeq, ShortcutExtensions.DOLocalMoveY(this.contentRectTrans, num, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(this.scrollSeq, delegate
			{
				this.loopListView.ScrollRect.StopMovement();
			});
		}

		public void PlayerEventTipAni(string info)
		{
			this.textEventTip.text = info;
			this.sequencePool.Clear(false);
			Sequence sequence = this.sequencePool.Get();
			float num = 0.3f;
			this.textEventTip.color = new Color(this.textEventTipColor.r, this.textEventTipColor.g, this.textEventTipColor.b, num);
			TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textEventTip, 0.95f, 0.7f)), ShortcutExtensions46.DOFade(this.textEventTip, num, 0.7f)), -1);
		}

		public void ChangeEventInfo(string info)
		{
			if (!string.IsNullOrEmpty(info))
			{
				this.buttonsCtrl.ChangeInfo(info);
			}
		}

		public void ChangeEventDefaultInfo()
		{
			this.buttonsCtrl.ChangeDefaultInfo();
		}

		public void SetMoveToNpc(bool isMoveTo)
		{
			this.isMoveToNpc = isMoveTo;
			this.m_currentEvent.ShowLoading(this.isMoveToNpc);
			this.buttonsCtrl.SetMoveToNpc(this.isMoveToNpc);
		}

		private void OnDragStart(PointerEventData data)
		{
			this.isDragging = true;
		}

		private void OnDragEnd(PointerEventData data)
		{
			this.isDragging = false;
		}

		private void OnClickScroll()
		{
			if (this.isDragging)
			{
				return;
			}
			if (this.isShowAni)
			{
				return;
			}
			if (this.isTextSlot)
			{
				return;
			}
			if (this.isMoveToNpc)
			{
				return;
			}
			this.copyFly = null;
			if (this.viewModule.IsClickEnabled())
			{
				if (this.buttonsCtrl.IsNextDayButton())
				{
					this.isShowAni = true;
					this.buttonsCtrl.SetNextDayButtonDown();
					float num = 0.25f;
					Color color = this.normalColor;
					GameEventPoolDataFactory eventPoolDataFactory = Singleton<GameEventController>.Instance.GetEventPoolDataFactory();
					EventSizeType sizeType = eventPoolDataFactory.GetNextShowSize();
					this.flySizeType = sizeType;
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ChangeHour, null);
					Sequence sequence = DOTween.Sequence();
					this.imageLoading.fillAmount = 0f;
					this.imageLoading.color = this.normalColor;
					this.imageLoadingBigWin.fillAmount = 1f;
					Color color2 = this.imageLoadingBigWin.color;
					color2.a = 0f;
					this.imageLoadingBigWin.color = color2;
					float num2 = 1f;
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFillAmount(this.imageLoading, num2, num));
					this.flyDelay = 0f;
					switch (sizeType)
					{
					case EventSizeType.Fail:
						this.ShowResultAni(sizeType);
						goto IL_027F;
					case EventSizeType.MinorWin:
					{
						this.copyFly = this.minorWinFly;
						float num3 = 0.15f;
						TweenSettingsExtensions.AppendCallback(sequence, delegate
						{
							this.ShowResultAni(sizeType);
						});
						TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOColor(this.imageLoading, this.minorWinColor, num3));
						this.flyDelay = 0.08f;
						goto IL_027F;
					}
					case EventSizeType.BigWin:
					{
						this.copyFly = this.bigWinFly;
						float num3 = 1.2f;
						TweenSettingsExtensions.AppendCallback(sequence, delegate
						{
							this.ShowResultAni(sizeType);
						});
						TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageLoading, 0f, num3));
						TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.imageLoadingBigWin, 1f, num3));
						this.flyDelay = 0.35f;
						goto IL_027F;
					}
					case EventSizeType.Activity:
					{
						this.copyFly = this.jackpotFly;
						float num3 = 0.15f;
						TweenSettingsExtensions.AppendCallback(sequence, delegate
						{
							this.ShowResultAni(sizeType);
						});
						TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOColor(this.imageLoading, this.jackpotColor, num3));
						this.flyDelay = 0.35f;
						goto IL_027F;
					}
					}
					this.ShowResultAni(sizeType);
					IL_027F:
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.imageLoading.fillAmount = 0f;
						this.imageLoadingBigWin.fillAmount = 0f;
						this.viewModule.OnClickScroll();
						this.isShowAni = false;
					});
					return;
				}
				this.buttonsCtrl.SetNextDayButtonDown();
			}
		}

		private void OnEventShowLoading(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = eventArgs as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.m_currentEvent.ShowLoading(eventArgsBool.Value);
			}
		}

		private void OnEventShowEventTipInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsString eventArgsString = eventArgs as EventArgsString;
			if (eventArgsString != null)
			{
				this.ChangeEventInfo(eventArgsString.Value);
			}
		}

		public void AddHeight(int index, float addHeight)
		{
			this.loopListView.OnItemSizeChanged(index);
			this.totalHeight += addHeight;
			this.AddEventAni();
		}

		public void ShakeButton()
		{
			this.buttonsCtrl.ShakeButton();
		}

		public void SetNextDayButtonInfo(int food, int beginFood, long hp, long maxHp)
		{
			this.buttonsCtrl.SetNextDayFood(food, beginFood);
			this.buttonsCtrl.SetNextDayHp(hp, maxHp);
		}

		public void TextSlotStart()
		{
			this.isTextSlot = true;
			this.buttonsCtrl.SetNextDayButtonDown();
		}

		public void TextSlotEnd()
		{
			this.isTextSlot = false;
		}

		public void EventAnimationFinish(Action aniFinish)
		{
			this.buttonsCtrl.RefreshButton(this.currentUIData, new Action(this.OnClickScroll), aniFinish);
			if (this.currentUIData.GetButtons().Count >= 2)
			{
				this.bonusCtrl.HideButtons();
				return;
			}
			this.bonusCtrl.ShowButtons();
		}

		private string GetNextDayAni(EventSizeType sizeType)
		{
			string text = "Anim_UIFX_NextDay_Click";
			switch (sizeType)
			{
			case EventSizeType.Fail:
				text = "Anim_UIFX_NextDay_BadLuck";
				break;
			case EventSizeType.MinorWin:
				text = "Anim_UIFX_NextDay_MinorWin";
				break;
			case EventSizeType.BigWin:
				text = "Anim_UIFX_NextDay_BigWin";
				break;
			case EventSizeType.Activity:
				text = "Anim_UIFX_NextDay_Jackpot";
				break;
			}
			return text;
		}

		private void ShowResultAni(EventSizeType sizeType)
		{
			this.RefreshBonus();
			string text = this.GetNextDayAni(sizeType);
			this.nextDayAni.speed = 1.3f;
			this.nextDayAni.Play(text, 0, 0f);
			if (this.copyFly != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.imagFly.gameObject);
				gameObject.SetParentNormal(this.imagFly.transform.parent, false);
				Image component = gameObject.GetComponent<Image>();
				if (component)
				{
					component.sprite = this.copyFly.sprite;
					component.SetNativeSize();
					component.transform.position = this.starPos.position;
					component.transform.localScale = this.copyFly.transform.localScale;
					this.cacheFlyList.Add(gameObject);
				}
			}
		}

		private void OnWinAniFinish(GameObject obj, string param)
		{
			if (param.Equals("Finish"))
			{
				GameObject fly = null;
				if (this.cacheFlyList.Count > 0)
				{
					fly = this.cacheFlyList[0];
					this.cacheFlyList.RemoveAt(0);
				}
				if (fly)
				{
					fly.SetActive(true);
					fly.transform.position = this.starPos.position;
					fly.transform.localScale = this.copyFly.transform.localScale;
					UIGameEventItem curItem = this.GetCurrentItem();
					Transform transform = curItem.GetFlyTarget();
					if (this.flySizeType == EventSizeType.MinorWin && this.isShowMinorBonus)
					{
						transform = this.bonusCtrl.GetMinorWinFlyPosition();
					}
					else if (this.flySizeType == EventSizeType.BigWin && this.isShowBigBonus)
					{
						transform = this.bonusCtrl.GetBigWinFlyPosition();
					}
					float num = 0.2f;
					Sequence sequence = this.sequencePool.Get();
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOMove(fly.transform, transform.position, num, false));
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(fly.transform, 0.2f, num));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						if (curItem)
						{
							curItem.ShowResultText();
						}
						EventSizeType eventSizeType = this.flySizeType;
						if (eventSizeType != EventSizeType.MinorWin)
						{
							if (eventSizeType == EventSizeType.BigWin)
							{
								this.bonusCtrl.DoGetBigWin();
							}
						}
						else
						{
							this.bonusCtrl.DoGetMinorWin();
						}
						if (fly)
						{
							Object.Destroy(fly);
						}
					});
					return;
				}
			}
			else
			{
				if (param.Equals("Normal"))
				{
					DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
					GameApp.Sound.PlayClip(81, 1f);
					return;
				}
				if (param.Equals("Lucky"))
				{
					DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
					GameApp.Sound.PlayClip(82, 1f);
					return;
				}
				if (param.Equals("SuperLucky"))
				{
					DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Middle);
					GameApp.Sound.PlayClip(83, 1f);
					return;
				}
				if (param.Equals("Unlucky"))
				{
					DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
					GameApp.Sound.PlayClip(84, 1f);
				}
			}
		}

		private void RefreshBonus()
		{
			this.bonusCtrl.DoGetBigWin();
			this.bonusCtrl.DoGetMinorWin();
		}

		private void OnCloseBonus()
		{
			if (this.cacheUIList.Count > 1 && this.cacheUIList[0].IsShowButton())
			{
				GameEventUIData gameEventUIData = this.cacheUIList[0];
				this.cacheUIList.RemoveAt(0);
				this.cacheUIList.Add(gameEventUIData);
			}
			for (int i = 0; i < this.cacheUIList.Count; i++)
			{
				this.AddEvent(this.cacheUIList[i]);
			}
			this.cacheUIList.Clear();
		}

		public GameObject eventItemObj;

		public CustomText textEventTip;

		public GameObject eventLoading;

		public ScrollRectBase scrollRect;

		public LoopListView2 loopListView;

		public UIEventButtonsCtrl buttonsCtrl;

		public UIBonusGameButtonCtrl bonusCtrl;

		public Image imageLoading;

		public Image imageLoadingBigWin;

		[Header("事件结果")]
		public Animator nextDayAni;

		public AnimatorListen nextDayAniListen;

		public Color normalColor;

		public Color minorWinColor;

		public Color jackpotColor;

		public Image minorWinFly;

		public Image bigWinFly;

		public Image jackpotFly;

		public Image imagFly;

		public Transform starPos;

		public UILanguageHelper[] helpers;

		public float TestDuration = 0.75f;

		private Dictionary<int, UIGameEventItem> itemDic = new Dictionary<int, UIGameEventItem>();

		private List<GameEventUIData> uiDataList = new List<GameEventUIData>();

		private UIGameEventItem m_currentEvent;

		private GameEventUIData currentUIData;

		private RectTransform contentRectTrans;

		private float scrollHeight;

		private int previousIndex = -1;

		private float totalHeight;

		private float m_scrollPositionY;

		private float m_scrollSizeY;

		private float height;

		private float endHeight;

		private bool isScrollUpdate;

		private bool isExpand;

		private bool isShrink;

		private float currentHeight;

		private float targetHeight;

		private bool isPlayEventAni;

		private float scrollTarget;

		private float time;

		private bool isDragging;

		private Color textEventTipColor;

		private float scrollStart;

		private float ScrollBottomHeight = 150f;

		private GameEventViewModule viewModule;

		private SequencePool sequencePool = new SequencePool();

		private Sequence scrollSeq;

		private bool isShowAni;

		private bool isTextSlot;

		private bool isMoveToNpc;

		private float initY;

		private Image copyFly;

		private float flyDuration = 1f;

		private float curflyTime;

		private Vector3 startPos;

		private float startScale;

		private bool isWaitFly;

		private bool isFly;

		private float flyDelay;

		private UIGameEventItem currentFlyItem;

		private EventSizeType flySizeType;

		private bool isShowBigBonus;

		private bool isShowMinorBonus;

		private List<GameObject> cacheFlyList = new List<GameObject>();

		private List<GameEventUIData> cacheUIList = new List<GameEventUIData>();
	}
}
