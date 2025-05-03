using System;
using System.Collections.Generic;
using System.Linq;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildCheckSimplePopViewModule : GuildProxy.GuildProxy_BaseView
	{
		private bool IsViewOpen
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		protected override void OnViewCreate()
		{
		}

		protected override void OnViewOpen(object data)
		{
			this.m_openData = data as GuildCheckSimplePopViewModule.OpenData;
			if (this.m_openData == null)
			{
				return;
			}
			this.m_guildIcon.Init();
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.m_netLoading.SetActive(true);
			this.m_content.SetActive(false);
			GuildNetUtil.Guild.DoRequest_GetGuildDetailInfo(this.m_openData.m_guildID, delegate(bool result, GuildGetDetailResponse response)
			{
				if (!this.IsViewOpen)
				{
					return;
				}
				if (result)
				{
					this.m_guildDetailData = response.GuildDetailInfoDto.ToGuildDetailData();
					List<GuildUserShareData> list = (from dto in this.m_guildDetailData.Members
						orderby dto.Power descending, dto.GuildPosition
						select dto).ToList<GuildUserShareData>();
					this.m_memberDatas.Clear();
					int num = 3;
					for (int i = 0; i < list.Count; i++)
					{
						GuildUserShareData guildUserShareData = list[i];
						if (guildUserShareData != null)
						{
							this.m_memberDatas.Add(guildUserShareData);
							num--;
							if (num <= 0)
							{
								break;
							}
						}
					}
					this.RefreshUI();
				}
			}, false);
			this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
		}

		protected override void OnViewClose()
		{
			this.m_maskBt.onClick.RemoveAllListeners();
			this.m_closeBt.onClick.RemoveAllListeners();
			this.m_sequencePool.Clear(false);
			this.m_guildIcon.DeInit();
			foreach (KeyValuePair<int, GuildCheckSimplePopMemberNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_memberDatas.Clear();
			this.m_openData = null;
			this.m_guildDetailData = null;
		}

		protected override void OnViewDelete()
		{
		}

		private void RefreshUI()
		{
			if (this.m_guildDetailData == null)
			{
				return;
			}
			if (this.m_netLoading != null)
			{
				this.m_netLoading.SetActive(false);
			}
			if (this.m_content != null)
			{
				this.m_content.SetActive(true);
			}
			GuildShareData shareData = this.m_guildDetailData.ShareData;
			if (shareData == null)
			{
				return;
			}
			if (this.m_guildIcon != null)
			{
				this.m_guildIcon.SetIcon(shareData.GuildIcon);
			}
			if (this.m_guildNameTxt != null)
			{
				this.m_guildNameTxt.text = shareData.GuildShowName;
			}
			if (this.m_guildPowerTxt != null)
			{
				this.m_guildPowerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("400273", new object[] { DxxTools.FormatNumber(shareData.GuildPower) });
			}
			if (this.m_guildLevelTxt != null)
			{
				this.m_guildLevelTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("400272", new object[] { shareData.GuildLevel });
			}
			this.m_scroll.SetListItemCount(this.m_memberDatas.Count, true);
			this.m_scroll.RefreshAllShowItems();
			this.PlayScale();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			GuildUserShareData guildUserShareData = this.m_memberDatas[index];
			if (guildUserShareData == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("MemberItem");
			GuildCheckSimplePopMemberNode component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.GetComponent<GuildCheckSimplePopMemberNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshData(guildUserShareData, index);
			return loopListViewItem;
		}

		private void PlayScale()
		{
			this.m_sequencePool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_sequencePool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.GuildCheckSimplePopViewModule, null);
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBt;

		public GameObject m_netLoading;

		public GameObject m_content;

		public UIGuildIcon m_guildIcon;

		public CustomText m_guildNameTxt;

		public CustomText m_guildPowerTxt;

		public CustomText m_guildLevelTxt;

		public LoopListView2 m_scroll;

		private SequencePool m_sequencePool = new SequencePool();

		public List<GuildUserShareData> m_memberDatas = new List<GuildUserShareData>();

		private Dictionary<int, GuildCheckSimplePopMemberNode> m_nodes = new Dictionary<int, GuildCheckSimplePopMemberNode>();

		private GuildCheckSimplePopViewModule.OpenData m_openData;

		private GuildShareDetailData m_guildDetailData;

		public class OpenData
		{
			public void SetData(string guildID)
			{
				this.m_guildID = guildID;
			}

			public string m_guildID;
		}
	}
}
