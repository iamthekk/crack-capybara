using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class RelicDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RelicDto> Parser
		{
			get
			{
				return RelicDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint RelicId
		{
			get
			{
				return this.relicId_;
			}
			set
			{
				this.relicId_ = value;
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
		public uint Star
		{
			get
			{
				return this.star_;
			}
			set
			{
				this.star_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RelicId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.RelicId);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Level);
			}
			if (this.Star != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Star);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RelicId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RelicId);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Star != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Star);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Star = input.ReadUInt32();
						}
					}
					else
					{
						this.Level = input.ReadUInt32();
					}
				}
				else
				{
					this.RelicId = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<RelicDto> _parser = new MessageParser<RelicDto>(() => new RelicDto());

		public const int RelicIdFieldNumber = 1;

		private uint relicId_;

		public const int LevelFieldNumber = 2;

		private uint level_;

		public const int StarFieldNumber = 3;

		private uint star_;
	}
}
