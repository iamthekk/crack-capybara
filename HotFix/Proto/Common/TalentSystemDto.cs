using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class TalentSystemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentSystemDto> Parser
		{
			get
			{
				return TalentSystemDto._parser;
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
			if (this.Id != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Id);
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
			if (this.Id != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Id);
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
					this.Id = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<TalentSystemDto> _parser = new MessageParser<TalentSystemDto>(() => new TalentSystemDto());

		public const int IdFieldNumber = 1;

		private uint id_;

		public const int LevelFieldNumber = 2;

		private uint level_;
	}
}
