using System;

namespace Framework.Logic.Tools
{
	public class FrameworkTaskOutValue<T> where T : class
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
