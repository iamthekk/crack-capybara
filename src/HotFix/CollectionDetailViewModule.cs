using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Collection;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CollectionDetailViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.collectionDataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.btnMerge.gameObject.SetActive(false);
			this.passiveAttributeItemPrefab.gameObject.SetActive(false);
			for (int i = 0; i < this.costItems.Count; i++)
			{
				this.costItems[i].Init();
			}
			for (int j = 0; j < 4; j++)
			{
				CollectionAttributeItem collectionAttributeItem = Object.Instantiate<CollectionAttributeItem>(this.passiveAttributeItemPrefab, this.passiveAttributeItemPrefab.transform.parent, false);
				this.passiveItemList.Add(collectionAttributeItem);
				collectionAttributeItem.Init();
				collectionAttributeItem.gameObject.SetActive(false);
			}
			this.txtFullLevel.gameObject.SetActive(false);
			this.txtLevelUpTip.gameObject.SetActive(false);
		}

		private void OnRefresh(object obj, int type, BaseEventArgs args)
		{
			this.UpdateView();
		}

		public override void OnOpen(object data)
		{
			this.data = data as CollectionData;
			if (this.data == null)
			{
				return;
			}
			this.UpdateView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			for (int i = 0; i < this.costItems.Count; i++)
			{
				this.costItems[i].DeInit();
			}
			GameApp.Event.DispatchNow(this, 390, null);
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			if (this.isRegisterEvent)
			{
				return;
			}
			this.isRegisterEvent = true;
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionMerge, new HandlerEvent(this.OnEventCollectionMerge));
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, new HandlerEvent(this.OnEventCollectionLevelUp));
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, new HandlerEvent(this.OnEventCollectionStarUpgrade));
			manager.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnRefresh));
			this.btnBg.m_onClick = new Action(this.OnBtnCloseClick);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnSuit.m_onClick = new Action(this.OnBtnSuitClick);
			this.btnMerge.m_onClick = new Action(this.OnBtnMergeClick);
			this.btnStarUpgrade.m_onClick = new Action(this.OnBtnStarUpgradeClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			if (!this.isRegisterEvent)
			{
				return;
			}
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionMerge, new HandlerEvent(this.OnEventCollectionMerge));
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, new HandlerEvent(this.OnEventCollectionLevelUp));
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, new HandlerEvent(this.OnEventCollectionStarUpgrade));
			manager.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnRefresh));
			this.btnBg.m_onClick = null;
			this.uiPopCommon.OnClick = null;
			this.btnSuit.m_onClick = null;
			this.btnMerge.m_onClick = null;
			this.btnStarUpgrade.m_onClick = null;
		}

		private void OnEventCollectionMerge(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateView();
		}

		private void OnEventCollectionLevelUp(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateView();
		}

		private void OnEventCollectionStarUpgrade(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateView();
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			this.OnBtnCloseClick();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.CollectionDetailViewModule, null);
		}

		private void OnBtnSuitClick()
		{
			CollectionSuitData collectionSuitData = this.collectionDataModule.GetCollectionSuitData(this.data.suitId);
			if (collectionSuitData == null)
			{
				HLog.LogError(string.Format("suitId:{0} can't find in collection suit table, please check", this.data.suitId));
				return;
			}
			GameApp.View.OpenView(ViewName.CollectionSuitShowViewModule, collectionSuitData, 1, null, null);
		}

		private void UpdateView()
		{
			this.costNode.gameObject.SetActive(this.data.collectionType == 1U);
			this.UpdateBasicInfo();
			this.UpdateBasicAttribute();
			this.UpdatePassiveAttribute();
			this.UpdateCost();
		}

		private void UpdateBasicInfo()
		{
			Quality_collectionQuality elementById = GameApp.Table.GetManager().GetQuality_collectionQualityModelInstance().GetElementById(this.data.quality);
			this.imgCardBg.SetImage(elementById.atlasId, elementById.cardBg);
			this.imgIcon.SetImage(this.data.atlasId, this.data.iconName);
			this.starNode.SetStar(this.data.collectionStar);
			this.sliderFragment.gameObject.SetActive(this.data.collectionType == 2U);
			this.txtSlider.text = string.Format("{0}/{1}", this.data.fragMentCount, this.data.MergeNeedFragment);
			this.sliderFragment.maxValue = (float)this.data.MergeNeedFragment;
			this.sliderFragment.value = (float)this.data.fragMentCount;
			Collection_collection elementById2 = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById(this.data.itemId);
			this.txtDesc.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.descId), true);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.data.nameId);
			this.txtTitle.text = string.Concat(new string[] { "<color=", elementById.colorName, ">", infoByID, "</color>" });
			this.txtRarity.text = CollectionHelper.GetRarityName(this.data.rarity);
			int num;
			int num2;
			this.collectionDataModule.GetCollectionSuitData(this.data.suitId).GetSuitProgress(out num, out num2);
			this.txtSuitProgress.text = string.Format("{0}/{1}", num, num2);
		}

		private void UpdateBasicAttribute()
		{
			Dictionary<string, FP> basicAttributes = this.data.GetBasicAttributes(this.data.collectionStar);
			Dictionary<string, FP> basicAttributes2 = this.data.GetBasicAttributes(this.data.collectionStar + 1);
			int num = 0;
			foreach (KeyValuePair<string, FP> keyValuePair in basicAttributes)
			{
				string attributeFormatStr = this.GetAttributeFormatStr(keyValuePair.Key, keyValuePair.Value);
				string text = "";
				FP fp;
				if (basicAttributes2.TryGetValue(keyValuePair.Key, out fp))
				{
					FP fp2 = fp - keyValuePair.Value;
					if (fp2 > 0)
					{
						string text2 = "";
						if (keyValuePair.Key.Contains("%"))
						{
							text2 = "%";
						}
						text = string.Format(" <color=#329133>(+{0}{1})</color>", fp2, text2);
					}
				}
				this.basicAttributeItemList[num].SetData(keyValuePair.Key, attributeFormatStr, text);
				num++;
			}
			for (int i = num; i < this.basicAttributeItemList.Count; i++)
			{
				this.basicAttributeItemList[i].SetData("", "", "");
			}
		}

		private void UpdatePassiveAttribute()
		{
			Dictionary<string, FP> passiveAttributes = this.data.GetPassiveAttributes(this.data.collectionStar);
			CollectionConditionChecker conditionChecker = this.data.conditionChecker;
			if (this.data.conditionChecker.conditionId == 1)
			{
				this.passiveItemList[1].gameObject.SetActive(true);
				this.passiveItemList[1].txtAttribute.text = this.GetAttributeFormatStr(conditionChecker.conditionJson.Attribute, conditionChecker.conditionJson.Value);
				return;
			}
			this.passiveItemList[0].gameObject.SetActive(true);
			string text = string.Format(" ({0}/{1})", conditionChecker.triggerCurValue, conditionChecker.conditionJson.Cost);
			if (conditionChecker.triggerCount < conditionChecker.conditionJson.Limit)
			{
				this.passiveItemList[0].txtAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID(string.Format("ui_collection_passive_condition_{0}", conditionChecker.conditionId), new object[] { conditionChecker.conditionJson.Cost }) + text;
			}
			else
			{
				this.passiveItemList[0].txtAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID(string.Format("ui_collection_passive_condition_{0}", conditionChecker.conditionId), new object[] { conditionChecker.conditionJson.Cost });
			}
			this.passiveItemList[1].gameObject.SetActive(true);
			string text2 = this.GetAttributeFormatStr(conditionChecker.conditionJson.Attribute, conditionChecker.conditionJson.Value);
			string text3 = "";
			if (!this.data.IsFullStar)
			{
				int starId = this.data.GetStarId(this.data.collectionStar);
				Collection_collectionStar elementById = GameApp.Table.GetManager().GetCollection_collectionStarModelInstance().GetElementById(starId);
				string text4 = "";
				string text5 = "";
				FP fp = 0;
				FP fp2 = 0;
				if (elementById != null)
				{
					elementById.effectAttributeEx.GetAttributeKV(out text4, out fp);
				}
				int starId2 = this.data.GetStarId(this.data.collectionStar + 1);
				Collection_collectionStar elementById2 = GameApp.Table.GetManager().GetCollection_collectionStarModelInstance().GetElementById(starId2);
				if (elementById2 != null)
				{
					elementById2.effectAttributeEx.GetAttributeKV(out text5, out fp2);
				}
				if (fp2 - fp > 0)
				{
					string text6 = "";
					if (text5.Contains("%"))
					{
						text6 = "%";
					}
					text3 = string.Format("<color=#329133>(+{0}{1})</color>", fp2 - fp, text6);
				}
			}
			text2 += text3;
			this.passiveItemList[1].SetData(conditionChecker.conditionJson.Attribute, text2, "");
			this.passiveItemList[2].gameObject.SetActive(true);
			this.passiveItemList[2].txtAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_collection_passive_condition_limit", new object[]
			{
				conditionChecker.triggerCount,
				conditionChecker.conditionJson.Limit
			});
			this.passiveItemList[3].gameObject.SetActive(true);
			FP fp4;
			FP fp3 = (passiveAttributes.TryGetValue(conditionChecker.conditionJson.Attribute, out fp4) ? fp4 : 0);
			string attributeFormatStr = this.GetAttributeFormatStr(conditionChecker.conditionJson.Attribute, fp3);
			this.passiveItemList[3].txtAttribute.text = "<color=#F4863C>" + Singleton<LanguageManager>.Instance.GetInfoByID("ui_collection_passive_total", new object[] { attributeFormatStr }) + "</color>";
		}

		private string GetAttributeFormatStr(string key, FP value)
		{
			string text = "ui_collection_passive_attribute_add";
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(key);
			if (elementById == null)
			{
				HLog.LogError("attribute key:" + key + " can't find in attribute table, please check");
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
			string text2;
			if (key.Contains("%"))
			{
				text2 = string.Format("+{0}%", Utility.Math.Round(value.AsDouble(), 1));
			}
			else
			{
				text2 = "+" + DxxTools.FormatNumber(Utility.Math.RoundToLong(value.AsDouble()));
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID(text, new object[] { infoByID, text2 });
		}

		private void UpdateCost()
		{
			this.UpdateStarUpgradeCost();
		}

		private void UpdateStarUpgradeCost()
		{
			this.btnStarUpgrade.gameObject.SetActive(!this.data.IsFullStar);
			this.costItemsNode.SetActive(!this.data.IsFullStar);
			this.txtFullLevel.gameObject.SetActive(this.data.IsFullStar);
			this.txtFullLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_collection_full_star");
			this.txtLevelUpTip.gameObject.SetActive(false);
			this.txtTalentLimit.gameObject.SetActive(!this.data.IsFullStar);
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			if (((dataModule != null) ? dataModule.TalentStage : 0) >= this.data.ConditionTalentId)
			{
				this.txtTalentLimit.text = "";
			}
			else
			{
				TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.data.ConditionTalentId);
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
				this.txtTalentLimit.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_collection_starupgrade_limit", new object[] { infoByID });
			}
			for (int i = 0; i < this.costItems.Count; i++)
			{
				if (!this.data.IsFullStar && i < this.data.StarUpgradeItemCost.Count)
				{
					this.costItems[i].gameObject.SetActive(true);
					this.propDataModule.GetItemDataCountByid((ulong)((long)this.data.StarUpgradeItemCost[i].ID));
					this.costItems[i].SetData(this.data.StarUpgradeItemCost[i]);
				}
				else
				{
					this.costItems[i].gameObject.SetActive(false);
				}
			}
		}

		private void OnBtnMergeClick()
		{
			if (this.data == null)
			{
				return;
			}
			if (!this.data.IsCanMerge)
			{
				return;
			}
			NetworkUtils.Collection.CollectionMergeRequest((uint)this.data.fragmentRowId);
		}

		private void OnBtnStarUpgradeClick()
		{
			if (this.data == null)
			{
				return;
			}
			if (this.data.IsFullStar)
			{
				return;
			}
			if (this.data.collectionType != 1U)
			{
				return;
			}
			if (this.data.StarUpgradeItemCost.Count <= 0)
			{
				return;
			}
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			if (((dataModule != null) ? dataModule.TalentStage : 0) < this.data.ConditionTalentId)
			{
				TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.data.ConditionTalentId);
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("ui_collection_starupgrade_limit", new object[] { infoByID }));
				return;
			}
			for (int i = 0; i < this.data.StarUpgradeItemCost.Count; i++)
			{
				long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)this.data.StarUpgradeItemCost[i].ID));
				long totalCount = this.data.StarUpgradeItemCost[i].TotalCount;
				if (itemDataCountByid < totalCount)
				{
					GameApp.View.ShowItemNotEnoughTip(this.data.StarUpgradeItemCost[i].ID, true);
					return;
				}
			}
			int fromStar = this.data.collectionStar;
			int toStar = this.data.collectionStar + 1;
			int oldTriggerCount = this.data.conditionChecker.triggerCount;
			FP oldTotalValue = oldTriggerCount * this.data.conditionChecker.conditionJson.Value;
			NetworkUtils.Collection.CollectionStarUpgradeRequest((uint)this.data.rowId, delegate(bool isOk, CollecStarResponse resp)
			{
				if (isOk)
				{
					if (this != null && this.isActiveAndEnabled)
					{
						GameApp.Sound.PlayClip(630, 1f);
						Dictionary<string, FP> basicAttributes = this.data.GetBasicAttributes(fromStar);
						foreach (KeyValuePair<string, FP> keyValuePair in this.data.GetBasicAttributes(toStar))
						{
							FP fp;
							basicAttributes.TryGetValue(keyValuePair.Key, out fp);
							if (keyValuePair.Value - fp > 0)
							{
								for (int j = 0; j < this.basicAttributeItemList.Count; j++)
								{
									if (this.basicAttributeItemList[j].attributeKey == keyValuePair.Key)
									{
										this.basicAttributeItemList[j].PlayEffect();
									}
								}
							}
						}
						Dictionary<string, FP> passiveAttributes = this.data.GetPassiveAttributes(fromStar);
						foreach (KeyValuePair<string, FP> keyValuePair2 in this.data.GetPassiveAttributes(toStar))
						{
							FP fp2;
							passiveAttributes.TryGetValue(keyValuePair2.Key, out fp2);
							if (keyValuePair2.Value - fp2 > 0 && this.passiveItemList[1].attributeKey == keyValuePair2.Key)
							{
								this.passiveItemList[1].PlayEffect();
								break;
							}
						}
						int triggerCount = this.data.conditionChecker.triggerCount;
						if (triggerCount - oldTriggerCount > 0)
						{
							this.passiveItemList[2].PlayEffect();
						}
						if (triggerCount * this.data.conditionChecker.conditionJson.Value - oldTotalValue > 0)
						{
							this.passiveItemList[3].PlayEffect();
						}
					}
					GameApp.SDK.Analyze.Track_CollectionUpgrade(this.data.itemId);
				}
			});
		}

		public UIPopCommon uiPopCommon;

		public CustomButton btnBg;

		public GameObject costNode;

		public CustomImage imgCardBg;

		public CustomImage imgIcon;

		public CollectionStarNode starNode;

		public Slider sliderFragment;

		public CustomText txtSlider;

		public CustomTextScrollView txtDesc;

		public CustomText txtTitle;

		public CustomText txtRarity;

		public CustomText txtSuitProgress;

		public List<CollectionAttributeItem> basicAttributeItemList;

		public CollectionAttributeItem passiveAttributeItemPrefab;

		public List<CostItem> costItems;

		public CustomButton btnMerge;

		public CustomButton btnStarUpgrade;

		public CustomButton btnSuit;

		public CustomText txtTalentLimit;

		public CustomText txtFullLevel;

		public CustomText txtLevelUpTip;

		public GameObject costItemsNode;

		private CollectionDataModule collectionDataModule;

		private PropDataModule propDataModule;

		private CollectionData data;

		private bool isRegisterEvent;

		private List<CollectionAttributeItem> passiveItemList = new List<CollectionAttributeItem>();
	}
}
