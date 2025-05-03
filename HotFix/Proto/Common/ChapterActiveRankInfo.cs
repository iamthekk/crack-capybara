using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ChapterActiveRankInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActiveRankInfo> Parser
		{
			get
			{
				return ChapterActiveRankInfo._parser;
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
		public uint ActiveType
		{
			get
			{
				return this.activeType_;
			}
			set
			{
				this.activeType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ActiveId
		{
			get
			{
				return this.activeId_;
			}
			set
			{
				this.activeId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = value;
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
			if (this.ActiveType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ActiveType);
			}
			if (this.ActiveId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ActiveId);
			}
			if (this.Rank != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Rank);
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
			if (this.ActiveType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveType);
			}
			if (this.ActiveId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveId);
			}
			if (this.Rank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rank);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.RowId = input.ReadUInt64();
						continue;
					}
					if (num == 16U)
					{
						this.ActiveType = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.ActiveId = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.Rank = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActiveRankInfo> _parser = new MessageParser<ChapterActiveRankInfo>(() => new ChapterActiveRankInfo());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int ActiveTypeFieldNumber = 2;

		private uint activeType_;

		public const int ActiveIdFieldNumber = 3;

		private uint activeId_;

		public const int RankFieldNumber = 4;

		private uint rank_;
	}
}
