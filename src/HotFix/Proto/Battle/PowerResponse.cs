using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class PowerResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PowerResponse> Parser
		{
			get
			{
				return PowerResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public RpcPowerResp Resp
		{
			get
			{
				return this.resp_;
			}
			set
			{
				this.resp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.resp_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.Resp);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.resp_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Resp);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
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
						input.SkipLastField();
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					if (this.resp_ == null)
					{
						this.resp_ = new RpcPowerResp();
					}
					input.ReadMessage(this.resp_);
				}
			}
		}

		private static readonly MessageParser<PowerResponse> _parser = new MessageParser<PowerResponse>(() => new PowerResponse());

		public const int RespFieldNumber = 1;

		private RpcPowerResp resp_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;
	}
}
