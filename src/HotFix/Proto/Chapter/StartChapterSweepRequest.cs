using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class StartChapterSweepRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<StartChapterSweepRequest> Parser
		{
			get
			{
				return StartChapterSweepRequest._parser;
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
		public uint Rate
		{
			get
			{
				return this.rate_;
			}
			set
			{
				this.rate_ = value;
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
			if (this.Rate != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Rate);
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
			if (this.Rate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rate);
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
					if (num != 24U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Rate = input.ReadUInt32();
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

		private static readonly MessageParser<StartChapterSweepRequest> _parser = new MessageParser<StartChapterSweepRequest>(() => new StartChapterSweepRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RateFieldNumber = 3;

		private uint rate_;
	}
}
