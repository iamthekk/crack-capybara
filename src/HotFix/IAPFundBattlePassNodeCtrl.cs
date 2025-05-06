using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using SuperScrollView;
using UnityEngine.UI;

namespace HotFix
{
	public class IAPFundBattlePassNodeCtrl : IAPFundCtrlBase
	{
		private IAPBattlePass BattlePassData
		{
			get
			{
				if (this.mDataModule != null)
				{
					return this.mDataModule.BattlePass;
				}
				return null;
			}
		}

		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnBattlePassRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, new HandlerEvent(this.OnBattlePassRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassScore, new HandlerEvent(this.OnBattlePassScore));
			this.mDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.mLoginModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.LoopList.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.TextSession.text = "";
			this.TextTitle.text = "";
			this.ButtonPay.m_onClick = null;
			this.TimeCtrl.OnRefreshText += this.OnRefreshNextTimeText;
			this.TimeCtrl.OnChangeState += this.OnChangeStateNextTime;
			this.TimeCtrl.Init();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnBattlePassRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, new HandlerEvent(this.OnBattlePassRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassScore, new HandlerEvent(this.OnBattlePassScore));
			this.m_seqPool.Clear(false);
			foreach (KeyValuePair<int, IAPBattlePassBaseCell> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
			this.TimeCtrl.DeInit();
			this.isNoTime = false;
		}

		public override void OnShow()
		{
			base.OnShow();
			this.RefreshUI();
			int showIndex = this.GetShowIndex();
			if (showIndex > 0)
			{
				this.LoopList.MovePanelToItemIndex(showIndex, 80f);
			}
		}

		public override void OnHide()
		{
			base.OnHide();
			this.m_seqPool.Clear(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIRechargeGift_Refresh, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.TimeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		private void UpdatePreviewItem(float deltaTime, float unscaledDeltaTime)
		{
			this.mUpdateCheckPosSec -= deltaTime;
			if (this.mUpdateCheckPosSec <= 0f)
			{
				this.mUpdateCheckPosSec = 0.3f;
				LoopListViewItem2 loopListViewItem = this.FindLastItemInView();
				if (loopListViewItem != null && loopListViewItem.gameObject != null)
				{
					int instanceID = loopListViewItem.gameObject.GetInstanceID();
					if (instanceID != this.mLastShowBottomItemID)
					{
						this.mLastShowBottomItemID = instanceID;
						for (int i = loopListViewItem.ItemIndex; i < this.DataList.Count; i++)
						{
							IAPBattlePassData iapbattlePassData = this.DataList[i];
							if (iapbattlePassData.Type == BattlePassType.Special)
							{
								break;
							}
						}
					}
				}
			}
		}

		private LoopListViewItem2 FindLastItemInView()
		{
			int shownItemCount = this.LoopList.ShownItemCount;
			LoopListViewItem2 loopListViewItem = null;
			for (int i = shownItemCount - 1; i >= 0; i--)
			{
				LoopListViewItem2 shownItemByIndex = this.LoopList.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null) && -this.LoopList.GetItemCornerPosInViewPort(shownItemByIndex, 1).y < this.LoopList.ViewPortHeight)
				{
					loopListViewItem = shownItemByIndex;
					break;
				}
			}
			return loopListViewItem;
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.DataList.Count + 1)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			if (index == 0)
			{
				loopListViewItem = listView.NewListViewItem("IAPBattlePassCellTop");
				return loopListViewItem;
			}
			index--;
			IAPBattlePassData iapbattlePassData = this.DataList[index];
			IAPBattlePassBaseCell iapbattlePassBaseCell = null;
			IAPBattlePass battlePassData = this.BattlePassData;
			bool flag;
			bool flag2;
			battlePassData.GetBattlePassRewardGet(iapbattlePassData.ID, out flag, out flag2);
			BattlePassType type = iapbattlePassData.Type;
			if (type - BattlePassType.Normal > 1)
			{
				if (type == BattlePassType.FinalLoop)
				{
					loopListViewItem = listView.NewListViewItem("IAPBattlePassCellFinal");
					int num = loopListViewItem.gameObject.GetInstanceID();
					iapbattlePassBaseCell = this.TryGetUI(num);
					loopListViewItem.GetComponent<IAPBattlePassBaseCell>();
					if (iapbattlePassBaseCell == null)
					{
						iapbattlePassBaseCell = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<IAPBattlePassCellFinal>());
					}
					this.mFinalCell = iapbattlePassBaseCell as IAPBattlePassCellFinal;
				}
			}
			else
			{
				loopListViewItem = listView.NewListViewItem("IAPBattlePassCellNormal");
				int num = loopListViewItem.gameObject.GetInstanceID();
				iapbattlePassBaseCell = this.TryGetUI(num);
				if (iapbattlePassBaseCell == null)
				{
					iapbattlePassBaseCell = this.TryAddUI(num, loopListViewItem, loopListViewItem.GetComponent<IAPBattlePassCellNormal>());
				}
			}
			if (loopListViewItem == null)
			{
				return loopListViewItem;
			}
			if (iapbattlePassBaseCell != null)
			{
				iapbattlePassBaseCell.OnClickGotoBuy = new Action(this.OnBuyBattlePass);
				iapbattlePassBaseCell.SetData(iapbattlePassData, index, new Action(this.OnCollectAll));
				iapbattlePassBaseCell.SetStatus(battlePassData.CurrentScore, flag, flag2);
				iapbattlePassBaseCell.SetActive(true);
				iapbattlePassBaseCell.RefreshUI(this.m_seqPool);
			}
			List<IAPBattlePassCellNormal> list = new List<IAPBattlePassCellNormal>();
			foreach (IAPBattlePassBaseCell iapbattlePassBaseCell2 in this.mUICtrlDic.Values)
			{
				if (iapbattlePassBaseCell2 is IAPBattlePassCellNormal)
				{
					list.Add(iapbattlePassBaseCell2 as IAPBattlePassCellNormal);
				}
			}
			list.Sort((IAPBattlePassCellNormal a, IAPBattlePassCellNormal b) => a.Index.CompareTo(b.Index));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetAsLastSibling();
			}
			if (this.mFinalCell != null)
			{
				this.mFinalCell.transform.SetAsLastSibling();
			}
			return loopListViewItem;
		}

		private IAPBattlePassBaseCell TryGetUI(int key)
		{
			IAPBattlePassBaseCell iapbattlePassBaseCell;
			if (this.mUICtrlDic.TryGetValue(key, out iapbattlePassBaseCell))
			{
				return iapbattlePassBaseCell;
			}
			return null;
		}

		private IAPBattlePassBaseCell TryAddUI(int key, LoopListViewItem2 loopitem, IAPBattlePassBaseCell ui)
		{
			ui.Init();
			IAPBattlePassBaseCell iapbattlePassBaseCell;
			if (this.mUICtrlDic.TryGetValue(key, out iapbattlePassBaseCell))
			{
				if (iapbattlePassBaseCell == null)
				{
					iapbattlePassBaseCell = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		public void RefreshUI()
		{
			if (base.gameObject == null || this.BattlePassData == null)
			{
				return;
			}
			this.m_seqPool.Clear(false);
			if (this.mDataModule.BattlePass.IsAllEnd())
			{
				this.ShowEndPop();
				return;
			}
			this.DataList = this.BattlePassData.DataList;
			this.LoopList.SetListItemCount(this.DataList.Count + 1, true);
			this.LoopList.RefreshAllShownItem();
			if (this.BattlePassData.BattlePassID == 0)
			{
				return;
			}
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(this.BattlePassData.BattlePassID);
			if (elementById == null)
			{
				return;
			}
			this.TextSession.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_season", new object[] { elementById.seasonID });
			this.TextTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.TimeCtrl.Play();
			if (this.BattlePassData.HasBuy)
			{
				this.ButtonPay.m_onClick = null;
				this.payBtnGray.SetUIGray();
				this.textButton.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_active");
			}
			else
			{
				this.ButtonPay.m_onClick = new Action(this.OnBuyBattlePass);
				this.payBtnGray.Recovery();
				this.textButton.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_unactive");
			}
			IAPBattlePassData currentData = this.BattlePassData.GetCurrentData();
			if (currentData == null)
			{
				this.CurProgress.minValue = 0f;
				this.CurProgress.maxValue = 1f;
				this.CurProgress.value = 1f;
				this.Text_Progress.text = "";
				this.Text_NextLevel.text = "1";
				return;
			}
			ValueTuple<int, int, int> showProgress = this.BattlePassData.GetShowProgress();
			int item = showProgress.Item1;
			int item2 = showProgress.Item2;
			int item3 = showProgress.Item3;
			this.CurProgress.minValue = (float)item;
			this.CurProgress.maxValue = (float)item2;
			this.CurProgress.value = (float)item3;
			this.Text_NextLevel.text = currentData.Level.ToString();
			if (currentData.IsFinal)
			{
				this.Text_Progress.text = string.Format("{0}/{1}", this.CurProgress.value - this.CurProgress.minValue, this.CurProgress.maxValue - this.CurProgress.minValue);
				return;
			}
			this.Text_Progress.text = string.Format("{0}/{1}", this.BattlePassData.CurrentScore - item, currentData.Score - item);
		}

		private void OnBuyBattlePass()
		{
			if (this.BattlePassData == null)
			{
				return;
			}
			if (this.BattlePassData.HasBuy)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.IAPBattlePassBuyViewModule, null, 1, null, null);
		}

		private void OnBattlePassRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUI();
		}

		private void OnBattlePassScore(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUI();
		}

		private void OnJumpReward(IAPBattlePassData data)
		{
			if (this.LoopList != null)
			{
				int num = data.Index;
				if (num < 0)
				{
					num = 0;
				}
				this.LoopList.SetSnapTargetItemIndex(num, -1f);
				this.LoopList.MovePanelToItemIndex(num, 0f);
			}
		}

		private IAPShopTimeCtrl.State OnChangeStateNextTime(IAPShopTimeCtrl arg)
		{
			if (this.isNoTime)
			{
				return IAPShopTimeCtrl.State.Load;
			}
			if (this.mDataModule.BattlePass.IsAllEnd())
			{
				this.ShowEndPop();
				return IAPShopTimeCtrl.State.Load;
			}
			if (this.mDataModule.BattlePass.GetNextTime() <= 0L)
			{
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.AppendInterval(sequence, 2f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					NetworkUtils.PlayerData.TipSendUserGetInfoRequest("uibattlepass_data_refresh", delegate
					{
						if (!GameApp.View.IsOpened(ViewName.IAPBattlePassViewModule))
						{
							this.RefreshUI();
						}
						if (this.mDataModule.BattlePass.GetNextTime() < 0L)
						{
							this.isNoTime = true;
						}
					});
				});
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshNextTimeText(IAPShopTimeCtrl arg)
		{
			return Singleton<LanguageManager>.Instance.GetTime(this.mDataModule.BattlePass.GetNextTime());
		}

		private void ShowEndPop()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_allend");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Confirm");
			DxxTools.UI.OpenPopCommon(infoByID, delegate(int id)
			{
				GameApp.View.CloseView(ViewName.IAPBattlePassViewModule, null);
			}, string.Empty, infoByID2, string.Empty, false, 2);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.IAPBattlePassViewModule, null);
		}

		private void OnCollectAll()
		{
			List<uint> canCollectRewardDataIds = this.GetCanCollectRewardDataIds();
			if (canCollectRewardDataIds.Count > 0)
			{
				List<int> preFreeReward = new List<int>();
				if (this.BattlePassData.BattlePassDto.FreeRewardIdList != null)
				{
					foreach (uint num in this.BattlePassData.BattlePassDto.FreeRewardIdList)
					{
						if (!preFreeReward.Contains((int)num))
						{
							preFreeReward.Add((int)num);
						}
					}
				}
				List<int> prePayReward = new List<int>();
				if (this.BattlePassData.BattlePassDto.BattlePassRewardIdList != null)
				{
					foreach (uint num2 in this.BattlePassData.BattlePassDto.BattlePassRewardIdList)
					{
						if (!prePayReward.Contains((int)num2))
						{
							prePayReward.Add((int)num2);
						}
					}
				}
				NetworkUtils.Purchase.BattlePassRewardRequest(canCollectRewardDataIds, delegate(bool result, BattlePassRewardResponse resp)
				{
					if (result && resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
					{
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
						int level = this.BattlePassData.Level;
						List<int> list = new List<int>();
						if (this.BattlePassData.BattlePassDto.FreeRewardIdList != null)
						{
							foreach (uint num3 in this.BattlePassData.BattlePassDto.FreeRewardIdList)
							{
								if (!list.Contains((int)num3))
								{
									list.Add((int)num3);
								}
							}
						}
						foreach (int num4 in preFreeReward)
						{
							if (list.Contains(num4))
							{
								list.Remove(num4);
							}
						}
						List<int> list2 = new List<int>();
						if (this.BattlePassData.BattlePassDto.BattlePassRewardIdList != null)
						{
							foreach (uint num5 in this.BattlePassData.BattlePassDto.BattlePassRewardIdList)
							{
								if (!list2.Contains((int)num5))
								{
									list2.Add((int)num5);
								}
							}
						}
						foreach (int num6 in prePayReward)
						{
							if (list2.Contains(num6))
							{
								list2.Remove(num6);
							}
						}
						List<int> list3 = new List<int>();
						List<int> list4 = new List<int>();
						for (int i = 0; i < this.BattlePassData.DataList.Count; i++)
						{
							IAPBattlePassData iapbattlePassData = this.BattlePassData.DataList[i];
							if (list.Contains(iapbattlePassData.ID))
							{
								list3.Add(i + 1);
							}
							if (list2.Contains(iapbattlePassData.ID))
							{
								list4.Add(i + 1);
							}
						}
						GameApp.SDK.Analyze.Track_BattlePassGet_BattlePass(level, resp.CommonData.Reward, this.BattlePassData.BattlePassPurchaseID, list3, list4, null);
					}
				});
			}
		}

		private List<uint> GetCanCollectRewardDataIds()
		{
			List<uint> list = new List<uint>();
			if (this.BattlePassData != null)
			{
				for (int i = 0; i < this.DataList.Count; i++)
				{
					IAPBattlePassData iapbattlePassData = this.DataList[i];
					if (iapbattlePassData.Type != BattlePassType.FinalLoop)
					{
						bool flag;
						bool flag2;
						this.BattlePassData.GetBattlePassRewardGet(iapbattlePassData.ID, out flag, out flag2);
						bool flag3 = !flag && iapbattlePassData.Score <= this.BattlePassData.CurrentScore;
						bool flag4 = !flag2 && iapbattlePassData.Score <= this.BattlePassData.CurrentScore && this.BattlePassData.HasBuy;
						if (flag3 || flag4)
						{
							list.Add((uint)iapbattlePassData.ID);
						}
					}
				}
			}
			return list;
		}

		private int GetShowIndex()
		{
			if (this.BattlePassData != null)
			{
				for (int i = 0; i < this.DataList.Count; i++)
				{
					IAPBattlePassData iapbattlePassData = this.DataList[i];
					bool flag;
					bool flag2;
					this.BattlePassData.GetBattlePassRewardGet(iapbattlePassData.ID, out flag, out flag2);
					if (this.BattlePassData.HasBuy)
					{
						if (!flag || !flag2)
						{
							return i;
						}
					}
					else if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		public LoopListView2 LoopList;

		public IAPShopTimeCtrl TimeCtrl;

		public CustomText TextTitle;

		public CustomText TextSession;

		public CustomButton ButtonPay;

		public CustomText textButton;

		public Slider CurProgress;

		public CustomText Text_Progress;

		public CustomText Text_NextLevel;

		public UIGrays payBtnGray;

		private const float Max_UpdateCheckPosSec = 0.3f;

		private float mUpdateCheckPosSec = 0.3f;

		private int mLastShowBottomItemID;

		private List<IAPBattlePassData> DataList = new List<IAPBattlePassData>();

		private Dictionary<int, IAPBattlePassBaseCell> mUICtrlDic = new Dictionary<int, IAPBattlePassBaseCell>();

		private IAPBattlePassCellFinal mFinalCell;

		private IAPDataModule mDataModule;

		private LoginDataModule mLoginModule;

		private SequencePool m_seqPool = new SequencePool();

		private bool isNoTime;
	}
}
