using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPShopRewardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rewardPrefab.SetActive(false);
			this.CreateNodes();
			this.RefreshViewObjects();
		}

		protected override void OnDeInit()
		{
			this.DestroyNodes();
		}

		public void OnRefreshTo(List<ItemData> data, bool isCanClickVal = false)
		{
			this.DestroyNodes();
			this.SetData(data, this.isCanClick);
			this.CreateNodes();
			this.RefreshViewObjects();
		}

		public void SetData(List<ItemData> dataVal, bool isCanClickVal = false)
		{
			this.itemDataList = dataVal;
			this.isCanClick = isCanClickVal;
		}

		private void CreateNodes()
		{
			for (int i = 0; i < this.itemDataList.Count; i++)
			{
				IAPShopReward iapshopReward = Object.Instantiate<IAPShopReward>(this.rewardPrefab, this.rewardParent);
				iapshopReward.SetActive(true);
				iapshopReward.SetCountShowType(UIItem.CountShowType.MissOne);
				iapshopReward.SetData(this.itemDataList[i].ToPropData(), i);
				iapshopReward.SetOnClick(new Action<UIItem, PropData, object>(DxxTools.UI.OnItemClick));
				iapshopReward.Init();
				iapshopReward.SetCanClick(this.isCanClick);
				iapshopReward.OnRefresh();
				this.rewards[iapshopReward.GetInstanceID()] = iapshopReward;
			}
		}

		private void DestroyNodes()
		{
			this.itemDataList.Clear();
			foreach (KeyValuePair<int, IAPShopReward> keyValuePair in this.rewards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.rewards.Clear();
		}

		private void RefreshViewObjects()
		{
			bool flag = this.rewards.Count > 0;
			foreach (GameObject gameObject in this.viewObjects)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(flag);
				}
			}
		}

		public void SetCanClick(bool isCanClickVal)
		{
			for (int i = 0; i < this.rewards.Count; i++)
			{
				IAPShopReward iapshopReward = this.rewards[i];
				if (!(iapshopReward == null))
				{
					iapshopReward.SetCanClick(isCanClickVal);
				}
			}
		}

		public void SetGrayState(bool isGray)
		{
			foreach (KeyValuePair<int, IAPShopReward> keyValuePair in this.rewards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetGrayState(isGray);
				}
			}
		}

		public void SetActiveForReceive(bool isReceive)
		{
			foreach (KeyValuePair<int, IAPShopReward> keyValuePair in this.rewards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetActiveForReceive(isReceive);
				}
			}
		}

		[SerializeField]
		private IAPShopReward rewardPrefab;

		[SerializeField]
		private RectTransform rewardParent;

		[SerializeField]
		private bool isCanClick;

		[SerializeField]
		private GameObject[] viewObjects;

		private List<ItemData> itemDataList = new List<ItemData>();

		private readonly Dictionary<int, IAPShopReward> rewards = new Dictionary<int, IAPShopReward>();
	}
}
