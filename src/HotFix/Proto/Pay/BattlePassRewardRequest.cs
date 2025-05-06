using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class BattlePassRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BattlePassRewardRequest> Parser
		{
			get
			{
				return BattlePassRewardRequest._parser;
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
		public RepeatedField<uint> BattlePassRewardIdList
		{
			get
			{
				return this.battlePassRewardIdList_;
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
			this.battlePassRewardIdList_.WriteTo(output, BattlePassRewardRequest._repeated_battlePassRewardIdList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.battlePassRewardIdList_.CalculateSize(BattlePassRewardRequest._repeated_battlePassRewardIdList_codec);
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
						this.battlePassRewardIdList_.AddEntriesFrom(input, BattlePassRewardRequest._repeated_battlePassRewardIdList_codec);
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

		private static readonly MessageParser<BattlePassRewardRequest> _parser = new MessageParser<BattlePassRewardRequest>(() => new BattlePassRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int BattlePassRewardIdListFieldNumber = 2;

		private static readonly FieldCodec<uint> _repeated_battlePassRewardIdList_codec = FieldCodec.ForUInt32(18U);

		private readonly RepeatedField<uint> battlePassRewardIdList_ = new RepeatedField<uint>();
	}
}
