using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mount
{
	public sealed class MountItemStarResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MountItemStarResponse> Parser
		{
			get
			{
				return MountItemStarResponse._parser;
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
		public MountItemDto MountItemDto
		{
			get
			{
				return this.mountItemDto_;
			}
			set
			{
				this.mountItemDto_ = value;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ConfigId);
			}
			if (this.mountItemDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.MountItemDto);
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
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.mountItemDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MountItemDto);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
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
					if (num == 34U)
					{
						if (this.mountItemDto_ == null)
						{
							this.mountItemDto_ = new MountItemDto();
						}
						input.ReadMessage(this.mountItemDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MountItemStarResponse> _parser = new MessageParser<MountItemStarResponse>(() => new MountItemStarResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ConfigIdFieldNumber = 3;

		private int configId_;

		public const int MountItemDtoFieldNumber = 4;

		private MountItemDto mountItemDto_;
	}
}
