using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableReceiveCumulativeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableReceiveCumulativeRewardResponse> Parser
		{
			get
			{
				return TurnTableReceiveCumulativeRewardResponse._parser;
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
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TurntableRewardIds
		{
			get
			{
				return this.turntableRewardIds_;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			this.turntableRewardIds_.WriteTo(output, TurnTableReceiveCumulativeRewardResponse._repeated_turntableRewardIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num + this.turntableRewardIds_.CalculateSize(TurnTableReceiveCumulativeRewardResponse._repeated_turntableRewardIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.turntableRewardIds_.AddEntriesFrom(input, TurnTableReceiveCumulativeRewardResponse._repeated_turntableRewardIds_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TurnTableReceiveCumulativeRewardResponse> _parser = new MessageParser<TurnTableReceiveCumulativeRewardResponse>(() => new TurnTableReceiveCumulativeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TurntableRewardIdsFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_turntableRewardIds_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> turntableRewardIds_ = new RepeatedField<int>();
	}
}
