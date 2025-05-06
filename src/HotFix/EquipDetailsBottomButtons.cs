using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Equip;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class EquipDetailsBottomButtons : CustomBehaviour
	{
		protected override void OnInit()
		{
			CommonCostItem component = this.m_equipCostItem.GetComponent<CommonCostItem>();
			if (component != null)
			{
				component.Init();
				this.m_commonCostItemList.Add(component);
			}
			this.m_btnPutUp.onClick.AddListener(new UnityAction(this.OnBtnPutUpClick));
			this.m_btnPutDown.onClick.AddListener(new UnityAction(this.OnBtnPutDownClick));
			this.m_btnLevelUp.onClick.AddListener(new UnityAction(this.OnBtnLevelUpClick));
			this.m_btnQuickLevelUp.onClick.AddListener(new UnityAction(this.OnBtnQuickLevelUpClick));
			this.m_btnEvolution.onClick.AddListener(new UnityAction(this.OnBtnEvolutionClick));
			if (this.m_btnRefining != null)
			{
				this.m_btnRefining.onClick.AddListener(new UnityAction(this.OnBtnRefiningClick));
			}
		}

		protected override void OnDeInit()
		{
			this.m_btnPutUp.onClick.RemoveListener(new UnityAction(this.OnBtnPutUpClick));
			this.m_btnPutDown.onClick.RemoveListener(new UnityAction(this.OnBtnPutDownClick));
			this.m_btnLevelUp.onClick.RemoveListener(new UnityAction(this.OnBtnLevelUpClick));
			this.m_btnQuickLevelUp.onClick.RemoveListener(new UnityAction(this.OnBtnQuickLevelUpClick));
			this.m_btnEvolution.onClick.RemoveListener(new UnityAction(this.OnBtnEvolutionClick));
			if (this.m_btnRefining != null)
			{
				this.m_btnRefining.onClick.RemoveListener(new UnityAction(this.OnBtnRefiningClick));
			}
		}

		public void SetData(EquipDetailsViewModule param)
		{
			this.equipDetailsViewModule = param;
		}

		public void RefreshButtons()
		{
			bool flag = this.equipDetailsViewModule.m_equipDataModule.IsPutOn(this.equipDetailsViewModule.m_openData.m_equipData.rowID);
			this.m_btnPutUp.gameObject.SetActive(!flag);
			this.m_btnPutDown.gameObject.SetActive(flag);
			this.m_equipCostObj.gameObject.SetActiveSafe(true);
			bool flag2 = this.equipDetailsViewModule.m_openData.m_equipData.IsCanEvolution();
			bool flag3 = this.equipDetailsViewModule.m_equipDataModule.IsCanLevelUp(this.equipDetailsViewModule.m_openData.m_equipData);
			if (flag2)
			{
				int id = (int)this.equipDetailsViewModule.m_openData.m_equipData.id;
				int evolution = this.equipDetailsViewModule.m_openData.m_equipData.evolution;
				int evolutionId = this.equipDetailsViewModule.m_openData.m_equipData.GetEvolutionId();
				Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionId);
				bool flag4 = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage >= elementById.talentLimit;
				List<ItemData> evolutionCosts = this.equipDetailsViewModule.m_equipDataModule.GetEvolutionCosts(id, evolution);
				bool flag5 = this.OnRefreshCost(evolutionCosts);
				this.m_btnEvolution.gameObject.SetActive(true);
				this.m_btnLevelUp.gameObject.SetActive(false);
				this.m_btnQuickLevelUp.gameObject.SetActive(false);
				if (!flag5 || !flag4)
				{
					this.m_btnEvolution.GetComponent<UIGrays>().SetUIGray();
					return;
				}
				this.m_btnEvolution.GetComponent<UIGrays>().Recovery();
				return;
			}
			else
			{
				if (!flag3)
				{
					this.m_btnEvolution.gameObject.SetActive(false);
					this.m_btnLevelUp.gameObject.SetActive(false);
					this.m_btnQuickLevelUp.gameObject.SetActive(false);
					this.m_equipCostObj.gameObject.SetActiveSafe(false);
					return;
				}
				List<ItemData> levelUpCosts = this.equipDetailsViewModule.m_equipDataModule.GetLevelUpCosts((int)this.equipDetailsViewModule.m_openData.m_equipData.id, (int)this.equipDetailsViewModule.m_openData.m_equipData.level);
				bool flag6 = this.OnRefreshCost(levelUpCosts);
				this.m_btnEvolution.gameObject.SetActive(false);
				this.m_btnLevelUp.gameObject.SetActive(true);
				this.m_btnQuickLevelUp.gameObject.SetActive(true);
				if (!flag6)
				{
					this.m_btnLevelUp.GetComponent<UIGrays>().SetUIGray();
					this.m_btnQuickLevelUp.GetComponent<UIGrays>().SetUIGray();
					return;
				}
				this.m_btnLevelUp.GetComponent<UIGrays>().Recovery();
				this.m_btnQuickLevelUp.GetComponent<UIGrays>().Recovery();
				return;
			}
		}

		private bool OnRefreshCost(List<ItemData> itemDataList)
		{
			if (this.m_commonCostItemList.Count < itemDataList.Count)
			{
				int num = itemDataList.Count - this.m_commonCostItemList.Count;
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_equipCostItem);
					gameObject.SetParentNormal(this.m_equipCostObj, false);
					CommonCostItem component = gameObject.GetComponent<CommonCostItem>();
					component.gameObject.SetActiveSafe(true);
					component.Init();
					this.m_commonCostItemList.Add(component);
				}
			}
			bool flag = true;
			int num2 = 0;
			for (int j = 0; j < itemDataList.Count; j++)
			{
				num2 = 0;
				CommonCostItem commonCostItem = this.m_commonCostItemList[j];
				if (commonCostItem != null)
				{
					commonCostItem.gameObject.SetActiveSafe(true);
					long itemDataCountByid = this.equipDetailsViewModule.m_propDataModule.GetItemDataCountByid((ulong)((long)itemDataList[j].ID));
					long totalCount = itemDataList[j].TotalCount;
					commonCostItem.SetData(itemDataList[j], itemDataCountByid, totalCount);
					if (itemDataCountByid < totalCount)
					{
						flag = false;
					}
				}
			}
			if (num2 < this.m_commonCostItemList.Count - 1)
			{
				for (int k = num2; k < this.m_commonCostItemList.Count; k++)
				{
					this.m_commonCostItemList[k].gameObject.SetActiveSafe(false);
				}
			}
			return flag;
		}

		private void OnBtnPutUpClick()
		{
			NetworkUtils.Equip.DoEquipDress(this.equipDetailsViewModule.m_equipDataModule.ReplaceEquipDressRowIds(this.equipDetailsViewModule.m_equipDataModule.m_equipDressRowIds, this.equipDetailsViewModule.m_openData.m_equipData.equipType, this.equipDetailsViewModule.m_openData.m_equipData.rowID, 0UL), delegate(bool isOk, EquipDressResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.equipDetailsViewModule.OnBtnCloseClick();
			});
		}

		private void OnBtnPutDownClick()
		{
			NetworkUtils.Equip.DoEquipDress(this.equipDetailsViewModule.m_equipDataModule.ReplaceEquipDressRowIds(this.equipDetailsViewModule.m_equipDataModule.m_equipDressRowIds, this.equipDetailsViewModule.m_openData.m_equipData.equipType, 0UL, this.equipDetailsViewModule.m_openData.m_equipData.rowID), delegate(bool isOk, EquipDressResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.equipDetailsViewModule.OnBtnCloseClick();
			});
		}

		private void OnBtnLevelUpClick()
		{
			this.OnLevelUpImpl(1U);
		}

		private void OnBtnQuickLevelUpClick()
		{
			this.OnLevelUpImpl(0U);
		}

		private void OnLevelUpImpl(uint levelUpCount)
		{
			if (this.equipDetailsViewModule.m_openData.m_equipData.IsFullLevel())
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("tip_equip_level_max"));
				return;
			}
			if (!this.equipDetailsViewModule.m_equipDataModule.IsCanLevelUp(this.equipDetailsViewModule.m_openData.m_equipData))
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("tip_equip_compose"));
				return;
			}
			int num;
			if (!this.equipDetailsViewModule.m_equipDataModule.IsHaveLevelUpCost(this.equipDetailsViewModule.m_openData.m_equipData, out num))
			{
				GameApp.View.ShowItemNotEnoughTip(num, true);
				return;
			}
			ulong equipRowId = this.equipDetailsViewModule.m_openData.m_equipData.rowID;
			uint id = this.equipDetailsViewModule.m_openData.m_equipData.id;
			EquipData equipDataByRowId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetEquipDataByRowId(this.equipDetailsViewModule.m_openData.m_equipData.rowID);
			List<MergeAttributeData> mergeAttributeData = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipDataByRowId.id)
				.GetMergeAttributeData((int)equipDataByRowId.level, equipDataByRowId.evolution);
			MemberAttributeData oldMemberAttributeData = new MemberAttributeData();
			oldMemberAttributeData.MergeAttributes(mergeAttributeData, false);
			NetworkUtils.Equip.DoEquipUpgradeRequest(equipRowId, levelUpCount, delegate(bool isOk, EquipUpgradeResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.ShowAttributeToast(oldMemberAttributeData, equipRowId);
				GameApp.Sound.PlayClip(602, 1f);
				this.equipDetailsViewModule.levelAnimator.gameObject.SetActive(true);
				this.equipDetailsViewModule.levelAnimator.Play("Show");
				if (this.equipDetailsViewModule.m_textBasicAttr1.gameObject.activeSelf)
				{
					this.equipDetailsViewModule.baseAttribute1Animator.gameObject.SetActive(true);
					this.equipDetailsViewModule.baseAttribute1Animator.Play("Show");
				}
				if (this.equipDetailsViewModule.m_textBasicAttr2.gameObject.activeSelf)
				{
					this.equipDetailsViewModule.baseAttribute2Animator.gameObject.SetActive(true);
					this.equipDetailsViewModule.baseAttribute2Animator.Play("Show");
				}
				this.equipDetailsViewModule.m_equipItem.rectTransform.localScale = Vector3.one;
				Sequence sequence = new SequencePool().Get();
				Vector3 vector = Vector3.one * 1.1f;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.equipDetailsViewModule.m_equipItem.transform, vector, 0.15f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.equipDetailsViewModule.m_equipItem.transform, Vector3.one, 0.15f));
			});
		}

		private void OnBtnEvolutionClick()
		{
			if (this.equipDetailsViewModule.m_openData.m_equipData.IsCanEvolution())
			{
				int id = (int)this.equipDetailsViewModule.m_openData.m_equipData.id;
				int evolution = this.equipDetailsViewModule.m_openData.m_equipData.evolution;
				int evolutionId = this.equipDetailsViewModule.m_openData.m_equipData.GetEvolutionId();
				Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionId);
				if (GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage < elementById.talentLimit)
				{
					int talentLimit = elementById.talentLimit;
					TalentNew_talentEvolution elementById2 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(talentLimit);
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.stepLanguageId);
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("equip_evolution_talent_limit", new object[] { infoByID }));
					return;
				}
				ItemData itemData = this.equipDetailsViewModule.m_equipDataModule.GetEvolutionCosts(id, evolution)[0];
				long itemDataCountByid = this.equipDetailsViewModule.m_propDataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
				long totalCount = itemData.TotalCount;
				if (itemDataCountByid < totalCount)
				{
					GameApp.View.ShowItemNotEnoughTip(itemData.ID, true);
					return;
				}
			}
			ulong equipRowId = this.equipDetailsViewModule.m_openData.m_equipData.rowID;
			EquipData equipDataByRowId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetEquipDataByRowId(this.equipDetailsViewModule.m_openData.m_equipData.rowID);
			List<MergeAttributeData> mergeAttributeData = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipDataByRowId.id)
				.GetMergeAttributeData((int)equipDataByRowId.level, equipDataByRowId.evolution);
			MemberAttributeData oldMemberAttributeData = new MemberAttributeData();
			oldMemberAttributeData.MergeAttributes(mergeAttributeData, false);
			NetworkUtils.Equip.DoEquipEvolutionRequest(equipRowId, delegate(bool isOk, EquipEvolutionResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.ShowAttributeToast(oldMemberAttributeData, equipRowId);
				GameApp.Sound.PlayClip(628, 1f);
				if (this.equipDetailsViewModule.m_textLevel != null && this.equipDetailsViewModule.m_textLevel.gameObject.activeSelf)
				{
					this.equipDetailsViewModule.m_textLevel.rectTransform.localScale = Vector3.one;
					Sequence sequence = new SequencePool().Get();
					Vector3 vector = Vector3.one * 2f;
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.equipDetailsViewModule.m_textLevel.transform, vector, 0.15f));
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.equipDetailsViewModule.m_textLevel.transform, Vector3.one, 0.15f));
				}
			});
		}

		private void OnBtnRefiningClick()
		{
		}

		private void ShowAttributeToast(MemberAttributeData oldMemberAttributeData, ulong equipRowId)
		{
			EquipData equipDataByRowId = this.equipDetailsViewModule.m_equipDataModule.GetEquipDataByRowId(equipRowId);
			List<MergeAttributeData> mergeAttributeData = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipDataByRowId.id)
				.GetMergeAttributeData((int)equipDataByRowId.level, equipDataByRowId.evolution);
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(mergeAttributeData, false);
			List<string> list = new List<string>();
			for (int i = 0; i < mergeAttributeData.Count; i++)
			{
				MergeAttributeData mergeAttributeData2 = mergeAttributeData[i];
				if (mergeAttributeData2 != null && !list.Contains(mergeAttributeData2.Header))
				{
					list.Add(mergeAttributeData2.Header);
				}
			}
			EventArgsAddAttributeTipNode eventArgsAddAttributeTipNode = new EventArgsAddAttributeTipNode();
			for (int j = 0; j < list.Count; j++)
			{
				string text = list[j];
				long basicAttributeValue = oldMemberAttributeData.GetBasicAttributeValue(text);
				long num = memberAttributeData.GetBasicAttributeValue(text) - basicAttributeValue;
				if (num > 0L)
				{
					eventArgsAddAttributeTipNode.AddData(text, num);
				}
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddAttributeTipNode, eventArgsAddAttributeTipNode);
		}

		private EquipDetailsViewModule equipDetailsViewModule;

		public CustomButton m_btnPutUp;

		public CustomButton m_btnPutDown;

		public CustomButton m_btnLevelUp;

		public CustomButton m_btnQuickLevelUp;

		public CustomButton m_btnEvolution;

		public CustomButton m_btnRefining;

		public GameObject m_equipCostObj;

		public GameObject m_equipCostItem;

		private List<CommonCostItem> m_commonCostItemList = new List<CommonCostItem>();
	}
}
