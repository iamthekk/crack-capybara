using System;

namespace LitJson
{
	public enum JsonToken
	{
		None,
		ObjectStart,
		PropertyName,
		ObjectEnd,
		ArrayStart,
		ArrayEnd,
		Int,
		Long,
		Double,
		Fp,
		String,
		Boolean,
		Null
	}
}
