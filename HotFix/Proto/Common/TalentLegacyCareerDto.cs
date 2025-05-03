using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class TalentLegacyCareerDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyCareerDto> Parser
		{
			get
			{
				return TalentLegacyCareerDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TalentLegacyDto> TalentLegacys
		{
			get
			{
				return this.talentLegacys_;
			}
		}

		[DebuggerNonUserCode]
		public int CareerId
		{
			get
			{
				return this.careerId_;
			}
			set
			{
				this.careerId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> AssemblyTalentLegacyId
		{
			get
			{
				return this.assemblyTalentLegacyId_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.talentLegacys_.WriteTo(output, TalentLegacyCareerDto._repeated_talentLegacys_codec);
			if (this.CareerId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.CareerId);
			}
			this.assemblyTalentLegacyId_.WriteTo(output, TalentLegacyCareerDto._repeated_assemblyTalentLegacyId_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.talentLegacys_.CalculateSize(TalentLegacyCareerDto._repeated_talentLegacys_codec);
			if (this.CareerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.CareerId);
			}
			return num + this.assemblyTalentLegacyId_.CalculateSize(TalentLegacyCareerDto._repeated_assemblyTalentLegacyId_codec);
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
						this.talentLegacys_.AddEntriesFrom(input, TalentLegacyCareerDto._repeated_talentLegacys_codec);
						continue;
					}
					if (num == 16U)
					{
						this.CareerId = input.ReadInt32();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.assemblyTalentLegacyId_.AddEntriesFrom(input, TalentLegacyCareerDto._repeated_assemblyTalentLegacyId_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentLegacyCareerDto> _parser = new MessageParser<TalentLegacyCareerDto>(() => new TalentLegacyCareerDto());

		public const int TalentLegacysFieldNumber = 1;

		private static readonly FieldCodec<TalentLegacyDto> _repeated_talentLegacys_codec = FieldCodec.ForMessage<TalentLegacyDto>(10U, TalentLegacyDto.Parser);

		private readonly RepeatedField<TalentLegacyDto> talentLegacys_ = new RepeatedField<TalentLegacyDto>();

		public const int CareerIdFieldNumber = 2;

		private int careerId_;

		public const int AssemblyTalentLegacyIdFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_assemblyTalentLegacyId_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> assemblyTalentLegacyId_ = new RepeatedField<int>();
	}
}
