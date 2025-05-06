using System;
using Framework;
using HotFix;
using LocalModels.Bean;

namespace Dxx.Chat
{
	public class ChatEmojiData
	{
		public string Atlas
		{
			get
			{
				if (this.Tab == null)
				{
					return "";
				}
				return GameApp.Table.GetAtlasPath(this.Tab.atlasId);
			}
		}

		public string Icon
		{
			get
			{
				if (this.Tab == null)
				{
					return "";
				}
				return this.Tab.icon;
			}
		}

		public void SetTable(Emoji_Emoji tab)
		{
			this.ID = tab.id;
			this.Path = tab.path;
			this.Tab = tab;
		}

		public void SetTableByID(int id)
		{
			this.ID = id;
			Emoji_Emoji emojiTab = ChatProxy.Table.GetEmojiTab(id);
			if (emojiTab != null)
			{
				this.Path = emojiTab.path;
				this.Tab = emojiTab;
			}
		}

		public string MakeEmojiContent()
		{
			return string.Format("[#{0}]", this.ID);
		}

		public int ID;

		public int Group;

		public string Path;

		public Emoji_Emoji Tab;
	}
}
