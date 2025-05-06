using System;
using UnityEngine;

namespace Framework.Logic.Platform
{
	public class PlatformHelper : Singleton<PlatformHelper>
	{
		public bool IsFringe()
		{
			return this.IsFringeScreen();
		}

		private bool IsFringeScreen()
		{
			Rect safeArea = Screen.safeArea;
			return (float)Screen.height - safeArea.height - safeArea.y > 0f;
		}

		public float GetTopHeight()
		{
			if (this.IsFringe())
			{
				return -110f;
			}
			return 0f;
		}

		public float GetBottomHeight()
		{
			if (this.IsFringe())
			{
				return 50f;
			}
			return 0f;
		}

		public int GetKeyboardHeight()
		{
			if (Application.platform == 11)
			{
				return this.AndroidGetKeyboardHeight(true);
			}
			if (Application.platform == 8)
			{
				return this.IOSGetKeyboardHeight();
			}
			return 0;
		}

		public int AndroidGetKeyboardHeight(bool includeInput = true)
		{
			int num;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getView", Array.Empty<object>());
				AndroidJavaObject androidJavaObject3 = androidJavaObject.Get<AndroidJavaObject>("mSoftInputDialog");
				if (androidJavaObject2 == null || androidJavaObject3 == null)
				{
					num = 0;
				}
				else
				{
					int num2 = 0;
					if (includeInput)
					{
						AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getWindow", Array.Empty<object>()).Call<AndroidJavaObject>("getDecorView", Array.Empty<object>());
						if (androidJavaObject4 != null)
						{
							num2 = androidJavaObject4.Call<int>("getHeight", Array.Empty<object>());
						}
					}
					using (AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("android.graphics.Rect", Array.Empty<object>()))
					{
						androidJavaObject2.Call("getWindowVisibleDisplayFrame", new object[] { androidJavaObject5 });
						num = Display.main.systemHeight - androidJavaObject5.Call<int>("height", Array.Empty<object>()) + num2;
					}
				}
			}
			return num;
		}

		public int IOSGetKeyboardHeight()
		{
			return (int)TouchScreenKeyboard.area.height;
		}

		public bool IsEditor()
		{
			return false;
		}

		public bool IsAndroid()
		{
			return true;
		}

		public bool IsIOS()
		{
			return false;
		}

		public int GetQualityByDevice()
		{
			int systemMemorySize = this.GetSystemMemorySize();
			int num;
			if (systemMemorySize < 2480)
			{
				num = 1;
			}
			else if (systemMemorySize <= 4960)
			{
				num = 2;
			}
			else
			{
				num = 3;
			}
			return num;
		}

		private bool GetFlagShipForIOS()
		{
			string[] array = SystemInfo.deviceModel.Split(',', StringSplitOptions.None);
			if (array.Length != 0 && array[0].Contains("iPhone"))
			{
				string text = array[0].Replace("iPhone", "");
				int num = -1;
				int num2 = -1;
				int.TryParse(text, out num);
				if (array.Length > 1)
				{
					int.TryParse(array[1], out num2);
				}
				if (num == 10)
				{
					if (num2 == 3 || num2 == 6)
					{
						return true;
					}
				}
				else if (num > 10)
				{
					return true;
				}
			}
			return false;
		}

		private int GetIOSDeviceLevel()
		{
			string[] array = SystemInfo.deviceModel.Split(',', StringSplitOptions.None);
			if (array.Length != 0 && array[0].Contains("iPhone"))
			{
				string text = array[0].Replace("iPhone", "");
				int num = -1;
				int num2 = -1;
				int.TryParse(text, out num);
				if (array.Length > 1)
				{
					int.TryParse(array[1], out num2);
				}
				if (num < 10)
				{
					return 1;
				}
				if (num == 10)
				{
					if (num2 == 3 || num2 == 6)
					{
						return 2;
					}
					return 1;
				}
				else
				{
					if (num > 10 && num < 13)
					{
						return 2;
					}
					if (num >= 13)
					{
						return 3;
					}
				}
			}
			if (array.Length != 0 && array[0].Contains("iPad"))
			{
				string text2 = array[0].Replace("iPad", "");
				int num3 = -1;
				int.TryParse(text2, out num3);
				if (num3 < 5)
				{
					return 1;
				}
				if (num3 >= 5 && num3 < 12)
				{
					return 2;
				}
				if (num3 >= 12)
				{
					return 3;
				}
			}
			return 2;
		}

		public string GetDeviceModel()
		{
			return SystemInfo.deviceModel;
		}

		public int GetSystemMemorySize()
		{
			return SystemInfo.systemMemorySize;
		}

		public string GetOperationSystem()
		{
			return SystemInfo.operatingSystem;
		}

		public string GetGraphicsDeviceName()
		{
			return SystemInfo.graphicsDeviceName;
		}

		public bool GetIsRoot()
		{
			return false;
		}

		public string GetAppVersion()
		{
			return GameApp.Config.GetString("Version");
		}

		public string GetAppVersionCode()
		{
			return GameApp.Config.GetString("VersionCode");
		}

		public string GetPlatformID()
		{
			return Application.platform.ToString();
		}

		public int GetPlatformIndex()
		{
			return 2;
		}

		public int GetChannelIndex()
		{
			string @string = GameApp.Config.GetString("ChannelName");
			if (string.IsNullOrEmpty(@string))
			{
				return 0;
			}
			int num = 0;
			if (!(@string == "AppStore"))
			{
				if (!(@string == "GoogleStore"))
				{
					if (!(@string == "AppStoreChina"))
					{
						if (@string == "WeChatMiniGame")
						{
							num = 2000;
						}
					}
					else
					{
						num = 1000;
					}
				}
				else
				{
					num = 2;
				}
			}
			else
			{
				num = 1;
			}
			return num;
		}

		public static string GetUUID()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public static string GetSignatureHash()
		{
			string text = string.Empty;
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>());
					string text2 = @static.Call<string>("getPackageName", Array.Empty<object>());
					AndroidJavaObject[] array = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { text2, 64 }).Get<AndroidJavaObject[]>("signatures");
					if (array.Length != 0)
					{
						byte[] array2 = array[0].Call<byte[]>("toByteArray", Array.Empty<object>());
						using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("java.security.MessageDigest"))
						{
							AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("getInstance", new object[] { "SHA-256" });
							androidJavaObject2.Call<byte>("update", array2);
							text = BitConverter.ToString(androidJavaObject2.Call<byte[]>("digest", Array.Empty<object>())).Replace("-", "").ToLower();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log(string.Format("Failed to get signature hash: {0}", ex));
			}
			return text;
		}

		private const float TopHeight = -110f;

		private const float BottomHeight = 50f;
	}
}
