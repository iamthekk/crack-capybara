using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellEnterSelectSkillRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellEnterSelectSkillRequest> Parser
		{
			get
			{
				return HellEnterSelectSkillRequest._parser;
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
		public RepeatedField<int> SkillList
		{
			get
			{
				return this.skillList_;
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
			this.skillList_.WriteTo(output, HellEnterSelectSkillRequest._repeated_skillList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.skillList_.CalculateSize(HellEnterSelectSkillRequest._repeated_skillList_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.skillList_.AddEntriesFrom(input, HellEnterSelectSkillRequest._repeated_skillList_codec);
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

		private static readonly MessageParser<HellEnterSelectSkillRequest> _parser = new MessageParser<HellEnterSelectSkillRequest>(() => new HellEnterSelectSkillRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int SkillListFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_skillList_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> skillList_ = new RepeatedField<int>();
	}
}
