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
	public class ChainPacksPushDataModule : IDataModule
	{
		public int GetName()
		{
			return 169;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.ClearLocalData));
		}

		private void ClearLocalData(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnSetChainPacksPopState();
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.ClearLocalData));
		}

		public void OnRefreshData(RepeatedField<ChainActvDto> actvDtos, bool isInit = true)
		{
			if (isInit)
			{
				this.ChainActvDic.Clear();
			}
			for (int i = 0; i < actvDtos.Count; i++)
			{
				if (this.ChainActvDic.ContainsKey(actvDtos[i].Id))
				{
					this.ChainActvDic[actvDtos[i].Id].SetData(actvDtos[i]);
				}
				else
				{
					this.ChainActvDic.Add(actvDtos[i].Id, new ChainPacksPushInfo(actvDtos[i]));
				}
			}
			this.OnStarCountDown();
		}

		public List<ChainActvDto> OnGetChainActvDtoList()
		{
			List<ChainActvDto> list = new List<ChainActvDto>();
			foreach (KeyValuePair<int, ChainPacksPushInfo> keyValuePair in this.ChainActvDic)
			{
				if (keyValuePair.Value.IsCanShow())
				{
					list.Add(keyValuePair.Value.ActvDto);
				}
			}
			list.Sort(delegate(ChainActvDto a, ChainActvDto b)
			{
				if (a.EndTime == b.EndTime)
				{
					ChainPacks_ChainActv chainPacks_ChainActv = GameApp.Table.GetManager().GetChainPacks_ChainActv(a.Id);
					ChainPacks_ChainType chainPacks_ChainType = null;
					if (chainPacks_ChainActv != null)
					{
						chainPacks_ChainType = GameApp.Table.GetManager().GetChainPacks_ChainType(chainPacks_ChainActv.type);
					}
					ChainPacks_ChainType chainPacks_ChainType2 = null;
					ChainPacks_ChainActv chainPacks_ChainActv2 = GameApp.Table.GetManager().GetChainPacks_ChainActv(b.Id);
					if (chainPacks_ChainActv2 != null)
					{
						chainPacks_ChainType2 = GameApp.Table.GetManager().GetChainPacks_ChainType(chainPacks_ChainActv2.type);
					}
					if (chainPacks_ChainType != null && chainPacks_ChainType2 != null)
					{
						if (chainPacks_ChainType.sort == chainPacks_ChainType2.sort)
						{
							return a.Id.CompareTo(b.Id);
						}
						return chainPacks_ChainType.sort.CompareTo(chainPacks_ChainType2.sort);
					}
				}
				return b.EndTime.CompareTo(a.EndTime);
			});
			return list;
		}

		public bool IsCheckChainPacksPop()
		{
			foreach (KeyValuePair<int, ChainPacksPushInfo> keyValuePair in this.ChainActvDic)
			{
				if (!keyValuePair.Value.IsPop && keyValuePair.Value.IsCanShow())
				{
					return true;
				}
			}
			return false;
		}

		public void OnSetChainPacksPopState()
		{
			foreach (KeyValuePair<int, ChainPacksPushInfo> keyValuePair in this.ChainActvDic)
			{
				keyValuePair.Value.IsPop = true;
			}
		}

		private void OnStarCountDown()
		{
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
			GlobalUpdater.Instance.RegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
		}

		private void OnUpdateCheckRemainTimes()
		{
			this.m_needRemoveId.Clear();
			foreach (KeyValuePair<int, ChainPacksPushInfo> keyValuePair in this.ChainActvDic)
			{
				if (DxxTools.Time.ServerTimestamp >= (long)keyValuePair.Value.ActvDto.EndTime)
				{
					this.m_needRemoveId.Add(keyValuePair.Value.ActvDto.Id);
				}
			}
			if (this.m_needRemoveId.Count > 0)
			{
				for (int i = 0; i < this.m_needRemoveId.Count; i++)
				{
					if (this.ChainActvDic.ContainsKey(this.m_needRemoveId[i]))
					{
						this.ChainActvDic.Remove(this.m_needRemoveId[i]);
					}
				}
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChainPacksPush_Rerfresh, null);
			}
		}

		public void Reset()
		{
			this.ChainActvDic.Clear();
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
		}

		private Dictionary<int, ChainPacksPushInfo> ChainActvDic = new Dictionary<int, ChainPacksPushInfo>();

		private List<int> m_needRemoveId = new List<int>();
	}
}
