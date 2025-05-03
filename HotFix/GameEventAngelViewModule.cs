using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventAngelViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 939;
		}

		public override void OnCreate(object data)
		{
			this.buttonRecover.onClick.AddListener(new UnityAction(this.OnSelectRecover));
			this.buttonLearnedSkills.onClick.AddListener(new UnityAction(this.OnClickLearnedSkills));
			this.spineModelItem.Init();
			this.selectSkillItem.Init();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GameEventAngelViewModule.OpenData;
			if (this.openData == null)
			{
				this.CloseSelf();
				return;
			}
			GameApp.Sound.PlayClip(59, 1f);
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(this.openData.sourceType, 1, this.openData.seed);
			this.skillBuild = randomSkillList[0];
			GameTGATools.Ins.AddStageClickTempSkillShow(new List<GameEventSkillBuildData> { this.skillBuild });
			this.Refresh();
			this.PlayOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
		}

		public override void OnDelete()
		{
			this.buttonRecover.onClick.RemoveListener(new UnityAction(this.OnSelectRecover));
			this.buttonLearnedSkills.onClick.RemoveListener(new UnityAction(this.OnClickLearnedSkills));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private async void Refresh()
		{
			if (this.openData != null)
			{
				if (this.openData.modelId > 0)
				{
					await this.spineModelItem.ShowModel(this.openData.modelId, "Idle", true);
				}
				else if (this.openData.memberId > 0)
				{
					await this.spineModelItem.ShowMemberModel(this.openData.memberId, "Idle", true);
				}
				this.spineModelItem.SetAnimationTimeScale(0.5f);
				this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventAngel_91");
				this.recoverHp = (Singleton<GameEventController>.Instance.PlayerData.HpMax * new FP((long)GameConfig.Angel_Recover_HpRate, 100L)).AsLong();
				this.textDes.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventAngel_101", new object[] { DxxTools.FormatNumber(this.recoverHp) });
				this.selectSkillItem.Refresh(this.skillBuild, new Action<GameEventSkillBuildData>(this.OnSelectSkill));
			}
		}

		private void CloseSelf()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			GameApp.View.CloseView(ViewName.GameEventAngelViewModule, null);
		}

		private void OnSelectSkill(GameEventSkillBuildData data)
		{
			Singleton<GameEventController>.Instance.SelectSkill(data, false);
			GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { data }, true);
			this.CloseSelf();
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseAngel, data);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData> { data };
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(list, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		private void OnSelectRecover()
		{
			NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.RecoverHpRate, (double)GameConfig.Angel_Recover_HpRate, ChapterDropSource.Event, 1);
			GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
			Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
			this.CloseSelf();
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseAngel, this.recoverHp);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(null, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		private void OnClickLearnedSkills()
		{
			GameApp.View.OpenView(ViewName.LearnedSkillsViewModule, null, 1, null, null);
		}

		private void PlayOpenAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.downObj.transform.localPosition = new Vector3(0f, 500f, 0f);
			Color color = this.imageLight.color;
			color.a = 0f;
			this.imageLight.color = color;
			this.textTip.transform.localScale = Vector3.zero;
			this.selectSkillItem.transform.localScale = Vector3.zero;
			this.recoverHpObj.transform.localScale = Vector3.zero;
			float num = 0.2f;
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(this.downObj.transform, 0f, num, false)), delegate
			{
				this.downObj.transform.localPosition = Vector3.zero;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageLight, 1f, num)), delegate
			{
				color = this.imageLight.color;
				color.a = 1f;
				this.imageLight.color = color;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.textTip.transform, Vector3.one, num), 22)), delegate
			{
				this.textTip.transform.localScale = Vector3.one;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.selectSkillItem.transform, Vector3.one, num), 22)), delegate
			{
				this.selectSkillItem.transform.localScale = Vector3.one;
			});
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.selectSkillItem.PlayEffect(this.skillBuild.quality);
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.recoverHpObj.transform, Vector3.one, num), 22)), delegate
			{
				this.recoverHpObj.transform.localScale = Vector3.one;
			});
		}

		[GameTestMethod("事件", "天使", "", 411)]
		private static void OpenGameEventAngel()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			GameEventAngelViewModule.OpenData openData = new GameEventAngelViewModule.OpenData();
			openData.seed = random.Next();
			GameApp.View.OpenView(ViewName.GameEventAngelViewModule, openData, 1, null, null);
		}

		public UISpineModelItem spineModelItem;

		public UIGameEventSelectSkillItem selectSkillItem;

		public CustomText textName;

		public CustomText textDes;

		public CustomButton buttonRecover;

		public Image imageLight;

		public GameObject downObj;

		public GameObject textTip;

		public GameObject recoverHpObj;

		public CustomButton buttonLearnedSkills;

		private GameEventSkillBuildData skillBuild;

		private long recoverHp;

		private GameEventAngelViewModule.OpenData openData;

		private SequencePool sequencePool = new SequencePool();

		public class OpenData
		{
			public int modelId;

			public int memberId;

			public int seed;

			public SkillBuildSourceType sourceType = SkillBuildSourceType.Angel;
		}
	}
}
