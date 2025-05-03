using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.ServerList
{
	public sealed class ServerGroupDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ServerGroupDto> Parser
		{
			get
			{
				return ServerGroupDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Group
		{
			get
			{
				return this.group_;
			}
			set
			{
				this.group_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint StartServer
		{
			get
			{
				return this.startServer_;
			}
			set
			{
				this.startServer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint EndServer
		{
			get
			{
				return this.endServer_;
			}
			set
			{
				this.endServer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ServerInfoDto> ServerInfoDto
		{
			get
			{
				return this.serverInfoDto_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Group != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Group);
			}
			if (this.StartServer != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.StartServer);
			}
			if (this.EndServer != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.EndServer);
			}
			this.serverInfoDto_.WriteTo(output, ServerGroupDto._repeated_serverInfoDto_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Group != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Group);
			}
			if (this.StartServer != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.StartServer);
			}
			if (this.EndServer != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EndServer);
			}
			return num + this.serverInfoDto_.CalculateSize(ServerGroupDto._repeated_serverInfoDto_codec);
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
						this.Group = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.StartServer = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.EndServer = input.ReadUInt32();
						continue;
					}
					if (num == 34U)
					{
						this.serverInfoDto_.AddEntriesFrom(input, ServerGroupDto._repeated_serverInfoDto_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ServerGroupDto> _parser = new MessageParser<ServerGroupDto>(() => new ServerGroupDto());

		public const int GroupFieldNumber = 1;

		private uint group_;

		public const int StartServerFieldNumber = 2;

		private uint startServer_;

		public const int EndServerFieldNumber = 3;

		private uint endServer_;

		public const int ServerInfoDtoFieldNumber = 4;

		private static readonly FieldCodec<ServerInfoDto> _repeated_serverInfoDto_codec = FieldCodec.ForMessage<ServerInfoDto>(34U, Proto.ServerList.ServerInfoDto.Parser);

		private readonly RepeatedField<ServerInfoDto> serverInfoDto_ = new RepeatedField<ServerInfoDto>();
	}
}
