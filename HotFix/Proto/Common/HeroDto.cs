using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class HeroDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HeroDto> Parser
		{
			get
			{
				return HeroDto._parser;
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
		public uint HeroId
		{
			get
			{
				return this.heroId_;
			}
			set
			{
				this.heroId_ = value;
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
		public uint Quality
		{
			get
			{
				return this.quality_;
			}
			set
			{
				this.quality_ = value;
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
			if (this.HeroId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.HeroId);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Exp);
			}
			if (this.Star != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Star);
			}
			if (this.Quality != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Quality);
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
			if (this.HeroId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.HeroId);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.Star != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Star);
			}
			if (this.Quality != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Quality);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.RowId = input.ReadUInt64();
						continue;
					}
					if (num == 16U)
					{
						this.HeroId = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.Star = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.Quality = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HeroDto> _parser = new MessageParser<HeroDto>(() => new HeroDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int HeroIdFieldNumber = 2;

		private uint heroId_;

		public const int LevelFieldNumber = 3;

		private uint level_;

		public const int ExpFieldNumber = 4;

		private uint exp_;

		public const int StarFieldNumber = 5;

		private uint star_;

		public const int QualityFieldNumber = 6;

		private uint quality_;
	}
}
