using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class MissionGetHangUpItemsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MissionGetHangUpItemsResponse> Parser
		{
			get
			{
				return MissionGetHangUpItemsResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Reward
		{
			get
			{
				return this.reward_;
			}
		}

		[DebuggerNonUserCode]
		public UserMission UserMission
		{
			get
			{
				return this.userMission_;
			}
			set
			{
				this.userMission_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			this.reward_.WriteTo(output, MissionGetHangUpItemsResponse._repeated_reward_codec);
			if (this.userMission_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserMission);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			num += this.reward_.CalculateSize(MissionGetHangUpItemsResponse._repeated_reward_codec);
			if (this.userMission_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserMission);
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
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.userMission_ == null)
							{
								this.userMission_ = new UserMission();
							}
							input.ReadMessage(this.userMission_);
						}
					}
					else
					{
						this.reward_.AddEntriesFrom(input, MissionGetHangUpItemsResponse._repeated_reward_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MissionGetHangUpItemsResponse> _parser = new MessageParser<MissionGetHangUpItemsResponse>(() => new MissionGetHangUpItemsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RewardFieldNumber = 2;

		private static readonly FieldCodec<RewardDto> _repeated_reward_codec = FieldCodec.ForMessage<RewardDto>(18U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> reward_ = new RepeatedField<RewardDto>();

		public const int UserMissionFieldNumber = 3;

		private UserMission userMission_;
	}
}
