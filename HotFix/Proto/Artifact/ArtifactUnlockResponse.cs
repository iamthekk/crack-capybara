using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Artifact
{
	public sealed class ArtifactUnlockResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ArtifactUnlockResponse> Parser
		{
			get
			{
				return ArtifactUnlockResponse._parser;
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
		public RepeatedField<ArtifactItemDto> ArtifactItemDto
		{
			get
			{
				return this.artifactItemDto_;
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
			this.artifactItemDto_.WriteTo(output, ArtifactUnlockResponse._repeated_artifactItemDto_codec);
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
			return num + this.artifactItemDto_.CalculateSize(ArtifactUnlockResponse._repeated_artifactItemDto_codec);
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
							this.artifactItemDto_.AddEntriesFrom(input, ArtifactUnlockResponse._repeated_artifactItemDto_codec);
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

		private static readonly MessageParser<ArtifactUnlockResponse> _parser = new MessageParser<ArtifactUnlockResponse>(() => new ArtifactUnlockResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ArtifactItemDtoFieldNumber = 3;

		private static readonly FieldCodec<ArtifactItemDto> _repeated_artifactItemDto_codec = FieldCodec.ForMessage<ArtifactItemDto>(26U, Proto.Common.ArtifactItemDto.Parser);

		private readonly RepeatedField<ArtifactItemDto> artifactItemDto_ = new RepeatedField<ArtifactItemDto>();
	}
}
