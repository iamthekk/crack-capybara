using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using Framework.Logic.UI.Chat;
using HotFix.ChatUI.ChatItemUI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.ChatUI
{
	public class ChatOutputUI : ChatProxy.ChatProxy_BaseBehaviour, IChatGroupDataChange
	{
		private int LoadingIndex
		{
			get
			{
				return this.ShowDataList.Count;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.ChatGroupID = string.Empty;
			this.ChatGroupType = SocketGroupType.UNKNOW;
			this.LoopListUI.mOnBeginDragAction = new Action(this.OnBeginDrag);
			this.LoopListUI.mOnDragingAction = new Action(this.OnDraging);
			this.LoopListUI.mOnEndDragAction = new Action(this.OnEndDrag);
			this.ShowDataList.Clear();
			this.ShowDataList.Add(this.mEmptyListData);
			this.LoopListUI.InitListView(this.ShowDataList.Count, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Button_NewMsg.onClick.AddListener(new UnityAction(this.OnClickScrollToBottom));
			this.Button_View.onClick.AddListener(new UnityAction(this.OnClickView));
			this.ItemSizeWidth = this.LoopListUI.ContainerTrans.rect.width;
			Singleton<ChatManager>.Instance.OnQuitGroup += this.InstanceOnQuitGroup;
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.Button_NewMsg != null)
			{
				this.Button_NewMsg.onClick.RemoveListener(new UnityAction(this.OnClickScrollToBottom));
			}
			if (this.Button_View != null)
			{
				this.Button_View.onClick.RemoveListener(new UnityAction(this.OnClickView));
			}
			Singleton<ChatManager>.Instance.OnQuitGroup -= this.InstanceOnQuitGroup;
		}

		public void OnShow()
		{
			this.LoopListUI.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.m_seqPool.Clear(false);
			this.mOpenToBottom = true;
			if (this.ChatGroup != null)
			{
				this.ChatGroup.RemoveDataListener(this);
				this.ChatGroup.AddDataListener(this);
				this.InsertMoreMessageToList(this.ChatGroup.ChatMessageList, true);
			}
			this.RefreshAllUI(true);
		}

		public void OnUpdate()
		{
			if (this.mNewMessageToBottom)
			{
				float num = this.LoopListUI.ScrollRect.verticalNormalizedPosition;
				num = Mathf.Lerp(num, 0f, 0.6f);
				if (num <= 1E-05f)
				{
					this.mNewMessageToBottom = false;
					num = 0f;
				}
				this.LoopListUI.ScrollRect.verticalNormalizedPosition = num;
			}
		}

		public void OnClose()
		{
			this.LoopListUI.ScrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.m_seqPool.Clear(false);
			if (this.ChatGroup != null)
			{
				this.ChatGroup.RemoveDataListener(this);
			}
		}

		private void InstanceOnQuitGroup(SocketGroupType arg1, string arg2)
		{
			if (this.ChatGroup != null && this.ChatGroup.ChatGroupType == arg1 && this.ChatGroup.ChatGroupID == arg2)
			{
				this.ChatGroup.RemoveDataListener(this);
				this.ChatGroup = null;
				this.ChatGroupID = string.Empty;
				this.ChatGroupType = SocketGroupType.UNKNOW;
			}
		}

		public void RefreshAllUI(bool isMoveToIndex0)
		{
			this.SetScrollSize();
			this.LoopListUI.RefreshAllShownItem();
			if (isMoveToIndex0)
			{
				this.LoopListUI.MovePanelToItemIndex(0, 0f);
			}
		}

		private void SetScrollSize()
		{
			this.LoopListUI.SetListItemCount(this.ShowDataList.Count + 1, false);
		}

		public void ResetListView()
		{
			this.LoopListUI.ResetListView(false);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.ShowDataList.Count + 1)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			ChatData chatData;
			if (index == this.LoadingIndex)
			{
				chatData = this.mLoadingData;
			}
			else
			{
				chatData = this.ShowDataList[this.ShowDataList.Count - 1 - index];
			}
			ChatItemBase chatItemBase = null;
			string text;
			if (chatData.IsMyChat)
			{
				text = "Me";
			}
			else
			{
				text = "Other";
			}
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
				chatItemBase.ChatGroupType = this.ChatGroupType;
				chatItemBase.SetData(chatData);
				chatItemBase.RefreshUI();
				chatItemBase.RefreshSize();
			}
			if (chatData.ShowKind == ChatShowKind.Loading)
			{
				this.UpdateLoadingTip(loopListViewItem);
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
				HLog.LogError("ChatOutputUI : TryAddUI ui is null! " + ((loopitem != null) ? loopitem.name : null));
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

		public void OnSwitchChannel(SocketGroupType groupType, string groupid)
		{
			if (this.ChatGroupType == groupType && this.ChatGroupID == groupid)
			{
				this.OpenMoveToBottom();
				return;
			}
			if (this.ChatGroup != null)
			{
				this.ChatGroup.RemoveDataListener(this);
			}
			this.ChatGroupID = groupid;
			this.ChatGroupType = groupType;
			this.ChatGroup = Singleton<ChatManager>.Instance.GetGroup(this.ChatGroupType, this.ChatGroupID);
			if (this.ChatGroup == null)
			{
				this.InsertMoreMessageToList(null, true);
				return;
			}
			this.ChatGroup.AddDataListener(this);
			this.InsertMoreMessageToList(this.ChatGroup.ChatMessageList, true);
			if (this.ChatGroup.ChatMessageList.Count > 0)
			{
				this.OpenMoveToBottom();
				return;
			}
			ChatHistoryMessageRequire historyRequire = this.HistoryRequire;
			if (historyRequire == null)
			{
				return;
			}
			historyRequire.RequireHistory(null, delegate(bool result)
			{
				this.OpenMoveToBottom();
			});
		}

		private void OpenMoveToBottom()
		{
			if (this.mOpenToBottom)
			{
				this.mOpenToBottom = false;
				this.LoopListUI.MovePanelToItemIndex(0, 0f);
			}
		}

		private void OnClickScrollToBottom()
		{
			this.Button_NewMsg.gameObject.SetActive(false);
			this.LoopListUI.MovePanelToItemIndex(0, 0f);
			this.mNewMessageToBottom = true;
		}

		private void OnClickView()
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
			if (this.ShowDataList.Count > 0)
			{
				ChatData chatData = this.ShowDataList[this.ShowDataList.Count - 1];
			}
			if (this.ShowDataList.Count > 0)
			{
				ChatData chatData2 = this.ShowDataList[0];
			}
			ChatOutputUI.MsgListMergeResult msgListMergeResult;
			this.AutoMergeMessageList(datalist, out msgListMergeResult);
			float verticalNormalizedPosition = this.LoopListUI.ScrollRect.verticalNormalizedPosition;
			this.m_seqPool.Clear(false);
			bool flag = verticalNormalizedPosition > 0.001f && (msgListMergeResult & ChatOutputUI.MsgListMergeResult.HasNew) > ChatOutputUI.MsgListMergeResult.None;
			this.Button_NewMsg.gameObject.SetActive(flag);
			if (!flag)
			{
				this.SetScrollSize();
				this.LoopListUI.RefreshAllShownItem();
			}
			this.RefreshListAfterGetDataList();
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
										break;
									}
									if (chatData.timestamp >= chatData6.timestamp)
									{
										long timestamp = chatData.timestamp;
										long timestamp2 = chatData7.timestamp;
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

		public void OnInputAreaHeightChange(float height)
		{
			if (this.LoopListUI != null && this.mIsScrollAtBottom)
			{
				this.LoopListUI.ScrollRect.verticalNormalizedPosition = 0f;
			}
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

		private void OnBeginDrag()
		{
			this.mNewMessageToBottom = false;
			this.dragPosY = this.LoopListUI.ScrollRect.verticalNormalizedPosition;
		}

		private void OnDraging()
		{
			this.RefreshScrollOnDraging();
		}

		private void OnEndDrag()
		{
			this.RefreshScrollAfterDragEnd();
		}

		private void RefreshScrollOnDraging()
		{
			if (this.LoopListUI.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.None && this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.LoopListUI.GetShownItemByItemIndex(this.LoadingIndex);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.LoopListUI.GetShownItemByItemIndex(this.LoadingIndex - 1);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			float y = this.LoopListUI.GetItemCornerPosInViewPort(shownItemByItemIndex2, 1).y;
			float num = this.LoopListUI.ViewPortSize - y;
			if (this.dragPosY < this.LoopListUI.ScrollRect.verticalNormalizedPosition && num >= ChatItem_Loading.LoadingTipItemHeight)
			{
				if (this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.None)
				{
					return;
				}
				this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.WaitRelease;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.WaitRelease)
				{
					return;
				}
				this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void RefreshScrollAfterDragEnd()
		{
			if (this.LoopListUI.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.None && this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.LoopListUI.GetShownItemByItemIndex(this.LoadingIndex);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.LoopListUI.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (this.mLoadingStatus != ChatOutputUI.ChatLoadingTipStatus.WaitRelease)
			{
				return;
			}
			float y = this.LoopListUI.GetItemCornerPosInViewPort(shownItemByItemIndex, 1).y;
			if (this.LoopListUI.ViewPortSize - y < ChatItem_Loading.LoadingTipItemHeight)
			{
				this.mLoadingStatus = (this.hasallserverdata ? ChatOutputUI.ChatLoadingTipStatus.Loaded : ChatOutputUI.ChatLoadingTipStatus.None);
				this.SetScrollSize();
				this.LoopListUI.RefreshAllShownItem();
				return;
			}
			if (this.hasallserverdata)
			{
				this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.Loaded;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.WaitLoad;
			this.UpdateLoadingTip(shownItemByItemIndex);
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			ChatItem_Loading chatItem_Loading = this.TryGetUI(item.gameObject.GetInstanceID()) as ChatItem_Loading;
			if (chatItem_Loading == null)
			{
				return;
			}
			float loadingTipItemHeight = ChatItem_Loading.LoadingTipItemHeight;
			switch (this.mLoadingStatus)
			{
			case ChatOutputUI.ChatLoadingTipStatus.None:
				chatItem_Loading.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				this.LoopListUI.OnItemSizeChanged(this.LoadingIndex);
				return;
			case ChatOutputUI.ChatLoadingTipStatus.WaitRelease:
				chatItem_Loading.SetActive(true);
				chatItem_Loading.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, loadingTipItemHeight);
				this.LoopListUI.OnItemSizeChanged(this.LoadingIndex);
				return;
			case ChatOutputUI.ChatLoadingTipStatus.WaitLoad:
			case ChatOutputUI.ChatLoadingTipStatus.RequireData:
				chatItem_Loading.SetActive(true);
				chatItem_Loading.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, loadingTipItemHeight);
				this.LoopListUI.OnItemSizeChanged(this.LoadingIndex);
				return;
			case ChatOutputUI.ChatLoadingTipStatus.Loaded:
				chatItem_Loading.SetActive(true);
				chatItem_Loading.SetAsNoMoreData();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				this.LoopListUI.OnItemSizeChanged(this.LoadingIndex);
				return;
			default:
				return;
			}
		}

		private void OnScrollValueChange(Vector2 arg0)
		{
			if (this.LoopListUI.ScrollRect.verticalNormalizedPosition < 0.001f)
			{
				this.Button_NewMsg.gameObject.SetActive(false);
			}
			this.mIsScrollAtBottom = this.LoopListUI.ScrollRect.verticalNormalizedPosition < 0.001f;
			if (this.mLoadingStatus == ChatOutputUI.ChatLoadingTipStatus.WaitLoad && this.LoopListUI.ScrollRect.verticalNormalizedPosition < 1.0001f)
			{
				this.LoopListUI.ScrollRect.verticalNormalizedPosition = 1f;
				this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.RequireData;
				ChatData chatData = null;
				for (int i = 0; i < this.ShowDataList.Count; i++)
				{
					ChatData chatData2 = this.ShowDataList[i];
					if (chatData2 != null && chatData2.ShowKind != ChatShowKind.Loading && chatData2.ShowKind != ChatShowKind.TimeShow && chatData2.ShowKind != ChatShowKind.EmptyList)
					{
						chatData = chatData2;
						break;
					}
				}
				if (this.HistoryRequire != null)
				{
					this.HistoryRequire.RequireHistory(chatData, delegate(bool result)
					{
						this.RefreshListAfterGetDataList();
					});
				}
			}
		}

		private void RefreshListAfterGetDataList()
		{
			if (this.LoopListUI.ShownItemCount == 0)
			{
				this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.None;
				return;
			}
			if (this.mLoadingStatus == ChatOutputUI.ChatLoadingTipStatus.RequireData)
			{
				if (this.hasallserverdata)
				{
					this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.Loaded;
				}
				else
				{
					this.mLoadingStatus = ChatOutputUI.ChatLoadingTipStatus.None;
				}
				this.SetScrollSize();
				this.LoopListUI.RefreshAllShownItem();
			}
		}

		public LoopListView2 LoopListUI;

		public CustomButton Button_NewMsg;

		public CustomButton Button_View;

		public List<ChatData> ShowDataList = new List<ChatData>();

		public Dictionary<int, ChatItemBase> UICtrlDic = new Dictionary<int, ChatItemBase>();

		public ChatHistoryMessageRequire HistoryRequire = new ChatHistoryMessageRequire();

		public Action OnClickViewArea;

		[Label]
		public string ChatGroupID;

		[Label]
		public SocketGroupType ChatGroupType;

		private ChatGroupData ChatGroup;

		private ChatData mLoadingData = new ChatData
		{
			ShowKind = ChatShowKind.Loading
		};

		private ChatData mEmptyListData = new ChatData
		{
			ShowKind = ChatShowKind.EmptyList
		};

		private ChatOutputUI.ChatLoadingTipStatus mLoadingStatus;

		private bool hasallserverdata;

		private float ItemSizeWidth;

		private SequencePool m_seqPool = new SequencePool();

		private bool mIsScrollAtBottom;

		private bool mOpenToBottom;

		private bool mNewMessageToBottom;

		private float dragPosY;

		public enum ChatLoadingTipStatus
		{
			None,
			WaitRelease,
			WaitLoad,
			RequireData,
			Loaded
		}

		public enum MsgListMergeResult
		{
			None,
			HasNew,
			HasHistory
		}
	}
}
