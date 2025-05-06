using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossBuyCntRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossBuyCntRequest> Parser
		{
			get
			{
				return GuildBossBuyCntRequest._parser;
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
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
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
			if (this.Type != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Type);
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
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
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
						this.Type = input.ReadUInt32();
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

		private static readonly MessageParser<GuildBossBuyCntRequest> _parser = new MessageParser<GuildBossBuyCntRequest>(() => new GuildBossBuyCntRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int TypeFieldNumber = 2;

		private uint type_;
	}
}
