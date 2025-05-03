using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class ActorAdvanceUpRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActorAdvanceUpRequest> Parser
		{
			get
			{
				return ActorAdvanceUpRequest._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
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
					input.SkipLastField();
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

		private static readonly MessageParser<ActorAdvanceUpRequest> _parser = new MessageParser<ActorAdvanceUpRequest>(() => new ActorAdvanceUpRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;
	}
}
