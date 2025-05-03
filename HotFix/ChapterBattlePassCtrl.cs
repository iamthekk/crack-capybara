using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterBattlePassCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterBattlePass_RefreshData, new HandlerEvent(this.OnEventRefreshData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterBattlePass_RefreshScore, new HandlerEvent(this.OnEventRefreshScore));
			this.buttonActivity.onClick.AddListener(new UnityAction(this.OnClickActivity));
			this.progressCtrl.Init();
			this.buttonActivity.gameObject.SetActiveSafe(false);
			RedPointController.Instance.RegRecordChange("Main.ChapterBattlePass", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterBattlePass_RefreshData, new HandlerEvent(this.OnEventRefreshData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterBattlePass_RefreshScore, new HandlerEvent(this.OnEventRefreshScore));
			this.buttonActivity.onClick.RemoveListener(new UnityAction(this.OnClickActivity));
			this.progressCtrl.DeInit();
			RedPointController.Instance.UnRegRecordChange("Main.ChapterBattlePass", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isEnd)
			{
				return;
			}
			if (this.dataModule == null || this.dataModule.BattlePassDto == null)
			{
				return;
			}
			if (this.dataModule.IsEnd())
			{
				this.isEnd = true;
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData(3);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_CheckShow, eventArgsInt);
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			this.lastTime += deltaTime;
			if (this.textTime.gameObject.activeInHierarchy && this.lastTime > 1f)
			{
				this.lastTime -= 1f;
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(this.dataModule.BattlePassDto.EndTime - serverTimestamp);
			}
		}

		public void SetChapterId(int id, bool isShowGot)
		{
			this.chapterId = id;
			this.isShowGotoBtn = isShowGot;
			this.LoadBg();
		}

		private void OnClickActivity()
		{
			if (this.isClickDisabled)
			{
				return;
			}
			ChapterBattlePassViewModule.OpenData openData = new ChapterBattlePassViewModule.OpenData();
			openData.chapterId = this.chapterId;
			openData.isShowGotoBtn = this.isShowGotoBtn;
			GameApp.View.OpenView(ViewName.ChapterBattlePassViewModule, openData, 1, null, null);
		}

		public void SetData(bool isPlayAni)
		{
			this.isEnd = false;
			if (this.dataModule.BattlePassDto == null || !this.dataModule.IsInProgress())
			{
				this.buttonActivity.gameObject.SetActiveSafe(false);
				return;
			}
			this.battlePass = GameApp.Table.GetManager().GetChapterActivity_Battlepass(this.dataModule.BattlePassDto.ConfigId);
			if (this.battlePass != null)
			{
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.battlePass.name);
				this.buttonActivity.gameObject.SetActiveSafe(true);
				this.progressCtrl.SetAnimationDisabled(!isPlayAni);
				this.progressCtrl.SetData(this.battlePass.atlasID, this.battlePass.itemIcon, this.dataModule.uiDataList, null, new Action(this.OnProgressAniFinish));
				this.progressCtrl.SetStatus(this.dataModule.BattlePassDto.Score, this.dataModule.BattlePassDto.FinalRewardCount, this.dataModule.GetFinalRewardLimit());
			}
			else
			{
				this.buttonActivity.gameObject.SetActiveSafe(false);
			}
			this.redNode.SetActiveSafe(this.dataModule.IsRedPoint());
		}

		public void PlayAni(bool isShow)
		{
			if (isShow)
			{
				this.PlayShowAni();
				return;
			}
			this.PlayHideAni();
		}

		private void PlayShowAni()
		{
			this.child.gameObject.SetActiveSafe(true);
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = new Vector2(this.child.anchoredPosition.x, 600f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.3f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 0f, 0.2f, false));
		}

		private void PlayHideAni()
		{
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = Vector2.zero;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 600f, 0.3f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.child.gameObject.SetActiveSafe(false);
			});
		}

		public Transform GetFlyNode()
		{
			return this.flyNode;
		}

		private Task LoadBg()
		{
			ChapterBattlePassCtrl.<LoadBg>d__25 <LoadBg>d__;
			<LoadBg>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadBg>d__.<>4__this = this;
			<LoadBg>d__.<>1__state = -1;
			<LoadBg>d__.<>t__builder.Start<ChapterBattlePassCtrl.<LoadBg>d__25>(ref <LoadBg>d__);
			return <LoadBg>d__.<>t__builder.Task;
		}

		public void SetClickDisabled(bool isDisabled)
		{
			this.isClickDisabled = isDisabled;
		}

		private void OnEventRefreshData(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.dataModule.BattlePassDto != null)
			{
				this.progressCtrl.SetStatus(this.dataModule.BattlePassDto.Score, this.dataModule.BattlePassDto.FinalRewardCount, this.dataModule.GetFinalRewardLimit());
			}
		}

		private void OnEventRefreshScore(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.dataModule.BattlePassDto != null)
			{
				if (this.progressCtrl.isAnimationDisabled)
				{
					this.progressCtrl.SetStatus(this.dataModule.BattlePassDto.Score, this.dataModule.BattlePassDto.FinalRewardCount, this.dataModule.GetFinalRewardLimit());
					return;
				}
				EventArgsInt eventArgsInt = eventArgs as EventArgsInt;
				if (eventArgsInt != null)
				{
					this.progressCtrl.PlayAnimation(eventArgsInt.Value, this.dataModule.BattlePassDto.Score);
				}
			}
		}

		private void OnProgressAniFinish()
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter)
			{
				EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
				eventArgsShowActivity.SetData(new List<ChapterActivityKind> { ChapterActivityKind.BattlePass }, false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
			}
		}

		private void OnRedPointChange(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		public RectTransform child;

		public CustomButton buttonActivity;

		public CommonProgressCtrl progressCtrl;

		public CustomText textTitle;

		public CustomText textTime;

		public RectTransform flyNode;

		public CustomImage imageBg;

		public GameObject redNode;

		private ChapterBattlePassDataModule dataModule;

		private ChapterActivity_Battlepass battlePass;

		private bool isClickDisabled;

		private int chapterId;

		private bool isShowGotoBtn;

		private bool isEnd;

		private float lastTime;
	}
}
