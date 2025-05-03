using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using HotFix.EventArgs;
using LocalModels.Bean;
using Proto.Common;
using Proto.LeaderBoard;
using Proto.Talents;
using Server;

namespace HotFix
{
	public class TalentLegacyDataModule : IDataModule
	{
		public List<TalentLegacy_talentLegacyEffect> GetTalentLegacySkillListById(int id)
		{
			List<TalentLegacy_talentLegacyEffect> list = new List<TalentLegacy_talentLegacyEffect>();
			IList<TalentLegacy_talentLegacyEffect> talentLegacy_talentLegacyEffectElements = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyEffectElements();
			for (int i = 0; i < talentLegacy_talentLegacyEffectElements.Count; i++)
			{
				if (talentLegacy_talentLegacyEffectElements[i].tagID == id)
				{
					list.Add(talentLegacy_talentLegacyEffectElements[i]);
				}
			}
			return list;
		}

		public TalentLegacy_talentLegacyEffect GetTalentLegacySkillCfgByLevel(int nodeId, int level)
		{
			if (level == 0)
			{
				level = 1;
			}
			return GameApp.Table.GetManager().GetTalentLegacy_talentLegacyEffect(nodeId * 100 + level);
		}

		public List<TalentLegacy_talentLegacyNode> GetCareerTalentLegacyListAllCfg(int careerId, int type = -1)
		{
			List<TalentLegacy_talentLegacyNode> list = new List<TalentLegacy_talentLegacyNode>();
			IList<TalentLegacy_talentLegacyNode> talentLegacy_talentLegacyNodeElements = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNodeElements();
			for (int i = 0; i < talentLegacy_talentLegacyNodeElements.Count; i++)
			{
				if (type == -1)
				{
					if (talentLegacy_talentLegacyNodeElements[i].career == careerId)
					{
						list.Add(talentLegacy_talentLegacyNodeElements[i]);
					}
				}
				else if (talentLegacy_talentLegacyNodeElements[i].career == careerId && talentLegacy_talentLegacyNodeElements[i].type == type)
				{
					list.Add(talentLegacy_talentLegacyNodeElements[i]);
				}
			}
			return list;
		}

		public int GetCareerTotalProgress(int careerId)
		{
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.GetCareerTalentLegacyListAllCfg(careerId, -1);
			int num = 0;
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				List<TalentLegacy_talentLegacyEffect> talentLegacySkillListById = this.GetTalentLegacySkillListById(careerTalentLegacyListAllCfg[i].id);
				num += talentLegacySkillListById.Count;
			}
			return num;
		}

		public TalentLegacy_talentLegacyNode GetShowTalentLegacyNodeByCareerId(int careerId)
		{
			int num = -1;
			List<int> list = new List<int>();
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.GetCareerTalentLegacyListAllCfg(careerId, -1);
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				if (careerTalentLegacyListAllCfg[i].type == 3)
				{
					if (num == -1)
					{
						num = careerTalentLegacyListAllCfg[i].id;
					}
					list.Add(careerTalentLegacyListAllCfg[i].id);
				}
			}
			if (this.m_LegacyInfo == null)
			{
				return null;
			}
			if (!this.m_LegacyInfo.CareerDic.ContainsKey(careerId))
			{
				return GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
			}
			TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_LegacyInfo.CareerDic[careerId];
			int num2 = 0;
			foreach (KeyValuePair<int, TalentLegacyDataModule.TalentLegacySkillInfo> keyValuePair in talentLegacyCareerInfo.SkillDic)
			{
				if (keyValuePair.Value.TalentLegacyNodeId > num2)
				{
					num2 = keyValuePair.Value.TalentLegacyNodeId;
				}
			}
			if (num2 <= num)
			{
				return GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (j + 1 >= list.Count)
				{
					return GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(list[j]);
				}
				if (num2 > list[j] && num2 <= list[j + 1])
				{
					return GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
				}
			}
			return null;
		}

		public Dictionary<int, List<TalentLegacy_talentLegacyNode>> GetTalentLegacyNodeTree(int careerId)
		{
			Dictionary<int, List<TalentLegacy_talentLegacyNode>> dictionary = new Dictionary<int, List<TalentLegacy_talentLegacyNode>>();
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.GetCareerTalentLegacyListAllCfg(careerId, -1);
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				int num = int.Parse(careerTalentLegacyListAllCfg[i].pos[0]);
				if (dictionary.ContainsKey(num))
				{
					dictionary[num].Add(careerTalentLegacyListAllCfg[i]);
				}
				else
				{
					dictionary.Add(num, new List<TalentLegacy_talentLegacyNode> { careerTalentLegacyListAllCfg[i] });
				}
			}
			return dictionary;
		}

		public RepeatedField<RankUserDto> Top3User { get; private set; }

		public int GetName()
		{
			return 165;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void OnInit()
		{
			NetworkUtils.TalentLegacy.DoTalentLegacyInfoRequest(null, true);
			NetworkUtils.DoRankRequest(RankType.NewWorld, 1, false, false, null);
			this.OnInitTree();
		}

		private void OnInitTree()
		{
			this.m_treeDic.Clear();
			IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
			for (int i = 0; i < talentLegacy_careerElements.Count; i++)
			{
				this.m_treeDic.Add(talentLegacy_careerElements[i].id, this.GetTalentLegacyNodeTree(talentLegacy_careerElements[i].id));
			}
		}

		public Dictionary<int, List<TalentLegacy_talentLegacyNode>> OnGetTree(int careerId)
		{
			if (this.m_treeDic.ContainsKey(careerId))
			{
				return this.m_treeDic[careerId];
			}
			return null;
		}

		public void OnUpdateRankData(LeaderBoardResponse resp)
		{
			if (resp.Top3 != null)
			{
				this.RankSaveTimeStamp = DxxTools.Time.ServerTimestamp;
				this.Top3User = resp.Top3;
			}
		}

		public void OnSetRankInfo(RepeatedField<UserRankInfoSimpleDto> infoDtos)
		{
			if (this.m_RankInfo == null)
			{
				this.m_RankInfo = new TalentLegacyDataModule.TalentLegacyRankInfo(infoDtos);
				return;
			}
			this.m_RankInfo.SetData(infoDtos);
		}

		public void OnSetTalentLegacyInfo(TalentLegacyInfoResponse dto)
		{
			if (this.m_LegacyInfo == null)
			{
				this.m_LegacyInfo = new TalentLegacyDataModule.TalentLegacyInfo(dto);
			}
			else
			{
				this.m_LegacyInfo.SetData(dto);
			}
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(this, 145, null);
		}

		public void OnTalentLegacyLevelUpBack(TalentLegacyLevelUpResponse dto)
		{
			if (this.m_LegacyInfo != null)
			{
				this.m_LegacyInfo.OnSetDataCareerInfoData(dto.CareerInfo);
			}
		}

		public void OnTalentLegacySkillSwitchBack(TalentLegacySwitchResponse dto)
		{
			if (this.m_LegacyInfo != null)
			{
				this.m_LegacyInfo.OnSetDataCareerInfoData(dto.CareerInfo);
			}
		}

		public void OnTalentLegacyLevelUpCoolDownBack(TalentLegacyLevelUpCoolDownResponse dto)
		{
			if (this.m_LegacyInfo != null)
			{
				this.m_LegacyInfo.OnSetDataCareerInfoData(dto.CareerInfo);
				if (this.m_LegacyInfo.CareerDic.ContainsKey(dto.CareerInfo.CareerId))
				{
					this.m_LegacyInfo.AssemblySlotCount = dto.AssemblySlotCount;
				}
			}
		}

		public void OnTalentLegacySelectCareerBack(TalentLegacySelectCareerResponse dto)
		{
			if (this.m_LegacyInfo != null)
			{
				this.m_LegacyInfo.SelectCareerId = dto.CareerId;
			}
		}

		public bool IsContainUnlockCareer(int careerId, int nodeId)
		{
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.OnGetTalentLegacySkillInfo(careerId, nodeId);
			if (talentLegacySkillInfo == null)
			{
				return false;
			}
			IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
			for (int i = 0; i < talentLegacy_careerElements.Count; i++)
			{
				if (talentLegacy_careerElements[i].condition.Length > 0)
				{
					int num = int.Parse(talentLegacy_careerElements[i].condition.Split(',', StringSplitOptions.None)[0]);
					int num2 = int.Parse(talentLegacy_careerElements[i].condition.Split(',', StringSplitOptions.None)[1]);
					if (num == nodeId && talentLegacySkillInfo.Level >= num2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public TalentLegacyDataModule.TalentLegacyRankInfo OnGetRankInfo()
		{
			return this.m_RankInfo;
		}

		public TalentLegacyDataModule.TalentLegacyInfo OnGetTalentLegacyInfo()
		{
			return this.m_LegacyInfo;
		}

		public TalentLegacyDataModule.TalentLegacyCareerInfo OnGetTalentLegacyCareerInfo(int careerId)
		{
			if (this.m_LegacyInfo == null)
			{
				return null;
			}
			if (this.m_LegacyInfo.CareerDic.ContainsKey(careerId))
			{
				return this.m_LegacyInfo.CareerDic[careerId];
			}
			return null;
		}

		public ValueTuple<int, int> IsHaveStudyingNode()
		{
			if (this.m_LegacyInfo == null)
			{
				return new ValueTuple<int, int>(-1, -1);
			}
			foreach (KeyValuePair<int, TalentLegacyDataModule.TalentLegacyCareerInfo> keyValuePair in this.m_LegacyInfo.CareerDic)
			{
				foreach (KeyValuePair<int, TalentLegacyDataModule.TalentLegacySkillInfo> keyValuePair2 in keyValuePair.Value.SkillDic)
				{
					if (keyValuePair2.Value.LevelUpTime > 0L)
					{
						return new ValueTuple<int, int>(keyValuePair.Value.CareerId, keyValuePair2.Value.TalentLegacyNodeId);
					}
				}
			}
			return new ValueTuple<int, int>(-1, -1);
		}

		public TalentLegacyDataModule.TalentLegacySkillInfo OnGetTalentLegacySkillInfo(int careerId, int skillId)
		{
			if (this.m_LegacyInfo == null)
			{
				return null;
			}
			if (this.m_LegacyInfo.CareerDic.ContainsKey(careerId))
			{
				TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_LegacyInfo.CareerDic[careerId];
				if (talentLegacyCareerInfo.SkillDic.ContainsKey(skillId))
				{
					return talentLegacyCareerInfo.SkillDic[skillId];
				}
			}
			return null;
		}

		public bool IsUnlockTalentLegacyCareer(int careerId)
		{
			TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(careerId);
			if (talentLegacy_career == null)
			{
				return false;
			}
			string condition = talentLegacy_career.condition;
			if (string.IsNullOrEmpty(condition))
			{
				return true;
			}
			int num = int.Parse(condition.Split(',', StringSplitOptions.None)[0]);
			int num2 = int.Parse(condition.Split(',', StringSplitOptions.None)[1]);
			if (this.m_LegacyInfo == null)
			{
				return false;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
			if (talentLegacy_talentLegacyNode == null)
			{
				return false;
			}
			if (this.m_LegacyInfo.CareerDic.ContainsKey(talentLegacy_talentLegacyNode.career))
			{
				TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_LegacyInfo.CareerDic[talentLegacy_talentLegacyNode.career];
				if (talentLegacyCareerInfo.SkillDic.ContainsKey(num) && talentLegacyCareerInfo.SkillDic[num].Level >= num2)
				{
					return true;
				}
			}
			return false;
		}

		public string GetCareerTotalFinishProgress(int careerId)
		{
			if (this.m_LegacyInfo == null)
			{
				return "";
			}
			string text = "";
			if (this.m_LegacyInfo.CareerDic.ContainsKey(careerId))
			{
				List<TalentLegacyDataModule.TalentLegacySkillInfo> list = this.m_LegacyInfo.CareerDic[careerId].SkillDic.Values.ToList<TalentLegacyDataModule.TalentLegacySkillInfo>();
				list.Sort((TalentLegacyDataModule.TalentLegacySkillInfo a, TalentLegacyDataModule.TalentLegacySkillInfo b) => a.TalentLegacyNodeId - b.TalentLegacyNodeId);
				for (int i = 0; i < list.Count; i++)
				{
					TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(list[i].TalentLegacyNodeId);
					if (talentLegacy_talentLegacyNode != null && !string.IsNullOrEmpty(talentLegacy_talentLegacyNode.romeNumber) && this.GetTalentLegacySkillCfgByLevel(list[i].TalentLegacyNodeId, list[i].Level + 1) == null)
					{
						text = talentLegacy_talentLegacyNode.romeNumber;
					}
				}
			}
			return text;
		}

		public bool IsUnlockTalentLegacyNode(int nodeId)
		{
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(nodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return true;
			}
			if (talentLegacy_talentLegacyNode.condition.Length == 0)
			{
				return true;
			}
			if (this.m_LegacyInfo == null)
			{
				return false;
			}
			for (int i = 0; i < talentLegacy_talentLegacyNode.condition.Length; i++)
			{
				int num = int.Parse(talentLegacy_talentLegacyNode.condition[i].Split(',', StringSplitOptions.None)[0]);
				int num2 = int.Parse(talentLegacy_talentLegacyNode.condition[i].Split(',', StringSplitOptions.None)[1]);
				TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode2 = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
				if (talentLegacy_talentLegacyNode2 == null)
				{
					return false;
				}
				if (!this.m_LegacyInfo.CareerDic.ContainsKey(talentLegacy_talentLegacyNode2.career))
				{
					return false;
				}
				TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_LegacyInfo.CareerDic[talentLegacy_talentLegacyNode2.career];
				if (!talentLegacyCareerInfo.SkillDic.ContainsKey(num))
				{
					return false;
				}
				if (talentLegacyCareerInfo.SkillDic[num].Level < num2)
				{
					return false;
				}
			}
			return true;
		}

		public TalentLegacyDataModule.ELegacySkillEquipState GetSkillEquipState(int targetCareerId, int nodeId, int tabCareerId, int index)
		{
			if (this.m_LegacyInfo == null)
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.UnActive;
			}
			if (!this.m_LegacyInfo.CareerDic.ContainsKey(tabCareerId))
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.UnActive;
			}
			if (!this.m_LegacyInfo.CareerDic.ContainsKey(targetCareerId))
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.UnEquip;
			}
			bool flag = false;
			bool flag2 = false;
			List<int> assemblyTalentLegacySkillIdList = this.m_LegacyInfo.CareerDic[targetCareerId].AssemblyTalentLegacySkillIdList;
			int i = 0;
			while (i < assemblyTalentLegacySkillIdList.Count)
			{
				if (nodeId == assemblyTalentLegacySkillIdList[i])
				{
					if (i == index)
					{
						flag = true;
						break;
					}
					flag2 = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (flag)
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.Equip;
			}
			if (flag2)
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.UnDown;
			}
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.OnGetTalentLegacySkillInfo(tabCareerId, nodeId);
			if (talentLegacySkillInfo != null && talentLegacySkillInfo.Level >= 1)
			{
				return TalentLegacyDataModule.ELegacySkillEquipState.UnEquip;
			}
			return TalentLegacyDataModule.ELegacySkillEquipState.UnActive;
		}

		public int OnGetTalentLegacyCareerRed(int careerId, int nodeId = -1, bool isSkipCheckStudying = false)
		{
			if (!GameApp.Data.GetDataModule(DataName.TalentDataModule).IsCheckMaxLevel() || !Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
			{
				return 0;
			}
			TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(careerId);
			bool flag = this.IsUnlockTalentLegacyCareer(careerId);
			ValueTuple<int, int> valueTuple = this.IsHaveStudyingNode();
			if (flag && talentLegacy_career.isOpen == 1)
			{
				List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.GetCareerTalentLegacyListAllCfg(careerId, -1);
				for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
				{
					bool flag2 = false;
					if (nodeId != -1)
					{
						if (nodeId != careerTalentLegacyListAllCfg[i].id)
						{
							flag2 = true;
						}
						else if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1 && valueTuple.Item2 != careerTalentLegacyListAllCfg[i].id && !isSkipCheckStudying)
						{
							flag2 = true;
						}
					}
					else if (valueTuple.Item1 != -1 && valueTuple.Item1 != careerId && !isSkipCheckStudying)
					{
						break;
					}
					if (!flag2 && this.IsUnlockTalentLegacyNode(careerTalentLegacyListAllCfg[i].id))
					{
						TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.OnGetTalentLegacySkillInfo(careerTalentLegacyListAllCfg[i].career, careerTalentLegacyListAllCfg[i].id);
						if (talentLegacySkillInfo == null)
						{
							int num = this.OnGetTalentLegacyNodeStudyRed(careerTalentLegacyListAllCfg[i].id, 1);
							if (num == 1)
							{
								return num;
							}
						}
						else if (talentLegacySkillInfo.LevelUpTime <= 0L)
						{
							int num2 = this.OnGetTalentLegacyNodeStudyRed(careerTalentLegacyListAllCfg[i].id, talentLegacySkillInfo.Level + 1);
							if (num2 == 1)
							{
								return num2;
							}
						}
						else
						{
							AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
							int maxTimes = dataModule.GetMaxTimes(14);
							if (dataModule.GetWatchTimes(14) < maxTimes)
							{
								return 1;
							}
							if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(47UL) > 0L)
							{
								return 1;
							}
						}
					}
				}
			}
			return 0;
		}

		private int OnGetTalentLegacyNodeStudyRed(int nodeId, int level)
		{
			ValueTuple<int, int> valueTuple = this.IsHaveStudyingNode();
			List<ItemData> list = new List<ItemData>();
			TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.GetTalentLegacySkillCfgByLevel(nodeId, level);
			if (talentLegacySkillCfgByLevel != null)
			{
				list.AddRange(talentLegacySkillCfgByLevel.levelupCost.ToItemDataList());
				bool flag = true;
				for (int i = 0; i < list.Count; i++)
				{
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)list[i].ID)) < list[i].TotalCount)
					{
						flag = false;
						break;
					}
				}
				if (flag && valueTuple.Item1 == -1 && valueTuple.Item2 == -1)
				{
					return 1;
				}
			}
			return 0;
		}

		public int OnGetTalentLegacySkillRed(int targetCareerId, int tabCareerId, int index)
		{
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.GetCareerTalentLegacyListAllCfg(tabCareerId, 3);
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				if (this.GetSkillEquipState(targetCareerId, careerTalentLegacyListAllCfg[i].id, careerTalentLegacyListAllCfg[i].career, index) == TalentLegacyDataModule.ELegacySkillEquipState.UnEquip)
				{
					return 1;
				}
			}
			return 0;
		}

		public void SetTalentChange(CustomText textTalent, CustomImage imageTalent)
		{
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false);
			textTalent.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("legacy_main_title") : Singleton<LanguageManager>.Instance.GetInfoByID("UIMain_Talent"));
			imageTalent.SetImage(100, flag ? "tab_icon_talentLegacy" : "tab_icon_talent");
		}

		public void MathAddAttributeData()
		{
			if (this.m_LegacyInfo == null)
			{
				return;
			}
			this.m_addAttributeData.Clear();
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<MergeAttributeData> list3 = new List<MergeAttributeData>();
			foreach (TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo in this.m_LegacyInfo.CareerDic.Values)
			{
				bool flag = talentLegacyCareerInfo.CareerId == this.m_LegacyInfo.SelectCareerId;
				if (flag)
				{
					list.AddRange(talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList);
				}
				foreach (TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo in talentLegacyCareerInfo.SkillDic.Values)
				{
					if (talentLegacySkillInfo.Level > 0)
					{
						TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(talentLegacySkillInfo.TalentLegacyNodeId);
						TalentLegacy_talentLegacyEffect talentLegacy_talentLegacyEffect = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyEffect(talentLegacySkillInfo.TalentLegacyNodeId * 100 + talentLegacySkillInfo.Level);
						if (talentLegacy_talentLegacyEffect != null)
						{
							if (flag && talentLegacy_talentLegacyNode != null && talentLegacy_talentLegacyNode.type == 4 && talentLegacy_talentLegacyEffect.skills.Length != 0)
							{
								list2.AddRange(talentLegacy_talentLegacyEffect.skills);
							}
							List<MergeAttributeData> mergeAttributeData = talentLegacy_talentLegacyEffect.attributes.GetMergeAttributeData();
							list3.AddRange(mergeAttributeData);
						}
					}
				}
			}
			this.m_addAttributeData.m_attributeDatas = list3;
			this.m_addAttributeData.m_skillIDs.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				if (num > 0)
				{
					TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode2 = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
					if (talentLegacy_talentLegacyNode2 != null)
					{
						TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo2 = this.OnGetTalentLegacySkillInfo(talentLegacy_talentLegacyNode2.career, num);
						if (talentLegacySkillInfo2 != null)
						{
							int level = talentLegacySkillInfo2.Level;
							TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.GetTalentLegacySkillCfgByLevel(num, level);
							if (talentLegacySkillCfgByLevel != null)
							{
								int[] skills = talentLegacySkillCfgByLevel.skills;
								this.m_addAttributeData.m_skillIDs.AddRange(skills.ToList<int>());
							}
						}
					}
				}
			}
			this.m_addAttributeData.m_skillIDs.AddRange(list2);
			this.m_addAttributeData.m_attributeDatas.Merge();
		}

		public void OpenStudyFinishPanel(int careerId, int nodeId)
		{
			if (GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(nodeId) == null)
			{
				return;
			}
			TalentLegacyStudyFinishViewModule.OpenData openData = default(TalentLegacyStudyFinishViewModule.OpenData);
			openData.CareerId = careerId;
			openData.TalentLegacyNodeId = nodeId;
			GameApp.View.OpenView(ViewName.TalentLegacyStudyFinishViewModule, openData, 1, null, null);
		}

		public void Reset()
		{
			this.m_RankInfo = null;
			this.m_addAttributeData.Clear();
		}

		public bool IsAni;

		public long RankSaveTimeStampDelay = 30L;

		public long RankSaveTimeStamp;

		private TalentLegacyDataModule.TalentLegacyRankInfo m_RankInfo;

		private TalentLegacyDataModule.TalentLegacyInfo m_LegacyInfo;

		public AddAttributeData m_addAttributeData = new AddAttributeData();

		private Dictionary<int, Dictionary<int, List<TalentLegacy_talentLegacyNode>>> m_treeDic = new Dictionary<int, Dictionary<int, List<TalentLegacy_talentLegacyNode>>>();

		public enum ELegacySkillEquipState
		{
			UnActive,
			UnEquip,
			Equip,
			UnDown
		}

		public class TalentLegacyRankInfo
		{
			public TalentLegacyRankInfo(RepeatedField<UserRankInfoSimpleDto> infoDtos)
			{
				this.SetData(infoDtos);
			}

			public void SetData(RepeatedField<UserRankInfoSimpleDto> infoDtos)
			{
				this.UserInfoList.Clear();
				foreach (UserRankInfoSimpleDto userRankInfoSimpleDto in infoDtos)
				{
					this.UserInfoList.Add(userRankInfoSimpleDto);
				}
			}

			public List<UserRankInfoSimpleDto> UserInfoList = new List<UserRankInfoSimpleDto>();
		}

		public class TalentLegacyInfo
		{
			public TalentLegacyInfo(TalentLegacyInfoResponse dto)
			{
				this.SetData(dto);
			}

			public void SetData(TalentLegacyInfoResponse dto)
			{
				this.AssemblySlotCount = dto.TalentLegacyInfo.AssemblySlotCount;
				this.SelectCareerId = dto.TalentLegacyInfo.SelectCareer;
				this.CareerDic.Clear();
				for (int i = 0; i < dto.TalentLegacyInfo.CareerList.Count; i++)
				{
					this.CareerDic.Add(dto.TalentLegacyInfo.CareerList[i].CareerId, new TalentLegacyDataModule.TalentLegacyCareerInfo(dto.TalentLegacyInfo.CareerList[i]));
				}
			}

			public void OnSetDataCareerInfoData(TalentLegacyCareerDto dto)
			{
				if (dto == null)
				{
					return;
				}
				if (this.CareerDic.ContainsKey(dto.CareerId))
				{
					this.CareerDic[dto.CareerId].SetData(dto);
				}
			}

			public Dictionary<int, TalentLegacyDataModule.TalentLegacyCareerInfo> CareerDic = new Dictionary<int, TalentLegacyDataModule.TalentLegacyCareerInfo>();

			public int AssemblySlotCount;

			public int SelectCareerId;
		}

		public class TalentLegacyCareerInfo
		{
			public TalentLegacyCareerInfo(TalentLegacyCareerDto dto)
			{
				this.SetData(dto);
			}

			public void SetData(TalentLegacyCareerDto dto)
			{
				this.AssemblyTalentLegacySkillIdList.Clear();
				this.AssemblyTalentLegacySkillIdList.AddRange(dto.AssemblyTalentLegacyId);
				this.CareerId = dto.CareerId;
				foreach (KeyValuePair<int, TalentLegacyDataModule.TalentLegacySkillInfo> keyValuePair in this.SkillDic)
				{
					keyValuePair.Value.OnStopCountDown();
				}
				this.SkillDic.Clear();
				for (int i = 0; i < dto.TalentLegacys.Count; i++)
				{
					this.SkillDic.Add(dto.TalentLegacys[i].TalentLegacyId, new TalentLegacyDataModule.TalentLegacySkillInfo(dto.TalentLegacys[i], dto.CareerId));
				}
				this.SkillList.Clear();
				foreach (KeyValuePair<int, TalentLegacyDataModule.TalentLegacySkillInfo> keyValuePair2 in this.SkillDic)
				{
					this.SkillList.Add(keyValuePair2.Value);
				}
			}

			public Dictionary<int, TalentLegacyDataModule.TalentLegacySkillInfo> SkillDic = new Dictionary<int, TalentLegacyDataModule.TalentLegacySkillInfo>();

			public List<TalentLegacyDataModule.TalentLegacySkillInfo> SkillList = new List<TalentLegacyDataModule.TalentLegacySkillInfo>();

			public List<int> AssemblyTalentLegacySkillIdList = new List<int>();

			public int CareerId;
		}

		public class TalentLegacySkillInfo
		{
			public TalentLegacySkillInfo(TalentLegacyDto dto, int careerId)
			{
				this.TalentLegacyNodeId = dto.TalentLegacyId;
				this.Level = dto.Level;
				this.LevelUpTime = dto.LevelUpTime;
				this.m_careerId = careerId;
				this.OnStopCountDown();
				if (this.LevelUpTime > 0L)
				{
					PlayerPrefsKeys.SetTalentLegacyNodeFinish(HLog.StringBuilder(this.m_careerId.ToString(), "_", this.TalentLegacyNodeId.ToString()));
					DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.OnStartCountDown));
				}
			}

			private void OnStartCountDown()
			{
				if (this.LevelUpTime - DxxTools.Time.ServerTimestamp <= 0L)
				{
					if (GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).IsContainUnlockCareer(this.m_careerId, this.TalentLegacyNodeId))
					{
						NetworkUtils.TalentLegacy.DoTalentLegacyInfoRequest(null, false);
					}
					this.Level++;
					this.LevelUpTime = 0L;
					GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
					GameApp.Event.DispatchNow(this, 145, null);
					GameApp.View.CloseView(ViewName.CommonUseTipViewModule, null);
					EventArgsNodeTimeEnd eventArgsNodeTimeEnd = new EventArgsNodeTimeEnd(this.m_careerId, this.TalentLegacyNodeId);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_TalentLegacyNodeTimeEnd, eventArgsNodeTimeEnd);
					DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnStartCountDown));
				}
			}

			public void OnStopCountDown()
			{
				DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnStartCountDown));
			}

			public int TalentLegacyNodeId;

			public int Level;

			public long LevelUpTime;

			private int m_careerId;
		}
	}
}
