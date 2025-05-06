using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Proto.GuildRace;

namespace Dxx.Guild
{
	public static class GuildRaceGuildRecordResponseEx
	{
		public static List<GuildRaceGuildVSRecord> ToGuildVSRecordList(this GuildRaceGuildRecordResponse recordDto)
		{
			GuildRaceGuildRecordResponseEx.SearchTemp searchTemp = new GuildRaceGuildRecordResponseEx.SearchTemp();
			searchTemp.BuildCacheBase(recordDto);
			searchTemp.BuildCacheGuildVS();
			searchTemp.BuildCacheUserVS();
			searchTemp.CompletionBattleRecord();
			return searchTemp.GuildVSList;
		}

		public const int BattleGuildVSCount = 5;

		private class SearchTemp
		{
			public void BuildCacheBase(GuildRaceGuildRecordResponse response)
			{
				this.Response = response;
				this.GuildList.Clear();
				this.GuildList.AddRange(response.Guilds);
				RepeatedField<RaceGuildDto> guilds = response.Guilds;
				RepeatedField<RaceUserDto> users = response.Users;
				foreach (RaceGuildDto raceGuildDto in guilds)
				{
					if (raceGuildDto.GuildId != 0UL)
					{
						this.GuildDic[raceGuildDto.GuildId] = raceGuildDto;
					}
				}
				foreach (RaceUserDto raceUserDto in users)
				{
					if (raceUserDto.UserId != 0L)
					{
						this.UserDic[raceUserDto.UserId] = raceUserDto;
					}
				}
			}

			public void BuildCacheGuildVS()
			{
				ulong num = 0UL;
				if (GuildSDKManager.Instance.GuildInfo.HasGuild)
				{
					num = GuildSDKManager.Instance.GuildInfo.GuildData.GuildID_ULong;
				}
				foreach (KeyValuePair<ulong, RaceGuildDto> keyValuePair in this.GuildDic)
				{
					if (keyValuePair.Value != null)
					{
						RaceGuildDto value = keyValuePair.Value;
						RaceGuildDto raceGuildDto = null;
						if (!this.GuildDic.TryGetValue(value.OppGuildId, out raceGuildDto))
						{
							raceGuildDto = null;
						}
						GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
						if (!this.GuildVSDic.TryGetValue(value.GuildId, out guildRaceGuildVSRecord) && raceGuildDto != null)
						{
							this.GuildVSDic.TryGetValue(raceGuildDto.GuildId, out guildRaceGuildVSRecord);
						}
						if (guildRaceGuildVSRecord == null)
						{
							guildRaceGuildVSRecord = new GuildRaceGuildVSRecord();
							if (value != null)
							{
								guildRaceGuildVSRecord.Guild1 = value.ToGuildRaceGuild();
							}
							if (raceGuildDto != null)
							{
								guildRaceGuildVSRecord.Guild2 = raceGuildDto.ToGuildRaceGuild();
							}
							if (raceGuildDto.GuildId == num)
							{
								GuildRaceGuild guild = guildRaceGuildVSRecord.Guild1;
								guildRaceGuildVSRecord.Guild1 = guildRaceGuildVSRecord.Guild2;
								guildRaceGuildVSRecord.Guild2 = guild;
							}
						}
						if (value != null)
						{
							this.GuildVSDic[value.GuildId] = guildRaceGuildVSRecord;
						}
						if (raceGuildDto != null)
						{
							this.GuildVSDic[raceGuildDto.GuildId] = guildRaceGuildVSRecord;
						}
					}
				}
				this.GuildVSList.Clear();
				foreach (KeyValuePair<ulong, GuildRaceGuildVSRecord> keyValuePair2 in this.GuildVSDic)
				{
					if (!this.GuildVSList.Contains(keyValuePair2.Value))
					{
						this.GuildVSList.Add(keyValuePair2.Value);
					}
				}
			}

			public void BuildCacheUserVS()
			{
				if (this.Response == null)
				{
					return;
				}
				RepeatedField<UserPVPRecordDto> records = this.Response.Records;
				long num = GuildProxy.GameUser.UserID();
				for (int i = 0; i < records.Count; i++)
				{
					UserPVPRecordDto userPVPRecordDto = records[i];
					if (userPVPRecordDto != null && (userPVPRecordDto.UserId1 != 0L || userPVPRecordDto.UserId2 != 0L))
					{
						int num2 = -1;
						RaceUserDto raceUserDto = this.FindUser(userPVPRecordDto.UserId1);
						RaceGuildDto raceGuildDto = this.FindUserGuild(userPVPRecordDto.UserId1);
						if (userPVPRecordDto.UserId1 != 0L && (raceUserDto == null || raceGuildDto == null))
						{
							HLog.LogError(string.Format("[GuildRace]{0}未找到对战记录中用户{1}对应的用户或公会数据", num, userPVPRecordDto.UserId1));
						}
						RaceUserDto raceUserDto2 = this.FindUser(userPVPRecordDto.UserId2);
						RaceGuildDto raceGuildDto2 = this.FindUserGuild(userPVPRecordDto.UserId2);
						if (userPVPRecordDto.UserId2 != 0L && (raceUserDto2 == null || raceGuildDto2 == null))
						{
							HLog.LogError(string.Format("[GuildRace]{0}未找到对战记录中用户{1}对应的用户或公会数据", num, userPVPRecordDto.UserId1));
						}
						GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
						if (raceUserDto != null)
						{
							num2 = (int)raceUserDto.Seq;
						}
						if (num2 == -1 && raceUserDto2 != null)
						{
							num2 = (int)raceUserDto2.Seq;
						}
						if (num2 != -1)
						{
							if (raceGuildDto != null)
							{
								guildRaceGuildVSRecord = this.FindGuildVSRecordByGuildID(raceGuildDto.GuildId);
							}
							if (guildRaceGuildVSRecord == null && raceGuildDto2 != null)
							{
								guildRaceGuildVSRecord = this.FindGuildVSRecordByGuildID(raceGuildDto2.GuildId);
							}
							if (guildRaceGuildVSRecord != null)
							{
								List<GuildRaceUserVSRecord> resultList = guildRaceGuildVSRecord.ResultList;
								GuildRaceUserVSRecord guildRaceUserVSRecord = null;
								for (int j = 0; j < resultList.Count; j++)
								{
									if (resultList[j].Index == num2)
									{
										guildRaceUserVSRecord = resultList[j];
										break;
									}
								}
								if (guildRaceUserVSRecord == null)
								{
									guildRaceUserVSRecord = new GuildRaceUserVSRecord();
									if (raceUserDto2 != null && guildRaceGuildVSRecord.GuildID1_Ulong == raceUserDto2.GuildId)
									{
										guildRaceUserVSRecord.User1 = raceUserDto2.ToGuildRaceUser();
										guildRaceUserVSRecord.User2 = raceUserDto.ToGuildRaceUser();
									}
									else
									{
										guildRaceUserVSRecord.User1 = raceUserDto.ToGuildRaceUser();
										guildRaceUserVSRecord.User2 = raceUserDto2.ToGuildRaceUser();
									}
									guildRaceUserVSRecord.Index = num2;
									guildRaceUserVSRecord.Position = GuildProxy.Table.GuildRaceUserIndexToPosition(num2);
									resultList.Add(guildRaceUserVSRecord);
									guildRaceGuildVSRecord.SortResultList();
								}
								GuildRaceUserVSRecordResult guildRaceUserVSRecordResult = new GuildRaceUserVSRecordResult();
								guildRaceUserVSRecordResult.HomeUserID = userPVPRecordDto.UserId1;
								guildRaceUserVSRecordResult.WinUserID = ((userPVPRecordDto.Result == 1U) ? userPVPRecordDto.UserId1 : userPVPRecordDto.UserId2);
								guildRaceUserVSRecordResult.BattleRecordID = userPVPRecordDto.RecordRowId;
								guildRaceUserVSRecord.ResultList.Add(guildRaceUserVSRecordResult);
								guildRaceUserVSRecord.CorrectResultList();
							}
						}
					}
				}
			}

			public void CompletionBattleRecord()
			{
				List<GuildRaceGuildVSRecord> guildVSList = this.GuildVSList;
				for (int i = 0; i < guildVSList.Count; i++)
				{
					GuildRaceGuildVSRecord guildRaceGuildVSRecord = guildVSList[i];
					List<GuildRaceUserVSRecord> resultList = guildRaceGuildVSRecord.ResultList;
					List<int> list = new List<int>();
					for (int j = 0; j < 5; j++)
					{
						list.Add(j + 1);
					}
					for (int k = 0; k < resultList.Count; k++)
					{
						list.Remove(resultList[k].Index);
					}
					for (int l = 0; l < list.Count; l++)
					{
						resultList.Add(new GuildRaceUserVSRecord
						{
							Index = list[l],
							Position = GuildProxy.Table.GuildRaceUserIndexToPosition(list[l])
						});
					}
					guildRaceGuildVSRecord.SortResultList();
				}
			}

			public GuildRaceGuildVSRecord GetMyGuildVSRecord()
			{
				GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
				if (GuildSDKManager.Instance.GuildInfo.HasGuild)
				{
					guildRaceGuildVSRecord = this.FindGuildVSRecordByGuildID(GuildSDKManager.Instance.GuildInfo.GuildData.GuildID_ULong);
				}
				return guildRaceGuildVSRecord;
			}

			private GuildRaceGuildVSRecord FindGuildVSRecordByGuildID(ulong guildid)
			{
				GuildRaceGuildVSRecord guildRaceGuildVSRecord;
				if (this.GuildVSDic.TryGetValue(guildid, out guildRaceGuildVSRecord))
				{
					return guildRaceGuildVSRecord;
				}
				return null;
			}

			private GuildRaceGuildVSRecord FindGuildVSRecordByUserID(long userid)
			{
				if (userid != 0L)
				{
					RaceGuildDto raceGuildDto = this.FindUserGuild(userid);
					if (raceGuildDto != null)
					{
						GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
						if (this.GuildVSDic.TryGetValue(raceGuildDto.GuildId, out guildRaceGuildVSRecord))
						{
							return guildRaceGuildVSRecord;
						}
						if (raceGuildDto.OppGuildId != 0UL && this.GuildVSDic.TryGetValue(raceGuildDto.OppGuildId, out guildRaceGuildVSRecord))
						{
							if (guildRaceGuildVSRecord != null)
							{
								this.GuildVSDic[raceGuildDto.GuildId] = guildRaceGuildVSRecord;
							}
							return guildRaceGuildVSRecord;
						}
					}
				}
				return null;
			}

			private RaceUserDto FindUser(long userid)
			{
				if (userid == 0L)
				{
					return null;
				}
				RaceUserDto raceUserDto;
				if (this.UserDic.TryGetValue(userid, out raceUserDto))
				{
					return raceUserDto;
				}
				return null;
			}

			private RaceGuildDto FindUserGuild(long userid)
			{
				RaceUserDto raceUserDto = this.FindUser(userid);
				if (raceUserDto == null)
				{
					return null;
				}
				RaceGuildDto raceGuildDto;
				if (this.GuildDic.TryGetValue(raceUserDto.GuildId, out raceGuildDto))
				{
					return raceGuildDto;
				}
				return null;
			}

			public GuildRaceGuildRecordResponse Response;

			public List<RaceGuildDto> GuildList = new List<RaceGuildDto>();

			public Dictionary<ulong, RaceGuildDto> GuildDic = new Dictionary<ulong, RaceGuildDto>();

			public Dictionary<long, RaceUserDto> UserDic = new Dictionary<long, RaceUserDto>();

			public Dictionary<ulong, GuildRaceGuildVSRecord> GuildVSDic = new Dictionary<ulong, GuildRaceGuildVSRecord>();

			public List<GuildRaceGuildVSRecord> GuildVSList = new List<GuildRaceGuildVSRecord>();
		}
	}
}
