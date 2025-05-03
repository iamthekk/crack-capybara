using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildLogDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildLogDto> Parser
		{
			get
			{
				return GuildLogDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong TimeStamp
		{
			get
			{
				return this.timeStamp_;
			}
			set
			{
				this.timeStamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LogType
		{
			get
			{
				return this.logType_;
			}
			set
			{
				this.logType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<string> LogParam
		{
			get
			{
				return this.logParam_;
			}
		}

		[DebuggerNonUserCode]
		public string ServerName
		{
			get
			{
				return this.serverName_;
			}
			set
			{
				this.serverName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TimeStamp != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.TimeStamp);
			}
			if (this.LogType != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.LogType);
			}
			this.logParam_.WriteTo(output, GuildLogDto._repeated_logParam_codec);
			if (this.ServerName.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ServerName);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TimeStamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TimeStamp);
			}
			if (this.LogType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LogType);
			}
			num += this.logParam_.CalculateSize(GuildLogDto._repeated_logParam_codec);
			if (this.ServerName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ServerName);
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
						this.TimeStamp = input.ReadUInt64();
						continue;
					}
					if (num == 16U)
					{
						this.LogType = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.logParam_.AddEntriesFrom(input, GuildLogDto._repeated_logParam_codec);
						continue;
					}
					if (num == 34U)
					{
						this.ServerName = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildLogDto> _parser = new MessageParser<GuildLogDto>(() => new GuildLogDto());

		public const int TimeStampFieldNumber = 1;

		private ulong timeStamp_;

		public const int LogTypeFieldNumber = 2;

		private int logType_;

		public const int LogParamFieldNumber = 3;

		private static readonly FieldCodec<string> _repeated_logParam_codec = FieldCodec.ForString(26U);

		private readonly RepeatedField<string> logParam_ = new RepeatedField<string>();

		public const int ServerNameFieldNumber = 4;

		private string serverName_ = "";
	}
}
