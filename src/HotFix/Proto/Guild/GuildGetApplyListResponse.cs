using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildGetApplyListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildGetApplyListResponse> Parser
		{
			get
			{
				return GuildGetApplyListResponse._parser;
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
		public RepeatedField<GuildMemberInfoDto> ApplyList
		{
			get
			{
				return this.applyList_;
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
			this.applyList_.WriteTo(output, GuildGetApplyListResponse._repeated_applyList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.applyList_.CalculateSize(GuildGetApplyListResponse._repeated_applyList_codec);
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
						input.SkipLastField();
					}
					else
					{
						this.applyList_.AddEntriesFrom(input, GuildGetApplyListResponse._repeated_applyList_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildGetApplyListResponse> _parser = new MessageParser<GuildGetApplyListResponse>(() => new GuildGetApplyListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ApplyListFieldNumber = 2;

		private static readonly FieldCodec<GuildMemberInfoDto> _repeated_applyList_codec = FieldCodec.ForMessage<GuildMemberInfoDto>(18U, GuildMemberInfoDto.Parser);

		private readonly RepeatedField<GuildMemberInfoDto> applyList_ = new RepeatedField<GuildMemberInfoDto>();
	}
}
