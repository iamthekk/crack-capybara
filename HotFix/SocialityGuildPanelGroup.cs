using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class SocialityGuildPanelGroup : BaseSocialityPanel
	{
		protected override void OnInit()
		{
			this.m_socialityDataModule = GameApp.Data.GetDataModule(DataName.SocialityDataModule);
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, SocialityGuildNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas.Clear();
			this.m_socialityDataModule = null;
			this.m_loginDataModule = null;
		}

		public override void OnShow()
		{
			if (this.m_socialityDataModule.IsHaveGuild)
			{
				this.m_unAddGuild.SetActive(false);
				this.m_scroll.gameObject.SetActive(true);
				this.m_datas.Clear();
				this.m_netLoading.SetActive(true);
				this.m_scroll.SetListItemCount(this.m_datas.Count, true);
				this.m_scroll.RefreshAllShowItems();
				GameApp.Event.RegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshGuildData, new HandlerEvent(this.OnRefreshUI));
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_CheckLoadingGuildData, null);
				return;
			}
			this.m_netLoading.SetActive(false);
			this.m_unAddGuild.SetActive(true);
			this.m_scroll.gameObject.SetActive(false);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshGuildData, new HandlerEvent(this.OnRefreshUI));
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshGuildData, new HandlerEvent(this.OnRefreshUI));
			this.m_sequencePool.Clear(false);
		}

		private void OnRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.m_datas.Clear();
			for (int i = 0; i < this.m_socialityDataModule.m_guilds.Count; i++)
			{
				GuildMemberInfoDto guildMemberInfoDto = this.m_socialityDataModule.m_guilds[i];
				if (guildMemberInfoDto != null)
				{
					this.m_datas.Add(guildMemberInfoDto);
				}
			}
			bool activeSelf = this.m_netLoading.activeSelf;
			this.m_netLoading.SetActive(false);
			this.m_scroll.SetListItemCount(this.m_datas.Count, true);
			this.m_scroll.RefreshAllShowItems();
			if (activeSelf)
			{
				this.PlayScale();
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			GuildMemberInfoDto guildMemberInfoDto = this.m_datas[index];
			if (guildMemberInfoDto == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Node");
			SocialityGuildNode component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.GetComponent<SocialityGuildNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshData(guildMemberInfoDto, index);
			return loopListViewItem;
		}

		private void PlayScale()
		{
			this.m_sequencePool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_sequencePool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		public LoopListView2 m_scroll;

		public GameObject m_unAddGuild;

		public GameObject m_netLoading;

		private List<GuildMemberInfoDto> m_datas = new List<GuildMemberInfoDto>();

		private Dictionary<int, SocialityGuildNode> m_nodes = new Dictionary<int, SocialityGuildNode>();

		private SequencePool m_sequencePool = new SequencePool();

		private SocialityDataModule m_socialityDataModule;

		private LoginDataModule m_loginDataModule;
	}
}
