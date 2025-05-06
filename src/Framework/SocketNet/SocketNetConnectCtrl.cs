using System;
using UnityEngine;

namespace Framework.SocketNet
{
	public class SocketNetConnectCtrl : MonoBehaviour
	{
		private void OnApplicationPause(bool pause)
		{
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				SocketNet bindSocketNet = this.BindSocketNet;
				if (bindSocketNet == null)
				{
					return;
				}
				bindSocketNet.CheckReconnect("OnApplicationFocus");
			}
		}

		private void OnApplicationQuit()
		{
		}

		public SocketNet BindSocketNet;
	}
}
