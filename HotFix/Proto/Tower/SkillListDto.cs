using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Tower
{
	public sealed class SkillListDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SkillListDto> Parser
		{
			get
			{
				return SkillListDto._parser;
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
			this.skillList_.WriteTo(output, SkillListDto._repeated_skillList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.skillList_.CalculateSize(SkillListDto._repeated_skillList_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U && num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					this.skillList_.AddEntriesFrom(input, SkillListDto._repeated_skillList_codec);
				}
			}
		}

		private static readonly MessageParser<SkillListDto> _parser = new MessageParser<SkillListDto>(() => new SkillListDto());

		public const int SkillListFieldNumber = 1;

		private static readonly FieldCodec<int> _repeated_skillList_codec = FieldCodec.ForInt32(10U);

		private readonly RepeatedField<int> skillList_ = new RepeatedField<int>();
	}
}
