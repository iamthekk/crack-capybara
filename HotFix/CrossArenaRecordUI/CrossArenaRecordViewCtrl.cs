using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Proto.CrossArena;
using Proto.User;
using SuperScrollView;
using UnityEngine;

namespace HotFix.CrossArenaRecordUI
{
	public class CrossArenaRecordViewCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.Obj_NetLoading.SetActive(false);
			this.Obj_EmptyList.SetActive(false);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, CrossArenaRecordItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			CrossArenaChallengeRecord crossArenaChallengeRecord = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("CorssArenaRecord_Item");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			CrossArenaRecordItem crossArenaRecordItem = this.TryGetUI(instanceID);
			CrossArenaRecordItem component = loopListViewItem.GetComponent<CrossArenaRecordItem>();
			if (crossArenaRecordItem == null)
			{
				crossArenaRecordItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			crossArenaRecordItem.SetData(crossArenaChallengeRecord);
			crossArenaRecordItem.SetActive(true);
			crossArenaRecordItem.RefreshUI();
			return loopListViewItem;
		}

		private CrossArenaRecordItem TryGetUI(int key)
		{
			CrossArenaRecordItem crossArenaRecordItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRecordItem))
			{
				return crossArenaRecordItem;
			}
			return null;
		}

		private CrossArenaRecordItem TryAddUI(int key, LoopListViewItem2 loopitem, CrossArenaRecordItem ui)
		{
			ui.Init();
			ui.OnJumpToBattle = new Action<CrossArenaChallengeRecord>(this.OnGetBattleRecord);
			CrossArenaRecordItem crossArenaRecordItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRecordItem))
			{
				if (crossArenaRecordItem == null)
				{
					crossArenaRecordItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void OnViewOpen(object data)
		{
			this.mIsEnterBattle = false;
			this.mDataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.mDataList.Clear();
			this.RefreshList();
			this.Obj_NetLoading.SetActive(true);
			this.Obj_EmptyList.SetActive(false);
			this.m_seqPool.Clear(false);
			NetworkUtils.CrossArena.DoCrossArenaRecordRequest(delegate(bool result, CrossArenaRecordResponse resp)
			{
				if (this.mView == null || !this.mView.isActiveAndEnabled)
				{
					return;
				}
				this.Obj_NetLoading.SetActive(false);
				if (result)
				{
					this.mDataList.Clear();
					this.mDataList.AddRange(CrossArenaChallengeRecord.ToList(resp.Records));
					this.mDataList.Sort(new Comparison<CrossArenaChallengeRecord>(CrossArenaChallengeRecord.SortByTime));
					this.RefreshList();
				}
			});
		}

		public void SetView(CrossArenaRecordViewModule view)
		{
			this.mView = view;
		}

		public bool IsViewOpen()
		{
			return this.mView != null && this.mView.isActiveAndEnabled && base.gameObject != null && base.gameObject.activeSelf;
		}

		private void OnCloseThis()
		{
			if (this.mView != null)
			{
				GameApp.View.CloseView(this.mView.GetName(), null);
			}
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnCloseThis();
		}

		private void RefreshList()
		{
			this.m_seqPool.Clear(false);
			this.Scroll.SetListItemCount(this.mDataList.Count, true);
			this.Scroll.RefreshAllShowItems();
			this.Obj_EmptyList.SetActive(this.mDataList.Count <= 0);
			this.PlayScale();
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.3f, 9);
		}

		private void OnGetBattleRecord(CrossArenaChallengeRecord data)
		{
			if (this.mIsEnterBattle)
			{
				return;
			}
			if (data == null || this.mView == null || !this.mView.isActiveAndEnabled)
			{
				return;
			}
			this.mIsEnterBattle = true;
			NetworkUtils.User.DoUserGetBattleReportRequest((ulong)data.ReportID, delegate(bool result, UserGetBattleReportResponse resp)
			{
				if (result)
				{
					this.JumpToBattle(resp);
					return;
				}
				this.mIsEnterBattle = false;
			});
		}

		public void JumpToBattle(UserGetBattleReportResponse resp)
		{
			if (resp == null || this.mView == null || !this.mView.isActiveAndEnabled)
			{
				return;
			}
			EventArgsBattleCrossArenaEnter instance = Singleton<EventArgsBattleCrossArenaEnter>.Instance;
			instance.SetData(resp.Record, true);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleCrossArena_BattleCrossArenaEnter, instance);
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					this.mIsEnterBattle = false;
					EventArgsRefreshMainOpenData instance2 = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance2.SetData(DxxTools.UI.GetCrossArenaRecordOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance2);
					if (GameApp.View.IsOpened(ViewName.CrossArenaChallengeViewModule))
					{
						GameApp.View.CloseView(ViewName.CrossArenaChallengeViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.CrossArenaViewModule))
					{
						GameApp.View.CloseView(ViewName.CrossArenaViewModule, null);
					}
					EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
					instance3.SetData(GameModel.CrossArena, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance3);
					GameApp.State.ActiveState(StateName.BattleCrossArenaState);
				});
			});
		}

		public UIPopCommon PopCommon;

		public LoopListView2 Scroll;

		public GameObject Obj_NetLoading;

		public GameObject Obj_EmptyList;

		private List<CrossArenaChallengeRecord> mDataList = new List<CrossArenaChallengeRecord>();

		public Dictionary<int, CrossArenaRecordItem> UICtrlDic = new Dictionary<int, CrossArenaRecordItem>();

		private SequencePool m_seqPool = new SequencePool();

		private CrossArenaDataModule mDataModule;

		private CrossArenaRecordViewModule mView;

		private bool mIsEnterBattle;
	}
}
