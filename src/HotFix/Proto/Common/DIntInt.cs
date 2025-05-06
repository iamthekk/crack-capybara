using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class DIntInt : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DIntInt> Parser
		{
			get
			{
				return DIntInt._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Key
		{
			get
			{
				return this.key_;
			}
			set
			{
				this.key_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Val
		{
			get
			{
				return this.val_;
			}
			set
			{
				this.val_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Key != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Key);
			}
			if (this.Val != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Val);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Key != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Key);
			}
			if (this.Val != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Val);
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
						this.Val = input.ReadInt32();
					}
				}
				else
				{
					this.Key = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<DIntInt> _parser = new MessageParser<DIntInt>(() => new DIntInt());

		public const int KeyFieldNumber = 1;

		private int key_;

		public const int ValFieldNumber = 2;

		private int val_;
	}
}
