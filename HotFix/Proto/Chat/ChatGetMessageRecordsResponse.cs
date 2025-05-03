using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Chat
{
	public sealed class ChatGetMessageRecordsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatGetMessageRecordsResponse> Parser
		{
			get
			{
				return ChatGetMessageRecordsResponse._parser;
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
		public RepeatedField<PushMessageDto> MessageRecords
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
			this.messageRecords_.WriteTo(output, ChatGetMessageRecordsResponse._repeated_messageRecords_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.messageRecords_.CalculateSize(ChatGetMessageRecordsResponse._repeated_messageRecords_codec);
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
						this.messageRecords_.AddEntriesFrom(input, ChatGetMessageRecordsResponse._repeated_messageRecords_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ChatGetMessageRecordsResponse> _parser = new MessageParser<ChatGetMessageRecordsResponse>(() => new ChatGetMessageRecordsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int MessageRecordsFieldNumber = 2;

		private static readonly FieldCodec<PushMessageDto> _repeated_messageRecords_codec = FieldCodec.ForMessage<PushMessageDto>(18U, PushMessageDto.Parser);

		private readonly RepeatedField<PushMessageDto> messageRecords_ = new RepeatedField<PushMessageDto>();
	}
}
