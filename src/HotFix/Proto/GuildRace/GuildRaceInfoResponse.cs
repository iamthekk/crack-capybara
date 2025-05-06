using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceInfoResponse> Parser
		{
			get
			{
				return GuildRaceInfoResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong InitTime
		{
			get
			{
				return this.initTime_;
			}
			set
			{
				this.initTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Stage
		{
			get
			{
				return this.stage_;
			}
			set
			{
				this.stage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool UserApply
		{
			get
			{
				return this.userApply_;
			}
			set
			{
				this.userApply_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RaceGuildDto RaceGuild
		{
			get
			{
				return this.raceGuild_;
			}
			set
			{
				this.raceGuild_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RaceGuildDto> GroupRaceGuilds
		{
			get
			{
				return this.groupRaceGuilds_;
			}
		}

		[DebuggerNonUserCode]
		public uint LastDon
		{
			get
			{
				return this.lastDon_;
			}
			set
			{
				this.lastDon_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.Type != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Type);
			}
			if (this.InitTime != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.InitTime);
			}
			if (this.Stage != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Stage);
			}
			if (this.UserApply)
			{
				output.WriteRawTag(48);
				output.WriteBool(this.UserApply);
			}
			if (this.raceGuild_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.RaceGuild);
			}
			this.groupRaceGuilds_.WriteTo(output, GuildRaceInfoResponse._repeated_groupRaceGuilds_codec);
			if (this.LastDon != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.LastDon);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			if (this.InitTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.InitTime);
			}
			if (this.Stage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Stage);
			}
			if (this.UserApply)
			{
				num += 2;
			}
			if (this.raceGuild_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RaceGuild);
			}
			num += this.groupRaceGuilds_.CalculateSize(GuildRaceInfoResponse._repeated_groupRaceGuilds_codec);
			if (this.LastDon != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LastDon);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 18U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 18U)
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.Type = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.InitTime = input.ReadUInt64();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.Stage = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.UserApply = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						if (this.raceGuild_ == null)
						{
							this.raceGuild_ = new RaceGuildDto();
						}
						input.ReadMessage(this.raceGuild_);
						continue;
					}
					if (num == 66U)
					{
						this.groupRaceGuilds_.AddEntriesFrom(input, GuildRaceInfoResponse._repeated_groupRaceGuilds_codec);
						continue;
					}
					if (num == 72U)
					{
						this.LastDon = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildRaceInfoResponse> _parser = new MessageParser<GuildRaceInfoResponse>(() => new GuildRaceInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TypeFieldNumber = 3;

		private uint type_;

		public const int InitTimeFieldNumber = 4;

		private ulong initTime_;

		public const int StageFieldNumber = 5;

		private uint stage_;

		public const int UserApplyFieldNumber = 6;

		private bool userApply_;

		public const int RaceGuildFieldNumber = 7;

		private RaceGuildDto raceGuild_;

		public const int GroupRaceGuildsFieldNumber = 8;

		private static readonly FieldCodec<RaceGuildDto> _repeated_groupRaceGuilds_codec = FieldCodec.ForMessage<RaceGuildDto>(66U, RaceGuildDto.Parser);

		private readonly RepeatedField<RaceGuildDto> groupRaceGuilds_ = new RepeatedField<RaceGuildDto>();

		public const int LastDonFieldNumber = 9;

		private uint lastDon_;
	}
}
