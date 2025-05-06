using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class GetWaveRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GetWaveRewardResponse> Parser
		{
			get
			{
				return GetWaveRewardResponse._parser;
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
		public RepeatedField<int> CanRewardList
		{
			get
			{
				return this.canRewardList_;
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
			this.canRewardList_.WriteTo(output, GetWaveRewardResponse._repeated_canRewardList_codec);
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
			return num + this.canRewardList_.CalculateSize(GetWaveRewardResponse._repeated_canRewardList_codec);
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
					this.canRewardList_.AddEntriesFrom(input, GetWaveRewardResponse._repeated_canRewardList_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GetWaveRewardResponse> _parser = new MessageParser<GetWaveRewardResponse>(() => new GetWaveRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int CanRewardListFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_canRewardList_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> canRewardList_ = new RepeatedField<int>();
	}
}
