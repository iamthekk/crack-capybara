using System;

namespace HotFix
{
	public class Singleton<T> where T : class, new()
	{
		public static T Instance
		{
			get
			{
				if (Singleton<T>.instance == null)
				{
					Singleton<T>.instance = new T();
				}
				return Singleton<T>.instance;
			}
		}

		private static T instance;
	}
}
