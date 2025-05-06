using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Mail
{
	public sealed class MailGetListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MailGetListResponse> Parser
		{
			get
			{
				return MailGetListResponse._parser;
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
		public RepeatedField<MailDto> Mails
		{
			get
			{
				return this.mails_;
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
			this.mails_.WriteTo(output, MailGetListResponse._repeated_mails_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.mails_.CalculateSize(MailGetListResponse._repeated_mails_codec);
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
						this.mails_.AddEntriesFrom(input, MailGetListResponse._repeated_mails_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MailGetListResponse> _parser = new MessageParser<MailGetListResponse>(() => new MailGetListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int MailsFieldNumber = 2;

		private static readonly FieldCodec<MailDto> _repeated_mails_codec = FieldCodec.ForMessage<MailDto>(18U, MailDto.Parser);

		private readonly RepeatedField<MailDto> mails_ = new RepeatedField<MailDto>();
	}
}
