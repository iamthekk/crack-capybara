using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class ArtifactViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonTagUpgrade.onClick.AddListener(new UnityAction(this.OnClickUpgrade));
			this.buttonTagAdvance.onClick.AddListener(new UnityAction(this.OnClickAdvance));
			RedPointController.Instance.RegRecordChange("Equip.Artifact.UpgradeTag", new Action<RedNodeListenData>(this.OnRedPointUpgrade));
			RedPointController.Instance.RegRecordChange("Equip.Artifact.AdvanceTag", new Action<RedNodeListenData>(this.OnRedPointAdvance));
			this.modelNodeCtrl.Init();
			this.modelNodeCtrl.SetData(new Action(this.OnClickPrevious), new Action(this.OnClickNext), new Action(this.OnRide));
			this.artifactUpgradeNodeCtrl.Init();
			this.artifactAdvanceNodeCtrl.Init();
			this.artifactDataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
		}

		public override void OnOpen(object data)
		{
			this.advanceArtifactIndex = 0;
			this.basicArtifactList = this.artifactDataModule.GetBasicDataList();
			this.advanceArtifactList = this.artifactDataModule.GetAdvanceDataList();
			this.artifactAdvanceNodeCtrl.OnShow();
			this.CheckBasicCurrentStage();
			this.OnClickUpgrade();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.artifactAdvanceNodeCtrl.OnHide();
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonTagUpgrade.onClick.RemoveListener(new UnityAction(this.OnClickUpgrade));
			this.buttonTagAdvance.onClick.RemoveListener(new UnityAction(this.OnClickAdvance));
			this.modelNodeCtrl.DeInit();
			this.artifactUpgradeNodeCtrl.DeInit();
			this.artifactAdvanceNodeCtrl.DeInit();
			RedPointController.Instance.UnRegRecordChange("Equip.Artifact.UpgradeTag", new Action<RedNodeListenData>(this.OnRedPointUpgrade));
			RedPointController.Instance.UnRegRecordChange("Equip.Artifact.AdvanceTag", new Action<RedNodeListenData>(this.OnRedPointAdvance));
			this.basicArtifactList = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIArtifact_SelectAdvance, new HandlerEvent(this.OnEventSelectAdvance));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIArtifact_BasicUpgrade, new HandlerEvent(this.OnEventBasicUpgrade));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIArtifact_AdvanceUpgrade, new HandlerEvent(this.OnEventAdvanceUpgrade));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIArtifact_SelectAdvance, new HandlerEvent(this.OnEventSelectAdvance));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIArtifact_BasicUpgrade, new HandlerEvent(this.OnEventBasicUpgrade));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIArtifact_AdvanceUpgrade, new HandlerEvent(this.OnEventAdvanceUpgrade));
		}

		private void OnEventSelectAdvance(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsInt eventArgsInt = eventArgs as EventArgsInt;
			if (eventArgsInt != null)
			{
				this.advanceArtifactIndex = eventArgsInt.Value;
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
			for (int i = 0; i < this.basicArtifactList.Count; i++)
			{
				if ((long)this.basicArtifactList[i].Stage == (long)((ulong)this.artifactDataModule.ArtifactInfo.Stage))
				{
					this.baseArtifactIndex = i;
					return;
				}
			}
		}

		private void RefreshBasic()
		{
			ArtifactBasicData artifactBasicData = this.basicArtifactList[this.baseArtifactIndex];
			this.modelNodeCtrl.Refresh(artifactBasicData.ArtifactName, artifactBasicData.StageConfig.model, false, 0);
			this.modelNodeCtrl.RefreshStage(artifactBasicData.Stage);
			this.artifactUpgradeNodeCtrl.Refresh();
			this.RefreshButton();
		}

		private void RefreshAdvance()
		{
			ArtifactAdvanceData artifactAdvanceData = this.advanceArtifactList[this.advanceArtifactIndex];
			this.modelNodeCtrl.Refresh(artifactAdvanceData.ArtifactName, artifactAdvanceData.Config.model, false, 0);
			this.modelNodeCtrl.RefreshStar(artifactAdvanceData.Star, artifactAdvanceData.Config.maxStar);
			this.artifactAdvanceNodeCtrl.SetData(artifactAdvanceData);
			this.RefreshButton();
		}

		private void RefreshButton()
		{
			this.modelNodeCtrl.ShowPreviousButton(true);
			this.modelNodeCtrl.ShowNextButton(true);
			if (this.uiTag == ArtifactViewModule.UITag.Basic)
			{
				if (this.baseArtifactIndex == 0)
				{
					this.modelNodeCtrl.ShowPreviousButton(false);
					this.modelNodeCtrl.ShowNextButton(this.basicArtifactList.Count > 1);
					return;
				}
				if (this.baseArtifactIndex == this.basicArtifactList.Count - 1)
				{
					this.modelNodeCtrl.ShowNextButton(false);
					this.modelNodeCtrl.ShowPreviousButton(this.basicArtifactList.Count > 1);
					return;
				}
			}
			else if (this.uiTag == ArtifactViewModule.UITag.Advance)
			{
				if (this.advanceArtifactIndex == 0)
				{
					this.modelNodeCtrl.ShowPreviousButton(false);
					this.modelNodeCtrl.ShowNextButton(this.advanceArtifactList.Count > 1);
					return;
				}
				if (this.advanceArtifactIndex == this.advanceArtifactList.Count - 1)
				{
					this.modelNodeCtrl.ShowNextButton(false);
					this.modelNodeCtrl.ShowPreviousButton(this.advanceArtifactList.Count > 1);
				}
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ArtifactViewModule, null);
		}

		private void OnClickUpgrade()
		{
			this.uiTag = ArtifactViewModule.UITag.Basic;
			this.buttonTagUpgrade.SetSelect(true);
			this.buttonTagAdvance.SetSelect(false);
			this.artifactUpgradeNodeCtrl.SetActive(true);
			this.artifactAdvanceNodeCtrl.SetActive(false);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_title_upgrade");
			this.RefreshBasic();
		}

		private void OnClickAdvance()
		{
			this.uiTag = ArtifactViewModule.UITag.Advance;
			this.buttonTagUpgrade.SetSelect(false);
			this.buttonTagAdvance.SetSelect(true);
			this.artifactUpgradeNodeCtrl.SetActive(false);
			this.artifactAdvanceNodeCtrl.SetActive(true);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_title_advance");
			this.RefreshAdvance();
		}

		private void OnClickPrevious()
		{
			if (this.uiTag == ArtifactViewModule.UITag.Basic && this.baseArtifactIndex - 1 >= 0)
			{
				this.baseArtifactIndex--;
				this.RefreshBasic();
				return;
			}
			if (this.uiTag == ArtifactViewModule.UITag.Advance && this.advanceArtifactIndex - 1 >= 0)
			{
				this.advanceArtifactIndex--;
				this.RefreshAdvance();
			}
		}

		private void OnClickNext()
		{
			if (this.uiTag == ArtifactViewModule.UITag.Basic && this.baseArtifactIndex + 1 < this.basicArtifactList.Count)
			{
				this.baseArtifactIndex++;
				this.RefreshBasic();
				return;
			}
			if (this.uiTag == ArtifactViewModule.UITag.Advance && this.advanceArtifactIndex + 1 < this.advanceArtifactList.Count)
			{
				this.advanceArtifactIndex++;
				this.RefreshAdvance();
			}
		}

		private void OnRide()
		{
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

		public RedNodeOneCtrl redNodeUpgradeTag;

		public RedNodeOneCtrl redNodeAdvanceTag;

		public UIArtifactModelNodeCtrl modelNodeCtrl;

		public UIArtifactUpgradeNodeCtrl artifactUpgradeNodeCtrl;

		public UIArtifactAdvanceNodeCtrl artifactAdvanceNodeCtrl;

		private ArtifactDataModule artifactDataModule;

		private List<ArtifactBasicData> basicArtifactList;

		private List<ArtifactAdvanceData> advanceArtifactList;

		private ArtifactViewModule.UITag uiTag;

		private int baseArtifactIndex;

		private int advanceArtifactIndex;

		public enum UITag
		{
			Basic,
			Advance
		}
	}
}
