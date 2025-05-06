using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Chapter;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GameEventFinishViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonContinue.Init();
			this.buttonContinue.SetData(new Action(this.OnClickContinue));
			this.buttonLearnSkill.onClick.AddListener(new UnityAction(this.OnClickLearnSkill));
			this.itemDefault.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			GameEventFinishViewModule.EventEndData eventEndData = data as GameEventFinishViewModule.EventEndData;
			if (eventEndData != null)
			{
				this.eventEndData = eventEndData;
				this.Refresh();
				this.ResetAni();
				base.StartCoroutine(this.PlayResultAni());
				if (this.eventEndData.isPass)
				{
					GameApp.Sound.PlayClip(88, 1f);
					return;
				}
				GameApp.Sound.PlayClip(89, 1f);
			}
		}

		private IEnumerator PlayResultAni()
		{
			if (this.eventEndData.isPass)
			{
				float duration = this.spineWin.SkeletonData.FindAnimation("ShengLi_1").Duration;
				this.spineWin.AnimationState.SetAnimation(0, "ShengLi_1", false);
				yield return new WaitForSeconds(duration);
				this.winEffect.SetActiveSafe(true);
				this.spineWin.AnimationState.SetAnimation(0, "ShengLi_2", true);
				this.PlayWinAni();
			}
			else
			{
				float duration2 = this.spineLose.SkeletonData.FindAnimation("ShiBai_1").Duration;
				this.spineLose.AnimationState.SetAnimation(0, "ShiBai_1", false);
				yield return new WaitForSeconds(duration2);
				this.spineLose.AnimationState.SetAnimation(0, "ShiBai_2", true);
				this.PlayFailAni();
			}
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
			this.buttonLearnSkill.onClick.RemoveListener(new UnityAction(this.OnClickLearnSkill));
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
			this.victoryObj.SetActiveSafe(this.eventEndData.isPass);
			this.failObj.SetActiveSafe(!this.eventEndData.isPass);
			this.textStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_Stage", new object[] { this.eventEndData.endStage });
			this.textChapter.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_45", new object[] { this.eventEndData.chapterId });
			this.textMaxStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_46", new object[] { this.eventEndData.maxStage });
			if (this.victoryObj.activeSelf)
			{
				this.stageOutline.effectColor = this.outlineWin;
			}
			else
			{
				this.stageOutline.effectColor = this.outlineFail;
			}
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

		private void OnClickContinue()
		{
			this.OnCloseSelf();
		}

		private void OnContinue()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if ((ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum < (ulong)((long)dataModule.CurrentChapter.CostEnergy))
			{
				EventArgsString eventArgsString = new EventArgsString();
				eventArgsString.SetData("体力不足");
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, eventArgsString);
				return;
			}
			NetworkUtils.Chapter.DoStartChapterRequest(dataModule.ChapterID, delegate(bool result, StartChapterResponse response)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
					{
						GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
						{
							GameApp.View.CloseView(ViewName.GameEventViewModule, null);
							GameApp.View.CloseView(ViewName.GameEventFinishViewModule, null);
							GameApp.State.ActiveState(StateName.BattleChapterState);
						});
					});
				}
			});
		}

		private void OnCloseSelf()
		{
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					GameApp.View.CloseView(ViewName.GameEventViewModule, null);
					GameApp.View.CloseView(ViewName.GameEventFinishViewModule, null);
					if (GameApp.View.IsOpened(ViewName.BattlePauseViewModule))
					{
						GameApp.View.CloseView(ViewName.BattlePauseViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.SelectSkillViewModule))
					{
						GameApp.View.CloseView(ViewName.SelectSkillViewModule, null);
					}
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private void OnClickLearnSkill()
		{
			GameApp.View.OpenView(ViewName.LearnedSkillsViewModule, null, 1, null, null);
		}

		private void ResetAni()
		{
			this.winEffect.SetActiveSafe(false);
			this.winLightObj.SetActiveSafe(false);
			this.winTextObj.SetActiveSafe(false);
			this.winSurviveTextObj.SetActiveSafe(false);
			this.winRewardBgTrans.sizeDelta = new Vector2(this.winRewardBgTrans.sizeDelta.x, 0f);
			this.failLightObj.SetActiveSafe(false);
			this.failTextObj.SetActiveSafe(false);
			this.failSurviveTextObj.SetActiveSafe(false);
			this.failRewardBgTrans.sizeDelta = new Vector2(this.failRewardBgTrans.sizeDelta.x, 0f);
			this.textChapter.gameObject.SetActiveSafe(false);
			this.textStage.gameObject.SetActiveSafe(false);
			this.textMaxStage.gameObject.SetActiveSafe(false);
			this.textNoReward.gameObject.SetActiveSafe(false);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].transform.localScale = Vector3.zero;
			}
			this.buttonContinue.transform.localScale = Vector3.zero;
			this.buttonLearnSkill.transform.localScale = Vector3.zero;
		}

		private void PlayWinAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.15f;
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

		private void PlayFailAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.15f;
			this.failTextObj.SetActiveSafe(true);
			this.failTextObj.transform.localScale = Vector3.one * 3f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.failTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textChapter.gameObject.SetActiveSafe(true);
				this.textChapter.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textChapter.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.failSurviveTextObj.SetActiveSafe(true);
				this.failSurviveTextObj.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.failSurviveTextObj.transform, Vector3.one, num));
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
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOSizeDelta(this.failRewardBgTrans, new Vector2(this.failRewardBgTrans.sizeDelta.x, 750f), num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.failLightObj.SetActiveSafe(true);
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
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(transform, Vector3.one * 1.1f, 0.15f)), ShortcutExtensions.DOScale(transform, Vector3.one, 0.05f));
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
				float num = 0.15f;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textNoReward, 1f, num));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(component, -235f, num, false));
			}
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one * 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one, 0.05f));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonLearnSkill.transform, Vector3.one * 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.buttonLearnSkill.transform, Vector3.one, 0.05f));
		}

		[GameTestMethod("事件", "章节胜利", "", 401)]
		private static void OpenGameEventWin()
		{
			GameEventFinishViewModule.EventEndData eventEndData = new GameEventFinishViewModule.EventEndData();
			eventEndData.chapterId = 1;
			eventEndData.endStage = 60;
			eventEndData.result = 1;
			eventEndData.isPass = true;
			eventEndData.maxStage = 60;
			GameApp.View.OpenView(ViewName.GameEventFinishViewModule, eventEndData, 1, null, null);
		}

		[GameTestMethod("事件", "章节失败", "", 402)]
		private static void OpenGameEventFail()
		{
			GameEventFinishViewModule.EventEndData eventEndData = new GameEventFinishViewModule.EventEndData();
			eventEndData.chapterId = 1;
			eventEndData.endStage = 59;
			eventEndData.result = -1;
			eventEndData.isPass = false;
			eventEndData.maxStage = 59;
			GameApp.View.OpenView(ViewName.GameEventFinishViewModule, eventEndData, 1, null, null);
		}

		public GameObject winEffect;

		public SkeletonGraphic spineWin;

		public SkeletonGraphic spineLose;

		public CustomText textStage;

		public CustomText textChapter;

		public CustomText textMaxStage;

		public CustomText textNoReward;

		public GameObject gridObj;

		public UIOneButtonCtrl buttonContinue;

		public GameObject victoryObj;

		public GameObject failObj;

		public CustomOutLine stageOutline;

		public GameObject itemDefault;

		public GameObject winLightObj;

		public GameObject winTextObj;

		public GameObject winSurviveTextObj;

		public RectTransform winRewardBgTrans;

		public GameObject failLightObj;

		public GameObject failTextObj;

		public GameObject failSurviveTextObj;

		public RectTransform failRewardBgTrans;

		public CustomButton buttonLearnSkill;

		public Color outlineWin;

		public Color outlineFail;

		private GameEventFinishViewModule.EventEndData eventEndData;

		private List<UIItem> itemList = new List<UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		public class EventEndData
		{
			public int chapterId;

			public int endStage;

			public int result;

			public int maxStage;

			public int currentTask;

			public bool isPass;

			public List<ItemData> rewardList;
		}
	}
}
