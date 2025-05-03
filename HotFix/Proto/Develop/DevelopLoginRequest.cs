using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Develop
{
	public sealed class DevelopLoginRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DevelopLoginRequest> Parser
		{
			get
			{
				return DevelopLoginRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.UserId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.UserId);
			}
			if (this.Type != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Type);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
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
						this.Type = input.ReadUInt32();
					}
				}
				else
				{
					this.UserId = input.ReadInt64();
				}
			}
		}

		private static readonly MessageParser<DevelopLoginRequest> _parser = new MessageParser<DevelopLoginRequest>(() => new DevelopLoginRequest());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int TypeFieldNumber = 2;

		private uint type_;
	}
}
