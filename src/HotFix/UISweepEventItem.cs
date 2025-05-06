using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UISweepEventItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rectTransform = base.gameObject.GetComponent<RectTransform>();
			this.imageRectTrans = this.imageBg.GetComponent<RectTransform>();
			this.mAttributeUpdateItem.Init();
			this.eventLoading.SetActiveSafe(false);
			this.sweepLight.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.seqPool.Clear(false);
			this.uiData = null;
			if (this.mAttributeUpdateItem != null)
			{
				this.mAttributeUpdateItem.DeInit();
				this.mAttributeUpdateItem = null;
			}
		}

		public void Refresh(GameEventUIData data, int index, int totalCount)
		{
			if (data == null)
			{
				return;
			}
			this.uiData = data;
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
			this.RefreshInfo();
			if (index == totalCount - 1)
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
			this.textEvent.gameObject.SetActiveSafe(true);
			this.textEvent.text = this.uiData.info;
			this.mAttributeUpdateItem.SetData(this.uiData.GetAttParamList(), this.uiData.GetItemParamList(), this.uiData.GetSkillParamList(), this.uiData.GetInfoParamList(), this.uiData.GetScoreParamList());
			this.attributeUpdateNode.SetActiveSafe(this.uiData.IsHaveParams());
			if (this.uiData.CacheTotalHeight == 0f && this.uiData.CacheBgHeight == 0f)
			{
				float num;
				if (this.uiData.isRoot)
				{
					num = this.dayObj.GetComponent<RectTransform>().sizeDelta.y;
				}
				else
				{
					num = this.lineObj.GetComponent<RectTransform>().sizeDelta.y;
				}
				float num2 = 100f;
				num2 += this.textEvent.preferredHeight + 30f;
				int itemCount = this.mAttributeUpdateItem.GetItemCount();
				if (itemCount > 3)
				{
					num2 += 200f;
				}
				else if (itemCount > 0)
				{
					num2 += 100f;
				}
				float num3 = num2 + num;
				this.uiData.CacheBgHeight = num2;
				this.uiData.CacheTotalHeight = num3;
			}
			this.imageRectTrans.sizeDelta = new Vector2(this.imageRectTrans.sizeDelta.x, this.uiData.CacheBgHeight);
			this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.uiData.CacheTotalHeight);
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
			this.root.transform.localScale = Vector3.one * 0.7f;
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
				return;
			}
			this.sweepLight.gameObject.SetActiveSafe(false);
		}

		public float GetHeight()
		{
			return this.rectTransform.sizeDelta.y;
		}

		public GameObject root;

		public CustomText textStage;

		public CustomText textEvent;

		public GameObject attributeUpdateNode;

		public UIAttributeUpdateItem mAttributeUpdateItem;

		public GameObject eventLoading;

		public CustomImage imageBg;

		public CustomText textResult;

		public CustomOutLine resultOutline;

		public Animator sweepLight;

		public GameObject dayObj;

		public GameObject lineObj;

		private GameEventUIData uiData;

		private RectTransform rectTransform;

		private RectTransform imageRectTrans;

		private SequencePool seqPool = new SequencePool();
	}
}
