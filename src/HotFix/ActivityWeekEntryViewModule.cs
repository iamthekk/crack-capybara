using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using LocalModels.Model;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class ActivityWeekEntryViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Obj_NetLoading.SetActive(false);
			this.buttonClose.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.buttonRushShop.m_onClick = new Action(this.OnClickRushShop);
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.buttonRushShop.m_onClick = null;
			this.entryNodeCtrls.Clear();
			foreach (ActivityEntryNodeCtrl activityEntryNodeCtrl in this.entryNodeCacheList)
			{
				activityEntryNodeCtrl.btn_Entry.m_onClick = null;
				activityEntryNodeCtrl.DeInit();
				Object.Destroy(activityEntryNodeCtrl.gameObject);
			}
			this.entryNodeCacheList.Clear();
		}

		public override void OnOpen(object data)
		{
			this.SetBgVisible(true);
			this.Obj_NetLoading.SetActive(false);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_title");
			this.textShop.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_shop");
			this.contentRoot.SetActiveSafe(true);
			this.actDataModule = GameApp.Data.GetDataModule(DataName.ActivityWeekDataModule);
			this.actCommonTab = GameApp.Table.GetManager().GetCommonActivity_CommonActivityModelInstance();
			this.RefreshEntryNodes();
		}

		public override void OnClose()
		{
			this.actCommonTab = null;
			this.actDataModule = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = this.entryNodeCtrls.Count - 1; i >= 0; i--)
			{
				ActivityEntryNodeCtrl activityEntryNodeCtrl = this.entryNodeCtrls[i];
				if (!activityEntryNodeCtrl.isEnd)
				{
					this.UpdateActEndTime(activityEntryNodeCtrl);
				}
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_ActivityWeek_DataPull, new HandlerEvent(this.OnEventDayChanged));
			manager.RegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnViewOpen));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_ActivityWeek_DataPull, new HandlerEvent(this.OnEventDayChanged));
			manager.UnRegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnViewOpen));
		}

		private void OnViewOpen(object sender, int type, BaseEventArgs eventargs)
		{
			this.Obj_NetLoading.SetActive(false);
			this.RefreshEntryNodes();
		}

		private void RefreshEntryNodes()
		{
			foreach (ActivityEntryNodeCtrl activityEntryNodeCtrl in this.entryNodeCacheList)
			{
				activityEntryNodeCtrl.gameObject.SetActive(false);
			}
			this.entryNodeCtrls.Clear();
			this.actIds.Clear();
			long svrtime = DxxTools.Time.ServerTimestamp;
			if (this.actDataModule.DropData != null)
			{
				foreach (Drop drop in this.actDataModule.DropData)
				{
					long num = drop.TimeBase.ETime - svrtime;
					if (num > 0L && drop.TimeBase.STime <= svrtime)
					{
						int actId = drop.TimeBase.ActId;
						this.CreateActItem(3, actId, drop.TimeBase, num);
					}
				}
			}
			if (this.actDataModule.ChapterData != null)
			{
				foreach (Chapter chapter in this.actDataModule.ChapterData)
				{
					long num2 = chapter.TimeBase.ETime - svrtime;
					if (num2 > 0L && chapter.TimeBase.STime <= svrtime)
					{
						int actId2 = chapter.TimeBase.ActId;
						this.CreateActItem(5, actId2, chapter.TimeBase, num2);
					}
				}
			}
			if (this.actDataModule.ConsumeData != null)
			{
				foreach (Consume consume in this.actDataModule.ConsumeData)
				{
					long num3 = consume.TimeBase.ETime - svrtime;
					if (num3 > 0L && consume.TimeBase.STime <= svrtime)
					{
						int actId3 = consume.TimeBase.ActId;
						this.CreateActItem(1, actId3, consume.TimeBase, num3);
					}
				}
			}
			if (this.actDataModule.ShopData != null)
			{
				foreach (Shop shop in this.actDataModule.ShopData)
				{
					long num4 = shop.TimeBase.ETime - svrtime;
					if (num4 > 0L && shop.TimeBase.STime <= svrtime)
					{
						int actId4 = shop.TimeBase.ActId;
						this.CreateActItem(2, actId4, shop.TimeBase, num4);
					}
				}
			}
			if (this.actDataModule.PayData != null)
			{
				foreach (Pay pay in this.actDataModule.PayData)
				{
					long num5 = pay.TimeBase.ETime - svrtime;
					if (num5 > 0L && pay.TimeBase.STime <= svrtime)
					{
						int actId5 = pay.TimeBase.ActId;
						this.CreateActItem(4, actId5, pay.TimeBase, num5);
					}
				}
			}
			this.entryNodeCtrls.Sort(delegate(ActivityEntryNodeCtrl a, ActivityEntryNodeCtrl b)
			{
				if ((a.endTime - svrtime) * (b.endTime - svrtime) < 0L)
				{
					return -a.endTime.CompareTo(b.endTime);
				}
				if (a.actCfg.SortID != b.actCfg.SortID)
				{
					return a.actCfg.SortID.CompareTo(b.actCfg.SortID);
				}
				return a.actId - b.actId;
			});
			for (int i = 0; i < this.entryNodeCtrls.Count; i++)
			{
				this.entryNodeCtrls[i].rectTransform.SetSiblingIndex(i);
			}
			this.CheckEmpty();
		}

		private void CreateActItem(int actType, int actId, TimeBase timeBase, long leftTime)
		{
			CommonActivity_CommonActivity elementById = this.actCommonTab.GetElementById(actId);
			if (elementById == null || elementById.Type != actType || this.actIds.Contains(actId))
			{
				return;
			}
			this.actIds.Add(actId);
			int count = this.entryNodeCtrls.Count;
			ActivityEntryNodeCtrl item;
			if (count < this.entryNodeCacheList.Count)
			{
				item = this.entryNodeCacheList[count];
			}
			else
			{
				item = Object.Instantiate<ActivityEntryNodeCtrl>(this.entryNodeCtrlPrefab, this.entryNodeCtrlParent);
				this.entryNodeCacheList.Add(item);
				item.Init();
			}
			if (!item.gameObject.activeSelf)
			{
				item.gameObject.SetActive(true);
			}
			this.entryNodeCtrls.Add(item);
			item.actId = actId;
			item.actCfg = elementById;
			item.btn_Entry.m_onClick = delegate
			{
				this.OnClickEntryNode(item);
			};
			item.icon_Entry.SetImage(elementById.atlasID, elementById.banner);
			item.txt_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.Name);
			DateTime dateTime = DxxTools.Time.UnixTimestampToServerLocalDateTime((double)timeBase.STime);
			DateTime dateTime2 = DxxTools.Time.UnixTimestampToServerLocalDateTime((double)timeBase.ETime);
			item.txt_Duration.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_durantiontime") + DxxTools.DataTimeToLocalMothDay(dateTime, "MM/dd hh:mm") + " - " + DxxTools.DataTimeToLocalMothDay(dateTime2, "MM/dd hh:mm");
			item.endTime = timeBase.ETime;
			item.isEnd = false;
			this.UpdateActEndTime(item);
			if (this.actDataModule.ShowRed(actId, this.actCommonTab))
			{
				item.redNodeOneCtrl.gameObject.SetActiveSafe(true);
				item.redNodeOneCtrl.SetType(240);
				item.redNodeOneCtrl.Value = 1;
				return;
			}
			item.redNodeOneCtrl.gameObject.SetActiveSafe(false);
		}

		private void UpdateActEndTime(ActivityEntryNodeCtrl item)
		{
			long num = item.endTime - DxxTools.Time.ServerTimestamp;
			if (num > 0L)
			{
				item.txt_Left.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_lefttime", new object[] { DxxTools.FormatFullTimeWithDay(num) });
				return;
			}
			item.isEnd = true;
			item.txt_Left.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_end");
			this.entryNodeCtrls.Remove(item);
			item.gameObject.SetActive(false);
			this.CheckEmpty();
		}

		private void CheckEmpty()
		{
			this.emptyTipObj.SetActiveSafe(this.entryNodeCtrls.Count == 0);
		}

		public void SetBgVisible(bool visible)
		{
			this.bgObj.SetActiveSafe(visible);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ActivityWeekEntryModule, null);
		}

		private void OnClickRushShop()
		{
			GameApp.View.OpenView(ViewName.ActivityShopModule, null, 1, null, null);
		}

		private void OnClickEntryNode(ActivityEntryNodeCtrl item)
		{
			GameApp.View.OpenView(ViewName.ActivityWeekViewModule, item.actId, 1, null, null);
		}

		private void OnEventDayChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("9100009"));
			if (GameApp.View.IsOpened(ViewName.ActivityWeekViewModule))
			{
				GameApp.View.CloseView(ViewName.ActivityWeekViewModule, null);
			}
			this.OnClickClose();
		}

		public CustomButton buttonMask;

		public GameObject bgObj;

		public GameObject contentRoot;

		public CustomText textTitle;

		public CustomText textShop;

		public CustomButton buttonClose;

		public CustomButton buttonRushShop;

		public GameObject emptyTipObj;

		public RectTransform entryNodeCtrlParent;

		public ActivityEntryNodeCtrl entryNodeCtrlPrefab;

		private List<ActivityEntryNodeCtrl> entryNodeCtrls = new List<ActivityEntryNodeCtrl>();

		private List<ActivityEntryNodeCtrl> entryNodeCacheList = new List<ActivityEntryNodeCtrl>();

		public GameObject Obj_NetLoading;

		private CommonActivity_CommonActivityModel actCommonTab;

		private ActivityWeekDataModule actDataModule;

		private HashSet<int> actIds = new HashSet<int>();

		private ActivityWeekViewModule.ActViewType[] redViewTypes = new ActivityWeekViewModule.ActViewType[]
		{
			ActivityWeekViewModule.ActViewType.ConsumeType,
			ActivityWeekViewModule.ActViewType.ShopType,
			ActivityWeekViewModule.ActViewType.PayType
		};
	}
}
