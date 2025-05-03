using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mail
{
	public sealed class MailReceiveAwardsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MailReceiveAwardsResponse> Parser
		{
			get
			{
				return MailReceiveAwardsResponse._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			this.rowIds_.WriteTo(output, MailReceiveAwardsResponse._repeated_rowIds_codec);
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			num += this.rowIds_.CalculateSize(MailReceiveAwardsResponse._repeated_rowIds_codec);
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						goto IL_0032;
					}
				}
				else
				{
					if (num == 18U)
					{
						goto IL_0032;
					}
					if (num == 26U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_0032:
				this.rowIds_.AddEntriesFrom(input, MailReceiveAwardsResponse._repeated_rowIds_codec);
			}
		}

		private static readonly MessageParser<MailReceiveAwardsResponse> _parser = new MessageParser<MailReceiveAwardsResponse>(() => new MailReceiveAwardsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RowIdsFieldNumber = 2;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(18U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
