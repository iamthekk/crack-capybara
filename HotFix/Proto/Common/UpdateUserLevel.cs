using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class UpdateUserLevel : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateUserLevel> Parser
		{
			get
			{
				return UpdateUserLevel._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsChange
		{
			get
			{
				return this.isChange_;
			}
			set
			{
				this.isChange_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserLevel UserLevel
		{
			get
			{
				return this.userLevel_;
			}
			set
			{
				this.userLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> LevelUpReward
		{
			get
			{
				return this.levelUpReward_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsChange)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsChange);
			}
			if (this.userLevel_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserLevel);
			}
			this.levelUpReward_.WriteTo(output, UpdateUserLevel._repeated_levelUpReward_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsChange)
			{
				num += 2;
			}
			if (this.userLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserLevel);
			}
			return num + this.levelUpReward_.CalculateSize(UpdateUserLevel._repeated_levelUpReward_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.levelUpReward_.AddEntriesFrom(input, UpdateUserLevel._repeated_levelUpReward_codec);
						}
					}
					else
					{
						if (this.userLevel_ == null)
						{
							this.userLevel_ = new UserLevel();
						}
						input.ReadMessage(this.userLevel_);
					}
				}
				else
				{
					this.IsChange = input.ReadBool();
				}
			}
		}

		private static readonly MessageParser<UpdateUserLevel> _parser = new MessageParser<UpdateUserLevel>(() => new UpdateUserLevel());

		public const int IsChangeFieldNumber = 1;

		private bool isChange_;

		public const int UserLevelFieldNumber = 2;

		private UserLevel userLevel_;

		public const int LevelUpRewardFieldNumber = 3;

		private static readonly FieldCodec<RewardDto> _repeated_levelUpReward_codec = FieldCodec.ForMessage<RewardDto>(26U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> levelUpReward_ = new RepeatedField<RewardDto>();
	}
}
