using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildApplyJoinViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.guildApplyJoinItemObj.SetActive(false);
			this.loopLitView2.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.buttonDisagreeAll.onClick.AddListener(new UnityAction(this.DisagreeAll));
			this.buttonLeave.onClick.AddListener(new UnityAction(this.CloseSelfView));
			GuildSDKManager.Instance.Event.RegisterEvent(8, new GuildHandlerEvent(this.OnRefresh));
			this.toggle.isOn = GuildSDKManager.Instance.GuildInfo.GuildData.JoinKind == GuildJoinKind.Free;
			this.toggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				if (((GuildSDKManager.Instance.GuildInfo.GuildData.JoinKind == GuildJoinKind.Free) & isOn) || (GuildSDKManager.Instance.GuildInfo.GuildData.JoinKind == GuildJoinKind.Conditional && !isOn))
				{
					return;
				}
				if (GuildSDKManager.Instance.Permission.MyGuildPosition != GuildPositionType.President)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("guild_applyJoin_autoJoin_permisionNotEnough"));
					this.toggle.isOn = GuildSDKManager.Instance.GuildInfo.GuildData.JoinKind == GuildJoinKind.Free;
					return;
				}
				if (isOn)
				{
					this.AgreeAll();
				}
				GuildCreateData guildCreateData = new GuildCreateData();
				guildCreateData.CloneFromShareData(base.SDK.GuildInfo.GuildData);
				guildCreateData.JoinKind = (isOn ? GuildJoinKind.Free : GuildJoinKind.Conditional);
				GuildNetUtil.Guild.DoRequest_ModifyGuildInfo(guildCreateData, delegate(bool result, GuildModifyResponse response)
				{
					if (result)
					{
						this.toggle.isOn = isOn;
						return;
					}
					this.toggle.isOn = !isOn;
				});
			});
		}

		private void OnRefresh(int type, GuildBaseEvent args)
		{
			if (GuildSDKManager.Instance.GuildInfo.GuildData != null)
			{
				this.toggle.isOn = GuildSDKManager.Instance.GuildInfo.GuildData.JoinKind == GuildJoinKind.Free;
			}
		}

		protected override void OnViewOpen(object data)
		{
			this.m_seqPool.Clear(false);
			this.OnOpenGetApplyJoinList();
		}

		protected override void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			if (this.buttonDisagreeAll != null)
			{
				this.buttonDisagreeAll.onClick.RemoveListener(new UnityAction(this.DisagreeAll));
			}
			GuildSDKManager.Instance.Event.UnRegisterEvent(23, new GuildHandlerEvent(this.OnRefresh));
			this.DeInitAllScrollUI();
		}

		private void OnOpenGetApplyJoinList()
		{
			if (!base.CheckIsViewOpen())
			{
				return;
			}
			List<GuildUserShareData> playerDataApplyJoin = GuildSDKManager.Instance.GuildInfo.PlayerDataApplyJoin;
			this.guildApplyJoinDataList.Clear();
			this.guildApplyJoinDataList.AddRange(playerDataApplyJoin);
			this.loopLitView2.SetListItemCount(playerDataApplyJoin.Count, true);
			this.loopLitView2.RefreshAllShowItems();
			if (playerDataApplyJoin.Count == 0)
			{
				this.noBodyText.SetActiveSafe(true);
				return;
			}
			this.noBodyText.SetActiveSafe(false);
		}

		private List<long> GetAllUserIDList()
		{
			List<long> list = new List<long>();
			for (int i = 0; i < this.guildApplyJoinDataList.Count; i++)
			{
				GuildUserShareData guildUserShareData = this.guildApplyJoinDataList[i];
				if (guildUserShareData != null && guildUserShareData.UserID != 0L)
				{
					list.Add(guildUserShareData.UserID);
				}
			}
			return list;
		}

		private void AgreeOne(UIGuildApplyJoinItem item)
		{
			if (item == null || item.UserID == 0L)
			{
				return;
			}
			Action <>9__1;
			GuildNetUtil.Guild.DoRequest_AgreeJoinGuild(new List<long> { item.UserID }, delegate(bool result, GuildAgreeJoinResponse response)
			{
				if (result)
				{
					GuildApplyJoinViewModule <>4__this = this;
					UIGuildApplyJoinItem item2 = item;
					float num = 0f;
					bool flag = true;
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate
						{
							this.DeleteOneAndForceRefreshList(item);
						});
					}
					<>4__this.PlayItemOut(item2, num, flag, action);
				}
			});
		}

		private void AgreeAll()
		{
			List<long> allUserIDList = this.GetAllUserIDList();
			if (allUserIDList.Count == 0)
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_AgreeJoinGuild(allUserIDList, delegate(bool result, GuildAgreeJoinResponse response)
			{
				if (result)
				{
					this.PlayAllItemOut(null);
				}
			});
		}

		private void DisagreeOne(UIGuildApplyJoinItem item)
		{
			if (item == null || item.UserID == 0L)
			{
				return;
			}
			Action <>9__1;
			GuildNetUtil.Guild.DoRequest_RefuseJoinGuild(new List<long> { item.UserID }, delegate(bool result, GuildRefuseJoinResponse response)
			{
				if (item != null)
				{
					GuildApplyJoinViewModule <>4__this = this;
					UIGuildApplyJoinItem item2 = item;
					float num = 0f;
					bool flag = true;
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate
						{
							this.DeleteOneAndForceRefreshList(item);
						});
					}
					<>4__this.PlayItemOut(item2, num, flag, action);
				}
			});
		}

		private void DeleteOneAndForceRefreshList(UIGuildApplyJoinItem item)
		{
			if (base.CheckIsViewOpen())
			{
				List<GuildUserShareData> list = new List<GuildUserShareData>();
				list.AddRange(this.guildApplyJoinDataList);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].UserID == item.UserID)
					{
						list.RemoveAt(i);
						break;
					}
				}
				this.guildApplyJoinDataList.Clear();
				this.guildApplyJoinDataList.AddRange(list);
				this.loopLitView2.SetListItemCount(this.guildApplyJoinDataList.Count, true);
				this.loopLitView2.RefreshAllShowItems();
				if (list.Count == 0)
				{
					this.noBodyText.SetActiveSafe(true);
					return;
				}
				this.noBodyText.SetActiveSafe(false);
			}
		}

		private void DisagreeAll()
		{
			List<long> userids = this.GetAllUserIDList();
			if (userids.Count == 0)
			{
				return;
			}
			Action <>9__2;
			Action<bool, GuildRefuseJoinResponse> <>9__1;
			DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("guild_disagreeAll_confirm"), delegate(int yes)
			{
				if (yes == 1)
				{
					List<long> userids2 = userids;
					Action<bool, GuildRefuseJoinResponse> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(bool result, GuildRefuseJoinResponse response)
						{
							if (result)
							{
								GuildApplyJoinViewModule <>4__this = this;
								Action action2;
								if ((action2 = <>9__2) == null)
								{
									action2 = (<>9__2 = delegate
									{
										this.noBodyText.SetActiveSafe(true);
										List<GuildUserShareData> list = new List<GuildUserShareData>();
										for (int i = 0; i < this.guildApplyJoinDataList.Count; i++)
										{
											if (!userids.Contains(this.guildApplyJoinDataList[i].UserID))
											{
												list.Add(this.guildApplyJoinDataList[i]);
											}
										}
										this.guildApplyJoinDataList.Clear();
										this.guildApplyJoinDataList.AddRange(list);
									});
								}
								<>4__this.PlayAllItemOut(action2);
							}
						});
					}
					GuildNetUtil.Guild.DoRequest_RefuseJoinGuild(userids2, action);
				}
			});
		}

		private void PlayAllItemOut(Action callback)
		{
			this.m_seqPool.Clear(false);
			float num = 0f;
			List<int> list = this.itemDic.Keys.ToList<int>();
			list.Sort((int a, int b) => a.CompareTo(b));
			for (int i = 0; i < list.Count; i++)
			{
				int num2 = list[i];
				UIGuildApplyJoinItem uiguildApplyJoinItem = this.itemDic[num2];
				if (!(uiguildApplyJoinItem == null) && !(uiguildApplyJoinItem.gameObject == null) && uiguildApplyJoinItem.gameObject.activeSelf)
				{
					if (num + 0.4f >= 2f)
					{
						break;
					}
					this.PlayItemOut(uiguildApplyJoinItem, num, false, null);
					num += 0.1f;
				}
			}
			Sequence sequence = this.m_seqPool.Get();
			float num3 = num + 0.4f;
			if (num3 > 2f)
			{
				num3 = 2f;
			}
			TweenSettingsExtensions.AppendInterval(sequence, num3);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
		}

		private void PlayItemOut(UIGuildApplyJoinItem item, float delay, bool hide, Action callback)
		{
			item.PlayItemOut(this.m_seqPool, delay, hide, callback);
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildApplyJoinViewModule, null);
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType obj)
		{
			this.CloseSelfView();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.guildApplyJoinDataList.Count)
			{
				return null;
			}
			GuildUserShareData guildUserShareData = this.guildApplyJoinDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGuildApplyJoinItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildApplyJoinItem uiguildApplyJoinItem = this.TryGetUI(instanceID);
			UIGuildApplyJoinItem component = loopListViewItem.GetComponent<UIGuildApplyJoinItem>();
			if (uiguildApplyJoinItem == null)
			{
				uiguildApplyJoinItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uiguildApplyJoinItem.OnClickAgree = new Action<UIGuildApplyJoinItem>(this.AgreeOne);
			uiguildApplyJoinItem.Refresh(guildUserShareData);
			return loopListViewItem;
		}

		private UIGuildApplyJoinItem TryGetUI(int key)
		{
			UIGuildApplyJoinItem uiguildApplyJoinItem;
			if (this.itemDic.TryGetValue(key, out uiguildApplyJoinItem))
			{
				return uiguildApplyJoinItem;
			}
			return null;
		}

		private UIGuildApplyJoinItem TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildApplyJoinItem ui)
		{
			ui.Init();
			UIGuildApplyJoinItem uiguildApplyJoinItem;
			if (this.itemDic.TryGetValue(key, out uiguildApplyJoinItem))
			{
				if (uiguildApplyJoinItem == null)
				{
					uiguildApplyJoinItem = ui;
					this.itemDic[key] = ui;
				}
				return ui;
			}
			this.itemDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UIGuildApplyJoinItem> keyValuePair in this.itemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.itemDic.Clear();
		}

		public UIPopCommon popCommon;

		public CustomButton buttonDisagreeAll;

		public CustomButton buttonLeave;

		public Toggle toggle;

		public UIGuildApplyJoinItem guildApplyJoinItemObj;

		public LoopListView2 loopLitView2;

		public GameObject noBodyText;

		private readonly List<GuildUserShareData> guildApplyJoinDataList = new List<GuildUserShareData>();

		private Dictionary<int, UIGuildApplyJoinItem> itemDic = new Dictionary<int, UIGuildApplyJoinItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
