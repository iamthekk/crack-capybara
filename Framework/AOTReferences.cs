using System;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Framework
{
	public class AOTReferences
	{
		public void Reference()
		{
			new RepeatedField<long>();
			new RepeatedField<ulong>();
			new RepeatedField<int>();
			new RepeatedField<uint>();
			new RepeatedField<float>();
			new RepeatedField<string>();
			new RepeatedField<IMessage>();
			new MapField<long, long>();
			new MapField<long, ulong>();
			new MapField<long, int>();
			new MapField<long, uint>();
			new MapField<long, float>();
			new MapField<long, string>();
			new MapField<long, IMessage>();
			new MapField<ulong, long>();
			new MapField<ulong, ulong>();
			new MapField<ulong, int>();
			new MapField<ulong, uint>();
			new MapField<ulong, float>();
			new MapField<ulong, string>();
			new MapField<ulong, IMessage>();
			new MapField<int, long>();
			new MapField<int, ulong>();
			new MapField<int, int>();
			new MapField<int, uint>();
			new MapField<int, float>();
			new MapField<int, string>();
			new MapField<int, IMessage>();
			new MapField<uint, long>();
			new MapField<uint, ulong>();
			new MapField<uint, int>();
			new MapField<uint, uint>();
			new MapField<uint, float>();
			new MapField<uint, string>();
			new MapField<uint, IMessage>();
			new MapField<float, long>();
			new MapField<float, ulong>();
			new MapField<float, int>();
			new MapField<float, uint>();
			new MapField<float, float>();
			new MapField<float, string>();
			new MapField<float, IMessage>();
			new MapField<string, long>();
			new MapField<string, ulong>();
			new MapField<string, int>();
			new MapField<string, uint>();
			new MapField<string, float>();
			new MapField<string, string>();
			new MapField<string, IMessage>();
			new MapField<IMessage, long>();
			new MapField<IMessage, ulong>();
			new MapField<IMessage, int>();
			new MapField<IMessage, uint>();
			new MapField<IMessage, float>();
			new MapField<IMessage, string>();
			new MapField<IMessage, IMessage>();
		}
	}
}
