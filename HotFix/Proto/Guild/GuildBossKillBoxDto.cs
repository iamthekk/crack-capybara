using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossKillBoxDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossKillBoxDto> Parser
		{
			get
			{
				return GuildBossKillBoxDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint BoxId
		{
			get
			{
				return this.boxId_;
			}
			set
			{
				this.boxId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Need
		{
			get
			{
				return this.need_;
			}
			set
			{
				this.need_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Progress
		{
			get
			{
				return this.progress_;
			}
			set
			{
				this.progress_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Rewards
		{
			get
			{
				return this.rewards_;
			}
		}

		[DebuggerNonUserCode]
		public bool IsFinish
		{
			get
			{
				return this.isFinish_;
			}
			set
			{
				this.isFinish_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsReceive
		{
			get
			{
				return this.isReceive_;
			}
			set
			{
				this.isReceive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.BoxId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.BoxId);
			}
			if (this.Need != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Need);
			}
			if (this.Progress != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Progress);
			}
			this.rewards_.WriteTo(output, GuildBossKillBoxDto._repeated_rewards_codec);
			if (this.IsFinish)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.IsFinish);
			}
			if (this.IsReceive)
			{
				output.WriteRawTag(48);
				output.WriteBool(this.IsReceive);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.BoxId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BoxId);
			}
			if (this.Need != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Need);
			}
			if (this.Progress != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Progress);
			}
			num += this.rewards_.CalculateSize(GuildBossKillBoxDto._repeated_rewards_codec);
			if (this.IsFinish)
			{
				num += 2;
			}
			if (this.IsReceive)
			{
				num += 2;
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
						this.BoxId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Need = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Progress = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.rewards_.AddEntriesFrom(input, GuildBossKillBoxDto._repeated_rewards_codec);
						continue;
					}
					if (num == 40U)
					{
						this.IsFinish = input.ReadBool();
						continue;
					}
					if (num == 48U)
					{
						this.IsReceive = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossKillBoxDto> _parser = new MessageParser<GuildBossKillBoxDto>(() => new GuildBossKillBoxDto());

		public const int BoxIdFieldNumber = 1;

		private uint boxId_;

		public const int NeedFieldNumber = 2;

		private uint need_;

		public const int ProgressFieldNumber = 3;

		private uint progress_;

		public const int RewardsFieldNumber = 4;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(34U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();

		public const int IsFinishFieldNumber = 5;

		private bool isFinish_;

		public const int IsReceiveFieldNumber = 6;

		private bool isReceive_;
	}
}
