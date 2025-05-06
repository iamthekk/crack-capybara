using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainMainEquipRelic : BaseMainEquipPanel
	{
		protected override void OnInit()
		{
			this.m_relicDataModule = GameApp.Data.GetDataModule(DataName.RelicDataModule);
			this.m_normalNodePrefab.SetActive(false);
			this.m_specialNodePrefab.SetActive(false);
		}

		public override void OnShow()
		{
			IList<Relic_group> allElements = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Relic_group relic_group = allElements[i];
				if (relic_group != null)
				{
					List<RelicData> relicDatasByGroup = this.m_relicDataModule.GetRelicDatasByGroup(relic_group.id);
					if (relic_group.type == 1)
					{
						UIRelicNormalNode uirelicNormalNode = Object.Instantiate<UIRelicNormalNode>(this.m_normalNodePrefab);
						uirelicNormalNode.transform.SetParentNormal(this.m_content, false);
						uirelicNormalNode.SetActive(true);
						uirelicNormalNode.SetData(relic_group, relicDatasByGroup);
						uirelicNormalNode.Init();
						this.m_dic[uirelicNormalNode.GetObjectInstanceID()] = uirelicNormalNode;
					}
					else if (relic_group.type == 2)
					{
						UIRelicSpecialNode uirelicSpecialNode = Object.Instantiate<UIRelicSpecialNode>(this.m_specialNodePrefab);
						uirelicSpecialNode.transform.SetParentNormal(this.m_content, false);
						uirelicSpecialNode.SetActive(true);
						uirelicSpecialNode.SetData(relic_group, relicDatasByGroup);
						uirelicSpecialNode.Init();
						this.m_dic[uirelicSpecialNode.GetObjectInstanceID()] = uirelicSpecialNode;
					}
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_content);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventRefreshAll));
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventRefreshAll));
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_dic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_dic.Clear();
		}

		public override void PlayAnimation()
		{
		}

		protected override void OnDeInit()
		{
			this.m_relicDataModule = null;
		}

		private void OnEventRefreshAll(object sender, int type, BaseEventArgs eventargs)
		{
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null))
				{
					UIRelicNormalNode uirelicNormalNode = keyValuePair.Value as UIRelicNormalNode;
					if (uirelicNormalNode != null)
					{
						uirelicNormalNode.OnRefreshUI();
					}
					else
					{
						UIRelicSpecialNode uirelicSpecialNode = keyValuePair.Value as UIRelicSpecialNode;
						if (uirelicSpecialNode != null)
						{
							uirelicSpecialNode.OnRefreshUI();
						}
					}
				}
			}
		}

		[SerializeField]
		private ScrollRect m_scrollRect;

		[SerializeField]
		private RectTransform m_content;

		[SerializeField]
		private UIRelicNormalNode m_normalNodePrefab;

		[SerializeField]
		private UIRelicSpecialNode m_specialNodePrefab;

		public Dictionary<int, CustomBehaviour> m_dic = new Dictionary<int, CustomBehaviour>();

		private RelicDataModule m_relicDataModule;
	}
}
