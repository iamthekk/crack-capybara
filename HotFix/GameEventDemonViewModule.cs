using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventDemonViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 938;
		}

		public override void OnCreate(object data)
		{
			this.spineModelItem.Init();
			this.skillItem.Init();
			this.buttonCancel.onClick.AddListener(new UnityAction(this.OnCancel));
			this.buttonDeal.onClick.AddListener(new UnityAction(this.OnDeal));
			this.buttonLearnedSkills.onClick.AddListener(new UnityAction(this.OnClickLearnedSkills));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GameEventDemonViewModule.OpenData;
			if (this.openData == null)
			{
				this.CloseSelf();
				return;
			}
			GameApp.Sound.PlayClip(59, 1f);
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(this.openData.sourceType, 1, this.openData.seed);
			this.skillBuild = randomSkillList[0];
			GameTGATools.Ins.AddStageClickTempSkillShow(new List<GameEventSkillBuildData> { this.skillBuild });
			this.costMaxHpPercent = ((this.skillBuild.cost > 0) ? (-this.skillBuild.cost) : this.skillBuild.cost);
			this.Refresh();
			this.PlayOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonCancel.onClick.RemoveListener(new UnityAction(this.OnCancel));
			this.buttonDeal.onClick.RemoveListener(new UnityAction(this.OnDeal));
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
				FP fp = this.costMaxHpPercent;
				fp.Abs();
				fp /= 100;
				this.m_costMaxHp = (Singleton<GameEventController>.Instance.PlayerData.HpMax * fp).AsLong();
				this.textLostHp.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventDemon_94", new object[] { DxxTools.FormatNumber(this.m_costMaxHp) });
				this.textGetSkill.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGetSkill_95", new object[] { this.skillBuild.skillName });
				this.skillItem.Refresh(this.skillBuild, null);
			}
		}

		private void OnCancel()
		{
			this.CloseSelf();
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseDemon, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(null, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		private void OnDeal()
		{
			Singleton<GameEventController>.Instance.SelectSkill(this.skillBuild, false);
			GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { this.skillBuild }, true);
			NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.HPMaxPercent, (double)this.costMaxHpPercent, ChapterDropSource.Event, 1);
			GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
			Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
			this.CloseSelf();
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseDemon, this.skillBuild);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
			EventMemberController.Instance.DoEventPoint(4);
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData> { this.skillBuild };
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("HPMax%", this.costMaxHpPercent);
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(list, dictionary);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		private void CloseSelf()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			GameApp.View.CloseView(ViewName.GameEventDemonViewModule, null);
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
			this.priceObj.transform.localScale = Vector3.zero;
			this.skillItem.transform.localScale = Vector3.zero;
			this.textGetSkill.transform.localScale = Vector3.zero;
			this.buttonDeal.transform.localScale = Vector3.zero;
			this.buttonCancel.transform.localScale = Vector3.zero;
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
			this.DoScale(sequence, this.textTip, Vector3.one, num);
			this.DoScale(sequence, this.skillItem.gameObject, Vector3.one, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.skillItem.PlayEffect(this.skillBuild.quality);
			});
			this.DoScale(sequence, this.priceObj, Vector3.one, num);
			this.DoScale(sequence, this.buttonCancel.gameObject, Vector3.one, num);
			this.DoScale(sequence, this.buttonDeal.gameObject, Vector3.one, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutGroup);
			});
		}

		private void DoScale(Sequence seq, GameObject obj, Vector3 target, float duration)
		{
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(obj.transform, target, duration), 22)), delegate
			{
				obj.transform.localScale = Vector3.one;
			});
		}

		[GameTestMethod("事件", "恶魔", "", 410)]
		private static void OpenGameEventDemon()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			GameEventDemonViewModule.OpenData openData = new GameEventDemonViewModule.OpenData();
			openData.seed = random.Next();
			GameApp.View.OpenView(ViewName.GameEventDemonViewModule, openData, 1, null, null);
		}

		public UISpineModelItem spineModelItem;

		public CustomText textLostHp;

		public CustomText textGetSkill;

		public UIGameEventSelectSkillItem skillItem;

		public CustomButton buttonCancel;

		public CustomButton buttonDeal;

		public Image imageLight;

		public GameObject downObj;

		public GameObject textTip;

		public GameObject priceObj;

		public CustomButton buttonLearnedSkills;

		public RectTransform layoutGroup;

		private GameEventSkillBuildData skillBuild;

		private int costMaxHpPercent;

		private long m_costMaxHp;

		private GameEventDemonViewModule.OpenData openData;

		private SequencePool sequencePool = new SequencePool();

		public class OpenData
		{
			public int modelId;

			public int memberId;

			public int seed;

			public SkillBuildSourceType sourceType = SkillBuildSourceType.Demon;
		}
	}
}
