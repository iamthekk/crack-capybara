using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildUpdateInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildUpdateInfoDto> Parser
		{
			get
			{
				return GuildUpdateInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Active
		{
			get
			{
				return this.active_;
			}
			set
			{
				this.active_ = value;
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
		public uint MaxMembers
		{
			get
			{
				return this.maxMembers_;
			}
			set
			{
				this.maxMembers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Active != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Active);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Exp);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Level);
			}
			if (this.MaxMembers != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.MaxMembers);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Active != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Active);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.MaxMembers != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxMembers);
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
						this.Active = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.MaxMembers = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildUpdateInfoDto> _parser = new MessageParser<GuildUpdateInfoDto>(() => new GuildUpdateInfoDto());

		public const int ActiveFieldNumber = 1;

		private uint active_;

		public const int ExpFieldNumber = 2;

		private uint exp_;

		public const int LevelFieldNumber = 3;

		private uint level_;

		public const int MaxMembersFieldNumber = 4;

		private uint maxMembers_;
	}
}
