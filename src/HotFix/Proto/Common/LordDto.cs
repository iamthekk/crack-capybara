using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class LordDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LordDto> Parser
		{
			get
			{
				return LordDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long LordUid
		{
			get
			{
				return this.lordUid_;
			}
			set
			{
				this.lordUid_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LordAvatar
		{
			get
			{
				return this.lordAvatar_;
			}
			set
			{
				this.lordAvatar_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LordAvatarFrame
		{
			get
			{
				return this.lordAvatarFrame_;
			}
			set
			{
				this.lordAvatarFrame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string LordNickName
		{
			get
			{
				return this.lordNickName_;
			}
			set
			{
				this.lordNickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.LordUid != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.LordUid);
			}
			if (this.LordAvatar != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.LordAvatar);
			}
			if (this.LordAvatarFrame != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.LordAvatarFrame);
			}
			if (this.LordNickName.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.LordNickName);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.LordUid != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LordUid);
			}
			if (this.LordAvatar != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LordAvatar);
			}
			if (this.LordAvatarFrame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LordAvatarFrame);
			}
			if (this.LordNickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LordNickName);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.LordUid = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.LordAvatar = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.LordAvatarFrame = input.ReadInt32();
						continue;
					}
					if (num == 34U)
					{
						this.LordNickName = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<LordDto> _parser = new MessageParser<LordDto>(() => new LordDto());

		public const int LordUidFieldNumber = 1;

		private long lordUid_;

		public const int LordAvatarFieldNumber = 2;

		private int lordAvatar_;

		public const int LordAvatarFrameFieldNumber = 3;

		private int lordAvatarFrame_;

		public const int LordNickNameFieldNumber = 4;

		private string lordNickName_ = "";
	}
}
