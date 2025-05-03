using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacyLevelUpCoolDownRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyLevelUpCoolDownRequest> Parser
		{
			get
			{
				return TalentLegacyLevelUpCoolDownRequest._parser;
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
		public int TalentLegacyId
		{
			get
			{
				return this.talentLegacyId_;
			}
			set
			{
				this.talentLegacyId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int UseType
		{
			get
			{
				return this.useType_;
			}
			set
			{
				this.useType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int UseNum
		{
			get
			{
				return this.useNum_;
			}
			set
			{
				this.useNum_ = value;
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
			if (this.CareerId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.CareerId);
			}
			if (this.TalentLegacyId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.TalentLegacyId);
			}
			if (this.UseType != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.UseType);
			}
			if (this.UseNum != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.UseNum);
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
			if (this.CareerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.CareerId);
			}
			if (this.TalentLegacyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TalentLegacyId);
			}
			if (this.UseType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.UseType);
			}
			if (this.UseNum != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.UseNum);
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
						this.CareerId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.TalentLegacyId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.UseType = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.UseNum = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentLegacyLevelUpCoolDownRequest> _parser = new MessageParser<TalentLegacyLevelUpCoolDownRequest>(() => new TalentLegacyLevelUpCoolDownRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int CareerIdFieldNumber = 2;

		private int careerId_;

		public const int TalentLegacyIdFieldNumber = 3;

		private int talentLegacyId_;

		public const int UseTypeFieldNumber = 4;

		private int useType_;

		public const int UseNumFieldNumber = 5;

		private int useNum_;
	}
}
