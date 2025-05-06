using System;
using LitJson;
using Newtonsoft.Json;

namespace Server
{
	public class JsonManager
	{
		public static T ToObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static string SerializeObject(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static T LitJson_ToObject<T>(string json)
		{
			return JsonMapper.ToObject<T>(json);
		}

		public static T LitJson_ToObjectFp<T>(string json)
		{
			return JsonMapper.ToObjectFp<T>(json);
		}

		public static string LitJson_SerializeObject(object obj)
		{
			return JsonMapper.ToJson(obj);
		}
	}
}
