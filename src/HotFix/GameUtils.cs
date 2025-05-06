using System;
using UnityEngine;

namespace HotFix
{
	public static class GameUtils
	{
		public static T FindChild<T>(GameObject from, string name) where T : Component
		{
			GameObject gameObject = from.FindChildByName(name);
			if (gameObject == null)
			{
				HLog.LogError(string.Format("Can not Find GameObject：" + name + " ", Array.Empty<object>()));
				return default(T);
			}
			T component = gameObject.GetComponent<T>();
			if (component == null)
			{
				HLog.LogError(string.Format(string.Format("Can not GetComponent：{0} GameObject is {1} ", typeof(T), name), Array.Empty<object>()));
			}
			return component;
		}

		public static GameObject FindChildByName(this GameObject go, string name)
		{
			if (go.name == name)
			{
				return go;
			}
			Transform transform = go.transform.FindChildByName(name);
			if (!(transform != null))
			{
				return null;
			}
			return transform.gameObject;
		}

		public static Transform FindChildByName(this Transform tran, string name)
		{
			if (tran.name == name)
			{
				return tran;
			}
			for (int i = 0; i < tran.childCount; i++)
			{
				Transform transform = tran.GetChild(i).FindChildByName(name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}
	}
}
