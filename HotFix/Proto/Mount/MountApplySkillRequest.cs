using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mount
{
	public sealed class MountApplySkillRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MountApplySkillRequest> Parser
		{
			get
			{
				return MountApplySkillRequest._parser;
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
		public int AdvanceId
		{
			get
			{
				return this.advanceId_;
			}
			set
			{
				this.advanceId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OptType
		{
			get
			{
				return this.optType_;
			}
			set
			{
				this.optType_ = value;
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
			if (this.AdvanceId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.AdvanceId);
			}
			if (this.OptType != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.OptType);
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
			if (this.AdvanceId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AdvanceId);
			}
			if (this.OptType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptType);
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
							this.OptType = input.ReadInt32();
						}
					}
					else
					{
						this.AdvanceId = input.ReadInt32();
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

		private static readonly MessageParser<MountApplySkillRequest> _parser = new MessageParser<MountApplySkillRequest>(() => new MountApplySkillRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AdvanceIdFieldNumber = 2;

		private int advanceId_;

		public const int OptTypeFieldNumber = 3;

		private int optType_;
	}
}
