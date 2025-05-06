using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattlePauseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.settingData = GameApp.Data.GetDataModule(DataName.SettingModule);
			this.BtnContinue.Init();
			this.BtnContinue.SetData(new Action(this.OnBtnContinueHandler));
			this.BtnContinue.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("UIBattlePause_82"));
			this.BtnBack.onClick.AddListener(new UnityAction(this.OnBtnBackHandler));
			this.BtnAudio.onClick.AddListener(new UnityAction(this.OnBtnAudioHandler));
			this.btnLanguage.onClick.AddListener(new UnityAction(this.OnBtnLanguage));
			this.buttonLog.onClick.AddListener(new UnityAction(this.OnClickLog));
			this.skillObj.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			DelayCall.Instance.SetPause(true);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
			this.RefreshSkills();
			if (this.settingData.GetValue(SettingDataName.Background) || this.settingData.GetValue(SettingDataName.SoundEffect))
			{
				this.BtnAudio.SetSelect(true);
				return;
			}
			this.BtnAudio.SetSelect(false);
		}

		private void RefreshAudio()
		{
			if (this.settingData.GetValue(SettingDataName.Background))
			{
				GameApp.Sound.SetBackgroundVolume(0f);
			}
			else
			{
				GameApp.Sound.SetBackgroundVolume(-80f);
			}
			if (this.settingData.GetValue(SettingDataName.SoundEffect))
			{
				GameApp.Sound.SetSoundEffectOpen(true);
				return;
			}
			GameApp.Sound.SetSoundEffectOpen(false);
		}

		public override void OnClose()
		{
			EventArgsBool instance = Singleton<EventArgsBool>.Instance;
			instance.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Pause, instance);
			DelayCall.Instance.SetPause(false);
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
			this.m_seqPool.Clear(false);
			for (int i = 0; i < this.skillItemList.Count; i++)
			{
				this.skillItemList[i].DeInit();
			}
		}

		public override void OnDelete()
		{
			this.BtnContinue.DeInit();
			this.BtnBack.onClick.RemoveListener(new UnityAction(this.OnBtnBackHandler));
			this.BtnAudio.onClick.RemoveListener(new UnityAction(this.OnBtnAudioHandler));
			this.btnLanguage.onClick.RemoveListener(new UnityAction(this.OnBtnLanguage));
			this.buttonLog.onClick.RemoveListener(new UnityAction(this.OnClickLog));
			this.skillItemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnBtnContinueHandler()
		{
			GameApp.View.CloseView(ViewName.BattlePauseViewModule, null);
		}

		private void OnBtnBackHandler()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("UIBattlePause_CloseTip");
			string popCommonTitle = DxxTools.UI.GetPopCommonTitle();
			string popCommonSure = DxxTools.UI.GetPopCommonSure();
			string popCommonCancel = DxxTools.UI.GetPopCommonCancel();
			DxxTools.UI.OpenPopCommon(infoByID, delegate(int result)
			{
				if (result == -1)
				{
					ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
					GameTGATools.Ins.ChapterEndQuitType = 1;
					int currentStage = Singleton<GameEventController>.Instance.GetCurrentStage();
					dataModule.FinishEvent(currentStage, -1, false);
				}
			}, popCommonTitle, popCommonCancel, popCommonSure, true, 2);
		}

		private void OnBtnAudioHandler()
		{
			this.settingData.SetValue(SettingDataName.Background, !this.BtnAudio.IsSelected);
			this.settingData.SetValue(SettingDataName.SoundEffect, !this.BtnAudio.IsSelected);
			this.RefreshAudio();
		}

		private void RefreshSkills()
		{
			List<GameEventSkillBuildData> list = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkillBuildList();
			this.textNoSkill.SetActive(list.Count == 0);
			for (int j = 0; j < this.skillItemList.Count; j++)
			{
				this.skillItemList[j].gameObject.SetActiveSafe(false);
			}
			int i;
			int i2;
			for (i = 0; i < list.Count; i = i2 + 1)
			{
				int index = i;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)i * 0.05f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					UIGameEventLearnedSkillItem uigameEventLearnedSkillItem;
					if (i < this.skillItemList.Count)
					{
						uigameEventLearnedSkillItem = this.skillItemList[index];
						uigameEventLearnedSkillItem.Init();
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.skillObj);
						gameObject.transform.SetParentNormal(this.gridLayoutGroup.transform, false);
						uigameEventLearnedSkillItem = gameObject.GetComponent<UIGameEventLearnedSkillItem>();
						uigameEventLearnedSkillItem.Init();
						this.skillItemList.Add(uigameEventLearnedSkillItem);
					}
					uigameEventLearnedSkillItem.gameObject.SetActiveSafe(true);
					uigameEventLearnedSkillItem.Refresh(list[index]);
					uigameEventLearnedSkillItem.PlayShowAnimation();
				});
				i2 = i;
			}
		}

		private void OnShowDetailInfo(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgClickSkill eventArgClickSkill = eventargs as EventArgClickSkill;
			if (eventArgClickSkill == null)
			{
				return;
			}
			GameEventSkillBuildData skillBuildData = eventArgClickSkill.skillItem.GetSkillBuildData();
			new InfoTipViewModule.InfoTipData
			{
				m_name = skillBuildData.skillName,
				m_info = skillBuildData.skillFullDetail,
				m_position = eventArgClickSkill.skillItem.GetPosition(),
				m_offsetY = 280f
			}.Open();
		}

		private void OnBtnLanguage()
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			EventArgLanguageType instance = Singleton<EventArgLanguageType>.Instance;
			if (dataModule.GetCurrentLanguageType == null)
			{
				instance.SetData(2);
			}
			else
			{
				instance.SetData(0);
			}
			GameApp.Event.DispatchNow(this, 1, instance);
			CustomLanguageText[] array = Object.FindObjectsOfType<CustomLanguageText>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnRefresh();
			}
		}

		private void OnClickLog()
		{
		}

		public UIOneButtonCtrl BtnContinue;

		public CustomButton BtnBack;

		public CustomChooseButton BtnAudio;

		public GridLayoutGroup gridLayoutGroup;

		public GameObject skillObj;

		public GameObject textNoSkill;

		public CustomButton btnLanguage;

		public CustomButton buttonLog;

		private List<UIGameEventLearnedSkillItem> skillItemList = new List<UIGameEventLearnedSkillItem>();

		private SequencePool m_seqPool = new SequencePool();

		private SettingDataModule settingData;
	}
}
