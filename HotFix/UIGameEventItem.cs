using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIGameEventItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rectTransform = base.gameObject.GetComponent<RectTransform>();
			this.imageRectTrans = this.imageBg.GetComponent<RectTransform>();
			this.mAttributeUpdateItem.Init();
			this.eventLoading.SetActiveSafe(false);
			this.textSlotItem.text = "";
			this.sweepLight.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.seqPool.Clear(false);
			this.m_textInfoSeq.Clear(false);
			this.buttonItems.Clear();
			this.uiData = null;
			if (this.mAttributeUpdateItem != null)
			{
				this.mAttributeUpdateItem.DeInit();
				this.mAttributeUpdateItem = null;
			}
			this.textSlotItemList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void Refresh(GameEventUIData data, UIEventController ctrl, int index, bool isNewAdd)
		{
			if (data == null)
			{
				return;
			}
			this.uiEventController = ctrl;
			this.uiData = data;
			this.mIndex = index;
			this.dayObj.SetActiveSafe(this.uiData.isRoot);
			this.lineObj.SetActiveSafe(!this.uiData.isRoot);
			if (this.uiData.sizeType == EventSizeType.Activity)
			{
				this.textStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_SurpriseDay");
			}
			else
			{
				this.textStage.text = ((this.uiData.stage == 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_57") : Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_15", new object[] { this.uiData.stage }));
			}
			if (isNewAdd && this.uiData.IsTextSlot)
			{
				if (this.uiEventController)
				{
					this.uiEventController.TextSlotStart();
				}
				this.InitTextSlot();
				this.RefreshSlotInfo();
				this.textResult.transform.localScale = Vector3.zero;
				this.PlaySlotAni();
				return;
			}
			this.RefreshInfo();
			if (isNewAdd)
			{
				this.PlayAttributeAni();
			}
		}

		private void RefreshInfo()
		{
			if (this.uiData == null)
			{
				return;
			}
			this.textSlotObj.SetActiveSafe(false);
			this.textEvent.gameObject.SetActiveSafe(true);
			this.textEvent.text = this.uiData.info;
			this.mAttributeUpdateItem.SetData(this.uiData.GetAttParamList(), this.uiData.GetItemParamList(), this.uiData.GetSkillParamList(), this.uiData.GetInfoParamList(), this.uiData.GetScoreParamList());
			this.attributeUpdateNode.SetActiveSafe(this.uiData.IsHaveParams());
			float num = 0f;
			if (this.uiData.CacheTotalHeight == 0f && this.uiData.CacheBgHeight == 0f)
			{
				float num2;
				if (this.uiData.isRoot)
				{
					num2 = this.dayObj.GetComponent<RectTransform>().sizeDelta.y;
				}
				else
				{
					num2 = this.lineObj.GetComponent<RectTransform>().sizeDelta.y;
				}
				float num3 = 100f;
				num3 += this.textEvent.preferredHeight + 30f;
				int itemCount = this.mAttributeUpdateItem.GetItemCount();
				if (itemCount > 3)
				{
					num3 += 200f;
				}
				else if (itemCount > 0)
				{
					num3 += 100f;
				}
				num = num3 + num2;
				this.uiData.CacheBgHeight = num3;
				this.uiData.CacheTotalHeight = num;
			}
			this.imageRectTrans.sizeDelta = new Vector2(this.imageRectTrans.sizeDelta.x, this.uiData.CacheBgHeight);
			this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.uiData.CacheTotalHeight);
			if (num > 0f && this.oldHeight > 0f)
			{
				float num4 = num - this.oldHeight;
				if (this.uiEventController)
				{
					this.uiEventController.AddHeight(this.mIndex, num4);
				}
			}
			this.RefreshResult();
		}

		private void PlayAttributeAni()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_UIAnimationStart, null);
			this.mAttributeUpdateItem.ShowItems(delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_UIAnimationFinish, null);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			});
		}

		private void RefreshSlotInfo()
		{
			if (this.uiData == null)
			{
				return;
			}
			this.textSlotObj.SetActiveSafe(true);
			this.textEvent.gameObject.SetActiveSafe(false);
			this.textStage.text = ((this.uiData.stage == 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_57") : Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_15", new object[] { this.uiData.stage }));
			float num = ((this.textSlotItemList.Count > 0) ? this.textSlotItemList[0].preferredHeight : 50f);
			float num2;
			if (this.uiData.isRoot)
			{
				num2 = this.dayObj.GetComponent<RectTransform>().sizeDelta.y;
			}
			else
			{
				num2 = this.lineObj.GetComponent<RectTransform>().sizeDelta.y;
			}
			float num3 = 100f + num + 30f;
			this.oldHeight = num3 + num2;
			this.imageRectTrans.sizeDelta = new Vector2(this.imageRectTrans.sizeDelta.x, num3);
			this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.oldHeight);
			this.RefreshResult();
		}

		private void RefreshResult()
		{
			string atlasPath = GameApp.Table.GetAtlasPath(115);
			this.imageBg.SetImage(atlasPath, UIGameEventItem.GetSpriteBg(this.uiData.sizeType));
			Color white = Color.white;
			Color black = Color.black;
			switch (this.uiData.sizeType)
			{
			case EventSizeType.Normal:
				this.textResult.text = "";
				break;
			case EventSizeType.Fail:
				this.textResult.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_Result_Fail");
				ColorUtility.TryParseHtmlString("#dddbf4", ref white);
				ColorUtility.TryParseHtmlString("#000000", ref black);
				break;
			case EventSizeType.MinorWin:
				this.textResult.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_Result_MinorWin");
				ColorUtility.TryParseHtmlString("#cdfbff", ref white);
				ColorUtility.TryParseHtmlString("#0f4677", ref black);
				break;
			case EventSizeType.BigWin:
				this.textResult.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_Result_BigWin");
				ColorUtility.TryParseHtmlString("#fff392", ref white);
				ColorUtility.TryParseHtmlString("#701d35", ref black);
				break;
			case EventSizeType.Activity:
				this.textResult.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_SurpriseDay");
				ColorUtility.TryParseHtmlString("#FFF39A", ref white);
				ColorUtility.TryParseHtmlString("#481F7D", ref black);
				break;
			}
			this.textResult.color = white;
			this.resultOutline.effectColor = black;
		}

		public void PlayAni()
		{
			Sequence sequence = this.seqPool.Get();
			this.root.transform.localScale = Vector3.one * 0.3f;
			TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.root.transform, Vector3.one, 0.2f), 27)), delegate
			{
				this.root.transform.localScale = Vector3.one;
				this.mAttributeUpdateItem.Rebuild();
			}), true);
			if (this.uiData.sizeType == EventSizeType.BigWin)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.sweepLight.gameObject.SetActiveSafe(true);
					this.sweepLight.Play("Anim_UIFX_NextDay_SweepLight_Red", 0, 0f);
				});
				return;
			}
			if (this.uiData.sizeType == EventSizeType.MinorWin)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.sweepLight.gameObject.SetActiveSafe(true);
					this.sweepLight.Play("Anim_UIFX_NextDay_SweepLight_Blue", 0, 0f);
				});
				return;
			}
			if (this.uiData.sizeType == EventSizeType.Activity)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.sweepLight.gameObject.SetActiveSafe(true);
					this.sweepLight.Play("Anim_UIFX_NextDay_SweepLight_Purple", 0, 0f);
				});
			}
		}

		public float GetHeight()
		{
			return this.rectTransform.sizeDelta.y;
		}

		public void CloseInfo()
		{
			this.m_textInfoSeq.Clear(false);
			this.eventLoading.SetActiveSafe(false);
		}

		public void ShowLoading(bool isShow)
		{
			this.eventLoading.SetActiveSafe(isShow);
		}

		public static string GetSpriteBg(EventSizeType sizeType)
		{
			switch (sizeType)
			{
			case EventSizeType.Fail:
				return "battlenew_text_bg_fail";
			case EventSizeType.MinorWin:
				return "battlenew_text_bg_win";
			case EventSizeType.BigWin:
				return "battlenew_text_bg_bigwin";
			case EventSizeType.Activity:
				return "battlenew_text_bg_activity";
			}
			return "battle_event_text_bg";
		}

		private void InitTextSlot()
		{
			string text = "";
			List<string> list = new List<string>();
			IList<Chapter_eventRes> allElements = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Chapter_eventRes chapter_eventRes = allElements[i];
				if (chapter_eventRes.id == this.uiData.eventResId)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID(chapter_eventRes.randomId);
				}
				else if (!string.IsNullOrEmpty(chapter_eventRes.randomId))
				{
					list.Add(Singleton<LanguageManager>.Instance.GetInfoByID(chapter_eventRes.randomId));
				}
			}
			int num = 20;
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.textSlotItem.gameObject);
				gameObject.SetParentNormal(this.textSlotParent, false);
				CustomText component = gameObject.GetComponent<CustomText>();
				this.textSlotItemList.Add(component);
				if (j == 0)
				{
					component.text = text;
				}
				else
				{
					int num2 = Utility.Math.Random(0, list.Count);
					component.text = list[num2];
				}
			}
		}

		private void PlaySlotAni()
		{
			Sequence sequence = DOTween.Sequence();
			int num = 100;
			float num2 = 1.5f;
			int num3 = this.textSlotItemList.Count * num;
			for (int i = 0; i < this.textSlotItemList.Count; i++)
			{
				RectTransform component = this.textSlotItemList[i].gameObject.GetComponent<RectTransform>();
				int num4 = (this.textSlotItemList.Count - i) * num;
				component.anchoredPosition = new Vector2(0f, (float)num4);
				float num5 = (float)(num4 - num3);
				if (i == 0)
				{
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(component, num5, num2, false));
				}
				else
				{
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(component, num5, num2, false));
				}
			}
			TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.RefreshInfo));
			float num6 = 0.1f;
			Sequence sequence2 = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence2, num6);
			this.root.transform.localScale = Vector3.one;
			TweenSettingsExtensions.Append(sequence2, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.root.transform, Vector3.one * 1.05f, 1.3f), 9));
			TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence2, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.root.transform, Vector3.one * 0.7f, 0.1f), 8)), ShortcutExtensions.DOScale(this.root.transform, Vector3.one, 0.1f));
			TweenSettingsExtensions.AppendInterval(sequence2, num6);
			TweenSettingsExtensions.AppendCallback(sequence2, delegate
			{
				this.PlayAttributeAni();
				if (this.uiEventController)
				{
					this.uiEventController.TextSlotEnd();
				}
			});
		}

		public Transform GetFlyTarget()
		{
			return this.textResult.transform;
		}

		public void ShowResultText()
		{
			Sequence sequence = DOTween.Sequence();
			this.textResult.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textResult.transform, Vector3.one * 1.2f, 0.2f)), ShortcutExtensions.DOScale(this.textResult.transform, Vector3.one, 0.1f));
		}

		public GameObject root;

		public CustomText textStage;

		public CustomText textEvent;

		public GameObject attributeUpdateNode;

		public UIAttributeUpdateItem mAttributeUpdateItem;

		public GameObject eventLoading;

		public GameObject textSlotObj;

		public GameObject textSlotParent;

		public CustomText textSlotItem;

		public CustomImage imageBg;

		public CustomText textResult;

		public CustomOutLine resultOutline;

		public Animator sweepLight;

		public GameObject dayObj;

		public GameObject lineObj;

		private float oldHeight;

		private GameEventUIData uiData;

		private List<UIGameEventButtonItem> buttonItems = new List<UIGameEventButtonItem>();

		private RectTransform rectTransform;

		private RectTransform imageRectTrans;

		private SequencePool seqPool = new SequencePool();

		private SequencePool m_textInfoSeq = new SequencePool();

		private Color m_textInfoColor;

		public const float OneLine_Height = 100f;

		public const float AddHeight = 30f;

		public const string TextColor_MinorWin = "#cdfbff";

		public const string TextColor_BigWin = "#fff392";

		public const string TextColor_Fail = "#dddbf4";

		public const string TextColor_Activity = "#FFF39A";

		public const string OutlineColor_MinorWin = "#0f4677";

		public const string OutlineColor_BigWin = "#701d35";

		public const string OutlineColor_Fail = "#000000";

		public const string OutlineColor_Activity = "#481F7D";

		private UIEventController uiEventController;

		private List<CustomText> textSlotItemList = new List<CustomText>();

		private int mIndex;
	}
}
