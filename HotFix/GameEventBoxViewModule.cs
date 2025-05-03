using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using Spine;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GameEventBoxViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 937;
		}

		public override void OnCreate(object data)
		{
			this.boxObjInitPos = this.boxObj.transform.position;
			this.spineModelItem.Init();
			this.coinParticles.Add(this.coinEff1);
			this.coinParticles.Add(this.coinEff2);
			this.coinParticles.Add(this.coinEff3);
			this.ResetUI();
			this.skillSelectCtrl.Init();
		}

		public override async void OnOpen(object data)
		{
			if (data != null)
			{
				this.tapToCloseCtrl.OnClose = new Action(this.OnCloseSelf);
				this.buttonOpen.onClick.AddListener(new UnityAction(this.OnOpenBox));
				this.buttonOpen.transform.localScale = Vector3.zero;
				this.randomBoxData = data as GameEventBoxViewModule.RandomBoxData;
				if (this.randomBoxData != null)
				{
					this.tapToCloseCtrl.Show(false);
					await this.spineModelItem.ShowMemberModel(this.randomBoxData.memberId, "Idle", true);
					this.skills = Singleton<GameEventController>.Instance.GetRandomSkillList(this.randomBoxData.sourceType, 3, this.randomBoxData.seed);
					GameTGATools.Ins.AddStageClickTempSkillShow(this.skills);
					this.skillSelectCtrl.Refresh(this.skills, new Action<GameEventSkillBuildData>(this.OnSelectFinish));
					this.PlayShowAni();
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			bool flag = this.isAni;
			this.skillSelectCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.buttonOpen.onClick.RemoveListener(new UnityAction(this.OnOpenBox));
			this.tapToCloseCtrl.OnClose = null;
			this.boxSkillItems.Clear();
			this.coinParticles.Clear();
			this.sequencePool.Clear(false);
			this.ResetUI();
		}

		public override void OnDelete()
		{
			this.skillSelectCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnCloseSelf()
		{
			if (this.isAni)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.GameEventBoxViewModule, null);
		}

		private void OnOpenBox()
		{
			EventMemberController.Instance.DoEventPoint(2);
			this.buttonOpen.gameObject.SetActiveSafe(false);
			this.StartAni();
		}

		private void PlayShowAni()
		{
			Sequence sequence = this.sequencePool.Get();
			Color color = this.textTitle.color;
			color.a = 0f;
			this.textTitle.color = color;
			this.buttonOpen.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOFade(this.textTitle, 1f, 0.5f), 22));
			if (this.spineModelItem.GetAni("Appear") != null)
			{
				this.spineModelItem.PlayAnimation("Appear", false);
				this.spineModelItem.AddAnimation("Idle", true, 0f);
			}
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonOpen.transform, Vector3.one * 1.3f, 0.15f)), ShortcutExtensions.DOScale(this.buttonOpen.transform, Vector3.one, 0.1f));
		}

		private void StartAni()
		{
			this.isAni = true;
			Color color = this.textTitle.color;
			color.a = 0f;
			this.textTitle.color = color;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.boxObj.transform, 90f, 0.2f, false), 11));
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.boxObj.transform, -800f, 0.2f, false), 12), delegate
			{
				GameApp.Sound.PlayClip(685, 1f);
			}));
			Animation ani = this.spineModelItem.GetAni("Open");
			int num = 0;
			if (ani != null)
			{
				num = (int)((ani.Duration - 0.3f) * 1000f);
				if (num < 0)
				{
					num = 0;
				}
				this.spineModelItem.PlayAnimation("Open", false);
				this.spineModelItem.AddAnimation("Open_Idle", true, 0f);
			}
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OpenBoxAniEnd));
			DelayCall.Instance.CallOnce(num, new DelayCall.CallAction(this.OpenBoxAniEnd));
		}

		private void OpenBoxAniEnd()
		{
			this.isGoldAni = true;
			this.openBox.SetActiveSafe(true);
			this.sequencePool.Get();
			this.skillSelectCtrl.gameObject.SetActiveSafe(true);
			this.skillSelectCtrl.PlayAni(this.bornObj.transform.position, new Action(this.EndAni));
		}

		private void EndAni()
		{
			this.isGoldAni = false;
			this.isAni = false;
			this.boxLight.SetActiveSafe(false);
			this.effectStar.SetActiveSafe(false);
		}

		private void OnSelectFinish(GameEventSkillBuildData data)
		{
			GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { data }, true);
			Singleton<GameEventController>.Instance.SelectSkill(data, false);
			EventArgSelectSkillEnd instance = Singleton<EventArgSelectSkillEnd>.Instance;
			instance.SetData(new List<GameEventSkillBuildData> { data });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseBoxUI, instance);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			this.OnCloseSelf();
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData> { data };
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(list, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		public void PlayBGFx()
		{
			for (int i = 0; i < this.coinParticles.Count; i++)
			{
				this.coinParticles[i].gameObject.SetActiveSafe(true);
				this.coinParticles[i].Play();
			}
		}

		public void StopBGFx()
		{
			for (int i = 0; i < this.coinParticles.Count; i++)
			{
				this.coinParticles[i].Stop();
			}
		}

		public void ResetUI()
		{
			for (int i = 0; i < this.coinParticles.Count; i++)
			{
				this.coinParticles[i].gameObject.SetActiveSafe(false);
			}
			this.skillItemObj.SetActiveSafe(false);
			this.openBox.SetActiveSafe(false);
			this.effectStar.SetActiveSafe(true);
			this.skillSelectCtrl.gameObject.SetActiveSafe(false);
			this.buttonOpen.gameObject.SetActiveSafe(true);
			this.boxObj.transform.position = this.boxObjInitPos;
		}

		[GameTestMethod("事件小游戏", "青铜宝箱", "", 401)]
		private static void OpenGameEventBox()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			GameEventBoxViewModule.RandomBoxData randomBoxData = new GameEventBoxViewModule.RandomBoxData();
			randomBoxData.seed = random.Next();
			Singleton<GameEventController>.Instance.SetFixBoxId(1, randomBoxData.seed);
			GameApp.View.OpenView(ViewName.GameEventBoxViewModule, randomBoxData, 1, null, null);
		}

		public CustomButton buttonOpen;

		public GameObject openBox;

		public CustomText textCoin;

		public GameObject skillItemObj;

		public Transform goldTrans;

		public ParticleSystem coinEff1;

		public ParticleSystem coinEff2;

		public ParticleSystem coinEff3;

		public GameObject boxObj;

		public TapToCloseCtrl tapToCloseCtrl;

		public CustomLanguageText textTitle;

		public GameObject effectStar;

		public GameObject boxLight;

		public GameObject bornObj;

		public UISpineModelItem spineModelItem;

		public GameEventBoxSelectSkillCtrl skillSelectCtrl;

		private List<GameEventSkillBuildData> skills = new List<GameEventSkillBuildData>();

		private List<UIGameEventBoxSkillItem> boxSkillItems = new List<UIGameEventBoxSkillItem>();

		private List<ParticleSystem> coinParticles = new List<ParticleSystem>();

		private SequencePool sequencePool = new SequencePool();

		private bool isAni;

		private bool isGoldAni;

		private GameEventBoxViewModule.RandomBoxData randomBoxData;

		private Vector3 boxObjInitPos;

		private const float GoldAni_Duration = 4.5f;

		private const float GoldAni_Scale = 1f;

		public class RandomBoxData
		{
			public int memberId;

			public int seed;

			public SkillBuildSourceType sourceType;
		}
	}
}
