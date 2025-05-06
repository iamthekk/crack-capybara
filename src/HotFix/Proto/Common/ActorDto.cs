using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ActorDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActorDto> Parser
		{
			get
			{
				return ActorDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RowId);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Level);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
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
						this.Level = input.ReadUInt32();
					}
				}
				else
				{
					this.RowId = input.ReadUInt64();
				}
			}
		}

		private static readonly MessageParser<ActorDto> _parser = new MessageParser<ActorDto>(() => new ActorDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int LevelFieldNumber = 2;

		private uint level_;
	}
}
