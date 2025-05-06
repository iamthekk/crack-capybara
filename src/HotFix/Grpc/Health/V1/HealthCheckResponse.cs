using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Grpc.Health.V1
{
	public sealed class HealthCheckResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HealthCheckResponse> Parser
		{
			get
			{
				return HealthCheckResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public ServingStatus Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Status != ServingStatus.Unknown)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Status);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Status != ServingStatus.Unknown)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Status);
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
					input.SkipLastField();
				}
				else
				{
					this.status_ = (ServingStatus)input.ReadEnum();
				}
			}
		}

		private static readonly MessageParser<HealthCheckResponse> _parser = new MessageParser<HealthCheckResponse>(() => new HealthCheckResponse());

		public const int StatusFieldNumber = 1;

		private ServingStatus status_;
	}
}
