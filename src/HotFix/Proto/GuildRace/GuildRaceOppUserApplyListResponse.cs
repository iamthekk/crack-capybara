using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceOppUserApplyListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceOppUserApplyListResponse> Parser
		{
			get
			{
				return GuildRaceOppUserApplyListResponse._parser;
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
		public RepeatedField<RaceUserDto> List
		{
			get
			{
				return this.list_;
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
			this.list_.WriteTo(output, GuildRaceOppUserApplyListResponse._repeated_list_codec);
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
			return num + this.list_.CalculateSize(GuildRaceOppUserApplyListResponse._repeated_list_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.list_.AddEntriesFrom(input, GuildRaceOppUserApplyListResponse._repeated_list_codec);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildRaceOppUserApplyListResponse> _parser = new MessageParser<GuildRaceOppUserApplyListResponse>(() => new GuildRaceOppUserApplyListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ListFieldNumber = 3;

		private static readonly FieldCodec<RaceUserDto> _repeated_list_codec = FieldCodec.ForMessage<RaceUserDto>(26U, RaceUserDto.Parser);

		private readonly RepeatedField<RaceUserDto> list_ = new RepeatedField<RaceUserDto>();
	}
}
