using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class UserVipLevel : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserVipLevel> Parser
		{
			get
			{
				return UserVipLevel._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint VipLevel
		{
			get
			{
				return this.vipLevel_;
			}
			set
			{
				this.vipLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint VipExp
		{
			get
			{
				return this.vipExp_;
			}
			set
			{
				this.vipExp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> RewardId
		{
			get
			{
				return this.rewardId_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.VipLevel != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.VipLevel);
			}
			if (this.VipExp != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.VipExp);
			}
			this.rewardId_.WriteTo(output, UserVipLevel._repeated_rewardId_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.VipLevel != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.VipLevel);
			}
			if (this.VipExp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.VipExp);
			}
			return num + this.rewardId_.CalculateSize(UserVipLevel._repeated_rewardId_codec);
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
						this.VipLevel = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.VipExp = input.ReadUInt32();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.rewardId_.AddEntriesFrom(input, UserVipLevel._repeated_rewardId_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserVipLevel> _parser = new MessageParser<UserVipLevel>(() => new UserVipLevel());

		public const int VipLevelFieldNumber = 1;

		private uint vipLevel_;

		public const int VipExpFieldNumber = 2;

		private uint vipExp_;

		public const int RewardIdFieldNumber = 3;

		private static readonly FieldCodec<uint> _repeated_rewardId_codec = FieldCodec.ForUInt32(26U);

		private readonly RepeatedField<uint> rewardId_ = new RepeatedField<uint>();
	}
}
