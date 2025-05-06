using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Social
{
	public sealed class InteractDetailRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<InteractDetailRequest> Parser
		{
			get
			{
				return InteractDetailRequest._parser;
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
		public long RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
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
			if (this.RowId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.RowId);
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
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
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
						this.RowId = input.ReadInt64();
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

		private static readonly MessageParser<InteractDetailRequest> _parser = new MessageParser<InteractDetailRequest>(() => new InteractDetailRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdFieldNumber = 2;

		private long rowId_;
	}
}
