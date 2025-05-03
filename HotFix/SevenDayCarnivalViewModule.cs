using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.SevenDayTask;
using UnityEngine;

namespace HotFix
{
	public class SevenDayCarnivalViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this._carnivalDataModule = GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule);
			for (int i = 0; i < this.m_ViewTabs.Count; i++)
			{
				SevenDayCarnivalViewModule.ViewTab viewTab = this.m_ViewTabs[i];
				int index = i;
				viewTab.button.onClick.AddListener(delegate
				{
					this.OnClickViewTab(index);
				});
			}
			this.InfoButton.m_onClick = new Action(this.OnInfoButtonClick);
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.BookMarkGroup.OnBookMarkSwitch = new Func<int, bool>(this.OnMarkClick);
			this._taskPool = LocalUnityObjctPool.Create(base.transform.GetChild(0).gameObject);
			this._taskPool.CreateCache<CarnivalTaskOneCtrl>(this.CopyCarnivalOne.gameObject);
			this.CopyCarnivalOne.gameObject.SetActive(false);
			this._initIndex = Mathf.Clamp(this._carnivalDataModule.UnLockDay, 1, this.BookMarkGroup.MaxBookMarkCount);
			this._viewTabIndex = 0;
			this._viewTab = this.m_ViewTabs[this._viewTabIndex];
			this.BookMarkGroup.InitBookMarkBar(this._viewTab.viewType, this._initIndex);
			this.ActiveProgress.Init();
			this.ActiveProgress.OnSendGetActive = new Action<int, int>(this.Send_Get_Active);
			this.ItemFly.Init();
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this.m_ViewTabs.Count; i++)
			{
				this.m_ViewTabs[i].button.onClick.RemoveAllListeners();
			}
			this.InfoButton.m_onClick = null;
			this.PopCommon.OnClick = null;
			this.BookMarkGroup.OnBookMarkSwitch = null;
			this.BookMarkGroup.DelInit();
			this.ActiveProgress.OnSendGetActive = null;
			this.ActiveProgress.DeInit();
			this.ItemFly.DeInit();
		}

		public override void OnOpen(object data)
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.OnSelectViewTab(0);
			this.BookMarkGroup.ClearSelect();
			this.HideItems();
			if (this._carnivalDataModule.IFGetCarnivalSeverData())
			{
				this.RefreshCountdown();
				this._activeDay = this._initIndex;
				this.BookMarkGroup.ResfreshBookMarks(this._viewTab.viewType, this._activeDay);
				this.ActiveProgress.RefreshActiveProgress(this._carnivalDataModule.ActivePower);
				this.ActiveProgress.RefreshActiveItemsAnim();
				this.ShowItems();
				this.LoadingView.gameObject.SetActive(false);
			}
			else
			{
				this.LoadingView.gameObject.SetActive(true);
				this._carnivalDataModule.RequestCarnivalGetInfo(delegate
				{
					this.RefreshCountdown();
					this._activeDay = this._initIndex;
					this.BookMarkGroup.ResfreshBookMarks(this._viewTab.viewType, this._activeDay);
					this.ActiveProgress.RefreshActiveProgress(this._carnivalDataModule.ActivePower);
					this.ActiveProgress.RefreshActiveItemsAnim();
					this.ShowItems();
					this.LoadingView.gameObject.SetActive(false);
				}, delegate(int errorId)
				{
					this.LoadingView.gameObject.SetActive(false);
					GameApp.View.CloseView(ViewName.SevenDayCarnivalViewModule, null);
				});
			}
			this.FreshViewTabsRedNode();
			GlobalUpdater.Instance.RegisterUpdater(new Action(this.RefreshCountdown));
		}

		public override void OnClose()
		{
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.RefreshCountdown));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.ActiveProgress.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_SevenDayCarnival_Refresh, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshTasks, new HandlerEvent(this.OnEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CarnivalData_RedNode_Refresh, new HandlerEvent(this.OnEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveBar, new HandlerEvent(this.OnEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveItemState, new HandlerEvent(this.OnEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveBarBoxState, new HandlerEvent(this.OnEvent));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_SevenDayCarnival_Refresh, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshTasks, new HandlerEvent(this.OnEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CarnivalData_RedNode_Refresh, new HandlerEvent(this.OnEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveBar, new HandlerEvent(this.OnEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveItemState, new HandlerEvent(this.OnEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CarnivalPanel_RefreshActiveBarBoxState, new HandlerEvent(this.OnEvent));
		}

		private void OnEventRefresh(object sender, int type, BaseEventArgs eventargs)
		{
			if (this._carnivalDataModule.IfTimeOut)
			{
				this.OnCloseSelf();
				return;
			}
			this.RefreshCountdown();
			if (this._activeDay > this._carnivalDataModule.UnLockDay)
			{
				this._activeDay = Utility.Math.Clamp(this._carnivalDataModule.UnLockDay, 1, this.BookMarkGroup.MaxBookMarkCount);
			}
			this.BookMarkGroup.ClearSelect();
			this.BookMarkGroup.ResfreshBookMarks(this._viewTab.viewType, this._activeDay);
			this.ActiveProgress.RefreshActiveProgress(this._carnivalDataModule.ActivePower);
			this.ActiveProgress.RefreshActiveItemsAnim();
			this.RefreshTasks(this._activeDay, true);
		}

		private void OnEvent(object sender, int type, BaseEventArgs eventargs)
		{
			if (this._carnivalDataModule.IfTimeOut)
			{
				this.OnCloseSelf();
				return;
			}
			switch (type)
			{
			case 270:
			{
				int num = (eventargs as EventArgsInt).Value;
				this.RefreshCountdown();
				if (num == 0)
				{
					num = this._activeDay;
				}
				this.RefreshTasks(num, false);
				return;
			}
			case 271:
			{
				int value = (eventargs as EventArgsInt).Value;
				this.ItemFly.Fly(value);
				this.ActiveProgress.PlayProgress(this._carnivalDataModule.ActivePower - value, this._carnivalDataModule.ActivePower);
				return;
			}
			case 272:
				this.ActiveProgress.RefreshActiveItemsState();
				return;
			case 273:
				this.ActiveProgress.RefreshActiveItemsAnim();
				return;
			case 274:
				break;
			case 275:
			{
				int num2 = (eventargs as EventArgsInt).Value;
				if (num2 == 0)
				{
					num2 = this._activeDay;
				}
				this.BookMarkGroup.RefreshBookMarksRedNode(num2);
				this.FreshViewTabsRedNode();
				break;
			}
			default:
				return;
			}
		}

		public void OnClickViewTab(int index)
		{
			if (this._viewTabIndex == index)
			{
				return;
			}
			this.OnSelectViewTab(index);
		}

		private void OnInfoButtonClick()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
			openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID("9510101");
			openData.m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID("9510102");
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.SevenDayCarnivalViewModule, null);
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnCloseSelf();
		}

		private bool OnMarkClick(int markIndex)
		{
			if (!this.isUnlock(markIndex))
			{
				return false;
			}
			this._activeDay = markIndex;
			this.RefreshTasks(markIndex, true);
			return true;
		}

		private void Send_Get_Active(int id, int selectedIndex)
		{
			this._carnivalDataModule.RequestGetCarnivalActiveReward(id, selectedIndex, delegate(SevenDayTaskActiveRewardResponse resp)
			{
				this._carnivalDataModule.IfCanShowRedNode = false;
				if (resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				}
			}, delegate(int errorMsg)
			{
			});
		}

		private void OnSelectViewTab(int index)
		{
			if (index < 0 || index >= this.m_ViewTabs.Count)
			{
				return;
			}
			if (this._viewTabIndex != index)
			{
				this._viewTabIndex = index;
				this._viewTab = this.m_ViewTabs[index];
				this.CarnivalContent.localPosition = Vector2.zero;
			}
			SevenDayCarnivalDataModule.ViewType viewType = this._viewTab.viewType;
			if (viewType != SevenDayCarnivalDataModule.ViewType.Reward)
			{
				if (viewType == SevenDayCarnivalDataModule.ViewType.Pay)
				{
					this.SetViewTab(this._viewTab);
				}
			}
			else
			{
				this.SetViewTab(this._viewTab);
			}
			this.BookMarkGroup.SetViewType(this._viewTab.viewType);
			this.RefreshTasks(this._activeDay, true);
			this.BookMarkGroup.RefreshBookMarksRedNode(0);
		}

		private void SetViewTab(SevenDayCarnivalViewModule.ViewTab viewTab)
		{
			for (int i = 0; i < this.m_ViewTabs.Count; i++)
			{
				SevenDayCarnivalViewModule.ViewTab viewTab2 = this.m_ViewTabs[i];
				if (viewTab2 != null)
				{
					viewTab2.button.SetSelect(viewTab2.button == viewTab.button);
				}
			}
		}

		private void HideItems()
		{
			this.CarnivalContent.gameObject.SetActive(false);
			this.BookMarkGroup.gameObject.SetActive(false);
			this.ActiveProgress.gameObject.SetActive(false);
		}

		private void ShowItems()
		{
			this.CarnivalContent.gameObject.SetActive(true);
			this.BookMarkGroup.gameObject.SetActive(true);
			this.ActiveProgress.gameObject.SetActive(true);
		}

		private void RefreshCountdown()
		{
			if (this == null || base.gameObject == null)
			{
				return;
			}
			string text = DxxTools.FormatFullTimeWithDay(Utility.Math.Max(this._carnivalDataModule.EndTimeStamp - DxxTools.Time.ServerTimestamp, 0L));
			this.txtCountdown.text = Singleton<LanguageManager>.Instance.GetInfoByID("seven_day_carnival_remaintime", new object[] { text });
			if (this._carnivalDataModule.IfTimeOut)
			{
				this.OnCloseSelf();
			}
		}

		private void RefreshTasks(int day, bool bIsOpAnim = true)
		{
			this._taskPool.Collect<CarnivalTaskOneCtrl>();
			if (this._carnivalDataModule.TaskDic.ContainsKey(day))
			{
				int num = 0;
				SevenDayCarnivalDataModule.ViewType viewType = this.m_ViewTabs[this._viewTabIndex].viewType;
				if (viewType != SevenDayCarnivalDataModule.ViewType.Reward)
				{
					if (viewType != SevenDayCarnivalDataModule.ViewType.Pay)
					{
						goto IL_01DD;
					}
				}
				else
				{
					using (List<CarnivalTaskData>.Enumerator enumerator = this._carnivalDataModule.TaskDic[day].TaskDatas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CarnivalTaskData carnivalTaskData = enumerator.Current;
							CarnivalTaskOneCtrl carnivalTaskOneCtrl = this._taskPool.DeQueue<CarnivalTaskOneCtrl>();
							carnivalTaskOneCtrl.SetActive(true);
							carnivalTaskOneCtrl.RefreshRewardData(carnivalTaskData, num, bIsOpAnim);
							carnivalTaskOneCtrl.gameObject.SetParentNormal(this.CarnivalContent, false);
							num++;
						}
						goto IL_01DD;
					}
				}
				IList<SevenDay_SevenDayPay> allElements = GameApp.Table.GetManager().GetSevenDay_SevenDayPayModelInstance().GetAllElements();
				List<CarnivalTaskOneCtrl.PayData> list = new List<CarnivalTaskOneCtrl.PayData>();
				for (int i = 0; i < allElements.Count; i++)
				{
					SevenDay_SevenDayPay sevenDay_SevenDayPay = allElements[i];
					if (sevenDay_SevenDayPay.Day == day)
					{
						CarnivalTaskOneCtrl.PayData payData = new CarnivalTaskOneCtrl.PayData();
						payData.cfg = sevenDay_SevenDayPay;
						payData.buyCount = SevenDayCarnivalDataModule.GetTaskOneBuyCount(this._carnivalDataModule, this.iapDataModule, sevenDay_SevenDayPay);
						payData.finished = sevenDay_SevenDayPay.objToplimit > 0 && (ulong)payData.buyCount >= (ulong)((long)sevenDay_SevenDayPay.objToplimit);
						payData.isFree = SevenDayCarnivalDataModule.GetTaskOneBuyPrice(sevenDay_SevenDayPay) <= 0f;
						payData.bIsOpAnim = bIsOpAnim;
						list.Add(payData);
					}
				}
				list.Sort(new Comparison<CarnivalTaskOneCtrl.PayData>(CarnivalTaskOneCtrl.PayData.Compare));
				for (int j = 0; j < list.Count; j++)
				{
					CarnivalTaskOneCtrl carnivalTaskOneCtrl2 = this._taskPool.DeQueue<CarnivalTaskOneCtrl>();
					carnivalTaskOneCtrl2.SetActive(true);
					carnivalTaskOneCtrl2.RefreshPayData(num, list[j]);
					carnivalTaskOneCtrl2.gameObject.SetParentNormal(this.CarnivalContent, false);
					num++;
				}
			}
			IL_01DD:
			this.CarnivalContent.anchoredPosition = new Vector2(this.CarnivalContent.anchoredPosition.x, 0f);
		}

		private bool isUnlock(int markIndex)
		{
			return markIndex <= this._carnivalDataModule.UnLockDay;
		}

		private void FreshViewTabsRedNode()
		{
			for (int i = 0; i < this.m_ViewTabs.Count; i++)
			{
				SevenDayCarnivalViewModule.ViewTab viewTab = this.m_ViewTabs[i];
				bool flag = false;
				for (int j = 0; j < this.BookMarkGroup.MaxBookMarkCount; j++)
				{
					if (SevenDayCarnivalDataModule.IsTaskOneShowRed(this._carnivalDataModule, viewTab.viewType, j + 1))
					{
						flag = true;
						break;
					}
				}
				viewTab.redNodeCtrl.gameObject.SetActive(flag);
			}
		}

		[Header("页卡")]
		[SerializeField]
		private List<SevenDayCarnivalViewModule.ViewTab> m_ViewTabs = new List<SevenDayCarnivalViewModule.ViewTab>();

		[Header("倒计时")]
		[SerializeField]
		private CustomButton InfoButton;

		[SerializeField]
		private CustomText txtCountdown;

		[Header("进度条")]
		[SerializeField]
		private CarnivalActiveProgressCtrl ActiveProgress;

		[Header("嘉年华任务")]
		[SerializeField]
		private RectTransform CarnivalContent;

		[SerializeField]
		private CarnivalTaskOneCtrl CopyCarnivalOne;

		[Header("侧边分页按钮")]
		[SerializeField]
		private CarnivalBookMarkGroup BookMarkGroup;

		[Header("PopCommon")]
		[SerializeField]
		private UIPopCommon PopCommon;

		[Header("Loading")]
		[SerializeField]
		private RectTransform LoadingView;

		[Header("ItemFly")]
		[SerializeField]
		private UIFlyCtrl ItemFly;

		private int _viewTabIndex;

		private int _initIndex;

		private int _activeDay;

		private SevenDayCarnivalViewModule.ViewTab _viewTab;

		private LocalUnityObjctPool _taskPool;

		private SevenDayCarnivalDataModule _carnivalDataModule;

		private IAPDataModule iapDataModule;

		[Serializable]
		public class ViewTab
		{
			public SevenDayCarnivalDataModule.ViewType viewType;

			public CustomChooseButton button;

			public RedNodeOneCtrl redNodeCtrl;
		}
	}
}
