using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework.Logic.UI;
using HotFix.ChatUI.ChatShareItemUI;
using SuperScrollView;

namespace HotFix
{
	public class ChatShareItemViewModule : ChatProxy.ChatUIBase
	{
		protected override void OnViewCreate()
		{
			this.LoopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.SwitchGroup.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchGroup);
		}

		protected override void OnViewOpen(object data)
		{
			this.SwitchGroup.ChooseButtonName("Equip");
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			this.uiPopCommon.OnClick = null;
			this.SwitchGroup = null;
			foreach (KeyValuePair<int, ChatShareItemCell> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUIItemDic.Clear();
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			ChatShareItemData chatShareItemData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = this.LoopGridView.NewListViewItem("ShareItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			ChatShareItemCell chatShareItemCell = this.TryGetUI(instanceID);
			if (chatShareItemCell == null)
			{
				chatShareItemCell = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<ChatShareItemCell>());
			}
			chatShareItemCell.SetData(chatShareItemData);
			chatShareItemCell.RefreshUI();
			return loopGridViewItem;
		}

		private ChatShareItemCell TryGetUI(int key)
		{
			ChatShareItemCell chatShareItemCell;
			if (this.mUIItemDic.TryGetValue(key, out chatShareItemCell))
			{
				return chatShareItemCell;
			}
			return null;
		}

		private ChatShareItemCell TryAddUI(int key, LoopGridViewItem loopitem, ChatShareItemCell ui)
		{
			ui.Init();
			ChatShareItemCell chatShareItemCell;
			if (this.mUIItemDic.TryGetValue(key, out chatShareItemCell))
			{
				if (chatShareItemCell == null)
				{
					chatShareItemCell = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickThisView();
			}
		}

		private void OnClickThisView()
		{
			ChatProxy.UI.CloseChatShareView();
		}

		private void OnSwitchGroup(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			string name = button.name;
			if (!(name == "Equip"))
			{
				if (!(name == "Pet"))
				{
					if (name == "Hero")
					{
						this.mDataList = ChatProxy.UI.BuildChatShareHeroDataList();
					}
				}
				else
				{
					this.mDataList = ChatProxy.UI.BuildChatSharePetDataList();
				}
			}
			else
			{
				this.mDataList = ChatProxy.UI.BuildChatShareEquipDataList();
			}
			this.LoopGridView.SetListItemCount(this.mDataList.Count, true);
			this.LoopGridView.RefreshAllShownItem();
		}

		public LoopGridView LoopGridView;

		public UIPopCommon uiPopCommon;

		public CustomChooseButtonGroup SwitchGroup;

		private List<ChatShareItemData> mDataList = new List<ChatShareItemData>();

		private Dictionary<int, ChatShareItemCell> mUIItemDic = new Dictionary<int, ChatShareItemCell>();
	}
}
