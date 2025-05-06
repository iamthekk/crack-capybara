using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class CollectionInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CollectionInfo> Parser
		{
			get
			{
				return CollectionInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CollectionDto> CollectionList
		{
			get
			{
				return this.collectionList_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> DataMap
		{
			get
			{
				return this.dataMap_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.collectionList_.WriteTo(output, CollectionInfo._repeated_collectionList_codec);
			this.dataMap_.WriteTo(output, CollectionInfo._map_dataMap_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.collectionList_.CalculateSize(CollectionInfo._repeated_collectionList_codec) + this.dataMap_.CalculateSize(CollectionInfo._map_dataMap_codec);
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
						this.dataMap_.AddEntriesFrom(input, CollectionInfo._map_dataMap_codec);
					}
				}
				else
				{
					this.collectionList_.AddEntriesFrom(input, CollectionInfo._repeated_collectionList_codec);
				}
			}
		}

		private static readonly MessageParser<CollectionInfo> _parser = new MessageParser<CollectionInfo>(() => new CollectionInfo());

		public const int CollectionListFieldNumber = 1;

		private static readonly FieldCodec<CollectionDto> _repeated_collectionList_codec = FieldCodec.ForMessage<CollectionDto>(10U, CollectionDto.Parser);

		private readonly RepeatedField<CollectionDto> collectionList_ = new RepeatedField<CollectionDto>();

		public const int DataMapFieldNumber = 2;

		private static readonly MapField<uint, uint>.Codec _map_dataMap_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 18U);

		private readonly MapField<uint, uint> dataMap_ = new MapField<uint, uint>();
	}
}
