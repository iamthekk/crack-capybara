using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Grpc.Health.V1
{
	public sealed class HealthCheckRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HealthCheckRequest> Parser
		{
			get
			{
				return HealthCheckRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public string Service
		{
			get
			{
				return this.service_;
			}
			set
			{
				this.service_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Service.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Service);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Service.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Service);
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
					this.Service = input.ReadString();
				}
			}
		}

		private static readonly MessageParser<HealthCheckRequest> _parser = new MessageParser<HealthCheckRequest>(() => new HealthCheckRequest());

		public const int ServiceFieldNumber = 1;

		private string service_ = "";
	}
}
