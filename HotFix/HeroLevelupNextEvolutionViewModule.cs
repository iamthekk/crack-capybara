using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Actor;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class HeroLevelupNextEvolutionViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_heroLevelupDataModule = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as HeroLevelupNextEvolutionViewModule.OpenData;
			if (this.m_openData == null)
			{
				HLog.LogError("HeroLevelupNextEvolutionViewModule OnOpen data is null");
				return;
			}
			if (this.m_openData.m_cardData == null)
			{
				HLog.LogError("HeroLevelupNextEvolutionViewModule OnOpen m_cardData is null");
				return;
			}
			this.m_cardData = new CardData();
			this.m_cardData.CloneFrom(this.m_openData.m_cardData);
			IList<HeroLevelup_HeroLevelup> allElements = GameApp.Table.GetManager().GetHeroLevelup_HeroLevelupModelInstance().GetAllElements();
			HeroLevelup_HeroLevelup heroLevelup_HeroLevelup = null;
			HeroLevelup_HeroLevelup heroLevelup_HeroLevelup2 = null;
			for (int i = 0; i < allElements.Count; i++)
			{
				HeroLevelup_HeroLevelup heroLevelup_HeroLevelup3 = allElements[i];
				if (heroLevelup_HeroLevelup3.ID == this.m_openData.m_tableID)
				{
					heroLevelup_HeroLevelup = heroLevelup_HeroLevelup3;
				}
				else if (heroLevelup_HeroLevelup != null)
				{
					heroLevelup_HeroLevelup2 = heroLevelup_HeroLevelup3;
					break;
				}
			}
			if (heroLevelup_HeroLevelup == null || heroLevelup_HeroLevelup2 == null)
			{
				HLog.LogError(string.Format("HeroLevelupNextEvolutionViewModule OnOpen currentTable= {0}, nextTable={1}", heroLevelup_HeroLevelup != null, heroLevelup_HeroLevelup2 != null));
				return;
			}
			this.m_nextTableID = heroLevelup_HeroLevelup2.ID;
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			if (this.m_okBt != null)
			{
				this.m_okBt.onClick.AddListener(new UnityAction(this.OnClickOkBt));
			}
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(this.m_openData.m_cardData.m_memberID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[HeroLevelupNextEvolutionViewModule]tab[{0}] == null", this.m_openData.m_cardData));
			}
			if (string.IsNullOrEmpty(elementById.iconSpriteName))
			{
				HLog.LogError(string.Format("[HeroLevelupNextEvolutionViewModule]未设置Member立绘 id={0}", this.m_openData.m_cardData));
			}
			string text = string.Format("{0}{1}H.png", "Assets/_Resources/Sprites/UIPlayerWholeLength/", elementById.iconSpriteName);
			this.m_icon.color = new Color(1f, 1f, 1f, 1f);
			this.m_icon.rectTransform.sizeDelta = Vector2.zero;
			this.m_icon.KeepNativeSize = true;
			this.m_icon.SetImageSingle(text);
			this.m_nameNode.Init();
			this.m_nameNode.SetFromTxt(this.m_heroLevelupDataModule.GetTrainingTitleName(heroLevelup_HeroLevelup, Color.yellow));
			this.m_nameNode.SetToTxt(this.m_heroLevelupDataModule.GetTrainingTitleName(heroLevelup_HeroLevelup2, Color.yellow));
			List<string> list = new List<string>();
			List<MergeAttributeData> list2 = new List<MergeAttributeData>();
			List<MergeAttributeData> list3 = new List<MergeAttributeData>();
			for (int j = 0; j < allElements.Count; j++)
			{
				HeroLevelup_HeroLevelup heroLevelup_HeroLevelup4 = allElements[j];
				if (heroLevelup_HeroLevelup4.ID == this.m_openData.m_tableID)
				{
					List<MergeAttributeData> levelUpRewards = this.m_heroLevelupDataModule.GetLevelUpRewards(heroLevelup_HeroLevelup4.ID);
					list3.AddRange(list2);
					list3.AddRange(levelUpRewards);
					break;
				}
				List<MergeAttributeData> levelUpRewards2 = this.m_heroLevelupDataModule.GetLevelUpRewards(heroLevelup_HeroLevelup4.ID);
				list2.AddRange(levelUpRewards2);
			}
			list2 = list2.Merge();
			list3 = list3.Merge();
			for (int k = 0; k < list3.Count; k++)
			{
				list.Add(list3[k].Header);
			}
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(list2, false);
			MemberAttributeData memberAttributeData2 = new MemberAttributeData();
			memberAttributeData2.MergeAttributes(list3, false);
			for (int l = 0; l < list.Count; l++)
			{
				string text2 = list[l];
				long basicAttributeValue = memberAttributeData.GetBasicAttributeValue(text2);
				long basicAttributeValue2 = memberAttributeData2.GetBasicAttributeValue(text2);
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_attributesNodePrefab, this.m_attributesParent);
				gameObject.transform.SetParent(this.m_attributesParent);
				gameObject.transform.position = this.m_attributesParent.position;
				gameObject.transform.localScale = Vector3.one;
				HeroLevelupAttributeController component = gameObject.GetComponent<HeroLevelupAttributeController>();
				component.Init();
				component.SetNameTxt(Singleton<LanguageManager>.Instance.GetInfoByID(GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(text2)
					.LanguageId));
				component.SetFromTxt(DxxTools.FormatNumber(basicAttributeValue));
				component.SetToTxt(DxxTools.FormatNumber(basicAttributeValue2));
				component.gameObject.SetActive(true);
				this.m_attributeNodes[gameObject.GetInstanceID()] = component;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_attributesParent);
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			List<ItemData> gradeUpCost = this.m_heroLevelupDataModule.GetGradeUpCost(heroLevelup_HeroLevelup.ID);
			for (int m = 0; m < gradeUpCost.Count; m++)
			{
				ItemData itemData = gradeUpCost[m];
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_costsNodePrefab, this.m_costsParent);
				gameObject2.transform.SetParent(this.m_costsParent);
				gameObject2.transform.position = this.m_costsParent.position;
				gameObject2.transform.localScale = Vector3.one;
				UIItem component2 = gameObject2.GetComponent<UIItem>();
				component2.Init();
				component2.SetData(itemData.ToPropData());
				component2.OnRefresh();
				long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
				component2.SetCountText((itemDataCountByid < itemData.TotalCount) ? ("<color=red>" + DxxTools.FormatNumber(itemData.TotalCount) + "</color>") : DxxTools.FormatNumber(itemData.TotalCount));
				component2.gameObject.SetActive(true);
				this.m_costNodes[gameObject2.GetInstanceID()] = component2;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_uiPopCommon.OnClick = null;
			if (this.m_okBt != null)
			{
				this.m_okBt.onClick.RemoveListener(new UnityAction(this.OnClickOkBt));
			}
			foreach (KeyValuePair<int, HeroLevelupAttributeController> keyValuePair in this.m_attributeNodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_attributeNodes.Clear();
			foreach (KeyValuePair<int, UIItem> keyValuePair2 in this.m_costNodes)
			{
				if (!(keyValuePair2.Value == null))
				{
					keyValuePair2.Value.DeInit();
					Object.Destroy(keyValuePair2.Value.gameObject);
				}
			}
			this.m_costNodes.Clear();
			this.m_openData = null;
			this.m_cardData = null;
		}

		public override void OnDelete()
		{
			if (this.m_nameNode != null)
			{
				this.m_nameNode.DeInit();
			}
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
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.HeroLevelupNextEvolutionViewModule, null);
		}

		private void OnClickOkBt()
		{
			if (!this.m_heroLevelupDataModule.IsHaveGradeUpCost(this.m_openData.m_tableID))
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30110"));
				return;
			}
			NetworkUtils.Actor.DoActorAdvanceUpRequest(delegate(bool isOk, ActorAdvanceUpResponse request)
			{
				if (!isOk)
				{
					return;
				}
				HeroLevelupEvolutionViewModule.OpenData openData = new HeroLevelupEvolutionViewModule.OpenData();
				openData.m_cardData = this.m_openData.m_cardData;
				openData.m_currentTableID = this.m_nextTableID;
				GameApp.View.OpenView(ViewName.HeroLevelupEvolutionViewModule, openData, 1, null, null);
				GameApp.View.CloseView(ViewName.HeroLevelupNextEvolutionViewModule, null);
			});
		}

		public UIPopCommon m_uiPopCommon;

		public CustomButton m_okBt;

		public CustomImage m_icon;

		public HeroLevelupAttributeController m_nameNode;

		public RectTransform m_attributesParent;

		public GameObject m_attributesNodePrefab;

		public RectTransform m_costsParent;

		public GameObject m_costsNodePrefab;

		public HeroLevelupNextEvolutionViewModule.OpenData m_openData;

		public CardData m_cardData;

		public int m_nextTableID;

		public const string PlayerPath = "Assets/_Resources/Sprites/UIPlayerWholeLength/";

		private Dictionary<int, HeroLevelupAttributeController> m_attributeNodes = new Dictionary<int, HeroLevelupAttributeController>();

		private Dictionary<int, UIItem> m_costNodes = new Dictionary<int, UIItem>();

		private HeroLevelUpDataModule m_heroLevelupDataModule;

		public class OpenData
		{
			public int m_tableID;

			public CardData m_cardData;
		}
	}
}
