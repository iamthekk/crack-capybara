using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ErrorMsg : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ErrorMsg> Parser
		{
			get
			{
				return ErrorMsg._parser;
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
		public string Msg
		{
			get
			{
				return this.msg_;
			}
			set
			{
				this.msg_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Msg.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Msg);
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
			if (this.Msg.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Msg);
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
						this.Msg = input.ReadString();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ErrorMsg> _parser = new MessageParser<ErrorMsg>(() => new ErrorMsg());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int MsgFieldNumber = 2;

		private string msg_ = "";
	}
}
