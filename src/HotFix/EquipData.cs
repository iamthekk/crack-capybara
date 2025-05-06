using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public class EquipData
	{
		public int GetEvolutionId()
		{
			return this.tagId * 100 + this.evolution;
		}

		public int GetEvolutionId(int evolution)
		{
			return this.tagId * 100 + evolution;
		}

		public int GetUpdateLevelId()
		{
			return (int)(this.equipType * (EquipType)10000 + (int)this.level);
		}

		public int GetUpdateLevelId(int level)
		{
			return (int)(this.equipType * (EquipType)10000 + level);
		}

		public EquipData SetEquipData(EquipmentDto data)
		{
			this.rowID = data.RowId;
			this.id = data.EquipId;
			this.level = data.Level;
			this.exp = data.Exp;
			this.evolution = Mathf.Clamp((int)data.Evolution, 1, int.MaxValue);
			Equip_equip equip_equip = GameApp.Table.GetManager().GetEquip_equip((int)this.id);
			if (equip_equip == null)
			{
				HLog.LogError(string.Format("EquipData.quality:[{0}] == null", this.id));
				return null;
			}
			this.tagId = equip_equip.tagID;
			this.equipType = (EquipType)equip_equip.Type;
			this.composeId = equip_equip.composeId;
			this.rank = equip_equip.rank;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("EquipData.itemData:[{0}] == null", this.id));
			}
			this.nameId = elementById.nameID;
			Equip_equipCompose equip_equipCompose = GameApp.Table.GetManager().GetEquip_equipCompose(this.composeId);
			this.qualityStr = Singleton<LanguageManager>.Instance.GetInfoByID(equip_equipCompose.nameID);
			this.qualityColor = equip_equipCompose.qualityColor;
			return this;
		}

		public void CloneFrom(EquipData ed)
		{
			if (ed == null)
			{
				return;
			}
			this.rowID = ed.rowID;
			this.id = ed.id;
			this.count = ed.count;
			this.level = ed.level;
			this.exp = ed.exp;
			this.equipType = ed.equipType;
			this.composeId = ed.composeId;
			this.qualityStr = ed.qualityStr;
			this.qualityColor = ed.qualityColor;
			this.nameId = ed.nameId;
			this.evolution = ed.evolution;
			this.tagId = ed.tagId;
		}

		public bool IsFullLevel()
		{
			int updateLevelId = this.GetUpdateLevelId();
			if (GameApp.Table.GetManager().GetEquip_updateLevel(updateLevelId + 1) == null)
			{
				return true;
			}
			int evolutionId = this.GetEvolutionId();
			Equip_equipEvolution equip_equipEvolution = GameApp.Table.GetManager().GetEquip_equipEvolution(evolutionId);
			return (ulong)this.level >= (ulong)((long)equip_equipEvolution.maxLevel);
		}

		public bool IsCanEvolution()
		{
			int evolutionId = this.GetEvolutionId();
			Equip_equipEvolution equip_equipEvolution = GameApp.Table.GetManager().GetEquip_equipEvolution(evolutionId);
			return equip_equipEvolution.nextID > 0 && (ulong)this.level >= (ulong)((long)equip_equipEvolution.maxLevel);
		}

		public int GetMaxLevel()
		{
			int evolutionId = this.GetEvolutionId();
			return GameApp.Table.GetManager().GetEquip_equipEvolution(evolutionId).maxLevel;
		}

		public override string ToString()
		{
			return string.Format("ID:{0},RowID:{1},Count:{2},Level:{3},Exp:{4}", new object[] { this.id, this.rowID, this.count, this.level, this.exp });
		}

		public static List<EquipData> EquipListFix(RepeatedField<EquipmentDto> equips)
		{
			List<EquipData> list = new List<EquipData>();
			for (int i = 0; i < 6; i++)
			{
				list.Add(null);
			}
			for (int j = 0; j < equips.Count; j++)
			{
				EquipData equipData = equips[j].ToEquipData();
				list[j] = equipData;
			}
			return list;
		}

		public ulong rowID;

		public uint id;

		public int count = 1;

		public uint level = 1U;

		public uint exp;

		public EquipType equipType;

		public int composeId;

		public string qualityStr;

		public int qualityColor;

		public string nameId;

		public int evolution;

		public int tagId;

		public int rank;

		public bool isNew;
	}
}
