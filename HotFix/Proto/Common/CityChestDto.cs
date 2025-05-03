using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CityChestDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityChestDto> Parser
		{
			get
			{
				return CityChestDto._parser;
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
			if (this.Quality != 0U)
			{
				output.WriteRawTag(16);
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
				if (num != 8U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Quality = input.ReadUInt32();
					}
				}
				else
				{
					this.RowId = input.ReadUInt64();
				}
			}
		}

		private static readonly MessageParser<CityChestDto> _parser = new MessageParser<CityChestDto>(() => new CityChestDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int QualityFieldNumber = 2;

		private uint quality_;
	}
}
