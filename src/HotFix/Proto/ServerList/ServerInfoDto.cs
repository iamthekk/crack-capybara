using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.ServerList
{
	public sealed class ServerInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ServerInfoDto> Parser
		{
			get
			{
				return ServerInfoDto._parser;
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
		public ulong Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ServerId);
			}
			if (this.Status != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.Status);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ServerId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.Status != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Status);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Status = input.ReadUInt64();
					}
				}
				else
				{
					this.ServerId = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<ServerInfoDto> _parser = new MessageParser<ServerInfoDto>(() => new ServerInfoDto());

		public const int ServerIdFieldNumber = 1;

		private uint serverId_;

		public const int StatusFieldNumber = 2;

		private ulong status_;
	}
}
