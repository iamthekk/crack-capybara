using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class PetRanchPage : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_redNodePetList.Value = 0;
			this.m_redNodeCollection.Value = 0;
			this.m_petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.m_petBoxGroup.Init();
			this.mPetRanchGroup.Init();
			this.m_petShowGroup.Init();
			this.m_btnBack.m_onClick = new Action(this.OnBtnBackClick);
			this.m_btnPetListPage.m_onClick = new Action(this.OnBtnPetListPageClick);
			this.m_btnCollection.m_onClick = new Action(this.OnBtnCollectionClick);
			this.m_btnPetTraining.m_onClick = new Action(this.OnBtnPetTrainingClick);
			this.m_btnPetShow.m_onClick = new Action(this.OnBtnPetShowClick);
			RedPointController.Instance.RegRecordChange("Equip.Pet.List", new Action<RedNodeListenData>(this.OnRedPointPetListChange));
			RedPointController.Instance.RegRecordChange("Equip.Pet.Ranch.Collection", new Action<RedNodeListenData>(this.OnRedPointPetCollectionChange));
			this.m_btnCollection.gameObject.SetActive(false);
			this.m_petShowGroup.OnHide();
		}

		protected override void OnDeInit()
		{
			this.m_petBoxGroup.DeInit();
			this.mPetRanchGroup.DeInit();
			this.m_petShowGroup.DeInit();
			this.m_btnBack.m_onClick = null;
			this.m_btnPetListPage.m_onClick = null;
			this.m_btnCollection.m_onClick = null;
			this.m_btnPetTraining.m_onClick = null;
			this.m_btnPetShow.m_onClick = null;
			RedPointController.Instance.UnRegRecordChange("Equip.Pet.List", new Action<RedNodeListenData>(this.OnRedPointPetListChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Pet.Ranch.Collection", new Action<RedNodeListenData>(this.OnRedPointPetCollectionChange));
		}

		public void OnShow()
		{
			base.SetActive(true);
			this.UpdateBtnView();
			this.m_petBoxGroup.OnShow();
			this.m_petBoxGroup.Refresh(this.m_petDataModule);
			this.mPetRanchGroup.OnShow();
		}

		public void OnHide()
		{
			this.mPetRanchGroup.OnHide();
			this.m_petBoxGroup.OnHide();
			this.m_petShowGroup.OnHide();
			base.SetActive(false);
		}

		public void UpdateBtnView()
		{
			this.m_btnPetListPage.gameObject.SetActive(this.m_petDataModule.m_petDataDict.Count > 0);
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.PetTraining, false);
			this.m_btnPetTraining.gameObject.SetActive(flag && this.m_petDataModule.m_petDataDict.Count > 0);
		}

		private void OnBtnBackClick()
		{
			this.m_petViewModule.OnClosePetRanchPage();
		}

		private void OnBtnPetListPageClick()
		{
			this.m_petViewModule.OpenPetListPage();
		}

		private void OnBtnCollectionClick()
		{
			GameApp.View.OpenView(ViewName.PetCollectionViewModule, null, 1, null, null);
		}

		private void OnBtnPetTrainingClick()
		{
			GameApp.View.OpenView(ViewName.PetTrainingViewModule, null, 1, null, null);
		}

		private void OnBtnPetShowClick()
		{
			this.m_petShowGroup.OnShow();
		}

		private void OnRedPointPetCollectionChange(RedNodeListenData redData)
		{
			if (this.m_redNodeCollection == null)
			{
				return;
			}
			this.m_redNodeCollection.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointPetListChange(RedNodeListenData redData)
		{
			if (this.m_redNodePetList == null)
			{
				return;
			}
			this.m_redNodePetList.gameObject.SetActive(redData.m_count > 0);
		}

		public PetViewModule m_petViewModule;

		public CustomButton m_btnBack;

		public CustomButton m_btnPetListPage;

		public CustomButton m_btnCollection;

		public CustomButton m_btnPetTraining;

		public CustomButton m_btnPetShow;

		public PetShowGroup m_petShowGroup;

		public PetBoxGroup m_petBoxGroup;

		public PetRanchGroup mPetRanchGroup;

		[Header("红点")]
		public RedNodeOneCtrl m_redNodePetList;

		public RedNodeOneCtrl m_redNodeCollection;

		private PetDataModule m_petDataModule;
	}
}
