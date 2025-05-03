using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Common;
using Proto.Mount;
using UnityEngine.Events;

namespace HotFix
{
	public class MountViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonTagUpgrade.onClick.AddListener(new UnityAction(this.OnClickUpgrade));
			this.buttonTagAdvance.onClick.AddListener(new UnityAction(this.OnClickAdvance));
			RedPointController.Instance.RegRecordChange("Equip.Mount.UpgradeTag", new Action<RedNodeListenData>(this.OnRedPointUpgrade));
			RedPointController.Instance.RegRecordChange("Equip.Mount.AdvanceTag", new Action<RedNodeListenData>(this.OnRedPointAdvance));
			this.mountModelNodeCtrl.Init();
			this.mountModelNodeCtrl.SetData(new Action(this.OnClickPrevious), new Action(this.OnClickNext), new Action(this.OnRide));
			this.mountUpgradeNodeCtrl.Init();
			this.mountAdvanceNodeCtrl.Init();
			this.mountDataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
		}

		public override void OnOpen(object data)
		{
			this.advanceMountIndex = 0;
			this.basicMountList = this.mountDataModule.GetBasicDataList();
			this.advanceMountList = this.mountDataModule.GetAdvanceDataList();
			this.mountModelNodeCtrl.OnShow();
			this.mountAdvanceNodeCtrl.OnShow();
			this.CheckBasicCurrentStage();
			this.OnClickUpgrade();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.mountModelNodeCtrl.OnHide();
			this.mountAdvanceNodeCtrl.OnHide();
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonTagUpgrade.onClick.RemoveListener(new UnityAction(this.OnClickUpgrade));
			this.buttonTagAdvance.onClick.RemoveListener(new UnityAction(this.OnClickAdvance));
			this.mountModelNodeCtrl.DeInit();
			this.mountUpgradeNodeCtrl.DeInit();
			this.mountAdvanceNodeCtrl.DeInit();
			RedPointController.Instance.UnRegRecordChange("Equip.Mount.UpgradeTag", new Action<RedNodeListenData>(this.OnRedPointUpgrade));
			RedPointController.Instance.UnRegRecordChange("Equip.Mount.AdvanceTag", new Action<RedNodeListenData>(this.OnRedPointAdvance));
			this.basicMountList = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIMount_SelectAdvance, new HandlerEvent(this.OnEventSelectAdvance));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIMount_BasicUpgrade, new HandlerEvent(this.OnEventBasicUpgrade));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIMount_AdvanceUpgrade, new HandlerEvent(this.OnEventAdvanceUpgrade));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_SelectAdvance, new HandlerEvent(this.OnEventSelectAdvance));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_BasicUpgrade, new HandlerEvent(this.OnEventBasicUpgrade));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_AdvanceUpgrade, new HandlerEvent(this.OnEventAdvanceUpgrade));
		}

		private void OnEventSelectAdvance(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsInt eventArgsInt = eventArgs as EventArgsInt;
			if (eventArgsInt != null)
			{
				this.advanceMountIndex = eventArgsInt.Value;
				this.RefreshAdvance();
			}
		}

		private void OnEventBasicUpgrade(object sender, int type, BaseEventArgs eventArgs)
		{
			this.CheckBasicCurrentStage();
			this.OnClickUpgrade();
		}

		private void OnEventAdvanceUpgrade(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnClickAdvance();
		}

		private void CheckBasicCurrentStage()
		{
			for (int i = 0; i < this.basicMountList.Count; i++)
			{
				if ((long)this.basicMountList[i].Stage == (long)((ulong)this.mountDataModule.MountInfo.Stage))
				{
					this.baseMountIndex = i;
					return;
				}
			}
		}

		private void RefreshBasic()
		{
			MountBasicData mountBasicData = this.basicMountList[this.baseMountIndex];
			MountInfo mountInfo = this.mountDataModule.MountInfo;
			int num = 0;
			if (mountInfo.ConfigType == 1U && (ulong)mountInfo.ConfigId == (ulong)((long)mountBasicData.ID))
			{
				num = -1;
			}
			bool flag = (long)mountBasicData.Stage <= (long)((ulong)mountInfo.Stage);
			this.mountModelNodeCtrl.Refresh(mountBasicData.MountName, mountBasicData.MemberId, flag, num);
			this.mountModelNodeCtrl.RefreshStage(mountBasicData.Stage);
			this.mountUpgradeNodeCtrl.Refresh();
			this.RefreshButton();
		}

		private void RefreshAdvance()
		{
			MountAdvanceData mountAdvanceData = this.advanceMountList[this.advanceMountIndex];
			MountInfo mountInfo = this.mountDataModule.MountInfo;
			int num = 0;
			if (mountInfo.ConfigType == 2U && (ulong)mountInfo.ConfigId == (ulong)((long)mountAdvanceData.ID))
			{
				num = -1;
			}
			bool isUnlock = mountAdvanceData.IsUnlock;
			this.mountModelNodeCtrl.Refresh(mountAdvanceData.MountName, mountAdvanceData.MemberId, isUnlock, num);
			this.mountModelNodeCtrl.RefreshStar(mountAdvanceData.Star, mountAdvanceData.Config.maxStar);
			this.mountAdvanceNodeCtrl.SetData(mountAdvanceData);
			this.RefreshButton();
		}

		private void RefreshButton()
		{
			this.mountModelNodeCtrl.ShowPreviousButton(true);
			this.mountModelNodeCtrl.ShowNextButton(true);
			if (this.uiTag == MountViewModule.UITag.Basic)
			{
				if (this.baseMountIndex == 0)
				{
					this.mountModelNodeCtrl.ShowPreviousButton(false);
					this.mountModelNodeCtrl.ShowNextButton(this.basicMountList.Count > 1);
					return;
				}
				if (this.baseMountIndex == this.basicMountList.Count - 1)
				{
					this.mountModelNodeCtrl.ShowNextButton(false);
					this.mountModelNodeCtrl.ShowPreviousButton(this.basicMountList.Count > 1);
					return;
				}
			}
			else if (this.uiTag == MountViewModule.UITag.Advance)
			{
				if (this.advanceMountIndex == 0)
				{
					this.mountModelNodeCtrl.ShowPreviousButton(false);
					this.mountModelNodeCtrl.ShowNextButton(this.advanceMountList.Count > 1);
					return;
				}
				if (this.advanceMountIndex == this.advanceMountList.Count - 1)
				{
					this.mountModelNodeCtrl.ShowNextButton(false);
					this.mountModelNodeCtrl.ShowPreviousButton(this.advanceMountList.Count > 1);
				}
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.MountViewModule, null);
		}

		private void OnClickUpgrade()
		{
			this.uiTag = MountViewModule.UITag.Basic;
			this.buttonTagUpgrade.SetSelect(true);
			this.buttonTagAdvance.SetSelect(false);
			this.mountUpgradeNodeCtrl.SetActive(true);
			this.mountAdvanceNodeCtrl.SetActive(false);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_title_upgrade");
			this.RefreshBasic();
		}

		private void OnClickAdvance()
		{
			this.uiTag = MountViewModule.UITag.Advance;
			this.buttonTagUpgrade.SetSelect(false);
			this.buttonTagAdvance.SetSelect(true);
			this.mountUpgradeNodeCtrl.SetActive(false);
			this.mountAdvanceNodeCtrl.SetActive(true);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_title_advance");
			this.RefreshAdvance();
		}

		private void OnClickPrevious()
		{
			if (this.uiTag == MountViewModule.UITag.Basic && this.baseMountIndex - 1 >= 0)
			{
				this.baseMountIndex--;
				this.RefreshBasic();
				return;
			}
			if (this.uiTag == MountViewModule.UITag.Advance && this.advanceMountIndex - 1 >= 0)
			{
				this.advanceMountIndex--;
				this.RefreshAdvance();
			}
		}

		private void OnClickNext()
		{
			if (this.uiTag == MountViewModule.UITag.Basic && this.baseMountIndex + 1 < this.basicMountList.Count)
			{
				this.baseMountIndex++;
				this.RefreshBasic();
				return;
			}
			if (this.uiTag == MountViewModule.UITag.Advance && this.advanceMountIndex + 1 < this.advanceMountList.Count)
			{
				this.advanceMountIndex++;
				this.RefreshAdvance();
			}
		}

		private void OnRide()
		{
			int num = 0;
			int num2 = 0;
			if (this.uiTag == MountViewModule.UITag.Basic)
			{
				num = 1;
				num2 = this.basicMountList[this.baseMountIndex].ID;
			}
			else if (this.uiTag == MountViewModule.UITag.Advance)
			{
				num = 2;
				num2 = this.advanceMountList[this.advanceMountIndex].ID;
			}
			if (num <= 0 || num2 <= 0)
			{
				return;
			}
			int num3 = 0;
			if ((long)num == (long)((ulong)this.mountDataModule.MountInfo.ConfigType) && (long)num2 == (long)((ulong)this.mountDataModule.MountInfo.ConfigId))
			{
				num3 = -1;
			}
			NetworkUtils.Mount.DoMountDressRequest(num, num2, num3, delegate(bool result, MountDressResponse response)
			{
				if (this.uiTag == MountViewModule.UITag.Basic)
				{
					this.RefreshBasic();
					return;
				}
				if (this.uiTag == MountViewModule.UITag.Advance)
				{
					this.RefreshAdvance();
				}
			});
		}

		private void OnRedPointUpgrade(RedNodeListenData redData)
		{
			if (this.redNodeUpgradeTag == null)
			{
				return;
			}
			this.redNodeUpgradeTag.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointAdvance(RedNodeListenData redData)
		{
			if (this.redNodeAdvanceTag == null)
			{
				return;
			}
			this.redNodeAdvanceTag.gameObject.SetActive(redData.m_count > 0);
		}

		public CustomText textTitle;

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomChooseButton buttonTagUpgrade;

		public CustomChooseButton buttonTagAdvance;

		public UIMountModelNodeCtrl mountModelNodeCtrl;

		public RedNodeOneCtrl redNodeUpgradeTag;

		public RedNodeOneCtrl redNodeAdvanceTag;

		public UIMountUpgradeNodeCtrl mountUpgradeNodeCtrl;

		public UIMountAdvanceNodeCtrl mountAdvanceNodeCtrl;

		private MountDataModule mountDataModule;

		private List<MountBasicData> basicMountList;

		private List<MountAdvanceData> advanceMountList;

		private MountViewModule.UITag uiTag;

		private int baseMountIndex;

		private int advanceMountIndex;

		public enum UITag
		{
			Basic,
			Advance
		}
	}
}
