using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class SocialityInteractivePanelGroup : BaseSocialityPanel
	{
		protected override void OnInit()
		{
			this.m_socialityDataModule = GameApp.Data.GetDataModule(DataName.SocialityDataModule);
			this.m_unNodeGroup.SetActive(false);
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, SocialityInteractiveNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas.Clear();
			this.m_socialityDataModule = null;
		}

		public override void OnShow()
		{
			this.m_datas.Clear();
			this.m_netLoading.SetActive(this.m_datas.Count == 0);
			this.m_unNodeGroup.SetActive(false);
			this.m_scroll.SetListItemCount(this.m_datas.Count, true);
			this.m_scroll.RefreshAllShowItems();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshInteractiveData, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_CheckLoadingInteractiveData, null);
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshInteractiveData, new HandlerEvent(this.OnRefreshUI));
			this.m_sequencePool.Clear(false);
		}

		private void OnRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.m_datas.Clear();
			for (int i = 0; i < this.m_socialityDataModule.m_interacts.Count; i++)
			{
				SocialityInteractiveData socialityInteractiveData = this.m_socialityDataModule.m_interacts[i];
				if (socialityInteractiveData != null)
				{
					this.m_datas.Add(socialityInteractiveData);
				}
			}
			IOrderedEnumerable<SocialityInteractiveData> orderedEnumerable = from dto in this.m_datas
				orderby dto.m_status, dto.m_duration
				select dto;
			this.m_datas = orderedEnumerable.ToList<SocialityInteractiveData>();
			bool activeSelf = this.m_netLoading.activeSelf;
			this.m_netLoading.SetActive(false);
			if (this.m_datas.Count > 0)
			{
				this.m_unNodeGroup.SetActive(false);
				this.m_scroll.gameObject.SetActive(true);
				this.m_scroll.SetListItemCount(this.m_datas.Count, true);
				this.m_scroll.RefreshAllShowItems();
				if (activeSelf)
				{
					this.PlayScale();
					return;
				}
			}
			else
			{
				this.m_unNodeGroup.SetActive(true);
				this.m_scroll.gameObject.SetActive(false);
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			SocialityInteractiveData socialityInteractiveData = this.m_datas[index];
			if (socialityInteractiveData == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Node");
			SocialityInteractiveNode component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.GetComponent<SocialityInteractiveNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshData(socialityInteractiveData, index);
			return loopListViewItem;
		}

		private void PlayScale()
		{
			this.m_sequencePool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_sequencePool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		public LoopListView2 m_scroll;

		public GameObject m_unNodeGroup;

		public GameObject m_netLoading;

		private List<SocialityInteractiveData> m_datas = new List<SocialityInteractiveData>();

		private Dictionary<int, SocialityInteractiveNode> m_nodes = new Dictionary<int, SocialityInteractiveNode>();

		private SequencePool m_sequencePool = new SequencePool();

		private SocialityDataModule m_socialityDataModule;
	}
}
