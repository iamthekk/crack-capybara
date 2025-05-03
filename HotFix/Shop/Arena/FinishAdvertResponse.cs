using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class FinishAdvertResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<FinishAdvertResponse> Parser
		{
			get
			{
				return FinishAdvertResponse._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.AdData);
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
			if (this.adData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						if (this.adData_ == null)
						{
							this.adData_ = new AdDataDto();
						}
						input.ReadMessage(this.adData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<FinishAdvertResponse> _parser = new MessageParser<FinishAdvertResponse>(() => new FinishAdvertResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int AdDataFieldNumber = 2;

		private AdDataDto adData_;
	}
}
