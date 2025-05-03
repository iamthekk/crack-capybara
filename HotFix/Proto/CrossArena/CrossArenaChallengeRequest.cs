using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.CrossArena
{
	public sealed class CrossArenaChallengeRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaChallengeRequest> Parser
		{
			get
			{
				return CrossArenaChallengeRequest._parser;
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
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ClientVersion
		{
			get
			{
				return this.clientVersion_;
			}
			set
			{
				this.clientVersion_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.UserId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.UserId);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ClientVersion);
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
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.ClientVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ClientVersion);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.ClientVersion = input.ReadString();
						}
					}
					else
					{
						this.UserId = input.ReadInt64();
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

		private static readonly MessageParser<CrossArenaChallengeRequest> _parser = new MessageParser<CrossArenaChallengeRequest>(() => new CrossArenaChallengeRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int UserIdFieldNumber = 2;

		private long userId_;

		public const int ClientVersionFieldNumber = 3;

		private string clientVersion_ = "";
	}
}
