using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class IAPLevelFund
	{
		public IAPLevelFund(PurchaseCommonData commonData)
		{
			this.m_commonData = commonData;
		}

		internal void SetDatas(IList<uint> buyGroupList, IDictionary<uint, IntegerArray> payRewardGetList, IDictionary<uint, IntegerArray> freeRewardGetList, bool isClear)
		{
			if (isClear)
			{
				this.Groups.Clear();
			}
			if (this.Groups.Count <= 0)
			{
				this.GroupsByGroupID.Clear();
				IList<IAP_LevelFund> allElements = GameApp.Table.GetManager().GetIAP_LevelFundModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					IAPLevelFundGroup iaplevelFundGroup = new IAPLevelFundGroup();
					iaplevelFundGroup.BuildData(allElements[i]);
					this.Groups[iaplevelFundGroup.ID] = iaplevelFundGroup;
					this.GroupsByGroupID[iaplevelFundGroup.GroupID] = iaplevelFundGroup;
				}
			}
			if (buyGroupList != null)
			{
				for (int j = 0; j < buyGroupList.Count; j++)
				{
					int num = (int)buyGroupList[j];
					IAPLevelFundGroup iaplevelFundGroup2;
					if (this.GroupsByGroupID.TryGetValue(num, out iaplevelFundGroup2))
					{
						iaplevelFundGroup2.HasBuy = true;
					}
				}
			}
			if (payRewardGetList != null)
			{
				foreach (KeyValuePair<uint, IntegerArray> keyValuePair in payRewardGetList)
				{
					int key = (int)keyValuePair.Key;
					IAPLevelFundGroup iaplevelFundGroup3;
					if (this.GroupsByGroupID.TryGetValue(key, out iaplevelFundGroup3))
					{
						iaplevelFundGroup3.SetPayRewardCollected(keyValuePair.Value);
					}
				}
			}
			if (freeRewardGetList != null)
			{
				foreach (KeyValuePair<uint, IntegerArray> keyValuePair2 in freeRewardGetList)
				{
					int key2 = (int)keyValuePair2.Key;
					IAPLevelFundGroup iaplevelFundGroup4;
					if (this.GroupsByGroupID.TryGetValue(key2, out iaplevelFundGroup4))
					{
						iaplevelFundGroup4.SetFreeRewardCollected(keyValuePair2.Value);
					}
				}
			}
			RedPointController.Instance.ReCalc("IAPRechargeGift", true);
		}

		public IAPLevelFundGroup GetLevelFundGroup(int levelfundid)
		{
			IAPLevelFundGroup iaplevelFundGroup;
			if (this.Groups.TryGetValue(levelfundid, out iaplevelFundGroup))
			{
				return iaplevelFundGroup;
			}
			return null;
		}

		public bool IsHaveRedPoint()
		{
			List<IAPLevelFundGroupKind> list = DxxTools.EnumToList<IAPLevelFundGroupKind>();
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				IAPLevelFundGroup currentFundGroup = this.GetCurrentFundGroup(list[i]);
				if (currentFundGroup != null && currentFundGroup.GetCanCollectList().Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		public IAPLevelFundGroup GetCurrentFundGroup(IAPLevelFundGroupKind groupKind)
		{
			IAPLevelFundGroup iaplevelFundGroup = null;
			List<IAPLevelFundGroup> list = new List<IAPLevelFundGroup>();
			foreach (IAPLevelFundGroup iaplevelFundGroup2 in this.Groups.Values)
			{
				if (iaplevelFundGroup2.GroupKind == groupKind && !iaplevelFundGroup2.IsAllRewardCollected())
				{
					list.Add(iaplevelFundGroup2);
				}
			}
			list.Sort((IAPLevelFundGroup a, IAPLevelFundGroup b) => a.Index.CompareTo(b.Index));
			if (list.Count > 0)
			{
				iaplevelFundGroup = list[0];
			}
			return iaplevelFundGroup;
		}

		private PurchaseCommonData m_commonData;

		public Dictionary<int, IAPLevelFundGroup> Groups = new Dictionary<int, IAPLevelFundGroup>();

		public Dictionary<int, IAPLevelFundGroup> GroupsByGroupID = new Dictionary<int, IAPLevelFundGroup>();

		public enum RewardState
		{
			Lock,
			Collected,
			CanCollect
		}
	}
}
