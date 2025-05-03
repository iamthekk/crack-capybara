using System;
using UnityEngine;

namespace Framework.TableModule
{
	public class TableManager : MonoBehaviour
	{
		public void SetITableManager(ITableManager manager)
		{
			this.m_manager = manager;
		}

		public ITableManager GetITableManager()
		{
			return this.m_manager;
		}

		private ITableManager m_manager;
	}
}
