using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossBattleGRankRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossBattleGRankRequest> Parser
		{
			get
			{
				return GuildBossBattleGRankRequest._parser;
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
		public int OptType
		{
			get
			{
				return this.optType_;
			}
			set
			{
				this.optType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Page
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
			if (this.OptType != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.OptType);
			}
			if (this.Page != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Page);
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
			if (this.OptType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptType);
			}
			if (this.Page != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Page);
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
							this.Page = input.ReadInt32();
						}
					}
					else
					{
						this.OptType = input.ReadInt32();
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

		private static readonly MessageParser<GuildBossBattleGRankRequest> _parser = new MessageParser<GuildBossBattleGRankRequest>(() => new GuildBossBattleGRankRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int OptTypeFieldNumber = 2;

		private int optType_;

		public const int PageFieldNumber = 3;

		private int page_;
	}
}
