using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework.Logic.UI;
using Framework.Logic.UI.Chat;
using HotFix.ChatUI.ChatItemUI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.ChatUI
{
	public class ChatOutputMiniUI : GuildProxy.GuildProxy_BaseBehaviour, IChatGroupDataChange
	{
		protected override void GuildUI_OnInit()
		{
			this.ShowDataList.Clear();
			this.ShowDataList.Add(this.mEmptyListData);
			this.LoopListUI.InitListView(this.ShowDataList.Count, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Button_View.onClick.AddListener(new UnityAction(this.OnClickView));
			this.ItemSizeWidth = this.LoopListUI.ContainerTrans.rect.width;
			this.mItemSizeCtrl = new ChatOutputItemSizeCtrl();
			this.mItemSizeCtrl.LoopListUI = this.LoopListUI;
			this.Button_Chat.onClick.AddListener(new UnityAction(this.OnOpenChatUI));
		}

		protected override void GuildUI_OnShow()
		{
			this.m_seqPool.Clear(false);
			if (this.ChatGroup != null)
			{
				this.ChatGroup.AddDataListener(this);
			}
			this.RefreshAllUI();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mItemSizeCtrl != null)
			{
				this.mItemSizeCtrl.Update(Time.deltaTime);
			}
			this.OnUpdateCheckScrollToEnd();
		}

		protected override void GuildUI_OnClose()
		{
			this.m_seqPool.Clear(false);
			if (this.ChatGroup != null)
			{
				this.ChatGroup.RemoveDataListener(this);
			}
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.Button_View != null)
			{
				this.Button_View.onClick.RemoveListener(new UnityAction(this.OnClickView));
			}
			if (this.Button_Chat != null)
			{
				this.Button_Chat.onClick.RemoveListener(new UnityAction(this.OnOpenChatUI));
			}
			foreach (KeyValuePair<int, ChatItemBase> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		public void RefreshAllUI()
		{
		}

		private void SetScrollSize()
		{
			this.LoopListUI.SetListItemCount(this.ShowDataList.Count, false);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.ShowDataList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			ChatData chatData = this.ShowDataList[index];
			ChatItemBase chatItemBase = null;
			string text = "Other";
			switch (chatData.ShowKind)
			{
			case ChatShowKind.CustomText:
			case ChatShowKind.Emoji:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_" + text);
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_CustomChat>());
				}
				break;
			}
			case ChatShowKind.SystemTips:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_System");
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_System>());
				}
				break;
			}
			case ChatShowKind.TimeShow:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_Time");
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_Time>());
				}
				break;
			}
			case ChatShowKind.Loading:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_Loading");
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_Loading>());
					chatItemBase.gameObject.GetComponent<ChatItem_LoadingMono>().SetScroll(this.LoopListUI, loopListViewItem);
				}
				break;
			}
			case ChatShowKind.EmptyList:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_EmptyList");
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_EmptyList>());
				}
				chatItemBase.ExtureHeight = (int)this.LoopListUI.ViewPortHeight;
				break;
			}
			case ChatShowKind.MultipleSystemTips:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_MultipleSystem");
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_MultipleSystem>());
				}
				break;
			}
			case ChatShowKind.ItemShare:
			{
				loopListViewItem = listView.NewListViewItem("Chat_Item_Share_" + text);
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_GuildShareItem>());
				}
				break;
			}
			case ChatShowKind.GuildDonation:
			{
				if (chatData.IsMyChat)
				{
					text = "Me";
				}
				else
				{
					text = "Other";
				}
				loopListViewItem = listView.NewListViewItem("Chat_Item_Donation_" + text);
				int num = loopListViewItem.gameObject.GetInstanceID();
				chatItemBase = this.TryGetUI(num);
				if (chatItemBase == null)
				{
					chatItemBase = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<ChatItem_GuildDonation>());
				}
				break;
			}
			}
			if (chatItemBase != null)
			{
				chatItemBase.ExtureHeight = 20;
				chatItemBase.ChatGroupID = this.ChatGroupID;
				chatItemBase.ChatGroupType = SocketGroupType.GUILD;
				chatItemBase.SetData(chatData);
				chatItemBase.RefreshUI();
				chatItemBase.RefreshSize();
				if (index == this.ShowDataList.Count)
				{
					this.mItemSizeCtrl.SetBottomItem(chatItemBase);
					this.mItemSizeCtrl.Update(0f);
				}
			}
			return loopListViewItem;
		}

		private ChatItemBase TryGetUI(int key)
		{
			ChatItemBase chatItemBase;
			if (this.UICtrlDic.TryGetValue(key, out chatItemBase))
			{
				return chatItemBase;
			}
			return null;
		}

		private ChatItemBase TryAddUI(int key, LoopListViewItem2 loopitem, ChatItemBase ui)
		{
			if (ui == null)
			{
				HLog.LogError("ChatOutputMiniUI : TryAddUI ui is null! " + ((loopitem != null) ? loopitem.name : null));
				return null;
			}
			ui.LoopItem = loopitem;
			ui.RootMaxWidth = (int)this.ItemSizeWidth;
			ui.OnSizeChange = new Action<ChatItemBase>(this.OnItemUISizeChange);
			ui.Init();
			ChatItemBase chatItemBase;
			if (this.UICtrlDic.TryGetValue(key, out chatItemBase))
			{
				if (chatItemBase == null)
				{
					chatItemBase = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void OnSwitchChannel(string groupid, bool force)
		{
			if (this.ChatGroupID == groupid && !force)
			{
				return;
			}
			if (this.ChatGroupID != groupid)
			{
				if (this.ChatGroup != null)
				{
					this.ChatGroup.RemoveDataListener(this);
				}
				this.ChatGroupID = groupid;
				this.ChatGroup = Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.GUILD, this.ChatGroupID);
				if (this.ChatGroup != null)
				{
					this.ChatGroup.AddDataListener(this);
				}
			}
			if (this.ChatGroup != null)
			{
				this.InsertMoreMessageToList(this.ChatGroup.ChatMessageList, true);
				return;
			}
			this.InsertMoreMessageToList(null, true);
		}

		private void OpenMoveToBottom()
		{
		}

		public void OnUpdateCheckScrollToEnd()
		{
			if (this.LoopListUI.ScrollRect.verticalNormalizedPosition > 1E-05f)
			{
				float num = this.LoopListUI.ScrollRect.verticalNormalizedPosition;
				num = Mathf.Lerp(num, 0f, 0.8f);
				if (num <= 1E-05f)
				{
					num = 0f;
				}
				this.LoopListUI.ScrollRect.verticalNormalizedPosition = num;
			}
		}

		private void OnClickView()
		{
			if (this.mViewClickTime > DateTime.Now)
			{
				return;
			}
			if ((DateTime.Now - this.mViewClickTime).TotalMilliseconds > 500.0)
			{
				this.mViewClickTime = DateTime.Now;
				return;
			}
			this.mViewClickTime = DateTime.Now.AddSeconds(1.0);
			this.OnOpenChatUI();
		}

		private void OnOpenChatUI()
		{
			Action onClickViewArea = this.OnClickViewArea;
			if (onClickViewArea == null)
			{
				return;
			}
			onClickViewArea();
		}

		private void InsertMoreMessageToList(IList<ChatData> datalist, bool clear = false)
		{
			if (clear)
			{
				this.ShowDataList.Clear();
			}
			ChatOutputUI.MsgListMergeResult msgListMergeResult;
			this.AutoMergeMessageList(datalist, out msgListMergeResult);
			this.SetScrollSize();
		}

		private void AutoMergeMessageList(IList<ChatData> datalist, out ChatOutputUI.MsgListMergeResult mergeresult)
		{
			mergeresult = ChatOutputUI.MsgListMergeResult.None;
			bool flag = false;
			bool flag2 = false;
			List<ChatData> showDataList = this.ShowDataList;
			if (datalist == null)
			{
				datalist = new List<ChatData>();
			}
			double totalSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
			if (showDataList.Count > 0 && showDataList[0].ShowKind == ChatShowKind.EmptyList)
			{
				showDataList.RemoveAt(0);
			}
			List<ChatData> list = new List<ChatData>();
			for (int i = 0; i < datalist.Count; i++)
			{
				ChatData chatData = datalist[i];
				if (chatData != null)
				{
					if (showDataList.Count == 0)
					{
						ChatData chatData2 = ChatData.TestCreateTimePartData(chatData, 0L);
						if (chatData2 != null)
						{
							showDataList.Add(chatData2);
						}
						showDataList.Add(chatData);
						flag2 = true;
					}
					else
					{
						ChatData chatData3 = showDataList[0];
						if (chatData.timestamp <= chatData3.timestamp)
						{
							if (!chatData3.IsSameMessageID(chatData))
							{
								list.Insert(0, chatData);
							}
						}
						else
						{
							ChatData chatData4 = showDataList[showDataList.Count - 1];
							if (chatData.timestamp >= chatData4.timestamp)
							{
								if (!chatData4.IsSameMessageID(chatData))
								{
									ChatData chatData5 = ChatData.TestCreateTimePartData(chatData, chatData4.timestamp);
									if (chatData5 != null)
									{
										showDataList.Add(chatData5);
									}
									showDataList.Add(chatData);
									flag = true;
								}
							}
							else
							{
								for (int j = 1; j < showDataList.Count; j++)
								{
									ChatData chatData6 = showDataList[j - 1];
									if (chatData6.IsSameMessageID(chatData))
									{
										break;
									}
									ChatData chatData7 = showDataList[j];
									if (chatData7.IsSameMessageID(chatData))
									{
										break;
									}
									if (chatData.msgId < chatData6.msgId && chatData.msgId >= chatData7.msgId)
									{
										showDataList.Insert(j, chatData);
										HLog.LogError("消息体 向中间插入消息？？？");
										break;
									}
								}
							}
						}
					}
				}
			}
			list.Sort(new Comparison<ChatData>(ChatData.SortByTime));
			if (list.Count > 0)
			{
				ChatData chatData8 = ChatData.TestCreateTimePartData(list[0], 0L);
				if (chatData8 != null)
				{
					list.Insert(0, chatData8);
				}
			}
			int num = 1;
			while (num + 1 < list.Count)
			{
				ChatData chatData9 = list[num];
				ChatData chatData10 = ChatData.TestCreateTimePartData(list[num + 1], chatData9.timestamp);
				if (chatData10 != null)
				{
					list.Insert(num + 1, chatData10);
					num++;
				}
				num++;
			}
			if (list.Count > 0)
			{
				flag2 = true;
				showDataList.InsertRange(0, list);
			}
			if (showDataList.Count <= 0)
			{
				showDataList.Add(this.mEmptyListData);
			}
			if (flag)
			{
				mergeresult |= ChatOutputUI.MsgListMergeResult.HasNew;
			}
			if (flag2)
			{
				mergeresult |= ChatOutputUI.MsgListMergeResult.HasHistory;
			}
		}

		public void OnItemUISizeChange(ChatItemBase ui)
		{
			if (ui == null || this.LoopListUI == null)
			{
				return;
			}
			LoopListViewItem2 loopItem = ui.GetLoopItem();
			if (loopItem == null)
			{
				return;
			}
			this.LoopListUI.OnItemSizeChanged(loopItem.ItemIndex);
		}

		public void OnAddChatData(List<ChatData> list)
		{
			if (list == null)
			{
				return;
			}
			this.InsertMoreMessageToList(list, false);
		}

		public void OnChangeChatData(List<ChatData> list)
		{
			this.LoopListUI.RefreshAllShownItem();
		}

		public void OnDelChatData(List<ChatData> list)
		{
			Dictionary<long, ChatData> dictionary = new Dictionary<long, ChatData>();
			for (int i = 0; i < list.Count; i++)
			{
				dictionary[list[i].msgId] = list[i];
			}
			for (int j = 0; j < this.ShowDataList.Count; j++)
			{
				ChatData chatData = this.ShowDataList[j];
				if (dictionary.ContainsKey(chatData.msgId) || dictionary.ContainsKey(chatData.UIBindMsgID))
				{
					this.ShowDataList.RemoveAt(j);
					j--;
				}
			}
			this.SetScrollSize();
		}

		[SerializeField]
		private LoopListView2 LoopListUI;

		[SerializeField]
		private CustomButton Button_View;

		[SerializeField]
		private CustomButton Button_Chat;

		public List<ChatData> ShowDataList = new List<ChatData>();

		public Dictionary<int, ChatItemBase> UICtrlDic = new Dictionary<int, ChatItemBase>();

		public Action OnClickViewArea;

		private string ChatGroupID;

		private ChatGroupData ChatGroup;

		private DateTime mViewClickTime;

		private ChatData mLoadingData = new ChatData
		{
			ShowKind = ChatShowKind.Loading
		};

		private ChatData mEmptyListData = new ChatData
		{
			ShowKind = ChatShowKind.EmptyList
		};

		private float ItemSizeWidth;

		private SequencePool m_seqPool = new SequencePool();

		private ChatOutputItemSizeCtrl mItemSizeCtrl;
	}
}
