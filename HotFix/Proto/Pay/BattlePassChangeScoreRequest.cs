using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class BattlePassChangeScoreRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BattlePassChangeScoreRequest> Parser
		{
			get
			{
				return BattlePassChangeScoreRequest._parser;
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
		public uint AddScore
		{
			get
			{
				return this.addScore_;
			}
			set
			{
				this.addScore_ = value;
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
			if (this.AddScore != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.AddScore);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.AddScore != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AddScore);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.AddScore = input.ReadUInt32();
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

		private static readonly MessageParser<BattlePassChangeScoreRequest> _parser = new MessageParser<BattlePassChangeScoreRequest>(() => new BattlePassChangeScoreRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AddScoreFieldNumber = 2;

		private uint addScore_;
	}
}
