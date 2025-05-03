using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildModifyRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildModifyRequest> Parser
		{
			get
			{
				return GuildModifyRequest._parser;
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
		public string GuildName
		{
			get
			{
				return this.guildName_;
			}
			set
			{
				this.guildName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string GuildIntro
		{
			get
			{
				return this.guildIntro_;
			}
			set
			{
				this.guildIntro_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint GuildIcon
		{
			get
			{
				return this.guildIcon_;
			}
			set
			{
				this.guildIcon_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GuildIconBg
		{
			get
			{
				return this.guildIconBg_;
			}
			set
			{
				this.guildIconBg_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ApplyType
		{
			get
			{
				return this.applyType_;
			}
			set
			{
				this.applyType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ApplyCondition
		{
			get
			{
				return this.applyCondition_;
			}
			set
			{
				this.applyCondition_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsModifyGuildIntro
		{
			get
			{
				return this.isModifyGuildIntro_;
			}
			set
			{
				this.isModifyGuildIntro_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Language
		{
			get
			{
				return this.language_;
			}
			set
			{
				this.language_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GuildNotice
		{
			get
			{
				return this.guildNotice_;
			}
			set
			{
				this.guildNotice_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool IsModifyGuildNotice
		{
			get
			{
				return this.isModifyGuildNotice_;
			}
			set
			{
				this.isModifyGuildNotice_ = value;
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
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.GuildName);
			}
			if (this.GuildIntro.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.GuildIntro);
			}
			if (this.GuildIcon != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.GuildIconBg);
			}
			if (this.ApplyType != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.ApplyType);
			}
			if (this.ApplyCondition != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.ApplyCondition);
			}
			if (this.IsModifyGuildIntro)
			{
				output.WriteRawTag(64);
				output.WriteBool(this.IsModifyGuildIntro);
			}
			if (this.Language != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.Language);
			}
			if (this.GuildNotice.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.GuildNotice);
			}
			if (this.IsModifyGuildNotice)
			{
				output.WriteRawTag(88);
				output.WriteBool(this.IsModifyGuildNotice);
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
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.GuildIntro.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildIntro);
			}
			if (this.GuildIcon != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIconBg);
			}
			if (this.ApplyType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyType);
			}
			if (this.ApplyCondition != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyCondition);
			}
			if (this.IsModifyGuildIntro)
			{
				num += 2;
			}
			if (this.Language != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Language);
			}
			if (this.GuildNotice.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildNotice);
			}
			if (this.IsModifyGuildNotice)
			{
				num += 2;
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
					if (num <= 18U)
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
						if (num == 18U)
						{
							this.GuildName = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.GuildIntro = input.ReadString();
							continue;
						}
						if (num == 32U)
						{
							this.GuildIcon = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.GuildIconBg = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 48U)
					{
						this.ApplyType = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.ApplyCondition = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.IsModifyGuildIntro = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.Language = input.ReadUInt32();
						continue;
					}
					if (num == 82U)
					{
						this.GuildNotice = input.ReadString();
						continue;
					}
					if (num == 88U)
					{
						this.IsModifyGuildNotice = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildModifyRequest> _parser = new MessageParser<GuildModifyRequest>(() => new GuildModifyRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int GuildNameFieldNumber = 2;

		private string guildName_ = "";

		public const int GuildIntroFieldNumber = 3;

		private string guildIntro_ = "";

		public const int GuildIconFieldNumber = 4;

		private uint guildIcon_;

		public const int GuildIconBgFieldNumber = 5;

		private uint guildIconBg_;

		public const int ApplyTypeFieldNumber = 6;

		private uint applyType_;

		public const int ApplyConditionFieldNumber = 7;

		private uint applyCondition_;

		public const int IsModifyGuildIntroFieldNumber = 8;

		private bool isModifyGuildIntro_;

		public const int LanguageFieldNumber = 9;

		private uint language_;

		public const int GuildNoticeFieldNumber = 10;

		private string guildNotice_ = "";

		public const int IsModifyGuildNoticeFieldNumber = 11;

		private bool isModifyGuildNotice_;
	}
}
