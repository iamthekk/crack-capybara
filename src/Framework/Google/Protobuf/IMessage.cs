using System;

namespace Google.Protobuf
{
	public interface IMessage
	{
		void MergeFrom(CodedInputStream input);

		void WriteTo(CodedOutputStream output);

		int CalculateSize();
	}
}
