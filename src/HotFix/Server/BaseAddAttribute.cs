using System;
using LocalModels;

namespace Server
{
	public abstract class BaseAddAttribute
	{
		public BaseAddAttribute(LocalModelManager tableManager)
		{
			this.m_tableManager = tableManager;
		}

		public abstract AddAttributeData MathAll();

		public LocalModelManager m_tableManager;
	}
}
