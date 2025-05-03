using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class VIPDataModule : IDataModule
	{
		public UserVipLevel UserVip
		{
			get
			{
				return this.m_userVip;
			}
		}

		public int PrivilegeCount
		{
			get
			{
				return this.m_privilegeCount;
			}
		}

		public int MaxVIPLevel
		{
			get
			{
				return this.m_maxVIPLevel;
			}
		}

		public int LastVipLevel
		{
			get
			{
				return this.m_lastVipLevel;
			}
		}

		public int VipLevel
		{
			get
			{
				if (this.m_userVip != null)
				{
					return (int)this.m_userVip.VipLevel;
				}
				return 0;
			}
		}

		public int VipExp
		{
			get
			{
				if (this.m_userVip != null)
				{
					return (int)this.m_userVip.VipExp;
				}
				return 0;
			}
		}

		public bool IsCanGetReward(int viplv)
		{
			return viplv > 0 && viplv <= this.VipLevel && !this.IsRewardGetted(viplv);
		}

		public bool IsRewardGetted(int viplv)
		{
			return this.m_userVip != null && this.m_userVip.RewardId != null && this.m_userVip.RewardId.Contains((uint)viplv);
		}

		public int GetName()
		{
			return 135;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.LoginDataSet));
			manager.RegisterEvent(LocalMessageName.CC_CurrecyVIPLevel_Update, new HandlerEvent(this.OnRefreshVIPUpdate));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.LoginDataSet));
			manager.UnRegisterEvent(LocalMessageName.CC_CurrecyVIPLevel_Update, new HandlerEvent(this.OnRefreshVIPUpdate));
		}

		public void Reset()
		{
		}

		private void InitDatas()
		{
			this.m_vipDicDatas.Clear();
			IList<Vip_vip> allElements = GameApp.Table.GetManager().GetVip_vipModelInstance().GetAllElements();
			List<int> list = new List<int>();
			Vip_data[] array = new Vip_data[10];
			for (int i = 0; i < 10; i++)
			{
				array[i] = GameApp.Table.GetManager().GetVip_dataModelInstance().GetElementById(i + 1);
			}
			this.m_privilegeCount = 10;
			for (int j = 0; j < allElements.Count; j++)
			{
				Dictionary<int, VIPDataModule.VIPData> dictionary = new Dictionary<int, VIPDataModule.VIPData>();
				Vip_vip vip_vip = allElements[j];
				this.m_maxVIPLevel = vip_vip.id;
				string[] array2 = new string[] { vip_vip.Power1, vip_vip.Power2, vip_vip.Power3, vip_vip.Power4, vip_vip.Power5, vip_vip.Power6, vip_vip.Power7, vip_vip.Power8, vip_vip.Power9, vip_vip.Power10 };
				for (int k = 0; k < array2.Length; k++)
				{
					if (!string.IsNullOrEmpty(array2[k]))
					{
						Vip_data vip_data = array[k];
						bool flag = !list.Contains(vip_data.id);
						VIPDataModule.VIPData vipdata = new VIPDataModule.VIPData();
						vipdata.SetTable(vip_data, array2[k], flag);
						dictionary[vip_data.id] = vipdata;
					}
				}
				list = dictionary.Keys.ToList<int>();
				VIPDataModule.VIPDatas vipdatas = new VIPDataModule.VIPDatas();
				vipdatas.m_id = vip_vip.id;
				vipdatas.m_exp = vip_vip.Exp;
				vipdatas.m_vipDatas = dictionary.Values.ToList<VIPDataModule.VIPData>().OrderBy(delegate(VIPDataModule.VIPData c)
				{
					if (!c.m_isNew)
					{
						return 1;
					}
					return 0;
				}).ToList<VIPDataModule.VIPData>();
				vipdatas.m_vipDicDatas = dictionary;
				List<PropData> list2 = vip_vip.Price.ToPropDataList();
				if (list2 != null && list2.Count > 0)
				{
					vipdatas.BuyRewardsCost = list2[0];
				}
				list2 = vip_vip.PriceOld.ToPropDataList();
				if (list2 != null && list2.Count > 0)
				{
					vipdatas.BuyRewardsCostOld = list2[0];
				}
				vipdatas.BuyRewards = vip_vip.UnlockReward.ToPropDataList();
				if (vipdatas.BuyRewards == null)
				{
					vipdatas.BuyRewards = new List<PropData>();
				}
				this.m_vipDicDatas[vip_vip.id] = vipdatas;
			}
			list.Clear();
		}

		public VIPDataModule.VIPDatas GetVIPDatas(int vipTableID)
		{
			VIPDataModule.VIPDatas vipdatas;
			this.m_vipDicDatas.TryGetValue(vipTableID, out vipdatas);
			return vipdatas;
		}

		public List<VIPDataModule.VIPDatas> GetAllVIPDatas()
		{
			List<VIPDataModule.VIPDatas> list = new List<VIPDataModule.VIPDatas>();
			int maxVIPLevel = this.MaxVIPLevel;
			for (int i = 1; i <= maxVIPLevel; i++)
			{
				list.Add(this.GetVIPDatas(i));
			}
			return list;
		}

		public VIPDataModule.VIPData GetVIPData(int vipTableID, int dataTableID)
		{
			VIPDataModule.VIPDatas vipdatas;
			this.m_vipDicDatas.TryGetValue(vipTableID, out vipdatas);
			if (vipdatas == null)
			{
				return null;
			}
			VIPDataModule.VIPData vipdata;
			vipdatas.m_vipDicDatas.TryGetValue(dataTableID, out vipdata);
			return vipdata;
		}

		public VIPDataModule.VIPData GetCurrentVIPData(int dataTableID)
		{
			return this.GetVIPData(this.VipLevel, dataTableID);
		}

		public VIPDataModule.VIPDatas GetCurrentVIPDatas()
		{
			return this.GetVIPDatas(this.VipLevel);
		}

		private void OnRefreshUserVipLevel(UserVipLevel vipLevel, UserVipLevel lastVipLevel)
		{
			if (vipLevel == null)
			{
				return;
			}
			this.m_lastVipLevel = (int)((lastVipLevel != null) ? lastVipLevel.VipLevel : vipLevel.VipLevel);
			this.m_userVip = vipLevel;
			if (this.m_userVip == null)
			{
				this.m_userVip = new UserVipLevel();
			}
		}

		public VIPDataModule.VIPDatas GetVIPDatas(int fromID, int toID)
		{
			int num = ((fromID < toID) ? fromID : toID);
			int num2 = ((toID > fromID) ? toID : fromID);
			VIPDataModule.VIPDatas vipdatas = new VIPDataModule.VIPDatas();
			if (num == num2)
			{
				VIPDataModule.VIPDatas vipdatas2;
				this.m_vipDicDatas.TryGetValue(num2, out vipdatas2);
				if (vipdatas2 == null)
				{
					return vipdatas;
				}
				vipdatas = vipdatas2.CopyTo();
				for (int i = 0; i < vipdatas.m_vipDatas.Count; i++)
				{
					VIPDataModule.VIPData vipdata = vipdatas.m_vipDatas[i];
					if (vipdata != null)
					{
						vipdata.m_isNew = true;
					}
				}
			}
			else
			{
				VIPDataModule.VIPDatas vipdatas3;
				this.m_vipDicDatas.TryGetValue(num, out vipdatas3);
				VIPDataModule.VIPDatas vipdatas4;
				this.m_vipDicDatas.TryGetValue(num2, out vipdatas4);
				if (vipdatas4 == null)
				{
					return vipdatas;
				}
				vipdatas = vipdatas4.CopyTo();
				for (int j = 0; j < vipdatas.m_vipDatas.Count; j++)
				{
					VIPDataModule.VIPData vipdata2 = vipdatas.m_vipDatas[j];
					if (vipdata2 != null)
					{
						bool flag = vipdatas3 == null || !vipdatas3.m_vipDicDatas.ContainsKey(vipdata2.m_id);
						vipdata2.m_isNew = flag;
					}
				}
			}
			vipdatas.m_vipDatas = vipdatas.m_vipDatas.ToList<VIPDataModule.VIPData>().OrderBy(delegate(VIPDataModule.VIPData c)
			{
				if (!c.m_isNew)
				{
					return 1;
				}
				return 0;
			}).ToList<VIPDataModule.VIPData>();
			return vipdatas;
		}

		public List<ItemData> GetRewards(int tableID)
		{
			List<ItemData> list = new List<ItemData>();
			Vip_vip elementById = GameApp.Table.GetManager().GetVip_vipModelInstance().GetElementById(tableID);
			if (elementById == null)
			{
				return list;
			}
			return elementById.UnlockReward.ToItemDataList();
		}

		public List<ItemData> GetRewards(int fromID, int toID)
		{
			int num = ((fromID < toID) ? fromID : toID);
			int num2 = ((toID > fromID) ? toID : fromID);
			List<ItemData> list = new List<ItemData>();
			if (num == num2)
			{
				list = this.GetRewards(num2);
			}
			else
			{
				for (int i = num + 1; i <= num2; i++)
				{
					list.AddRange(this.GetRewards(i));
				}
			}
			return list.ToCombineList();
		}

		public bool IsHaveRedPoint()
		{
			int vipLevel = this.VipLevel;
			for (int i = 1; i <= vipLevel; i++)
			{
				if (this.IsCanGetReward(i))
				{
					return true;
				}
			}
			return false;
		}

		private void LoginDataSet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventLogin eventLogin = eventArgs as EventLogin;
			if (eventLogin == null)
			{
				return;
			}
			this.OnRefreshUserVipLevel(eventLogin.userLoginResponse.UserVipLevel, null);
			this.InitDatas();
		}

		private void OnRefreshVIPUpdate(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsUserVIPLevelUpdata eventArgsUserVIPLevelUpdata = eventargs as EventArgsUserVIPLevelUpdata;
			if (eventArgsUserVIPLevelUpdata == null)
			{
				return;
			}
			this.OnRefreshUserVipLevel(eventArgsUserVIPLevelUpdata.m_newLevel, eventArgsUserVIPLevelUpdata.m_oldLevel);
		}

		[SerializeField]
		private UserVipLevel m_userVip = new UserVipLevel();

		[SerializeField]
		private int m_lastVipLevel;

		private Dictionary<int, VIPDataModule.VIPDatas> m_vipDicDatas = new Dictionary<int, VIPDataModule.VIPDatas>();

		[SerializeField]
		private int m_privilegeCount;

		[SerializeField]
		private int m_maxVIPLevel;

		public enum ParamType
		{
			Int = 1,
			Float,
			Bool
		}

		public class VIPDatas
		{
			public VIPDataModule.VIPDatas CopyTo()
			{
				VIPDataModule.VIPDatas vipdatas = new VIPDataModule.VIPDatas();
				vipdatas.m_id = this.m_id;
				vipdatas.m_vipDatas = new List<VIPDataModule.VIPData>(this.m_vipDatas.Count);
				vipdatas.m_vipDicDatas = new Dictionary<int, VIPDataModule.VIPData>(this.m_vipDatas.Count);
				for (int i = 0; i < this.m_vipDatas.Count; i++)
				{
					VIPDataModule.VIPData vipdata = this.m_vipDatas[i];
					if (vipdata != null)
					{
						VIPDataModule.VIPData vipdata2 = vipdata.CopyTo();
						vipdatas.m_vipDatas.Add(vipdata2);
						vipdatas.m_vipDicDatas[vipdata2.m_id] = vipdata2;
					}
				}
				return vipdatas;
			}

			public int m_id;

			public int m_exp;

			public List<VIPDataModule.VIPData> m_vipDatas = new List<VIPDataModule.VIPData>();

			public Dictionary<int, VIPDataModule.VIPData> m_vipDicDatas = new Dictionary<int, VIPDataModule.VIPData>();

			public PropData BuyRewardsCost;

			public PropData BuyRewardsCostOld;

			public List<PropData> BuyRewards = new List<PropData>();
		}

		public class VIPData
		{
			public void SetTable(Vip_data table, string value, bool isNew)
			{
				this.m_id = table.id;
				this.m_value = value;
				this.m_isNew = isNew;
				this.m_languageID = table.LangugaeID.ToString();
				this.m_paramType = (VIPDataModule.ParamType)table.ParamType;
			}

			public int GetInt()
			{
				int num;
				int.TryParse(this.m_value, out num);
				return num;
			}

			public float GetFloat()
			{
				float num;
				float.TryParse(this.m_value, out num);
				return num;
			}

			public bool GetBool()
			{
				return string.Equals(this.m_value, "1");
			}

			public object GetObject()
			{
				object obj = null;
				switch (this.m_paramType)
				{
				case VIPDataModule.ParamType.Int:
					obj = this.GetInt();
					break;
				case VIPDataModule.ParamType.Float:
					obj = this.GetFloat();
					break;
				case VIPDataModule.ParamType.Bool:
					obj = this.GetBool();
					break;
				}
				return obj;
			}

			public string GetValueString()
			{
				string text = string.Empty;
				object @object = this.GetObject();
				switch (this.m_paramType)
				{
				case VIPDataModule.ParamType.Int:
					text = @object.ToString();
					break;
				case VIPDataModule.ParamType.Float:
					text = ((float)@object * 100f).ToString() + "%";
					break;
				case VIPDataModule.ParamType.Bool:
					text = string.Empty;
					break;
				}
				return text;
			}

			public VIPDataModule.VIPData CopyTo()
			{
				return new VIPDataModule.VIPData
				{
					m_id = this.m_id,
					m_isNew = this.m_isNew,
					m_value = this.m_value,
					m_paramType = this.m_paramType,
					m_languageID = this.m_languageID
				};
			}

			public int m_id;

			public bool m_isNew;

			public string m_value = string.Empty;

			public VIPDataModule.ParamType m_paramType = VIPDataModule.ParamType.Int;

			public string m_languageID;
		}
	}
}
