using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Mail
{
	public sealed class MailDeleteResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MailDeleteResponse> Parser
		{
			get
			{
				return MailDeleteResponse._parser;
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
		public RepeatedField<ulong> RowIds
		{
			get
			{
				return this.rowIds_;
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
			this.rowIds_.WriteTo(output, MailDeleteResponse._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.rowIds_.CalculateSize(MailDeleteResponse._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.rowIds_.AddEntriesFrom(input, MailDeleteResponse._repeated_rowIds_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MailDeleteResponse> _parser = new MessageParser<MailDeleteResponse>(() => new MailDeleteResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RowIdsFieldNumber = 2;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(18U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();
	}
}
