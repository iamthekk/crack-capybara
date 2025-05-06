using System;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgsBindCamera : BaseEventArgs
	{
		public override void Clear()
		{
			this.m_camera = null;
		}

		public Camera m_camera;
	}
}
