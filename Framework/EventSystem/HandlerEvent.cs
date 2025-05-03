using System;

namespace Framework.EventSystem
{
	public delegate void HandlerEvent(object sender, int type, BaseEventArgs eventArgs = null);
}
