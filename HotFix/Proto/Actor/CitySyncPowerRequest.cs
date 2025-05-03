using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CitySyncPowerRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CitySyncPowerRequest> Parser
		{
			get
			{
				return CitySyncPowerRequest._parser;
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
		public long Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ClientVersion
		{
			get
			{
				return this.clientVersion_;
			}
			set
			{
				this.clientVersion_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Power != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Power);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ClientVersion);
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
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.ClientVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ClientVersion);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.ClientVersion = input.ReadString();
						}
					}
					else
					{
						this.Power = input.ReadInt64();
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

		private static readonly MessageParser<CitySyncPowerRequest> _parser = new MessageParser<CitySyncPowerRequest>(() => new CitySyncPowerRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PowerFieldNumber = 2;

		private long power_;

		public const int ClientVersionFieldNumber = 3;

		private string clientVersion_ = "";
	}
}
