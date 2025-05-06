using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.ServerList
{
	public sealed class ZoneInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ZoneInfoDto> Parser
		{
			get
			{
				return ZoneInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxServer
		{
			get
			{
				return this.maxServer_;
			}
			set
			{
				this.maxServer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ServerGroupDto> ServerList
		{
			get
			{
				return this.serverList_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.MaxServer != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.MaxServer);
			}
			this.serverList_.WriteTo(output, ZoneInfoDto._map_serverList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.MaxServer != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxServer);
			}
			return num + this.serverList_.CalculateSize(ZoneInfoDto._map_serverList_codec);
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
						this.serverList_.AddEntriesFrom(input, ZoneInfoDto._map_serverList_codec);
					}
				}
				else
				{
					this.MaxServer = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<ZoneInfoDto> _parser = new MessageParser<ZoneInfoDto>(() => new ZoneInfoDto());

		public const int MaxServerFieldNumber = 1;

		private uint maxServer_;

		public const int ServerListFieldNumber = 2;

		private static readonly MapField<uint, ServerGroupDto>.Codec _map_serverList_codec = new MapField<uint, ServerGroupDto>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<ServerGroupDto>(18U, ServerGroupDto.Parser), 18U);

		private readonly MapField<uint, ServerGroupDto> serverList_ = new MapField<uint, ServerGroupDto>();
	}
}
