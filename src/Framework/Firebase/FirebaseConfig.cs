using System;
using UnityEngine;

namespace Firebase
{
	public class FirebaseConfig : MonoBehaviour
	{
		[InspectorName("Show debug button")]
		public bool EnableDebug;

		[InspectorName("LogLevel")]
		public LogLevel LogLevel = 1;
	}
}
