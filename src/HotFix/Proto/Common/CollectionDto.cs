using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CollectionDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CollectionDto> Parser
		{
			get
			{
				return CollectionDto._parser;
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
		public uint CollecType
		{
			get
			{
				return this.collecType_;
			}
			set
			{
				this.collecType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CollecCount
		{
			get
			{
				return this.collecCount_;
			}
			set
			{
				this.collecCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CollecLv
		{
			get
			{
				return this.collecLv_;
			}
			set
			{
				this.collecLv_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CollecStar
		{
			get
			{
				return this.collecStar_;
			}
			set
			{
				this.collecStar_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong CreateTime
		{
			get
			{
				return this.createTime_;
			}
			set
			{
				this.createTime_ = value;
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
			if (this.CollecType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.CollecType);
			}
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.CollecCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.CollecCount);
			}
			if (this.CollecLv != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.CollecLv);
			}
			if (this.CollecStar != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.CollecStar);
			}
			if (this.CreateTime != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.CreateTime);
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
			if (this.CollecType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CollecType);
			}
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.CollecCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CollecCount);
			}
			if (this.CollecLv != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CollecLv);
			}
			if (this.CollecStar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CollecStar);
			}
			if (this.CreateTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.CreateTime);
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
						this.CollecType = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.CollecCount = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.CollecLv = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.CollecStar = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.CreateTime = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CollectionDto> _parser = new MessageParser<CollectionDto>(() => new CollectionDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int CollecTypeFieldNumber = 2;

		private uint collecType_;

		public const int ConfigIdFieldNumber = 3;

		private uint configId_;

		public const int CollecCountFieldNumber = 4;

		private uint collecCount_;

		public const int CollecLvFieldNumber = 5;

		private uint collecLv_;

		public const int CollecStarFieldNumber = 6;

		private uint collecStar_;

		public const int CreateTimeFieldNumber = 7;

		private ulong createTime_;
	}
}
