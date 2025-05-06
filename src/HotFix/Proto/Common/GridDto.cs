using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class GridDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GridDto> Parser
		{
			get
			{
				return GridDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Pos
		{
			get
			{
				return this.pos_;
			}
			set
			{
				this.pos_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OreBuildId
		{
			get
			{
				return this.oreBuildId_;
			}
			set
			{
				this.oreBuildId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Floors
		{
			get
			{
				return this.floors_;
			}
			set
			{
				this.floors_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Grade
		{
			get
			{
				return this.grade_;
			}
			set
			{
				this.grade_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TreasurePos
		{
			get
			{
				return this.treasurePos_;
			}
			set
			{
				this.treasurePos_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Item
		{
			get
			{
				return this.item_;
			}
		}

		[DebuggerNonUserCode]
		public int BombStatus
		{
			get
			{
				return this.bombStatus_;
			}
			set
			{
				this.bombStatus_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Pos != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Pos);
			}
			if (this.Status != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Status);
			}
			if (this.OreBuildId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.OreBuildId);
			}
			if (this.Floors != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Floors);
			}
			if (this.Grade != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.Grade);
			}
			if (this.TreasurePos != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.TreasurePos);
			}
			this.item_.WriteTo(output, GridDto._repeated_item_codec);
			if (this.BombStatus != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.BombStatus);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Pos != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Pos);
			}
			if (this.Status != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Status);
			}
			if (this.OreBuildId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OreBuildId);
			}
			if (this.Floors != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Floors);
			}
			if (this.Grade != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Grade);
			}
			if (this.TreasurePos != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TreasurePos);
			}
			num += this.item_.CalculateSize(GridDto._repeated_item_codec);
			if (this.BombStatus != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BombStatus);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Pos = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.Status = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.OreBuildId = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Floors = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.Grade = input.ReadInt32();
						continue;
					}
					if (num == 48U)
					{
						this.TreasurePos = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.item_.AddEntriesFrom(input, GridDto._repeated_item_codec);
						continue;
					}
					if (num == 64U)
					{
						this.BombStatus = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GridDto> _parser = new MessageParser<GridDto>(() => new GridDto());

		public const int PosFieldNumber = 1;

		private int pos_;

		public const int StatusFieldNumber = 2;

		private int status_;

		public const int OreBuildIdFieldNumber = 3;

		private int oreBuildId_;

		public const int FloorsFieldNumber = 4;

		private int floors_;

		public const int GradeFieldNumber = 5;

		private int grade_;

		public const int TreasurePosFieldNumber = 6;

		private int treasurePos_;

		public const int ItemFieldNumber = 7;

		private static readonly FieldCodec<RewardDto> _repeated_item_codec = FieldCodec.ForMessage<RewardDto>(58U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> item_ = new RepeatedField<RewardDto>();

		public const int BombStatusFieldNumber = 8;

		private int bombStatus_;
	}
}
