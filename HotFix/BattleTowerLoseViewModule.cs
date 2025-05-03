using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleTowerLoseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.tapToCloseCtrl.OnClose = new Action(this.OnBtnCloseHandler);
			this.buttonRole.onClick.AddListener(new UnityAction(this.OnBtnRole));
			this.buttonTalent.onClick.AddListener(new UnityAction(this.OnBtnTalent));
		}

		public override void OnOpen(object data)
		{
			GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).SetTalentChange(this.textTalent, this.imageTalent);
			this.PlayOpenAni();
			GameApp.Sound.PlayClip(89, 1f);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.tapToCloseCtrl.OnClose = null;
			this.buttonRole.onClick.RemoveListener(new UnityAction(this.OnBtnRole));
			this.buttonTalent.onClick.RemoveListener(new UnityAction(this.OnBtnTalent));
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

		private void OnBtnCloseHandler()
		{
			if (this.isLoadAni)
			{
				return;
			}
			this.isLoadAni = true;
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					this.isLoadAni = false;
					GameApp.View.CloseView(ViewName.BattleTowerLoseViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private void OnBtnRole()
		{
			EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
			instance.SetData(DxxTools.UI.GetEquipOpenData());
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
			this.OnBtnCloseHandler();
		}

		private void OnBtnTalent()
		{
			EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
			instance.SetData(DxxTools.UI.GetTalentOpenData());
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
			this.OnBtnCloseHandler();
		}

		private void PlayOpenAni()
		{
			this.tapToCloseCtrl.gameObject.SetActiveSafe(false);
			this.flagObj.transform.localScale = Vector3.zero;
			Color color = this.textTip.color;
			color.a = 0f;
			this.textTip.color = color;
			this.buttonRole.transform.localScale = Vector3.zero;
			this.buttonTalent.transform.localScale = Vector3.zero;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one, this.duration2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textTip, 1f, this.duration1));
			TweenSettingsExtensions.Append(sequence, this.PlayerButtonScale(this.buttonRole.transform));
			TweenSettingsExtensions.Join(sequence, this.PlayerButtonScale(this.buttonTalent.transform));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.tapToCloseCtrl.gameObject.SetActiveSafe(true);
			});
		}

		private Sequence PlayerButtonScale(Transform btnTrans)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(btnTrans, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(btnTrans, Vector3.one, this.duration2));
			return sequence;
		}

		[GameTestMethod("爬塔", "爬塔失败", "", 402)]
		private static void OpenTowerFail()
		{
			GameApp.View.OpenView(ViewName.BattleTowerLoseViewModule, null, 1, null, null);
		}

		[SerializeField]
		private TapToCloseCtrl tapToCloseCtrl;

		[SerializeField]
		private CustomButton buttonRole;

		[SerializeField]
		private CustomButton buttonTalent;

		[SerializeField]
		private CustomText textTalent;

		[SerializeField]
		private CustomImage imageTalent;

		[SerializeField]
		private GameObject flagObj;

		[SerializeField]
		private CustomLanguageText textTip;

		private SequencePool sequencePool = new SequencePool();

		private bool isLoadAni;

		private float scale = 1.1f;

		private float duration1 = 0.15f;

		private float duration2 = 0.05f;
	}
}
