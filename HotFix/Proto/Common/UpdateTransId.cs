using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UpdateTransId : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateTransId> Parser
		{
			get
			{
				return UpdateTransId._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsChange
		{
			get
			{
				return this.isChange_;
			}
			set
			{
				this.isChange_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TransId
		{
			get
			{
				return this.transId_;
			}
			set
			{
				this.transId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsChange)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsChange);
			}
			if (this.TransId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.TransId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsChange)
			{
				num += 2;
			}
			if (this.TransId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TransId);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.TransId = input.ReadUInt64();
					}
				}
				else
				{
					this.IsChange = input.ReadBool();
				}
			}
		}

		private static readonly MessageParser<UpdateTransId> _parser = new MessageParser<UpdateTransId>(() => new UpdateTransId());

		public const int IsChangeFieldNumber = 1;

		private bool isChange_;

		public const int TransIdFieldNumber = 2;

		private ulong transId_;
	}
}
