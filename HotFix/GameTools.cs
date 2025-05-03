using System;
using System.Collections.Generic;
using System.Globalization;
using Framework.Logic;
using Google.Protobuf.Collections;
using UnityEngine;

namespace HotFix
{
	public static class GameTools
	{
		public static void RandomSort<T>(this List<T> list)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				T t = list[i];
				list.RemoveAt(i);
				int num = Utility.Math.Random(0, list.Count);
				list.Insert(num, t);
				i++;
			}
		}

		public static void RandomSort<T>(this List<T> list, Random random)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				T t = list[i];
				list.RemoveAt(i);
				int num = random.Next(0, list.Count);
				list.Insert(num, t);
				i++;
			}
		}

		public static void DestroyChildren(this Transform t)
		{
			bool isPlaying = Application.isPlaying;
			while (t.childCount != 0)
			{
				Transform child = t.GetChild(0);
				if (isPlaying)
				{
					child.SetParent(null);
					Object.Destroy(child.gameObject);
				}
				else
				{
					Object.DestroyImmediate(child.gameObject);
				}
			}
		}

		public static void DestroyGameObject(this Transform t)
		{
			if (Application.isPlaying)
			{
				t.SetParent(null);
				Object.Destroy(t.gameObject);
				return;
			}
			Object.DestroyImmediate(t.gameObject);
		}

		public static List<T> GetComponentsInChildrens<T>(this GameObject t) where T : Component
		{
			List<T> list = new List<T>();
			int childCount = t.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = t.transform.GetChild(i);
				T component = child.GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
				List<T> componentsInChildrens = child.gameObject.GetComponentsInChildrens<T>();
				for (int j = 0; j < componentsInChildrens.Count; j++)
				{
					list.Add(componentsInChildrens[j]);
				}
			}
			return list;
		}

		public static void SetParentNormal(this GameObject child, Transform parent, bool keepSize = false)
		{
			GameTools.SetParentNormalInternal(child.transform, parent, keepSize);
		}

		public static void SetParentNormal(this Transform child, GameObject parent, bool keepSize = false)
		{
			GameTools.SetParentNormalInternal(child, parent.transform, keepSize);
		}

		public static void SetParentNormal(this GameObject child, GameObject parent, bool keepSize = false)
		{
			GameTools.SetParentNormalInternal(child.transform, parent.transform, keepSize);
		}

		public static void SetParentNormal(this Transform child, Transform parent, bool keepSize = false)
		{
			GameTools.SetParentNormalInternal(child, parent, keepSize);
		}

		public static string DebugParent(this Transform child)
		{
			Transform transform = child.parent;
			string text = "parent : " + child.name;
			while (transform != null)
			{
				text = text + "->" + transform.name;
				transform = transform.parent;
			}
			return text;
		}

		private static void SetParentNormalInternal(Transform child, Transform parent, bool keepSize = false)
		{
			if (child == null || parent == null)
			{
				return;
			}
			child.SetParent(parent, false);
			RectTransform rectTransform = child as RectTransform;
			child.localPosition = Vector3.zero;
			if (!keepSize)
			{
				child.localScale = Vector3.one;
			}
			child.localEulerAngles = Vector3.zero;
			if (rectTransform)
			{
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}

		public static Transform GetFirstParent(this Transform t)
		{
			Transform transform = t.parent;
			Transform transform2 = transform;
			while (transform2 != null)
			{
				transform2 = transform2.parent;
				if (transform2 != null)
				{
					transform = transform2;
				}
			}
			return transform;
		}

		public static void SetCenter(this Transform t)
		{
			RectTransform rectTransform = t as RectTransform;
			if (rectTransform)
			{
				GameTools.SetCenterInternal(rectTransform);
			}
		}

		public static void SetCenter(this GameObject t)
		{
			RectTransform rectTransform = t.transform as RectTransform;
			if (rectTransform)
			{
				GameTools.SetCenterInternal(rectTransform);
			}
		}

		public static void SetCenter(this RectTransform t)
		{
			if (t)
			{
				GameTools.SetCenterInternal(t);
			}
		}

		public static void SetCenterInternal(RectTransform t)
		{
			t.anchorMin = new Vector2(0.5f, 0.5f);
			t.anchorMax = new Vector2(0.5f, 0.5f);
			t.pivot = new Vector2(0.5f, 0.5f);
		}

		public static void SetLeft(this Transform t)
		{
			RectTransform rectTransform = t as RectTransform;
			if (rectTransform)
			{
				GameTools.SetLeftInternal(rectTransform);
			}
		}

		public static void SetLeft(this GameObject o)
		{
			RectTransform rectTransform = o.transform as RectTransform;
			if (rectTransform)
			{
				GameTools.SetLeftInternal(rectTransform);
			}
		}

		public static void SetLeft(this RectTransform t)
		{
			GameTools.SetLeftInternal(t);
		}

		public static void SetLeftInternal(RectTransform t)
		{
			t.anchorMin = new Vector2(0f, 0.5f);
			t.anchorMax = new Vector2(0f, 0.5f);
			t.pivot = new Vector2(0f, 0.5f);
		}

		public static void SetLeftTop(this Transform t)
		{
			RectTransform rectTransform = t as RectTransform;
			if (rectTransform)
			{
				GameTools.SetLeftTopInternal(rectTransform);
			}
		}

		public static void SetLeftTop(this GameObject o)
		{
			RectTransform rectTransform = o.transform as RectTransform;
			if (rectTransform)
			{
				GameTools.SetLeftTopInternal(rectTransform);
			}
		}

		public static void SetLeftTop(this RectTransform t)
		{
			GameTools.SetLeftTopInternal(t);
		}

		public static void SetLeftTopInternal(RectTransform t)
		{
			t.anchorMin = Vector2.up;
			t.anchorMax = Vector2.up;
			t.pivot = new Vector2(0f, 1f);
		}

		public static void SetTop(this RectTransform t)
		{
			if (t)
			{
				GameTools.SetTopInternal(t);
			}
		}

		public static void SetTop(this Transform t)
		{
			if (t)
			{
				RectTransform rectTransform = t as RectTransform;
				if (rectTransform)
				{
					GameTools.SetTopInternal(rectTransform);
				}
			}
		}

		public static void SetTopInternal(RectTransform t)
		{
			t.anchorMin = new Vector2(0.5f, 1f);
			t.anchorMax = new Vector2(0.5f, 1f);
			t.pivot = new Vector2(0.5f, 1f);
		}

		public static bool TryParseToFloat(string source, out float val)
		{
			return float.TryParse(source, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
		}

		public static bool TryParseToLong(string source, out long val)
		{
			return long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out val);
		}

		public static float ParseToFloat(string source, float defaultValue = 0f)
		{
			float num;
			if (GameTools.TryParseToFloat(source, out num))
			{
				return num;
			}
			return defaultValue;
		}

		public static long ParseToLong(string source, long defaultValue = 0L)
		{
			long num;
			if (GameTools.TryParseToLong(source, out num))
			{
				return num;
			}
			return defaultValue;
		}

		public static bool TryParseToDouble(string source, out double val)
		{
			return double.TryParse(source, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
		}

		public static double ParseToDouble(string source, double defaultValue = 0.0)
		{
			double num;
			if (double.TryParse(source, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return num;
			}
			return defaultValue;
		}

		public static bool TryParseToInt(string source, out int val)
		{
			return int.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out val);
		}

		public static int ParseToInt(string source, int defaultValue = 0)
		{
			int num;
			if (int.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				return num;
			}
			return defaultValue;
		}

		public static string Debug<T>(this List<T> list)
		{
			string text = typeof(T).ToString() + " ";
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				string text2 = text;
				T t = list[i];
				text = text2 + ((t != null) ? t.ToString() : null);
				if (i < count - 1)
				{
					text += ",";
				}
				i++;
			}
			return text;
		}

		public static string Debug<T>(this T[] list)
		{
			string text = typeof(T).ToString() + " ";
			int i = 0;
			int num = list.Length;
			while (i < num)
			{
				string text2 = text;
				T t = list[i];
				text = text2 + ((t != null) ? t.ToString() : null);
				if (i < num - 1)
				{
					text += ",";
				}
				i++;
			}
			return text;
		}

		public static string Debug<T>(this RepeatedField<T> list)
		{
			string text = typeof(T).ToString() + " ";
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				string text2 = text;
				T t = list[i];
				text = text2 + ((t != null) ? t.ToString() : null);
				if (i < count - 1)
				{
					text += ",";
				}
				i++;
			}
			return text;
		}

		public static void Sort<T>(this RepeatedField<T> list, Func<T, T, int> sortFunc)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = i + 1; j < count; j++)
				{
					if (sortFunc(list[i], list[j]) > 0)
					{
						T t = list[i];
						list[i] = list[j];
						list[j] = t;
					}
				}
			}
		}

		public static bool TryParse(this string str, out float value)
		{
			return float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		public static bool IsHave(this ulong value, int index)
		{
			ulong num = 1UL << index;
			return (value & num) == num;
		}

		public static bool IsEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		public static void SetActiveSafe(this GameObject o, bool value)
		{
			if (o == null)
			{
				return;
			}
			if (o.activeSelf == value)
			{
				return;
			}
			o.SetActive(value);
		}
	}
}
