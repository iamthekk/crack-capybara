using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Artifact
{
	public sealed class ArtifactItemStarResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ArtifactItemStarResponse> Parser
		{
			get
			{
				return ArtifactItemStarResponse._parser;
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
		public ArtifactItemDto ArtifactItemDto
		{
			get
			{
				return this.artifactItemDto_;
			}
			set
			{
				this.artifactItemDto_ = value;
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
			if (this.artifactItemDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.ArtifactItemDto);
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
			if (this.artifactItemDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ArtifactItemDto);
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
						if (this.artifactItemDto_ == null)
						{
							this.artifactItemDto_ = new ArtifactItemDto();
						}
						input.ReadMessage(this.artifactItemDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ArtifactItemStarResponse> _parser = new MessageParser<ArtifactItemStarResponse>(() => new ArtifactItemStarResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ConfigIdFieldNumber = 3;

		private int configId_;

		public const int ArtifactItemDtoFieldNumber = 4;

		private ArtifactItemDto artifactItemDto_;
	}
}
