using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class SevenDayTaskActiveRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SevenDayTaskActiveRewardRequest> Parser
		{
			get
			{
				return SevenDayTaskActiveRewardRequest._parser;
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
		public uint SelectIdx
		{
			get
			{
				return this.selectIdx_;
			}
			set
			{
				this.selectIdx_ = value;
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
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.SelectIdx != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.SelectIdx);
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
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.SelectIdx != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SelectIdx);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.SelectIdx = input.ReadUInt32();
						}
					}
					else
					{
						this.ConfigId = input.ReadUInt32();
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

		private static readonly MessageParser<SevenDayTaskActiveRewardRequest> _parser = new MessageParser<SevenDayTaskActiveRewardRequest>(() => new SevenDayTaskActiveRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigIdFieldNumber = 2;

		private uint configId_;

		public const int SelectIdxFieldNumber = 3;

		private uint selectIdx_;
	}
}
