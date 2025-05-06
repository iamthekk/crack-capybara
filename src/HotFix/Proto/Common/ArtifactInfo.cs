using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ArtifactInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ArtifactInfo> Parser
		{
			get
			{
				return ArtifactInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Stage
		{
			get
			{
				return this.stage_;
			}
			set
			{
				this.stage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkillArtifactId
		{
			get
			{
				return this.skillArtifactId_;
			}
			set
			{
				this.skillArtifactId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigType
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
		public uint ConfigId
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Stage != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Stage);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Exp);
			}
			if (this.SkillArtifactId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.SkillArtifactId);
			}
			if (this.ConfigType != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ConfigType);
			}
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.ConfigId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Stage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Stage);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.SkillArtifactId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkillArtifactId);
			}
			if (this.ConfigType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigType);
			}
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Stage = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.SkillArtifactId = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.ConfigType = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ArtifactInfo> _parser = new MessageParser<ArtifactInfo>(() => new ArtifactInfo());

		public const int StageFieldNumber = 1;

		private uint stage_;

		public const int LevelFieldNumber = 2;

		private uint level_;

		public const int ExpFieldNumber = 3;

		private uint exp_;

		public const int SkillArtifactIdFieldNumber = 4;

		private uint skillArtifactId_;

		public const int ConfigTypeFieldNumber = 5;

		private uint configType_;

		public const int ConfigIdFieldNumber = 6;

		private uint configId_;
	}
}
