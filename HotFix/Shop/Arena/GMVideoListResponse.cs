using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class GMVideoListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GMVideoListResponse> Parser
		{
			get
			{
				return GMVideoListResponse._parser;
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
		public RepeatedField<GmVideoDto> GmVideoDtos
		{
			get
			{
				return this.gmVideoDtos_;
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
			this.gmVideoDtos_.WriteTo(output, GMVideoListResponse._repeated_gmVideoDtos_codec);
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
			return num + this.gmVideoDtos_.CalculateSize(GMVideoListResponse._repeated_gmVideoDtos_codec);
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
							this.gmVideoDtos_.AddEntriesFrom(input, GMVideoListResponse._repeated_gmVideoDtos_codec);
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

		private static readonly MessageParser<GMVideoListResponse> _parser = new MessageParser<GMVideoListResponse>(() => new GMVideoListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GmVideoDtosFieldNumber = 3;

		private static readonly FieldCodec<GmVideoDto> _repeated_gmVideoDtos_codec = FieldCodec.ForMessage<GmVideoDto>(26U, GmVideoDto.Parser);

		private readonly RepeatedField<GmVideoDto> gmVideoDtos_ = new RepeatedField<GmVideoDto>();
	}
}
