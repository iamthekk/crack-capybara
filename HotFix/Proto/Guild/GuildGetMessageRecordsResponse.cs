using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildGetMessageRecordsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildGetMessageRecordsResponse> Parser
		{
			get
			{
				return GuildGetMessageRecordsResponse._parser;
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
		public RepeatedField<GuildPushMessageDto> MessageRecords
		{
			get
			{
				return this.messageRecords_;
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
			this.messageRecords_.WriteTo(output, GuildGetMessageRecordsResponse._repeated_messageRecords_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.messageRecords_.CalculateSize(GuildGetMessageRecordsResponse._repeated_messageRecords_codec);
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
						this.messageRecords_.AddEntriesFrom(input, GuildGetMessageRecordsResponse._repeated_messageRecords_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildGetMessageRecordsResponse> _parser = new MessageParser<GuildGetMessageRecordsResponse>(() => new GuildGetMessageRecordsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int MessageRecordsFieldNumber = 2;

		private static readonly FieldCodec<GuildPushMessageDto> _repeated_messageRecords_codec = FieldCodec.ForMessage<GuildPushMessageDto>(18U, GuildPushMessageDto.Parser);

		private readonly RepeatedField<GuildPushMessageDto> messageRecords_ = new RepeatedField<GuildPushMessageDto>();
	}
}
