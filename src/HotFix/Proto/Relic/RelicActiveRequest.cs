using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Relic
{
	public sealed class RelicActiveRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RelicActiveRequest> Parser
		{
			get
			{
				return RelicActiveRequest._parser;
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
		public int RelicId
		{
			get
			{
				return this.relicId_;
			}
			set
			{
				this.relicId_ = value;
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
			if (this.RelicId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.RelicId);
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
			if (this.RelicId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RelicId);
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
						this.RelicId = input.ReadInt32();
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

		private static readonly MessageParser<RelicActiveRequest> _parser = new MessageParser<RelicActiveRequest>(() => new RelicActiveRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RelicIdFieldNumber = 2;

		private int relicId_;
	}
}
