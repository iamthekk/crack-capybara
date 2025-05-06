using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class RewardDtoListDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RewardDtoListDto> Parser
		{
			get
			{
				return RewardDtoListDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> RewardDtos
		{
			get
			{
				return this.rewardDtos_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.rewardDtos_.WriteTo(output, RewardDtoListDto._repeated_rewardDtos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.rewardDtos_.CalculateSize(RewardDtoListDto._repeated_rewardDtos_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					this.rewardDtos_.AddEntriesFrom(input, RewardDtoListDto._repeated_rewardDtos_codec);
				}
			}
		}

		private static readonly MessageParser<RewardDtoListDto> _parser = new MessageParser<RewardDtoListDto>(() => new RewardDtoListDto());

		public const int RewardDtosFieldNumber = 1;

		private static readonly FieldCodec<RewardDto> _repeated_rewardDtos_codec = FieldCodec.ForMessage<RewardDto>(10U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewardDtos_ = new RepeatedField<RewardDto>();
	}
}
