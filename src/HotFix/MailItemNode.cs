using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Habby.Mail.Data;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MailItemNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_bt.onClick.AddListener(new UnityAction(this.OnClickBt));
			this.m_rewardsScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.m_infoTxt.text = "";
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_bt.onClick.RemoveAllListeners();
			foreach (KeyValuePair<int, MailItemRewardNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas.Clear();
			this.m_data = null;
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			MailReward mailReward = this.m_datas[index];
			if (mailReward == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("item");
			MailItemRewardNode component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<MailItemRewardNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshUI(mailReward, this.m_isRead, index);
			return loopListViewItem;
		}

		private void OnClickBt()
		{
			MailInfoViewModule.OpenData openData = new MailInfoViewModule.OpenData();
			openData.m_mailData = this.m_data;
			GameApp.View.OpenView(ViewName.MailInfoViewModule, openData, 1, null, null);
			GameApp.Mail.GetManager().Control.SendMarkMailAsRead(this.m_data.mailId, this.m_data.mailScope, delegate(MailData resp)
			{
				EventArgsReadMail eventArgsReadMail = new EventArgsReadMail();
				eventArgsReadMail.SetData(resp);
				GameApp.Event.DispatchNow(this, 228, eventArgsReadMail);
			}, false);
		}

		public void RefreshData(MailData data, int index)
		{
			this.m_data = data;
			if (data == null)
			{
				return;
			}
			if (this.m_titleTxt != null)
			{
				this.m_titleTxt.text = data.mailTitle;
			}
			if (this.m_timeTxt != null)
			{
				long effectiveAt = data.GetEffectiveAt();
				long num = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - effectiveAt;
				this.m_timeTxt.text = Singleton<LanguageManager>.Instance.GetGoTime(num);
			}
			bool flag = data.IsHaveReward();
			this.m_isRead = data.IsReadShow();
			if (this.m_iconUnread != null)
			{
				this.m_iconUnread.SetActive(!this.m_isRead);
			}
			if (this.m_iconRead != null)
			{
				this.m_iconRead.SetActive(this.m_isRead);
			}
			bool flag2 = false;
			if (this.m_newTxt != null)
			{
				this.m_newTxt.SetActive(flag2);
			}
			bool flag3 = false;
			if (this.m_readTxt != null)
			{
				this.m_readTxt.SetActive(flag3);
			}
			bool flag4 = flag && data.isReward;
			if (this.m_receivedTxt != null)
			{
				this.m_receivedTxt.SetActive(flag4);
			}
			if ((flag && !flag4) || !this.m_isRead)
			{
				this.m_redNode.Value = 1;
			}
			else
			{
				this.m_redNode.Value = 0;
			}
			if (flag)
			{
				this.m_datas = data.rewards.ToList<MailReward>();
				if (this.m_rewardsScroll != null)
				{
					this.m_rewardsScroll.gameObject.SetActive(true);
					this.m_rewardsScroll.SetListItemCount(data.rewards.Length, true);
					this.m_rewardsScroll.RefreshAllShowItems();
				}
			}
			else
			{
				this.m_datas.Clear();
				if (this.m_rewardsScroll != null)
				{
					this.m_rewardsScroll.SetListItemCount(0, true);
					this.m_rewardsScroll.RefreshAllShowItems();
					this.m_rewardsScroll.gameObject.SetActive(false);
				}
			}
			if (this.m_isRead)
			{
				if (this.m_grays != null)
				{
					this.m_grays.SetUIGray();
				}
			}
			else if (this.m_grays != null)
			{
				this.m_grays.Recovery();
			}
			this.m_imgMask.SetActive(this.m_isRead);
		}

		private const int ContentMaxLength = 40;

		private MailData m_data;

		public CustomButton m_bt;

		public CustomText m_titleTxt;

		public CustomText m_infoTxt;

		public LoopListView2 m_rewardsScroll;

		public CustomText m_timeTxt;

		public UIGrays m_grays;

		public GameObject m_iconUnread;

		public GameObject m_iconRead;

		public RedNodeOneCtrl m_redNode;

		public GameObject m_imgMask;

		public GameObject m_newTxt;

		public GameObject m_readTxt;

		public GameObject m_receivedTxt;

		private Dictionary<int, MailItemRewardNode> m_nodes = new Dictionary<int, MailItemRewardNode>();

		[NonSerialized]
		public bool m_isRead;

		[NonSerialized]
		public List<MailReward> m_datas = new List<MailReward>();
	}
}
