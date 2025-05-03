using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Mount;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMountAdvanceNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mountDataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
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
			this.mountList = this.mountDataModule.GetAdvanceDataList();
			this.loopListView.SetListItemCount(this.mountList.Count + 1, true);
			this.loopListView.RefreshAllShowItems();
		}

		public void OnHide()
		{
			this.mountList = new List<MountAdvanceData>();
			this.loopListView.SetListItemCount(this.mountList.Count + 1, true);
			this.loopListView.RefreshAllShowItems();
		}

		public void SetData(MountAdvanceData data)
		{
			this.mountAdvanceData = data;
			this.skill = data.GetSkill();
			this.Refresh();
		}

		private void Refresh()
		{
			this.redPointUpgrade.gameObject.SetActive(this.mountAdvanceData.IsRedPoint());
			this.buttonPreview.gameObject.SetActiveSafe(this.mountAdvanceData.IsUnlock && !this.mountAdvanceData.IsMaxStar);
			this.buttonEquipSkill.gameObject.SetActiveSafe(this.mountAdvanceData.IsUnlock);
			if (this.skill != null)
			{
				this.uiSkillItem.SetData(this.skill, this.mountAdvanceData.Config.quality);
				this.textSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.skill.nameID);
				this.textSkillDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.skill.fullDetailID);
			}
			this.textSkillTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_skill_title");
			this.textAttrTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_attribute_title");
			long currentAttributeValue = this.mountAdvanceData.GetCurrentAttributeValue();
			long nextAttributeValue = this.mountAdvanceData.GetNextAttributeValue();
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.mountAdvanceData.InitAttributeData.Header);
			if (elementById != null)
			{
				this.textAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
				this.textCurrentAttr.text = (elementById.ID.Contains("%") ? string.Format("{0}%", currentAttributeValue) : currentAttributeValue.ToString());
				this.textNextAttr.text = (elementById.ID.Contains("%") ? string.Format("{0}%", nextAttributeValue) : nextAttributeValue.ToString());
			}
			else
			{
				HLog.LogError("Table [Attribute_AttrText] not found id=" + this.mountAdvanceData.InitAttributeData.Header);
			}
			if (this.mountAdvanceData.IsMaxStar)
			{
				this.textNextAttr.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_stage_max");
			}
			this.maxObj.SetActive(this.mountAdvanceData.IsMaxStar);
			this.uiCostItem.SetActive(true);
			if (this.mountAdvanceData.IsUnlock)
			{
				this.textUpgrade.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_upgrade");
				this.buttonUpgrade.gameObject.SetActiveSafe(!this.mountAdvanceData.IsMaxStar);
				if (this.mountAdvanceData.IsMaxStar)
				{
					this.uiCostItem.SetActive(false);
				}
				else
				{
					PropData nextStarCost = this.mountAdvanceData.GetNextStarCost();
					if (nextStarCost != null)
					{
						long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)nextStarCost.id);
						this.uiCostItem.SetData((int)nextStarCost.id, itemDataCountByid, (long)nextStarCost.count);
					}
					else
					{
						HLog.LogError(string.Format("未配置{0}星升级道具", this.mountAdvanceData.Star + 1));
					}
				}
			}
			else
			{
				this.textUpgrade.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_unlock");
				int unlockCostId = this.mountAdvanceData.Config.unlockCostId;
				long itemDataCountByid2 = this.propDataModule.GetItemDataCountByid((ulong)((long)unlockCostId));
				this.buttonUpgrade.gameObject.SetActiveSafe(itemDataCountByid2 > 0L);
				this.uiCostItem.SetData(unlockCostId, itemDataCountByid2, 1L);
			}
			if ((ulong)this.mountDataModule.MountInfo.SkillMountId == (ulong)((long)this.mountAdvanceData.ID))
			{
				this.textEquipSkill.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_skill_cancel");
			}
			else
			{
				this.textEquipSkill.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_skill_use");
			}
			int num = this.mountList.IndexOf(this.mountAdvanceData);
			foreach (UIAdvanceMountItem uiadvanceMountItem in this.dic.Values)
			{
				int index = uiadvanceMountItem.GetIndex();
				uiadvanceMountItem.Refresh(num, this.mountList[index].IsUnlock, this.mountList[index].IsRedPoint());
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.skillRT);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mountList.Count + 1)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			int num = index - 1;
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIAdvanceMountItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIAdvanceMountItem uiadvanceMountItem = this.GetUIItem(instanceID);
			if (uiadvanceMountItem == null)
			{
				uiadvanceMountItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			int num2 = this.mountList.IndexOf(this.mountAdvanceData);
			uiadvanceMountItem.SetData(this.mountList[num], num);
			uiadvanceMountItem.Refresh(num2, this.mountList[num].IsUnlock, this.mountList[num].IsRedPoint());
			return loopListViewItem;
		}

		private UIAdvanceMountItem GetUIItem(int instanceId)
		{
			UIAdvanceMountItem uiadvanceMountItem;
			if (this.dic.TryGetValue(instanceId, out uiadvanceMountItem))
			{
				return uiadvanceMountItem;
			}
			return null;
		}

		private UIAdvanceMountItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIAdvanceMountItem uiadvanceMountItem = this.GetUIItem(instanceID);
			if (uiadvanceMountItem == null)
			{
				uiadvanceMountItem = obj.GetComponent<UIAdvanceMountItem>();
				uiadvanceMountItem.Init();
				this.dic.Add(instanceID, uiadvanceMountItem);
				return uiadvanceMountItem;
			}
			return uiadvanceMountItem;
		}

		private void OnClickPreview()
		{
			SkillUpgradePreviewViewModule.OpenData openData = new SkillUpgradePreviewViewModule.OpenData
			{
				currentSkillId = this.mountAdvanceData.Config.initSkill,
				nextSkillId = this.mountAdvanceData.Config.maxStarSkill,
				star = this.mountAdvanceData.Config.maxStar
			};
			GameApp.View.OpenView(ViewName.SkillUpgradePreviewViewModule, openData, 1, null, null);
		}

		private void OnClickEquipSkill()
		{
			int num = 0;
			if ((ulong)this.mountDataModule.MountInfo.SkillMountId == (ulong)((long)this.mountAdvanceData.ID))
			{
				num = -1;
			}
			NetworkUtils.Mount.DoMountApplySkillRequest(this.mountAdvanceData.ID, num, delegate(bool result, MountApplySkillResponse response)
			{
				this.Refresh();
			});
		}

		private void OnClickUpgrade()
		{
			if (!this.mountAdvanceData.IsUnlock)
			{
				NetworkUtils.Mount.DoMountUnlockRequest(this.mountAdvanceData.ID, delegate(bool result, MountUnlockResponse response)
				{
					if (result)
					{
						this.levelUpAni.Play("Show");
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIMount_AdvanceUpgrade, null);
					}
				});
				return;
			}
			bool flag = false;
			PropData nextStarCost = this.mountAdvanceData.GetNextStarCost();
			if (nextStarCost != null)
			{
				flag = this.propDataModule.GetItemDataCountByid((ulong)nextStarCost.id) >= (long)nextStarCost.count;
			}
			if (flag)
			{
				NetworkUtils.Mount.DoMountItemStarRequest(this.mountAdvanceData.ID, delegate(bool result, MountItemStarResponse response)
				{
					if (result)
					{
						this.levelUpAni.Play("Show");
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIMount_AdvanceUpgrade, null);
					}
				});
				return;
			}
			this.ShowNotEnoughTip();
		}

		private void ShowNotEnoughTip()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_upgrade_disabled");
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

		private MountDataModule mountDataModule;

		private PropDataModule propDataModule;

		private MountAdvanceData mountAdvanceData;

		private GameSkill_skill skill;

		private Dictionary<int, UIAdvanceMountItem> dic = new Dictionary<int, UIAdvanceMountItem>();

		private List<MountAdvanceData> mountList;

		private const int EmptyNodeCount = 1;
	}
}
