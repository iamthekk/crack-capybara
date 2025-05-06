using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class ChainPacksDataModule : IDataModule
	{
		public int GetName()
		{
			return 167;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_ChainPacks_DataPull, new HandlerEvent(this.OnEvent_DayChangeDataPull));
			manager.RegisterEvent(LocalMessageName.CC_ChainPacksPush_Rerfresh, new HandlerEvent(this.OnEvent_ChainPacksPushDataPull));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_ChainPacks_DataPull, new HandlerEvent(this.OnEvent_DayChangeDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_ChainPacksPush_Rerfresh, new HandlerEvent(this.OnEvent_ChainPacksPushDataPull));
		}

		public void Reset()
		{
			this.actInfoDic.Clear();
			this.actInfoDic.Clear();
		}

		public ChainPacksDataModule.ChainPacksTypeBind GetChainPacksTypeBind(ChainPacksDataModule.ChainPacksType type)
		{
			ChainPacksDataModule.ChainPacksTypeBind chainPacksTypeBind;
			if (this.chainPacksTypeBindDic.TryGetValue(type, out chainPacksTypeBind))
			{
				return chainPacksTypeBind;
			}
			switch (type)
			{
			case ChainPacksDataModule.ChainPacksType.Permanent:
				chainPacksTypeBind = new ChainPacksDataModule.ChainPacksTypeBind();
				chainPacksTypeBind.Type = ChainPacksDataModule.ChainPacksType.Permanent;
				chainPacksTypeBind.Function_ID = FunctionID.ChainPacks_Permanent;
				chainPacksTypeBind.Function_TargetName = "ChainPacks.Permanent";
				chainPacksTypeBind.REDPOINT_NAME = "ChainPacks.Permanent";
				chainPacksTypeBind.UIMainButton_Priority = 10;
				break;
			case ChainPacksDataModule.ChainPacksType.Halloween:
				chainPacksTypeBind = new ChainPacksDataModule.ChainPacksTypeBind();
				chainPacksTypeBind.Type = ChainPacksDataModule.ChainPacksType.Halloween;
				chainPacksTypeBind.Function_ID = FunctionID.ChainPacks_Halloween;
				chainPacksTypeBind.Function_TargetName = "ChainPacks.Halloween";
				chainPacksTypeBind.REDPOINT_NAME = "ChainPacks.Halloween";
				chainPacksTypeBind.UIMainButton_Priority = 11;
				break;
			case ChainPacksDataModule.ChainPacksType.Thanks:
				chainPacksTypeBind = new ChainPacksDataModule.ChainPacksTypeBind();
				chainPacksTypeBind.Type = ChainPacksDataModule.ChainPacksType.Thanks;
				chainPacksTypeBind.Function_ID = FunctionID.ChainPacks_Thanks;
				chainPacksTypeBind.Function_TargetName = "ChainPacks.Thanks";
				chainPacksTypeBind.REDPOINT_NAME = "ChainPacks.Thanks";
				chainPacksTypeBind.UIMainButton_Priority = 12;
				break;
			default:
				chainPacksTypeBind = new ChainPacksDataModule.ChainPacksTypeBind();
				chainPacksTypeBind.Type = ChainPacksDataModule.ChainPacksType.All;
				chainPacksTypeBind.Function_ID = FunctionID.ChainPacks;
				chainPacksTypeBind.Function_TargetName = "ChainPacks";
				chainPacksTypeBind.REDPOINT_NAME = "ChainPacks";
				chainPacksTypeBind.UIMainButton_Priority = 9;
				break;
			}
			this.chainPacksTypeBindDic.Add(type, chainPacksTypeBind);
			return chainPacksTypeBind;
		}

		public ChainPacksDataModule.ActInfo GetActInfoById(int actId)
		{
			ChainPacksDataModule.ActInfo actInfo;
			if (this.actInfoDic.TryGetValue(actId, out actInfo))
			{
				return actInfo;
			}
			return null;
		}

		public ChainPacksDataModule.ActInfo GetActInfoIndex(int index)
		{
			foreach (ChainPacksDataModule.ActInfo actInfo in this.actInfoDic.Values)
			{
				if (actInfo.actIndex == index)
				{
					return actInfo;
				}
			}
			return null;
		}

		public int GetPickedIndex(int actId)
		{
			ChainPacksDataModule.ActInfo actInfo;
			if (this.actInfoDic.TryGetValue(actId, out actInfo))
			{
				return actInfo.pickedIndex;
			}
			return -1;
		}

		public int GetActEndIndex(int actId)
		{
			ChainPacksDataModule.ActInfo actInfo;
			if (this.actInfoDic.TryGetValue(actId, out actInfo) && actInfo.cfgList.Count > 0)
			{
				return actInfo.cfgList.Count - 1;
			}
			return 0;
		}

		private void OnEvent_DayChangeDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.ChainPacks.DoChainPacksTimeRequest(false, true, null, true);
		}

		private void OnEvent_ChainPacksPushDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnAddChainPacksInfo();
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChainPacksPush_RerfreshView, null);
		}

		public void OnAddChainPacksInfo()
		{
			if (this.m_chainActvDto != null)
			{
				this.NetUpdateChainPacksInfo(this.m_chainActvDto);
				return;
			}
			this.NetUpdateChainPacksInfo(null);
		}

		public void CommonUpdateChainPacksInfo(RepeatedField<ChainActvDto> chainActvDto)
		{
			this.NetUpdateChainPacksInfo(chainActvDto);
		}

		public void NetUpdateChainPacksInfo(RepeatedField<ChainActvDto> chainActvDto)
		{
			this.m_chainActvDto = chainActvDto;
			this.actInfoDic.Clear();
			this.actinfoList.Clear();
			ChainPacksPushDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksPushDataModule);
			int num = 0;
			if (dataModule != null)
			{
				List<ChainActvDto> list = dataModule.OnGetChainActvDtoList();
				for (int i = 0; i < list.Count; i++)
				{
					this.UpdateActInfos(list[i], num, ChainPacksDataModule.EChainPacksOriginType.Push);
					num++;
				}
			}
			if (chainActvDto != null)
			{
				for (int j = 0; j < chainActvDto.Count; j++)
				{
					this.UpdateActInfos(chainActvDto[j], num, ChainPacksDataModule.EChainPacksOriginType.Common);
					num++;
				}
				for (int k = 0; k < this.actinfoList.Count; k++)
				{
					this.actinfoList[k].actIndex = k;
				}
			}
			RedPointController.Instance.ReCalc("ChainPacks", true);
		}

		private int CompareActInfo(ChainPacksDataModule.ActInfo a, ChainPacksDataModule.ActInfo b)
		{
			if (a.actTypeCfg.sort != b.actTypeCfg.sort)
			{
				return a.actTypeCfg.sort.CompareTo(b.actTypeCfg.sort);
			}
			return a.ActId.CompareTo(b.ActId);
		}

		public void NetPickedReward(int actId, int pkId)
		{
			if (this.GetActInfoById(actId) != null)
			{
				EventArgsChainPacksPickedReward instance = Singleton<EventArgsChainPacksPickedReward>.Instance;
				instance.SetData(pkId);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChainPacks_PickedReward, instance);
			}
			else
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChainPacks_ActFreshed, null);
			}
			RedPointController.Instance.ReCalc("ChainPacks", true);
		}

		private void UpdateActInfos(ChainActvDto actDto, int index, ChainPacksDataModule.EChainPacksOriginType type = ChainPacksDataModule.EChainPacksOriginType.None)
		{
			ChainPacksDataModule.ActInfo actInfo = null;
			if (type == ChainPacksDataModule.EChainPacksOriginType.Common)
			{
				ChainPacks_ChainActv chainPacks_ChainActv = GameApp.Table.GetManager().GetChainPacks_ChainActv(actDto.Id);
				if (chainPacks_ChainActv == null)
				{
					return;
				}
				ChainPacksDataModule.ChainActInfo chainActInfo = new ChainPacksDataModule.ChainActInfo();
				chainActInfo.actIndex = index;
				chainActInfo.actCfg = chainPacks_ChainActv;
				chainActInfo.actDto = actDto;
				chainActInfo.actTypeCfg = GameApp.Table.GetManager().GetChainPacks_ChainType(chainPacks_ChainActv.type);
				chainActInfo.OriginType = type;
				IList<ChainPacks_ChainPacks> chainPacks_ChainPacksElements = GameApp.Table.GetManager().GetChainPacks_ChainPacksElements();
				for (int i = 0; i < chainPacks_ChainPacksElements.Count; i++)
				{
					if (chainPacks_ChainPacksElements[i].group == chainActInfo.ActGroupId)
					{
						chainActInfo.cfgList.Add(chainPacks_ChainPacksElements[i]);
					}
				}
				actInfo = chainActInfo;
			}
			else if (type == ChainPacksDataModule.EChainPacksOriginType.Push)
			{
				ChainPacks_PushChainActv chainPacks_PushChainActv = GameApp.Table.GetManager().GetChainPacks_PushChainActv(actDto.Id);
				if (chainPacks_PushChainActv == null)
				{
					return;
				}
				ChainPacksDataModule.ChainPushInfo chainPushInfo = new ChainPacksDataModule.ChainPushInfo();
				chainPushInfo.actIndex = index;
				chainPushInfo.actCfg = chainPacks_PushChainActv;
				chainPushInfo.actDto = actDto;
				chainPushInfo.OriginType = type;
				chainPushInfo.actTypeCfg = GameApp.Table.GetManager().GetChainPacks_ChainType(chainPacks_PushChainActv.type);
				IList<ChainPacks_ChainPacks> chainPacks_ChainPacksElements2 = GameApp.Table.GetManager().GetChainPacks_ChainPacksElements();
				for (int j = 0; j < chainPacks_ChainPacksElements2.Count; j++)
				{
					if (chainPacks_ChainPacksElements2[j].group == chainPushInfo.ActGroupId)
					{
						chainPushInfo.cfgList.Add(chainPacks_ChainPacksElements2[j]);
					}
				}
				actInfo = chainPushInfo;
			}
			actInfo.cfgList.Sort((ChainPacks_ChainPacks a, ChainPacks_ChainPacks b) => a.id.CompareTo(b.id));
			this.UpdateActInfoPickIndex(actInfo);
			if (actInfo.CanShow(ChainPacksDataModule.ChainPacksType.All))
			{
				this.actInfoDic[actDto.Id] = actInfo;
				this.actinfoList.Add(actInfo);
			}
		}

		private void UpdateActInfoPickIndex(ChainPacksDataModule.ActInfo actInfo)
		{
			actInfo.pickedIndex = -1;
			int num = 0;
			for (int i = 0; i < actInfo.actDto.GetRewardIds.Count; i++)
			{
				if (actInfo.actDto.GetRewardIds[i] > num)
				{
					num = actInfo.actDto.GetRewardIds[i];
				}
			}
			for (int j = 0; j < actInfo.cfgList.Count; j++)
			{
				if (actInfo.cfgList[j].id == num)
				{
					actInfo.pickedIndex = j;
					return;
				}
			}
		}

		public bool CanShow(ChainPacksDataModule.ChainPacksType type = ChainPacksDataModule.ChainPacksType.All)
		{
			if (!this.IsOpen(type))
			{
				return false;
			}
			if (this.actInfoDic.Count < 1)
			{
				return false;
			}
			foreach (ChainPacksDataModule.ActInfo actInfo in this.actInfoDic.Values)
			{
				if (this.IsOpen((ChainPacksDataModule.ChainPacksType)actInfo.ActType) && actInfo.CanShow(type))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsOpen(ChainPacksDataModule.ChainPacksType type = ChainPacksDataModule.ChainPacksType.All)
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.ChainPacks, false) && (type == ChainPacksDataModule.ChainPacksType.All || this.IsOpenPacks(this.GetChainPacksTypeBind(type)));
		}

		private bool IsOpenPacks(ChainPacksDataModule.ChainPacksTypeBind bind)
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(bind.Function_ID, false);
		}

		public bool ShowAnyRed(ChainPacksDataModule.ChainPacksType type = ChainPacksDataModule.ChainPacksType.All)
		{
			if (!this.IsOpen(type))
			{
				return false;
			}
			if (this.actInfoDic.Count < 1)
			{
				return false;
			}
			using (Dictionary<int, ChainPacksDataModule.ActInfo>.ValueCollection.Enumerator enumerator = this.actInfoDic.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AnyRed(type, true))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool ShowRedPreAct(int index)
		{
			if (index < 0 || index >= this.actinfoList.Count)
			{
				return false;
			}
			for (int i = 0; i < index; i++)
			{
				if (this.actinfoList[i].AnyRed(ChainPacksDataModule.ChainPacksType.All, true))
				{
					return true;
				}
			}
			return false;
		}

		public bool ShowRedNextAct(int index)
		{
			if (index < 0 || index >= this.actinfoList.Count)
			{
				return false;
			}
			for (int i = index + 1; i < this.actinfoList.Count; i++)
			{
				if (this.actinfoList[i].AnyRed(ChainPacksDataModule.ChainPacksType.All, true))
				{
					return true;
				}
			}
			return false;
		}

		public Dictionary<ChainPacksDataModule.ChainPacksType, ChainPacksDataModule.ChainPacksTypeBind> chainPacksTypeBindDic = new Dictionary<ChainPacksDataModule.ChainPacksType, ChainPacksDataModule.ChainPacksTypeBind>();

		private List<ChainPacksDataModule.ActInfo> actinfoList = new List<ChainPacksDataModule.ActInfo>();

		public Dictionary<int, ChainPacksDataModule.ActInfo> actInfoDic = new Dictionary<int, ChainPacksDataModule.ActInfo>();

		private RepeatedField<ChainActvDto> m_chainActvDto;

		public enum ChainPacksType
		{
			All,
			Permanent,
			Halloween,
			Thanks
		}

		public enum EChainPacksOriginType
		{
			None,
			Common,
			Push
		}

		public class ChainPacksTypeBind
		{
			public ChainPacksDataModule.ChainPacksType Type;

			public FunctionID Function_ID;

			public string Function_TargetName;

			public string REDPOINT_NAME;

			public int UIMainButton_Priority;
		}

		public abstract class ActInfo
		{
			public abstract int ActId { get; }

			public abstract int ActGroupId { get; }

			public abstract int ActType { get; }

			public abstract string ActName { get; }

			public abstract int ActRateValue { get; }

			public abstract ChainPacksDataModule.EChainPacksOriginType ActOriginType { get; }

			public bool CanShow(ChainPacksDataModule.ChainPacksType type = ChainPacksDataModule.ChainPacksType.All)
			{
				return (type == ChainPacksDataModule.ChainPacksType.All || this.ActType == (int)type) && this.actDto.EndTime > (ulong)DxxTools.Time.ServerTimestamp && this.actDto.StartTime <= (ulong)DxxTools.Time.ServerTimestamp && this.pickedIndex < this.cfgList.Count - 1;
			}

			public bool AnyRed(ChainPacksDataModule.ChainPacksType type = ChainPacksDataModule.ChainPacksType.All, bool checkShow = true)
			{
				if (type != ChainPacksDataModule.ChainPacksType.All && this.ActType != (int)type)
				{
					return false;
				}
				if (checkShow && !this.CanShow(type))
				{
					return false;
				}
				int num = this.pickedIndex + 1;
				return num <= this.cfgList.Count - 1 && this.cfgList[num].PurchaseId == 0;
			}

			public ChainPacks_ChainType actTypeCfg;

			public int actIndex;

			public ChainActvDto actDto;

			public List<ChainPacks_ChainPacks> cfgList = new List<ChainPacks_ChainPacks>();

			public int pickedIndex = -1;

			public ChainPacksDataModule.EChainPacksOriginType OriginType;
		}

		public class ChainActInfo : ChainPacksDataModule.ActInfo
		{
			public override int ActId
			{
				get
				{
					return this.actCfg.id;
				}
			}

			public override int ActGroupId
			{
				get
				{
					return this.actCfg.groupID;
				}
			}

			public override int ActType
			{
				get
				{
					return this.actCfg.type;
				}
			}

			public override string ActName
			{
				get
				{
					return this.actCfg.name;
				}
			}

			public override int ActRateValue
			{
				get
				{
					return this.actCfg.RateValue;
				}
			}

			public override ChainPacksDataModule.EChainPacksOriginType ActOriginType
			{
				get
				{
					return this.OriginType;
				}
			}

			public ChainPacks_ChainActv actCfg;
		}

		public class ChainPushInfo : ChainPacksDataModule.ActInfo
		{
			public override int ActId
			{
				get
				{
					return this.actCfg.id;
				}
			}

			public override int ActGroupId
			{
				get
				{
					return this.actCfg.groupID;
				}
			}

			public override int ActType
			{
				get
				{
					return this.actCfg.type;
				}
			}

			public override string ActName
			{
				get
				{
					return this.actCfg.name;
				}
			}

			public override int ActRateValue
			{
				get
				{
					return this.actCfg.RateValue;
				}
			}

			public override ChainPacksDataModule.EChainPacksOriginType ActOriginType
			{
				get
				{
					return this.OriginType;
				}
			}

			public ChainPacks_PushChainActv actCfg;
		}
	}
}
