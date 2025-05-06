using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIEquipMergeEquipGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_equip_next.Init();
			this.m_equip_next.SetActive(false);
			this.m_equip_select.Init();
			this.m_equip_select.SetActive(false);
			this.m_oneEquipItem.Init();
			this.m_oneObj.SetActive(false);
			this.m_onePropItem.Init();
			this.m_onePropItem.SetActive(false);
			this.m_twoEquipItem1.Init();
			this.m_twoEquipItem2.Init();
			this.m_twoObj.SetActive(false);
			this.m_twoPropItem1.Init();
			this.m_twoPropItem2.Init();
			this.m_twoPropItem1.SetActive(false);
			this.m_twoPropItem2.SetActive(false);
			this.m_costObj.SetActive(false);
			this.m_nullSelectObject.SetActive(true);
			this.m_selectObject.gameObject.SetActive(false);
			this.m_attributePrefab.SetActive(false);
			this.m_skillPrefab.SetActive(false);
			this.m_equip_next.m_onClick = new Action<UIHeroEquipItem>(this.OnClickNextEquipItem);
			this.m_equip_select.m_onClick = new Action<UIHeroEquipItem>(this.OnClickEquipItem);
			this.m_oneEquipItem.m_onClick = new Action<UIHeroEquipItem>(this.OnClickEquipItem);
			this.m_twoEquipItem1.m_onClick = new Action<UIHeroEquipItem>(this.OnClickEquipItem);
			this.m_twoEquipItem2.m_onClick = new Action<UIHeroEquipItem>(this.OnClickEquipItem);
			this.m_onePropItem.onClick = new Action<UIItem, PropData, object>(this.OnClickPropItem);
			this.m_twoPropItem1.onClick = new Action<UIItem, PropData, object>(this.OnClickPropItem);
			this.m_twoPropItem2.onClick = new Action<UIItem, PropData, object>(this.OnClickPropItem);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_equip_next.m_onClick = null;
			this.m_equip_select.m_onClick = null;
			this.m_oneEquipItem.m_onClick = null;
			this.m_twoEquipItem1.m_onClick = null;
			this.m_twoEquipItem2.m_onClick = null;
			this.m_onClickEquipItem = null;
			this.m_onClickPropItem = null;
			this.m_onClickNextEquipItem = null;
			foreach (KeyValuePair<int, UIEquipMergeAttributeNode> keyValuePair in this.m_attributeDic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_attributeDic.Clear();
			foreach (KeyValuePair<int, UIEquipMergeSkillNode> keyValuePair2 in this.m_skillDic)
			{
				keyValuePair2.Value.DeInit();
				Object.Destroy(keyValuePair2.Value.gameObject);
			}
			this.m_skillDic.Clear();
			this.m_equip_next.DeInit();
			this.m_equip_select.DeInit();
			this.m_oneEquipItem.DeInit();
			this.m_twoEquipItem1.DeInit();
			this.m_twoEquipItem2.DeInit();
		}

		public void SetSelectEquip(EquipData equipData, bool isPuton = false)
		{
			if (this.m_equip_select == null)
			{
				return;
			}
			this.m_currentSelectRowID = ((equipData != null) ? equipData.rowID : 0UL);
			this.m_equip_select.RefreshData(equipData);
			this.m_equip_select.SetPutOnActive(isPuton);
			this.m_equip_select.SetActive(equipData != null);
		}

		public void SetNextEquip(EquipData equipData)
		{
			if (this.m_equip_next == null)
			{
				return;
			}
			this.m_equip_next.RefreshData(equipData);
			this.m_equip_next.SetActive(equipData != null);
		}

		public void SetCostCount(int count)
		{
			if (this.m_oneObj != null)
			{
				this.m_oneObj.SetActive(false);
			}
			if (this.m_twoObj != null)
			{
				this.m_twoObj.SetActive(false);
			}
			if (this.m_onePropItem != null)
			{
				this.m_onePropItem.SetActive(false);
			}
			if (this.m_twoPropItem1 != null)
			{
				this.m_twoPropItem1.SetActive(false);
			}
			if (this.m_twoEquipItem2 != null)
			{
				this.m_twoPropItem2.SetActive(false);
			}
			if (this.m_oneEquipItem != null)
			{
				this.m_oneEquipItem.RefreshData(null);
				this.m_oneEquipItem.SetActive(false);
			}
			if (this.m_twoEquipItem1 != null)
			{
				this.m_twoEquipItem1.RefreshData(null);
				this.m_twoEquipItem1.SetActive(false);
			}
			if (this.m_twoEquipItem2 != null)
			{
				this.m_twoEquipItem2.RefreshData(null);
				this.m_twoEquipItem2.SetActive(false);
			}
			if (count == 1)
			{
				if (this.m_oneEquipItem != null)
				{
					this.m_oneEquipItem.SetActive(true);
				}
				if (this.m_oneObj != null)
				{
					this.m_oneObj.SetActive(true);
				}
			}
			if (count == 2)
			{
				if (this.m_twoEquipItem1 != null)
				{
					this.m_twoEquipItem1.SetActive(true);
				}
				if (this.m_twoEquipItem2 != null)
				{
					this.m_twoEquipItem2.SetActive(true);
				}
				if (this.m_twoObj != null)
				{
					this.m_twoObj.SetActive(true);
				}
			}
		}

		public void SetOneEquipItem(EquipData equipData, int equipId, int composeId)
		{
			if (this.m_oneEquipItem == null)
			{
				return;
			}
			this.m_oneEquipItem.SetMergeMaterialData(equipData, equipId, composeId);
		}

		public void SetOnePropItem(PropData propData)
		{
			if (this.m_onePropItem == null)
			{
				return;
			}
			if (propData == null)
			{
				this.m_onePropItem.SetActive(false);
				return;
			}
			this.m_onePropItem.SetData(propData);
			this.m_onePropItem.OnRefresh();
			this.m_onePropItem.SetActive(true);
			this.m_onePropItem.SetCountText("");
		}

		public void SetTwoEquipItem1(EquipData equipData, int equipId, int composeId)
		{
			if (this.m_twoEquipItem1 == null)
			{
				return;
			}
			this.m_twoEquipItem1.SetMergeMaterialData(equipData, equipId, composeId);
		}

		public void SetTwoEquipItem2(EquipData equipData, int equipId, int composeId)
		{
			if (this.m_twoEquipItem2 == null)
			{
				return;
			}
			this.m_twoEquipItem2.SetMergeMaterialData(equipData, equipId, composeId);
		}

		public void SetTwoPropItem1(PropData propData)
		{
			if (this.m_twoPropItem1 == null)
			{
				return;
			}
			if (propData == null)
			{
				this.m_twoPropItem1.SetActive(false);
				return;
			}
			this.m_twoPropItem1.SetData(propData);
			this.m_twoPropItem1.OnRefresh();
			this.m_twoPropItem1.SetActive(true);
			this.m_twoPropItem1.SetCountText("");
		}

		public void SetTwoPropItem2(PropData propData)
		{
			if (this.m_twoPropItem2 == null)
			{
				return;
			}
			if (propData == null)
			{
				this.m_twoPropItem2.SetActive(false);
				return;
			}
			this.m_twoPropItem2.SetData(propData);
			this.m_twoPropItem2.OnRefresh();
			this.m_twoPropItem2.SetActive(true);
			this.m_twoPropItem2.SetCountText("");
		}

		public void SetCostActive(bool active)
		{
			if (this.m_costObj == null)
			{
				return;
			}
			this.m_costObj.SetActive(active);
		}

		public void SetCost(EquipData equipData)
		{
			if (equipData == null)
			{
				return;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
			if (elementById == null)
			{
				return;
			}
			Equip_equipCompose elementById2 = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(elementById.composeNeed2);
			if (elementById2 == null)
			{
				return;
			}
			Quality_equipQuality elementById3 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById2.qualityColor);
			switch (elementById.composeNeed1)
			{
			case 0:
			{
				if (this.m_costHaveIconImage != null)
				{
					this.m_costHaveIconImage.gameObject.SetActive(false);
				}
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("text_merge_require_0", new object[]
				{
					elementById.composeNeed3,
					string.Concat(new string[]
					{
						"<color=",
						elementById3.colorNumDark,
						">",
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID),
						"</color>"
					})
				});
				this.m_costHaveIconTxt.text = infoByID;
				return;
			}
			case 1:
			{
				Equip_equipType elementById4 = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById((int)equipData.equipType);
				string atlasPath = GameApp.Table.GetAtlasPath(elementById4.atlasID);
				if (this.m_costHaveIconImage != null)
				{
					this.m_costHaveIconImage.gameObject.SetActive(true);
					this.m_costHaveIconImage.SetImage(atlasPath, elementById4.iconName);
				}
				if (this.m_costHaveIconTxt != null)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("text_merge_require_1", new object[]
					{
						elementById.composeNeed3,
						string.Concat(new string[]
						{
							"<color=",
							elementById3.colorNumDark,
							">",
							Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID),
							"</color>"
						})
					});
					this.m_costHaveIconTxt.text = infoByID2;
					return;
				}
				break;
			}
			case 2:
			{
				if (this.m_costHaveIconImage != null)
				{
					this.m_costHaveIconImage.gameObject.SetActive(false);
				}
				string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("text_merge_require_2", new object[]
				{
					elementById.composeNeed3,
					string.Concat(new string[]
					{
						"<color=",
						elementById3.colorNumDark,
						">",
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID),
						"</color>"
					}),
					Singleton<LanguageManager>.Instance.GetInfoByID(equipData.nameId)
				});
				this.m_costHaveIconTxt.text = infoByID3;
				return;
			}
			case 3:
			{
				if (this.m_costHaveIconImage != null)
				{
					this.m_costHaveIconImage.gameObject.SetActive(false);
				}
				int num = equipData.tagId * 100 + elementById2.composeNeed2;
				Item_Item elementById5 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
				string infoByID4 = Singleton<LanguageManager>.Instance.GetInfoByID("text_merge_require_2", new object[]
				{
					elementById.composeNeed3,
					string.Concat(new string[]
					{
						"<color=",
						elementById3.colorNumDark,
						">",
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID),
						"</color>"
					}),
					Singleton<LanguageManager>.Instance.GetInfoByID(elementById5.nameID)
				});
				this.m_costHaveIconTxt.text = infoByID4;
				break;
			}
			default:
				return;
			}
		}

		public void SetInfo(EquipData equipData, EquipData nextEquipData)
		{
			if (equipData == null)
			{
				if (this.m_selectObject != null)
				{
					this.m_selectObject.gameObject.SetActive(false);
				}
				if (this.m_nullSelectObject != null)
				{
					this.m_nullSelectObject.gameObject.SetActive(true);
				}
				return;
			}
			if (this.m_nullSelectObject != null)
			{
				this.m_nullSelectObject.gameObject.SetActive(false);
			}
			if (this.m_selectObject != null)
			{
				this.m_selectObject.gameObject.SetActive(true);
			}
			if (this.m_selectEquipName != null)
			{
				this.m_selectEquipName.text = Singleton<LanguageManager>.Instance.GetInfoByID(equipData.nameId);
			}
			GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(nextEquipData.composeId);
			foreach (KeyValuePair<int, UIEquipMergeAttributeNode> keyValuePair in this.m_attributeDic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_attributeDic.Clear();
			foreach (KeyValuePair<int, UIEquipMergeSkillNode> keyValuePair2 in this.m_skillDic)
			{
				keyValuePair2.Value.DeInit();
				Object.Destroy(keyValuePair2.Value.gameObject);
			}
			this.m_skillDic.Clear();
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipData.id);
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			List<MergeAttributeData> list = elementById2.GetMergeAttributeData((int)equipData.level, equipData.evolution);
			list = list.Merge();
			memberAttributeData.MergeAttributes(list, false);
			Equip_equip elementById3 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)nextEquipData.id);
			MemberAttributeData memberAttributeData2 = new MemberAttributeData();
			List<MergeAttributeData> mergeAttributeData = elementById3.GetMergeAttributeData((int)nextEquipData.level, nextEquipData.evolution);
			memberAttributeData2.MergeAttributes(mergeAttributeData, false);
			List<MergeAttributeData> mergeAttributeData2 = elementById2.baseAttributes.GetMergeAttributeData();
			List<string> list2 = new List<string>();
			List<FP> list3 = new List<FP>();
			for (int i = 0; i < mergeAttributeData2.Count; i++)
			{
				MergeAttributeData mergeAttributeData3 = mergeAttributeData2[i];
				if (mergeAttributeData3 != null && !list2.Contains(mergeAttributeData3.Header))
				{
					list2.Add(mergeAttributeData3.Header);
					list3.Add(mergeAttributeData3.Value);
				}
			}
			int num = elementById.qualityAttributes / 100;
			for (int j = 0; j < list2.Count; j++)
			{
				string text = list2[j];
				FP fp = list3[j];
				long basicAttributeValue = memberAttributeData.GetBasicAttributeValue(text);
				memberAttributeData2.GetBasicAttributeValue(text);
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_attributePrefab);
				gameObject.SetParentNormal(this.m_attributeGroup, false);
				Transform transform = gameObject.transform;
				UIEquipMergeAttributeNode component = gameObject.GetComponent<UIEquipMergeAttributeNode>();
				component.Init();
				component.SetActive(true);
				Attribute_AttrText elementById4 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(text);
				component.SetNameTxt(Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.LanguageId));
				component.SetFromTxt(basicAttributeValue.ToString());
				if (text == "Attack" || text == "Defence" || text == "HPMax")
				{
					component.SetToTxt("+" + num.ToString() + "%");
				}
				else
				{
					component.SetToTxt(string.Format("+{0}%", fp / 2));
				}
				this.m_attributeDic[component.GetObjectInstanceID()] = component;
			}
			List<EquipSkillInfo> skillInfoList = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetSkillInfoList((int)equipData.id, nextEquipData.composeId);
			if (skillInfoList.Count > 0)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_skillPrefab);
				gameObject2.SetParentNormal(this.m_skillGroup, false);
				Transform transform2 = gameObject2.transform;
				UIEquipMergeSkillNode component2 = gameObject2.GetComponent<UIEquipMergeSkillNode>();
				component2.Init();
				component2.SetActive(true);
				component2.SetTxt(Singleton<LanguageManager>.Instance.GetInfoByID("equip_skill_level_up_to", new object[] { skillInfoList[skillInfoList.Count - 1].m_skillLevel }));
				this.m_skillDic[component2.GetObjectInstanceID()] = component2;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_selectObject);
		}

		public Vector3 GetSelectPosition()
		{
			return this.m_equip_select.transform.position;
		}

		public Vector3 GetNextPosition()
		{
			return this.m_equip_next.transform.position;
		}

		public Vector3 GetMergeNodePosition(int count, int index)
		{
			if (count == 1)
			{
				return this.m_oneEquipItem.transform.position;
			}
			if (index == 0)
			{
				return this.m_twoEquipItem1.transform.position;
			}
			if (index == 1)
			{
				return this.m_twoEquipItem2.transform.position;
			}
			return Vector3.zero;
		}

		private void OnClickNextEquipItem(UIHeroEquipItem obj)
		{
			if (this.m_onClickNextEquipItem == null)
			{
				return;
			}
			this.m_onClickNextEquipItem(obj);
		}

		private void OnClickEquipItem(UIHeroEquipItem obj)
		{
			if (this.m_onClickEquipItem == null)
			{
				return;
			}
			this.m_onClickEquipItem(obj);
		}

		private void OnClickPropItem(UIItem uiItem, PropData propData, object obj)
		{
			if (this.m_onClickPropItem == null)
			{
				return;
			}
			this.m_onClickPropItem(uiItem);
		}

		[SerializeField]
		private UIHeroEquipItem m_equip_next;

		[SerializeField]
		private UIHeroEquipItem m_equip_select;

		[SerializeField]
		private GameObject m_oneObj;

		[SerializeField]
		private UIHeroEquipItem m_oneEquipItem;

		[SerializeField]
		private UIItem m_onePropItem;

		[SerializeField]
		private GameObject m_twoObj;

		[SerializeField]
		private UIHeroEquipItem m_twoEquipItem1;

		[SerializeField]
		private UIHeroEquipItem m_twoEquipItem2;

		[SerializeField]
		private UIItem m_twoPropItem1;

		[SerializeField]
		private UIItem m_twoPropItem2;

		[SerializeField]
		private GameObject m_costObj;

		[SerializeField]
		private CustomText m_costHaveIconTxt;

		[SerializeField]
		private CustomImage m_costHaveIconImage;

		[SerializeField]
		private GameObject m_info;

		[SerializeField]
		private GameObject m_nullSelectObject;

		[SerializeField]
		private RectTransform m_selectObject;

		[SerializeField]
		private CustomText m_selectEquipName;

		[SerializeField]
		private RectTransform m_attributeGroup;

		[SerializeField]
		private GameObject m_attributePrefab;

		private Dictionary<int, UIEquipMergeAttributeNode> m_attributeDic = new Dictionary<int, UIEquipMergeAttributeNode>();

		[SerializeField]
		private RectTransform m_skillGroup;

		[SerializeField]
		private GameObject m_skillPrefab;

		private Dictionary<int, UIEquipMergeSkillNode> m_skillDic = new Dictionary<int, UIEquipMergeSkillNode>();

		public Action<UIHeroEquipItem> m_onClickEquipItem;

		public Action<UIItem> m_onClickPropItem;

		public Action<UIHeroEquipItem> m_onClickNextEquipItem;

		private ulong m_currentSelectRowID;
	}
}
