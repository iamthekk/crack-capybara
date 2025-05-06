using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacySwitchRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacySwitchRequest> Parser
		{
			get
			{
				return TalentLegacySwitchRequest._parser;
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
		public int FromCareerId
		{
			get
			{
				return this.fromCareerId_;
			}
			set
			{
				this.fromCareerId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Index
		{
			get
			{
				return this.index_;
			}
			set
			{
				this.index_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ToTalentLegacyId
		{
			get
			{
				return this.toTalentLegacyId_;
			}
			set
			{
				this.toTalentLegacyId_ = value;
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
			if (this.FromCareerId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.FromCareerId);
			}
			if (this.Index != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Index);
			}
			if (this.ToTalentLegacyId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.ToTalentLegacyId);
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
			if (this.FromCareerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FromCareerId);
			}
			if (this.Index != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Index);
			}
			if (this.ToTalentLegacyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ToTalentLegacyId);
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
						this.FromCareerId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Index = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.ToTalentLegacyId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentLegacySwitchRequest> _parser = new MessageParser<TalentLegacySwitchRequest>(() => new TalentLegacySwitchRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int FromCareerIdFieldNumber = 2;

		private int fromCareerId_;

		public const int IndexFieldNumber = 3;

		private int index_;

		public const int ToTalentLegacyIdFieldNumber = 4;

		private int toTalentLegacyId_;
	}
}
