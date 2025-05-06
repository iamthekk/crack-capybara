using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class TurnTableTaskDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableTaskDto> Parser
		{
			get
			{
				return TurnTableTaskDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Process
		{
			get
			{
				return this.process_;
			}
			set
			{
				this.process_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Id);
			}
			if (this.Process != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Process);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Id);
			}
			if (this.Process != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Process);
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
						this.Process = input.ReadUInt32();
					}
				}
				else
				{
					this.Id = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<TurnTableTaskDto> _parser = new MessageParser<TurnTableTaskDto>(() => new TurnTableTaskDto());

		public const int IdFieldNumber = 1;

		private uint id_;

		public const int ProcessFieldNumber = 2;

		private uint process_;
	}
}
