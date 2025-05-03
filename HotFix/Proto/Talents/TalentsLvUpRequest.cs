using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentsLvUpRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentsLvUpRequest> Parser
		{
			get
			{
				return TalentsLvUpRequest._parser;
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
		public string AttributeType
		{
			get
			{
				return this.attributeType_;
			}
			set
			{
				this.attributeType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint Step
		{
			get
			{
				return this.step_;
			}
			set
			{
				this.step_ = value;
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
			if (this.AttributeType.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.AttributeType);
			}
			if (this.Step != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Step);
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
			if (this.AttributeType.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AttributeType);
			}
			if (this.Step != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Step);
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
					if (num != 18U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Step = input.ReadUInt32();
						}
					}
					else
					{
						this.AttributeType = input.ReadString();
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

		private static readonly MessageParser<TalentsLvUpRequest> _parser = new MessageParser<TalentsLvUpRequest>(() => new TalentsLvUpRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AttributeTypeFieldNumber = 2;

		private string attributeType_ = "";

		public const int StepFieldNumber = 3;

		private uint step_;
	}
}
