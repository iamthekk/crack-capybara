using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using SuperScrollView;

namespace HotFix
{
	public class CommonFundScrollCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.loopView.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(List<CommonFundUIData> dataList, int atlasId, string icon, string finalTipId, Action<int, int> onGetReward, Action onGetFinal)
		{
			this.DataList = dataList;
			this.ScoreAtlasId = atlasId;
			this.ScoreIcon = icon;
			this.FinalTipId = finalTipId;
			this.OnGetReward = onGetReward;
			this.OnGetFinal = onGetFinal;
			this.loopView.SetListItemCount(this.DataList.Count + 1, true);
			this.loopView.RefreshAllShownItem();
		}

		public void SetStatus(int curScore, List<int> freeGet, List<int> payGet, int getFinalNum, bool hasBuy)
		{
			this.CurrentScore = curScore;
			this.FreeGetList = freeGet;
			this.PayGetList = payGet;
			this.GetFinalNum = getFinalNum;
			this.HasBuy = hasBuy;
		}

		public void SetJumpItem(int index, float offset)
		{
			if (index > 0)
			{
				this.loopView.MovePanelToItemIndex(index, offset);
			}
		}

		public void Refresh()
		{
			this.loopView.RefreshAllShownItem();
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
				loopListViewItem = listView.NewListViewItem("CommonFundNodeTop");
				return loopListViewItem;
			}
			index--;
			CommonFundUIData commonFundUIData = this.DataList[index];
			if (commonFundUIData.IsLoopReward)
			{
				loopListViewItem = listView.NewListViewItem("CommonFundNodeBottom");
				int num = loopListViewItem.gameObject.GetInstanceID();
				CommonFundNodeBase commonFundNodeBase = this.TryGetUI(num);
				loopListViewItem.GetComponent<CommonFundNodeBase>();
				if (commonFundNodeBase == null)
				{
					commonFundNodeBase = this.TryAddUI(num, loopListViewItem.GetComponent<CommonFundNodeBottom>());
				}
				this.bottomItem = commonFundNodeBase as CommonFundNodeBottom;
				if (this.bottomItem != null)
				{
					this.bottomItem.SetData(commonFundUIData, index, this.ScoreAtlasId, this.ScoreIcon, this.FinalTipId, this.OnGetFinal);
					this.bottomItem.SetStatus(this.CurrentScore, this.GetFinalNum, this.HasBuy);
					this.bottomItem.Refresh();
					this.bottomItem.gameObject.SetActiveSafe(true);
				}
			}
			else
			{
				loopListViewItem = listView.NewListViewItem("CommonFundNodeItem");
				int num = loopListViewItem.gameObject.GetInstanceID();
				CommonFundNodeBase commonFundNodeBase = this.TryGetUI(num);
				if (commonFundNodeBase == null)
				{
					commonFundNodeBase = this.TryAddUI(num, loopListViewItem.GetComponent<CommonFundNodeItem>());
				}
				CommonFundNodeItem commonFundNodeItem = commonFundNodeBase as CommonFundNodeItem;
				if (commonFundNodeItem != null)
				{
					bool flag = this.IsFreeGet(commonFundUIData.ConfigId);
					bool flag2 = this.IsPayGet(commonFundUIData.ConfigId);
					commonFundNodeItem.SetData(commonFundUIData, index, this.ScoreAtlasId, this.ScoreIcon, this.OnGetReward);
					commonFundNodeItem.SetStatus(this.CurrentScore, flag, flag2, this.HasBuy);
					commonFundNodeItem.Refresh();
					commonFundNodeItem.gameObject.SetActiveSafe(true);
				}
			}
			List<CommonFundNodeItem> list = new List<CommonFundNodeItem>();
			foreach (CommonFundNodeBase commonFundNodeBase2 in this.itemDic.Values)
			{
				if (commonFundNodeBase2 is CommonFundNodeItem)
				{
					list.Add(commonFundNodeBase2 as CommonFundNodeItem);
				}
			}
			list.Sort((CommonFundNodeItem a, CommonFundNodeItem b) => a.SortIndex.CompareTo(b.SortIndex));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetAsLastSibling();
			}
			if (this.bottomItem != null)
			{
				this.bottomItem.transform.SetAsLastSibling();
			}
			return loopListViewItem;
		}

		private CommonFundNodeBase TryGetUI(int key)
		{
			CommonFundNodeBase commonFundNodeBase;
			if (this.itemDic.TryGetValue(key, out commonFundNodeBase))
			{
				return commonFundNodeBase;
			}
			return null;
		}

		private CommonFundNodeBase TryAddUI(int key, CommonFundNodeBase ui)
		{
			ui.Init();
			CommonFundNodeBase commonFundNodeBase;
			if (this.itemDic.TryGetValue(key, out commonFundNodeBase))
			{
				if (commonFundNodeBase == null)
				{
					this.itemDic[key] = ui;
				}
				return ui;
			}
			this.itemDic.Add(key, ui);
			return ui;
		}

		private bool IsFreeGet(int id)
		{
			return this.FreeGetList != null && this.FreeGetList.Contains(id);
		}

		private bool IsPayGet(int id)
		{
			return this.PayGetList != null && this.PayGetList.Contains(id);
		}

		public LoopListView2 loopView;

		private List<CommonFundUIData> DataList;

		private Dictionary<int, CommonFundNodeBase> itemDic = new Dictionary<int, CommonFundNodeBase>();

		private CommonFundNodeBottom bottomItem;

		private int ScoreAtlasId;

		private string ScoreIcon;

		private string FinalTipId;

		private Action<int, int> OnGetReward;

		private Action OnGetFinal;

		private int CurrentScore;

		private int GetFinalNum;

		private List<int> FreeGetList;

		private List<int> PayGetList;

		private bool HasBuy;
	}
}
