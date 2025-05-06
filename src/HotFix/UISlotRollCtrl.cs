using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UISlotRollCtrl : CustomBehaviour
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

		protected override void OnInit()
		{
			this.CurveScroll = new AnimationCurve();
			this.CurveScroll.AddKey(new Keyframe(0f, 0f, 0f, 1.5f));
			this.CurveScroll.AddKey(new Keyframe(0.92f, 1.012f, 0.03f, -0.15f));
			this.CurveScroll.AddKey(new Keyframe(1f, 1f, -0.15f, 0f));
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Scroll.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
			this.Scroll.ScrollRect.vertical = false;
			this.Scroll.ScrollRect.horizontal = false;
			this.fxWin.gameObject.SetActive(false);
			this.fxFail.gameObject.SetActive(false);
			this.failRT.localScale = Vector3.zero;
		}

		protected override void OnDeInit()
		{
			this.sequencePool.Clear(false);
			this.Scroll.ScrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.OnScroll));
			this.DeInitAllScrollUI();
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
				float num4 = Mathf.LerpUnclamped(this.ScaleRange.x, this.ScaleRange.y, this.ScaleCurve.Evaluate(num));
				this.ScaleTrans.localScale = new Vector3(1f, num4, 1f);
				if (!this.mPlayResultSound && this.mScrollTime <= 0.1f)
				{
					this.mPlayResultSound = true;
					if (this.mResultData != null)
					{
						GameApp.Sound.PlayClip(126, 1f);
					}
				}
				if (this.mScrollTime <= 0f)
				{
					if (this.mResultData != null)
					{
						if (this.mResultData.IsEmpty)
						{
							this.fxFail.gameObject.SetActive(true);
							this.fxFail.Play();
							this.failRT.localScale = Vector3.zero;
							this.failRT.anchoredPosition = Vector2.zero;
							float num5 = 0.1f;
							Sequence sequence = this.sequencePool.Get();
							TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.failRT, Vector3.one, num5));
							TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.failRT, 200f, num5, false));
							TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
							TweenSettingsExtensions.AppendCallback(sequence, delegate
							{
								this.failRT.localScale = Vector3.zero;
							});
						}
						else
						{
							this.fxWin.gameObject.SetActive(true);
							this.fxWin.Play();
						}
						if (this.mResultData.IsSkill && this.mResultData.SkillBuildData != null && this.mResultData.SkillBuildData.quality >= SkillBuildQuality.Red)
						{
							GameApp.Sound.PlayClip(128, 1f);
						}
						else if (!this.mResultData.IsEmpty)
						{
							GameApp.Sound.PlayClip(127, 1f);
						}
					}
					this.ScaleTrans.localScale = Vector3.one;
					this.StopRoll();
				}
			}
			if (this.mResultUI != null)
			{
				this.mResultUI.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void SetData(int seed, int groupId, int index, Action onEndRoll)
		{
			this.mGroupId = groupId;
			this.OnEndRoll = onEndRoll;
			this.paySlotManager = new PaySlotManager(seed, this.mGroupId);
			this.Refresh();
			this.OnScroll(Vector2.zero);
			this.imageBg.sprite = this.imageSR.GetSprite(index.ToString());
		}

		private void Refresh()
		{
			this.sequencePool.Clear(false);
			this.mScrollTime = 0f;
			this.BuildRandomShowDataList();
			this.Scroll.SetListItemCount(this.mDataList.Count, false);
			this.Scroll.PrepareAllItemSize(this.ItemSize);
			this.Scroll.RefreshAllShowItems();
			this.RTFContent.anchoredPosition = new Vector2(0f, this.ScrollStartPositionY);
			this.ScrollStartPositionY = this.RTFContent.anchoredPosition.y;
		}

		private void BuildRandomShowDataList()
		{
			this.mDataList.Clear();
			List<PaySlotRewardData> showReward = this.paySlotManager.GetShowReward(this.TotalDataListCount);
			this.mDataList.AddRange(showReward);
			float num = 20f;
			this.ScrollStartPositionY = -(this.ItemSize.y / 2f + num);
			this.ScrollEndPositionY = -(((float)this.ResultIndex - 0.5f) * (this.ItemSize.y + num) + 10f);
		}

		private void BuildResult()
		{
			this.mResultData = this.paySlotManager.RandomResult();
			this.mDataList[this.ResultIndex] = this.mResultData;
		}

		private void OnScroll(Vector2 arg0)
		{
			Vector3 position = this.RTFViewCenter.position;
			foreach (KeyValuePair<int, UISlotRollItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.AutoScale(position);
			}
		}

		public void Roll()
		{
			this.Refresh();
			this.BuildResult();
			this.StartRoll();
		}

		private void StartRoll()
		{
			if (this.isSpine)
			{
				return;
			}
			this.isSpine = true;
			this.sequencePool.Clear(false);
			this.fxWin.gameObject.SetActive(false);
			this.fxFail.gameObject.SetActive(false);
			this.ScrollStartPositionY = this.RTFContent.anchoredPosition.y;
			this.RTFContent.anchoredPosition = new Vector2(0f, this.ScrollStartPositionY);
			this.mScrollTime = (this.mScrollTimeMax = 0.7f);
			GameApp.Sound.PlayClip(125, 1f);
		}

		private void StopRoll()
		{
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.ShowResult();
				this.mPlayResultSound = false;
				if (this.mResultData != null)
				{
					if (this.mResultData.IsSkill && this.mResultData.SkillBuildData != null)
					{
						Singleton<GameEventController>.Instance.SelectSkill(this.mResultData.SkillBuildData, false);
						GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { this.mResultData.SkillBuildData }, true);
					}
					else if (this.mResultData.IsAttribute && this.mResultData.NodeAttParam != null)
					{
						GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { this.mResultData.NodeAttParam }, false);
						Singleton<GameEventController>.Instance.MergerAttribute(this.mResultData.NodeAttParam);
					}
				}
				this.isSpine = false;
				Action onEndRoll = this.OnEndRoll;
				if (onEndRoll == null)
				{
					return;
				}
				onEndRoll();
			});
		}

		private void ShowResult()
		{
			if (this.mResultData == null)
			{
				return;
			}
			SlotTrainRewardViewModule.OpenData openData = new SlotTrainRewardViewModule.OpenData();
			if (this.mResultData.IsSkill)
			{
				openData.skillBuild = this.mResultData.SkillBuildData;
			}
			else if (this.mResultData.IsAttribute)
			{
				openData.atlasId = this.mResultData.Config.atlas;
				openData.icon = this.mResultData.Config.icon;
				openData.showInfo = NodeAttParam.GetUIShowAttributeInfo(this.mResultData.NodeAttParam.attType, (long)this.mResultData.NodeAttParam.FinalCount);
			}
			if (!this.mResultData.IsEmpty)
			{
				GameApp.View.OpenView(ViewName.SlotTrainRewardViewModule, openData, 1, null, null);
			}
		}

		public void OnHide()
		{
			this.fxWin.gameObject.SetActiveSafe(false);
			this.fxFail.gameObject.SetActiveSafe(false);
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
			PaySlotRewardData paySlotRewardData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UISlotRollItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UISlotRollItem uislotRollItem = this.TryGetUI(instanceID);
			UISlotRollItem component = loopListViewItem.GetComponent<UISlotRollItem>();
			if (uislotRollItem == null)
			{
				uislotRollItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uislotRollItem.SetData(paySlotRewardData);
			uislotRollItem.SetActive(true);
			if (index == this.ResultIndex)
			{
				this.mResultUI = uislotRollItem;
			}
			return loopListViewItem;
		}

		private UISlotRollItem TryGetUI(int key)
		{
			UISlotRollItem uislotRollItem;
			if (this.mUICtrlDic.TryGetValue(key, out uislotRollItem))
			{
				return uislotRollItem;
			}
			return null;
		}

		private UISlotRollItem TryAddUI(int key, LoopListViewItem2 loopitem, UISlotRollItem ui)
		{
			ui.Init();
			UISlotRollItem uislotRollItem;
			if (this.mUICtrlDic.TryGetValue(key, out uislotRollItem))
			{
				if (uislotRollItem == null)
				{
					uislotRollItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UISlotRollItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public RectTransform RTFContent;

		public RectTransform RTFViewCenter;

		public LoopListView2 Scroll;

		public CustomImage imageBg;

		public SpriteRegister imageSR;

		[Header("特效动画")]
		public ParticleSystem fxWin;

		public ParticleSystem fxFail;

		public RectTransform failRT;

		[Header("动画调试")]
		public AnimationCurve CurveScroll;

		public Vector2 ItemSize = new Vector2(100f, 100f);

		public int TotalDataListCount = 20;

		public float ScrollStartPositionY;

		public float ScrollEndPositionY;

		[Header("果冻动画")]
		public RectTransform ScaleTrans;

		public AnimationCurve ScaleCurve;

		public Vector2 ScaleRange = new Vector2(0.8f, 1.4f);

		private const int ScrollTopCount = 0;

		private const int ScrollBottomCount = 0;

		private const int ScrollExCount = 0;

		private List<PaySlotRewardData> mDataList = new List<PaySlotRewardData>();

		private Dictionary<int, UISlotRollItem> mUICtrlDic = new Dictionary<int, UISlotRollItem>();

		private PaySlotRewardData mResultData;

		private UISlotRollItem mResultUI;

		private SequencePool sequencePool = new SequencePool();

		private PaySlotManager paySlotManager;

		private const float TOTAL_SCROLL_TIME = 0.7f;

		private float mScrollTimeMax = 0.7f;

		private float mScrollTime = -1f;

		private bool mPlayResultSound;

		private bool isSpine;

		private int mGroupId;

		private Action OnEndRoll;
	}
}
