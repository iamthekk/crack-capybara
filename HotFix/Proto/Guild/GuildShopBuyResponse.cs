using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildShopBuyResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildShopBuyResponse> Parser
		{
			get
			{
				return GuildShopBuyResponse._parser;
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
		public GuildShopDto GuildShopDto
		{
			get
			{
				return this.guildShopDto_;
			}
			set
			{
				this.guildShopDto_ = value;
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
			if (this.guildShopDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.GuildShopDto);
			}
			this.tasks_.WriteTo(output, GuildShopBuyResponse._repeated_tasks_codec);
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
			if (this.guildShopDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildShopDto);
			}
			return num + this.tasks_.CalculateSize(GuildShopBuyResponse._repeated_tasks_codec);
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
						if (this.guildShopDto_ == null)
						{
							this.guildShopDto_ = new GuildShopDto();
						}
						input.ReadMessage(this.guildShopDto_);
						continue;
					}
					if (num == 34U)
					{
						this.tasks_.AddEntriesFrom(input, GuildShopBuyResponse._repeated_tasks_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildShopBuyResponse> _parser = new MessageParser<GuildShopBuyResponse>(() => new GuildShopBuyResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GuildShopDtoFieldNumber = 3;

		private GuildShopDto guildShopDto_;

		public const int TasksFieldNumber = 4;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(34U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();
	}
}
