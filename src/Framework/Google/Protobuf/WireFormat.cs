using System;

namespace Google.Protobuf
{
	public static class WireFormat
	{
		public static WireFormat.WireType GetTagWireType(uint tag)
		{
			return (WireFormat.WireType)(tag & 7U);
		}

		public static int GetTagFieldNumber(uint tag)
		{
			return (int)tag >> 3;
		}

		public static uint MakeTag(int fieldNumber, WireFormat.WireType wireType)
		{
			return (uint)((fieldNumber << 3) | (int)wireType);
		}

		private const int TagTypeBits = 3;

		private const uint TagTypeMask = 7U;

		public enum WireType : uint
		{
			Varint,
			Fixed64,
			LengthDelimited,
			StartGroup,
			EndGroup,
			Fixed32
		}
	}
}
