using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class IapPushRemoveResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IapPushRemoveResponse> Parser
		{
			get
			{
				return IapPushRemoveResponse._parser;
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
		public PushIapDto PushIapDto
		{
			get
			{
				return this.pushIapDto_;
			}
			set
			{
				this.pushIapDto_ = value;
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
			if (this.pushIapDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.PushIapDto);
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
			if (this.pushIapDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PushIapDto);
			}
			return num;
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
						if (this.pushIapDto_ == null)
						{
							this.pushIapDto_ = new PushIapDto();
						}
						input.ReadMessage(this.pushIapDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<IapPushRemoveResponse> _parser = new MessageParser<IapPushRemoveResponse>(() => new IapPushRemoveResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PushIapDtoFieldNumber = 2;

		private PushIapDto pushIapDto_;
	}
}
