using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using LocalModels.Bean;

namespace HotFix
{
	public class TalentLegacyTreeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.NodeItemList.Count; i++)
			{
				this.NodeItemList[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.NodeItemList.Count; i++)
			{
				this.NodeItemList[i].DeInit();
			}
		}

		public void OnShow()
		{
			for (int i = 0; i < this.NodeItemList.Count; i++)
			{
				this.NodeItemList[i].OnShow();
			}
		}

		public void OnClose()
		{
			for (int i = 0; i < this.NodeItemList.Count; i++)
			{
				this.NodeItemList[i].OnClose();
			}
		}

		public void SetData(int rowIndex, int careerId, List<TalentLegacy_talentLegacyNode> nodeCfgList)
		{
			this.m_rowIndex = rowIndex;
			this.m_careerId = careerId;
			if (nodeCfgList.Count <= 0)
			{
				for (int i = 0; i < this.NodeItemList.Count; i++)
				{
					this.NodeItemList[i].gameObject.SetActiveSafe(false);
				}
				return;
			}
			for (int j = 0; j < this.NodeItemList.Count; j++)
			{
				this.NodeItemList[j].gameObject.SetActiveSafe(false);
			}
			for (int k = 0; k < this.NodeItemList.Count; k++)
			{
				for (int l = 0; l < nodeCfgList.Count; l++)
				{
					int num = int.Parse(nodeCfgList[l].pos[1]);
					if (k + 1 == num)
					{
						this.NodeItemList[k].gameObject.SetActiveSafe(true);
						this.NodeItemList[k].SetData(careerId, nodeCfgList[l].id, true, true, true);
					}
				}
			}
		}

		public List<TalentLegacyNodeItem> NodeItemList = new List<TalentLegacyNodeItem>();

		private int m_rowIndex;

		private int m_careerId;

		private TalentLegacy_talentLegacyNode m_NodeCfg;
	}
}
