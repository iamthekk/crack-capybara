using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Battle
{
	public sealed class RpcPowerResp : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RpcPowerResp> Parser
		{
			get
			{
				return RpcPowerResp._parser;
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
		public long Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
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
			if (this.Result != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Result);
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
			if (this.Result != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Result);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Result = input.ReadInt64();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<RpcPowerResp> _parser = new MessageParser<RpcPowerResp>(() => new RpcPowerResp());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ResultFieldNumber = 2;

		private long result_;
	}
}
