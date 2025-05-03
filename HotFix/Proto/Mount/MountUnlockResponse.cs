using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mount
{
	public sealed class MountUnlockResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MountUnlockResponse> Parser
		{
			get
			{
				return MountUnlockResponse._parser;
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
		public RepeatedField<MountItemDto> MountItemDtos
		{
			get
			{
				return this.mountItemDtos_;
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
			this.mountItemDtos_.WriteTo(output, MountUnlockResponse._repeated_mountItemDtos_codec);
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
			return num + this.mountItemDtos_.CalculateSize(MountUnlockResponse._repeated_mountItemDtos_codec);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.mountItemDtos_.AddEntriesFrom(input, MountUnlockResponse._repeated_mountItemDtos_codec);
						}
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
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MountUnlockResponse> _parser = new MessageParser<MountUnlockResponse>(() => new MountUnlockResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int MountItemDtosFieldNumber = 3;

		private static readonly FieldCodec<MountItemDto> _repeated_mountItemDtos_codec = FieldCodec.ForMessage<MountItemDto>(26U, MountItemDto.Parser);

		private readonly RepeatedField<MountItemDto> mountItemDtos_ = new RepeatedField<MountItemDto>();
	}
}
