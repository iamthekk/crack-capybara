using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.PurchaseManager;
using LocalModels.Bean;
using Proto.SevenDayTask;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class CarnivalTaskOneCtrl : CustomBehaviour
	{
		public void RefreshRewardData(CarnivalTaskData taskOne, int nIndex, bool bIsOpAnim)
		{
			if (!this.isInit)
			{
				this.isInit = true;
				base.Init();
			}
			this.ConfirmBtn.gameObject.SetActiveSafe(false);
			this.CollectBtn.gameObject.SetActiveSafe(false);
			this.PayBtn.gameObject.SetActiveSafe(false);
			this.PayRedNode.gameObject.SetActiveSafe(false);
			if (taskOne == null)
			{
				HLog.LogError("Init CarnivalTaskOneCtrl Fail : CarnivalTaskData IS NULL");
				return;
			}
			int totalProgressNeed = taskOne.TotalProgressNeed;
			this.DescribeText.text = Singleton<LanguageManager>.Instance.GetInfoByID(taskOne.DescribeId, new object[] { DxxTools.FormatNumber((long)totalProgressNeed) });
			int num = Utility.Math.Min(taskOne.Progress, totalProgressNeed);
			if (taskOne.ProgressType == 0)
			{
				this.ActiveShow.text = (taskOne.IsFinish ? "(1/1)" : "(0/1)");
			}
			else
			{
				this.ActiveShow.text = string.Format("({0}/{1})", num, totalProgressNeed);
			}
			this.m_dataList.Clear();
			if (taskOne.DropItem != null)
			{
				this.m_dataList.AddRange(taskOne.DropItem);
			}
			this.m_rewardsScroll.SetListItemCount(this.m_dataList.Count, true);
			this.m_rewardsScroll.RefreshAllShownItem();
			this.ReceivedTick.SetActive(taskOne.IsReceive);
			this.ImgMask.SetActive(taskOne.IsReceive);
			if (!taskOne.Unlock)
			{
				this.ConfirmText.text = Singleton<LanguageManager>.Instance.GetInfoByID("190");
			}
			else if (taskOne.IsFinish)
			{
				this.CollectText.text = Singleton<LanguageManager>.Instance.GetInfoByID("400148");
			}
			else
			{
				this.ConfirmText.text = ((taskOne.JumpType <= 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("1256") : Singleton<LanguageManager>.Instance.GetInfoByID("1257"));
			}
			if (!taskOne.IsReceive && !taskOne.IsFinish)
			{
				this.ConfirmBtn.gameObject.SetActive(true);
				this.ConfirmBtn.m_onClick = delegate
				{
					if (!taskOne.Unlock)
					{
						EventArgsString instance = Singleton<EventArgsString>.Instance;
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("190");
						instance.SetData(infoByID);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
						return;
					}
					if (!this._carnivalDataModule.IfTaskTimeOut)
					{
						if (taskOne.JumpType <= 0)
						{
							return;
						}
						ViewJumpType jump = (ViewJumpType)GameApp.Table.GetManager().GetSevenDay_SevenDayTaskModelInstance().GetElementById(taskOne.Id)
							.Jump;
						this.JumpToView(jump);
					}
				};
			}
			if (!taskOne.IsReceive && taskOne.IsFinish)
			{
				this.CollectBtn.gameObject.SetActive(true);
				Action<SevenDayTaskRewardResponse> <>9__2;
				this.CollectBtn.m_onClick = delegate
				{
					if (taskOne.IsFinish)
					{
						SevenDayCarnivalDataModule carnivalDataModule = this._carnivalDataModule;
						int id = taskOne.Id;
						Action<SevenDayTaskRewardResponse> action;
						if ((action = <>9__2) == null)
						{
							action = (<>9__2 = delegate(SevenDayTaskRewardResponse resp)
							{
								this._carnivalDataModule.IfCanShowRedNode = false;
								List<ItemData> dropItem = taskOne.DropItem;
								GameApp.View.FlyItemDatas(FlyItemModel.Default, dropItem, null, null);
								GameApp.Event.DispatchNow(this, LocalMessageName.CC_CarnivalPanel_RefreshActiveBarBoxState, null);
							});
						}
						carnivalDataModule.RequestGetCarnivalTaskReward(id, action, delegate(int errorMsg)
						{
						});
					}
				};
			}
			if (bIsOpAnim)
			{
				this.Root.anchoredPosition = new Vector2((float)(400 + nIndex * 200), 0f);
				this.KillTween();
				this.tween = ShortcutExtensions46.DOAnchorPosX(this.Root, 0f, 0.2f + (float)nIndex * 0.1f, false);
			}
		}

		public void RefreshPayData(int nIndex, CarnivalTaskOneCtrl.PayData payData)
		{
			if (!this.isInit)
			{
				this.isInit = true;
				base.Init();
			}
			this.ConfirmBtn.gameObject.SetActiveSafe(false);
			this.CollectBtn.gameObject.SetActiveSafe(false);
			this.PayBtn.gameObject.SetActiveSafe(false);
			this.PayRedNode.gameObject.SetActiveSafe(false);
			SevenDay_SevenDayPay cfg = payData.cfg;
			this.DescribeText.text = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.ObjName);
			string[] objGoods = cfg.ObjGoods;
			int num = objGoods.Length;
			this.m_dataList.Clear();
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(objGoods[i].Split(',', StringSplitOptions.None)[0]);
				long num3 = Convert.ToInt64(objGoods[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num2, num3);
				this.m_dataList.Add(itemData);
			}
			this.m_rewardsScroll.SetListItemCount(num, true);
			this.m_rewardsScroll.RefreshAllShownItem();
			if (cfg.objToplimit > 0)
			{
				this.ActiveShow.text = string.Format("({0}/{1})", payData.buyCount, cfg.objToplimit);
			}
			else
			{
				this.ActiveShow.text = "";
			}
			if (payData.finished)
			{
				this.ReceivedTick.SetActive(true);
				this.ImgMask.SetActive(true);
			}
			else
			{
				this.ReceivedTick.SetActive(false);
				this.ImgMask.SetActive(false);
				if (payData.isFree)
				{
					this.CollectBtn.gameObject.SetActiveSafe(true);
					this.CollectText.text = Singleton<LanguageManager>.Instance.GetInfoByID("101");
					this.CollectBtn.m_onClick = delegate
					{
						this._carnivalDataModule.RequestSevenDayFreeReward(cfg.id, cfg.Day, null, null);
					};
				}
				else
				{
					this.PayBtn.gameObject.SetActiveSafe(true);
					if (payData.isFree)
					{
						this.PayRedNode.gameObject.SetActiveSafe(true);
						this.PayRedNode.SetType(240);
						this.PayRedNode.Value = 1;
					}
					Action<bool> <>9__2;
					this.PayBtn.SetData(cfg.PurchaseId, null, delegate(int purchaseId)
					{
						IPurchaseManager manager = GameApp.Purchase.Manager;
						int purchaseId2 = cfg.PurchaseId;
						int num4 = 2;
						string text = string.Format("0,{0}", cfg.id);
						Action<bool> action;
						if ((action = <>9__2) == null)
						{
							action = (<>9__2 = delegate(bool result)
							{
								if (result)
								{
									this._carnivalDataModule.RefreshCarnivalGetInfo();
								}
							});
						}
						manager.Buy(purchaseId2, num4, text, action, null);
					}, null, null, null);
				}
			}
			if (payData.bIsOpAnim)
			{
				this.Root.anchoredPosition = new Vector2((float)(400 + nIndex * 200), 0f);
				this.KillTween();
				this.tween = ShortcutExtensions46.DOAnchorPosX(this.Root, 0f, 0.2f + (float)nIndex * 0.1f, false);
			}
		}

		private async void JumpToView(ViewJumpType jumpType)
		{
			if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(jumpType, null, true))
			{
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
				GameApp.View.CloseView(ViewName.SevenDayCarnivalViewModule, null);
			}
		}

		protected override void OnInit()
		{
			this.m_rewardsScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this._carnivalDataModule = GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule);
			this.ConfirmBtn.m_onClick = null;
			this.CollectBtn.m_onClick = null;
		}

		protected override void OnDeInit()
		{
			this.KillTween();
		}

		private void KillTween()
		{
			if (this.tween != null)
			{
				TweenExtensions.Kill(this.tween, false);
				this.tween = null;
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			ItemData itemData = this.m_dataList[index];
			if (itemData == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("item");
			SevenDayCarnivalRewardItem component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<SevenDayCarnivalRewardItem>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshUI(itemData);
			return loopListViewItem;
		}

		public LoopListView2 m_rewardsScroll;

		[SerializeField]
		private RectTransform Root;

		[SerializeField]
		private CustomButton ConfirmBtn;

		[SerializeField]
		private GameObject ReceivedTick;

		[SerializeField]
		private CustomText ActiveShow;

		[SerializeField]
		private CustomText ConfirmText;

		[SerializeField]
		private CustomText DescribeText;

		[SerializeField]
		private GameObject ImgMask;

		[SerializeField]
		private CustomButton CollectBtn;

		[SerializeField]
		private CustomText CollectText;

		[SerializeField]
		public PurchaseButtonCtrl PayBtn;

		[SerializeField]
		private RedNodeOneCtrl PayRedNode;

		private SevenDayCarnivalDataModule _carnivalDataModule;

		private List<ItemData> m_dataList = new List<ItemData>();

		private Dictionary<int, SevenDayCarnivalRewardItem> m_nodes = new Dictionary<int, SevenDayCarnivalRewardItem>();

		private Tweener tween;

		private bool isInit;

		public class PayData
		{
			public static int Compare(CarnivalTaskOneCtrl.PayData a, CarnivalTaskOneCtrl.PayData b)
			{
				if (a.finished != b.finished)
				{
					return a.finished.CompareTo(b.finished);
				}
				if (a.isFree != b.isFree)
				{
					return -a.isFree.CompareTo(b.isFree);
				}
				return a.cfg.id.CompareTo(b.cfg.id);
			}

			public SevenDay_SevenDayPay cfg;

			public uint buyCount;

			public bool finished;

			public bool isFree;

			public bool bIsOpAnim;
		}
	}
}
