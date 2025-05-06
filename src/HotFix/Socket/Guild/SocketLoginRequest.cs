using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Socket.Guild
{
	public sealed class SocketLoginRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketLoginRequest> Parser
		{
			get
			{
				return SocketLoginRequest._parser;
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
			if (this.AccessToken.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AccessToken);
			}
			this.groupInfo_.WriteTo(output, SocketLoginRequest._map_groupInfo_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AccessToken.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccessToken);
			}
			return num + this.groupInfo_.CalculateSize(SocketLoginRequest._map_groupInfo_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.groupInfo_.AddEntriesFrom(input, SocketLoginRequest._map_groupInfo_codec);
					}
				}
				else
				{
					this.AccessToken = input.ReadString();
				}
			}
		}

		private static readonly MessageParser<SocketLoginRequest> _parser = new MessageParser<SocketLoginRequest>(() => new SocketLoginRequest());

		public const int AccessTokenFieldNumber = 1;

		private string accessToken_ = "";

		public const int GroupInfoFieldNumber = 2;

		private static readonly MapField<uint, string>.Codec _map_groupInfo_codec = new MapField<uint, string>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForString(18U), 18U);

		private readonly MapField<uint, string> groupInfo_ = new MapField<uint, string>();
	}
}
