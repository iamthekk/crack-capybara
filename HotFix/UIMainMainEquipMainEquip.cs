using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainMainEquipMainEquip : BaseMainEquipPanel
	{
		protected override void OnInit()
		{
			this.m_heroDataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.m_equipDataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			this.m_functionDataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
			this.CombatItem.Init();
			for (int i = 0; i < this.m_equipUpItemList.Count; i++)
			{
				UIEquipUpItem uiequipUpItem = this.m_equipUpItemList[i];
				EquipType equipType = uiequipUpItem.m_equipType;
				int equipTypeIndex = uiequipUpItem.m_equipTypeIndex;
				uiequipUpItem.Init();
				if (!(this.m_equipUpPrefab == null))
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_equipUpPrefab);
					gameObject.SetParentNormal(uiequipUpItem.transform.parent, false);
					gameObject.transform.localPosition = uiequipUpItem.transform.localPosition;
					gameObject.transform.rotation = uiequipUpItem.transform.rotation;
					gameObject.transform.localScale = Vector3.one;
					UIHeroEquipItem component = gameObject.GetComponent<UIHeroEquipItem>();
					component.m_onClick = new Action<UIHeroEquipItem>(this.OnClickPutOnEquipItem);
					component.SetEquipType(equipType, equipTypeIndex);
					component.Init();
					this.m_equipItems[gameObject.GetInstanceID()] = component;
				}
			}
			this.m_sortBt.onClick.AddListener(new UnityAction(this.OnClickSortBt));
			this.m_mergeBt.onClick.AddListener(new UnityAction(this.OnClickMergeBt));
			this.m_btnPictorial.onClick.AddListener(new UnityAction(this.OnBtnPictorialClick));
			this.m_btnPets.onClick.AddListener(new UnityAction(this.OnBtnPetsClick));
			this.m_btnCollection.onClick.AddListener(new UnityAction(this.OnBtnCollectionClick));
			this.m_btnMount.onClick.AddListener(new UnityAction(this.OnBtnMountClick));
			this.m_btnArtifact.onClick.AddListener(new UnityAction(this.OnBtnArtifactClick));
			this.m_btnFashion.onClick.AddListener(new UnityAction(this.OnBtnFashionClick));
			this.m_BtnAttrDetail.onClick.AddListener(new UnityAction(this.OnClickAttributeBt));
			this.Button_Career.m_onClick = new Action(this.OnClickTalentLegacyCareer);
			this.m_scrollView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			RedPointController.Instance.RegRecordChange("Equip.Hero.Compose", new Action<RedNodeListenData>(this.OnRedPointComposeChange));
			RedPointController.Instance.RegRecordChange("Equip.Pet", new Action<RedNodeListenData>(this.OnRedPointPetsChange));
			RedPointController.Instance.RegRecordChange("Equip.Pictorial", new Action<RedNodeListenData>(this.OnRedPointPictorialChange));
			RedPointController.Instance.RegRecordChange("Equip.Collection", new Action<RedNodeListenData>(this.OnRedPointCollectionChange));
			RedPointController.Instance.RegRecordChange("Equip.Mount", new Action<RedNodeListenData>(this.OnRedPointMountChange));
			RedPointController.Instance.RegRecordChange("Equip.Artifact", new Action<RedNodeListenData>(this.OnRedPointArtifactChange));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar", new Action<RedNodeListenData>(this.OnRedPointFashionChange));
			RedPointController.Instance.RegRecordChange("Equip.TalentLegacySkill", new Action<RedNodeListenData>(this.OnRedPointLegacyCareerSkillChange));
			GuideController.Instance.DelTarget("BtnPets");
			GuideController.Instance.AddTarget("BtnPets", this.m_btnPets.transform);
			GuideController.Instance.DelTarget("Button_EquipCareerItem");
			GuideController.Instance.AddTarget("Button_EquipCareerItem", this.Button_Career.transform);
		}

		public override void OnShow()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			this.RefreshCardData(this.m_heroDataModule.MainCardData);
			this.Skin_ModelItem.Init();
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PushUIModelItem(this.Skin_ModelItem, new Action(this.FreshSkin));
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
				this.TitleCtrl.SetAndFreshToMy();
			}
			this.UpdateEquipView();
			this.SetAttributeDatas(this.m_cardData);
			this.SetEquipTitleData();
			this.SetPlayerName();
			this.SetTalentLegacyCareer();
			this.CombatItem.OnShow();
			this._curTime = Time.unscaledTime;
			this.RefreshEquipDatas();
			this.m_scrollView.SetListItemCount(this.m_nodeDatas.Count, true);
			this.m_scrollView.RefreshAllShownItem();
			this.m_scrollView.MovePanelToItemIndex(0, 0f);
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			this.Button_Career.gameObject.SetActiveSafe(dataModule.IsCheckMaxLevel() && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIMount_ChangeRide, new HandlerEvent(this.OnEvent_MountChanged));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ClothesData_SelfClothesChanged, new HandlerEvent(this.OnEvent_SelfClothesChanged));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventUserInfoChanged));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TalentLegacySelectCareer, new HandlerEvent(this.OnEventTalentLegacySelectCareer));
			this.PlayAnimation();
		}

		public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
		{
		}

		public override void OnHide()
		{
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PopUIModelItem(this.Skin_ModelItem);
			this.Skin_ModelItem.OnHide(false);
			this.Skin_ModelItem.DeInit();
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
			this.CombatItem.OnHide();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_ChangeRide, new HandlerEvent(this.OnEvent_MountChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ClothesData_SelfClothesChanged, new HandlerEvent(this.OnEvent_SelfClothesChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventUserInfoChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TalentLegacySelectCareer, new HandlerEvent(this.OnEventTalentLegacySelectCareer));
		}

		public override void PlayAnimation()
		{
			if (this.m_aniamtor == null)
			{
				return;
			}
			this.m_aniamtor.SetTrigger("Run");
		}

		public void OnLanguageChange()
		{
		}

		protected override void OnDeInit()
		{
			this.CombatItem.DeInit();
			RedPointController.Instance.UnRegRecordChange("Equip.Hero", new Action<RedNodeListenData>(this.OnRedPointComposeChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Pet", new Action<RedNodeListenData>(this.OnRedPointPetsChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Pictorial", new Action<RedNodeListenData>(this.OnRedPointPictorialChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Collection", new Action<RedNodeListenData>(this.OnRedPointCollectionChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Mount", new Action<RedNodeListenData>(this.OnRedPointMountChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Artifact", new Action<RedNodeListenData>(this.OnRedPointArtifactChange));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar", new Action<RedNodeListenData>(this.OnRedPointFashionChange));
			RedPointController.Instance.UnRegRecordChange("Equip.TalentLegacySkill", new Action<RedNodeListenData>(this.OnRedPointLegacyCareerSkillChange));
			for (int i = 0; i < this.m_equipUpItemList.Count; i++)
			{
				UIEquipUpItem uiequipUpItem = this.m_equipUpItemList[i];
				if (!(uiequipUpItem == null))
				{
					uiequipUpItem.DeInit();
				}
			}
			foreach (KeyValuePair<int, UIHeroEquipItem> keyValuePair in this.m_equipItems)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_equipItems.Clear();
			this.m_currentHp = 0L;
			this.m_currentAttack = 0L;
			this.m_currentDefense = 0L;
			if (this.m_sortBt != null)
			{
				this.m_sortBt.onClick.RemoveListener(new UnityAction(this.OnClickSortBt));
			}
			if (this.m_mergeBt != null)
			{
				this.m_mergeBt.onClick.RemoveListener(new UnityAction(this.OnClickMergeBt));
			}
			if (this.m_btnPictorial != null)
			{
				this.m_btnPictorial.onClick.RemoveListener(new UnityAction(this.OnBtnPictorialClick));
			}
			if (this.m_btnPets != null)
			{
				this.m_btnPets.onClick.RemoveListener(new UnityAction(this.OnBtnPetsClick));
			}
			if (this.m_btnCollection != null)
			{
				this.m_btnCollection.onClick.RemoveListener(new UnityAction(this.OnBtnCollectionClick));
			}
			if (this.m_btnMount != null)
			{
				this.m_btnMount.onClick.RemoveListener(new UnityAction(this.OnBtnMountClick));
			}
			if (this.m_btnArtifact != null)
			{
				this.m_btnArtifact.onClick.RemoveListener(new UnityAction(this.OnBtnArtifactClick));
			}
			if (this.m_btnFashion != null)
			{
				this.m_btnFashion.onClick.RemoveListener(new UnityAction(this.OnBtnFashionClick));
			}
			if (this.m_BtnAttrDetail != null)
			{
				this.m_BtnAttrDetail.onClick.RemoveListener(new UnityAction(this.OnClickAttributeBt));
			}
			this.Button_Career.m_onClick = null;
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair2 in this.m_nodeDic)
			{
				keyValuePair2.Value.DeInit();
			}
			this.m_nodeDic.Clear();
			this.m_nodeDatas.Clear();
			this.m_equipDatas.Clear();
			this.m_heroDataModule = null;
			this.m_propDataModule = null;
			this.m_equipDataModule = null;
			this.m_addAttributeDataModule = null;
		}

		private void RefreshCardData(CardData cardData)
		{
			this.m_cardData = new CardData();
			this.m_cardData.CloneFrom(cardData);
			this.m_cardData.UpdateAttribute(this.m_addAttributeDataModule.AttributeDatas);
		}

		private void UpdateEquipView()
		{
			foreach (UIHeroEquipItem uiheroEquipItem in this.m_equipItems.Values)
			{
				EquipType equipType = uiheroEquipItem.equipType;
				int equipTypeIndex = uiheroEquipItem.equipTypeIndex;
				ulong equipDressRowId = this.m_equipDataModule.GetEquipDressRowId(equipType, equipTypeIndex);
				EquipData equipDataByRowId = this.m_equipDataModule.GetEquipDataByRowId(equipDressRowId);
				uiheroEquipItem.RefreshData(equipDataByRowId);
				uiheroEquipItem.SetActive(equipDataByRowId != null);
			}
			this.CheckShowUpLevel();
		}

		private void CheckShowUpLevel()
		{
			foreach (KeyValuePair<int, UIHeroEquipItem> keyValuePair in this.m_equipItems)
			{
				if (!(keyValuePair.Value == null) && keyValuePair.Value.m_equipData != null)
				{
					EquipData equipData = keyValuePair.Value.m_equipData;
					int num;
					bool flag = this.m_equipDataModule.IsCanLevelUp(equipData) && this.m_equipDataModule.IsHaveLevelUpCost(equipData, out num);
					if (!flag)
					{
						flag = equipData.IsCanEvolution() && this.m_equipDataModule.IsMatchTalentLimit(equipData) && this.m_equipDataModule.IsHaveEvolutionCost(equipData);
					}
					keyValuePair.Value.SetUpLevelActive(flag);
				}
			}
		}

		private void SetTalentLegacyCareer()
		{
			TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnGetTalentLegacyInfo();
			this.Image_Career.gameObject.SetActiveSafe(false);
			if (talentLegacyInfo != null)
			{
				TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(talentLegacyInfo.SelectCareerId);
				if (talentLegacy_career != null)
				{
					this.Image_Career.SetImage(talentLegacy_career.previewIconId, talentLegacy_career.previewIcon);
				}
				this.Image_Career.gameObject.SetActiveSafe(talentLegacyInfo.SelectCareerId > 0);
			}
		}

		private void SetPlayerName()
		{
		}

		private void SetAttributeDatas(CardData cardData)
		{
			MemberAttributeData memberAttributeData = cardData.m_memberAttributeData;
			this.m_currentHp = memberAttributeData.GetHpMax4UI();
			this.m_currentAttack = memberAttributeData.GetAttack4UI();
			this.m_currentDefense = memberAttributeData.GetDefence4UI();
			this.m_hpTxt.text = DxxTools.FormatNumber(this.m_currentHp);
			this.m_attackTxt.text = DxxTools.FormatNumber(this.m_currentAttack);
			this.m_defenseTxt.text = DxxTools.FormatNumber(this.m_currentDefense);
		}

		private void SetEquipTitleData()
		{
			this.m_sortBtTxt.text = this.GetSortName(this.m_sortType);
		}

		private string GetSortName(EquipSortType sortType)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(((int)(1458 + sortType)).ToString());
		}

		private EquipSortType GetNextSortType(EquipSortType sortType)
		{
			int length = Enum.GetValues(typeof(EquipSortType)).Length;
			int num = (int)(sortType + 1);
			if (num >= length)
			{
				num = 0;
			}
			return (EquipSortType)num;
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.m_nodeDatas.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			UIMainMainEquipMainEquip.NodeData nodeData = this.m_nodeDatas[index];
			if (nodeData.m_isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.m_nodeDic.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.m_nodeDic[instanceID] = component;
				}
				return loopListViewItem;
			}
			if (nodeData.m_isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.m_nodeDic.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.m_nodeDic[instanceID2] = component2;
				}
				return loopListViewItem;
			}
			if (nodeData.m_equipDatas != null)
			{
				int num = index - 1;
				if (num <= 0)
				{
					int count = nodeData.m_equipDatas.Count;
				}
				else
				{
					int count2 = nodeData.m_equipDatas.Count;
					int columnCount = this.m_columnCount;
				}
				float num2;
				if (num == 0)
				{
					num2 = this._curTime;
				}
				bool flag;
				if (num < 4)
				{
					num2 = this._curTime + (float)(num * this.m_columnCount) * 0.02f + 0.02f;
					flag = true;
				}
				else
				{
					num2 = this._curTime + (float)(num * this.m_columnCount) * 0.02f + 0.02f;
					flag = false;
				}
				loopListViewItem = view.NewListViewItem("Node");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.m_nodeDic.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					UIHeroEquipItemGroup component3 = loopListViewItem.gameObject.GetComponent<UIHeroEquipItemGroup>();
					component3.SetData(this.m_columnCount, new Action<UIHeroEquipItem>(this.OnClickEquipItem));
					component3.Init();
					this.m_nodeDic[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				UIHeroEquipItemGroup uiheroEquipItemGroup = customBehaviour3 as UIHeroEquipItemGroup;
				if (uiheroEquipItemGroup != null)
				{
					uiheroEquipItemGroup.RefreshData(nodeData.m_equipDatas, num2, 0.2f, flag);
					if (index == 1)
					{
						UIHeroEquipItem itemByIndex = uiheroEquipItemGroup.GetItemByIndex(0);
						if (itemByIndex != null)
						{
							GuideController.Instance.DelTarget("UI_EquipItem");
							GuideController.Instance.AddTarget("UI_EquipItem", itemByIndex.transform);
						}
					}
				}
				return loopListViewItem;
			}
			if (!string.IsNullOrEmpty(nodeData.m_titleName))
			{
				loopListViewItem = view.NewListViewItem("Title");
				int instanceID4 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour4;
				this.m_nodeDic.TryGetValue(instanceID4, out customBehaviour4);
				if (customBehaviour4 == null)
				{
					UIHeroEquipItemTitle component4 = loopListViewItem.gameObject.GetComponent<UIHeroEquipItemTitle>();
					component4.Init();
					this.m_nodeDic[instanceID4] = component4;
					customBehaviour4 = component4;
				}
				UIHeroEquipItemTitle uiheroEquipItemTitle = customBehaviour4 as UIHeroEquipItemTitle;
				if (uiheroEquipItemTitle != null)
				{
					uiheroEquipItemTitle.RefreshData(nodeData.m_titleName);
				}
				return loopListViewItem;
			}
			if (nodeData.m_propDatas != null)
			{
				loopListViewItem = view.NewListViewItem("PropNode");
				int instanceID5 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour5;
				this.m_nodeDic.TryGetValue(instanceID5, out customBehaviour5);
				if (customBehaviour5 == null)
				{
					UIHeroEquipItemPropGroup component5 = loopListViewItem.gameObject.GetComponent<UIHeroEquipItemPropGroup>();
					component5.SetData(this.m_columnCount, new Action<UIItem, PropData, object>(this.OnClickPopItem));
					component5.Init();
					this.m_nodeDic[instanceID5] = component5;
					customBehaviour5 = component5;
				}
				UIHeroEquipItemPropGroup uiheroEquipItemPropGroup = customBehaviour5 as UIHeroEquipItemPropGroup;
				if (uiheroEquipItemPropGroup != null)
				{
					uiheroEquipItemPropGroup.RefreshData(nodeData.m_propDatas);
				}
				return loopListViewItem;
			}
			return loopListViewItem;
		}

		private void RefreshEquipDatas()
		{
			List<EquipData> equipDatas = this.m_equipDataModule.GetEquipDatas(false, true);
			this.m_equipDatas = this.m_equipDataModule.SortEquipDatas(equipDatas, this.m_sortType);
			this.m_nodeDatas = new List<UIMainMainEquipMainEquip.NodeData>();
			this.m_nodeDatas.Add(new UIMainMainEquipMainEquip.NodeData
			{
				m_isTopSpace = true
			});
			for (int i = 0; i < this.m_equipDatas.Count; i += this.m_columnCount)
			{
				UIMainMainEquipMainEquip.NodeData nodeData = new UIMainMainEquipMainEquip.NodeData();
				nodeData.m_isTopSpace = false;
				nodeData.m_equipDatas = new List<UIHeroEquipItemGroup.EquipDataItem>();
				for (int j = 0; j < this.m_columnCount; j++)
				{
					int num = i + j;
					if (num < this.m_equipDatas.Count)
					{
						UIHeroEquipItemGroup.EquipDataItem equipDataItem = new UIHeroEquipItemGroup.EquipDataItem();
						equipDataItem.m_equipData = this.m_equipDatas[num];
						equipDataItem.m_isNeedEquipRedTip = this.m_equipDataModule.NeedEquipRedTip(equipDataItem.m_equipData.equipType, equipDataItem.m_equipData.composeId);
						nodeData.m_equipDatas.Add(equipDataItem);
					}
				}
				this.m_nodeDatas.Add(nodeData);
			}
			List<ItemData> itemDatas = this.m_propDataModule.GetItemDatas(ItemType.eUseItem, new PropType?(PropType.EquipComposeMaterial));
			if (itemDatas.Count > 0)
			{
				this.m_nodeDatas.Add(new UIMainMainEquipMainEquip.NodeData
				{
					m_titleName = Singleton<LanguageManager>.Instance.GetInfoByID("title_equip_material")
				});
				for (int k = 0; k < itemDatas.Count; k += this.m_columnCount)
				{
					UIMainMainEquipMainEquip.NodeData nodeData2 = new UIMainMainEquipMainEquip.NodeData();
					nodeData2.m_isTopSpace = false;
					nodeData2.m_propDatas = new List<PropData>();
					for (int l = 0; l < this.m_columnCount; l++)
					{
						int num2 = k + l;
						if (num2 < itemDatas.Count)
						{
							nodeData2.m_propDatas.Add(itemDatas[num2].ToPropData());
						}
					}
					this.m_nodeDatas.Add(nodeData2);
				}
			}
			this.m_nodeDatas.Add(new UIMainMainEquipMainEquip.NodeData
			{
				m_isBottomSpace = true
			});
		}

		private void OnClickTalentLegacyCareer()
		{
			int num = 0;
			TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnGetTalentLegacyInfo();
			if (talentLegacyInfo != null)
			{
				num = talentLegacyInfo.SelectCareerId;
			}
			if (num <= 0)
			{
				if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.MainTalent, null, true))
				{
					UIBaseMainPageNode.OpenData openData = new UIBaseMainPageNode.OpenData();
					openData.OriginType = UIBaseMainPageNode.EOriginType.Equip;
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainTalent, openData);
				}
				return;
			}
			TalentLegacySkillSelectViewModule.OpenData openData2 = default(TalentLegacySkillSelectViewModule.OpenData);
			openData2.CareerId = num;
			openData2.OriginType = 2;
			GameApp.View.OpenView(ViewName.TalentLegacySkillSelectViewModule, openData2, 1, null, null);
		}

		private void OnClickPutOnEquipItem(UIHeroEquipItem obj)
		{
			if (obj == null || obj.m_equipData == null)
			{
				return;
			}
			EquipDetailsViewModule.OpenData openData = new EquipDetailsViewModule.OpenData();
			openData.m_equipData = obj.m_equipData;
			GameApp.View.OpenView(ViewName.EquipDetailsViewModule, openData, 1, null, null);
		}

		private void OnClickAttributeBt()
		{
			GameApp.View.OpenView(ViewName.AttributeShowViewModule, this.m_addAttributeDataModule.MemberAttributeData, 1, null, null);
		}

		private void OnClickSortBt()
		{
			this.m_sortType = this.GetNextSortType(this.m_sortType);
			this.m_sortBtTxt.text = this.GetSortName(this.m_sortType);
			this.RefreshEquipDatas();
			this.m_scrollView.SetListItemCount(this.m_nodeDatas.Count, true);
			this.m_scrollView.RefreshAllShownItem();
		}

		private void OnClickMergeBt()
		{
			GameApp.View.OpenView(ViewName.EquipMergeViewModule, null, 1, null, null);
		}

		private void OnBtnPictorialClick()
		{
		}

		private void OnBtnPetsClick()
		{
			if (this.m_functionDataModule.IsFunctionOpened(FunctionID.Pet))
			{
				GameApp.View.OpenView(ViewName.PetViewModule, null, 1, null, null);
				return;
			}
			string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(51);
			GameApp.View.ShowStringTip(lockTips);
		}

		private void OnBtnCollectionClick()
		{
			if (this.m_functionDataModule.IsFunctionOpened(FunctionID.Collection))
			{
				GameApp.View.OpenView(ViewName.CollectionViewModule, null, 1, null, null);
				return;
			}
			string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(52);
			GameApp.View.ShowStringTip(lockTips);
		}

		private void OnBtnMountClick()
		{
			if (this.m_functionDataModule.IsFunctionOpened(FunctionID.Mount))
			{
				GameApp.View.OpenView(ViewName.MountViewModule, null, 1, null, null);
				return;
			}
			string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(53);
			GameApp.View.ShowStringTip(lockTips);
		}

		private void OnBtnArtifactClick()
		{
			if (this.m_functionDataModule.IsFunctionOpened(FunctionID.Artifact))
			{
				GameApp.View.OpenView(ViewName.ArtifactViewModule, null, 1, null, null);
				return;
			}
			string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(54);
			GameApp.View.ShowStringTip(lockTips);
		}

		private void OnBtnFashionClick()
		{
			GameApp.View.OpenView(ViewName.PlayerAvatarClothesViewModule, 2, 1, null, null);
		}

		private void OnClickEquipItem(UIHeroEquipItem obj)
		{
			if (obj == null || obj.m_equipData == null)
			{
				return;
			}
			EquipDetailsViewModule.OpenData openData = new EquipDetailsViewModule.OpenData();
			openData.m_equipData = obj.m_equipData;
			GameApp.View.OpenView(ViewName.EquipDetailsViewModule, openData, 1, null, null);
		}

		private void OnClickPopItem(UIItem itemCtrl, PropData prop, object arg)
		{
			if (itemCtrl == null || prop == null)
			{
				return;
			}
			HLog.LogError("未完成：打开 物品信息 界面");
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = prop,
				m_openDataType = ItemInfoOpenDataType.eShow,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, default(Vector3), 0f);
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshCardData(this.m_heroDataModule.MainCardData);
			this.UpdateEquipView();
			this.SetAttributeDatas(this.m_cardData);
			this.SetEquipTitleData();
			this.RefreshEquipDatas();
			this.m_scrollView.SetListItemCount(this.m_nodeDatas.Count, true);
			this.m_scrollView.RefreshAllShownItem();
		}

		private void OnEvent_MountChanged(object sender, int type, BaseEventArgs eventargs)
		{
			this.FreshSkin();
			this.Skin_ModelItem.MountChanged();
		}

		private void OnEvent_SelfClothesChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.FreshSkin();
		}

		private void OnEventUserInfoChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.SetPlayerName();
			this.FreshTitle();
		}

		private void OnEventTalentLegacySelectCareer(object sender, int type, BaseEventArgs eventargs)
		{
			this.SetTalentLegacyCareer();
		}

		private void OnRedPointLegacyCareerSkillChange(RedNodeListenData redData)
		{
			if (this.m_redNodeCareer != null)
			{
				this.m_redNodeCareer.Value = redData.m_count;
			}
		}

		private void OnRedPointComposeChange(RedNodeListenData redData)
		{
			if (this.m_redNodeCompose != null)
			{
				this.m_redNodeCompose.Value = redData.m_count;
			}
		}

		private void OnRedPointPetsChange(RedNodeListenData redData)
		{
			if (this.m_redNodePets != null)
			{
				this.m_redNodePets.Value = redData.m_count;
			}
		}

		private void OnRedPointPictorialChange(RedNodeListenData redData)
		{
			if (this.m_redNodePictorial != null)
			{
				this.m_redNodePictorial.Value = redData.m_count;
			}
		}

		private void OnRedPointCollectionChange(RedNodeListenData redData)
		{
			if (this.m_redNodeCollection != null)
			{
				this.m_redNodeCollection.Value = redData.m_count;
			}
		}

		private void OnRedPointMountChange(RedNodeListenData redData)
		{
			if (this.m_redNodeMount != null)
			{
				this.m_redNodeMount.Value = redData.m_count;
			}
		}

		private void OnRedPointArtifactChange(RedNodeListenData redData)
		{
			if (this.m_redNodeArtifact != null)
			{
				this.m_redNodeArtifact.Value = redData.m_count;
			}
		}

		private void OnRedPointFashionChange(RedNodeListenData redData)
		{
			if (this.m_redNodeFashion != null)
			{
				this.m_redNodeFashion.Value = redData.m_count;
			}
		}

		[GameTestMethod("主角", "打印主角属性", "", 0)]
		private static void PrintRoleAttribute()
		{
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			AddAttributeDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			CardData cardData = new CardData();
			cardData.CloneFrom(dataModule.MainCardData);
			cardData.UpdateAttribute(dataModule2.AttributeDatas);
			HLog.LogError(string.Format("attack:{0} ", cardData.m_memberAttributeData.GetAttack4UI()) + string.Format("hp:{0} ", cardData.m_memberAttributeData.GetHpMax4UI()) + string.Format("defense:{0} ", cardData.m_memberAttributeData.GetDefence4UI()) + string.Format("combat:{0}", dataModule2.Combat));
		}

		[ContextMenu("FreshSkin")]
		private void FreshSkin()
		{
			if (!this.Skin_ModelItem.IsCameraShow)
			{
				return;
			}
			int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
			this.Skin_ModelItem.rectTransform.anchoredPosition = ((mountMemberId > 0) ? this.Skin_ModelItemPos_Mount : this.Skin_ModelItemPos_Normal);
			this.Skin_ModelItem.rectTransform.localScale = ((mountMemberId > 0) ? this.Skin_ModelItemScale_Mount : this.Skin_ModelItemScale_Normal) * Vector3.one;
			this.Skin_ModelItem.OnShow();
			if (!this.Skin_ModelItem.RefreshPlayerSkins(null))
			{
				this.Skin_ModelItem.ShowSelfPlayerModel("UIMainMainEquipMainEquip_ModelNodeCtrl", true);
			}
		}

		private void FreshTitle()
		{
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.SetAndFreshToMy();
			}
		}

		[SerializeField]
		private Animator m_aniamtor;

		[Header("玩家信息")]
		public CustomText Text_Name;

		public CommonCombatItem CombatItem;

		public CustomButton Button_Career;

		public CustomImage Image_Career;

		[Header("称号")]
		public UITitleCtrl TitleCtrl;

		[Header("皮肤")]
		public UIModelItem Skin_ModelItem;

		public Vector2 Skin_ModelItemPos_Normal = new Vector2(0f, -160f);

		public Vector2 Skin_ModelItemPos_Mount = new Vector2(-20f, -200f);

		public float Skin_ModelItemScale_Normal = 2f;

		public float Skin_ModelItemScale_Mount = 1.5f;

		[Header("装备")]
		[SerializeField]
		private GameObject m_equipUpPrefab;

		[SerializeField]
		private List<UIEquipUpItem> m_equipUpItemList;

		[Header("英雄信息")]
		[SerializeField]
		private CustomText m_hpTxt;

		[SerializeField]
		private CustomText m_attackTxt;

		[SerializeField]
		private CustomText m_defenseTxt;

		[Header("我的装备Title")]
		[SerializeField]
		private CustomButton m_sortBt;

		[SerializeField]
		private CustomText m_sortBtTxt;

		[SerializeField]
		private CustomButton m_mergeBt;

		[SerializeField]
		private LoopListView2 m_scrollView;

		[SerializeField]
		private CustomButton m_btnPictorial;

		[SerializeField]
		private CustomButton m_btnPets;

		[SerializeField]
		private CustomButton m_btnCollection;

		[SerializeField]
		private CustomButton m_btnMount;

		[SerializeField]
		private CustomButton m_btnArtifact;

		[SerializeField]
		private CustomButton m_btnFashion;

		[SerializeField]
		private CustomButton m_BtnAttrDetail;

		[Header("红点")]
		[SerializeField]
		private RedNodeOneCtrl m_redNodeCompose;

		[SerializeField]
		private RedNodeOneCtrl m_redNodePets;

		[SerializeField]
		private RedNodeOneCtrl m_redNodePictorial;

		[SerializeField]
		private RedNodeOneCtrl m_redNodeCollection;

		[SerializeField]
		private RedNodeOneCtrl m_redNodeMount;

		[SerializeField]
		private RedNodeOneCtrl m_redNodeArtifact;

		[SerializeField]
		private RedNodeOneCtrl m_redNodeFashion;

		[SerializeField]
		private RedNodeOneCtrl m_redNodeCareer;

		private EquipSortType m_sortType;

		private CardData m_cardData;

		private long m_currentHp;

		private long m_currentAttack;

		private long m_currentDefense;

		public Dictionary<int, UIHeroEquipItem> m_equipItems = new Dictionary<int, UIHeroEquipItem>();

		private int m_columnCount = 5;

		public List<EquipData> m_equipDatas = new List<EquipData>();

		public Dictionary<int, CustomBehaviour> m_nodeDic = new Dictionary<int, CustomBehaviour>();

		public List<UIMainMainEquipMainEquip.NodeData> m_nodeDatas = new List<UIMainMainEquipMainEquip.NodeData>();

		private HeroDataModule m_heroDataModule;

		private PropDataModule m_propDataModule;

		private AddAttributeDataModule m_addAttributeDataModule;

		private EquipDataModule m_equipDataModule;

		private FunctionDataModule m_functionDataModule;

		private const int SingleAnimLine = 4;

		private const float AnimDelayPerItem = 0.02f;

		private const float AnimDuring = 0.2f;

		private float _curTime;

		public class NodeData
		{
			public int m_index;

			public bool m_isTopSpace;

			public bool m_isBottomSpace;

			public List<UIHeroEquipItemGroup.EquipDataItem> m_equipDatas;

			public List<PropData> m_propDatas;

			public string m_titleName = string.Empty;
		}
	}
}
