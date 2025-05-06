using System;
using UnityEngine;

namespace Framework
{
	[CreateAssetMenu]
	public class NetWorkConfig : ScriptableObject
	{
		public string m_debugURL = "";

		public string m_preURL = "";

		public string m_releaseURL = "";

		public string m_debugSocketNetUrl = "";

		public string m_releaseSocketNetUrl = "";
	}
}
