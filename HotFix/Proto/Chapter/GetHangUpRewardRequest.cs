using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class GetHangUpRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GetHangUpRewardRequest> Parser
		{
			get
			{
				return GetHangUpRewardRequest._parser;
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
		public int Advert
		{
			get
			{
				return this.advert_;
			}
			set
			{
				this.advert_ = value;
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
			if (this.Advert != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Advert);
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
			if (this.Advert != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Advert);
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
						this.Advert = input.ReadInt32();
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

		private static readonly MessageParser<GetHangUpRewardRequest> _parser = new MessageParser<GetHangUpRewardRequest>(() => new GetHangUpRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AdvertFieldNumber = 2;

		private int advert_;
	}
}
