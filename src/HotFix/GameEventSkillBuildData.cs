using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventSkillBuildData
	{
		public int id { get; private set; }

		public int groupId { get; private set; }

		public int level { get; private set; }

		public int costType { get; private set; }

		public int cost { get; private set; }

		public int weight { get; private set; }

		public int tag { get; private set; }

		public int skillId { get; private set; }

		public int[] composeArr { get; private set; }

		public SkillBuildComposeType composeType { get; private set; }

		public int[] showCompose { get; private set; }

		public string skillAtlas { get; private set; }

		public string skillIcon { get; private set; }

		public string skillIconBadge { get; private set; }

		public string skillName
		{
			get
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.SkillConfig.nameID);
			}
		}

		public string skillInfo
		{
			get
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.SkillConfig.infoID);
			}
		}

		public string skillDetailInfo
		{
			get
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.SkillConfig.infoDetailID);
			}
		}

		public string skillFullDetail
		{
			get
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.SkillConfig.fullDetailID);
			}
		}

		public SkillBuildQuality quality { get; private set; }

		public GameSkillBuild_skillBuild config
		{
			get
			{
				return this._table;
			}
		}

		public GameSkill_skill SkillConfig
		{
			get
			{
				return this._skillConfig;
			}
		}

		public bool IsComposeSkill
		{
			get
			{
				return this.composeType > SkillBuildComposeType.None;
			}
		}

		public List<int> composeSkillList
		{
			get
			{
				return this.composeArr.ToList<int>();
			}
		}

		public void SetTable(GameSkillBuild_skillBuild table)
		{
			this._table = table;
			this.id = table.id;
			this.groupId = table.groupId;
			this.level = table.level;
			this.costType = ((table.cost.Length != 0) ? table.cost[0] : 0);
			this.cost = ((table.cost.Length > 1) ? table.cost[1] : 0);
			this.weight = table.weight;
			this.tag = table.tag;
			this.skillId = table.skillId;
			this.composeArr = table.compose;
			this.composeType = (SkillBuildComposeType)table.composeType;
			this.quality = (SkillBuildQuality)table.quality;
			this.showCompose = table.showCompose;
			this.attributeList = table.attributes.GetMergeAttributeData();
			this._skillConfig = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.skillId);
			if (this.SkillConfig == null)
			{
				HLog.LogError(string.Format("GameEventSkillBuildData.SetTable: Not found table id={0}", this.skillId));
				return;
			}
			this.skillAtlas = GameApp.Table.GetAtlasPath(this.SkillConfig.iconAtlasID);
			this.skillIcon = this.SkillConfig.icon;
			this.skillIconBadge = this.SkillConfig.iconBadge;
		}

		public bool IsHaveComposeSkill(int buildId)
		{
			for (int i = 0; i < this.composeArr.Length; i++)
			{
				if (this.composeArr[i] == buildId)
				{
					return true;
				}
			}
			return false;
		}

		public static string GetQuality(SkillBuildQuality quality)
		{
			switch (quality)
			{
			case SkillBuildQuality.Gray:
				return "item_frame_1";
			case SkillBuildQuality.Gold:
				return "item_frame_5";
			case SkillBuildQuality.Red:
				return "item_frame_6";
			case SkillBuildQuality.Colorful:
				return "item_frame_7";
			default:
				return "item_frame_1";
			}
		}

		public static string GetQualityColor(SkillBuildQuality quality, string info)
		{
			switch (quality)
			{
			case SkillBuildQuality.Gray:
				return info;
			case SkillBuildQuality.Gold:
				return info;
			case SkillBuildQuality.Red:
			case SkillBuildQuality.Colorful:
				return "<color=#fff27b>" + info + "</color>";
			default:
				return info;
			}
		}

		public static string GetQualityBg(SkillBuildQuality quality)
		{
			switch (quality)
			{
			case SkillBuildQuality.Gray:
				return "skill_bg_1";
			case SkillBuildQuality.Gold:
				return "skill_bg_2";
			case SkillBuildQuality.Red:
				return "skill_bg_3";
			case SkillBuildQuality.Colorful:
				return "skill_bg_4";
			default:
				return "skill_bg_1";
			}
		}

		public static string GetQualityDetailBg(SkillBuildQuality quality)
		{
			switch (quality)
			{
			case SkillBuildQuality.Gray:
				return "skill_bg_1";
			case SkillBuildQuality.Gold:
				return "skill_bg_2";
			case SkillBuildQuality.Red:
				return "skill_bg_3";
			case SkillBuildQuality.Colorful:
				return "skill_bg_4";
			default:
				return "skill_bg_1";
			}
		}

		public static int GetQualitySoundId(SkillBuildQuality quality)
		{
			switch (quality)
			{
			case SkillBuildQuality.Gray:
				return 679;
			case SkillBuildQuality.Gold:
				return 680;
			case SkillBuildQuality.Red:
				return 681;
			case SkillBuildQuality.Colorful:
				return 684;
			default:
				return 679;
			}
		}

		public List<MergeAttributeData> GetMergeAttributeData()
		{
			return this.attributeList;
		}

		private List<MergeAttributeData> attributeList;

		private GameSkillBuild_skillBuild _table;

		private GameSkill_skill _skillConfig;

		private string coinNameId;

		private string coinDesId;
	}
}
