using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class EndWorldBossRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndWorldBossRequest> Parser
		{
			get
			{
				return EndWorldBossRequest._parser;
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
		public RepeatedField<int> RoundSkillList
		{
			get
			{
				return this.roundSkillList_;
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
			this.roundSkillList_.WriteTo(output, EndWorldBossRequest._repeated_roundSkillList_codec);
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
			num += this.roundSkillList_.CalculateSize(EndWorldBossRequest._repeated_roundSkillList_codec);
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
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						goto IL_0046;
					}
				}
				else
				{
					if (num == 18U)
					{
						goto IL_0046;
					}
					if (num == 26U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_0046:
				this.roundSkillList_.AddEntriesFrom(input, EndWorldBossRequest._repeated_roundSkillList_codec);
			}
		}

		private static readonly MessageParser<EndWorldBossRequest> _parser = new MessageParser<EndWorldBossRequest>(() => new EndWorldBossRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RoundSkillListFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_roundSkillList_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> roundSkillList_ = new RepeatedField<int>();

		public const int ClientVersionFieldNumber = 3;

		private string clientVersion_ = "";
	}
}
