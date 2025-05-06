using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Socket.Guild
{
	public sealed class SocketJoinGroupRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketJoinGroupRequest> Parser
		{
			get
			{
				return SocketJoinGroupRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, string> GroupInfo
		{
			get
			{
				return this.groupInfo_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.groupInfo_.WriteTo(output, SocketJoinGroupRequest._map_groupInfo_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.groupInfo_.CalculateSize(SocketJoinGroupRequest._map_groupInfo_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					this.groupInfo_.AddEntriesFrom(input, SocketJoinGroupRequest._map_groupInfo_codec);
				}
			}
		}

		private static readonly MessageParser<SocketJoinGroupRequest> _parser = new MessageParser<SocketJoinGroupRequest>(() => new SocketJoinGroupRequest());

		public const int GroupInfoFieldNumber = 1;

		private static readonly MapField<uint, string>.Codec _map_groupInfo_codec = new MapField<uint, string>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForString(18U), 10U);

		private readonly MapField<uint, string> groupInfo_ = new MapField<uint, string>();
	}
}
