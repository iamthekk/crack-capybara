using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRelicNormalNode : CustomBehaviour
	{
		public void SetData(Relic_group groupTable, List<RelicData> datas)
		{
			this.m_groupTable = groupTable;
			this.m_datas = datas;
		}

		protected override void OnInit()
		{
			this.m_nodePrefab.SetActive(false);
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				RelicData relicData = this.m_datas[i];
				if (relicData != null)
				{
					UIRelicNormalChildNode uirelicNormalChildNode = Object.Instantiate<UIRelicNormalChildNode>(this.m_nodePrefab);
					uirelicNormalChildNode.transform.SetParentNormal(this.m_content, false);
					uirelicNormalChildNode.SetActive(true);
					uirelicNormalChildNode.SetData(relicData);
					uirelicNormalChildNode.Init();
					this.m_dic[uirelicNormalChildNode.GetObjectInstanceID()] = uirelicNormalChildNode;
				}
			}
			this.m_btn.onClick.AddListener(new UnityAction(this.OnClickBtn));
			this.OnRefreshUI();
		}

		protected override void OnDeInit()
		{
			if (this.m_datas != null)
			{
				this.m_datas.Clear();
			}
			this.m_datas = null;
			this.m_groupTable = null;
			if (this.m_btn != null)
			{
				this.m_btn.onClick.RemoveAllListeners();
			}
			foreach (KeyValuePair<int, UIRelicNormalChildNode> keyValuePair in this.m_dic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_dic.Clear();
		}

		private void OnClickBtn()
		{
			RelicGroupDetailsViewModule.OpenData openData = new RelicGroupDetailsViewModule.OpenData();
			openData.m_goupIO = this.m_groupTable.id;
			GameApp.View.OpenView(ViewName.RelicGroupDetailsViewModule, openData, 1, null, null);
		}

		public void OnRefreshUI()
		{
			if (this.m_groupTable == null || this.m_datas == null)
			{
				return;
			}
			RelicDataModule dataModule = GameApp.Data.GetDataModule(DataName.RelicDataModule);
			if (this.m_titleTxt != null)
			{
				this.m_titleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_groupTable.NameId);
			}
			if (this.m_btnTxt != null)
			{
				this.m_btnTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("6101", new object[]
				{
					dataModule.GetActiveRelicCountByGroup(this.m_groupTable.id),
					this.m_datas.Count
				});
			}
			foreach (KeyValuePair<int, UIRelicNormalChildNode> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.OnRefreshUI();
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_content);
		}

		public List<RelicData> m_datas;

		public Relic_group m_groupTable;

		[SerializeField]
		private CustomText m_titleTxt;

		[SerializeField]
		private CustomButton m_btn;

		[SerializeField]
		private CustomText m_btnTxt;

		[SerializeField]
		private ScrollRect m_scrollRect;

		[SerializeField]
		private RectTransform m_content;

		[SerializeField]
		private UIRelicNormalChildNode m_nodePrefab;

		public Dictionary<int, UIRelicNormalChildNode> m_dic = new Dictionary<int, UIRelicNormalChildNode>();
	}
}
