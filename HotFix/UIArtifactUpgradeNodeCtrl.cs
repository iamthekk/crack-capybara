using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Artifact;
using Proto.Common;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIArtifactUpgradeNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.artifactDataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.uiItem.Init();
			this.currencyBtnUpgradeOne.Init();
			this.currencyBtnUpgradeAuto.Init();
			this.copyAttributeItem.SetActiveSafe(false);
			this.copyStarItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
			this.currencyBtnUpgradeOne.DeInit();
			this.currencyBtnUpgradeAuto.DeInit();
			for (int i = 0; i < this.attributeItems.Count; i++)
			{
				this.attributeItems[i].DeInit();
			}
			this.attributeItems.Clear();
			for (int j = 0; j < this.starItems.Count; j++)
			{
				this.starItems[j].DeInit();
			}
			this.starItems.Clear();
		}

		public void Refresh()
		{
			this.currentData = this.artifactDataModule.GetCurrentBasicData();
			this.nextData = this.artifactDataModule.GetNextBasicData();
			if (this.currentData == null || this.nextData == null)
			{
				return;
			}
			this.currentLevelInfo = this.artifactDataModule.GetCurrentLevelInfo();
			this.nextLevelInfo = this.artifactDataModule.GetNextLevelInfo();
			this.RefreshStageInfo();
			this.RefreshLevelInfo();
		}

		private void RefreshStageInfo()
		{
			PropData propData = new PropData();
			propData.id = (uint)this.currentData.StageConfig.itemId;
			this.uiItem.SetData(propData);
			this.uiItem.OnRefresh();
			this.uiItem.SetCountText("");
			this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage", new object[] { this.currentData.Stage });
			if (this.currentData.StageAttributeData != null && this.nextData.StageAttributeData != null)
			{
				Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.currentData.StageAttributeData.Header);
				string text = "";
				if (this.currentData.StageAttributeData.Header.Contains("%"))
				{
					text = "%";
				}
				if (elementById != null)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
					this.textCurrentAttr.text = string.Format("{0} {1}{2}", infoByID, this.currentData.StageAttributeData.Value, text);
				}
				Attribute_AttrText elementById2 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.nextData.StageAttributeData.Header);
				if (elementById2 != null)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.LanguageId);
					this.textNextAttr.text = string.Format("{0} {1}{2}", infoByID2, this.nextData.StageAttributeData.Value, text);
				}
			}
		}

		private void RefreshLevelInfo()
		{
			ArtifactInfo artifactInfo = this.artifactDataModule.ArtifactInfo;
			int basicArtifactMaxStage = this.artifactDataModule.GetBasicArtifactMaxStage();
			int num = this.currentLevelInfo.levelCost - (int)this.artifactDataModule.ArtifactInfo.Exp;
			if ((ulong)artifactInfo.Stage == (ulong)((long)basicArtifactMaxStage))
			{
				this.textNextStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_max");
				this.textNextAttr.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_max");
			}
			else
			{
				this.textNextStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_unlock", new object[] { artifactInfo.Stage + 1U });
			}
			bool flag = this.currentLevelInfo.stage == this.nextLevelInfo.stage && this.currentLevelInfo.star == this.nextLevelInfo.star;
			this.textLevelTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_star");
			this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("");
			this.textCurrentLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_star_text", new object[]
			{
				this.currentLevelInfo.stage,
				this.currentLevelInfo.star
			});
			if (flag)
			{
				this.maxObj.SetActive(true);
				this.textNextLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_max");
				this.currencyBtnUpgradeOne.gameObject.SetActiveSafe(false);
				this.currencyBtnUpgradeAuto.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.maxObj.SetActive(false);
				this.textNextLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_star_text", new object[]
				{
					this.nextLevelInfo.stage,
					this.nextLevelInfo.star
				});
				this.currencyBtnUpgradeOne.gameObject.SetActiveSafe(true);
				this.currencyBtnUpgradeAuto.gameObject.SetActiveSafe(true);
				this.currencyBtnUpgradeOne.SetData(this.currentLevelInfo.itemCost, 1, new Action(this.OnClickUpgradeOnce), true, true);
				this.currencyBtnUpgradeAuto.SetData(this.currentLevelInfo.itemCost, num, new Action(this.OnClickUpgradeAuto), true, true);
				long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)this.currentLevelInfo.itemCost));
				this.redNodeAuto.gameObject.SetActive(itemDataCountByid >= (long)num);
			}
			this.RefreshLevelAttribute(flag);
			this.RefreshExp(flag);
		}

		private void RefreshLevelAttribute(bool isMax)
		{
			List<MergeAttributeData> mergeAttributeData = this.currentLevelInfo.attribute.GetMergeAttributeData();
			List<MergeAttributeData> mergeAttributeData2 = this.nextLevelInfo.attribute.GetMergeAttributeData();
			if (mergeAttributeData.Count != mergeAttributeData2.Count)
			{
				HLog.LogError(string.Format("[Artifact_artifactLevel] attribute error, curId={0} nextId={1}", this.currentLevelInfo.id, this.nextLevelInfo.id));
				return;
			}
			for (int i = 0; i < mergeAttributeData2.Count; i++)
			{
				string header = mergeAttributeData2[i].Header;
				long value = mergeAttributeData2[i].Value.GetValue();
				long value2 = mergeAttributeData[i].Value.GetValue();
				UIAttributeItem uiattributeItem;
				if (i < this.attributeItems.Count)
				{
					uiattributeItem = this.attributeItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyAttributeItem);
					gameObject.SetParentNormal(this.attributeParent, false);
					gameObject.SetActiveSafe(true);
					uiattributeItem = gameObject.GetComponent<UIAttributeItem>();
					uiattributeItem.Init();
					this.attributeItems.Add(uiattributeItem);
				}
				Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(header);
				if (elementById == null)
				{
					HLog.LogError("Table [Attribute_AttrText] not found id =" + mergeAttributeData2[i].Header);
				}
				else
				{
					uiattributeItem.SetData(elementById, value2, value, isMax);
				}
			}
		}

		private void RefreshExp(bool isAllMax)
		{
			if (isAllMax)
			{
				this.textExp.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_max");
				this.sliderExp.value = 1f;
			}
			else
			{
				this.textExp.text = string.Format("{0}/{1}", this.artifactDataModule.ArtifactInfo.Exp, this.currentLevelInfo.levelCost);
				float num = Utility.Math.Clamp01(this.artifactDataModule.ArtifactInfo.Exp / (float)this.currentLevelInfo.levelCost);
				this.sliderExp.value = num;
			}
			int maxLevel = this.artifactDataModule.GetMaxLevel();
			for (int i = 0; i < maxLevel; i++)
			{
				UIStarItem uistarItem;
				if (i < this.starItems.Count)
				{
					uistarItem = this.starItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyStarItem);
					gameObject.SetParentNormal(this.starLayout, false);
					gameObject.SetActiveSafe(true);
					uistarItem = gameObject.GetComponent<UIStarItem>();
					this.starItems.Add(uistarItem);
					uistarItem.Init();
				}
				bool flag = (long)i < (long)((ulong)this.artifactDataModule.ArtifactInfo.Level);
				if (isAllMax)
				{
					flag = true;
				}
				else if ((ulong)this.artifactDataModule.ArtifactInfo.Level == (ulong)((long)maxLevel))
				{
					flag = false;
				}
				uistarItem.SetData(flag);
			}
		}

		private void OnClickUpgradeOnce()
		{
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)this.currentLevelInfo.itemCost)) > 0L)
			{
				uint oldStage = this.artifactDataModule.ArtifactInfo.Stage;
				uint oldLevel = this.artifactDataModule.ArtifactInfo.Level;
				NetworkUtils.Artifact.DoArtifactUpgradeRequest(delegate(bool result, ArtifactUpgradeResponse response)
				{
					if (this.artifactDataModule.ArtifactInfo.Level > oldLevel)
					{
						GameApp.Sound.PlayClip(658, 1f);
						for (int i = 0; i < this.attributeItems.Count; i++)
						{
							this.attributeItems[i].ShowAni();
						}
					}
					else if (this.artifactDataModule.ArtifactInfo.Stage > oldStage)
					{
						GameApp.Sound.PlayClip(658, 1f);
						this.levelUpAni.Play("Show");
						for (int j = 0; j < this.attributeItems.Count; j++)
						{
							this.attributeItems[j].ShowAni();
						}
					}
					else
					{
						GameApp.Sound.PlayClip(657, 1f);
					}
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIArtifact_BasicUpgrade, null);
				});
				return;
			}
			this.ShowNotEnoughTip(this.currentLevelInfo.itemCost);
		}

		private void OnClickUpgradeAuto()
		{
			int num = this.currentLevelInfo.levelCost - (int)this.artifactDataModule.ArtifactInfo.Exp;
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)this.currentLevelInfo.itemCost)) >= (long)num)
			{
				uint oldStage = this.artifactDataModule.ArtifactInfo.Stage;
				uint oldLevel = this.artifactDataModule.ArtifactInfo.Level;
				NetworkUtils.Artifact.DoArtifactUpgradeAllRequest(delegate(bool result, ArtifactUpgradeAllResponse response)
				{
					bool flag = false;
					if (this.artifactDataModule.ArtifactInfo.Stage > oldStage)
					{
						flag = true;
						GameApp.Sound.PlayClip(658, 1f);
						this.levelUpAni.Play("Show");
					}
					if (this.artifactDataModule.ArtifactInfo.Level > oldLevel)
					{
						flag = true;
						GameApp.Sound.PlayClip(658, 1f);
					}
					if (!flag)
					{
						GameApp.Sound.PlayClip(657, 1f);
					}
					for (int i = 0; i < this.attributeItems.Count; i++)
					{
						this.attributeItems[i].ShowAni();
					}
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIArtifact_BasicUpgrade, null);
				});
				return;
			}
			this.ShowNotEnoughTip(this.currentLevelInfo.itemCost);
		}

		private void ShowNotEnoughTip(int itemId)
		{
			GameApp.View.ShowItemNotEnoughTip(itemId, true);
		}

		[Header("基础")]
		public UIItem uiItem;

		public CustomText textName;

		public CustomText textNextStage;

		public CustomText textCurrentAttr;

		public CustomText textNextAttr;

		public Animator levelUpAni;

		[Header("升级")]
		public CustomText textLevelTitle;

		public CustomText textCurrentLevel;

		public CustomText textNextLevel;

		public GameObject attributeParent;

		public GameObject copyAttributeItem;

		public Slider sliderExp;

		public CustomText textExp;

		public CustomText textTip;

		public GameObject starLayout;

		public GameObject copyStarItem;

		public GameObject maxObj;

		[Header("按钮")]
		public UICurrencyButton currencyBtnUpgradeOne;

		public UICurrencyButton currencyBtnUpgradeAuto;

		public RedNodeOneCtrl redNodeAuto;

		private ArtifactDataModule artifactDataModule;

		private PropDataModule propDataModule;

		private ArtifactBasicData currentData;

		private ArtifactBasicData nextData;

		private Artifact_artifactLevel currentLevelInfo;

		private Artifact_artifactLevel nextLevelInfo;

		private List<UIAttributeItem> attributeItems = new List<UIAttributeItem>();

		private List<UIStarItem> starItems = new List<UIStarItem>();
	}
}
