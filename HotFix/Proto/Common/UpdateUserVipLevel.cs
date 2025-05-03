using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UpdateUserVipLevel : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateUserVipLevel> Parser
		{
			get
			{
				return UpdateUserVipLevel._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsChange
		{
			get
			{
				return this.isChange_;
			}
			set
			{
				this.isChange_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserVipLevel UserVipLevel
		{
			get
			{
				return this.userVipLevel_;
			}
			set
			{
				this.userVipLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsChange)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsChange);
			}
			if (this.userVipLevel_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserVipLevel);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsChange)
			{
				num += 2;
			}
			if (this.userVipLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserVipLevel);
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
						input.SkipLastField();
					}
					else
					{
						if (this.userVipLevel_ == null)
						{
							this.userVipLevel_ = new UserVipLevel();
						}
						input.ReadMessage(this.userVipLevel_);
					}
				}
				else
				{
					this.IsChange = input.ReadBool();
				}
			}
		}

		private static readonly MessageParser<UpdateUserVipLevel> _parser = new MessageParser<UpdateUserVipLevel>(() => new UpdateUserVipLevel());

		public const int IsChangeFieldNumber = 1;

		private bool isChange_;

		public const int UserVipLevelFieldNumber = 2;

		private UserVipLevel userVipLevel_;
	}
}
