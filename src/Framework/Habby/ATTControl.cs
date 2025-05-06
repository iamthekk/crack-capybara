using System;
using System.Runtime.InteropServices;
using AOT;

namespace Habby
{
	public class ATTControl
	{
		public static bool AuthorizationAvailable
		{
			get
			{
				return false;
			}
		}

		public static bool IsAdvertisingTrackingEnabled()
		{
			return true;
		}

		public static ATTStatus GetAuthorizationStatus(bool compatible = true)
		{
			return ATTStatus.Authorized;
		}

		[MonoPInvokeCallback(typeof(ATTControl.CallbackHandler))]
		private static void Callback(ATTStatus status, string idfa)
		{
			ATTControl._callback(status, idfa);
			ATTControl._callback = null;
		}

		public static void RequestAdvertisingIdentifier(bool compatible, ATTControl.CallbackHandler callback)
		{
			ATTControl._callback = callback;
			ATTControl.Callback(ATTStatus.Authorized, "00000000-0000-0000-0000-000000000000");
		}

		public static void RequestAdvertisingIdentifier(ATTControl.CallbackHandler callback)
		{
			ATTControl.RequestAdvertisingIdentifier(true, callback);
		}

		private static ATTControl.CallbackHandler _callback;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void CallbackHandler(ATTStatus status, string idfa);
	}
}
