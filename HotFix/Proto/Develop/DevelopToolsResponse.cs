using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Develop
{
	public sealed class DevelopToolsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DevelopToolsResponse> Parser
		{
			get
			{
				return DevelopToolsResponse._parser;
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
		public string RespData
		{
			get
			{
				return this.respData_;
			}
			set
			{
				this.respData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.RespData.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.RespData);
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
			if (this.RespData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RespData);
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
						this.RespData = input.ReadString();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<DevelopToolsResponse> _parser = new MessageParser<DevelopToolsResponse>(() => new DevelopToolsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RespDataFieldNumber = 2;

		private string respData_ = "";
	}
}
