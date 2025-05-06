using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleDungeonLoseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.tapToCloseCtrl.OnClose = new Action(this.OnClickClose);
			this.buttonRole.onClick.AddListener(new UnityAction(this.OnClickRole));
			this.buttonTalent.onClick.AddListener(new UnityAction(this.OnClickTalent));
			this.dungeonDataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
		}

		public override void OnOpen(object data)
		{
			this.PlayOpenAni();
			GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).SetTalentChange(this.textTalent, this.imageTalent);
			Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(this.dungeonDataModule.DungeonResponse.DungeonId);
			if (elementById != null)
			{
				this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.name);
				return;
			}
			this.textName.text = "";
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.tapToCloseCtrl.OnClose = null;
			this.buttonRole.onClick.RemoveListener(new UnityAction(this.OnClickRole));
			this.buttonTalent.onClick.RemoveListener(new UnityAction(this.OnClickTalent));
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

		private void OnClickClose()
		{
			if (this.isLoading)
			{
				return;
			}
			this.isLoading = true;
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					this.isLoading = false;
					GameApp.View.CloseView(ViewName.BattleDungeonLoseViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private void OnClickRole()
		{
			EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
			instance.SetData(DxxTools.UI.GetEquipOpenData());
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
			this.OnClickClose();
		}

		private void OnClickTalent()
		{
			EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
			instance.SetData(DxxTools.UI.GetTalentOpenData());
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
			this.OnClickClose();
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
			float num = 1.1f;
			float num2 = 0.15f;
			float num3 = 0.07f;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, num2 + num3);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one * num, num2)), ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one, num3));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textTip, 1f, num2));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonRole.transform, Vector3.one * num, num2)), ShortcutExtensions.DOScale(this.buttonRole.transform, Vector3.one, num3));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonTalent.transform, Vector3.one * num, num2)), ShortcutExtensions.DOScale(this.buttonTalent.transform, Vector3.one, num3));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.tapToCloseCtrl.gameObject.SetActiveSafe(true);
			});
		}

		[GameTestMethod("副本", "副本失败", "", 402)]
		private static void OpenDungeonFail()
		{
			GameApp.View.OpenView(ViewName.BattleDungeonLoseViewModule, null, 1, null, null);
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

		[SerializeField]
		private CustomText textName;

		private SequencePool sequencePool = new SequencePool();

		private DungeonDataModule dungeonDataModule;

		private bool isLoading;
	}
}
