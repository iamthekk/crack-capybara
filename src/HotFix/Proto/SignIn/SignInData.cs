using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SignIn
{
	public sealed class SignInData : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SignInData> Parser
		{
			get
			{
				return SignInData._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsCanSignIn
		{
			get
			{
				return this.isCanSignIn_;
			}
			set
			{
				this.isCanSignIn_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Timestamp
		{
			get
			{
				return this.timestamp_;
			}
			set
			{
				this.timestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Log
		{
			get
			{
				return this.log_;
			}
			set
			{
				this.log_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDtoListDto> RewardDtoList
		{
			get
			{
				return this.rewardDtoList_;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsCanSignIn)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsCanSignIn);
			}
			if (this.Timestamp != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.Timestamp);
			}
			if (this.Log != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Log);
			}
			this.rewardDtoList_.WriteTo(output, SignInData._repeated_rewardDtoList_codec);
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ConfigId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsCanSignIn)
			{
				num += 2;
			}
			if (this.Timestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Timestamp);
			}
			if (this.Log != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Log);
			}
			num += this.rewardDtoList_.CalculateSize(SignInData._repeated_rewardDtoList_codec);
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
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
						this.IsCanSignIn = input.ReadBool();
						continue;
					}
					if (num == 16U)
					{
						this.Timestamp = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Log = input.ReadUInt32();
						continue;
					}
					if (num == 34U)
					{
						this.rewardDtoList_.AddEntriesFrom(input, SignInData._repeated_rewardDtoList_codec);
						continue;
					}
					if (num == 40U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<SignInData> _parser = new MessageParser<SignInData>(() => new SignInData());

		public const int IsCanSignInFieldNumber = 1;

		private bool isCanSignIn_;

		public const int TimestampFieldNumber = 2;

		private ulong timestamp_;

		public const int LogFieldNumber = 3;

		private uint log_;

		public const int RewardDtoListFieldNumber = 4;

		private static readonly FieldCodec<RewardDtoListDto> _repeated_rewardDtoList_codec = FieldCodec.ForMessage<RewardDtoListDto>(34U, RewardDtoListDto.Parser);

		private readonly RepeatedField<RewardDtoListDto> rewardDtoList_ = new RepeatedField<RewardDtoListDto>();

		public const int ConfigIdFieldNumber = 5;

		private uint configId_;
	}
}
