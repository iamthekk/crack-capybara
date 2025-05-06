using System;

namespace Server
{
	public class IDCreator
	{
		public IDCreator(int number = 100)
		{
			this.m_number = number;
		}

		public int GetID()
		{
			int number = this.m_number;
			this.m_number = number + 1;
			return number;
		}

		private int m_number;
	}
}
