using System;

namespace HotFix
{
	public class TaskOutValue<T> where T : class
	{
		public T Value
		{
			get
			{
				return this.m_t;
			}
		}

		public void SetValue(T t)
		{
			this.m_t = t;
		}

		public void Release()
		{
			this.m_t = default(T);
		}

		private T m_t;
	}
}
