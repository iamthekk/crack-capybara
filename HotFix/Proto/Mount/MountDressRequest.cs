using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mount
{
	public sealed class MountDressRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MountDressRequest> Parser
		{
			get
			{
				return MountDressRequest._parser;
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
		public int ConfigType
		{
			get
			{
				return this.configType_;
			}
			set
			{
				this.configType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OptType
		{
			get
			{
				return this.optType_;
			}
			set
			{
				this.optType_ = value;
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
			if (this.ConfigType != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigType);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ConfigId);
			}
			if (this.OptType != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.OptType);
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
			if (this.ConfigType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigType);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.OptType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptType);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.ConfigType = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.ConfigId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.OptType = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MountDressRequest> _parser = new MessageParser<MountDressRequest>(() => new MountDressRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigTypeFieldNumber = 2;

		private int configType_;

		public const int ConfigIdFieldNumber = 3;

		private int configId_;

		public const int OptTypeFieldNumber = 4;

		private int optType_;
	}
}
