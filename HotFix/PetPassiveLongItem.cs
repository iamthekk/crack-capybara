using System;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetPassiveLongItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnBtnItemClicked);
			this.btnItem.enabled = true;
			this.effAnimator.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(PetData data, int index, bool isShowOrignal, bool isPassiveLock)
		{
			this.index = index;
			this.isShowOrignal = isShowOrignal;
			this.isPassiveItemLock = isPassiveLock;
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			Pet_PetTraining elementById = GameApp.Table.GetManager().GetPet_PetTrainingModelInstance().GetElementById(index + 1);
			this.unlockLv = elementById.level;
			this.isUnlock = dataModule.PetTrainingLv >= this.unlockLv;
			if (this.isUnlock)
			{
				this.petEntryId = data.GetPetEntryId(index, !this.isShowOrignal);
				this.petEntryValue = data.GetPetEntryValue(index, !this.isShowOrignal);
			}
			else
			{
				this.petEntryId = 0;
				this.petEntryValue = 0;
			}
			this.isPassiveCanUse = this.petEntryId > 0;
			this.isNewPassive = this.isUnlock && this.petEntryId > 0 && !isShowOrignal && !data.trainingAttributeLockIds.Contains(index);
			this.RefreshImgActive();
			this.RefreshPassiveDesc();
		}

		private void RefreshPassiveDesc()
		{
			string text = "";
			string text2 = "";
			if (this.isPassiveCanUse)
			{
				try
				{
					Pet_PetEntry elementById = GameApp.Table.GetManager().GetPet_PetEntryModelInstance().GetElementById(this.petEntryId);
					if (elementById != null)
					{
						if (elementById.entryType == 1 || elementById.actionType == 1)
						{
							int num = ((elementById != null) ? elementById.quality : 1);
							string[] passiveTextColor = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(num)
								.passiveTextColor2;
							MergeAttributeData mergeAttribute = elementById.action.GetMergeAttribute();
							string languageID = elementById.languageID;
							string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(languageID);
							FP fp = ((elementById.entryType == 1) ? (mergeAttribute.Value * this.petEntryValue * 0.01f / 10000f) : mergeAttribute.Value);
							if (elementById.entryType == 1)
							{
								string text3 = ((double)fp * 100.0).ToString("F2");
								text3 += (mergeAttribute.Header.Contains("%") ? "%" : "");
								text = infoByID + " +" + text3;
								if (passiveTextColor != null && passiveTextColor.Length != 0)
								{
									text2 = string.Concat(new string[]
									{
										" <color=",
										passiveTextColor[0],
										">(",
										((float)this.petEntryValue * 0.01f).ToString("F2"),
										"%)</color>"
									});
								}
								else
								{
									text2 = " (" + ((float)this.petEntryValue * 0.01f).ToString("F2") + "%)";
								}
							}
							else
							{
								string text4 = Utility.Math.Round((double)fp, 2).ToString() ?? "";
								text4 += (mergeAttribute.Header.Contains("%") ? "%" : "");
								text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageName) ?? "";
								text2 = infoByID + "+" + text4;
							}
						}
						else if (elementById.actionType == 2)
						{
							text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageName);
							text2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageID);
						}
						else if (elementById.actionType == 3)
						{
							float num2 = ((elementById.entryType == 2) ? 1f : ((float)this.petEntryValue * 1f / (float)elementById.attrRange[1]));
							string text5 = Utility.Math.Round((double)((float)int.Parse(elementById.action) * num2), 2).ToString() ?? "";
							text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageName);
							text2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageID) + " +" + text5 + "%";
						}
					}
					goto IL_0357;
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					goto IL_0357;
				}
			}
			if (this.isUnlock && !this.isPassiveCanUse)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_none");
			}
			else
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_passive_unlock_tip", new object[] { this.unlockLv });
			}
			IL_0357:
			this.textDesc1.text = text + " " + text2;
		}

		private void RefreshImgActive()
		{
			this.goTextNone.SetActive(this.isUnlock && this.petEntryId == 0);
			this.goIconLock.SetActive(!this.isUnlock);
			Pet_PetEntry pet_PetEntry = ((this.petEntryId > 0) ? GameApp.Table.GetManager().GetPet_PetEntryModelInstance().GetElementById(this.petEntryId) : null);
			int num = ((pet_PetEntry != null) ? pet_PetEntry.quality : 1);
			this.goIconE.SetActive(this.isUnlock && pet_PetEntry != null && num == 1);
			this.goIconD.SetActive(this.isUnlock && pet_PetEntry != null && num == 2);
			this.goIconC.SetActive(this.isUnlock && pet_PetEntry != null && num == 3);
			this.goIconB.SetActive(this.isUnlock && pet_PetEntry != null && num == 4);
			this.goIconA.SetActive(this.isUnlock && pet_PetEntry != null && num == 5);
			this.goIconS.SetActive(this.isUnlock && pet_PetEntry != null && num == 6);
			this.goIconSS.SetActive(this.isUnlock && pet_PetEntry != null && num == 7);
			this.goIconSSS.SetActive(this.isUnlock && pet_PetEntry != null && num == 8);
			this.textDesc1.color = (this.isUnlock ? this.colorTxtUnlock : this.colorTxtLock);
			this.goImgBg1.SetActive(!this.isUnlock);
			this.goImgBg2.SetActive(this.isUnlock && !this.isPassiveCanUse);
			this.goImgBg3.SetActive(this.isUnlock && this.isPassiveCanUse && !this.isPassiveItemLock);
			this.goImgBg4.SetActive(this.isUnlock && this.isPassiveCanUse && this.isPassiveItemLock);
			this.goLeftLine1.SetActive(!this.isUnlock);
			this.goLeftLine2.SetActive(this.isUnlock && !this.isPassiveCanUse);
			this.goLeftLine3.SetActive(this.isUnlock && this.isPassiveCanUse && !this.isPassiveItemLock);
			this.goLeftLine4.SetActive(this.isUnlock && this.isPassiveCanUse && this.isPassiveItemLock);
			this.goRightLine1.SetActive(false);
			this.goRightLine2.SetActive(false);
			this.goRightLine3.SetActive(this.isUnlock && this.isPassiveCanUse && !this.isPassiveItemLock);
			this.goRightLine4.SetActive(this.isUnlock && this.isPassiveCanUse && this.isPassiveItemLock);
			this.goPassiveLock.SetActive(this.isUnlock && this.isPassiveCanUse && !this.isNewPassive);
			this.goImgPassiveLock.SetActive(this.isPassiveItemLock);
			this.goImgPassiveUnlock.SetActive(!this.isPassiveItemLock);
			this.textNew.gameObject.SetActive(this.isNewPassive);
		}

		private void OnBtnItemClicked()
		{
			Action<PetPassiveLongItem> action = this.onItemClickedCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton btnItem;

		public GameObject goImgBg1;

		public GameObject goImgBg2;

		public GameObject goImgBg3;

		public GameObject goImgBg4;

		public GameObject goLeftLine1;

		public GameObject goLeftLine2;

		public GameObject goLeftLine3;

		public GameObject goLeftLine4;

		public GameObject goRightLine1;

		public GameObject goRightLine2;

		public GameObject goRightLine3;

		public GameObject goRightLine4;

		public GameObject goIconA;

		public GameObject goIconB;

		public GameObject goIconC;

		public GameObject goIconD;

		public GameObject goIconE;

		public GameObject goIconS;

		public GameObject goIconSS;

		public GameObject goIconSSS;

		public GameObject goIconLock;

		public GameObject goTextNone;

		public GameObject goPassiveLock;

		public GameObject goImgPassiveLock;

		public GameObject goImgPassiveUnlock;

		public CustomLanguageText textNew;

		public CustomText textDesc1;

		public Color colorTxtUnlock;

		public Color colorTxtLock;

		public Animator effAnimator;

		public Action<PetPassiveLongItem> onItemClickedCallback;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public bool isPassiveItemLock;

		[NonSerialized]
		public bool isPassiveCanUse;

		[NonSerialized]
		public bool isNewPassive;

		[NonSerialized]
		public bool isUnlock;

		[NonSerialized]
		public int unlockLv;

		[NonSerialized]
		public int petEntryId;

		[NonSerialized]
		public int petEntryValue;

		[NonSerialized]
		public bool isShowOrignal;
	}
}
