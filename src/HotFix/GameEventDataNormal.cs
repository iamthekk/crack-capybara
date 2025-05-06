using System;
using System.Collections.Generic;

namespace HotFix
{
	public class GameEventDataNormal : GameEventData
	{
		public string title
		{
			get
			{
				if (string.IsNullOrEmpty(this.titleId))
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.titleId);
			}
		}

		public string summary
		{
			get
			{
				if (string.IsNullOrEmpty(this.summaryId))
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.summaryId);
			}
		}

		public string content
		{
			get
			{
				if (string.IsNullOrEmpty(this.contentId))
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.contentId);
			}
		}

		public bool isServerDrop { get; private set; }

		public GameEventDataNormal(GameEventPoolData poolData, string titleId, string summaryId, string contentId, List<NodeAttParam> list, List<NodeItemParam> items, bool isDrop)
		{
			this.poolData = poolData;
			this.titleId = titleId;
			this.summaryId = summaryId;
			this.contentId = contentId;
			this.paramList = list;
			this.itemList = items;
			this.isServerDrop = isDrop;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Normal;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count > 0 && index < this.children.Count)
			{
				GameEventData gameEventData = this.children[index];
				while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
				{
					gameEventData = gameEventData.GetNext(0);
				}
				return gameEventData;
			}
			return null;
		}

		public override string GetInfo()
		{
			return this.content;
		}

		public List<NodeAttParam> GetParamList()
		{
			return this.paramList;
		}

		public List<NodeItemParam> GetItemList()
		{
			return this.itemList;
		}

		public string titleId;

		public string summaryId;

		public string contentId;

		private List<NodeAttParam> paramList;

		private List<NodeItemParam> itemList;
	}
}
