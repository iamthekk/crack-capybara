using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class MissionCompleteDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MissionCompleteDto> Parser
		{
			get
			{
				return MissionCompleteDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint BattleType
		{
			get
			{
				return this.battleType_;
			}
			set
			{
				this.battleType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong BattleStartTransId
		{
			get
			{
				return this.battleStartTransId_;
			}
			set
			{
				this.battleStartTransId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsWin
		{
			get
			{
				return this.isWin_;
			}
			set
			{
				this.isWin_ = value;
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
		public string Extar
		{
			get
			{
				return this.extar_;
			}
			set
			{
				this.extar_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong Damage
		{
			get
			{
				return this.damage_;
			}
			set
			{
				this.damage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.BattleType != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.BattleType);
			}
			if (this.ChapterId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ChapterId);
			}
			if (this.BattleStartTransId != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.BattleStartTransId);
			}
			if (this.IsWin)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.IsWin);
			}
			this.rewards_.WriteTo(output, MissionCompleteDto._repeated_rewards_codec);
			if (this.Extar.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.Extar);
			}
			if (this.Damage != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.Damage);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.BattleType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BattleType);
			}
			if (this.ChapterId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChapterId);
			}
			if (this.BattleStartTransId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.BattleStartTransId);
			}
			if (this.IsWin)
			{
				num += 2;
			}
			num += this.rewards_.CalculateSize(MissionCompleteDto._repeated_rewards_codec);
			if (this.Extar.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Extar);
			}
			if (this.Damage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Damage);
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
						this.BattleType = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.ChapterId = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.BattleStartTransId = input.ReadUInt64();
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 32U)
					{
						this.IsWin = input.ReadBool();
						continue;
					}
					if (num == 42U)
					{
						this.rewards_.AddEntriesFrom(input, MissionCompleteDto._repeated_rewards_codec);
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.Extar = input.ReadString();
						continue;
					}
					if (num == 56U)
					{
						this.Damage = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MissionCompleteDto> _parser = new MessageParser<MissionCompleteDto>(() => new MissionCompleteDto());

		public const int BattleTypeFieldNumber = 1;

		private uint battleType_;

		public const int ChapterIdFieldNumber = 2;

		private uint chapterId_;

		public const int BattleStartTransIdFieldNumber = 3;

		private ulong battleStartTransId_;

		public const int IsWinFieldNumber = 4;

		private bool isWin_;

		public const int RewardsFieldNumber = 5;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(42U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();

		public const int ExtarFieldNumber = 6;

		private string extar_ = "";

		public const int DamageFieldNumber = 7;

		private ulong damage_;
	}
}
