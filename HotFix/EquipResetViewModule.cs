using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Equip;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class EquipResetViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.tabGroup.CollectChildButtons();
			this.uiEquipMain.Init();
			this.uiEquipMain.SetButtonEnable(false);
			this.uiItemMaterialOrigin.gameObject.SetActive(false);
			this.txtEmpty.text = "";
		}

		public override void OnOpen(object data)
		{
			this.openData = data as EquipResetViewModule.OpenData;
			if (this.openData == null)
			{
				HLog.LogError("EquipResetViewModule openData is null");
				GameApp.View.CloseView(ViewName.EquipResetViewModule, null);
				return;
			}
			if (this.openData.equipData.level > 1U)
			{
				this.tabGroup.ChooseButtonName(this.tabRecycleLevel.name);
				return;
			}
			if (this.openData.equipData.composeId > 0)
			{
				this.tabGroup.ChooseButtonName(this.tabRecycleQuality.name);
				return;
			}
			this.tabGroup.ChooseButtonName(this.tabRecycleLevel.name);
		}

		public override void OnClose()
		{
			this.openData = null;
			this.currentTab = null;
		}

		public override void OnDelete()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.tabGroup.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchPage);
			this.btnRecycleLevel.onClick.AddListener(new UnityAction(this.OnBtnRecycleLevelClick));
			this.btnRecycleQuality.onClick.AddListener(new UnityAction(this.OnBtnRecycleQualityClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
			this.tabGroup.OnSwitch = null;
			this.btnRecycleLevel.onClick.RemoveAllListeners();
			this.btnRecycleQuality.onClick.RemoveAllListeners();
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnBtnCloseClick();
			}
		}

		public void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.EquipResetViewModule, null);
		}

		private void OnSwitchPage(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			if (button == this.currentTab)
			{
				return;
			}
			this.currentTab = button;
			this.UpdateView();
		}

		private void OnBtnRecycleLevelClick()
		{
			NetworkUtils.Equip.DoEquipResetRequest(this.openData.equipData.rowID, delegate(bool result, EquipLevelResetResponse resp)
			{
				if (result)
				{
					GameApp.View.CloseView(ViewName.EquipResetViewModule, null);
					GameApp.View.CloseView(ViewName.EquipDetailsViewModule, null);
				}
			});
		}

		private void OnBtnRecycleQualityClick()
		{
			NetworkUtils.Equip.DoEquipDecomposeRequest(this.openData.equipData.rowID, delegate(bool result, EquipDecomposeResponse resp)
			{
				if (result)
				{
					GameApp.View.CloseView(ViewName.EquipResetViewModule, null);
					GameApp.View.CloseView(ViewName.EquipDetailsViewModule, null);
				}
			});
		}

		private void UpdateView()
		{
			this.uiEquipMain.RefreshData(this.openData.equipData);
			if (this.currentTab == this.tabRecycleLevel)
			{
				this.UpdateRecycleLevelPage();
				return;
			}
			if (this.currentTab == this.tabRecycleQuality)
			{
				this.UpdateRecycleQualityPage();
			}
		}

		private void UpdateRecycleLevelPage()
		{
			this.btnRecycleLevel.gameObject.SetActive(true);
			this.btnRecycleQuality.gameObject.SetActive(false);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("title_equip_recycle_level");
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("desc_equip_recycle_level");
			List<ItemData> recycleReturnList = this.GetRecycleReturnList(false);
			this.UpdateRecycleItems(recycleReturnList);
		}

		private void UpdateRecycleQualityPage()
		{
			this.btnRecycleLevel.gameObject.SetActive(false);
			this.btnRecycleQuality.gameObject.SetActive(true);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("title_equip_recycle_quality");
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("desc_equip_recycle_quality");
			EquipData equipData = this.openData.equipData;
			int composeId = this.openData.equipData.composeId;
			new Dictionary<int, long>();
			List<ItemData> recycleReturnList = this.GetRecycleReturnList(true);
			this.UpdateRecycleItems(recycleReturnList);
		}

		private List<ItemData> GetRecycleReturnList(bool isRecycleQuality)
		{
			Dictionary<int, long> dictionary = new Dictionary<int, long>();
			EquipData equipData = this.openData.equipData;
			if (isRecycleQuality)
			{
				string[] array = new string[0];
				Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
				if (elementById != null)
				{
					if (elementById.qualityPlus > 0)
					{
						int num = (int)(equipData.id / 100U * 100U + (uint)elementById.id - (uint)elementById.qualityPlus);
						dictionary[num] = 1L;
					}
					if (elementById.RecycleSelfquality != null && elementById.RecycleSelfquality.Length != 0)
					{
						for (int i = 0; i < elementById.RecycleSelfquality.Length; i++)
						{
							int num2 = elementById.RecycleSelfquality[i];
							int num3 = (int)(equipData.id / 100U * 100U + (uint)num2);
							if (dictionary.ContainsKey(num3))
							{
								Dictionary<int, long> dictionary2 = dictionary;
								int num4 = num3;
								dictionary2[num4] += 1L;
							}
							else
							{
								dictionary[num3] = 1L;
							}
						}
					}
					switch (equipData.equipType)
					{
					case EquipType.Weapon:
						array = elementById.Recycle1New;
						break;
					case EquipType.Clothes:
						array = elementById.Recycle2New;
						break;
					case EquipType.Ring:
						array = elementById.Recycle3New;
						break;
					case EquipType.Accessory:
						array = elementById.Recycle4New;
						break;
					default:
						array = new string[0];
						break;
					}
					for (int j = 0; j < array.Length; j++)
					{
						string[] array2 = array[j].Split(',', StringSplitOptions.None);
						int num5 = int.Parse(array2[0]);
						long num6 = long.Parse(array2[1]);
						if (dictionary.ContainsKey(num5))
						{
							Dictionary<int, long> dictionary2 = dictionary;
							int num4 = num5;
							dictionary2[num4] += num6;
						}
						else
						{
							dictionary.Add(num5, num6);
						}
					}
				}
			}
			uint level = equipData.level;
			int num7 = 1;
			while ((long)num7 < (long)((ulong)level))
			{
				int updateLevelId = equipData.GetUpdateLevelId(num7);
				Equip_updateLevel elementById2 = GameApp.Table.GetManager().GetEquip_updateLevelModelInstance().GetElementById(updateLevelId);
				if (elementById2 != null)
				{
					string[] levelupCost = elementById2.levelupCost;
					for (int k = 0; k < levelupCost.Length; k++)
					{
						string[] array3 = levelupCost[k].Split(',', StringSplitOptions.None);
						int num8 = int.Parse(array3[0]);
						long num9 = long.Parse(array3[1]);
						if (dictionary.ContainsKey(num8))
						{
							Dictionary<int, long> dictionary2 = dictionary;
							int num4 = num8;
							dictionary2[num4] += num9;
						}
						else
						{
							dictionary.Add(num8, num9);
						}
					}
				}
				num7++;
			}
			int evolution = equipData.evolution;
			for (int l = 1; l < evolution; l++)
			{
				int evolutionId = equipData.GetEvolutionId(l);
				Equip_equipEvolution elementById3 = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionId);
				if (elementById3 != null)
				{
					string[] evolutionCost = elementById3.evolutionCost;
					for (int m = 0; m < evolutionCost.Length; m++)
					{
						string[] array4 = evolutionCost[m].Split(',', StringSplitOptions.None);
						int num10 = int.Parse(array4[0]);
						long num11 = long.Parse(array4[1]);
						if (dictionary.ContainsKey(num10))
						{
							Dictionary<int, long> dictionary2 = dictionary;
							int num4 = num10;
							dictionary2[num4] += num11;
						}
						else
						{
							dictionary.Add(num10, num11);
						}
					}
				}
			}
			List<ItemData> list = new List<ItemData>();
			foreach (KeyValuePair<int, long> keyValuePair in dictionary)
			{
				ItemData itemData = new ItemData();
				itemData.SetID(keyValuePair.Key);
				itemData.SetCount(keyValuePair.Value);
				list.Add(itemData);
			}
			return list;
		}

		private void UpdateRecycleItems(List<ItemData> itemDataList)
		{
			float time = Time.time;
			for (int i = 0; i < itemDataList.Count; i++)
			{
				UIItem uiitem;
				if (i >= this.uiItemMaterialList.Count)
				{
					uiitem = Object.Instantiate<UIItem>(this.uiItemMaterialOrigin, this.uiItemMaterialOrigin.transform.parent);
					uiitem.gameObject.SetActive(true);
					this.uiItemMaterialList.Add(uiitem);
					uiitem.Init();
				}
				else
				{
					uiitem = this.uiItemMaterialList[i];
				}
				uiitem.gameObject.SetActive(true);
				uiitem.SetData(itemDataList[i].ToPropData());
				uiitem.OnRefresh();
				uiitem.m_button.gameObject.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(time, i, 100025);
			}
			for (int j = itemDataList.Count; j < this.uiItemMaterialList.Count; j++)
			{
				this.uiItemMaterialList[j].gameObject.SetActive(false);
			}
			if (itemDataList.Count != 0)
			{
				this.txtEmpty.gameObject.SetActive(false);
				return;
			}
			this.txtEmpty.gameObject.SetActive(true);
			if (this.currentTab == this.tabRecycleLevel)
			{
				this.txtEmpty.text = Singleton<LanguageManager>.Instance.GetInfoByID("desc_equip_recycle_empty");
				this.btnRecycleLevel.gameObject.SetActive(false);
				return;
			}
			this.btnRecycleQuality.gameObject.SetActive(false);
			this.txtEmpty.text = Singleton<LanguageManager>.Instance.GetInfoByID("desc_equip_recycle_empty2");
		}

		public UIPopCommon uiPopCommon;

		public CustomChooseButtonGroup tabGroup;

		public CustomChooseButton tabRecycleLevel;

		public CustomChooseButton tabRecycleQuality;

		public CustomButton btnRecycleLevel;

		public CustomButton btnRecycleQuality;

		public CustomText txtTitle;

		public CustomText txtDesc;

		public CustomText txtEmpty;

		public UIHeroEquipItem uiEquipMain;

		public UIItem uiItemMaterialOrigin;

		private EquipResetViewModule.OpenData openData;

		private CustomChooseButton currentTab;

		private List<UIItem> uiItemMaterialList = new List<UIItem>();

		public class OpenData
		{
			public EquipData equipData;
		}
	}
}
