using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Dungeon;
using UnityEngine;

namespace HotFix
{
	public class DungeonDataModule : IDataModule
	{
		public StartDungeonResponse DungeonResponse { get; private set; }

		public int Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public int GetName()
		{
			return 154;
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

		public void SetLoginData(RepeatedField<DungeonInfo> info)
		{
			this.DungeonInfo = info;
		}

		public void UpdateData(StartDungeonResponse response)
		{
			if (response == null)
			{
				return;
			}
			this.DungeonInfo = response.DungeonInfo;
			this.DungeonResponse = response;
		}

		public List<Dungeon_DungeonLevel> GetAllLevel(int dungeonId)
		{
			List<Dungeon_DungeonLevel> list = new List<Dungeon_DungeonLevel>();
			IList<Dungeon_DungeonLevel> allElements = GameApp.Table.GetManager().GetDungeon_DungeonLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].dungeonID == dungeonId)
				{
					list.Add(allElements[i]);
				}
			}
			list.Sort((Dungeon_DungeonLevel a, Dungeon_DungeonLevel b) => a.level.CompareTo(b.level));
			return list;
		}

		public Dungeon_DungeonLevel GetDungeonLevel(int dungeonId, int level)
		{
			List<Dungeon_DungeonLevel> allLevel = this.GetAllLevel(dungeonId);
			for (int i = 0; i < allLevel.Count; i++)
			{
				if (allLevel[i].level == level)
				{
					return allLevel[i];
				}
			}
			return null;
		}

		public List<ItemData> GetDungeonLevelRewards(int dungeonId, uint level)
		{
			List<ItemData> list = new List<ItemData>();
			Dungeon_DungeonLevel dungeon_DungeonLevel = null;
			List<Dungeon_DungeonLevel> allLevel = this.GetAllLevel(dungeonId);
			for (int i = 0; i < allLevel.Count; i++)
			{
				if ((long)allLevel[i].level == (long)((ulong)level))
				{
					dungeon_DungeonLevel = allLevel[i];
					break;
				}
			}
			if (dungeon_DungeonLevel != null)
			{
				for (int j = 0; j < dungeon_DungeonLevel.reward.Length; j++)
				{
					List<int> listInt = dungeon_DungeonLevel.reward[j].GetListInt(',');
					if (listInt.Count >= 2)
					{
						ItemData itemData = new ItemData(listInt[0], (long)listInt[1]);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public uint GetCurrentLevel(int dungeonId)
		{
			List<Dungeon_DungeonLevel> allLevel = this.GetAllLevel(dungeonId);
			List<Dungeon_DungeonLevel> list = allLevel;
			uint level = (uint)list[list.Count - 1].level;
			if (allLevel.Count > 0)
			{
				uint num = 0U;
				for (int i = 0; i < this.DungeonInfo.Count; i++)
				{
					if (this.DungeonInfo[i].DungeonId == (uint)dungeonId)
					{
						num = this.DungeonInfo[i].LevelId;
						break;
					}
				}
				int j = 0;
				while (j < allLevel.Count)
				{
					if ((long)allLevel[j].level == (long)((ulong)num))
					{
						if (j + 1 < allLevel.Count)
						{
							return (uint)allLevel[j + 1].level;
						}
						return level;
					}
					else
					{
						j++;
					}
				}
			}
			return 1U;
		}

		public uint GetFinishLevel(int dungeonId)
		{
			for (int i = 0; i < this.DungeonInfo.Count; i++)
			{
				if (this.DungeonInfo[i].DungeonId == (uint)dungeonId)
				{
					Dungeon_DungeonLevel dungeon_DungeonLevel = GameApp.Table.GetManager().GetDungeon_DungeonLevel((int)this.DungeonInfo[i].LevelId);
					if (dungeon_DungeonLevel != null)
					{
						return (uint)dungeon_DungeonLevel.level;
					}
				}
			}
			return 0U;
		}

		public UserTicketKind GetTicketKind(int dungeonId)
		{
			Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(dungeonId);
			if (elementById != null)
			{
				Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(elementById.keyID);
				if (elementById2 != null)
				{
					switch (elementById2.itemType)
					{
					case 24:
						return UserTicketKind.DragonLair;
					case 25:
						return UserTicketKind.AstralTree;
					case 26:
						return UserTicketKind.SwordIsland;
					case 27:
						return UserTicketKind.DeepSeaRuins;
					}
				}
			}
			return UserTicketKind.None;
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			FunctionID functionID = (FunctionID)eventArgsFunctionOpen.FunctionID;
			if (functionID - FunctionID.Main_Activity_DragonsLair <= 3)
			{
				RedPointController.Instance.ReCalc("DailyActivity", true);
			}
		}

		private RepeatedField<DungeonInfo> DungeonInfo;

		[SerializeField]
		private int m_duration = 15;
	}
}
