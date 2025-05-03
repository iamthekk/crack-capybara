using System;
using UnityEngine;

namespace Framework.SocketNet
{
	public class SocketNetMessageQueue : MonoBehaviour
	{
		private void Update()
		{
			Action onUpdate = this.OnUpdate;
			if (onUpdate == null)
			{
				return;
			}
			onUpdate();
		}

		public Action OnUpdate;
	}
}
