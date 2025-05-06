using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pet;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class PetTrainingPage : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.lastPetId = -1;
			this.btnShowOriginPassive.m_onClick = new Action(this.OnBtnShowOriginPassiveClick);
			this.btnClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.btnArrowLeft.m_onClick = new Action(this.OnBtnLeftClick);
			this.btnArrowRight.m_onClick = new Action(this.OnBtnRightClick);
			this.btnPetChange.m_onClick = new Action(this.OnBtnPetChangeClick);
			this.btnTrainingProbabilityOpen.m_onClick = new Action(this.OnBtnTrainingProbabilityOpenClick);
			this.btnTraining.m_onClick = new Action(this.OnBtnTrainingClick);
			this.btnNewPassiveConfirm.m_onClick = new Action(this.OnBtnNewPassiveConfirmClick);
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				this.petPassiveItems[i].Init();
				this.petPassiveItems[i].onItemClickedCallback = new Action<PetPassiveLongItem>(this.OnPassiveLockItemClick);
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				this.petPassiveItems[i].DeInit();
			}
			this.btnShowOriginPassive.m_onClick = null;
			this.btnClose.m_onClick = null;
			this.btnArrowLeft.m_onClick = null;
			this.btnArrowRight.m_onClick = null;
			this.btnPetChange.m_onClick = null;
			this.btnTrainingProbabilityOpen.m_onClick = null;
			this.btnTraining.m_onClick = null;
			this.btnNewPassiveConfirm.m_onClick = null;
			this.lastPetId = -1;
		}

		public void Show()
		{
			this.UpdateView();
		}

		public void UpdateView()
		{
			if (this.lastPetId != this.petTrainingViewModule.curPetData.petId)
			{
				this.lockIndexList.Clear();
				this.lockIndexList = this.petTrainingViewModule.curPetData.trainingAttributeLockIds;
				this.isShowOriginPassive = false;
				this.lastPetId = this.petTrainingViewModule.curPetData.petId;
				if (this.petTrainingViewModule.curPetData.HaveNewPassive)
				{
					this.isShowOriginPassive = false;
				}
			}
			if (!this.petTrainingViewModule.curPetData.HaveNewPassive)
			{
				this.isShowOriginPassive = true;
			}
			this.petTrainingProbTable = GameApp.Table.GetManager().GetPet_PetTrainingProbModelInstance().GetElementById(this.petDataModule.PetTrainingLv);
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.petTrainingViewModule.curPetData.petId);
			Quality_petQuality elementById2 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById.quality);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.petTrainingViewModule.curPetData.nameId);
			this.txtName.text = string.Concat(new string[] { "<color=", elementById2.colorNum, ">", infoByID, "</color>" });
			this.UpdateBtnArrow();
			this.UpdateTrainingLevel();
			this.UpdatePassiveItems();
			this.UpdateLockItems();
		}

		private void UpdateTrainingLevel()
		{
			this.txtTrainingLv.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_lv", new object[] { this.petDataModule.PetTrainingLv });
			int exp = this.petTrainingProbTable.exp;
			if (exp == 0)
			{
				this.expSlider.value = 1f;
				this.txtExp.text = "MAX";
				return;
			}
			float num = (float)this.petDataModule.PetTrainingExp / (float)exp;
			num = Mathf.Clamp(num, 0f, 1f);
			this.expSlider.value = num;
			this.txtExp.text = string.Format("{0}/{1}", this.petDataModule.PetTrainingExp, exp);
		}

		private void UpdateLockItems()
		{
			this.UpdatePassiveItems();
			this.UpdateTrainingCost();
		}

		public void UpdateTrainingCost()
		{
			int petTrainingCostId = Singleton<GameConfig>.Instance.PetTrainingCostId;
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)petTrainingCostId));
			long num = (long)Singleton<GameConfig>.Instance.PetTrainingCostValue;
			this.trainingCostItem1.SetData(petTrainingCostId, itemDataCountByid, num);
			if (this.petTrainingViewModule.curPetData.trainingAttributeIdsTemp.Count > 0)
			{
				this.btnNewPassiveConfirm.gameObject.SetActive(true);
				this.textBtnTraining.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_again");
			}
			else
			{
				this.btnNewPassiveConfirm.gameObject.SetActive(false);
				this.textBtnTraining.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training");
			}
			this.textBtnNewPassiveConfirm.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_confirm_passive");
			if (this.lockIndexList.Count <= 0)
			{
				this.trainingCostItem2.gameObject.SetActive(false);
				return;
			}
			this.trainingCostItem2.gameObject.SetActive(true);
			int petTraningLockCostId = Singleton<GameConfig>.Instance.PetTraningLockCostId;
			long itemDataCountByid2 = dataModule.GetItemDataCountByid((ulong)((long)petTraningLockCostId));
			long petLockPassiveCostValue = Singleton<GameConfig>.Instance.GetPetLockPassiveCostValue(this.lockIndexList.Count);
			this.trainingCostItem2.SetData(petTraningLockCostId, itemDataCountByid2, petLockPassiveCostValue);
		}

		private void UpdatePassiveItems()
		{
			this.usefullPassiveCount = 0;
			this.textNew.gameObject.SetActive(!this.isShowOriginPassive);
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				PetPassiveLongItem petPassiveLongItem = this.petPassiveItems[i];
				bool flag = (this.isShowOriginPassive ? this.lockIndexList.Contains(i) : this.petTrainingViewModule.curPetData.trainingAttributeLockIds.Contains(i));
				petPassiveLongItem.SetData(this.petTrainingViewModule.curPetData, i, this.isShowOriginPassive, flag);
				if (petPassiveLongItem.isUnlock && petPassiveLongItem.petEntryId > 0)
				{
					this.usefullPassiveCount++;
				}
			}
			this.btnShowOriginPassive.gameObject.SetActive(this.petTrainingViewModule.curPetData.trainingAttributeIdsTemp.Count > 0);
			this.RefreshTextOrigin();
		}

		private void OnBtnShowOriginPassiveClick()
		{
			this.isShowOriginPassive = !this.isShowOriginPassive;
			this.UpdatePassiveItems();
			this.RefreshTextOrigin();
		}

		private void RefreshTextOrigin()
		{
			this.textBtnShowOriginPassive.text = (this.isShowOriginPassive ? Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_passive_new") : Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_passive_origin"));
		}

		private void OnBtnPetChangeClick()
		{
			this.petTrainingViewModule.petTrainingSelectPage.Show();
		}

		private void OnBtnTrainingProbabilityOpenClick()
		{
			GameApp.View.OpenView(ViewName.PetTrainingProbabilityTipViewModule, null, 1, null, null);
		}

		private void OnBtnTrainingClick()
		{
			PetData petData = this.petTrainingViewModule.curPetData;
			if (petData == null || petData.petRowId <= 0UL)
			{
				return;
			}
			int petTrainingCostId = Singleton<GameConfig>.Instance.PetTrainingCostId;
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)petTrainingCostId));
			long num = (long)Singleton<GameConfig>.Instance.PetTrainingCostValue;
			if (itemDataCountByid < num)
			{
				GameApp.View.ShowItemNotEnoughTip(petTrainingCostId, true);
				return;
			}
			if (this.lockIndexList.Count > 0)
			{
				int petTraningLockCostId = Singleton<GameConfig>.Instance.PetTraningLockCostId;
				long itemDataCountByid2 = dataModule.GetItemDataCountByid((ulong)((long)petTraningLockCostId));
				long petLockPassiveCostValue = Singleton<GameConfig>.Instance.GetPetLockPassiveCostValue(this.lockIndexList.Count);
				if (itemDataCountByid2 < petLockPassiveCostValue)
				{
					GameApp.View.ShowItemNotEnoughTip(petTraningLockCostId, true);
					return;
				}
			}
			int petTrainingLv = this.petDataModule.PetTrainingLv;
			int rareQualityId = GameApp.Table.GetManager().GetPet_PetTrainingProbModelInstance().GetElementById(petTrainingLv)
				.rareQualityId;
			bool flag = false;
			for (int i = 0; i < petData.trainingAttributeIdsTemp.Count; i++)
			{
				int num2 = petData.trainingAttributeIdsTemp[i];
				Pet_PetEntry elementById = GameApp.Table.GetManager().GetPet_PetEntryModelInstance().GetElementById(num2);
				if (elementById != null && elementById.quality >= rareQualityId && !petData.trainingAttributeLockIds.Contains(i))
				{
					flag = true;
					break;
				}
			}
			Action action = delegate
			{
				NetworkUtils.Pet.PetTrainRequest(petData.petRowId, this.lockIndexList, delegate(bool isOk, PetTrainResponse resp)
				{
					if (isOk && this != null && this.isActiveAndEnabled)
					{
						if (this.petTrainingViewModule.curPetData.HaveNewPassive)
						{
							this.isShowOriginPassive = false;
						}
						this.UpdateView();
						for (int j = 0; j < this.petPassiveItems.Count; j++)
						{
							PetPassiveLongItem petPassiveLongItem = this.petPassiveItems[j];
							if (!petPassiveLongItem.isShowOrignal && petPassiveLongItem.isNewPassive)
							{
								petPassiveLongItem.effAnimator.gameObject.SetActive(true);
								petPassiveLongItem.effAnimator.Play("Show");
							}
						}
					}
				});
			};
			if (flag)
			{
				RememberTipCommonViewModule.TryRunRememberTip(new RememberTipCommonViewModule.OpenData
				{
					rememberTipType = RememberTipType.PetRarePassive,
					contentStr = Singleton<LanguageManager>.Instance.GetInfoByID("pet_rare_passive_tip"),
					onConfirmCallback = action
				});
				return;
			}
			action();
		}

		private void OnBtnNewPassiveConfirmClick()
		{
			PetData curPetData = this.petTrainingViewModule.curPetData;
			if (curPetData == null || curPetData.petRowId <= 0UL)
			{
				return;
			}
			NetworkUtils.Pet.PetTrainSureRequest(curPetData.petRowId, delegate(bool isOk, PetTrainSureResponse resp)
			{
				if (isOk)
				{
					this.UpdateView();
				}
			});
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.PetTrainingViewModule, null);
		}

		private void OnBtnLeftClick()
		{
			if (this.petTrainingViewModule.curIndex <= 0)
			{
				return;
			}
			this.petTrainingViewModule.curIndex--;
			this.petTrainingViewModule.UpdateCurIndex(this.petTrainingViewModule.curIndex);
			this.UpdateBtnArrow();
		}

		private void OnBtnRightClick()
		{
			if (this.petTrainingViewModule.curIndex >= this.petTrainingViewModule.petListData.Count - 1)
			{
				return;
			}
			this.petTrainingViewModule.curIndex++;
			this.petTrainingViewModule.UpdateCurIndex(this.petTrainingViewModule.curIndex);
			this.UpdateBtnArrow();
		}

		private void OnPassiveLockItemClick(PetPassiveLongItem item)
		{
			if (item == null)
			{
				return;
			}
			int index = item.index;
			if (!item.isUnlock)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_passive_unlock_tip", new object[] { item.unlockLv }));
				return;
			}
			if (item.isUnlock && item.petEntryId <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_get_new_passive"));
				return;
			}
			if (this.isShowOriginPassive)
			{
				if (!this.lockIndexList.Contains(index) && this.lockIndexList.Count >= this.usefullPassiveCount - 1)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_passive_lock_tip", new object[] { this.usefullPassiveCount - 1 });
					GameApp.View.ShowStringTip(infoByID);
					return;
				}
				if (this.lockIndexList.Contains(index))
				{
					this.lockIndexList.Remove(index);
				}
				else
				{
					this.lockIndexList.Add(index);
				}
				this.UpdateLockItems();
			}
		}

		private void UpdateBtnArrow()
		{
			this.btnArrowLeft.gameObject.SetActive(this.petTrainingViewModule.curIndex > 0);
			this.btnArrowRight.gameObject.SetActive(this.petTrainingViewModule.curIndex < this.petTrainingViewModule.petListData.Count - 1);
		}

		public PetTrainingViewModule petTrainingViewModule;

		public CustomButton btnClose;

		public CustomButton btnArrowLeft;

		public CustomButton btnArrowRight;

		public CustomText txtName;

		public CustomText txtTrainingLv;

		public CustomButton btnPetChange;

		public CustomButton btnTrainingProbabilityOpen;

		public CustomLanguageText textNew;

		public List<PetPassiveLongItem> petPassiveItems = new List<PetPassiveLongItem>();

		public Slider expSlider;

		public CustomText txtExp;

		public CustomButton btnShowOriginPassive;

		public CustomText textBtnShowOriginPassive;

		public CustomButton btnTraining;

		public CustomText textBtnTraining;

		public CustomButton btnNewPassiveConfirm;

		public CustomText textBtnNewPassiveConfirm;

		public CommonCostItem trainingCostItem1;

		public CommonCostItem trainingCostItem2;

		public RedNodeOneCtrl redNodeTraining;

		private PetDataModule petDataModule;

		private Pet_PetTrainingProb petTrainingProbTable;

		private List<int> lockIndexList = new List<int>();

		private int usefullPassiveCount;

		private bool isShowOriginPassive;

		private int lastPetId = -1;
	}
}
