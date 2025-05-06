using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GetSkillViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.selectSkillItem.Init();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GetSkillViewModule.OpenData;
			if (this.openData == null)
			{
				GameApp.View.CloseView(ViewName.GetSkillViewModule, null);
				return;
			}
			this.buttonLearn.onClick.AddListener(new UnityAction(this.OnClickLearn));
			GameApp.Sound.PlayClip(58, 1f);
			this.modelItem.Init();
			this.modelItem.OnShow();
			this.modelItem.ShowSelfPlayerModel("GetSkillViewModule", false);
			if (this.openData.skillBuildId > 0)
			{
				GameEventSkillBuildData specifiedSkill = Singleton<GameEventController>.Instance.GetSpecifiedSkill(this.openData.skillBuildId);
				if (specifiedSkill == null)
				{
					HLog.LogError(string.Format("Not found skillBuildId={0}", this.openData.skillBuildId));
					return;
				}
				this.buildData = specifiedSkill;
			}
			else
			{
				SkillBuildSourceType sourceType = this.openData.sourceType;
				List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(sourceType, 1, this.openData.seed);
				if (randomSkillList == null || randomSkillList.Count == 0)
				{
					HLog.LogError("Random skill is null");
					return;
				}
				this.buildData = randomSkillList[0];
			}
			Singleton<GameEventController>.Instance.SelectSkill(this.buildData, false);
			this.selectSkillItem.Refresh(this.buildData, null);
			this.PlayOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.modelItem.OnHide(false);
			this.modelItem.DeInit();
			this.buttonLearn.onClick.RemoveListener(new UnityAction(this.OnClickLearn));
		}

		public override void OnDelete()
		{
		}

		private void OnClickLearn()
		{
			EventArgSelectSkillEnd instance = Singleton<EventArgSelectSkillEnd>.Instance;
			instance.SetData(new List<GameEventSkillBuildData> { this.buildData });
			GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { this.buildData }, true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSelectSkill, instance);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			GameApp.View.CloseView(ViewName.GetSkillViewModule, null);
		}

		private void PlayOpenAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.downAni.transform.localPosition = new Vector3(0f, 500f, 0f);
			this.selectSkillItem.transform.localScale = Vector3.zero;
			this.buttonLearn.transform.localScale = Vector3.zero;
			float num = 0.2f;
			float num2 = 0.1f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(this.downAni.transform, 0f, num, false));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.selectSkillItem.transform, Vector3.one * 1.1f, num2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.selectSkillItem.transform, Vector3.one, num2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonLearn.transform, Vector3.one * 1.1f, num2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonLearn.transform, Vector3.one, num2));
		}

		public GameObject downAni;

		public CustomButton buttonLearn;

		public UIModelItem modelItem;

		public UIGameEventSelectSkillItem selectSkillItem;

		private GameEventSkillBuildData buildData;

		private GetSkillViewModule.OpenData openData;

		private SequencePool sequencePool = new SequencePool();

		public class OpenData
		{
			public int skillBuildId;

			public SkillBuildSourceType sourceType;

			public int seed;
		}
	}
}
