using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuilSignInDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuilSignInDto> Parser
		{
			get
			{
				return GuilSignInDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Limit
		{
			get
			{
				return this.limit_;
			}
			set
			{
				this.limit_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint NeedItemId
		{
			get
			{
				return this.needItemId_;
			}
			set
			{
				this.needItemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint NeedItemCount
		{
			get
			{
				return this.needItemCount_;
			}
			set
			{
				this.needItemCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Diamonds
		{
			get
			{
				return this.diamonds_;
			}
			set
			{
				this.diamonds_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Count != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Count);
			}
			if (this.Limit != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Limit);
			}
			if (this.NeedItemId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.NeedItemId);
			}
			if (this.NeedItemCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.NeedItemCount);
			}
			if (this.Diamonds != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Diamonds);
			}
			this.rewards_.WriteTo(output, GuilSignInDto._repeated_rewards_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			if (this.Limit != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Limit);
			}
			if (this.NeedItemId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.NeedItemId);
			}
			if (this.NeedItemCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.NeedItemCount);
			}
			if (this.Diamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Diamonds);
			}
			return num + this.rewards_.CalculateSize(GuilSignInDto._repeated_rewards_codec);
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
						this.Count = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Limit = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.NeedItemId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.NeedItemCount = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.Diamonds = input.ReadUInt32();
						continue;
					}
					if (num == 50U)
					{
						this.rewards_.AddEntriesFrom(input, GuilSignInDto._repeated_rewards_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuilSignInDto> _parser = new MessageParser<GuilSignInDto>(() => new GuilSignInDto());

		public const int CountFieldNumber = 1;

		private uint count_;

		public const int LimitFieldNumber = 2;

		private uint limit_;

		public const int NeedItemIdFieldNumber = 3;

		private uint needItemId_;

		public const int NeedItemCountFieldNumber = 4;

		private uint needItemCount_;

		public const int DiamondsFieldNumber = 5;

		private uint diamonds_;

		public const int RewardsFieldNumber = 6;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(50U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();
	}
}
