using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserLevel : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserLevel> Parser
		{
			get
			{
				return UserLevel._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Level != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Exp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
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
						this.Exp = input.ReadUInt32();
					}
				}
				else
				{
					this.Level = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<UserLevel> _parser = new MessageParser<UserLevel>(() => new UserLevel());

		public const int LevelFieldNumber = 1;

		private uint level_;

		public const int ExpFieldNumber = 2;

		private uint exp_;
	}
}
