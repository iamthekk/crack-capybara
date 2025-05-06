using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Artifact
{
	public sealed class ArtifactUpgradeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ArtifactUpgradeResponse> Parser
		{
			get
			{
				return ArtifactUpgradeResponse._parser;
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
		public ArtifactInfo ArtifactInfo
		{
			get
			{
				return this.artifactInfo_;
			}
			set
			{
				this.artifactInfo_ = value;
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
			if (this.artifactInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.ArtifactInfo);
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
			if (this.artifactInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ArtifactInfo);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.artifactInfo_ == null)
							{
								this.artifactInfo_ = new ArtifactInfo();
							}
							input.ReadMessage(this.artifactInfo_);
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

		private static readonly MessageParser<ArtifactUpgradeResponse> _parser = new MessageParser<ArtifactUpgradeResponse>(() => new ArtifactUpgradeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ArtifactInfoFieldNumber = 3;

		private ArtifactInfo artifactInfo_;
	}
}
