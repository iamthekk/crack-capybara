using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class OpenBombRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<OpenBombRequest> Parser
		{
			get
			{
				return OpenBombRequest._parser;
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
		public int Pos
		{
			get
			{
				return this.pos_;
			}
			set
			{
				this.pos_ = value;
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
			if (this.Pos != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Pos);
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
			if (this.Pos != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Pos);
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
						this.Pos = input.ReadInt32();
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

		private static readonly MessageParser<OpenBombRequest> _parser = new MessageParser<OpenBombRequest>(() => new OpenBombRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PosFieldNumber = 2;

		private int pos_;
	}
}
