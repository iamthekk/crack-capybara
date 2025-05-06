using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class EndChapterCheckRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndChapterCheckRequest> Parser
		{
			get
			{
				return EndChapterCheckRequest._parser;
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
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int WaveIndex
		{
			get
			{
				return this.waveIndex_;
			}
			set
			{
				this.waveIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Attributes
		{
			get
			{
				return this.attributes_;
			}
			set
			{
				this.attributes_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> SkillIds
		{
			get
			{
				return this.skillIds_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> MonsterCfgId
		{
			get
			{
				return this.monsterCfgId_;
			}
		}

		[DebuggerNonUserCode]
		public ulong CurHp
		{
			get
			{
				return this.curHp_;
			}
			set
			{
				this.curHp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ReviveCount
		{
			get
			{
				return this.reviveCount_;
			}
			set
			{
				this.reviveCount_ = value;
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
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.WaveIndex);
			}
			if (this.Attributes.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Attributes);
			}
			this.skillIds_.WriteTo(output, EndChapterCheckRequest._repeated_skillIds_codec);
			this.monsterCfgId_.WriteTo(output, EndChapterCheckRequest._repeated_monsterCfgId_codec);
			if (this.CurHp != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.CurHp);
			}
			if (this.ReviveCount != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.ReviveCount);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(74);
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
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.Attributes.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Attributes);
			}
			num += this.skillIds_.CalculateSize(EndChapterCheckRequest._repeated_skillIds_codec);
			num += this.monsterCfgId_.CalculateSize(EndChapterCheckRequest._repeated_monsterCfgId_codec);
			if (this.CurHp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.CurHp);
			}
			if (this.ReviveCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ReviveCount);
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
				if (num <= 40U)
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
							this.ChapterId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
						if (num == 34U)
						{
							this.Attributes = input.ReadString();
							continue;
						}
						if (num == 40U)
						{
							goto IL_00C2;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 42U)
					{
						goto IL_00C2;
					}
					if (num == 48U || num == 50U)
					{
						this.monsterCfgId_.AddEntriesFrom(input, EndChapterCheckRequest._repeated_monsterCfgId_codec);
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.CurHp = input.ReadUInt64();
						continue;
					}
					if (num == 64U)
					{
						this.ReviveCount = input.ReadInt32();
						continue;
					}
					if (num == 74U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_00C2:
				this.skillIds_.AddEntriesFrom(input, EndChapterCheckRequest._repeated_skillIds_codec);
			}
		}

		private static readonly MessageParser<EndChapterCheckRequest> _parser = new MessageParser<EndChapterCheckRequest>(() => new EndChapterCheckRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ChapterIdFieldNumber = 2;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 3;

		private int waveIndex_;

		public const int AttributesFieldNumber = 4;

		private string attributes_ = "";

		public const int SkillIdsFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_skillIds_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> skillIds_ = new RepeatedField<int>();

		public const int MonsterCfgIdFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(50U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int CurHpFieldNumber = 7;

		private ulong curHp_;

		public const int ReviveCountFieldNumber = 8;

		private int reviveCount_;

		public const int ClientVersionFieldNumber = 9;

		private string clientVersion_ = "";
	}
}
