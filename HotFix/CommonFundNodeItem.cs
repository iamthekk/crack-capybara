using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class CommonFundNodeItem : CommonFundNodeBase
	{
		public int SortIndex
		{
			get
			{
				return this.mIndex;
			}
		}

		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
			this.maskObj.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.freeRewardItems.Count; i++)
			{
				GameObject gameObject = this.freeRewardItems[i].gameObject;
				this.freeRewardItems[i].DeInit();
				if (gameObject)
				{
					Object.Destroy(gameObject);
				}
			}
			this.freeRewardItems.Clear();
			for (int j = 0; j < this.payRewardItems.Count; j++)
			{
				GameObject gameObject2 = this.payRewardItems[j].gameObject;
				this.payRewardItems[j].DeInit();
				if (gameObject2)
				{
					Object.Destroy(gameObject2);
				}
			}
			this.payRewardItems.Clear();
		}

		public void SetData(CommonFundUIData data, int index, int atlasId, string icon, Action<int, int> onGetReward)
		{
			this.mData = data;
			this.mIndex = index;
			this.OnGetReward = onGetReward;
			string atlasPath = GameApp.Table.GetAtlasPath(atlasId);
			this.imageScore.SetImage(atlasPath, icon);
		}

		public void SetStatus(int currentScore, bool isFreeGet, bool isPayGet, bool hasBuy)
		{
			this.CurrentScore = currentScore;
			this.IsFreeGet = isFreeGet;
			this.IsPayGet = isPayGet;
			this.HasBuy = hasBuy;
		}

		public void Refresh()
		{
			if (this.mData == null || base.gameObject == null)
			{
				return;
			}
			this.maskObj.SetActiveSafe(this.mData.Score > this.CurrentScore);
			if (this.freeRewardItems.Count != this.mData.FreeRewards.Count)
			{
				this.CreateRewards(this.freeRewardItems, this.mData.FreeRewards, this.freeParent);
			}
			bool flag = this.mData.Score > this.CurrentScore;
			bool flag2 = !this.IsFreeGet && this.mData.Score <= this.CurrentScore;
			for (int i = 0; i < this.freeRewardItems.Count; i++)
			{
				CommonFundRewardItem commonFundRewardItem = this.freeRewardItems[i];
				if (i < this.mData.FreeRewards.Count)
				{
					commonFundRewardItem.gameObject.SetActiveSafe(true);
					commonFundRewardItem.SetData(this.mData.FreeRewards[i].ToPropData(true));
					commonFundRewardItem.Refresh();
					commonFundRewardItem.SetState(this.IsFreeGet, flag, this.IsFreeGet, flag2);
					commonFundRewardItem.SetClick(new Action<UIItem, PropData, object>(this.OnClickFreeItem));
				}
				else
				{
					commonFundRewardItem.gameObject.SetActiveSafe(false);
				}
			}
			if (this.payRewardItems.Count != this.mData.PayRewards.Count)
			{
				this.CreateRewards(this.payRewardItems, this.mData.PayRewards, this.payParent);
			}
			bool flag3 = this.mData.Score > this.CurrentScore || !this.HasBuy;
			bool flag4 = !this.IsPayGet && this.mData.Score <= this.CurrentScore && this.HasBuy;
			for (int j = 0; j < this.payRewardItems.Count; j++)
			{
				CommonFundRewardItem commonFundRewardItem2 = this.payRewardItems[j];
				if (j < this.mData.PayRewards.Count)
				{
					commonFundRewardItem2.gameObject.SetActiveSafe(true);
					commonFundRewardItem2.SetData(this.mData.PayRewards[j].ToPropData(true));
					commonFundRewardItem2.Refresh();
					commonFundRewardItem2.SetState(this.IsPayGet, flag3, this.IsPayGet, flag4);
					commonFundRewardItem2.SetClick(new Action<UIItem, PropData, object>(this.OnClickPayItem));
				}
				else
				{
					commonFundRewardItem2.gameObject.SetActiveSafe(false);
				}
			}
			this.textScore.text = this.mData.Score.ToString();
			float height = this.progressBgRT.rect.height;
			float num = ((this.CurrentScore >= this.mData.Score) ? 0f : height);
			Utility.SetBottom(this.progressFgRT, num);
		}

		private void CreateRewards(List<CommonFundRewardItem> itemList, List<ItemData> rewardList, GameObject parent)
		{
			for (int i = 0; i < itemList.Count; i++)
			{
				itemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < rewardList.Count; j++)
			{
				CommonFundRewardItem commonFundRewardItem;
				if (j < itemList.Count)
				{
					commonFundRewardItem = itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(parent, false);
					commonFundRewardItem = gameObject.GetComponent<CommonFundRewardItem>();
					commonFundRewardItem.Init();
					itemList.Add(commonFundRewardItem);
				}
				commonFundRewardItem.gameObject.SetActiveSafe(true);
			}
		}

		private void OnClickFreeItem(UIItem item, PropData data, object obj)
		{
			if (this.IsFreeGet || this.mData.Score > this.CurrentScore)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			Action<int, int> onGetReward = this.OnGetReward;
			if (onGetReward == null)
			{
				return;
			}
			onGetReward(this.mData.ConfigId, 0);
		}

		private void OnClickPayItem(UIItem item, PropData data, object obj)
		{
			if (this.IsPayGet || this.mData.Score > this.CurrentScore || !this.HasBuy)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			Action<int, int> onGetReward = this.OnGetReward;
			if (onGetReward == null)
			{
				return;
			}
			onGetReward(this.mData.ConfigId, 1);
		}

		public GameObject freeParent;

		public GameObject payParent;

		public GameObject copyItem;

		public RectTransform progressBgRT;

		public RectTransform progressFgRT;

		public CustomImage imageScore;

		public CustomText textScore;

		public GameObject maskObj;

		private CommonFundUIData mData;

		private int mIndex;

		private int CurrentScore;

		private bool IsFreeGet;

		private bool IsPayGet;

		private bool HasBuy;

		private Action<int, int> OnGetReward;

		private List<CommonFundRewardItem> freeRewardItems = new List<CommonFundRewardItem>();

		private List<CommonFundRewardItem> payRewardItems = new List<CommonFundRewardItem>();
	}
}
