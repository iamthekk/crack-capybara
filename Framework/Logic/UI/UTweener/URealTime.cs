using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	public class URealTime : MonoBehaviour
	{
		public static float time
		{
			get
			{
				return Time.unscaledTime;
			}
		}

		public static float deltaTime
		{
			get
			{
				return Time.unscaledDeltaTime;
			}
		}
	}
}
