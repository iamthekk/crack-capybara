using System;

namespace Framework.RunTimeManager
{
	public class RunTimeIDConnecterData
	{
		public RunTimeIDConnecterData(int id, object obj, Type type)
		{
			this.m_id = id;
			this.m_obj = obj;
			this.m_type = type;
		}

		public int m_id;

		public object m_obj;

		public Type m_type;
	}
}
