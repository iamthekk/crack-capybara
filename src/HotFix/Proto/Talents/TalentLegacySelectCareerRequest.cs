using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacySelectCareerRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacySelectCareerRequest> Parser
		{
			get
			{
				return TalentLegacySelectCareerRequest._parser;
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
						input.SkipLastField();
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

		private static readonly MessageParser<TalentLegacySelectCareerRequest> _parser = new MessageParser<TalentLegacySelectCareerRequest>(() => new TalentLegacySelectCareerRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int CareerIdFieldNumber = 2;

		private int careerId_;
	}
}
