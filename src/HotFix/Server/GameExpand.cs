using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Battle;
using Proto.Common;

namespace Server
{
	public static class GameExpand
	{
		public static List<int> GetListInt(this string info, char separator = '|')
		{
			List<int> list = new List<int>();
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			foreach (string text in info.Split(separator, StringSplitOptions.None))
			{
				int num;
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		public static string Debug<T>(this List<T> list)
		{
			string text = typeof(T).ToString() + "： ";
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				string text2 = text;
				T t = list[i];
				text = text2 + ((t != null) ? t.ToString() : null);
				if (i < count - 1)
				{
					text += ",";
				}
				i++;
			}
			return text;
		}

		public static long GetValue(this FP fp)
		{
			return fp.AsLong();
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this string[] attrs)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < attrs.Length; i++)
			{
				list.AddRange(attrs[i].GetMergeAttributeData());
			}
			return list;
		}

		public static MergeAttributeData GetMergeAttribute(this string attr)
		{
			if (string.IsNullOrEmpty(attr))
			{
				return null;
			}
			return new MergeAttributeData(attr.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", ""), null, null);
		}

		public static void GetAttributeKV(this string attrs, out string key, out FP value)
		{
			key = "";
			value = 0;
			if (string.IsNullOrEmpty(attrs))
			{
				return;
			}
			string[] array = attrs.Split('=', StringSplitOptions.None);
			if (array.Length != 2)
			{
				return;
			}
			key = array[0];
			value = double.Parse(array[1]);
		}

		public static Dictionary<string, FP> GetAttributeDict(this string attrs)
		{
			Dictionary<string, FP> dictionary = new Dictionary<string, FP>();
			if (string.IsNullOrEmpty(attrs))
			{
				return dictionary;
			}
			attrs = attrs.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
			foreach (string text in attrs.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array2 = text.Split('=', StringSplitOptions.None);
					if (array2.Length == 2)
					{
						string text2 = array2[0];
						FP fp = double.Parse(array2[1]);
						if (dictionary.ContainsKey(text2))
						{
							Dictionary<string, FP> dictionary2 = dictionary;
							string text3 = text2;
							dictionary2[text3] += fp;
						}
						else
						{
							dictionary[text2] = fp;
						}
					}
				}
			}
			return dictionary;
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this string attrs)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			if (string.IsNullOrEmpty(attrs))
			{
				return list;
			}
			attrs = attrs.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
			foreach (string text in attrs.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static List<MergeAttributeData> GetAddMergeAttributeData(this string info, List<string> argsName, List<string> arges, char separator = '|')
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>(24);
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			if (argsName.Count != arges.Count)
			{
				HLog.LogError(string.Concat(new string[]
				{
					"GetAddMergeAttributeData [",
					argsName.Debug<string>(),
					"] != [",
					arges.Debug<string>(),
					"]"
				}));
				return list;
			}
			info = info.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
			foreach (string text in info.Split(separator, StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, argsName, arges);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static Dictionary<HurtType, HurtData> ToHurtDatas(this List<MergeAttributeData> merageDatas, LocalModelManager table)
		{
			Dictionary<HurtType, HurtData> dictionary = new Dictionary<HurtType, HurtData>(merageDatas.Count);
			for (int i = 0; i < merageDatas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = merageDatas[i];
				if (mergeAttributeData != null)
				{
					HurtType hurtType = GameExpand.AttributeToHurtType(mergeAttributeData.Header);
					HurtData hurtData;
					dictionary.TryGetValue(hurtType, out hurtData);
					if (hurtData == null)
					{
						hurtData = new HurtData(hurtType, 0);
					}
					hurtData.m_attack += mergeAttributeData.Value;
					dictionary[hurtType] = hurtData;
				}
			}
			return dictionary;
		}

		public static HurtType AttributeToHurtType(string hurtType)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(hurtType);
			if (num <= 1830469340U)
			{
				if (num <= 579999733U)
				{
					if (num != 423407916U)
					{
						if (num == 579999733U)
						{
							if (hurtType == "CritAttack")
							{
								return HurtType.CritAttack;
							}
						}
					}
					else if (hurtType == "PoisonBuffAttack")
					{
						return HurtType.PoisonBuffAttack;
					}
				}
				else if (num != 1458760285U)
				{
					if (num != 1543180668U)
					{
						if (num == 1830469340U)
						{
							if (hurtType == "IceAttack")
							{
								return HurtType.IceAttack;
							}
						}
					}
					else if (hurtType == "PhysicalAttack")
					{
						return HurtType.PhysicalAttack;
					}
				}
				else if (hurtType == "TrueAttack")
				{
					return HurtType.TrueAttack;
				}
			}
			else if (num <= 2585665377U)
			{
				if (num != 2186524777U)
				{
					if (num != 2343121693U)
					{
						if (num == 2585665377U)
						{
							if (hurtType == "FireAttack")
							{
								return HurtType.FireAttack;
							}
						}
					}
					else if (hurtType == "Attack")
					{
						return HurtType.Attack;
					}
				}
				else if (hurtType == "ThunderAttack")
				{
					return HurtType.ThunderAttack;
				}
			}
			else if (num != 3685988384U)
			{
				if (num != 3814133339U)
				{
					if (num == 4121488228U)
					{
						if (hurtType == "FireBuffAttack")
						{
							return HurtType.FireBuffAttack;
						}
					}
				}
				else if (hurtType == "RecoverHP")
				{
					return HurtType.RecoverHP;
				}
			}
			else if (hurtType == "ShieldCounterAttack")
			{
				return HurtType.ShieldCounterAttack;
			}
			HLog.LogError("GameExpand.AttributeToHurtType ..HurtType is Error. 伤害类型：" + hurtType + " ");
			return HurtType.Attack;
		}

		public static void GetChapterMainPlayer(this RChapterCombatReq request, LocalModelManager table, out CardData friendMainCardData, out string log)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("userID={0}", request.UserInfo.UserId));
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = (long)cardData.m_rowID == request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[ChapterMainPlayerAttributes]");
						List<MergeAttributeData> list2 = request.UserInfo.Attributes.GetMergeAttributeData().Merge();
						cardData.AddAttributes(list2);
						cardData.AddSkill(request.UserInfo.SkillIds.ToList<int>());
						cardData.ConvertBaseData();
						stringBuilder.Append(string.Format("MainEventAttrCount = {0} ", list2.Count));
						for (int j = 0; j < list2.Count; j++)
						{
							MergeAttributeData mergeAttributeData = list2[j];
							stringBuilder.Append(string.Format("{0}={1} ", mergeAttributeData.Header, mergeAttributeData.Value));
						}
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						cardData.m_curHp = new FP(request.CurHp);
						friendMainCardData = cardData;
						stringBuilder.Append("[MainPlayerData]:" + cardData.Log());
					}
				}
			}
			log = stringBuilder.ToString();
		}

		public static void GetTowerMainPlayer(this RTowerCombatReq request, LocalModelManager table, out CardData friendMainCardData)
		{
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = cardData.m_rowID == (int)request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[TowerMainPlayerAttributes]");
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						cardData.m_curHp = cardData.m_memberAttributeData.GetHpMax();
						friendMainCardData = cardData;
					}
				}
			}
		}

		public static void GetDungeonMainPlayer(this RDungeonCombatReq request, LocalModelManager table, out CardData friendMainCardData)
		{
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = cardData.m_rowID == (int)request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[DungeonMainPlayerAttributes]");
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						cardData.m_curHp = cardData.m_memberAttributeData.GetHpMax();
						friendMainCardData = cardData;
					}
				}
			}
		}

		public static CardData GetMainPlayerPower(this BattleUserDto userDto, LocalModelManager table)
		{
			for (int i = 0; i < userDto.Heros.Count; i++)
			{
				HeroDto heroDto = userDto.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData
					{
						m_rowID = (int)heroDto.RowId,
						m_memberID = (int)heroDto.HeroId,
						m_instanceID = 100 + i,
						m_camp = MemberCamp.Friendly,
						m_posIndex = (MemberPos)i,
						m_isMainMember = true
					};
					if (cardData.m_isMainMember = cardData.m_rowID == (int)userDto.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, userDto);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.ConvertBaseData();
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.SetMemberRace(MemberRace.Hero);
						return cardData;
					}
				}
			}
			return null;
		}

		public static void GetGuildBossMainPlayer(this RGuildBossCombatReq request, LocalModelManager table, out CardData friendMainCardData, out string log)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("userID={0}", request.UserInfo.UserId));
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = cardData.m_rowID == (int)request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[TowerMainPlayerAttributes]");
						List<MergeAttributeData> list2 = new List<MergeAttributeData>();
						List<int> list3 = new List<int>();
						foreach (int num in request.UserInfo.SkillIds)
						{
							GameSkillBuild_skillBuild gameSkillBuild_skillBuild = table.GetGameSkillBuild_skillBuild(num);
							if (gameSkillBuild_skillBuild != null)
							{
								list2.AddRange(gameSkillBuild_skillBuild.attributes.GetMergeAttributeData());
								list3.Add(gameSkillBuild_skillBuild.skillId);
							}
						}
						List<MergeAttributeData> list4 = list2.Merge();
						cardData.AddAttributes(list4);
						cardData.AddSkill(list3);
						cardData.ConvertBaseData();
						stringBuilder.Append(string.Format("MainEventSkillCount = {0} ", list3.Count));
						GuildBOSS_guildBossStep guildBOSS_guildBossStep = table.GetGuildBOSS_guildBossStep(request.ConfigId);
						if (guildBOSS_guildBossStep != null)
						{
							List<MergeAttributeData> mergeAttributeData = guildBOSS_guildBossStep.PlayerAttributes.GetMergeAttributeData();
							List<MergeAttributeData> list5 = mergeAttributeData.Merge();
							if (mergeAttributeData.Count > 0)
							{
								cardData.AddAttributes(list5);
								cardData.ConvertBaseData();
							}
						}
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						cardData.m_curHp = cardData.m_memberAttributeData.GetHpMax();
						friendMainCardData = cardData;
						stringBuilder.Append("[MainPlayerData]:" + cardData.Log());
					}
				}
			}
			log = stringBuilder.ToString();
		}

		public static void GetRogueDungeonMainPlayer(this RHellTowerCombatReq request, LocalModelManager table, out CardData friendMainCardData, out string log)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("userID={0}", request.UserInfo.UserId));
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = (long)cardData.m_rowID == request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[RogueDungeonMainPlayerAttributes]");
						List<MergeAttributeData> mergeAttributeData = request.UserInfo.Attributes.GetMergeAttributeData();
						List<MergeAttributeData> list2 = mergeAttributeData.Merge();
						cardData.AddAttributes(list2);
						List<MergeAttributeData> list3 = new List<MergeAttributeData>();
						List<int> list4 = new List<int>();
						foreach (int num in request.UserInfo.SkillIds)
						{
							GameSkillBuild_skillBuild gameSkillBuild_skillBuild = table.GetGameSkillBuild_skillBuild(num);
							if (gameSkillBuild_skillBuild != null)
							{
								list3.AddRange(gameSkillBuild_skillBuild.attributes.GetMergeAttributeData());
								list4.Add(gameSkillBuild_skillBuild.skillId);
							}
						}
						List<MergeAttributeData> list5 = list3.Merge();
						cardData.AddAttributes(list5);
						cardData.AddSkill(list4);
						cardData.ConvertBaseData();
						stringBuilder.Append(string.Format("MainEventAttrCount = {0} ", mergeAttributeData.Count));
						stringBuilder.Append(string.Format("MainEventSkillCount = {0} ", list4.Count));
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						cardData.m_curHp = new FP(request.CurHp);
						friendMainCardData = cardData;
						stringBuilder.Append("[MainPlayerData]:" + cardData.Log());
					}
				}
			}
			log = stringBuilder.ToString();
		}

		public static void GetWorldBossMainPlayer(this RWorldBossCombatReq request, LocalModelManager table, out CardData friendMainCardData, out string log)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("userID={0}", request.UserInfo.UserId));
			friendMainCardData = null;
			for (int i = 0; i < request.UserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = request.UserInfo.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData((int)heroDto.RowId, 100 + i, (int)heroDto.HeroId, MemberCamp.Friendly, false);
					if (cardData.m_isMainMember = (long)cardData.m_rowID == request.UserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, request.UserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.UpdateSkills(addAttributeData.m_skillIDs);
						cardData.ConvertBaseData();
						BattleLogHelper.LogAttributeDataList(list, "[WorldBossMainPlayerAttributes]");
						List<MergeAttributeData> list2 = new List<MergeAttributeData>();
						List<int> list3 = new List<int>();
						foreach (int num in request.UserInfo.SkillIds)
						{
							GameSkillBuild_skillBuild gameSkillBuild_skillBuild = table.GetGameSkillBuild_skillBuild(num);
							if (gameSkillBuild_skillBuild != null)
							{
								list2.AddRange(gameSkillBuild_skillBuild.attributes.GetMergeAttributeData());
								list3.Add(gameSkillBuild_skillBuild.skillId);
							}
						}
						List<MergeAttributeData> list4 = list2.Merge();
						cardData.AddAttributes(list4);
						cardData.AddSkill(list3);
						cardData.ConvertBaseData();
						stringBuilder.Append(string.Format("MainEventSkillCount = {0} ", list3.Count));
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						friendMainCardData = cardData;
						stringBuilder.Append("[MainPlayerData]:" + cardData.Log());
					}
				}
			}
			log = stringBuilder.ToString();
		}

		public static List<MergeAttributeData> GetAddMergeAttributeData(this GameBuff_buff gameBuffBuff, string[] argsName, string[] arges)
		{
			return GameExpand.GetMergeAttributeDataImpl(gameBuffBuff.addAttributes, argsName, arges);
		}

		public static List<MergeAttributeData> GetAddMergeAttributeOnceData(this GameBuff_buff gameBuffBuff, string[] argsName, string[] arges)
		{
			return GameExpand.GetMergeAttributeDataImpl(gameBuffBuff.addAttributesOnce, argsName, arges);
		}

		private static List<MergeAttributeData> GetMergeAttributeDataImpl(string attributes, string[] argsName, string[] arges)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>(24);
			if (string.IsNullOrEmpty(attributes))
			{
				return list;
			}
			string text = attributes.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
			foreach (string text2 in text.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text2, argsName, arges);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static List<MergeAttributeData> GetMergeTriggerAttributeData(this GameBuff_buff gameBuffBuff, List<string> argsName, List<string> args)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>(24);
			if (argsName == null || args == null)
			{
				return list;
			}
			if (argsName.Count != args.Count)
			{
				HLog.LogError(string.Format("GameExpand GetMergeTriggerAttributeData Count:{0} != {1}", argsName.Count, args.Count));
				return list;
			}
			string triggerAttributes = gameBuffBuff.triggerAttributes;
			string text = triggerAttributes;
			if (!string.IsNullOrEmpty(triggerAttributes))
			{
				int num = triggerAttributes.IndexOf('=');
				text = triggerAttributes.Insert(num + 1, "(") + ")*Layer";
			}
			if (string.IsNullOrEmpty(text))
			{
				return list;
			}
			text = text.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
			foreach (string text2 in text.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text2, argsName, args);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static List<CardData> ToCardDatas(this BattleUserDto battleUserInfo, MemberCamp memberCamp, LocalModelManager table)
		{
			List<CardData> list = new List<CardData>();
			for (int i = 0; i < battleUserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = battleUserInfo.Heros[i];
				if (heroDto != null)
				{
					int num = ((memberCamp != MemberCamp.Friendly) ? (200 + i) : (100 + i));
					CardData cardData = new CardData((int)heroDto.RowId, num, (int)heroDto.HeroId, memberCamp, (MemberPos)i, false);
					if (cardData.m_isMainMember = cardData.m_rowID == (int)battleUserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, battleUserInfo);
						List<MergeAttributeData> list2 = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list2);
						cardData.ConvertBaseData();
						cardData.AddSkill(addAttributeData.m_skillIDs);
						cardData.SetMemberRace(MemberRace.Hero);
						list.Add(cardData);
					}
				}
			}
			return list;
		}

		public static CardData GetMainCard(this List<CardData> cards)
		{
			for (int i = 0; i < cards.Count; i++)
			{
				if (cards[i].m_isMainMember)
				{
					return cards[i];
				}
			}
			return null;
		}

		public static CardData GetMainCardData(this BattleUserDto battleUserInfo, MemberCamp memberCamp, LocalModelManager table)
		{
			return battleUserInfo.GetMainCardDataWithAddAttributeData(memberCamp, table).Item1;
		}

		public static ValueTuple<CardData, AddAttributeData> GetMainCardDataWithAddAttributeData(this BattleUserDto battleUserInfo, MemberCamp memberCamp, LocalModelManager table)
		{
			for (int i = 0; i < battleUserInfo.Heros.Count; i++)
			{
				HeroDto heroDto = battleUserInfo.Heros[i];
				if (heroDto != null)
				{
					int num = ((memberCamp != MemberCamp.Friendly) ? (200 + i) : (100 + i));
					CardData cardData = new CardData((int)heroDto.RowId, num, (int)heroDto.HeroId, memberCamp, (MemberPos)i, false);
					if (cardData.m_isMainMember = cardData.m_rowID == (int)battleUserInfo.ActorRowId)
					{
						AddAttributeData addAttributeData = new AddAttributeController(table).MathMainMember(cardData.m_memberID, battleUserInfo);
						List<MergeAttributeData> list = addAttributeData.m_attributeDatas.Merge();
						cardData.UpdateAttribute(list);
						cardData.ConvertBaseData();
						cardData.AddSkill(addAttributeData.m_skillIDs);
						cardData.SetMemberRace(MemberRace.Hero);
						cardData.m_posIndex = MemberPos.One;
						return new ValueTuple<CardData, AddAttributeData>(cardData, addAttributeData);
					}
				}
			}
			return new ValueTuple<CardData, AddAttributeData>(null, null);
		}

		public static EquipmentDto GetWeaponDto(this RepeatedField<EquipmentDto> equips, LocalModelManager localModelManager)
		{
			if (equips == null || equips.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < equips.Count; i++)
			{
				Equip_equip elementById = localModelManager.GetEquip_equipModelInstance().GetElementById((int)equips[i].EquipId);
				if (elementById != null && elementById.Type == 1)
				{
					return equips[i];
				}
			}
			return null;
		}

		public static List<SMemberBase> SortOrderBattle(this List<SMemberBase> members)
		{
			members.Sort(delegate(SMemberBase a, SMemberBase b)
			{
				int num = a.memberData.Camp.CompareTo(b.memberData.Camp);
				if (num.Equals(0))
				{
					num = a.memberData.PosIndex.CompareTo(b.memberData.PosIndex);
				}
				return num;
			});
			return members;
		}

		public static List<SSkillBase> SortOrderPlaying(this List<SSkillBase> skills)
		{
			return skills.OrderByDescending(delegate(SSkillBase s)
			{
				SSkillData skillData = s.skillData;
				if (skillData == null)
				{
					return null;
				}
				return new SkillFreedType?(skillData.m_freedType);
			}).ToList<SSkillBase>();
		}

		public static Dungeon_DungeonLevel GetDungeonLevel(this IList<Dungeon_DungeonLevel> allDungeonLevel, int dungeonID, int levelID)
		{
			for (int i = 0; i < allDungeonLevel.Count; i++)
			{
				Dungeon_DungeonLevel dungeon_DungeonLevel = allDungeonLevel[i];
				if (dungeonID == dungeon_DungeonLevel.dungeonID && levelID == dungeon_DungeonLevel.level)
				{
					return dungeon_DungeonLevel;
				}
			}
			return null;
		}

		public static int GetTotalExp(this TalentsInfo talentsInfo, LocalModelManager localModelManager)
		{
			int step = (int)talentsInfo.Step;
			int expProcess = (int)talentsInfo.ExpProcess;
			TalentNew_talentEvolution elementById = localModelManager.GetTalentNew_talentEvolutionModelInstance().GetElementById(step);
			if (elementById == null)
			{
				HLog.LogError(string.Format("talentStage:{0} not found in talentEvolutionTable", step));
				return 0;
			}
			return elementById.exp + expProcess;
		}
	}
}
