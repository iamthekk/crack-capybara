using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;

namespace HotFix
{
	public class MiningDataModule : IDataModule
	{
		public MiningInfoDto MiningInfo { get; private set; }

		public MiningDrawDto MiningDraw { get; private set; }

		public bool IsHaveKey
		{
			get
			{
				return this.MiningInfo != null && this.MiningInfo.OpenKey == 1;
			}
		}

		public bool IsHaveDoor
		{
			get
			{
				if (this.MiningInfo != null)
				{
					for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
					{
						GridDto gridDto = this.MiningInfo.Grids[i];
						if (gridDto.Status != 1)
						{
							Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
							if (mining_oreBuild != null && mining_oreBuild.oreType == 1)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		public bool IsHaveTreasure
		{
			get
			{
				if (this.MiningInfo != null)
				{
					foreach (GridDto gridDto in this.MiningInfo.Grids)
					{
						Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
						if (mining_oreBuild == null)
						{
							HLog.LogError(string.Format("Table Mining_ore not found id={0}", gridDto.OreBuildId));
						}
						else if (mining_oreBuild.oreType == 102)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		public bool IsTreasureCanGet
		{
			get
			{
				return this.MiningInfo != null && this.MiningInfo.OpenTreasure == 1;
			}
		}

		public bool IsTreasureGet
		{
			get
			{
				return this.MiningInfo != null && this.MiningInfo.OpenTreasure == 2;
			}
		}

		public int GetName()
		{
			return 160;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void Reset()
		{
		}

		public void UpdateMiningInfo(MiningInfoDto infODto)
		{
			if (infODto == null)
			{
				return;
			}
			this.MiningInfo = infODto;
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.Mining.Ore", true);
			this.MiningCounter.UpdateVariable(6024732);
		}

		public void UpdateMiningDrawInfo(MiningDrawDto drawDto)
		{
			if (drawDto == null)
			{
				return;
			}
			this.MiningDraw = drawDto;
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.Mining.Draw", true);
		}

		public void SetAutoOpt(ulong autoOpt)
		{
			if (this.MiningInfo != null)
			{
				this.MiningInfo.AutoOpt = autoOpt;
			}
		}

		public MiningGrid GetGridType()
		{
			if (this.MiningInfo != null)
			{
				foreach (Mining_miningBase mining_miningBase in GameApp.Table.GetManager().GetMining_miningBaseModelInstance().GetAllElements())
				{
					if (mining_miningBase.floor.Length >= 2)
					{
						int num = mining_miningBase.floor[0];
						int num2 = mining_miningBase.floor[1];
						if (this.MiningInfo.Stage >= num && this.MiningInfo.Stage < num2)
						{
							return (MiningGrid)mining_miningBase.gridType;
						}
					}
				}
				return MiningGrid.Grid6x5;
			}
			return MiningGrid.Grid6x5;
		}

		public List<MiningTreasureGridData> GetTreasureGrids()
		{
			List<MiningTreasureGridData> list = new List<MiningTreasureGridData>();
			if (this.MiningInfo != null)
			{
				int num = 9;
				for (int i = 0; i < num; i++)
				{
					list.Add(new MiningTreasureGridData
					{
						serverPos = 0,
						treasurePos = i + 1
					});
				}
				for (int j = 0; j < this.MiningInfo.Grids.Count; j++)
				{
					GridDto gridDto = this.MiningInfo.Grids[j];
					if (gridDto.TreasurePos != 0)
					{
						for (int k = 0; k < list.Count; k++)
						{
							MiningTreasureGridData miningTreasureGridData = list[k];
							if (miningTreasureGridData.treasurePos == gridDto.TreasurePos)
							{
								miningTreasureGridData.serverPos = gridDto.Pos;
								break;
							}
						}
					}
				}
			}
			return list;
		}

		public bool IsNotGetReward()
		{
			if (this.MiningInfo != null)
			{
				for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
				{
					GridDto gridDto = this.MiningInfo.Grids[i];
					if (gridDto.Status != 1 && gridDto.Floors <= 0 && gridDto.Item != null && gridDto.Item.Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<GridDto> GetSortedGridDtoList()
		{
			if (this.MiningInfo != null)
			{
				List<GridDto> list = this.MiningInfo.Grids.ToList<GridDto>();
				list.Sort((GridDto a, GridDto b) => a.Pos.CompareTo(b.Pos));
				return list;
			}
			return new List<GridDto>();
		}

		public GridDto GetGridDto(int pos)
		{
			if (this.MiningInfo != null)
			{
				for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
				{
					GridDto gridDto = this.MiningInfo.Grids[i];
					if (gridDto.Pos == pos)
					{
						return gridDto;
					}
				}
			}
			return null;
		}

		public GridDto GetTreasureFirstGridDto()
		{
			List<GridDto> list = new List<GridDto>();
			if (this.MiningInfo != null)
			{
				for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
				{
					GridDto gridDto = this.MiningInfo.Grids[i];
					Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
					if (mining_oreBuild != null && mining_oreBuild.oreResId == this.MiningInfo.TreasureResId)
					{
						list.Add(gridDto);
					}
				}
			}
			if (list.Count > 0)
			{
				list.Sort((GridDto a, GridDto b) => a.Pos.CompareTo(b.Pos));
				return list[0];
			}
			return null;
		}

		public GridDto GetTreasureEndGridDto()
		{
			List<GridDto> list = new List<GridDto>();
			if (this.MiningInfo != null)
			{
				for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
				{
					GridDto gridDto = this.MiningInfo.Grids[i];
					Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
					if (mining_oreBuild != null && mining_oreBuild.oreResId == this.MiningInfo.TreasureResId)
					{
						list.Add(gridDto);
					}
				}
			}
			if (list.Count > 0)
			{
				list.Sort((GridDto a, GridDto b) => a.Pos.CompareTo(b.Pos));
				List<GridDto> list2 = list;
				return list2[list2.Count - 1];
			}
			return null;
		}

		public bool IsOpenAnyTreasureGird()
		{
			List<GridDto> list = new List<GridDto>();
			if (this.MiningInfo != null)
			{
				for (int i = 0; i < this.MiningInfo.Grids.Count; i++)
				{
					GridDto gridDto = this.MiningInfo.Grids[i];
					Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
					if (mining_oreBuild != null && mining_oreBuild.oreResId == this.MiningInfo.TreasureResId)
					{
						list.Add(gridDto);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Status <= 0)
				{
					return true;
				}
			}
			return false;
		}

		public List<GridDto> GetAutoMiningList()
		{
			List<GridDto> list = new List<GridDto>();
			List<GridDto> sortedGridDtoList = this.GetSortedGridDtoList();
			GridDto treasureEndGridDto = this.GetTreasureEndGridDto();
			if (treasureEndGridDto != null)
			{
				for (int i = 0; i < sortedGridDtoList.Count; i++)
				{
					GridDto gridDto = sortedGridDtoList[i];
					list.Add(gridDto);
					if (gridDto.Pos == treasureEndGridDto.Pos)
					{
						break;
					}
				}
				return list;
			}
			return new List<GridDto>();
		}

		public float cacheTime { get; private set; }

		public void CacheTicket()
		{
			TicketDataModule dataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.cacheTicket.UpdateVariable(dataModule.GetTicketCount(UserTicketKind.Mining));
			this.cacheTime = Time.time;
		}

		public void UseCacheTicket()
		{
			if (this.cacheTicket.IsDataValid() && this.cacheTicket.mVariable > 0)
			{
				int num = this.cacheTicket.mVariable - 1;
				this.cacheTicket.UpdateVariable(num);
			}
			this.cacheTime = Time.time;
		}

		public void CacheMiningPos(int pos)
		{
			int num = this.MiningCounter.mVariable + 1;
			this.MiningCounter.UpdateVariable(num);
			if (this.MiningInfo == null)
			{
				return;
			}
			this.cachePos.Add(pos);
			PlayerPrefsKeys.CacheMiningPosition(this.MiningInfo.Stage, this.cachePos);
		}

		public void ClearCachePos()
		{
			if (this.MiningInfo == null)
			{
				return;
			}
			this.cachePos.Clear();
			PlayerPrefsKeys.ClearCacheMiningPos(this.MiningInfo.Stage);
		}

		public List<int> GetCacheMiningPos()
		{
			string cacheMiningPosition = PlayerPrefsKeys.GetCacheMiningPosition(this.MiningInfo.Stage);
			if (!string.IsNullOrEmpty(cacheMiningPosition))
			{
				return cacheMiningPosition.GetListInt('|');
			}
			return new List<int>();
		}

		public void AsyncCachePos()
		{
			if (this.MiningInfo == null || this.MiningInfo.Grids == null)
			{
				return;
			}
			List<int> cacheMiningPos = this.GetCacheMiningPos();
			if (cacheMiningPos.Count > 0)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < cacheMiningPos.Count; i++)
				{
					foreach (GridDto gridDto in this.MiningInfo.Grids)
					{
						if (cacheMiningPos[i] == gridDto.Pos)
						{
							if (gridDto.Status > 0)
							{
								list.Add(cacheMiningPos[i]);
								break;
							}
							break;
						}
					}
				}
				NetworkUtils.Mining.DoMiningRequest(2U, list, delegate(bool result, DoMiningResponse response)
				{
					if (result)
					{
						this.ClearCachePos();
					}
				});
			}
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			if (eventArgsFunctionOpen.FunctionID == 55)
			{
				RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.Mining", true);
			}
		}

		public bool IsMiningDisabled()
		{
			return !this.MiningCounter.IsDataValid() || this.MiningCounter.mVariable > 6024739;
		}

		private List<int> cachePos = new List<int>();

		public readonly SecureVariable MiningCounter = new SecureVariable();

		private const int MiningCounterStart = 6024732;

		private const int MingCounterLimit = 6024739;

		public readonly SecureVariable cacheTicket = new SecureVariable();
	}
}
