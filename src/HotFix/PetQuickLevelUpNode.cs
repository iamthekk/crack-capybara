using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class PetQuickLevelUpNode : MonoBehaviour
	{
		private void Awake()
		{
			this.btnHide.m_onClick = delegate
			{
				Action action = this.onClose;
				if (action != null)
				{
					action();
				}
				this.Hide();
			};
			this.btnLevelUp.m_onClick = delegate
			{
				Action action2 = this.onLevelUpOne;
				if (action2 == null)
				{
					return;
				}
				action2();
			};
			this.btnQuickLevelUp.m_onClick = delegate
			{
				Action action3 = this.onLevelUpMore;
				if (action3 == null)
				{
					return;
				}
				action3();
			};
		}

		private void OnDestroy()
		{
			this.btnHide.m_onClick = null;
			this.btnLevelUp.m_onClick = null;
			this.btnQuickLevelUp.m_onClick = null;
		}

		public void ReCalcLevelUpInfo(PetData petData)
		{
			this.cacheCostData.Clear();
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petData.petId);
			int talentStage = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage;
			int level = petData.level;
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num2++;
				int num3 = num2 + level;
				Pet_petLevel elementById2 = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(petData.GetPetLevelId(num3 - 1));
				if (elementById2 == null || elementById2.nextID <= 0 || talentStage < elementById2.talentNeed)
				{
					break;
				}
				List<ItemData> levelUpCosts = elementById.GetLevelUpCosts(level + num2 - 1);
				if (levelUpCosts == null || levelUpCosts.Count == 0)
				{
					break;
				}
				bool flag = true;
				for (int i = 0; i < levelUpCosts.Count; i++)
				{
					ItemData itemData = levelUpCosts[i];
					if (this.cacheCostData.ContainsKey(itemData.ID))
					{
						this.cacheCostData[itemData.ID].SetCount(this.cacheCostData[itemData.ID].Count + itemData.Count);
					}
					else
					{
						this.cacheCostData.Add(itemData.ID, itemData);
					}
					long totalCount = this.cacheCostData[itemData.ID].TotalCount;
					if (dataModule.GetItemDataCountByid((ulong)itemData.ID) < totalCount)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				num = num2 + level;
			}
			this.toLevel = num;
		}

		public void Show(PetData petData)
		{
			base.gameObject.SetActive(true);
			this.redNodeQuickLevelUp.Value = (petData.ConditionInDeployOk() ? 1 : 0);
			this.RefreshCost(petData);
			this.txtQuickLevelUp.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { this.toLevel });
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		private void RefreshCost(PetData petData)
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			List<ItemData> levelUpCosts = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petData.petId)
				.GetLevelUpCosts(petData.level);
			ItemData itemData = ((levelUpCosts.Count > 0) ? levelUpCosts[0] : null);
			ItemData itemData2 = ((levelUpCosts.Count > 1) ? levelUpCosts[1] : null);
			long num = ((itemData != null) ? dataModule.GetItemDataCountByid((ulong)((long)itemData.ID)) : 0L);
			long num2 = ((itemData != null) ? itemData.TotalCount : 0L);
			long num3 = ((itemData2 != null) ? dataModule.GetItemDataCountByid((ulong)((long)itemData2.ID)) : 0L);
			long num4 = ((itemData2 != null) ? itemData2.TotalCount : 0L);
			if (itemData != null)
			{
				this.costItem1.gameObject.SetActive(true);
				this.costItem1.SetData(itemData, num, num2);
			}
			else
			{
				this.costItem1.gameObject.SetActive(false);
			}
			if (itemData2 != null)
			{
				this.costItem2.gameObject.SetActive(true);
				this.costItem2.SetData(itemData2, num3, num4);
				return;
			}
			this.costItem2.gameObject.SetActive(false);
		}

		public CustomButton btnHide;

		public CustomButton btnLevelUp;

		public CustomButton btnQuickLevelUp;

		public RedNodeOneCtrl redNodeQuickLevelUp;

		public CommonCostItem costItem1;

		public CommonCostItem costItem2;

		public CustomText txtQuickLevelUp;

		public Action onLevelUpOne;

		public Action onLevelUpMore;

		public Action onClose;

		[NonSerialized]
		public int toLevel;

		private Dictionary<int, ItemData> cacheCostData = new Dictionary<int, ItemData>();
	}
}
