using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CommonParams : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CommonParams> Parser
		{
			get
			{
				return CommonParams._parser;
			}
		}

		[DebuggerNonUserCode]
		public string AccountId
		{
			get
			{
				return this.accountId_;
			}
			set
			{
				this.accountId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint Version
		{
			get
			{
				return this.version_;
			}
			set
			{
				this.version_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string DeviceId
		{
			get
			{
				return this.deviceId_;
			}
			set
			{
				this.deviceId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AccessToken
		{
			get
			{
				return this.accessToken_;
			}
			set
			{
				this.accessToken_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong TransId
		{
			get
			{
				return this.transId_;
			}
			set
			{
				this.transId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ServerId
		{
			get
			{
				return this.serverId_;
			}
			set
			{
				this.serverId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string LanguageMark
		{
			get
			{
				return this.languageMark_;
			}
			set
			{
				this.languageMark_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint ClientVersion
		{
			get
			{
				return this.clientVersion_;
			}
			set
			{
				this.clientVersion_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AbVersion
		{
			get
			{
				return this.abVersion_;
			}
			set
			{
				this.abVersion_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string PackageVersion
		{
			get
			{
				return this.packageVersion_;
			}
			set
			{
				this.packageVersion_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int HotFixVersion
		{
			get
			{
				return this.hotFixVersion_;
			}
			set
			{
				this.hotFixVersion_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.AccountId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AccountId);
			}
			if (this.Version != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Version);
			}
			if (this.DeviceId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.DeviceId);
			}
			if (this.AccessToken.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.AccessToken);
			}
			if (this.TransId != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.TransId);
			}
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.ServerId);
			}
			if (this.LanguageMark.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.LanguageMark);
			}
			if (this.ClientVersion != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.ClientVersion);
			}
			if (this.AbVersion != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.AbVersion);
			}
			if (this.PackageVersion.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.PackageVersion);
			}
			if (this.HotFixVersion != 0)
			{
				output.WriteRawTag(88);
				output.WriteInt32(this.HotFixVersion);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AccountId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccountId);
			}
			if (this.Version != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Version);
			}
			if (this.DeviceId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.DeviceId);
			}
			if (this.AccessToken.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccessToken);
			}
			if (this.TransId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TransId);
			}
			if (this.ServerId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.LanguageMark.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LanguageMark);
			}
			if (this.ClientVersion != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ClientVersion);
			}
			if (this.AbVersion != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AbVersion);
			}
			if (this.PackageVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.PackageVersion);
			}
			if (this.HotFixVersion != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.HotFixVersion);
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
							this.AccountId = input.ReadString();
							continue;
						}
						if (num == 16U)
						{
							this.Version = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.DeviceId = input.ReadString();
							continue;
						}
						if (num == 34U)
						{
							this.AccessToken = input.ReadString();
							continue;
						}
						if (num == 40U)
						{
							this.TransId = input.ReadUInt64();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 48U)
					{
						this.ServerId = input.ReadUInt32();
						continue;
					}
					if (num == 58U)
					{
						this.LanguageMark = input.ReadString();
						continue;
					}
					if (num == 64U)
					{
						this.ClientVersion = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.AbVersion = input.ReadUInt32();
						continue;
					}
					if (num == 82U)
					{
						this.PackageVersion = input.ReadString();
						continue;
					}
					if (num == 88U)
					{
						this.HotFixVersion = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CommonParams> _parser = new MessageParser<CommonParams>(() => new CommonParams());

		public const int AccountIdFieldNumber = 1;

		private string accountId_ = "";

		public const int VersionFieldNumber = 2;

		private uint version_;

		public const int DeviceIdFieldNumber = 3;

		private string deviceId_ = "";

		public const int AccessTokenFieldNumber = 4;

		private string accessToken_ = "";

		public const int TransIdFieldNumber = 5;

		private ulong transId_;

		public const int ServerIdFieldNumber = 6;

		private uint serverId_;

		public const int LanguageMarkFieldNumber = 7;

		private string languageMark_ = "";

		public const int ClientVersionFieldNumber = 8;

		private uint clientVersion_;

		public const int AbVersionFieldNumber = 9;

		private uint abVersion_;

		public const int PackageVersionFieldNumber = 10;

		private string packageVersion_ = "";

		public const int HotFixVersionFieldNumber = 11;

		private int hotFixVersion_;
	}
}
