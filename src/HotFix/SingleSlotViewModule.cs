using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class SingleSlotViewModule : BaseViewModule
	{
		private int ResultIndex
		{
			get
			{
				if (this.mDataList.Count > 3)
				{
					return this.mDataList.Count - 3;
				}
				return 0;
			}
		}

		public override void OnCreate(object data)
		{
			this.CurveScroll = new AnimationCurve();
			this.CurveScroll.AddKey(new Keyframe(0f, 0f, 0f, 2.517f));
			this.CurveScroll.AddKey(new Keyframe(0.92f, 1.012f, 0.03f, -0.15f));
			this.CurveScroll.AddKey(new Keyframe(1f, 1f, -0.15f, 0f));
			this.ButtonReSpin.Init();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("slot_spin");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("slot_spin");
			this.ButtonReSpin.SetTextUp(infoByID);
			this.ButtonReSpin.SetTextDown(infoByID2);
			this.ButtonReSpin.SetData(new Action(this.ReSpin));
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Scroll.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
			this.Scroll.ScrollRect.vertical = false;
			this.Scroll.ScrollRect.horizontal = false;
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.lockObj.SetActiveSafe(false);
			IList<ChapterMiniGame_singleSlotReward> chapterMiniGame_singleSlotRewardElements = GameApp.Table.GetManager().GetChapterMiniGame_singleSlotRewardElements();
			for (int i = 0; i < this.scoreItems.Length; i++)
			{
				if (i < chapterMiniGame_singleSlotRewardElements.Count)
				{
					this.scoreItems[i].gameObject.SetActiveSafe(true);
					this.scoreItems[i].Init();
					this.scoreItems[i].SetData(chapterMiniGame_singleSlotRewardElements[i]);
				}
				else
				{
					this.scoreItems[i].gameObject.SetActiveSafe(false);
				}
			}
			this.StopReset();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			SingleSlotViewModule.OpenData openData = data as SingleSlotViewModule.OpenData;
			if (openData != null)
			{
				this.openData = openData;
			}
			if (this.openData == null)
			{
				return;
			}
			int randSeed = this.openData.randSeed;
			this.slotManager = new SingleSlotManager(randSeed, this.openData.freePlayNum);
			this.ViewRefresh();
			if (this.openData.currentWin >= this.openData.needWin)
			{
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				GameTGATools.Ins.SetStageButtonContent(0, "大吉奖励");
				GameTGATools.Ins.SetStageButtonClickIndex(0);
				GameTGATools.Ins.OnStageClickButton();
				this.ButtonReSpin.gameObject.SetActiveSafe(true);
				this.lockObj.SetActiveSafe(false);
			}
			else
			{
				this.ButtonReSpin.gameObject.SetActiveSafe(false);
				this.lockObj.SetActiveSafe(true);
				this.textLockTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("uisingleslot_lock_tip", new object[]
				{
					this.openData.needWin,
					this.openData.currentWin,
					this.openData.needWin
				});
			}
			this.RefreshPlayNum();
			if (this.openData.isChangeBgm)
			{
				GameApp.Sound.PlayBGM(122, 1f);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mScrollTime > 0f)
			{
				this.mScrollTime -= deltaTime;
				float num = (this.mScrollTimeMax - this.mScrollTime) / this.mScrollTimeMax;
				float num2 = this.CurveScroll.Evaluate(num);
				float num3 = Mathf.LerpUnclamped(this.ScrollStartPositionY, this.ScrollEndPositionY, num2);
				this.RTFContent.anchoredPosition = new Vector2(0f, num3);
				float num4 = Mathf.LerpUnclamped(this.ShakeRange.x, this.ShakeRange.y, this.CurveShake.Evaluate(num));
				this.RTFShakeRoot.anchoredPosition = new Vector2(0f, num4);
				if (!this.mPlayResultSound && this.mScrollTime <= 0.1f)
				{
					this.mPlayResultSound = true;
					if (this.mResultData != null)
					{
						GameApp.Sound.PlayClip(64, 1f);
					}
				}
				if (this.mScrollTime <= 0f)
				{
					this.StopReset();
					if (this.mResultData != null && (this.mResultData.rewardType == SingleSlotRewardType.ANGEL_SCORE_ID || this.mResultData.rewardType == SingleSlotRewardType.DEMON_SCORE_ID))
					{
						this.EffectSuper.SetActive(true);
					}
					if (this.mResultUI != null)
					{
						this.mResultUI.PlayCombineAnimation(new Action(this.StopAnimation));
					}
					else
					{
						this.StopAnimation();
					}
				}
			}
			if (this.mResultUI != null)
			{
				this.mResultUI.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			if (this.openData != null && this.openData.isChangeBgm)
			{
				GameApp.Sound.PlayBGM(this.openData.oldBgm, 1f);
			}
			if (this.openData != null)
			{
				Action onCloseUI = this.openData.onCloseUI;
				if (onCloseUI == null)
				{
					return;
				}
				onCloseUI();
			}
		}

		public override void OnDelete()
		{
			this.ButtonReSpin.DeInit();
			this.Scroll.ScrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.OnScroll));
			this.DeInitAllScrollUI();
			for (int i = 0; i < this.scoreItems.Length; i++)
			{
				this.scoreItems[i].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			if (index < 0 || index + 1 >= this.mDataList.Count)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			index = index;
			SingleSlotData singleSlotData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ChapterEventSmallSlotItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UISingleSlotItem uisingleSlotItem = this.TryGetUI(instanceID);
			UISingleSlotItem component = loopListViewItem.GetComponent<UISingleSlotItem>();
			if (uisingleSlotItem == null)
			{
				uisingleSlotItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uisingleSlotItem.SetData(singleSlotData);
			uisingleSlotItem.SetActive(true);
			uisingleSlotItem.RefreshUI();
			if (index == this.ResultIndex)
			{
				this.mResultUI = uisingleSlotItem;
			}
			return loopListViewItem;
		}

		private UISingleSlotItem TryGetUI(int key)
		{
			UISingleSlotItem uisingleSlotItem;
			if (this.mUICtrlDic.TryGetValue(key, out uisingleSlotItem))
			{
				return uisingleSlotItem;
			}
			return null;
		}

		private UISingleSlotItem TryAddUI(int key, LoopListViewItem2 loopitem, UISingleSlotItem ui)
		{
			ui.Init();
			UISingleSlotItem uisingleSlotItem;
			if (this.mUICtrlDic.TryGetValue(key, out uisingleSlotItem))
			{
				if (uisingleSlotItem == null)
				{
					uisingleSlotItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UISingleSlotItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		private void OnScroll(Vector2 arg0)
		{
			Vector3 position = this.RTFViewCenter.position;
			foreach (KeyValuePair<int, UISingleSlotItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.AutoScale(position);
			}
		}

		private void ReSpin()
		{
			if (this.isSpine)
			{
				return;
			}
			if (this.mScrollTime > 0f)
			{
				return;
			}
			if (!this.slotManager.PlayNum.IsDataValid())
			{
				this.OnClickClose();
				return;
			}
			this.ViewRefresh();
			this.BuildResult();
			this.BeginSpin();
		}

		private void StopReset()
		{
			this.mPlayResultSound = false;
			this.EffectArrowLeft.SetActive(false);
			this.EffectArrowRight.SetActive(false);
			this.EffectSuper.SetActive(false);
			this.ArrowLeftAni.Play("idle");
			this.ArrowRightAni.Play("idle");
		}

		private void ViewRefresh()
		{
			this.m_seqPool.Clear(false);
			this.mScrollTime = 0f;
			this.EffectSuper.SetActive(false);
			this.EffectArrowLeft.SetActive(false);
			this.EffectArrowRight.SetActive(false);
			this.BuildRandomShowDataList();
			this.Scroll.SetListItemCount(this.mDataList.Count, false);
			this.Scroll.PrepareAllItemSize(this.ItemSize);
			this.Scroll.RefreshAllShowItems();
			this.RTFContent.anchoredPosition = new Vector2(0f, this.ScrollStartPositionY);
			this.ScrollStartPositionY = this.RTFContent.anchoredPosition.y;
			this.RTFShakeRoot.anchoredPosition = new Vector2(0f, this.ShakeRange.x);
			this.ViewAnim.Play("open");
		}

		private void BeginSpin()
		{
			this.isSpine = true;
			this.ButtonReSpin.SetLock(true);
			this.m_seqPool.Clear(false);
			this.EffectSuper.SetActive(false);
			this.ScrollStartPositionY = this.RTFContent.anchoredPosition.y;
			this.RTFContent.anchoredPosition = new Vector2(0f, this.ScrollStartPositionY);
			this.RTFShakeRoot.anchoredPosition = new Vector2(0f, this.ShakeRange.x);
			this.mScrollTime = (this.mScrollTimeMax = 1f);
			this.PlaySoundNew(117);
			this.EffectArrowLeft.SetActive(true);
			this.EffectArrowRight.SetActive(true);
			this.ArrowLeftAni.Play("show");
			this.ArrowRightAni.Play("show");
		}

		private void BuildRandomShowDataList()
		{
			this.mDataList.Clear();
			List<SingleSlotData> randomPool = this.slotManager.GetRandomPool();
			int count = randomPool.Count;
			List<SingleSlotData> list = new List<SingleSlotData>();
			list.AddRange(randomPool);
			int totalDataListCount = this.TotalDataListCount;
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < totalDataListCount; i++)
			{
				int num = random.Next(list.Count);
				SingleSlotData singleSlotData = list[num];
				list.RemoveAt(num);
				if ((float)list.Count < (float)count / 3f)
				{
					list.Clear();
					list.AddRange(randomPool);
				}
				this.mDataList.Add(singleSlotData);
			}
			this.ScrollStartPositionY = this.ItemSize.y / 2f + 10f;
			this.ScrollEndPositionY = ((float)this.ResultIndex - 0.5f) * (this.ItemSize.y + 10f) + 10f;
		}

		private void BuildResult()
		{
			this.mResultData = this.slotManager.RandomResult();
			this.mDataList[this.ResultIndex] = this.mResultData;
			this.RefreshPlayNum();
		}

		private void StopAnimation()
		{
			GameApp.Sound.StopSoundEffect(this.audioPath);
			foreach (UISingleSlotItem uisingleSlotItem in this.mUICtrlDic.Values)
			{
				uisingleSlotItem.ResetAnim();
			}
			if (this.mResultData != null)
			{
				this.slotManager.AddResult(this.mResultData);
				if (this.mResultUI != null)
				{
					if (this.mResultData.rewardType == SingleSlotRewardType.ANGEL_SCORE_ID || this.mResultData.rewardType == SingleSlotRewardType.DEMON_SCORE_ID)
					{
						this.PlayScoreAnimation(this.mResultUI.ImageIcon, (int)this.mResultData.rewardType, this.mResultData.rewardNum);
					}
					else if (this.mResultData.rewardType == SingleSlotRewardType.PLAY_NUM)
					{
						this.PlaySpinNumAnimation(this.mResultUI.TextPlayNum);
					}
					else
					{
						this.ResetSpinState();
					}
				}
				else
				{
					this.ResetSpinState();
				}
			}
			else
			{
				this.ResetSpinState();
			}
			if (this.slotManager.PlayNum.mVariable <= 0)
			{
				this.lockObj.SetActiveSafe(true);
				this.textLockTip.text = "";
				this.ButtonReSpin.gameObject.SetActiveSafe(false);
			}
		}

		private void ResetSpinState()
		{
			this.isSpine = false;
			this.ButtonReSpin.SetLock(false);
		}

		private void RefreshPlayNum()
		{
			this.textPlayNum.text = this.slotManager.PlayNum.mVariable.ToString();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.playNumLayout);
		}

		public void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.SingleSlotViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSingleSlot, null);
		}

		private void PlayScoreAnimation(CustomImage copyImage, int scoreId, int score)
		{
			this.flyImages.Clear();
			for (int i = 0; i < score; i++)
			{
				CustomImage customImage = Object.Instantiate<CustomImage>(copyImage, this.flyNode, false);
				customImage.name = "ImgScore" + i.ToString();
				customImage.transform.position = copyImage.transform.position;
				customImage.gameObject.SetActive(false);
				this.flyImages.Add(customImage);
			}
			Vector3 flyTargetPosition = this.GetFlyTargetPosition(scoreId);
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.4f;
			for (int j = 0; j < this.flyImages.Count; j++)
			{
				int num2 = j;
				CustomImage imgScore = this.flyImages[num2];
				int num3 = num2 * 100;
				TweenSettingsExtensions.AppendInterval(sequence, (float)num3);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					imgScore.gameObject.SetActive(true);
				});
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMoveX(imgScore.transform, flyTargetPosition.x, num, false), 27));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOMoveY(imgScore.transform, flyTargetPosition.y, num, false));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(imgScore.transform, 0.5f, num));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					GameApp.Sound.PlayClip(119, 1f);
					if (imgScore == null)
					{
						return;
					}
					if (imgScore != null && imgScore.gameObject != null)
					{
						Object.Destroy(imgScore.gameObject);
					}
					if (this.scoreItems != null)
					{
						for (int k = 0; k < this.scoreItems.Length; k++)
						{
							if (!(this.scoreItems[k].gameObject == null) && this.scoreItems[k].IsTargetReward(scoreId))
							{
								this.scoreItems[k].AddScore();
							}
						}
					}
				});
			}
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.ResetSpinState));
		}

		private void PlaySpinNumAnimation(CustomText copyText)
		{
			Sequence sequence = this.m_seqPool.Get();
			CustomText textScore = Object.Instantiate<CustomText>(copyText, this.flyNode, false);
			textScore.name = "TextScore";
			textScore.transform.position = copyText.transform.position;
			Vector3 position = this.textPlayNum.transform.position;
			float num = 0.4f;
			textScore.gameObject.SetActive(true);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMoveX(textScore.transform, position.x, num, false), 27));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOMoveY(textScore.transform, position.y, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(textScore.transform, 0.5f, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				textScore.transform.localScale = Vector3.zero;
				GameApp.Sound.PlayClip(120, 1f);
			});
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textPlayNum.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.textPlayNum.transform, 1f, 0.05f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (textScore != null && textScore.gameObject != null)
				{
					Object.Destroy(textScore.gameObject);
				}
				this.RefreshPlayNum();
				this.ResetSpinState();
			});
		}

		private Vector3 GetFlyTargetPosition(int scoreId)
		{
			for (int i = 0; i < this.scoreItems.Length; i++)
			{
				if (this.scoreItems[i].IsTargetReward(scoreId))
				{
					return this.scoreItems[i].GetFlyPosition();
				}
			}
			return Vector3.zero;
		}

		private async void PlaySoundNew(int id)
		{
			Sound_sound elementById = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(id);
			if (elementById != null && elementById.volume > 0f)
			{
				this.audioPath = elementById.path;
				await GameApp.Sound.PlaySoundEffectNew(this.audioPath, elementById.volume);
			}
		}

		[GameTestMethod("事件小游戏", "单列老虎机", "", 311)]
		private static void OpenGameEventSkillSlot()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			SingleSlotViewModule.OpenData openData = new SingleSlotViewModule.OpenData();
			openData.randSeed = random.Next();
			openData.freePlayNum = 999;
			openData.isChangeBgm = true;
			openData.oldBgm = 101;
			GameApp.View.OpenView(ViewName.SingleSlotViewModule, openData, 1, null, null);
		}

		public RectTransform RTFContent;

		public AnimationCurve CurveScroll;

		public UIOneButton4MiniGame ButtonReSpin;

		public CustomButton buttonClose;

		public GameObject lockObj;

		public CustomText textLockTip;

		public CustomText textPlayNum;

		public RectTransform playNumLayout;

		public UISingleSlotScoreItem[] scoreItems;

		public Transform flyNode;

		[Header("界面动画")]
		public Animator ViewAnim;

		[Header("震动")]
		public RectTransform RTFShakeRoot;

		public AnimationCurve CurveShake;

		public Vector2 ShakeRange;

		[Header("指针")]
		public Animator ArrowLeftAni;

		public GameObject EffectArrowLeft;

		public Animator ArrowRightAni;

		public GameObject EffectArrowRight;

		[Header("其他")]
		public GameObject EffectSuper;

		public RectTransform RTFViewCenter;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 0;

		private const int mSBottomCount = 0;

		private const int mSExCount = 0;

		private List<SingleSlotData> mDataList = new List<SingleSlotData>();

		private Dictionary<int, UISingleSlotItem> mUICtrlDic = new Dictionary<int, UISingleSlotItem>();

		public Vector2 ItemSize = new Vector2(630f, 250f);

		public int TotalDataListCount = 20;

		public float ScrollStartPositionY;

		public float ScrollEndPositionY;

		private const float TOTAL_SCROLL_TIME = 1f;

		private float mScrollTimeMax = 1f;

		private float mScrollTime = -1f;

		private SingleSlotData mResultData;

		private UISingleSlotItem mResultUI;

		private bool mPlayResultSound;

		private SequencePool m_seqPool = new SequencePool();

		private SingleSlotManager slotManager;

		private SingleSlotViewModule.OpenData openData;

		private List<CustomImage> flyImages = new List<CustomImage>();

		private bool isSpine;

		private string audioPath;

		public class OpenData
		{
			public int randSeed = 100;

			public int currentWin;

			public int needWin;

			public int freePlayNum;

			public bool isChangeBgm;

			public int oldBgm;

			public Action onCloseUI;
		}
	}
}
