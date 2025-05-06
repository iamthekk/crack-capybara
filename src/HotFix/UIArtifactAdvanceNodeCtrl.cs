using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Artifact;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIArtifactAdvanceNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.artifactDataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.uiSkillItem.Init();
			this.uiCostItem.Init();
			this.buttonPreview.onClick.AddListener(new UnityAction(this.OnClickPreview));
			this.buttonEquipSkill.onClick.AddListener(new UnityAction(this.OnClickEquipSkill));
			this.buttonUpgrade.onClick.AddListener(new UnityAction(this.OnClickUpgrade));
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			this.uiSkillItem.DeInit();
			this.uiCostItem.DeInit();
			this.buttonPreview.onClick.RemoveListener(new UnityAction(this.OnClickPreview));
			this.buttonEquipSkill.onClick.RemoveListener(new UnityAction(this.OnClickEquipSkill));
			this.buttonUpgrade.onClick.RemoveListener(new UnityAction(this.OnClickUpgrade));
		}

		public void OnShow()
		{
			this.artifactList = this.artifactDataModule.GetAdvanceDataList();
			this.loopListView.SetListItemCount(this.artifactList.Count + 1, true);
			this.loopListView.RefreshAllShowItems();
		}

		public void OnHide()
		{
			this.artifactList = new List<ArtifactAdvanceData>();
			this.loopListView.SetListItemCount(this.artifactList.Count + 1, true);
			this.loopListView.RefreshAllShowItems();
		}

		public void SetData(ArtifactAdvanceData data)
		{
			this.artifactAdvanceData = data;
			this.skill = data.GetSkill();
			this.Refresh();
		}

		private void Refresh()
		{
			this.redPointUpgrade.gameObject.SetActive(this.artifactAdvanceData.IsRedPoint());
			this.buttonPreview.gameObject.SetActiveSafe(this.artifactAdvanceData.IsUnlock && !this.artifactAdvanceData.IsMaxStar);
			this.buttonEquipSkill.gameObject.SetActiveSafe(this.artifactAdvanceData.IsUnlock);
			if (this.skill != null)
			{
				this.uiSkillItem.SetData(this.skill, this.artifactAdvanceData.Config.quality);
				this.textSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.skill.nameID);
				this.textSkillDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.skill.fullDetailID);
			}
			this.textSkillTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_skill_title");
			this.textAttrTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_attribute_title");
			long currentAttributeValue = this.artifactAdvanceData.GetCurrentAttributeValue();
			long nextAttributeValue = this.artifactAdvanceData.GetNextAttributeValue();
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.artifactAdvanceData.InitAttributeData.Header);
			if (elementById != null)
			{
				this.textAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
				this.textCurrentAttr.text = (elementById.ID.Contains("%") ? string.Format("{0}%", currentAttributeValue) : currentAttributeValue.ToString());
				this.textNextAttr.text = (elementById.ID.Contains("%") ? string.Format("{0}%", nextAttributeValue) : nextAttributeValue.ToString());
			}
			else
			{
				HLog.LogError("Table [Attribute_AttrText] not found id=" + this.artifactAdvanceData.InitAttributeData.Header);
			}
			if (this.artifactAdvanceData.IsMaxStar)
			{
				this.textNextAttr.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_stage_max");
			}
			this.maxObj.SetActive(this.artifactAdvanceData.IsMaxStar);
			this.uiCostItem.SetActive(true);
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (this.artifactAdvanceData.IsUnlock)
			{
				this.textUpgrade.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_upgrade");
				this.buttonUpgrade.gameObject.SetActiveSafe(!this.artifactAdvanceData.IsMaxStar);
				if (this.artifactAdvanceData.IsMaxStar)
				{
					this.uiCostItem.SetActive(false);
				}
				else
				{
					PropData nextStarCost = this.artifactAdvanceData.GetNextStarCost();
					if (nextStarCost != null)
					{
						long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)nextStarCost.id);
						this.uiCostItem.SetData((int)nextStarCost.id, itemDataCountByid, (long)nextStarCost.count);
					}
					else
					{
						HLog.LogError(string.Format("未配置{0}星升级道具", this.artifactAdvanceData.Star + 1));
					}
				}
			}
			else
			{
				this.textUpgrade.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_unlock");
				int unlockCostId = this.artifactAdvanceData.Config.unlockCostId;
				long itemDataCountByid2 = dataModule.GetItemDataCountByid((ulong)((long)unlockCostId));
				this.buttonUpgrade.gameObject.SetActiveSafe(itemDataCountByid2 > 0L);
				this.uiCostItem.SetData(unlockCostId, itemDataCountByid2, 1L);
			}
			if ((ulong)this.artifactDataModule.ArtifactInfo.SkillArtifactId == (ulong)((long)this.artifactAdvanceData.ID))
			{
				this.textEquipSkill.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_skill_cancel");
			}
			else
			{
				this.textEquipSkill.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_skill_use");
			}
			int num = this.artifactList.IndexOf(this.artifactAdvanceData);
			foreach (UIShowItem uishowItem in this.dic.Values)
			{
				int index = uishowItem.GetIndex();
				uishowItem.Refresh(num, this.artifactList[index].IsUnlock, this.artifactList[index].IsRedPoint());
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.skillRT);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.artifactList.Count + 1)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			int num = index - 1;
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIShowItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIShowItem uishowItem = this.GetUIItem(instanceID);
			if (uishowItem == null)
			{
				uishowItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			int num2 = this.artifactList.IndexOf(this.artifactAdvanceData);
			uishowItem.SetData(num, this.artifactList[num].Config.itemId);
			uishowItem.Refresh(num2, this.artifactList[num].IsUnlock, this.artifactList[num].IsRedPoint());
			return loopListViewItem;
		}

		private UIShowItem GetUIItem(int instanceId)
		{
			UIShowItem uishowItem;
			if (this.dic.TryGetValue(instanceId, out uishowItem))
			{
				return uishowItem;
			}
			return null;
		}

		private UIShowItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIShowItem uishowItem = this.GetUIItem(instanceID);
			if (uishowItem == null)
			{
				uishowItem = obj.GetComponent<UIShowItem>();
				uishowItem.Init();
				this.dic.Add(instanceID, uishowItem);
				return uishowItem;
			}
			return uishowItem;
		}

		private void OnClickPreview()
		{
			SkillUpgradePreviewViewModule.OpenData openData = new SkillUpgradePreviewViewModule.OpenData
			{
				currentSkillId = this.artifactAdvanceData.Config.initSkill,
				nextSkillId = this.artifactAdvanceData.Config.maxStarSkill,
				star = this.artifactAdvanceData.Config.maxStar
			};
			GameApp.View.OpenView(ViewName.SkillUpgradePreviewViewModule, openData, 1, null, null);
		}

		private void OnClickEquipSkill()
		{
			int num = 0;
			if ((ulong)this.artifactDataModule.ArtifactInfo.SkillArtifactId == (ulong)((long)this.artifactAdvanceData.ID))
			{
				num = -1;
			}
			NetworkUtils.Artifact.DoArtifactApplySkillRequest(this.artifactAdvanceData.ID, num, delegate(bool result, ArtifactApplySkillResponse response)
			{
				this.Refresh();
			});
		}

		private void OnClickUpgrade()
		{
			if (!this.artifactAdvanceData.IsUnlock)
			{
				NetworkUtils.Artifact.DoArtifactUnlockRequest(this.artifactAdvanceData.ID, delegate(bool result, ArtifactUnlockResponse response)
				{
					if (result)
					{
						this.levelUpAni.Play("Show");
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIArtifact_AdvanceUpgrade, null);
					}
				});
				return;
			}
			bool flag = false;
			PropData nextStarCost = this.artifactAdvanceData.GetNextStarCost();
			if (nextStarCost != null)
			{
				flag = this.propDataModule.GetItemDataCountByid((ulong)nextStarCost.id) >= (long)nextStarCost.count;
			}
			if (flag)
			{
				NetworkUtils.Artifact.DoArtifactItemStarRequest(this.artifactAdvanceData.ID, delegate(bool result, ArtifactItemStarResponse response)
				{
					if (result)
					{
						this.levelUpAni.Play("Show");
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIArtifact_AdvanceUpgrade, null);
					}
				});
				return;
			}
			this.ShowNotEnoughTip();
		}

		private void ShowNotEnoughTip()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uiartifact_upgrade_disabled");
			GameApp.View.ShowStringTip(infoByID);
		}

		[Header("技能")]
		public CustomText textSkillTitle;

		public UISkillItem uiSkillItem;

		public CustomText textSkillName;

		public CustomText textSkillDes;

		public CustomButton buttonPreview;

		public CustomButton buttonEquipSkill;

		public CustomText textEquipSkill;

		public RectTransform skillRT;

		[Header("升星")]
		public CustomText textAttrTitle;

		public CustomText textAttribute;

		public CustomText textCurrentAttr;

		public CustomText textNextAttr;

		public UICostItem uiCostItem;

		public CustomButton buttonUpgrade;

		public CustomText textUpgrade;

		public RedNodeOneCtrl redPointUpgrade;

		public Animator levelUpAni;

		public GameObject maxObj;

		[Header("列表")]
		public LoopListView2 loopListView;

		private ArtifactDataModule artifactDataModule;

		private PropDataModule propDataModule;

		private ArtifactAdvanceData artifactAdvanceData;

		private GameSkill_skill skill;

		private Dictionary<int, UIShowItem> dic = new Dictionary<int, UIShowItem>();

		private List<ArtifactAdvanceData> artifactList;

		private const int EmptyNodeCount = 1;
	}
}
