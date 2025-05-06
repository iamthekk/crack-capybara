using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class ChapterSweepFinishViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonContinue.Init();
			this.buttonContinue.SetData(new Action(this.OnCloseSelf));
			this.itemDefault.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			GameApp.Sound.PlayClip(88, 1f);
			ChapterSweepFinishViewModule.EventEndData eventEndData = data as ChapterSweepFinishViewModule.EventEndData;
			if (eventEndData != null)
			{
				this.eventEndData = eventEndData;
				this.Refresh();
				this.ResetAni();
				base.StartCoroutine(this.PlayResultAni());
			}
		}

		private IEnumerator PlayResultAni()
		{
			float duration = this.spineWin.SkeletonData.FindAnimation("ShengLi_1").Duration;
			this.spineWin.AnimationState.SetAnimation(0, "ShengLi_1", false);
			yield return new WaitForSeconds(duration);
			this.winEffect.SetActiveSafe(true);
			this.spineWin.AnimationState.SetAnimation(0, "ShengLi_2", true);
			this.PlayWinAni();
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].gameObject.SetActiveSafe(false);
			}
		}

		public override void OnDelete()
		{
			this.buttonContinue.DeInit();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			if (this.eventEndData == null)
			{
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_PassBack");
			this.buttonContinue.SetText(infoByID);
			this.textStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_Stage", new object[] { this.eventEndData.endStage });
			this.textChapter.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_45", new object[] { this.eventEndData.chapterId });
			this.textMaxStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_46", new object[] { this.eventEndData.endStage });
			this.RefreshRewards();
		}

		private void RefreshRewards()
		{
			if (this.eventEndData.rewardList != null)
			{
				for (int i = 0; i < this.eventEndData.rewardList.Count; i++)
				{
					UIItem uiitem;
					if (i < this.itemList.Count)
					{
						uiitem = this.itemList[i];
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.itemDefault);
						gameObject.transform.SetParentNormal(this.gridObj.transform, false);
						uiitem = gameObject.GetComponent<UIItem>();
						uiitem.Init();
						this.itemList.Add(uiitem);
					}
					uiitem.gameObject.SetActiveSafe(true);
					uiitem.SetData(this.eventEndData.rewardList[i].ToPropData());
					uiitem.OnRefresh();
				}
				this.textNoReward.text = ((this.eventEndData.rewardList.Count > 0) ? "" : Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_NoReward"));
				return;
			}
			this.textNoReward.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_NoReward");
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.ChapterSweepFinishViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterSweep_PlayOpenAni, null);
		}

		private void ResetAni()
		{
			this.winEffect.SetActiveSafe(false);
			this.winLightObj.SetActiveSafe(false);
			this.winTextObj.SetActiveSafe(false);
			this.winSurviveTextObj.SetActiveSafe(false);
			this.winRewardBgTrans.sizeDelta = new Vector2(this.winRewardBgTrans.sizeDelta.x, 0f);
			this.textChapter.gameObject.SetActiveSafe(false);
			this.textStage.gameObject.SetActiveSafe(false);
			this.textMaxStage.gameObject.SetActiveSafe(false);
			this.textNoReward.gameObject.SetActiveSafe(false);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].transform.localScale = Vector3.zero;
			}
			this.buttonContinue.transform.localScale = Vector3.zero;
		}

		private void PlayWinAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.3f;
			this.winTextObj.SetActiveSafe(true);
			this.winTextObj.transform.localScale = Vector3.one * 3f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.winTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textChapter.gameObject.SetActiveSafe(true);
				this.textChapter.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textChapter.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.winSurviveTextObj.SetActiveSafe(true);
				this.winSurviveTextObj.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.winSurviveTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textStage.gameObject.SetActiveSafe(true);
				this.textStage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textStage.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textMaxStage.gameObject.SetActiveSafe(true);
				this.textMaxStage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textMaxStage.transform, Vector3.one, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOSizeDelta(this.winRewardBgTrans, new Vector2(this.winRewardBgTrans.sizeDelta.x, 750f), num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.winLightObj.SetActiveSafe(true);
				this.ShowRewardAni();
			});
		}

		private void ShowRewardAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			if (this.itemList.Count > 0)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					Transform transform = this.itemList[i].transform;
					transform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(transform, Vector3.one, 0.2f)), ShortcutExtensions.DOScale(transform, Vector3.one * 0.8f, 0.1f));
				}
			}
			else
			{
				this.textNoReward.gameObject.SetActiveSafe(true);
				RectTransform component = this.textNoReward.GetComponent<RectTransform>();
				Color color = this.textNoReward.color;
				color.a = 0f;
				this.textNoReward.color = color;
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, -300f);
				float num = 0.2f;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textNoReward, 1f, num));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(component, -235f, num, false));
			}
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one * 1.2f, 0.2f)), ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one, 0.1f));
		}

		[GameTestMethod("扫荡", "扫荡完成", "", 401)]
		private static void OpenSweepWin()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			ChapterSweepFinishViewModule.EventEndData eventEndData = new ChapterSweepFinishViewModule.EventEndData();
			eventEndData.chapterId = 1;
			eventEndData.endStage = dataModule.CurrentChapter.Config.journeyStage;
			eventEndData.rewardList = new List<ItemData>();
			GameApp.View.OpenView(ViewName.ChapterSweepFinishViewModule, eventEndData, 1, null, null);
		}

		public GameObject winEffect;

		public SkeletonGraphic spineWin;

		public CustomText textStage;

		public CustomText textChapter;

		public CustomText textMaxStage;

		public CustomText textNoReward;

		public GameObject gridObj;

		public UIOneButtonCtrl buttonContinue;

		public GameObject itemDefault;

		public GameObject winLightObj;

		public GameObject winTextObj;

		public GameObject winSurviveTextObj;

		public RectTransform winRewardBgTrans;

		private ChapterSweepFinishViewModule.EventEndData eventEndData;

		private List<UIItem> itemList = new List<UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		public class EventEndData
		{
			public int chapterId;

			public int endStage;

			public List<ItemData> rewardList;
		}
	}
}
