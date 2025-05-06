using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceGuildRecordResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceGuildRecordResponse> Parser
		{
			get
			{
				return GuildRaceGuildRecordResponse._parser;
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
		public RepeatedField<UserPVPRecordDto> Records
		{
			get
			{
				return this.records_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RaceUserDto> Users
		{
			get
			{
				return this.users_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RaceGuildDto> Guilds
		{
			get
			{
				return this.guilds_;
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
			this.records_.WriteTo(output, GuildRaceGuildRecordResponse._repeated_records_codec);
			this.users_.WriteTo(output, GuildRaceGuildRecordResponse._repeated_users_codec);
			this.guilds_.WriteTo(output, GuildRaceGuildRecordResponse._repeated_guilds_codec);
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
			num += this.records_.CalculateSize(GuildRaceGuildRecordResponse._repeated_records_codec);
			num += this.users_.CalculateSize(GuildRaceGuildRecordResponse._repeated_users_codec);
			return num + this.guilds_.CalculateSize(GuildRaceGuildRecordResponse._repeated_guilds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
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
					if (num == 26U)
					{
						this.records_.AddEntriesFrom(input, GuildRaceGuildRecordResponse._repeated_records_codec);
						continue;
					}
					if (num == 34U)
					{
						this.users_.AddEntriesFrom(input, GuildRaceGuildRecordResponse._repeated_users_codec);
						continue;
					}
					if (num == 42U)
					{
						this.guilds_.AddEntriesFrom(input, GuildRaceGuildRecordResponse._repeated_guilds_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildRaceGuildRecordResponse> _parser = new MessageParser<GuildRaceGuildRecordResponse>(() => new GuildRaceGuildRecordResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RecordsFieldNumber = 3;

		private static readonly FieldCodec<UserPVPRecordDto> _repeated_records_codec = FieldCodec.ForMessage<UserPVPRecordDto>(26U, UserPVPRecordDto.Parser);

		private readonly RepeatedField<UserPVPRecordDto> records_ = new RepeatedField<UserPVPRecordDto>();

		public const int UsersFieldNumber = 4;

		private static readonly FieldCodec<RaceUserDto> _repeated_users_codec = FieldCodec.ForMessage<RaceUserDto>(34U, RaceUserDto.Parser);

		private readonly RepeatedField<RaceUserDto> users_ = new RepeatedField<RaceUserDto>();

		public const int GuildsFieldNumber = 5;

		private static readonly FieldCodec<RaceGuildDto> _repeated_guilds_codec = FieldCodec.ForMessage<RaceGuildDto>(42U, RaceGuildDto.Parser);

		private readonly RepeatedField<RaceGuildDto> guilds_ = new RepeatedField<RaceGuildDto>();
	}
}
