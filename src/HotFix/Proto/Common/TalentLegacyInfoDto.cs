using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class TalentLegacyInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyInfoDto> Parser
		{
			get
			{
				return TalentLegacyInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TalentLegacyCareerDto> CareerList
		{
			get
			{
				return this.careerList_;
			}
		}

		[DebuggerNonUserCode]
		public int AssemblySlotCount
		{
			get
			{
				return this.assemblySlotCount_;
			}
			set
			{
				this.assemblySlotCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int SelectCareer
		{
			get
			{
				return this.selectCareer_;
			}
			set
			{
				this.selectCareer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.careerList_.WriteTo(output, TalentLegacyInfoDto._repeated_careerList_codec);
			if (this.AssemblySlotCount != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.AssemblySlotCount);
			}
			if (this.SelectCareer != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.SelectCareer);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.careerList_.CalculateSize(TalentLegacyInfoDto._repeated_careerList_codec);
			if (this.AssemblySlotCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AssemblySlotCount);
			}
			if (this.SelectCareer != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.SelectCareer);
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
							this.SelectCareer = input.ReadInt32();
						}
					}
					else
					{
						this.AssemblySlotCount = input.ReadInt32();
					}
				}
				else
				{
					this.careerList_.AddEntriesFrom(input, TalentLegacyInfoDto._repeated_careerList_codec);
				}
			}
		}

		private static readonly MessageParser<TalentLegacyInfoDto> _parser = new MessageParser<TalentLegacyInfoDto>(() => new TalentLegacyInfoDto());

		public const int CareerListFieldNumber = 1;

		private static readonly FieldCodec<TalentLegacyCareerDto> _repeated_careerList_codec = FieldCodec.ForMessage<TalentLegacyCareerDto>(10U, TalentLegacyCareerDto.Parser);

		private readonly RepeatedField<TalentLegacyCareerDto> careerList_ = new RepeatedField<TalentLegacyCareerDto>();

		public const int AssemblySlotCountFieldNumber = 2;

		private int assemblySlotCount_;

		public const int SelectCareerFieldNumber = 3;

		private int selectCareer_;
	}
}
