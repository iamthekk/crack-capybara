using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacyLevelUpRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyLevelUpRequest> Parser
		{
			get
			{
				return TalentLegacyLevelUpRequest._parser;
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
							this.TalentLegacyId = input.ReadInt32();
						}
					}
					else
					{
						this.CareerId = input.ReadInt32();
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

		private static readonly MessageParser<TalentLegacyLevelUpRequest> _parser = new MessageParser<TalentLegacyLevelUpRequest>(() => new TalentLegacyLevelUpRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int CareerIdFieldNumber = 2;

		private int careerId_;

		public const int TalentLegacyIdFieldNumber = 3;

		private int talentLegacyId_;
	}
}
