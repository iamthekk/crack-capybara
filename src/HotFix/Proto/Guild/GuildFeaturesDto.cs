using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildFeaturesDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildFeaturesDto> Parser
		{
			get
			{
				return GuildFeaturesDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildTaskDto> Tasks
		{
			get
			{
				return this.tasks_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildShopDto> DailyShop
		{
			get
			{
				return this.dailyShop_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildShopDto> WeeklyShop
		{
			get
			{
				return this.weeklyShop_;
			}
		}

		[DebuggerNonUserCode]
		public GuilSignInDto SignInDto
		{
			get
			{
				return this.signInDto_;
			}
			set
			{
				this.signInDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong DailyRefreshTime
		{
			get
			{
				return this.dailyRefreshTime_;
			}
			set
			{
				this.dailyRefreshTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong WeeklyRefreshTime
		{
			get
			{
				return this.weeklyRefreshTime_;
			}
			set
			{
				this.weeklyRefreshTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GuildBossInfoDto GuildBossInfo
		{
			get
			{
				return this.guildBossInfo_;
			}
			set
			{
				this.guildBossInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TaskRefreshCount
		{
			get
			{
				return this.taskRefreshCount_;
			}
			set
			{
				this.taskRefreshCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxTaskRefreshCount
		{
			get
			{
				return this.maxTaskRefreshCount_;
			}
			set
			{
				this.maxTaskRefreshCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TaskRefreshCost
		{
			get
			{
				return this.taskRefreshCost_;
			}
			set
			{
				this.taskRefreshCost_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<string> SignInRecords
		{
			get
			{
				return this.signInRecords_;
			}
		}

		[DebuggerNonUserCode]
		public GuildDonationDto DonationDto
		{
			get
			{
				return this.donationDto_;
			}
			set
			{
				this.donationDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ApplyCount
		{
			get
			{
				return this.applyCount_;
			}
			set
			{
				this.applyCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DayContributeTimes
		{
			get
			{
				return this.dayContributeTimes_;
			}
			set
			{
				this.dayContributeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int DayALLContributeTimes
		{
			get
			{
				return this.dayALLContributeTimes_;
			}
			set
			{
				this.dayALLContributeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.tasks_.WriteTo(output, GuildFeaturesDto._repeated_tasks_codec);
			this.dailyShop_.WriteTo(output, GuildFeaturesDto._repeated_dailyShop_codec);
			this.weeklyShop_.WriteTo(output, GuildFeaturesDto._repeated_weeklyShop_codec);
			if (this.signInDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.SignInDto);
			}
			if (this.DailyRefreshTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.DailyRefreshTime);
			}
			if (this.WeeklyRefreshTime != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.WeeklyRefreshTime);
			}
			if (this.guildBossInfo_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.GuildBossInfo);
			}
			if (this.TaskRefreshCount != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.TaskRefreshCount);
			}
			if (this.MaxTaskRefreshCount != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.MaxTaskRefreshCount);
			}
			if (this.TaskRefreshCost != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.TaskRefreshCost);
			}
			this.signInRecords_.WriteTo(output, GuildFeaturesDto._repeated_signInRecords_codec);
			if (this.donationDto_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.DonationDto);
			}
			if (this.ApplyCount != 0U)
			{
				output.WriteRawTag(104);
				output.WriteUInt32(this.ApplyCount);
			}
			if (this.DayContributeTimes != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.DayContributeTimes);
			}
			if (this.DayALLContributeTimes != 0)
			{
				output.WriteRawTag(120);
				output.WriteInt32(this.DayALLContributeTimes);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.tasks_.CalculateSize(GuildFeaturesDto._repeated_tasks_codec);
			num += this.dailyShop_.CalculateSize(GuildFeaturesDto._repeated_dailyShop_codec);
			num += this.weeklyShop_.CalculateSize(GuildFeaturesDto._repeated_weeklyShop_codec);
			if (this.signInDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SignInDto);
			}
			if (this.DailyRefreshTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.DailyRefreshTime);
			}
			if (this.WeeklyRefreshTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.WeeklyRefreshTime);
			}
			if (this.guildBossInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildBossInfo);
			}
			if (this.TaskRefreshCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskRefreshCount);
			}
			if (this.MaxTaskRefreshCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxTaskRefreshCount);
			}
			if (this.TaskRefreshCost != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskRefreshCost);
			}
			num += this.signInRecords_.CalculateSize(GuildFeaturesDto._repeated_signInRecords_codec);
			if (this.donationDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.DonationDto);
			}
			if (this.ApplyCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyCount);
			}
			if (this.DayContributeTimes != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DayContributeTimes);
			}
			if (this.DayALLContributeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DayALLContributeTimes);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 58U)
				{
					if (num <= 26U)
					{
						if (num == 10U)
						{
							this.tasks_.AddEntriesFrom(input, GuildFeaturesDto._repeated_tasks_codec);
							continue;
						}
						if (num == 18U)
						{
							this.dailyShop_.AddEntriesFrom(input, GuildFeaturesDto._repeated_dailyShop_codec);
							continue;
						}
						if (num == 26U)
						{
							this.weeklyShop_.AddEntriesFrom(input, GuildFeaturesDto._repeated_weeklyShop_codec);
							continue;
						}
					}
					else if (num <= 40U)
					{
						if (num == 34U)
						{
							if (this.signInDto_ == null)
							{
								this.signInDto_ = new GuilSignInDto();
							}
							input.ReadMessage(this.signInDto_);
							continue;
						}
						if (num == 40U)
						{
							this.DailyRefreshTime = input.ReadUInt64();
							continue;
						}
					}
					else
					{
						if (num == 48U)
						{
							this.WeeklyRefreshTime = input.ReadUInt64();
							continue;
						}
						if (num == 58U)
						{
							if (this.guildBossInfo_ == null)
							{
								this.guildBossInfo_ = new GuildBossInfoDto();
							}
							input.ReadMessage(this.guildBossInfo_);
							continue;
						}
					}
				}
				else if (num <= 90U)
				{
					if (num <= 72U)
					{
						if (num == 64U)
						{
							this.TaskRefreshCount = input.ReadUInt32();
							continue;
						}
						if (num == 72U)
						{
							this.MaxTaskRefreshCount = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 80U)
						{
							this.TaskRefreshCost = input.ReadUInt32();
							continue;
						}
						if (num == 90U)
						{
							this.signInRecords_.AddEntriesFrom(input, GuildFeaturesDto._repeated_signInRecords_codec);
							continue;
						}
					}
				}
				else if (num <= 104U)
				{
					if (num == 98U)
					{
						if (this.donationDto_ == null)
						{
							this.donationDto_ = new GuildDonationDto();
						}
						input.ReadMessage(this.donationDto_);
						continue;
					}
					if (num == 104U)
					{
						this.ApplyCount = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 112U)
					{
						this.DayContributeTimes = input.ReadUInt32();
						continue;
					}
					if (num == 120U)
					{
						this.DayALLContributeTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildFeaturesDto> _parser = new MessageParser<GuildFeaturesDto>(() => new GuildFeaturesDto());

		public const int TasksFieldNumber = 1;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(10U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();

		public const int DailyShopFieldNumber = 2;

		private static readonly FieldCodec<GuildShopDto> _repeated_dailyShop_codec = FieldCodec.ForMessage<GuildShopDto>(18U, GuildShopDto.Parser);

		private readonly RepeatedField<GuildShopDto> dailyShop_ = new RepeatedField<GuildShopDto>();

		public const int WeeklyShopFieldNumber = 3;

		private static readonly FieldCodec<GuildShopDto> _repeated_weeklyShop_codec = FieldCodec.ForMessage<GuildShopDto>(26U, GuildShopDto.Parser);

		private readonly RepeatedField<GuildShopDto> weeklyShop_ = new RepeatedField<GuildShopDto>();

		public const int SignInDtoFieldNumber = 4;

		private GuilSignInDto signInDto_;

		public const int DailyRefreshTimeFieldNumber = 5;

		private ulong dailyRefreshTime_;

		public const int WeeklyRefreshTimeFieldNumber = 6;

		private ulong weeklyRefreshTime_;

		public const int GuildBossInfoFieldNumber = 7;

		private GuildBossInfoDto guildBossInfo_;

		public const int TaskRefreshCountFieldNumber = 8;

		private uint taskRefreshCount_;

		public const int MaxTaskRefreshCountFieldNumber = 9;

		private uint maxTaskRefreshCount_;

		public const int TaskRefreshCostFieldNumber = 10;

		private uint taskRefreshCost_;

		public const int SignInRecordsFieldNumber = 11;

		private static readonly FieldCodec<string> _repeated_signInRecords_codec = FieldCodec.ForString(90U);

		private readonly RepeatedField<string> signInRecords_ = new RepeatedField<string>();

		public const int DonationDtoFieldNumber = 12;

		private GuildDonationDto donationDto_;

		public const int ApplyCountFieldNumber = 13;

		private uint applyCount_;

		public const int DayContributeTimesFieldNumber = 14;

		private uint dayContributeTimes_;

		public const int DayALLContributeTimesFieldNumber = 15;

		private int dayALLContributeTimes_;
	}
}
