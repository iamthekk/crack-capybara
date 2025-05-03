using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class LevelFundRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LevelFundRewardRequest> Parser
		{
			get
			{
				return LevelFundRewardRequest._parser;
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
		public RepeatedField<int> LevelFundRewardId
		{
			get
			{
				return this.levelFundRewardId_;
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
			this.levelFundRewardId_.WriteTo(output, LevelFundRewardRequest._repeated_levelFundRewardId_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.levelFundRewardId_.CalculateSize(LevelFundRewardRequest._repeated_levelFundRewardId_codec);
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
						this.levelFundRewardId_.AddEntriesFrom(input, LevelFundRewardRequest._repeated_levelFundRewardId_codec);
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

		private static readonly MessageParser<LevelFundRewardRequest> _parser = new MessageParser<LevelFundRewardRequest>(() => new LevelFundRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int LevelFundRewardIdFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_levelFundRewardId_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> levelFundRewardId_ = new RepeatedField<int>();
	}
}
