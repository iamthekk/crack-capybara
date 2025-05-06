using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pet;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetInfoNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_redNodeActive.Value = 0;
			this.m_redNodeLevelUp.Value = 0;
			this.m_btnActive.m_onClick = new Action(this.OnBtnActiveClick);
			this.m_btnLevelUp.m_onClick = new Action(this.OnBtnLevelUpClick);
			this.m_btnReset.m_onClick = new Action(this.OnBtnResetClick);
			this.m_btnTraining.m_onClick = new Action(this.OnBtnTrainingClick);
			this.m_btnReset.gameObject.SetActive(false);
			this.m_btnActive.gameObject.SetActive(false);
			this.m_btnLevelUp.gameObject.SetActive(false);
			this.m_petSkillItem.Init();
			this.m_petSkillItem.onItemClickCallback = new Action<PetSkillItem>(this.OnSkillItemClick);
			this.m_petLevelProgressItem.Init();
			this.m_petPassiveNodeItem.Init();
			this.petQuickLevelUpNode.onLevelUpOne = new Action(this.OnLevelUpOne);
			this.petQuickLevelUpNode.onLevelUpMore = new Action(this.OnLevelUpMore);
			this.petQuickLevelUpNode.onClose = delegate
			{
				this.RefreshButtons();
			};
			this.petQuickLevelUpNode.Hide();
		}

		protected override void OnDeInit()
		{
			this.m_btnActive.m_onClick = null;
			this.m_btnLevelUp.m_onClick = null;
			this.m_btnReset.m_onClick = null;
			this.m_btnTraining.m_onClick = null;
			this.m_petSkillItem.DeInit();
			this.m_petLevelProgressItem.DeInit();
			this.m_petPassiveNodeItem.DeInit();
			this.memberAttributeData = null;
		}

		public void SetData(PetData petData)
		{
			this.data = petData;
			if (this.data == null)
			{
				return;
			}
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petData.petId);
			this.ShowPlayerModel(elementById.memberId);
			this.RefreshButtons();
			this.RefreshAttributes();
			this.RefreshPetQuickLevelUp();
			this.RefreshSkillInfo();
			this.m_petLevelProgressItem.SetData(petData.petId, petData.level);
			this.m_petPassiveNodeItem.SetData(petData.trainingAttributeIds, petData.trainingAttributeValues);
		}

		private void RefreshSkillInfo()
		{
			this.m_petSkillItem.SetData(this.data.battleSkill, this.data.quality);
		}

		private void RefreshPetQuickLevelUp()
		{
			if (this.petQuickLevelUpNode.gameObject.activeSelf)
			{
				this.petQuickLevelUpNode.ReCalcLevelUpInfo(this.data);
				if (this.petQuickLevelUpNode.toLevel - this.data.level > 1)
				{
					this.petQuickLevelUpNode.Show(this.data);
					this.m_btnLevelUp.gameObject.SetActive(false);
					return;
				}
				this.petQuickLevelUpNode.Hide();
				this.RefreshButtons();
			}
		}

		private void RefreshButtons()
		{
			this.m_btnReset.gameObject.SetActive(this.data.level > 1);
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.data.petId);
			EPetItemType petItemType = this.data.PetItemType;
			if (this.data.PetItemType == EPetItemType.Pet)
			{
				bool flag = elementById.IsFullMaxLevel(this.data.level);
				this.m_txtMaxLevel.gameObject.SetActive(flag);
				this.m_btnLevelUp.gameObject.SetActive(!flag);
				if (!flag)
				{
					List<ItemData> levelUpCosts = elementById.GetLevelUpCosts(this.data.level);
					ItemData itemData = ((levelUpCosts.Count > 0) ? levelUpCosts[0] : null);
					ItemData itemData2 = ((levelUpCosts.Count > 1) ? levelUpCosts[1] : null);
					long num = ((itemData != null) ? this.m_propDataModule.GetItemDataCountByid((ulong)((long)itemData.ID)) : 0L);
					long num2 = ((itemData != null) ? itemData.TotalCount : 0L);
					long num3 = ((itemData2 != null) ? this.m_propDataModule.GetItemDataCountByid((ulong)((long)itemData2.ID)) : 0L);
					long num4 = ((itemData2 != null) ? itemData2.TotalCount : 0L);
					if (itemData != null)
					{
						this.m_lvUpCostItem1.gameObject.SetActive(true);
						this.m_lvUpCostItem1.SetData(itemData, num, num2);
					}
					else
					{
						this.m_lvUpCostItem1.gameObject.SetActive(false);
					}
					if (itemData2 != null)
					{
						this.m_lvUpCostItem2.gameObject.SetActive(true);
						this.m_lvUpCostItem2.SetData(itemData2, num3, num4);
					}
					else
					{
						this.m_lvUpCostItem2.gameObject.SetActive(false);
					}
					Pet_petLevel elementById2 = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(elementById.GetPetLevelId(this.data.level));
					int talentStage = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage;
					if (elementById2 == null || talentStage < elementById2.talentNeed)
					{
						this.m_redNodeLevelUp.gameObject.SetActive(false);
						this.m_btnLevelUp.GetComponent<UIGrays>().SetUIGray();
						return;
					}
					if (num >= num2 && num3 >= num4)
					{
						this.m_redNodeLevelUp.gameObject.SetActive(this.data.ConditionInDeployOk());
						this.m_btnLevelUp.GetComponent<UIGrays>().Recovery();
						return;
					}
					this.m_redNodeLevelUp.gameObject.SetActive(false);
					this.m_btnLevelUp.GetComponent<UIGrays>().SetUIGray();
					return;
				}
			}
			else
			{
				this.m_txtMaxLevel.gameObject.SetActive(false);
				this.m_btnLevelUp.gameObject.SetActive(false);
			}
		}

		private void RefreshAttributes()
		{
			GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.data.petId);
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(this.data.quality);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.data.nameId);
			this.m_txtName.text = string.Concat(new string[] { "<color=", elementById.colorNumDark, ">", infoByID, "</color>" });
			Atlas_atlas elementById2 = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(elementById.atlasId);
			string text = ((elementById2 != null) ? elementById2.path : "");
			this.m_imgQualityBg.SetImage(text, elementById.typeTxtBg);
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.m_txtQuality.text = string.Concat(new string[] { "<color=", elementById.colorNum, ">", infoByID2, "</color>" });
			this.memberAttributeData = this.data.GetMemberAttributeData();
			this.m_txtLevelValue.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { this.data.level });
			this.m_txtHpValue.text = DxxTools.FormatNumber(this.memberAttributeData.GetHpMax4UI()) ?? "";
			this.m_txtAtkValue.text = DxxTools.FormatNumber(this.memberAttributeData.GetAttack4UI()) ?? "";
			this.m_txtDefValue.text = DxxTools.FormatNumber(this.memberAttributeData.GetDefence4UI()) ?? "";
		}

		private async void ShowPlayerModel(int memberId)
		{
			if (!this.cacheMemberId.Equals(memberId))
			{
				this.cacheMemberId = memberId;
				ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
				this.uiSpineModelItem.SetScale(elementById.uiScale);
				await this.uiSpineModelItem.ShowModel(memberId, 0, "Idle", true);
			}
		}

		private void ShowAttributeToast(int oldLevel, Dictionary<string, long> oldAttrDict, PetData pet)
		{
			int level = this.data.level;
			Dictionary<string, long> dictionary = new Dictionary<string, long>();
			dictionary.Add("Attack", this.memberAttributeData.GetAttack4UI());
			dictionary.Add("Defence", this.memberAttributeData.GetDefence4UI());
			dictionary.Add("HPMax", this.memberAttributeData.GetHpMax4UI());
			if (oldLevel != level)
			{
				this.m_animLevel.Play("Show");
			}
			EventArgsAddAttributeTipNode eventArgsAddAttributeTipNode = new EventArgsAddAttributeTipNode();
			foreach (KeyValuePair<string, long> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				long num = oldAttrDict[key];
				long num2 = keyValuePair.Value - num;
				if (num2 > 0L)
				{
					eventArgsAddAttributeTipNode.AddData(key, num2);
					if (key == "HPMax")
					{
						this.m_animHp.Play("Show");
					}
					else if (key == "Attack")
					{
						this.m_animAttack.Play("Show");
					}
					else if (key == "Defence")
					{
						this.m_animDefense.Play("Show");
					}
				}
			}
			if (eventArgsAddAttributeTipNode.data.Count > 0)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddAttributeTipNode, eventArgsAddAttributeTipNode);
			}
		}

		private void OnBtnTrainingClick()
		{
			if (this.data == null)
			{
				return;
			}
			PetTrainingViewModule.OpenData openData = new PetTrainingViewModule.OpenData();
			openData.petId = this.data.petId;
			GameApp.View.OpenView(ViewName.PetTrainingViewModule, openData, 1, null, null);
		}

		private void OnBtnActiveClick()
		{
			NetworkUtils.Pet.PetComposeRequest(this.data.fragmentRowId, delegate(bool isOk, PetComposeResponse res)
			{
			});
		}

		private void OnBtnStarUpgradeClick()
		{
		}

		private void OnBtnLevelUpClick()
		{
			if (this.data == null)
			{
				return;
			}
			if (this.data.PetItemType == EPetItemType.Fragment)
			{
				return;
			}
			this.petQuickLevelUpNode.ReCalcLevelUpInfo(this.data);
			if (this.petQuickLevelUpNode.toLevel - this.data.level > 1)
			{
				this.petQuickLevelUpNode.Show(this.data);
				this.m_btnLevelUp.gameObject.SetActive(false);
				return;
			}
			this.OnLevelUpImpl(false);
		}

		private void OnLevelUpOne()
		{
			this.OnLevelUpImpl(false);
		}

		private void OnLevelUpMore()
		{
			this.OnLevelUpImpl(true);
		}

		private void OnLevelUpImpl(bool isQuickLevelUp)
		{
			if (!isQuickLevelUp)
			{
				Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.data.petId);
				if (elementById.IsFullMaxLevel(this.data.level))
				{
					EventArgsString instance = Singleton<EventArgsString>.Instance;
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("tip_pet_level_max");
					instance.SetData(infoByID);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
					return;
				}
				Pet_petLevel elementById2 = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(elementById.GetPetLevelId(this.data.level));
				int talentStage = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage;
				if (elementById2 != null && talentStage < elementById2.talentNeed)
				{
					EventArgsString instance2 = Singleton<EventArgsString>.Instance;
					TalentNew_talentEvolution elementById3 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(elementById2.talentNeed);
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.stepLanguageId);
					string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("equip_evolution_talent_limit", new object[] { infoByID2 });
					instance2.SetData(infoByID3);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance2);
					return;
				}
				List<ItemData> levelUpCosts = elementById.GetLevelUpCosts(this.data.level);
				for (int i = 0; i < levelUpCosts.Count; i++)
				{
					ItemData itemData = levelUpCosts[i];
					long itemDataCountByid = this.m_propDataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					long totalCount = itemData.TotalCount;
					if (itemDataCountByid < totalCount)
					{
						GameApp.View.ShowItemNotEnoughTip(itemData.ID, true);
						return;
					}
				}
			}
			int oldLevel = this.data.level;
			Dictionary<string, long> oldAttrDict = new Dictionary<string, long>();
			oldAttrDict.Add("Attack", this.memberAttributeData.GetAttack4UI());
			oldAttrDict.Add("Defence", this.memberAttributeData.GetDefence4UI());
			oldAttrDict.Add("HPMax", this.memberAttributeData.GetHpMax4UI());
			NetworkUtils.Pet.PetLevelUpRequest(this.data.petRowId, isQuickLevelUp, delegate(bool isOk, PetStrengthResponse res)
			{
				if (!isOk)
				{
					return;
				}
				GameApp.Sound.PlayClip(602, 1f);
				this.ShowAttributeToast(oldLevel, oldAttrDict, this.data);
			});
		}

		private void OnBtnResetClick()
		{
			if (this.data == null || this.data.level <= 1)
			{
				return;
			}
			NetworkUtils.Pet.PetResetRequest(this.data.petRowId, delegate(bool isOk, PetResetResponse res)
			{
			});
		}

		private void OnSkillItemClick(PetSkillItem petSkillItem)
		{
			if (petSkillItem == null || petSkillItem.data == null)
			{
				return;
			}
			PetSkillEffectTipViewModule.OpenData openData = new PetSkillEffectTipViewModule.OpenData();
			openData.petId = this.data.petId;
			openData.battleSkill = petSkillItem.data;
			openData.petLevel = this.data.level;
			GameApp.View.OpenView(ViewName.PetSkillEffectTipViewModule, openData, 2, null, null);
		}

		public PetQuickLevelUpNode petQuickLevelUpNode;

		[Header("宠物模型")]
		public UISpineModelItem uiSpineModelItem;

		[Header("名称战力星级")]
		public CustomText m_txtName;

		public PetStarNode mPetStarNode;

		public CustomText m_txtQuality;

		public CustomImage m_imgQualityBg;

		[Header("属性")]
		public CustomText m_txtLevelValue;

		public CustomText m_txtHpValue;

		public CustomText m_txtAtkValue;

		public CustomText m_txtDefValue;

		public Animator m_animLevel;

		public Animator m_animHp;

		public Animator m_animAttack;

		public Animator m_animDefense;

		public PetSkillItem m_petSkillItem;

		public PetLevelProgressItem m_petLevelProgressItem;

		public PetPassiveNodeItem m_petPassiveNodeItem;

		[Header("功能按钮")]
		public CustomButton m_btnReset;

		public CustomButton m_btnActive;

		public CustomButton m_btnLevelUp;

		public CustomButton m_btnTraining;

		public CustomLanguageText m_txtMaxLevel;

		public RedNodeOneCtrl m_redNodeActive;

		public RedNodeOneCtrl m_redNodeLevelUp;

		public CommonCostItem m_lvUpCostItem1;

		public CommonCostItem m_lvUpCostItem2;

		public PetData data;

		private PropDataModule m_propDataModule;

		private int cacheMemberId;

		private MemberAttributeData memberAttributeData = new MemberAttributeData();
	}
}
