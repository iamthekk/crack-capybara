using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossBoxRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossBoxRewardRequest> Parser
		{
			get
			{
				return GuildBossBoxRewardRequest._parser;
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
		public uint BoxId
		{
			get
			{
				return this.boxId_;
			}
			set
			{
				this.boxId_ = value;
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
			if (this.BoxId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.BoxId);
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
			if (this.BoxId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BoxId);
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
						this.BoxId = input.ReadUInt32();
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

		private static readonly MessageParser<GuildBossBoxRewardRequest> _parser = new MessageParser<GuildBossBoxRewardRequest>(() => new GuildBossBoxRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int BoxIdFieldNumber = 2;

		private uint boxId_;
	}
}
