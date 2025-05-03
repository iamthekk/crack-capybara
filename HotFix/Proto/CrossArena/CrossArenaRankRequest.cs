using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.CrossArena
{
	public sealed class CrossArenaRankRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaRankRequest> Parser
		{
			get
			{
				return CrossArenaRankRequest._parser;
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
		public uint Page
		{
			get
			{
				return this.page_;
			}
			set
			{
				this.page_ = value;
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
			if (this.Page != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Page);
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
			if (this.Page != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Page);
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
						this.Page = input.ReadUInt32();
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

		private static readonly MessageParser<CrossArenaRankRequest> _parser = new MessageParser<CrossArenaRankRequest>(() => new CrossArenaRankRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PageFieldNumber = 2;

		private uint page_;
	}
}
