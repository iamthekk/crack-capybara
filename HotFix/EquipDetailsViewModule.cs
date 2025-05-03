using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class EquipDetailsViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.levelAnimator.gameObject.SetActive(false);
			this.baseAttribute1Animator.gameObject.SetActive(false);
			this.baseAttribute2Animator.gameObject.SetActive(false);
			this.textAttrList.Add(this.m_textBasicAttr1);
			this.textAttrList.Add(this.m_textBasicAttr2);
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_equipDataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			this.m_equipItem.Init();
			this.m_equipDetailsBottomButtons1.Init();
			this.m_equipDetailsBottomButtons1.SetData(this);
			if (this.m_skillItemPrefab != null)
			{
				this.m_skillItemPrefab.SetActive(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as EquipDetailsViewModule.OpenData;
			if (this.m_openData == null)
			{
				HLog.LogError("EquipDetailsViewModule OpenData is null");
				return;
			}
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			List<EquipSkillInfo> allSkillInfoList = this.m_equipDataModule.GetAllSkillInfoList((int)this.m_openData.m_equipData.id);
			for (int i = 0; i < allSkillInfoList.Count; i++)
			{
				EquipSkillInfo equipSkillInfo = allSkillInfoList[i];
				if (equipSkillInfo != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_skillItemPrefab);
					gameObject.SetParentNormal(this.m_scrollContent, false);
					UIEquipDetailsSkillItem component = gameObject.GetComponent<UIEquipDetailsSkillItem>();
					component.Init();
					component.SetActive(true);
					component.SetData(equipSkillInfo.m_skillID, equipSkillInfo.m_quality, equipSkillInfo.m_quality > this.m_openData.m_equipData.composeId, allSkillInfoList.Count, i + 1);
					this.m_skillItems[component.GetObjectInstanceID()] = component;
				}
			}
			this.m_scrollRect.verticalNormalizedPosition = 1f;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_scrollContent);
			this.m_btnReset.onClick.AddListener(new UnityAction(this.OnBtnResetClick));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnEventRefreshUI));
			this.RefreshUI();
			if (this.m_equipDetailsBottomButtons1 != null && this.m_equipDetailsBottomButtons1.m_btnPutUp != null)
			{
				GuideController.Instance.DelTarget("BtnPutUp");
				GuideController.Instance.AddTarget("BtnPutUp", this.m_equipDetailsBottomButtons1.m_btnPutUp.transform);
				GuideController.Instance.OpenViewTrigger(ViewName.EquipDetailsViewModule);
			}
		}

		private void RefreshButtons()
		{
			if (this.m_openData.m_isShowButtons)
			{
				this.m_buttomButtons.SetActive(true);
				this.m_bottomSpaceFill.SetActive(false);
				this.m_equipDetailsBottomButtons1.m_equipCostObj.SetActive(true);
				this.m_equipDetailsBottomButtons1.gameObject.SetActive(true);
				EquipData equipData = this.m_openData.m_equipData;
				Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
				this.m_btnReset.gameObject.SetActive(equipData.level > 1U || (elementById != null && elementById.qualityPlus > 0));
				if (this.m_equipDetailsBottomButtons1.gameObject.activeSelf)
				{
					this.m_equipDetailsBottomButtons1.RefreshButtons();
					return;
				}
			}
			else
			{
				this.m_equipDetailsBottomButtons1.m_equipCostObj.SetActive(false);
				this.m_buttomButtons.SetActive(false);
				this.m_btnReset.gameObject.SetActive(false);
				this.m_bottomSpaceFill.SetActive(true);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_btnReset.onClick.RemoveListener(new UnityAction(this.OnBtnResetClick));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnEventRefreshUI));
			foreach (KeyValuePair<int, UIEquipDetailsSkillItem> keyValuePair in this.m_skillItems)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					if (!(keyValuePair.Value.gameObject == null))
					{
						Object.Destroy(keyValuePair.Value.gameObject);
					}
				}
			}
			this.m_skillItems.Clear();
		}

		public override void OnDelete()
		{
			this.m_uiPopCommon.OnClick = null;
			this.m_equipItem.DeInit();
			this.m_equipDetailsBottomButtons1.DeInit();
			this.m_equipDetailsBottomButtons1 = null;
			this.m_openData = null;
			this.m_propDataModule = null;
			this.m_equipDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
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
			GameApp.View.CloseView(ViewName.EquipDetailsViewModule, null);
		}

		private void OnBtnResetClick()
		{
			EquipResetViewModule.OpenData openData = new EquipResetViewModule.OpenData();
			openData.equipData = this.m_openData.m_equipData;
			GameApp.View.OpenView(ViewName.EquipResetViewModule, openData, 1, null, null);
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(this.m_openData.m_equipData.composeId);
			Quality_equipQuality elementById2 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById.qualityColor);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_openData.m_equipData.nameId);
			this.m_titleTxt.text = infoByID;
			this.m_equipItem.RefreshData(this.m_openData.m_equipData);
			this.m_equipItem.SetButtonEnable(false);
			if (elementById != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(156);
				if (this.m_imageTitle != null)
				{
					this.m_imageTitle.SetImage(atlasPath, DxxTools.UI.GetTitlePathByQuality(elementById.qualityColor));
				}
				Quality_petQuality quality_petQuality = GameApp.Table.GetManager().GetQuality_petQuality(elementById.qualityColor);
				if (quality_petQuality != null)
				{
					string atlasPath2 = GameApp.Table.GetAtlasPath(quality_petQuality.atlasId);
					if (this.m_imageQuality != null)
					{
						this.m_imageQuality.SetImage(atlasPath2, quality_petQuality.typeTxtBg);
					}
					this.m_textQuality.text = string.Concat(new string[]
					{
						"<color=",
						quality_petQuality.colorNum,
						">",
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID),
						"</color>"
					});
				}
			}
			double num = this.m_equipDataModule.MathCombatData(this.m_openData.m_equipData);
			this.m_textCombat.text = DxxTools.FormatNumber((long)num);
			this.m_textLevel.text = string.Format("{0}/{1}", this.m_openData.m_equipData.level, this.m_openData.m_equipData.GetMaxLevel());
			Equip_equipType elementById3 = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById((int)this.m_openData.m_equipData.equipType);
			this.m_textType.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.typeName);
			List<MergeAttributeData> mergeAttributeData = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)this.m_openData.m_equipData.id)
				.GetMergeAttributeData((int)this.m_openData.m_equipData.level, this.m_openData.m_equipData.evolution);
			for (int i = 0; i < this.textAttrList.Count; i++)
			{
				CustomText customText = this.textAttrList[i];
				MergeAttributeData mergeAttributeData2 = ((mergeAttributeData.Count > i) ? mergeAttributeData[i] : null);
				if (mergeAttributeData2 != null)
				{
					customText.gameObject.SetActive(true);
					Attribute_AttrText elementById4 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData2.Header);
					if (mergeAttributeData2.Header.Contains("%"))
					{
						customText.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.LanguageId) + " <color=#d3f24e>+" + mergeAttributeData2.Value.ToString() + "%</color>";
					}
					else
					{
						customText.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.LanguageId) + " <color=#d3f24e>+" + DxxTools.FormatNumber(mergeAttributeData2.Value.AsLong()) + "</color>";
					}
				}
				else
				{
					customText.gameObject.SetActive(false);
				}
			}
			int rageSkill = this.m_equipDataModule.GetCurEquipAction((int)this.m_openData.m_equipData.id, this.m_openData.m_equipData.composeId).rageSkill;
			if (rageSkill > 0)
			{
				this.m_image_SkillBgMask.SetActiveSafe(true);
				this.m_image_SkillBg.SetActiveSafe(true);
				this.goSkillTitleNode.SetActive(true);
				this.goSkillNameNode.SetActive(true);
				this.goSkillDescNode.SetActive(true);
				List<EquipSkillInfo> unLockSkillInfoList = this.m_equipDataModule.GetUnLockSkillInfoList((int)this.m_openData.m_equipData.id, this.m_openData.m_equipData.composeId);
				GameSkill_skill elementById5 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(rageSkill);
				if (elementById5 != null)
				{
					string text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById5.nameID) + " " + Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { Mathf.Clamp(unLockSkillInfoList.Count, 1, int.MaxValue) });
					this.m_textSkillName.text = string.Concat(new string[] { "<color=", elementById2.colorNumLight, ">", text, "</color>" });
					this.m_textSkillDesc.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(elementById5.fullDetailID), true);
					this.m_textSkillTypeGo.gameObject.SetActive(elementById5.freedType == 4);
				}
				else
				{
					HLog.LogError(string.Format("skillId:{0} is not exist in GameSkill config", rageSkill));
				}
			}
			else
			{
				this.m_image_SkillBgMask.SetActiveSafe(false);
				this.m_image_SkillBg.SetActiveSafe(false);
				this.goSkillTitleNode.SetActive(false);
				this.goSkillNameNode.SetActive(false);
				this.goSkillDescNode.SetActive(false);
			}
			this.m_NodeRefining.SetActive(false);
			this.m_spaceFill.SetActive(false);
			this.RefreshButtons();
		}

		public UIPopCommon m_uiPopCommon;

		public CustomImage m_imageTitle;

		public CustomImage m_imageQuality;

		public CustomText m_textQuality;

		public CustomText m_titleTxt;

		public CustomText m_textCombat;

		public UIHeroEquipItem m_equipItem;

		public CustomText m_textLevel;

		public CustomText m_textType;

		public CustomText m_textBasicAttr1;

		public CustomText m_textBasicAttr2;

		public CustomText m_textRefiningAttr1;

		public CustomText m_textRefiningAttr2;

		public CustomText m_textRefiningAttr3;

		public CustomText m_textRefiningAttr4;

		public GameObject m_image_SkillBgMask;

		public GameObject m_image_SkillBg;

		public GameObject goSkillTitleNode;

		public GameObject goSkillNameNode;

		public GameObject goSkillDescNode;

		public GameObject goSkillAttributesNode;

		public CustomText m_textSkillName;

		public GameObject m_textSkillTypeGo;

		public CustomTextScrollView m_textSkillDesc;

		public ScrollRect m_scrollRect;

		public RectTransform m_scrollContent;

		public GameObject m_skillItemPrefab;

		public Dictionary<int, UIEquipDetailsSkillItem> m_skillItems = new Dictionary<int, UIEquipDetailsSkillItem>();

		public EquipDetailsBottomButtons m_equipDetailsBottomButtons1;

		public CustomButton m_btnReset;

		public GameObject m_NodeRefining;

		public Animator levelAnimator;

		public Animator baseAttribute1Animator;

		public Animator baseAttribute2Animator;

		public EquipDetailsViewModule.OpenData m_openData;

		public PropDataModule m_propDataModule;

		public EquipDataModule m_equipDataModule;

		public GameObject m_spaceFill;

		public GameObject m_bottomSpaceFill;

		public GameObject m_buttomButtons;

		private List<CustomText> textAttrList = new List<CustomText>();

		public class OpenData
		{
			public EquipData m_equipData;

			public bool m_isShowButtons = true;
		}
	}
}
