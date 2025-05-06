using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Framework.Logic.UI.UIAtlas;
using Framework.Platfrom;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Logic
{
	public static class Utility
	{
		public static string GetFourPaddingString(this RectTransform t)
		{
			return string.Format("Left:{0},Right:{1},Top:{2},Bottom:{3}", new object[]
			{
				t.offsetMin.x,
				t.offsetMax.x,
				-t.offsetMax.y,
				t.offsetMin.y
			});
		}

		public static void SetLeft(this RectTransform t, float value)
		{
			t.offsetMin = new Vector2(value, t.offsetMin.y);
		}

		public static void SetRight(this RectTransform t, float value)
		{
			t.offsetMax = new Vector2(-value, t.offsetMax.y);
		}

		public static void SetTop(this RectTransform t, float value)
		{
			t.offsetMax = new Vector2(t.offsetMax.x, -value);
		}

		public static void SetBottom(this RectTransform t, float value)
		{
			t.offsetMin = new Vector2(t.offsetMin.x, value);
		}

		public static void SetFourPadding(this RectTransform t, float left, float right, float top, float bottom)
		{
			t.offsetMin = new Vector2(left, bottom);
			t.offsetMax = new Vector2(right, -top);
		}

		public static float GetTop(this RectTransform t)
		{
			return -t.offsetMax.y;
		}

		public static float GetBottom(this RectTransform t)
		{
			return t.offsetMin.y;
		}

		public static float GetLeft(this RectTransform t)
		{
			return t.offsetMin.x;
		}

		public static float GetRight(this RectTransform t)
		{
			return -t.offsetMax.x;
		}

		public static string GetSecond2String(int second)
		{
			return string.Format("{0:D2}:{1:D2}", second / 60, second % 60);
		}

		public static class File
		{
			public static string ReadAllText(string path)
			{
				string text = string.Empty;
				try
				{
					if (global::System.IO.File.Exists(path))
					{
						text = global::System.IO.File.ReadAllText(path);
					}
				}
				catch (Exception ex)
				{
					text = string.Empty;
					HLog.LogException(ex);
					throw;
				}
				return text;
			}

			public static byte[] ReadAllBytes(string path)
			{
				byte[] array = null;
				try
				{
					if (global::System.IO.File.Exists(path))
					{
						array = global::System.IO.File.ReadAllBytes(path);
					}
				}
				catch (Exception ex)
				{
					array = null;
					HLog.LogException(ex);
					throw;
				}
				return array;
			}

			public static void CreateDirectory(string path)
			{
				try
				{
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}

			public static void CopyDirectory(string from, string to)
			{
				try
				{
					if (!Directory.Exists(to))
					{
						Directory.CreateDirectory(to);
					}
					if (Directory.Exists(from))
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(from);
						FileInfo[] files = directoryInfo.GetFiles();
						for (int i = 0; i < files.Length; i++)
						{
							string text = Path.Combine(to, files[i].Name);
							Utility.File.DeleteFile(text);
							files[i].CopyTo(text);
						}
						DirectoryInfo[] directories = directoryInfo.GetDirectories();
						for (int j = 0; j < directories.Length; j++)
						{
							Utility.File.CopyDirectory(Path.Combine(from, directories[j].Name), Path.Combine(to, directories[j].Name));
						}
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}

			public static void CopyFile(string from, string to)
			{
				try
				{
					if (global::System.IO.File.Exists(from))
					{
						Utility.File.CreateDirectory(Path.GetDirectoryName(to));
						Utility.File.DeleteFile(to);
						new FileInfo(from).CopyTo(to, true);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void WriteAllBytes(string path, string info, Encoding enconding)
			{
				try
				{
					global::System.IO.File.WriteAllText(path, info, enconding);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void WriteAllBytes(string path, byte[] bytes)
			{
				try
				{
					global::System.IO.File.WriteAllBytes(path, bytes);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void WriteAllText(string path, string info)
			{
				try
				{
					global::System.IO.File.WriteAllText(path, info);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void WriteAllText(string path, string info, Encoding encoding)
			{
				try
				{
					global::System.IO.File.WriteAllText(path, info, encoding);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void DelectDirectory(string path)
			{
				try
				{
					if (Directory.Exists(path))
					{
						string[] fileSystemEntries = Directory.GetFileSystemEntries(path);
						for (int i = 0; i < fileSystemEntries.Length; i++)
						{
							if (global::System.IO.File.Exists(fileSystemEntries[i]))
							{
								FileInfo fileInfo = new FileInfo(fileSystemEntries[i]);
								if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
								{
									fileInfo.Attributes = FileAttributes.Normal;
								}
								fileInfo.Delete();
							}
							else
							{
								Utility.File.DelectDirectory(fileSystemEntries[i]);
							}
						}
						DirectoryInfo directoryInfo = new DirectoryInfo(path);
						if (directoryInfo.Exists)
						{
							directoryInfo.Attributes = (FileAttributes)0;
							directoryInfo.Delete(true);
						}
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void DeleteFile(string path)
			{
				try
				{
					if (global::System.IO.File.Exists(path))
					{
						global::System.IO.File.Delete(path);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void DeleteFile(string[] paths)
			{
				try
				{
					for (int i = 0; i < paths.Length; i++)
					{
						Utility.File.DeleteFile(paths[i]);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			public static void DeleteFiles(string directoryName, params string[] searchPatterns)
			{
				try
				{
					if (searchPatterns != null)
					{
						if (Directory.Exists(directoryName))
						{
							List<FileInfo> files = Utility.File.GetFiles(directoryName, searchPatterns);
							for (int i = 0; i < files.Count; i++)
							{
								files[i].Delete();
							}
						}
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					throw;
				}
			}

			private static List<FileInfo> GetFiles(string directoryName, params string[] searchPatterns)
			{
				List<FileInfo> list = new List<FileInfo>();
				DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
				for (int i = 0; i < searchPatterns.Length; i++)
				{
					list.AddRange(directoryInfo.GetFiles(searchPatterns[i]).ToList<FileInfo>());
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int j = 0; j < directories.Length; j++)
				{
					list.AddRange(Utility.File.GetFiles(directories[j].FullName, searchPatterns));
				}
				return list;
			}

			public static long GetLength(string path)
			{
				return new FileInfo(path).Length;
			}

			public static byte[] GetBytesByString(string info)
			{
				return Encoding.UTF8.GetBytes(info);
			}

			public static string GetStringByBytes(byte[] bytes)
			{
				return Encoding.UTF8.GetString(bytes);
			}

			public static string GetMD5String(byte[] bytes)
			{
				HashAlgorithm hashAlgorithm = MD5.Create();
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array = hashAlgorithm.ComputeHash(bytes);
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				return stringBuilder.ToString();
			}
		}

		public static class Math
		{
			public static int CeilToInt(float value)
			{
				return Mathf.CeilToInt(value);
			}

			public static int FloorToInt(float value)
			{
				return Mathf.FloorToInt(value);
			}

			public static int CeilBig(float value)
			{
				return Utility.Math.GetSymbol(value) * Mathf.CeilToInt(Mathf.Abs(value));
			}

			public static int GetSymbol(float value)
			{
				if (value <= 0f)
				{
					return -1;
				}
				return 1;
			}

			public static int GetSymbol(int value)
			{
				if (value <= 0)
				{
					return -1;
				}
				return 1;
			}

			public static int GetSymbol(long value)
			{
				if (value <= 0L)
				{
					return -1;
				}
				return 1;
			}

			public static int GetSymbol(string s)
			{
				if (s == "+")
				{
					return 1;
				}
				if (!(s == "-"))
				{
					return 0;
				}
				return -1;
			}

			public static string GetSymbolString(long value)
			{
				if (value < 0L)
				{
					return "-";
				}
				return "+";
			}

			public static string GetSymbolString(float value)
			{
				if (value < 0f)
				{
					return "-";
				}
				return "+";
			}

			public static float Sin(float angle)
			{
				return Mathf.Sin(angle * 3.14159274f / 180f);
			}

			public static float Cos(float angle)
			{
				return Mathf.Cos(angle * 3.14159274f / 180f);
			}

			public static int Abs(int value)
			{
				return Mathf.Abs(value);
			}

			public static float Abs(float value)
			{
				return Mathf.Abs(value);
			}

			public static long Abs(long value)
			{
				if (value <= 0L)
				{
					return -value;
				}
				return value;
			}

			public static double Abs(double value)
			{
				if (value <= 0.0)
				{
					return -value;
				}
				return value;
			}

			public static float MoveTowardsAngle(float current, float target, float maxDelta)
			{
				return Mathf.MoveTowardsAngle(current, target, maxDelta);
			}

			public static float Clamp(float value, float min, float max)
			{
				return Mathf.Clamp(value, min, max);
			}

			public static int Clamp(int value, int min, int max)
			{
				return Mathf.Clamp(value, min, max);
			}

			public static uint Clamp(uint value, uint min, uint max)
			{
				return (uint)Utility.Math.Clamp((long)((ulong)value), (long)((ulong)min), (long)((ulong)max));
			}

			public static long Clamp(long value, long min, long max)
			{
				if (value < min)
				{
					value = min;
				}
				else if (value > max)
				{
					value = max;
				}
				return value;
			}

			public static Vector3 Clamp(Vector3 value, float min, float max)
			{
				value.x = Mathf.Clamp(value.x, min, max);
				value.y = Mathf.Clamp(value.y, min, max);
				value.z = Mathf.Clamp(value.z, min, max);
				return value;
			}

			public static float Clamp01(float value)
			{
				return Mathf.Clamp01(value);
			}

			public static float Pow(float f, float p)
			{
				return Mathf.Pow(f, p);
			}

			public static int Random(int min, int max)
			{
				return global::UnityEngine.Random.Range(min, max);
			}

			public static float Random(float min, float max)
			{
				return global::UnityEngine.Random.Range(min, max);
			}

			public static int RandomSymbol()
			{
				if (Utility.Math.Random(0, 2) != 0)
				{
					return -1;
				}
				return 1;
			}

			public static bool RandomBool()
			{
				return Utility.Math.Random(0, 2) == 0;
			}

			public static long RoundToLong(double value)
			{
				return (long)global::System.Math.Round(value);
			}

			public static int RoundToInt(float value)
			{
				return Mathf.RoundToInt(value);
			}

			public static float Min(float a, float b)
			{
				if (a <= b)
				{
					return a;
				}
				return b;
			}

			public static int Min(int a, int b)
			{
				if (a <= b)
				{
					return a;
				}
				return b;
			}

			public static long Min(long a, long b)
			{
				if (a <= b)
				{
					return a;
				}
				return b;
			}

			public static float Max(float a, float b)
			{
				if (a >= b)
				{
					return a;
				}
				return b;
			}

			public static int Max(int a, int b)
			{
				if (a >= b)
				{
					return a;
				}
				return b;
			}

			public static long Max(long a, long b)
			{
				if (a >= b)
				{
					return a;
				}
				return b;
			}

			public static Color RandomColor()
			{
				return new Color(Utility.Math.Random(0f, 1f), Utility.Math.Random(0f, 1f), Utility.Math.Random(0f, 1f), 1f);
			}

			public static int GetLine(int count, int lineCount)
			{
				if (lineCount == 0)
				{
					HLog.LogError(string.Format("DxxMath.GetLine({0},{1}) error.", count, lineCount));
					return 0;
				}
				return Utility.Math.CeilToInt((float)count / ((float)lineCount + 0f));
			}

			public static float GetFloat1(float f)
			{
				return (float)((int)(f * 10f)) / 10f;
			}

			public static float GetFloat2(float f)
			{
				return (float)((int)(f * 100f)) / 100f;
			}

			public static float GetFloat3(float f)
			{
				return (float)((int)(f * 1000f)) / 1000f;
			}

			public static string GetTime3String(long second)
			{
				return string.Format("{0:D2}:{1:D2}:{2:D2}", second / 3600L, second % 3600L / 60L, second % 60L);
			}

			public static string GetTime2String(long second)
			{
				return string.Format("{0:D2}:{1:D2}", second / 60L, second % 60L);
			}

			public static Vector3 GetDirection(float angle)
			{
				Vector3 vector = default(Vector3);
				vector.x = Utility.Math.Sin(angle);
				vector.y = 0f;
				vector.z = Utility.Math.Cos(angle);
				return vector;
			}

			public static long Lerp(long a, long b, float t)
			{
				return (long)((double)a * 1.0 + 1.0 * (double)(b - a) * (double)Mathf.Clamp01(t));
			}

			public static float Round(float value, int digits)
			{
				return (float)global::System.Math.Round((double)value, digits, MidpointRounding.AwayFromZero);
			}

			public static double Round(double value, int digits)
			{
				return global::System.Math.Round(value, digits, MidpointRounding.AwayFromZero);
			}
		}

		public static class PlayerPrefs
		{
			public static void DeleteAll()
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.DeleteAll();
				}
			}

			public static void DeleteKey(string key)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.DeleteKey(key);
				}
			}

			public static float GetFloat(string key)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetFloat(key);
				}
				return -1f;
			}

			public static float GetFloat(string key, float defaultValue)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetFloat(key, defaultValue);
				}
				return -1f;
			}

			public static int GetInt(string key)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetInt(key);
				}
				return -1;
			}

			public static int GetInt(string key, int defaultValue)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetInt(key, defaultValue);
				}
				return -1;
			}

			public static string GetString(string key)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetString(key);
				}
				return string.Empty;
			}

			public static string GetString(string key, string defaultValue)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					return PlayerPrefsExpand.GetString(key, defaultValue);
				}
				return string.Empty;
			}

			public static bool HasKey(string key)
			{
				return Utility.PlayerPrefs.m_enable && PlayerPrefsExpand.HasKey(key);
			}

			public static void Save()
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.Save();
				}
			}

			public static void SetFloat(string key, float value)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.SetFloat(key, value);
				}
			}

			public static void SetInt(string key, int value)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.SetInt(key, value);
				}
			}

			public static void SetString(string key, string value)
			{
				if (Utility.PlayerPrefs.m_enable)
				{
					PlayerPrefsExpand.SetString(key, value);
				}
			}

			private static string getUserKey(string key)
			{
				return string.Format("{0}_{1}", Utility.PlayerPrefs.m_userId, key);
			}

			public static void SetUserId(string id)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return;
				}
				Utility.PlayerPrefs.m_userId = id;
			}

			public static void SetUserInt(string key, int value)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return;
				}
				Utility.PlayerPrefs.SetInt(Utility.PlayerPrefs.getUserKey(key), value);
			}

			public static int GetUserInt(string key, int defaultValue)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return defaultValue;
				}
				return Utility.PlayerPrefs.GetInt(Utility.PlayerPrefs.getUserKey(key), defaultValue);
			}

			public static void SetUserFloat(string key, float value)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return;
				}
				Utility.PlayerPrefs.SetFloat(Utility.PlayerPrefs.getUserKey(key), value);
			}

			public static float GetUserFloat(string key, float defaultValue)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return defaultValue;
				}
				return Utility.PlayerPrefs.GetFloat(Utility.PlayerPrefs.getUserKey(key), defaultValue);
			}

			public static void SetUserBool(string key, bool value)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return;
				}
				Utility.PlayerPrefs.SetInt(Utility.PlayerPrefs.getUserKey(key), value ? 1 : 0);
			}

			public static bool GetUserBool(string key, bool defaultValue)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return defaultValue;
				}
				int num = (defaultValue ? 1 : 0);
				return Utility.PlayerPrefs.GetInt(Utility.PlayerPrefs.getUserKey(key), num) == 1;
			}

			public static void SetUserString(string key, string value)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return;
				}
				Utility.PlayerPrefs.SetString(Utility.PlayerPrefs.getUserKey(key), value);
			}

			public static string GetUserString(string key, string defaultValue)
			{
				if (!Utility.PlayerPrefs.m_enable)
				{
					return defaultValue;
				}
				return Utility.PlayerPrefs.GetString(Utility.PlayerPrefs.getUserKey(key), defaultValue);
			}

			private static bool m_enable = true;

			private static string m_userId = "";
		}

		public static class UI
		{
			public static Vector2 GetWindowSize()
			{
				if (Utility.UI.m_windowSize.x > 0f)
				{
					return Utility.UI.m_windowSize;
				}
				Utility.UI.m_windowSize.x = 1080f;
				Utility.UI.m_windowSize.y = (float)Utility.UI.Height / ((float)Utility.UI.Width + 0f) * 1080f;
				Utility.UI.m_windowSize.y = Utility.Math.Clamp(Utility.UI.m_windowSize.y, 1920f, Utility.UI.m_windowSize.y);
				return Utility.UI.m_windowSize;
			}

			public static float GetAddHeight()
			{
				throw new Exception("GetAddHeight() 为实现！！！！");
			}

			public static bool IsScreenLong
			{
				get
				{
					return Utility.UI.ScreenRatio > 1.77777779f;
				}
			}

			public static Vector2 GetUISize()
			{
				if (Utility.UI.UISize.x > 0f)
				{
					return Utility.UI.UISize;
				}
				Vector2 vector;
				if (Utility.UI.IsScreenLong)
				{
					vector..ctor(1080f, (float)Utility.UI.Height * 1080f / (float)Utility.UI.Width);
				}
				else
				{
					vector..ctor((float)Utility.UI.Width * 1920f / (float)Utility.UI.Height, 1920f);
				}
				Utility.UI.UISize = vector;
				return vector;
			}

			public static Vector2 ScreenPosToUIPos(Vector2 pos)
			{
				Vector2 uisize = Utility.UI.GetUISize();
				pos.x -= (float)Utility.UI.Width * 0.5f;
				pos.y -= (float)Utility.UI.Height * 0.5f;
				pos.x /= (float)Utility.UI.Width;
				pos.y /= (float)Utility.UI.Height;
				pos.x *= uisize.x;
				pos.y *= uisize.y;
				return pos;
			}

			public static void GetSprite(string atlasName, string spriteName, Action<Sprite> onFinish)
			{
				string text = string.Format("Assets/_Resources/Atlas/{0}/{1}.asset", atlasName, atlasName);
				GameApp.Resources.LoadAssetAsync<UAtlasData>(text).Completed += delegate(AsyncOperationHandle<UAtlasData> x)
				{
					if (x.Status != 1)
					{
						return;
					}
					Sprite spriteByName = x.Result.GetSpriteByName(spriteName);
					Action<Sprite> onFinish2 = onFinish;
					if (onFinish2 == null)
					{
						return;
					}
					onFinish2(spriteByName);
				};
			}

			public static void MoveUIInScreen(RectTransform targetTran, Vector2 offset, Vector2 padding)
			{
				Vector2 vector = targetTran.anchoredPosition + offset;
				targetTran.anchoredPosition = vector;
				targetTran.GetWorldCorners(Utility.UI.moveUIInScreenUiCorners);
				Vector2 vector2 = GameApp.View.UICamera.WorldToScreenPoint(Utility.UI.moveUIInScreenUiCorners[1]);
				Vector2 vector3 = GameApp.View.UICamera.WorldToScreenPoint(Utility.UI.moveUIInScreenUiCorners[3]);
				float num = (float)Utility.UI.Width;
				float num2 = (float)Utility.UI.Height;
				bool flag = false;
				float num3;
				if (Utility.UI.CalculateInScreenPos(offset.x, vector.x, vector2.x, vector3.x, num, padding.x, out num3))
				{
					vector.x = num3;
					flag = true;
				}
				float num4;
				if (Utility.UI.CalculateInScreenPos(offset.y, vector.y, vector3.y, vector2.y, num2, padding.y, out num4))
				{
					vector.y = num4;
					flag = true;
				}
				if (flag)
				{
					targetTran.anchoredPosition = vector;
				}
			}

			private static bool CalculateInScreenPos(float offsetValue, float originValue, float cornersMin, float cornersMax, float screenValue, float paddingValue, out float endValue)
			{
				if (offsetValue > 0f)
				{
					float num = cornersMin - screenValue - paddingValue;
					if (num > 0f)
					{
						endValue = originValue - (cornersMax - screenValue + screenValue - num % screenValue);
						return true;
					}
				}
				else if (offsetValue < 0f)
				{
					float num2 = -paddingValue - cornersMax;
					if (num2 > 0f)
					{
						endValue = originValue + (0f - cornersMin + screenValue - num2 % screenValue);
						return true;
					}
				}
				endValue = originValue;
				return false;
			}

			public static int Width = Screen.width;

			public static int Height = Screen.height;

			public static int ScreenWidth = Screen.width;

			public static int ScreenHeight = Screen.height;

			public static int DesignWidth = 1080;

			public static int DesignHeight = 1920;

			public static float ScreenRatio = ((float)Utility.UI.ScreenHeight + 0f) / (float)Utility.UI.ScreenWidth;

			public static float DesignRatio = ((float)Utility.UI.DesignHeight + 0f) / (float)Utility.UI.DesignWidth;

			public static Vector2 UISize = new Vector2(-1f, -1f);

			private static Vector2 m_windowSize = new Vector2(-1f, -1f);

			public const string ATLAS_COMMON = "uicommonhot";

			public const string ATLAS_EQUIP = "uiequipshot";

			private static readonly Vector3[] moveUIInScreenUiCorners = new Vector3[4];
		}

		public static class Vibration
		{
			public static void Init()
			{
				if (Utility.Vibration.initialized)
				{
					return;
				}
				if (Application.isMobilePlatform)
				{
					Utility.Vibration.unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					Utility.Vibration.currentActivity = Utility.Vibration.unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
					Utility.Vibration.vibrator = Utility.Vibration.currentActivity.Call<AndroidJavaObject>("getSystemService", new object[] { "vibrator" });
					Utility.Vibration.context = Utility.Vibration.currentActivity.Call<AndroidJavaObject>("getApplicationContext", Array.Empty<object>());
					if (Utility.Vibration.AndroidVersion >= 26)
					{
						Utility.Vibration.vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
					}
				}
				Utility.Vibration.initialized = true;
			}

			public static void VibrateIOS(Utility.ImpactFeedbackStyle style)
			{
			}

			public static void VibrateIOS(Utility.NotificationFeedbackStyle style)
			{
			}

			public static void VibrateIOS_SelectionChanged()
			{
			}

			public static void VibratePop()
			{
				if (Application.isMobilePlatform)
				{
					Utility.Vibration.VibrateAndroid(50L);
				}
			}

			public static void VibratePeek()
			{
				if (Application.isMobilePlatform)
				{
					Utility.Vibration.VibrateAndroid(100L);
				}
			}

			public static void VibrateNope()
			{
				if (Application.isMobilePlatform)
				{
					Utility.Vibration.VibrateAndroid(new long[] { 0L, 50L, 50L, 50L }, -1);
				}
			}

			public static void VibrateAndroid(long milliseconds)
			{
				if (Application.isMobilePlatform)
				{
					if (Utility.Vibration.AndroidVersion >= 26)
					{
						AndroidJavaObject androidJavaObject = Utility.Vibration.vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", new object[] { milliseconds, -1 });
						Utility.Vibration.vibrator.Call("vibrate", new object[] { androidJavaObject });
						return;
					}
					Utility.Vibration.vibrator.Call("vibrate", new object[] { milliseconds });
				}
			}

			public static void VibrateAndroid(long[] pattern, int repeat)
			{
				if (Application.isMobilePlatform)
				{
					if (Utility.Vibration.AndroidVersion >= 26)
					{
						AndroidJavaObject androidJavaObject = Utility.Vibration.vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", new object[] { pattern, repeat });
						Utility.Vibration.vibrator.Call("vibrate", new object[] { androidJavaObject });
						return;
					}
					Utility.Vibration.vibrator.Call("vibrate", new object[] { pattern, repeat });
				}
			}

			public static void CancelAndroid()
			{
				if (Application.isMobilePlatform)
				{
					Utility.Vibration.vibrator.Call("cancel", Array.Empty<object>());
				}
			}

			public static bool HasVibrator()
			{
				if (Application.isMobilePlatform)
				{
					string @static = new AndroidJavaClass("android.content.Context").GetStatic<string>("VIBRATOR_SERVICE");
					return Utility.Vibration.context.Call<AndroidJavaObject>("getSystemService", new object[] { @static }).Call<bool>("hasVibrator", Array.Empty<object>());
				}
				return false;
			}

			public static void Vibrate()
			{
				if (Application.isMobilePlatform)
				{
					Handheld.Vibrate();
				}
			}

			public static int AndroidVersion
			{
				get
				{
					int num = 0;
					if (Application.platform == 11)
					{
						string operatingSystem = SystemInfo.operatingSystem;
						int num2 = operatingSystem.IndexOf("API-");
						num = int.Parse(operatingSystem.Substring(num2 + 4, 2).ToString());
					}
					return num;
				}
			}

			public static AndroidJavaClass unityPlayer;

			public static AndroidJavaObject currentActivity;

			public static AndroidJavaObject vibrator;

			public static AndroidJavaObject context;

			public static AndroidJavaClass vibrationEffect;

			private static bool initialized;
		}

		public enum ImpactFeedbackStyle
		{
			Heavy,
			Medium,
			Light,
			Rigid,
			Soft
		}

		public enum NotificationFeedbackStyle
		{
			Error,
			Success,
			Warning
		}
	}
}
