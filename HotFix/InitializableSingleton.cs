using System;

namespace HotFix
{
	public class InitializableSingleton<T> where T : class, IInitializable, new()
	{
		public static T Instance
		{
			get
			{
				if (InitializableSingleton<T>.m_t == null)
				{
					InitializableSingleton<T>.m_t = new T();
					InitializableSingleton<T>.m_t.OnSingletonInit();
				}
				return InitializableSingleton<T>.m_t;
			}
		}

		private static T m_t;
	}
}
