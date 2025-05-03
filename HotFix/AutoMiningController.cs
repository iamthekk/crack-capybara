using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;

namespace HotFix
{
	public class AutoMiningController
	{
		public AutoMiningMode AutoMode { get; private set; }

		public bool IsAutoMining { get; private set; }

		private MiningDataModule miningDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.MiningDataModule);
			}
		}

		private TicketDataModule ticketDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.TicketDataModule);
			}
		}

		public AutoMiningController(MiningViewModule miningViewModule)
		{
			this.miningViewModule = miningViewModule;
		}

		public void StartAutoMining(AutoMiningMode mode)
		{
			this.AutoMode = mode;
			this.IsAutoMining = true;
			this.AutoMining();
		}

		public void EndAutoMining()
		{
			this.IsAutoMining = false;
		}

		private void AutoMining()
		{
			if (this.CheckEnd())
			{
				return;
			}
			this.CheckStep();
		}

		private bool CheckEnd()
		{
			AutoMiningMode autoMode = this.AutoMode;
			if (autoMode != AutoMiningMode.NextFloor)
			{
				if (autoMode == AutoMiningMode.Treasure)
				{
					if (this.miningDataModule.IsHaveTreasure && (this.miningDataModule.IsTreasureCanGet || this.miningDataModule.IsTreasureGet))
					{
						string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("uimining_treasure_finish"), Array.Empty<object>());
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Confirm");
						this.ShowResult(text, infoByID);
						this.MiningComplete(false);
						return true;
					}
				}
			}
			else if (this.miningDataModule.IsHaveKey && this.miningDataModule.IsHaveDoor)
			{
				string text2 = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("uimining_next_finish"), Array.Empty<object>());
				string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Confirm");
				this.ShowResult(text2, infoByID2);
				this.MiningComplete(false);
				return true;
			}
			return false;
		}

		private void CheckStep()
		{
			UserTicket ticket = this.ticketDataModule.GetTicket(UserTicketKind.Mining);
			if (ticket == null || ticket.NewNum == 0U)
			{
				this.ShowNotEnoughTip();
				this.MiningComplete(false);
				return;
			}
			List<int> list = new List<int>();
			List<GridDto> autoMiningList = this.miningDataModule.GetAutoMiningList();
			int num = 0;
			while (num < autoMiningList.Count && (long)list.Count < (long)((ulong)ticket.NewNum))
			{
				GridDto gridDto = autoMiningList[num];
				if (gridDto.Status != 0 || gridDto.Floors != 0)
				{
					Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
					if (mining_oreBuild != null)
					{
						if (mining_oreBuild.oreType == 4 || mining_oreBuild.oreType == 5 || mining_oreBuild.oreType == 6)
						{
							if (list.Count <= 0)
							{
								list.Add(gridDto.Pos);
								break;
							}
							if (gridDto.Status > 0)
							{
								list.Add(gridDto.Pos);
								break;
							}
							break;
						}
						else
						{
							list.Add(gridDto.Pos);
						}
					}
				}
				num++;
			}
			if (list.Count > 0)
			{
				NetworkUtils.Mining.DoMiningRequest(1U, list, delegate(bool result, DoMiningResponse response)
				{
					if (result)
					{
						this.AutoMiningAnimation(list);
						return;
					}
					this.MiningComplete(true);
				});
				return;
			}
			this.MiningComplete(true);
		}

		private void MiningStepFinish()
		{
			if (this.CheckEnd())
			{
				return;
			}
			if (this.IsAutoMining)
			{
				this.CheckStep();
				return;
			}
			this.MiningComplete(true);
		}

		private void MiningComplete(bool showTip = true)
		{
			if (showTip)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_card_complete");
				GameApp.View.ShowStringTip(infoByID);
				NetworkUtils.Mining.DoGetMiningRewardRequest(delegate(bool result, GetMiningRewardResponse response)
				{
					if (result)
					{
						GameApp.SDK.Analyze.Track_MiningMine_Reward(response.CommonData.Reward);
					}
				}, null);
			}
			this.IsAutoMining = false;
			this.miningViewModule.AutoEnd();
		}

		private void ShowResult(string context, string ok)
		{
			DxxTools.UI.OpenPopCommon(context, delegate(int id)
			{
				NetworkUtils.Mining.DoGetMiningRewardRequest(delegate(bool result, GetMiningRewardResponse response)
				{
					if (result)
					{
						GameApp.SDK.Analyze.Track_MiningMine_Reward(response.CommonData.Reward);
					}
				}, null);
			}, string.Empty, ok, string.Empty, false, 2);
		}

		private async void AutoMiningAnimation(List<int> list)
		{
			if (!(this.miningViewModule == null))
			{
				List<GridDto> list2 = new List<GridDto>();
				for (int i = 0; i < list.Count; i++)
				{
					GridDto gridDto = this.miningDataModule.GetGridDto(list[i]);
					if (gridDto != null)
					{
						list2.Add(gridDto);
					}
				}
				await this.miningViewModule.AutoMiningAni(list2);
				int bomb = this.GetBomb();
				if (bomb > 0)
				{
					this.AutoBomb(bomb);
				}
				else
				{
					this.MiningStepFinish();
				}
			}
		}

		private int GetBomb()
		{
			List<GridDto> sortedGridDtoList = this.miningDataModule.GetSortedGridDtoList();
			for (int i = 0; i < sortedGridDtoList.Count; i++)
			{
				GridDto gridDto = sortedGridDtoList[i];
				if (gridDto.Status <= 0)
				{
					Mining_oreBuild mining_oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(gridDto.OreBuildId);
					if (mining_oreBuild != null && mining_oreBuild.oreType == 101 && gridDto.BombStatus == 1)
					{
						return gridDto.Pos;
					}
				}
			}
			return 0;
		}

		private void AutoBomb(int pos)
		{
			NetworkUtils.Mining.DoOpenBombRequest(pos, delegate(bool result, OpenBombResponse response)
			{
				if (result && this.miningViewModule != null)
				{
					this.BombTask(pos, response.Grids);
				}
			});
		}

		private async void BombTask(int pos, RepeatedField<GridDto> Grids)
		{
			List<GridDto> list = new List<GridDto>();
			for (int i = 0; i < Grids.Count; i++)
			{
				list.Add(Grids[i]);
			}
			await this.miningViewModule.AutoBombAni(pos, list);
			int bomb = this.GetBomb();
			if (bomb > 0)
			{
				this.AutoBomb(bomb);
			}
			else
			{
				this.MiningStepFinish();
			}
		}

		private void ShowNotEnoughTip()
		{
			string text = "";
			int mining_Ticket_ID = GameConfig.Mining_Ticket_ID;
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(mining_Ticket_ID);
			if (item_Item != null)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(item_Item.nameID);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_mining_disabled", new object[] { text });
			GameApp.View.ShowStringTip(infoByID);
		}

		private MiningViewModule miningViewModule;
	}
}
