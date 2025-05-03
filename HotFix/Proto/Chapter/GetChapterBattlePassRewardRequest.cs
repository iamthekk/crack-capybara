using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class GetChapterBattlePassRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GetChapterBattlePassRewardRequest> Parser
		{
			get
			{
				return GetChapterBattlePassRewardRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> RewardIdList
		{
			get
			{
				return this.rewardIdList_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			this.rewardIdList_.WriteTo(output, GetChapterBattlePassRewardRequest._repeated_rewardIdList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.rewardIdList_.CalculateSize(GetChapterBattlePassRewardRequest._repeated_rewardIdList_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.rewardIdList_.AddEntriesFrom(input, GetChapterBattlePassRewardRequest._repeated_rewardIdList_codec);
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<GetChapterBattlePassRewardRequest> _parser = new MessageParser<GetChapterBattlePassRewardRequest>(() => new GetChapterBattlePassRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RewardIdListFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_rewardIdList_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> rewardIdList_ = new RepeatedField<int>();
	}
}
