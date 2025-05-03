using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mail
{
	public sealed class MailDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MailDto> Parser
		{
			get
			{
				return MailDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Title
		{
			get
			{
				return this.title_;
			}
			set
			{
				this.title_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Content
		{
			get
			{
				return this.content_;
			}
			set
			{
				this.content_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Rewards
		{
			get
			{
				return this.rewards_;
			}
		}

		[DebuggerNonUserCode]
		public bool RewardsState
		{
			get
			{
				return this.rewardsState_;
			}
			set
			{
				this.rewardsState_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong CreateTimestamp
		{
			get
			{
				return this.createTimestamp_;
			}
			set
			{
				this.createTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MailType
		{
			get
			{
				return this.mailType_;
			}
			set
			{
				this.mailType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint LanguageTitleId
		{
			get
			{
				return this.languageTitleId_;
			}
			set
			{
				this.languageTitleId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint LanguageContentId
		{
			get
			{
				return this.languageContentId_;
			}
			set
			{
				this.languageContentId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string LanguageContentParams
		{
			get
			{
				return this.languageContentParams_;
			}
			set
			{
				this.languageContentParams_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong ExpiredTimestamp
		{
			get
			{
				return this.expiredTimestamp_;
			}
			set
			{
				this.expiredTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RowId);
			}
			if (this.Title.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Title);
			}
			if (this.Content.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Content);
			}
			this.rewards_.WriteTo(output, MailDto._repeated_rewards_codec);
			if (this.RewardsState)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.RewardsState);
			}
			if (this.CreateTimestamp != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.CreateTimestamp);
			}
			if (this.MailType != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.MailType);
			}
			if (this.LanguageTitleId != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.LanguageTitleId);
			}
			if (this.LanguageContentId != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.LanguageContentId);
			}
			if (this.LanguageContentParams.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.LanguageContentParams);
			}
			if (this.ExpiredTimestamp != 0UL)
			{
				output.WriteRawTag(88);
				output.WriteUInt64(this.ExpiredTimestamp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.Title.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Title);
			}
			if (this.Content.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Content);
			}
			num += this.rewards_.CalculateSize(MailDto._repeated_rewards_codec);
			if (this.RewardsState)
			{
				num += 2;
			}
			if (this.CreateTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.CreateTimestamp);
			}
			if (this.MailType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MailType);
			}
			if (this.LanguageTitleId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LanguageTitleId);
			}
			if (this.LanguageContentId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LanguageContentId);
			}
			if (this.LanguageContentParams.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LanguageContentParams);
			}
			if (this.ExpiredTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ExpiredTimestamp);
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
						if (num == 8U)
						{
							this.RowId = input.ReadUInt64();
							continue;
						}
						if (num == 18U)
						{
							this.Title = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.Content = input.ReadString();
							continue;
						}
						if (num == 34U)
						{
							this.rewards_.AddEntriesFrom(input, MailDto._repeated_rewards_codec);
							continue;
						}
						if (num == 40U)
						{
							this.RewardsState = input.ReadBool();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 48U)
					{
						this.CreateTimestamp = input.ReadUInt64();
						continue;
					}
					if (num == 56U)
					{
						this.MailType = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.LanguageTitleId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.LanguageContentId = input.ReadUInt32();
						continue;
					}
					if (num == 82U)
					{
						this.LanguageContentParams = input.ReadString();
						continue;
					}
					if (num == 88U)
					{
						this.ExpiredTimestamp = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MailDto> _parser = new MessageParser<MailDto>(() => new MailDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int TitleFieldNumber = 2;

		private string title_ = "";

		public const int ContentFieldNumber = 3;

		private string content_ = "";

		public const int RewardsFieldNumber = 4;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(34U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();

		public const int RewardsStateFieldNumber = 5;

		private bool rewardsState_;

		public const int CreateTimestampFieldNumber = 6;

		private ulong createTimestamp_;

		public const int MailTypeFieldNumber = 7;

		private uint mailType_;

		public const int LanguageTitleIdFieldNumber = 8;

		private uint languageTitleId_;

		public const int LanguageContentIdFieldNumber = 9;

		private uint languageContentId_;

		public const int LanguageContentParamsFieldNumber = 10;

		private string languageContentParams_ = "";

		public const int ExpiredTimestampFieldNumber = 11;

		private ulong expiredTimestamp_;
	}
}
