using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class UserStatisticInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserStatisticInfo> Parser
		{
			get
			{
				return UserStatisticInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ulong> DataMap
		{
			get
			{
				return this.dataMap_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.dataMap_.WriteTo(output, UserStatisticInfo._map_dataMap_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.dataMap_.CalculateSize(UserStatisticInfo._map_dataMap_codec);
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
					this.dataMap_.AddEntriesFrom(input, UserStatisticInfo._map_dataMap_codec);
				}
			}
		}

		private static readonly MessageParser<UserStatisticInfo> _parser = new MessageParser<UserStatisticInfo>(() => new UserStatisticInfo());

		public const int DataMapFieldNumber = 1;

		private static readonly MapField<uint, ulong>.Codec _map_dataMap_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 10U);

		private readonly MapField<uint, ulong> dataMap_ = new MapField<uint, ulong>();
	}
}
