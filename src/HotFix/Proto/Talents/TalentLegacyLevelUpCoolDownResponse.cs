using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacyLevelUpCoolDownResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyLevelUpCoolDownResponse> Parser
		{
			get
			{
				return TalentLegacyLevelUpCoolDownResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TalentLegacyCareerDto CareerInfo
		{
			get
			{
				return this.careerInfo_;
			}
			set
			{
				this.careerInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public AdDataDto AdData
		{
			get
			{
				return this.adData_;
			}
			set
			{
				this.adData_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.careerInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CareerInfo);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.AdData);
			}
			if (this.AssemblySlotCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.AssemblySlotCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.careerInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CareerInfo);
			}
			if (this.adData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			if (this.AssemblySlotCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AssemblySlotCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.careerInfo_ == null)
						{
							this.careerInfo_ = new TalentLegacyCareerDto();
						}
						input.ReadMessage(this.careerInfo_);
						continue;
					}
					if (num == 34U)
					{
						if (this.adData_ == null)
						{
							this.adData_ = new AdDataDto();
						}
						input.ReadMessage(this.adData_);
						continue;
					}
					if (num == 40U)
					{
						this.AssemblySlotCount = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentLegacyLevelUpCoolDownResponse> _parser = new MessageParser<TalentLegacyLevelUpCoolDownResponse>(() => new TalentLegacyLevelUpCoolDownResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int CareerInfoFieldNumber = 3;

		private TalentLegacyCareerDto careerInfo_;

		public const int AdDataFieldNumber = 4;

		private AdDataDto adData_;

		public const int AssemblySlotCountFieldNumber = 5;

		private int assemblySlotCount_;
	}
}
