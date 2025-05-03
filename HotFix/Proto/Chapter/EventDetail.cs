using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class EventDetail : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EventDetail> Parser
		{
			get
			{
				return EventDetail._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Seed
		{
			get
			{
				return this.seed_;
			}
			set
			{
				this.seed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint EventTypeId
		{
			get
			{
				return this.eventTypeId_;
			}
			set
			{
				this.eventTypeId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint EventId
		{
			get
			{
				return this.eventId_;
			}
			set
			{
				this.eventId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Rate
		{
			get
			{
				return this.rate_;
			}
			set
			{
				this.rate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> ActRowId
		{
			get
			{
				return this.actRowId_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Drops
		{
			get
			{
				return this.drops_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> MonsterDrops
		{
			get
			{
				return this.monsterDrops_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> BattleDrops
		{
			get
			{
				return this.battleDrops_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Seed != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Seed);
			}
			if (this.EventTypeId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.EventTypeId);
			}
			if (this.EventId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.EventId);
			}
			if (this.Rate != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Rate);
			}
			this.actRowId_.WriteTo(output, EventDetail._repeated_actRowId_codec);
			this.drops_.WriteTo(output, EventDetail._repeated_drops_codec);
			this.monsterDrops_.WriteTo(output, EventDetail._repeated_monsterDrops_codec);
			this.battleDrops_.WriteTo(output, EventDetail._repeated_battleDrops_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			if (this.EventTypeId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EventTypeId);
			}
			if (this.EventId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EventId);
			}
			if (this.Rate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rate);
			}
			num += this.actRowId_.CalculateSize(EventDetail._repeated_actRowId_codec);
			num += this.drops_.CalculateSize(EventDetail._repeated_drops_codec);
			num += this.monsterDrops_.CalculateSize(EventDetail._repeated_monsterDrops_codec);
			return num + this.battleDrops_.CalculateSize(EventDetail._repeated_battleDrops_codec);
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
							this.Seed = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.EventTypeId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.EventId = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Rate = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 42U)
				{
					if (num == 40U || num == 42U)
					{
						this.actRowId_.AddEntriesFrom(input, EventDetail._repeated_actRowId_codec);
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.drops_.AddEntriesFrom(input, EventDetail._repeated_drops_codec);
						continue;
					}
					if (num == 58U)
					{
						this.monsterDrops_.AddEntriesFrom(input, EventDetail._repeated_monsterDrops_codec);
						continue;
					}
					if (num == 66U)
					{
						this.battleDrops_.AddEntriesFrom(input, EventDetail._repeated_battleDrops_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EventDetail> _parser = new MessageParser<EventDetail>(() => new EventDetail());

		public const int SeedFieldNumber = 1;

		private int seed_;

		public const int EventTypeIdFieldNumber = 2;

		private uint eventTypeId_;

		public const int EventIdFieldNumber = 3;

		private uint eventId_;

		public const int RateFieldNumber = 4;

		private uint rate_;

		public const int ActRowIdFieldNumber = 5;

		private static readonly FieldCodec<ulong> _repeated_actRowId_codec = FieldCodec.ForUInt64(42U);

		private readonly RepeatedField<ulong> actRowId_ = new RepeatedField<ulong>();

		public const int DropsFieldNumber = 6;

		private static readonly FieldCodec<RewardDto> _repeated_drops_codec = FieldCodec.ForMessage<RewardDto>(50U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> drops_ = new RepeatedField<RewardDto>();

		public const int MonsterDropsFieldNumber = 7;

		private static readonly FieldCodec<RewardDto> _repeated_monsterDrops_codec = FieldCodec.ForMessage<RewardDto>(58U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> monsterDrops_ = new RepeatedField<RewardDto>();

		public const int BattleDropsFieldNumber = 8;

		private static readonly FieldCodec<RewardDto> _repeated_battleDrops_codec = FieldCodec.ForMessage<RewardDto>(66U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> battleDrops_ = new RepeatedField<RewardDto>();
	}
}
