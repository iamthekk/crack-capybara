using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterActRankRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActRankRequest> Parser
		{
			get
			{
				return ChapterActRankRequest._parser;
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
		public int ActId
		{
			get
			{
				return this.actId_;
			}
			set
			{
				this.actId_ = value;
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
			if (this.ActId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ActId);
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
			if (this.ActId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ActId);
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
						this.ActId = input.ReadInt32();
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

		private static readonly MessageParser<ChapterActRankRequest> _parser = new MessageParser<ChapterActRankRequest>(() => new ChapterActRankRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ActIdFieldNumber = 2;

		private int actId_;
	}
}
