using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellDoChallengeRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellDoChallengeRequest> Parser
		{
			get
			{
				return HellDoChallengeRequest._parser;
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
		public int StageId
		{
			get
			{
				return this.stageId_;
			}
			set
			{
				this.stageId_ = value;
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
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.ClientVersion);
			}
			if (this.StageId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.StageId);
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
			if (this.ClientVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ClientVersion);
			}
			if (this.StageId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.StageId);
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
					if (num != 18U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.StageId = input.ReadInt32();
						}
					}
					else
					{
						this.ClientVersion = input.ReadString();
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

		private static readonly MessageParser<HellDoChallengeRequest> _parser = new MessageParser<HellDoChallengeRequest>(() => new HellDoChallengeRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ClientVersionFieldNumber = 2;

		private string clientVersion_ = "";

		public const int StageIdFieldNumber = 3;

		private int stageId_;
	}
}
