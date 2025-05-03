using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildLevelUpResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildLevelUpResponse> Parser
		{
			get
			{
				return GuildLevelUpResponse._parser;
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
		public GuildUpdateInfoDto GuildUpdateInfo
		{
			get
			{
				return this.guildUpdateInfo_;
			}
			set
			{
				this.guildUpdateInfo_ = value;
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
			if (this.guildUpdateInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.GuildUpdateInfo);
			}
			this.tasks_.WriteTo(output, GuildLevelUpResponse._repeated_tasks_codec);
			this.dailyShop_.WriteTo(output, GuildLevelUpResponse._repeated_dailyShop_codec);
			this.weeklyShop_.WriteTo(output, GuildLevelUpResponse._repeated_weeklyShop_codec);
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
			if (this.guildUpdateInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildUpdateInfo);
			}
			num += this.tasks_.CalculateSize(GuildLevelUpResponse._repeated_tasks_codec);
			num += this.dailyShop_.CalculateSize(GuildLevelUpResponse._repeated_dailyShop_codec);
			return num + this.weeklyShop_.CalculateSize(GuildLevelUpResponse._repeated_weeklyShop_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
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
					if (num == 26U)
					{
						if (this.guildUpdateInfo_ == null)
						{
							this.guildUpdateInfo_ = new GuildUpdateInfoDto();
						}
						input.ReadMessage(this.guildUpdateInfo_);
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.tasks_.AddEntriesFrom(input, GuildLevelUpResponse._repeated_tasks_codec);
						continue;
					}
					if (num == 42U)
					{
						this.dailyShop_.AddEntriesFrom(input, GuildLevelUpResponse._repeated_dailyShop_codec);
						continue;
					}
					if (num == 50U)
					{
						this.weeklyShop_.AddEntriesFrom(input, GuildLevelUpResponse._repeated_weeklyShop_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildLevelUpResponse> _parser = new MessageParser<GuildLevelUpResponse>(() => new GuildLevelUpResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GuildUpdateInfoFieldNumber = 3;

		private GuildUpdateInfoDto guildUpdateInfo_;

		public const int TasksFieldNumber = 4;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(34U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();

		public const int DailyShopFieldNumber = 5;

		private static readonly FieldCodec<GuildShopDto> _repeated_dailyShop_codec = FieldCodec.ForMessage<GuildShopDto>(42U, GuildShopDto.Parser);

		private readonly RepeatedField<GuildShopDto> dailyShop_ = new RepeatedField<GuildShopDto>();

		public const int WeeklyShopFieldNumber = 6;

		private static readonly FieldCodec<GuildShopDto> _repeated_weeklyShop_codec = FieldCodec.ForMessage<GuildShopDto>(50U, GuildShopDto.Parser);

		private readonly RepeatedField<GuildShopDto> weeklyShop_ = new RepeatedField<GuildShopDto>();
	}
}
