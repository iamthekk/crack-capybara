using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.User;
using Server;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public static class DxxTools
	{
		public static Vector3 GetDirection(float angle)
		{
			Vector3 vector = default(Vector3);
			vector.x = Utility.Math.Sin(angle);
			vector.z = Utility.Math.Cos(angle);
			return vector;
		}

		public static float GetAngle(Vector2 dir)
		{
			return DxxTools.GetAngle(dir.x, dir.y);
		}

		public static float GetAngleYVector3(Vector3 dir)
		{
			return DxxTools.GetAngle(dir.x, dir.z);
		}

		public static float GetAngle(float x, float y)
		{
			return (90f - Mathf.Atan2(y, x) * 57.29578f + 360f) % 360f;
		}

		public static long GetTotalSecend(DateTime date)
		{
			return (long)(date - DxxTools.mDateBegin).TotalSeconds;
		}

		public static DateTime TickToTime(long tick)
		{
			return DxxTools.mDateBegin.AddSeconds((double)tick);
		}

		public static List<T> EnumToList<T>() where T : Enum
		{
			return new List<T>((T[])Enum.GetValues(typeof(T)));
		}

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

		public static List<string> GetListString(this string info, char separator = '|')
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			foreach (string text in info.Split(separator, StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list;
		}

		public static List<float> GetListFloat(this string info, char separator = '|')
		{
			List<float> list = new List<float>();
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			foreach (string text in info.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "")
				.Split(separator, StringSplitOptions.None))
			{
				float num;
				if (!string.IsNullOrEmpty(text) && float.TryParse(text, out num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		public static List<long> GetListLong(this string info, char separator = '|')
		{
			List<long> list = new List<long>();
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			foreach (string text in info.Split(separator, StringSplitOptions.None))
			{
				long num;
				if (!string.IsNullOrEmpty(text) && long.TryParse(text, out num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		public static Dictionary<string, string> ParseToDic(this string arg)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(arg))
			{
				return dictionary;
			}
			foreach (string text in arg.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "")
				.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array2 = text.Split('=', StringSplitOptions.None);
					if (array2.Length != 0)
					{
						string text2 = array2[0];
						string text3 = string.Empty;
						if (!string.IsNullOrEmpty(text2))
						{
							if (array2.Length > 1)
							{
								text3 = array2[1];
							}
							dictionary[text2] = text3;
						}
					}
				}
			}
			return dictionary;
		}

		public static string ReplaceEmpty(this string arg)
		{
			return arg.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "");
		}

		public static string GetString(params object[] args)
		{
			DxxTools.mStringBudier.Clear();
			int i = 0;
			int num = args.Length;
			while (i < num)
			{
				DxxTools.mStringBudier.Append(args[i]);
				i++;
			}
			return DxxTools.mStringBudier.ToString();
		}

		public static string FormatNumber(long num)
		{
			if (num < 10000L)
			{
				return num.ToString();
			}
			if (num >= 1000000000L)
			{
				double num2 = (double)(num / 10000000L) / 100.0;
				int num3 = (int)num2;
				int num4 = (int)Math.Log10((double)num3) + 1;
				if (num4 < 5)
				{
					int num5 = Mathf.Clamp(5 - num4, 0, 2);
					string text = "0." + ((num5 == 1) ? "#" : "##");
					num2.ToString(text) + "B";
					return num2.ToString(text) + "B";
				}
				return num3.ToString() + "B";
			}
			else if (num >= 1000000L)
			{
				double num6 = (double)(num / 10000L) / 100.0;
				int num7 = (int)num6;
				int num8 = (int)Math.Log10((double)num7) + 1;
				if (num8 < 5)
				{
					int num9 = Mathf.Clamp(5 - num8, 0, 2);
					string text2 = "0." + ((num9 == 1) ? "#" : "##");
					return num6.ToString(text2) + "M";
				}
				return num7.ToString() + "M";
			}
			else
			{
				if (num < 10000L)
				{
					return num.ToString();
				}
				double num10 = (double)(num / 100L) / 10.0;
				int num11 = (int)num10;
				if ((int)Math.Log10((double)num11) + 1 < 5)
				{
					string text3 = "0.#";
					return num10.ToString(text3) + "K";
				}
				return num11.ToString() + "K";
			}
		}

		public static string FormatTime(long num)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)num);
			if (timeSpan.Days > 0)
			{
				return string.Format("{0}:{1:D2}", timeSpan.Days, timeSpan.Hours);
			}
			if (timeSpan.Hours > 0)
			{
				return string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes);
			}
			if (timeSpan.Minutes > 0)
			{
				return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
			}
			return string.Format("00:{0:D2}", timeSpan.Seconds);
		}

		public static string FormatFullTime(long num)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)num);
			return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}

		public static string FormatFullTimeWithDay(long num)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)num);
			if (timeSpan.Days > 0)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("15033", new object[]
				{
					timeSpan.Days,
					string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds)
				});
			}
			return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}

		public static string DataTimeToLocalMothDay(DateTime utcDateTime, string format = "MM/dd")
		{
			return utcDateTime.ToLocalTime().ToString(format);
		}

		public static long DateTimeToTimestamp(string dateTimeString)
		{
			return DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToUnixTimeSeconds();
		}

		public static string SecondsToTime(long totalSeconds)
		{
			long num = totalSeconds / 3600L;
			if (num > 0L)
			{
				long num2 = totalSeconds % 3600L / 60L;
				long num3 = totalSeconds % 60L;
				return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, num3);
			}
			long num4 = totalSeconds / 60L;
			long num5 = totalSeconds % 60L;
			return string.Format("{0:D2}:{1:D2}", num4, num5);
		}

		public static string GetDefaultNick(long userid)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("1000", new object[] { userid });
		}

		public static List<string> GetParameterForSocialityReport(int id, List<string> parameter)
		{
			List<string> list = new List<string>();
			switch (id)
			{
			case 1:
			case 2:
			case 3:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 13:
			case 14:
				list.Add(string.IsNullOrEmpty(parameter[1]) ? DxxTools.GetDefaultNick((long)int.Parse(parameter[0])) : parameter[1]);
				break;
			case 4:
			case 5:
			case 11:
			case 12:
				list.Add(string.IsNullOrEmpty(parameter[1]) ? DxxTools.GetDefaultNick((long)int.Parse(parameter[0])) : parameter[1]);
				list.Add(string.IsNullOrEmpty(parameter[3]) ? DxxTools.GetDefaultNick((long)int.Parse(parameter[2])) : parameter[3]);
				break;
			case 15:
				list.Add(string.IsNullOrEmpty(parameter[1]) ? DxxTools.GetDefaultNick((long)int.Parse(parameter[0])) : parameter[1]);
				list.Add(parameter[2]);
				break;
			}
			return list;
		}

		public static string GetChapterLevel(int chapterID, int wavaIndex)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Chapter", new object[] { chapterID });
		}

		public static string GetPVELevelShortName(int id)
		{
			return GameApp.Data.GetDataModule(DataName.MainDataModule).GetPVELevelShortName(id);
		}

		public static string GetPVELevelLongName(int id)
		{
			return GameApp.Data.GetDataModule(DataName.MainDataModule).GetPVELevelLongName(id);
		}

		public static string GetPercentageString(int number, string format = "0.00")
		{
			return ((double)number / 10000.0 * 100.0).ToString(format) + "%";
		}

		private static DateTime mDateBegin = new DateTime(1970, 1, 1, 0, 0, 0);

		private static StringBuilder mStringBudier = new StringBuilder();

		private const int MaxDigitCount = 5;

		public static class Animator
		{
			public static float GetAnimationLength(global::UnityEngine.Animator animator, string animationName)
			{
				AnimationClip animationClip = animator.runtimeAnimatorController.animationClips.ToList<AnimationClip>().FirstOrDefault((AnimationClip ani) => ani.name == animationName);
				if (animationClip == null)
				{
					return 0f;
				}
				return animationClip.length;
			}
		}

		public class Char
		{
			public static bool CheckEmoji(char addedChar)
			{
				if (DxxTools.Char.m_emojiPatterns.Count > 0)
				{
					string text = string.Format("{0}", addedChar);
					bool flag = false;
					for (int i = 0; i < DxxTools.Char.m_emojiPatterns.Count; i++)
					{
						flag = Regex.IsMatch(text, DxxTools.Char.m_emojiPatterns[i]);
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						return true;
					}
				}
				return false;
			}

			public static bool CheckLength(string text, char addedChar, int length)
			{
				string text2 = text + addedChar.ToString();
				int num = 0;
				for (int i = 0; i < text2.Length; i++)
				{
					int num2 = Encoding.UTF8.GetBytes(text2[i].ToString()).Length;
					num += ((num2 == 1) ? 2 : num2);
				}
				return num <= length;
			}

			public static bool CheckAlphanumeric(char addedChar)
			{
				if (DxxTools.Char.m_alphanumericPatterns.Count > 0)
				{
					string text = string.Format("{0}", addedChar);
					bool flag = false;
					for (int i = 0; i < DxxTools.Char.m_alphanumericPatterns.Count; i++)
					{
						flag = Regex.IsMatch(text, DxxTools.Char.m_alphanumericPatterns[i]);
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						return true;
					}
				}
				return false;
			}

			public static int GetLength(string text)
			{
				return Encoding.UTF8.GetBytes(text).Length;
			}

			public static int GetNameLength(string text)
			{
				int num = 0;
				for (int i = 0; i < text.Length; i++)
				{
					int num2 = Encoding.UTF8.GetBytes(text[i].ToString()).Length;
					num += ((num2 == 1) ? 2 : num2);
				}
				return num;
			}

			private static List<string> m_emojiPatterns = new List<string> { "\\p{Cs}", "\\p{Co}", "\\p{Cn}", "[\\u2702-\\u27B0]" };

			private static List<string> m_alphanumericPatterns = new List<string> { "[^a-zA-Z0-9\\s]" };

			public const char CharEmpty = '\0';
		}

		public static class Game
		{
			public static string GetValueInfoByAttributeName(MemberAttributeData memberAttributeData, string attributeName)
			{
				string text = string.Empty;
				if (string.IsNullOrEmpty(attributeName))
				{
					return text;
				}
				uint num = <PrivateImplementationDetails>.ComputeStringHash(attributeName);
				if (num <= 1069607449U)
				{
					if (num <= 379785653U)
					{
						if (num != 324772136U)
						{
							if (num == 379785653U)
							{
								if (attributeName == "CritValue%")
								{
									text = string.Format("{0}%", memberAttributeData.CritValue.AsLong());
								}
							}
						}
						else if (attributeName == "Attack%")
						{
							text = string.Format("{0}%", memberAttributeData.AttackPercent.AsLong());
						}
					}
					else if (num != 484614851U)
					{
						if (num == 1069607449U)
						{
							if (attributeName == "HPMax")
							{
								text = DxxTools.FormatNumber(memberAttributeData.HPMax.AsLong());
							}
						}
					}
					else if (attributeName == "Defence")
					{
						text = DxxTools.FormatNumber(memberAttributeData.Defence.AsLong());
					}
				}
				else if (num <= 2343121693U)
				{
					if (num != 1590063122U)
					{
						if (num == 2343121693U)
						{
							if (attributeName == "Attack")
							{
								text = DxxTools.FormatNumber(memberAttributeData.Attack.AsLong());
							}
						}
					}
					else if (attributeName == "Defence%")
					{
						text = string.Format("{0}%", memberAttributeData.DefencePercent.AsLong());
					}
				}
				else if (num != 2561719412U)
				{
					if (num == 3692514014U)
					{
						if (attributeName == "CritRate%")
						{
							text = string.Format("{0}%", memberAttributeData.CritRate.AsLong());
						}
					}
				}
				else if (attributeName == "HPMax%")
				{
					text = string.Format("{0}%", memberAttributeData.HPMaxPercent.AsLong());
				}
				return text;
			}

			public static void VersionMatch(PlayerInfoDto playerInfo)
			{
				if (playerInfo.Avatar != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar((int)playerInfo.Avatar) == null))
				{
					playerInfo.Avatar = (uint)GameConfig.NotExist_Avatar_Id;
				}
				if (playerInfo.AvatarFrame != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar((int)playerInfo.AvatarFrame) == null))
				{
					playerInfo.AvatarFrame = (uint)GameConfig.NotExist_AvatarFrame_Id;
				}
				if (playerInfo.TitleId != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar((int)playerInfo.TitleId) == null))
				{
					playerInfo.TitleId = (uint)GameConfig.NotExist_Title_Id;
				}
				if (playerInfo.SkinHeaddressId != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetArtMember_clothes((int)playerInfo.SkinHeaddressId) == null))
				{
					playerInfo.SkinHeaddressId = 0U;
				}
				if (playerInfo.SkinBodyId != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetArtMember_clothes((int)playerInfo.SkinBodyId) == null))
				{
					playerInfo.SkinBodyId = 0U;
				}
				if (playerInfo.SkinAccessoryId != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetArtMember_clothes((int)playerInfo.SkinAccessoryId) == null))
				{
					playerInfo.SkinAccessoryId = 0U;
				}
			}

			public static bool TryVersionMatch(BaseRankData rankData)
			{
				bool flag = true;
				if (rankData != null)
				{
					if (GameConfig.NotExist_Test || (rankData.AvatarId != 0 && GameApp.Table.GetManager().GetAvatar_Avatar(rankData.AvatarId) == null))
					{
						flag = false;
						rankData.AvatarId = GameConfig.NotExist_Avatar_Id;
					}
					if (GameConfig.NotExist_Test || (rankData.AvatarFrameId != 0 && GameApp.Table.GetManager().GetAvatar_Avatar(rankData.AvatarFrameId) == null))
					{
						flag = false;
						rankData.AvatarFrameId = GameConfig.NotExist_AvatarFrame_Id;
					}
					if (GameConfig.NotExist_Test || (rankData.TitleId != 0 && GameApp.Table.GetManager().GetAvatar_Avatar(rankData.TitleId) == null))
					{
						flag = false;
						rankData.TitleId = GameConfig.NotExist_Title_Id;
					}
				}
				return flag;
			}

			public static bool TryVersionMatchAvatar(ref int avatarId, ref int avatarFrameId)
			{
				bool flag = true;
				if (avatarId != 0 && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar(avatarId) == null))
				{
					flag = false;
					avatarId = GameConfig.NotExist_Avatar_Id;
				}
				if (avatarFrameId != 0 && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar(avatarFrameId) == null))
				{
					flag = false;
					avatarFrameId = GameConfig.NotExist_AvatarFrame_Id;
				}
				return flag;
			}

			public static bool TryVersionMatchTitle(ref int cfgId)
			{
				bool flag = true;
				if (cfgId != 0 && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_Avatar(cfgId) == null))
				{
					flag = false;
					cfgId = GameConfig.NotExist_Title_Id;
				}
				return flag;
			}

			public static bool TryVersionMatch(PropData propData)
			{
				bool flag = true;
				if (propData != null && propData.id != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetItem_Item((int)propData.id) == null))
				{
					flag = false;
				}
				return flag;
			}

			public static bool TryVersionMatch(EquipData equipData)
			{
				bool flag = true;
				if (equipData != null && equipData.id != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetEquip_equip((int)equipData.id) == null))
				{
					flag = false;
					switch (equipData.equipType)
					{
					case EquipType.Weapon:
						equipData.id = (uint)GameConfig.NotExist_Equip_Weapon_Id;
						break;
					case EquipType.Clothes:
						equipData.id = (uint)GameConfig.NotExist_Equip_Clothes_Id;
						break;
					case EquipType.Ring:
						equipData.id = (uint)GameConfig.NotExist_Equip_Ring_Id;
						break;
					case EquipType.Accessory:
						equipData.id = (uint)GameConfig.NotExist_Equip_Accessory_Id;
						break;
					default:
						HLog.LogError("暂不支持的装备类型 : " + equipData.equipType.ToString());
						break;
					}
				}
				return flag;
			}

			public static bool TryVersionMatch(PetDto petDto)
			{
				bool flag = true;
				if (petDto != null && petDto.ConfigId != 0U && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetPet_pet((int)petDto.ConfigId) == null))
				{
					flag = false;
					petDto.ConfigId = (uint)GameConfig.NotExist_Pet_Id;
				}
				return flag;
			}

			public static bool TryVersionMatch(MountInfo mountInfo)
			{
				bool flag = true;
				if (mountInfo != null && mountInfo.ConfigId != 0U)
				{
					if (GameConfig.NotExist_Test)
					{
						flag = false;
					}
					else if (mountInfo.ConfigType == 1U)
					{
						if (GameApp.Table.GetManager().GetMount_mountStage((int)mountInfo.ConfigId) == null)
						{
							flag = false;
						}
					}
					else if (GameApp.Table.GetManager().GetMount_advanceMount((int)mountInfo.ConfigId) == null)
					{
						flag = false;
					}
				}
				return flag;
			}

			public static bool TryVersionMatch(ArtifactInfo artifactInfo)
			{
				bool flag = true;
				if (artifactInfo != null && artifactInfo.ConfigId != 0U)
				{
					if (GameConfig.NotExist_Test)
					{
						flag = false;
						artifactInfo.ConfigId = 0U;
					}
					else if (artifactInfo.ConfigType == 1U)
					{
						if (GameApp.Table.GetManager().GetArtifact_artifactStage((int)artifactInfo.ConfigId) == null)
						{
							flag = false;
							artifactInfo.ConfigId = 0U;
						}
					}
					else if (GameApp.Table.GetManager().GetArtifact_advanceArtifact((int)artifactInfo.ConfigId) == null)
					{
						flag = false;
						artifactInfo.ConfigId = 0U;
					}
				}
				return flag;
			}

			public static bool TryVersionMatchWeapon(ref int weaponId)
			{
				bool flag = true;
				if (weaponId != 0 && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetEquip_equip(weaponId) == null))
				{
					flag = false;
					weaponId = GameConfig.NotExist_Equip_Weapon_Id;
				}
				return flag;
			}

			public static bool TryVersionMatchSkin(ref int skinId, SkinType skinType)
			{
				bool flag = true;
				if (skinId != 0 && (GameConfig.NotExist_Test || GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(skinId) == null))
				{
					flag = false;
					switch (skinType)
					{
					case SkinType.Body:
						skinId = Singleton<GameConfig>.Instance.ClothesDefaultBodyId;
						break;
					case SkinType.Head:
						skinId = Singleton<GameConfig>.Instance.ClothesDefaultHeadId;
						break;
					case SkinType.Back:
						skinId = Singleton<GameConfig>.Instance.ClothesDefaultAccessoryId;
						break;
					default:
						HLog.LogError("暂不支持的皮肤类型 : " + skinType.ToString());
						break;
					}
				}
				return flag;
			}

			public static bool TryVersionMatchOperate(bool matched)
			{
				if (!matched)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("VersionMatchOperateTip"));
				}
				return matched;
			}

			public static void VersionMatchNotExist_SetImage_Item_Icon(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Item_Icon_AtlasId, GameConfig.NotExist_Item_Icon_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Item_Quality(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Item_Quality_AtlasId, GameConfig.NotExist_Item_Quality_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Pet_Icon(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Pet_Icon_AtlasId, GameConfig.NotExist_Pet_Icon_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Pet_Quality(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Pet_Quality_AtlasId, GameConfig.NotExist_Pet_Quality_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Mount_Icon(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Mount_Icon_AtlasId, GameConfig.NotExist_Mount_Icon_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Mount_Quality(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Mount_Quality_AtlasId, GameConfig.NotExist_Mount_Quality_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Artifact_Icon(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Artifact_Icon_AtlasId, GameConfig.NotExist_Artifact_Icon_Sprite);
			}

			public static void VersionMatchNotExist_SetImage_Artifact_Quality(CustomImage img)
			{
				if (img == null)
				{
					return;
				}
				img.SetImage(GameConfig.NotExist_Artifact_Quality_AtlasId, GameConfig.NotExist_Artifact_Quality_Sprite);
			}
		}

		public static class Jump
		{
			public static void JumpToMoreExtension(Action<GameObject> onLoadedCallback = null, Action<GameObject> onOpenedCallback = null, Action onClosedCallback = null)
			{
				MoreExtensionViewModule.OpenData openData = new MoreExtensionViewModule.OpenData();
				openData.onCloseCallback = onClosedCallback;
				GameApp.View.OpenView(ViewName.MoreExtensionViewModule, openData, 1, onLoadedCallback, onOpenedCallback);
			}

			public static void JumpToBag(Action<GameObject> onLoadedCallback = null, Action<GameObject> onOpenedCallback = null, Action onClosedCallback = null)
			{
				BagViewModule.OpenData openData = new BagViewModule.OpenData();
				openData.onCloseCallback = onClosedCallback;
				GameApp.View.OpenView(ViewName.BagViewModule, openData, 1, onLoadedCallback, onOpenedCallback);
			}

			public static void JumpToMail(Action<GameObject> onLoadedCallback = null, Action<GameObject> onOpenedCallback = null, Action onClosedCallback = null)
			{
				MailViewModule.OpenData openData = new MailViewModule.OpenData();
				openData.onCloseCallback = onClosedCallback;
				GameApp.View.OpenView(ViewName.MailViewModule, openData, 1, onLoadedCallback, onOpenedCallback);
			}

			public static void JumpToSetting(Action<GameObject> onLoadedCallback = null, Action<GameObject> onOpenedCallback = null, Action onClosedCallback = null)
			{
				SettingViewModule.OpenData openData = new SettingViewModule.OpenData();
				openData.onCloseCallback = onClosedCallback;
				GameApp.View.OpenView(ViewName.SettingViewModule, openData, 1, onLoadedCallback, onOpenedCallback);
			}
		}

		public class Marerial
		{
			public static void SetColor(Renderer renderer, string name, Color color)
			{
				renderer.material.SetColor(name, color);
			}

			public static Color GetColor(Renderer renderer, string name)
			{
				return renderer.material.GetColor(name);
			}

			public static void SetFloat(Renderer renderer, string name, float value)
			{
				renderer.material.SetFloat(name, value);
			}

			public static float GetFloat(Renderer renderer, string name)
			{
				return renderer.material.GetFloat(name);
			}

			public static void SetInt(Renderer renderer, string name, int value)
			{
				renderer.material.SetInt(name, value);
			}

			public static int GetInt(Renderer renderer, string name)
			{
				return renderer.material.GetInt(name);
			}

			public static void SetSharedInt(Renderer renderer, string name, int value)
			{
				renderer.sharedMaterial.SetInt(name, value);
			}

			public static void SetSharedFloat(Renderer renderer, string name, float value)
			{
				renderer.sharedMaterial.SetFloat(name, value);
			}

			public static void SetSharedColor(Renderer renderer, string name, Color value)
			{
				renderer.sharedMaterial.SetColor(name, value);
			}
		}

		public class Mesh
		{
			public static float ToCenter(Transform t)
			{
				MeshFilter component = t.GetComponent<MeshFilter>();
				if (component == null)
				{
					return 0f;
				}
				global::UnityEngine.Mesh mesh = component.mesh;
				if (mesh == null)
				{
					return 0f;
				}
				Vector3[] vertices = mesh.vertices;
				int num = vertices.Length;
				Vector3 vector;
				vector..ctor(9999f, 0f, 0f);
				Vector3 vector2;
				vector2..ctor(-9999f, 0f, 0f);
				Vector3 vector3;
				vector3..ctor(0f, 0f, 9999f);
				Vector3 vector4;
				vector4..ctor(0f, 0f, -9999f);
				float num2 = 9999f;
				for (int i = 0; i < num; i++)
				{
					Vector3 vector5 = vertices[i];
					if (vector5.x < vector.x)
					{
						vector = vector5;
					}
					if (vector5.x > vector2.x)
					{
						vector2 = vector5;
					}
					if (vector5.z < vector3.z)
					{
						vector3 = vector5;
					}
					if (vector5.z > vector4.z)
					{
						vector4 = vector5;
					}
					if (vector5.y < num2)
					{
						num2 = vector5.y;
					}
				}
				Vector3 vector6 = new Vector3(vector.x + vector2.x, num2, vector3.z + vector4.z) / 2f;
				t.localPosition = -vector6;
				return num2;
			}
		}

		public class Node
		{
			public static Transform CoinFly
			{
				get
				{
					if (DxxTools.Node.coinflyparent == null)
					{
						GameObject gameObject = new GameObject("CoinFly");
						DxxTools.Node.coinflyparent = gameObject.transform;
						Object.DontDestroyOnLoad(gameObject);
						DxxTools.Node.coinflyparent.position = new Vector3(0f, 0f, 500f);
					}
					return DxxTools.Node.coinflyparent;
				}
			}

			public static void RemoveCoinFly()
			{
				if (DxxTools.Node.coinflyparent != null)
				{
					Object.Destroy(DxxTools.Node.coinflyparent.gameObject);
				}
			}

			private static Transform coinflyparent;
		}

		public static class Reward
		{
			public static List<ItemData> ParseReward(string[] rewards)
			{
				List<ItemData> list = new List<ItemData>();
				if (rewards == null)
				{
					return list;
				}
				foreach (string text in rewards)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array = text.Split(',', StringSplitOptions.None);
						if (array.Length == 2)
						{
							list.Add(new ItemData(int.Parse(array[0]), long.Parse(array[1])));
						}
					}
				}
				return list;
			}
		}

		public static class Time
		{
			[GameTestMethod("服务器", "服务器时间打印", "", 0)]
			private static void PrintServerTime()
			{
				DateTime serverLocal = DxxTools.Time.ServerLocal;
				DateTime dateTime = serverLocal.ToUniversalTime();
				HLog.LogError(string.Format("dt:{0} utc:{1}", serverLocal, dateTime));
			}

			public static long ServerTimestamp
			{
				get
				{
					return DxxTools.Time.lastServerTimestamp + (long)(global::UnityEngine.Time.realtimeSinceStartup - DxxTools.Time.lastSyncTime);
				}
			}

			public static DateTime ServerUTC
			{
				get
				{
					return DxxTools.Time.UnixTimestampToDateTime((double)DxxTools.Time.ServerTimestamp);
				}
			}

			public static DateTime ServerLocal
			{
				get
				{
					return DxxTools.Time.UnixTimestampToDateTime((double)DxxTools.Time.ServerTimestamp).AddHours((double)DxxTools.Time.Timezone);
				}
			}

			public static void InitServerTimestamp(long serverTimestamp, string timezoneStr = "")
			{
				DxxTools.Time.isServerTimeInit = true;
				DxxTools.Time.SyncServerTimestamp(serverTimestamp);
				if (!string.IsNullOrEmpty(timezoneStr))
				{
					int.TryParse(timezoneStr.Replace(" ", "").Replace("GMT+", ""), out DxxTools.Time.Timezone);
				}
				DxxTools.Time.lastRefreshDateTime = DxxTools.Time.ServerLocal;
			}

			public static void SyncServerTimestamp(long serverTimestamp)
			{
				DxxTools.Time.lastSyncTime = global::UnityEngine.Time.realtimeSinceStartup;
				DxxTools.Time.lastServerTimestamp = serverTimestamp;
			}

			public static void TryTriggerServerDayChange()
			{
				if (!DxxTools.Time.isServerTimeInit)
				{
					return;
				}
				DateTime serverLocal = DxxTools.Time.ServerLocal;
				if (serverLocal.Year != DxxTools.Time.lastRefreshDateTime.Year || serverLocal.Month != DxxTools.Time.lastRefreshDateTime.Month || serverLocal.Day != DxxTools.Time.lastRefreshDateTime.Day)
				{
					DxxTools.Time.lastRefreshDateTime = serverLocal;
					int num = Utility.Math.Random(2000, 10000);
					DelayCall.Instance.CallOnce(num, delegate
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_DAY_CHANGE, null);
					});
				}
			}

			public static void ResetServerTime()
			{
				DxxTools.Time.isServerTimeInit = false;
			}

			public static DateTime UnixTimestampToServerLocalDateTime(double unixSecondsTimestamp)
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				dateTime = dateTime.AddHours((double)DxxTools.Time.Timezone).AddSeconds(unixSecondsTimestamp);
				return dateTime;
			}

			public static DateTime UnixTimestampToDateTime(double unixTimestamp)
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return dateTime.AddSeconds(unixTimestamp);
			}

			public static double DateTimeToUnixTimestamp(DateTime dateTime)
			{
				DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return (dateTime - dateTime2).TotalSeconds;
			}

			public static long TimestampToMidNightTimestamp(long timestamp)
			{
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
				return new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, 0, 0, 0, DateTimeKind.Utc).ToUnixTimeSeconds();
			}

			private static bool isServerTimeInit;

			private static DateTime lastRefreshDateTime;

			public static long lastServerTimestamp;

			public static int Timezone;

			private static float lastSyncTime;
		}

		public class UI
		{
			public static void OpenRewardCommon(ItemData itemData, Action actionClose = null, bool isFly = true)
			{
				if (itemData == null)
				{
					if (actionClose != null)
					{
						actionClose();
					}
					return;
				}
				List<ItemData> list = new List<ItemData>();
				if (itemData != null)
				{
					list.Add(itemData);
				}
				DxxTools.UI.OpenRewardCommon(list, actionClose, isFly);
			}

			public static void OpenRewardCommon(List<ItemData> list, Action actionClose = null, bool isFly = true)
			{
				if (list == null || list.Count <= 0)
				{
					if (actionClose != null)
					{
						actionClose();
					}
					return;
				}
				RewardCommonData rewardCommonData = new RewardCommonData();
				rewardCommonData.list = list;
				rewardCommonData.OnClose = actionClose;
				rewardCommonData.m_isFly = isFly;
				GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
			}

			public static void OpenRewardCommon(RepeatedField<ItemDto> list)
			{
				if (list == null || list.Count <= 0)
				{
					return;
				}
				RewardCommonData rewardCommonData = new RewardCommonData();
				rewardCommonData.list = list.ToItemDatas();
				GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
			}

			public static void OpenRewardCommon(RepeatedField<RewardDto> list, Action actionClose = null, bool isFly = true)
			{
				if (list == null || list.Count <= 0)
				{
					if (actionClose != null)
					{
						actionClose();
					}
					return;
				}
				RewardCommonData rewardCommonData = new RewardCommonData();
				rewardCommonData.list = new List<ItemData>();
				for (int i = 0; i < list.Count; i++)
				{
					ItemData itemData = new ItemData();
					itemData.SetID(list[i].ConfigId);
					itemData.SetCount((long)list[i].Count);
					rewardCommonData.list.Add(itemData);
				}
				rewardCommonData.m_isFly = isFly;
				rewardCommonData.OnClose = actionClose;
				GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
			}

			public static void OpenRewardCommonEquips(List<EquipData> equipDatas, List<ItemData> itemDatas, string title = "")
			{
				RewardCommonData rewardCommonData = new RewardCommonData();
				rewardCommonData.list = itemDatas;
				rewardCommonData.epuipList = equipDatas;
				rewardCommonData.m_title = title;
				GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
			}

			public static PopCommonData GetPopCommonData(string content, Action<int> action, string title, string sure, string cancle, bool showclose)
			{
				PopCommonData popCommonData = new PopCommonData();
				popCommonData.m_title = title;
				popCommonData.m_content = content;
				popCommonData.m_isShowCancel = !string.IsNullOrEmpty(cancle);
				popCommonData.m_isShowSure = !string.IsNullOrEmpty(sure);
				popCommonData.m_isShowClose = showclose;
				popCommonData.m_sureContent = sure;
				popCommonData.m_cancelContent = cancle;
				if (popCommonData.m_isShowCancel)
				{
					popCommonData.m_onCancel = delegate
					{
						Action<int> action2 = action;
						if (action2 == null)
						{
							return;
						}
						action2(-1);
					};
				}
				else
				{
					popCommonData.m_onCancel = null;
				}
				if (popCommonData.m_isShowSure)
				{
					popCommonData.m_onSure = delegate
					{
						Action<int> action3 = action;
						if (action3 == null)
						{
							return;
						}
						action3(1);
					};
				}
				else
				{
					popCommonData.m_onSure = null;
				}
				if (popCommonData.m_isShowClose)
				{
					popCommonData.m_onClose = delegate
					{
						Action<int> action4 = action;
						if (action4 == null)
						{
							return;
						}
						action4(0);
					};
				}
				else
				{
					popCommonData.m_onClose = null;
				}
				return popCommonData;
			}

			public static void OpenPopCommon(string content, Action<int> action, string title, string sure, string cancle, bool showclose, UILayers layer = 2)
			{
				PopCommonData popCommonData = new PopCommonData();
				popCommonData.m_title = title;
				popCommonData.m_content = content;
				popCommonData.m_isShowCancel = !string.IsNullOrEmpty(cancle);
				popCommonData.m_isShowSure = !string.IsNullOrEmpty(sure);
				popCommonData.m_isShowClose = showclose;
				popCommonData.m_sureContent = sure;
				popCommonData.m_cancelContent = cancle;
				if (popCommonData.m_isShowCancel)
				{
					popCommonData.m_onCancel = delegate
					{
						Action<int> action2 = action;
						if (action2 == null)
						{
							return;
						}
						action2(-1);
					};
				}
				else
				{
					popCommonData.m_onCancel = null;
				}
				if (popCommonData.m_isShowSure)
				{
					popCommonData.m_onSure = delegate
					{
						Action<int> action3 = action;
						if (action3 == null)
						{
							return;
						}
						action3(1);
					};
				}
				else
				{
					popCommonData.m_onSure = null;
				}
				if (popCommonData.m_isShowClose)
				{
					popCommonData.m_onClose = delegate
					{
						Action<int> action4 = action;
						if (action4 == null)
						{
							return;
						}
						action4(0);
					};
				}
				else
				{
					popCommonData.m_onClose = null;
				}
				GameApp.View.OpenView(ViewName.PopCommonViewModule, popCommonData, layer, null, null);
			}

			public static void OpenPopCommon2(string content, Action<int> action, string title, string sure, string cancle, bool showclose, UILayers layer = 2)
			{
				PopCommonData popCommonData = new PopCommonData();
				popCommonData.m_title = title;
				popCommonData.m_content = content;
				popCommonData.m_isShowCancel = !string.IsNullOrEmpty(cancle);
				popCommonData.m_isShowSure = !string.IsNullOrEmpty(sure);
				popCommonData.m_isShowClose = showclose;
				popCommonData.m_sureContent = sure;
				popCommonData.m_cancelContent = cancle;
				if (popCommonData.m_isShowCancel)
				{
					popCommonData.m_onCancel = delegate
					{
						Action<int> action2 = action;
						if (action2 == null)
						{
							return;
						}
						action2(-1);
					};
				}
				else
				{
					popCommonData.m_onCancel = null;
				}
				if (popCommonData.m_isShowSure)
				{
					popCommonData.m_onSure = delegate
					{
						Action<int> action3 = action;
						if (action3 == null)
						{
							return;
						}
						action3(1);
					};
				}
				else
				{
					popCommonData.m_onSure = null;
				}
				if (popCommonData.m_isShowClose)
				{
					popCommonData.m_onClose = delegate
					{
						Action<int> action4 = action;
						if (action4 == null)
						{
							return;
						}
						action4(0);
					};
				}
				else
				{
					popCommonData.m_onClose = null;
				}
				GameApp.View.OpenView(ViewName.PopCommon2ViewModule, popCommonData, layer, null, null);
			}

			public static void OpenPopCommonDanger(string content, Action<int> action, string title, string sure, string cancle, bool showclose)
			{
				PopCommonData popCommonData = new PopCommonData();
				popCommonData.m_title = title;
				popCommonData.m_content = content;
				popCommonData.m_isShowCancel = !string.IsNullOrEmpty(cancle);
				popCommonData.m_isShowSure = !string.IsNullOrEmpty(sure);
				popCommonData.m_isShowClose = showclose;
				popCommonData.m_sureContent = sure;
				popCommonData.m_cancelContent = cancle;
				popCommonData.m_swappingButton = true;
				if (popCommonData.m_isShowCancel)
				{
					popCommonData.m_onCancel = delegate
					{
						Action<int> action2 = action;
						if (action2 == null)
						{
							return;
						}
						action2(-1);
					};
				}
				else
				{
					popCommonData.m_onCancel = null;
				}
				if (popCommonData.m_isShowSure)
				{
					popCommonData.m_onSure = delegate
					{
						Action<int> action3 = action;
						if (action3 == null)
						{
							return;
						}
						action3(1);
					};
				}
				else
				{
					popCommonData.m_onSure = null;
				}
				if (popCommonData.m_isShowClose)
				{
					popCommonData.m_onClose = delegate
					{
						Action<int> action4 = action;
						if (action4 == null)
						{
							return;
						}
						action4(0);
					};
				}
				else
				{
					popCommonData.m_onClose = null;
				}
				GameApp.View.OpenView(ViewName.PopCommonViewModule, popCommonData, 2, null, null);
			}

			public static void OpenPopCommon(string content, Action<int> action)
			{
				DxxTools.UI.OpenPopCommon(content, action, Singleton<LanguageManager>.Instance.GetInfoByID("43"), Singleton<LanguageManager>.Instance.GetInfoByID("17"), Singleton<LanguageManager>.Instance.GetInfoByID("18"), true, 2);
			}

			public static void OpenPopCommonNoCancle(string contentInfo, Action<int> action)
			{
				DxxTools.UI.OpenPopCommon(contentInfo, action, Singleton<LanguageManager>.Instance.GetInfoByID("43"), Singleton<LanguageManager>.Instance.GetInfoByID("17"), Singleton<LanguageManager>.Instance.GetInfoByID("18"), true, 2);
			}

			public static string GetPopCommonTitle()
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("Global_CommonPopTitle");
			}

			public static string GetPopCommonSure()
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("Global_Confirm");
			}

			public static string GetPopCommonCancel()
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("Global_Cancel");
			}

			public static void OpenPopBuy(string title, List<ItemData> rewards, ItemData cost, Action surecallback, Action canclecallback = null)
			{
				ShopBuyConfirmData shopBuyConfirmData = new ShopBuyConfirmData();
				shopBuyConfirmData.m_title = title;
				shopBuyConfirmData.SetRewards(rewards);
				shopBuyConfirmData.SetCost(cost);
				shopBuyConfirmData.m_onSure = surecallback;
				shopBuyConfirmData.m_onCancel = canclecallback;
				GameApp.View.OpenView(ViewName.ShopBuyConfirmViewModule, shopBuyConfirmData, 2, null, null);
			}

			public static void OpenPopCommonUse(CommonUseTipViewModule.OpenData openData)
			{
				GameApp.View.OpenView(ViewName.CommonUseTipViewModule, openData, 1, null, null);
			}

			public static bool CheckCurrencyAndShowTips(int itemid, int needcount, bool showtips = true)
			{
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemid)) >= (long)needcount)
				{
					return true;
				}
				if (itemid != 1)
				{
					if (itemid != 2)
					{
						GameApp.View.ShowItemNotEnoughTip(itemid, showtips);
					}
					else
					{
						GameApp.View.ShowItemNotEnoughTip(itemid, showtips);
					}
				}
				else
				{
					GameApp.View.ShowItemNotEnoughTip(itemid, showtips);
				}
				return false;
			}

			public static void OnItemClick(UIItem item, PropData data, object openData)
			{
				DxxTools.UI.OnItemClick(item, data, openData, Vector3.zero, 0f);
			}

			public static void OnItemClick(UIItem item, PropData data, object openData, Vector3 clickPos, float offsetY)
			{
				ItemInfoOpenData itemInfoOpenData = openData as ItemInfoOpenData;
				if (itemInfoOpenData == null)
				{
					itemInfoOpenData = new ItemInfoOpenData();
				}
				itemInfoOpenData.m_propData = data;
				itemInfoOpenData.m_openDataType = ItemInfoOpenDataType.eShow;
				itemInfoOpenData.m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume);
				DxxTools.UI.ShowItemInfo(itemInfoOpenData, clickPos, offsetY);
			}

			public static long OnItemInfoMathVolume(PropData data)
			{
				long num = 0L;
				if (data == null)
				{
					return num;
				}
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				PropDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.PropDataModule);
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)data.id);
				if (elementById == null)
				{
					return 0L;
				}
				ItemType itemType = (ItemType)elementById.itemType;
				TicketDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
				switch (itemType)
				{
				case ItemType.eCurrency:
					if (data.id == 1U || data.id == 4U)
					{
						return (long)((int)dataModule.userCurrency.Coins);
					}
					if (data.id == 2U)
					{
						return (long)dataModule.userCurrency.Diamonds;
					}
					if (data.id == 9U)
					{
						UserTicket ticket = dataModule3.GetTicket(UserTicketKind.UserLife);
						return (long)((ulong)((ticket != null) ? ticket.NewNum : 0U));
					}
					if (data.id == 32U)
					{
						UserTicket ticket2 = dataModule3.GetTicket(UserTicketKind.Mining);
						return (long)((ulong)((ticket2 != null) ? ticket2.NewNum : 0U));
					}
					if (data.id == 34U)
					{
						UserTicket ticket3 = dataModule3.GetTicket(UserTicketKind.RogueDungeon);
						return (long)((ulong)((ticket3 != null) ? ticket3.NewNum : 0U));
					}
					return num;
				case ItemType.eEquip:
				case ItemType.eHero:
					return num;
				case ItemType.eUseItem:
				case ItemType.eRandomPack:
				case ItemType.eCustomPack:
				case ItemType.eTimePack:
				case ItemType.eHeroePieces:
					if (data.id == 5U)
					{
						return (long)dataModule.userCurrency.CardExp;
					}
					return dataModule2.GetItemDataCountByid((ulong)data.id);
				case ItemType.eDreamlandItem:
				case ItemType.eFixedPropsPackage:
				case ItemType.eMagicStone:
				case ItemType.eBless:
				case (ItemType)12:
				case ItemType.eRelic:
				case ItemType.eRelicFragment:
				case ItemType.eTowerTicket:
				case ItemType.eCrossArena:
				case (ItemType)17:
				case ItemType.eSevenDayCarnival:
					break;
				case ItemType.ePet:
				{
					PetData petDataByConfigId = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId((int)data.id);
					if (petDataByConfigId != null)
					{
						return (long)petDataByConfigId.petCount;
					}
					return num;
				}
				case ItemType.ePetFragment:
				{
					PetData petDataByConfigId2 = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId(int.Parse(elementById.itemTypeParam[0]));
					if (petDataByConfigId2 != null)
					{
						return (long)petDataByConfigId2.fragmentCount;
					}
					return num;
				}
				case ItemType.eCollection:
				case ItemType.eCollectionFragment:
				case ItemType.eCollectionShareFragment:
					return (long)GameApp.Data.GetDataModule(DataName.CollectionDataModule).GetCollectionCount((int)data.id);
				case ItemType.DungeonDragonLair:
				{
					UserTicket ticket4 = dataModule3.GetTicket(UserTicketKind.DragonLair);
					return (long)((ulong)((ticket4 != null) ? ticket4.NewNum : 0U));
				}
				case ItemType.DungeonAstralTree:
				{
					UserTicket ticket5 = dataModule3.GetTicket(UserTicketKind.AstralTree);
					return (long)((ulong)((ticket5 != null) ? ticket5.NewNum : 0U));
				}
				case ItemType.SwordIsland:
				{
					UserTicket ticket6 = dataModule3.GetTicket(UserTicketKind.SwordIsland);
					return (long)((ulong)((ticket6 != null) ? ticket6.NewNum : 0U));
				}
				case ItemType.DeepSeaRuins:
				{
					UserTicket ticket7 = dataModule3.GetTicket(UserTicketKind.DeepSeaRuins);
					return (long)((ulong)((ticket7 != null) ? ticket7.NewNum : 0U));
				}
				default:
					switch (itemType)
					{
					case ItemType.eGuildGold:
						return dataModule2.GetItemDataCountByid((ulong)data.id);
					case ItemType.eGuildExp:
						return (long)GuildSDKManager.Instance.GuildInfo.GuildData.GuildExp;
					case ItemType.eGuildDust:
						return (long)GuildSDKManager.Instance.User.MyUserData.WeeklyActive;
					}
					break;
				}
				num = dataModule2.GetItemDataCountByid((ulong)data.id);
				return num;
			}

			public static void ShowItemInfo(ItemInfoOpenData openData, Vector3 clickPos = default(Vector3), float offsetY = 0f)
			{
				if (openData == null)
				{
					return;
				}
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)openData.m_propData.id);
				ItemType itemType = elementById.GetItemType();
				if (itemType <= ItemType.eRelic)
				{
					if (itemType == ItemType.eEquip)
					{
						EquipDetailsViewModule.OpenData openData2 = new EquipDetailsViewModule.OpenData();
						openData2.m_equipData = openData.m_propData.ToEquipData();
						openData2.m_isShowButtons = false;
						if (GameApp.View.IsOpened(ViewName.EquipDetailsViewModule))
						{
							GameApp.View.CloseView(ViewName.EquipDetailsViewModule, null);
						}
						GameApp.View.OpenView(ViewName.EquipDetailsViewModule, openData2, 2, null, null);
						return;
					}
					if (itemType == ItemType.eRelic)
					{
						RelicDetailsViewModule.OpenData openData3 = new RelicDetailsViewModule.OpenData();
						openData3.m_relicData = openData.m_propData.ToRelicData();
						openData3.m_isShowBt = false;
						GameApp.View.OpenView(ViewName.RelicDetailsViewModule, openData3, 2, null, null);
						return;
					}
				}
				else
				{
					if (itemType == ItemType.ePet)
					{
						PetItemTipInfoViewModule.OpenData openData4 = new PetItemTipInfoViewModule.OpenData();
						openData4.petDto = openData.m_propData.ToPetDto();
						GameApp.View.OpenView(ViewName.PetItemTipInfoViewModule, openData4, 2, null, null);
						return;
					}
					if (itemType == ItemType.ePetFragment)
					{
						if (elementById.itemTypeParam == null || elementById.itemTypeParam.Length == 0)
						{
							return;
						}
						PetItemTipInfoViewModule.OpenData openData5 = new PetItemTipInfoViewModule.OpenData();
						openData5.petDto = openData.m_propData.ToPetDto();
						openData5.petDto.ConfigId = uint.Parse(elementById.itemTypeParam[0]);
						GameApp.View.OpenView(ViewName.PetItemTipInfoViewModule, openData5, 2, null, null);
						return;
					}
				}
				ItemInfoTipViewModule.InfoTipData infoTipData = new ItemInfoTipViewModule.InfoTipData();
				infoTipData.openData = openData;
				infoTipData.m_position = clickPos;
				infoTipData.m_offsetY = offsetY;
				GameApp.View.OpenView(ViewName.ItemInfoTipViewModule, infoTipData, 3, null, null);
			}

			public static string GetPVELevelShortName(int id)
			{
				int num = id / 1000;
				int num2 = id % 1000;
				return string.Format("{0}-{1}", num, num2);
			}

			public static string GetPVELevelLongName(int id)
			{
				int num = id / 1000;
				int num2 = id % 1000;
				return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(5000, new object[] { num, num2 });
			}

			public static MainOpenData GetConquerForMainOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainViewModule viewModule = GameApp.View.GetViewModule(ViewName.MainViewModule);
				if (viewModule != null)
				{
					mainOpenData.m_pageName = viewModule.GetCurrentPageEnum();
				}
				Dictionary<BaseViewModule, ViewName> dictionary = new Dictionary<BaseViewModule, ViewName>();
				OtherMainCityViewModule viewModule2 = GameApp.View.GetViewModule(ViewName.OtherMainCityViewModule);
				if (viewModule2 != null && GameApp.View.IsOpened(ViewName.OtherMainCityViewModule))
				{
					dictionary.Add(viewModule2, ViewName.OtherMainCityViewModule);
				}
				PlayerInformationViewModule viewModule3 = GameApp.View.GetViewModule(ViewName.PlayerInformationViewModule);
				if (viewModule3 != null && GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
				{
					dictionary.Add(viewModule3, ViewName.PlayerInformationViewModule);
				}
				ConquerViewModule viewModule4 = GameApp.View.GetViewModule(ViewName.ConquerViewModule);
				if (viewModule4 != null && GameApp.View.IsOpened(ViewName.ConquerViewModule))
				{
					dictionary.Add(viewModule4, ViewName.ConquerViewModule);
				}
				SocialityViewModule viewModule5 = GameApp.View.GetViewModule(ViewName.SocialityViewModule);
				if (viewModule5 != null && GameApp.View.IsOpened(ViewName.SocialityViewModule))
				{
					dictionary.Add(viewModule5, ViewName.SocialityViewModule);
				}
				ReportConquerViewModule viewModule6 = GameApp.View.GetViewModule(ViewName.ReportConquerViewModule);
				if (viewModule6 != null && GameApp.View.IsOpened(ViewName.ReportConquerViewModule))
				{
					dictionary.Add(viewModule6, ViewName.ReportConquerViewModule);
				}
				dictionary = dictionary.OrderBy((KeyValuePair<BaseViewModule, ViewName> c) => c.Key.transform.GetSiblingIndex()).ToDictionary((KeyValuePair<BaseViewModule, ViewName> item) => item.Key, (KeyValuePair<BaseViewModule, ViewName> item) => item.Value);
				List<MainOpenViewData> list = new List<MainOpenViewData>();
				foreach (BaseViewModule baseViewModule in dictionary.Keys)
				{
					ViewName viewName = dictionary[baseViewModule];
					if (!(baseViewModule == null))
					{
						MainOpenViewData mainOpenViewData = new MainOpenViewData();
						mainOpenViewData.m_viewID = (int)viewName;
						mainOpenViewData.m_layer = 1;
						if (mainOpenViewData.m_viewID == 801)
						{
							SocialityViewModule viewModule7 = GameApp.View.GetViewModule(ViewName.SocialityViewModule);
							mainOpenViewData.m_openData = new SocialityViewModule.OpenData
							{
								m_selectIndex = viewModule7.GetSelectIndex()
							};
						}
						else
						{
							mainOpenViewData.m_openData = null;
						}
						list.Add(mainOpenViewData);
					}
				}
				mainOpenData.m_openViewDatas = list;
				return mainOpenData;
			}

			public static MainOpenData GetCrossArenaRecordOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 400;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 403;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetCrossArenaChallengeOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 984;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = new DailyActivitiesViewModule.OpenData
				{
					openPageType = DailyActivitiesPageType.Challenge
				};
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 400;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetGuildBossOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 515;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 513;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetGuildRaceOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 515;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 518;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				if (GameApp.View.IsOpened(ViewName.GuildRaceRecordMainViewModule))
				{
					MainOpenViewData mainOpenViewData3 = new MainOpenViewData();
					mainOpenViewData3.m_viewID = 519;
					mainOpenViewData3.m_layer = 1;
					mainOpenViewData3.m_openData = null;
					mainOpenData.m_openViewDatas.Add(mainOpenViewData3);
				}
				return mainOpenData;
			}

			public static MainOpenData GetTowerOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 984;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = new DailyActivitiesViewModule.OpenData
				{
					openPageType = DailyActivitiesPageType.Challenge
				};
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 816;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetDungeonOpenData(DungeonID dungeonId)
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 984;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = new DailyActivitiesViewModule.OpenData
				{
					openPageType = DailyActivitiesPageType.Dungeon
				};
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 983;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = dungeonId;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetEquipOpenData()
			{
				return new MainOpenData
				{
					m_pageName = UIMainPageName.Equip
				};
			}

			public static MainOpenData GetTalentOpenData()
			{
				return new MainOpenData
				{
					m_pageName = UIMainPageName.Talent
				};
			}

			public static MainOpenData GetRogueDungeonOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 984;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = new DailyActivitiesViewModule.OpenData
				{
					openPageType = DailyActivitiesPageType.Challenge
				};
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 1010;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static MainOpenData GetWorldBossOpenData()
			{
				MainOpenData mainOpenData = new MainOpenData();
				mainOpenData.m_pageName = UIMainPageName.Battle;
				MainOpenViewData mainOpenViewData = new MainOpenViewData();
				mainOpenViewData.m_viewID = 984;
				mainOpenViewData.m_layer = 1;
				mainOpenViewData.m_openData = new DailyActivitiesViewModule.OpenData
				{
					openPageType = DailyActivitiesPageType.Challenge
				};
				mainOpenData.m_openViewDatas.Add(mainOpenViewData);
				MainOpenViewData mainOpenViewData2 = new MainOpenViewData();
				mainOpenViewData2.m_viewID = 907;
				mainOpenViewData2.m_layer = 1;
				mainOpenViewData2.m_openData = null;
				mainOpenData.m_openViewDatas.Add(mainOpenViewData2);
				return mainOpenData;
			}

			public static void DoMoveRightToScreenAnim(Sequence sequence, RectTransform rectTran, float endPosX, float delayTime, float duration = 0.2f, Ease ease = 9)
			{
				if (rectTran == null)
				{
					return;
				}
				rectTran.anchoredPosition = new Vector2(endPosX - 2000f, rectTran.anchoredPosition.y);
				TweenSettingsExtensions.Insert(sequence, 0f, TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(rectTran, endPosX, duration, false), ease), delayTime));
			}

			public static void DoMoveRightToScreenAnim(Sequence sequence, List<RectTransform> rectTranList, float endPosX, float delayTime = 0.05f, float duration = 0.2f, Ease ease = 9)
			{
				for (int i = 0; i < rectTranList.Count; i++)
				{
					RectTransform rectTransform = rectTranList[i];
					if (!(rectTransform == null))
					{
						DxxTools.UI.DoMoveRightToScreenAnim(sequence, rectTransform, endPosX, (float)i * delayTime, duration, ease);
					}
				}
			}

			public static void DoMoveRightToScreenAnim(Sequence sequence, List<LoopListViewItem2> loopItemList, float endPosX, float delayTime = 0.05f, float duration = 0.2f, Ease ease = 9)
			{
				for (int i = 0; i < loopItemList.Count; i++)
				{
					LoopListViewItem2 loopListViewItem = loopItemList[i];
					if (!(loopListViewItem == null))
					{
						DxxTools.UI.DoMoveRightToScreenAnim(sequence, loopListViewItem.CachedRectTransform, endPosX, (float)i * delayTime, duration, ease);
					}
				}
			}

			public static void DoListScaleZeroToOneAnim(Sequence sequence, List<LoopListViewItem2> loopItemList, float delayTime = 0.05f, float duration = 0.2f, Ease ease = 0)
			{
				for (int i = 0; i < loopItemList.Count; i++)
				{
					LoopListViewItem2 loopListViewItem = loopItemList[i];
					if (!(loopListViewItem == null))
					{
						DxxTools.UI.DoScaleAnim(sequence, loopListViewItem.CachedRectTransform, 0f, 1f, (float)i * delayTime, duration, ease);
					}
				}
			}

			public static void DoListScaleAnim(Sequence sequence, List<CustomBehaviour> items, float from, float end, float delayTime = 0.05f, float duration = 0.2f, Ease ease = 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					CustomBehaviour customBehaviour = items[i];
					if (!(customBehaviour == null))
					{
						DxxTools.UI.DoScaleAnim(sequence, customBehaviour.transform, from, end, (float)i * delayTime, duration, ease);
					}
				}
			}

			public static void DoScaleAnim(Sequence sequence, Transform tf, float from, float end, float delayTime = 0.05f, float duration = 0.2f, Ease ease = 0)
			{
				if (tf == null)
				{
					return;
				}
				tf.localScale = new Vector3(from, from, from);
				TweenSettingsExtensions.Insert(sequence, 0f, TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(tf, end, duration), ease), delayTime));
			}

			public static void AutoChangeTextSize(Text text, int defaultfontsize)
			{
				string text2 = text.text;
				int i = defaultfontsize;
				text.fontSize = i;
				text.text = text2;
				Rect rect = text.rectTransform.rect;
				while (i > 0)
				{
					i--;
					if (text.preferredHeight <= rect.height)
					{
						break;
					}
					text.fontSize = i;
					text.text = text2;
				}
			}

			public static string GetColorString(string content, string color)
			{
				if (string.IsNullOrEmpty(content))
				{
					return content;
				}
				return string.Concat(new string[] { "<color=", color, ">", content, "</color>" });
			}

			public static void SetTextWithEllipsis(Text textComponent, string value)
			{
				if (textComponent == null)
				{
					return;
				}
				if (string.IsNullOrEmpty(value))
				{
					textComponent.text = "";
					return;
				}
				TextGenerator textGenerator = new TextGenerator();
				RectTransform component = textComponent.GetComponent<RectTransform>();
				TextGenerationSettings generationSettings = textComponent.GetGenerationSettings(component.rect.size);
				textGenerator.Populate(value, generationSettings);
				int characterCountVisible = textGenerator.characterCountVisible;
				textComponent.text = value;
			}

			public static void AddServerTimeClockCallback(string key, Action callback, int hour = 0, int minute = 0, int second = 0)
			{
				ServerTimeClockCall serverTimeClockCall = new ServerTimeClockCall(key, callback);
				serverTimeClockCall.SetClockTime(hour, minute, second);
				HeartBeatCtrl_ServerTimeClock.Instance.OnAddServerTimeClock(serverTimeClockCall);
			}

			public static void AddServerTimeCallback(string key, Action callback, long tick, int repeatinterval = 0)
			{
				if (tick <= 0L)
				{
					HLog.LogError(string.Format("AddServerTimeCallback > key = {0} , but tick = {1} !!!", key, tick));
					return;
				}
				if (repeatinterval > 0)
				{
				}
				if (DxxTools.Time.ServerTimestamp < tick)
				{
					ServerTimeClockCall serverTimeClockCall = new ServerTimeClockCall(key, callback);
					serverTimeClockCall.SetTick(tick);
					serverTimeClockCall.RepeatDaily = repeatinterval > 0;
					serverTimeClockCall.RepeatInterval = repeatinterval;
					HeartBeatCtrl_ServerTimeClock.Instance.OnAddServerTimeClock(serverTimeClockCall);
					return;
				}
				if (callback != null)
				{
					callback();
				}
				if (repeatinterval <= 0)
				{
					return;
				}
				HLog.LogError(string.Format("AddServerTimeCallback > key = {0} , set repeat ({1}) but tick = {2} is timeout", key, repeatinterval, tick));
			}

			public static void RemoveServerTimeClockCallback(string key)
			{
				HeartBeatCtrl_ServerTimeClock.Instance.OnRemoveServerTimeClock(key);
			}

			public static string GetTitlePathByQuality(int quality)
			{
				if (quality == 1)
				{
					return "title_gray";
				}
				if (quality == 2)
				{
					return "title_green";
				}
				if (quality == 3)
				{
					return "title_blue";
				}
				if (quality == 4)
				{
					return "title_purple";
				}
				if (quality == 5)
				{
					return "title_yellow";
				}
				if (quality == 6)
				{
					return "title_red";
				}
				return "";
			}

			public static string GetBoxQualityInfo(int quality)
			{
				switch (quality)
				{
				case 1:
					return "mining_quality_1";
				case 2:
					return "mining_quality_2";
				case 3:
					return "mining_quality_3";
				case 4:
					return "mining_quality_4";
				case 5:
					return "mining_quality_5";
				case 6:
					return "mining_quality_6";
				default:
					return "";
				}
			}

			public static string GetRromeNumberByNumber(int level)
			{
				switch (level)
				{
				case 1:
					return "Ⅰ";
				case 2:
					return "Ⅱ";
				case 3:
					return "Ⅲ";
				case 4:
					return "Ⅳ";
				case 5:
					return "Ⅴ";
				case 6:
					return "Ⅵ";
				default:
					return "";
				}
			}
		}
	}
}
