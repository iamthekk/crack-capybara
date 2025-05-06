using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Modules
{
	public static class GameObjectExpand
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}

		public static void SetLayer(this GameObject gameObject, int layer, bool children = true)
		{
			gameObject.gameObject.layer = layer;
			if (!children)
			{
				return;
			}
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		public static void SetLayer(this GameObject[] gameObjects, int layer)
		{
			foreach (GameObject gameObject in gameObjects)
			{
				if (!(gameObject == null))
				{
					gameObject.layer = layer;
				}
			}
		}

		public static void SetLayer(this Transform[] gameObjects, int layer)
		{
			foreach (Transform transform in gameObjects)
			{
				if (!(transform == null))
				{
					transform.gameObject.layer = layer;
				}
			}
		}

		public static void SetLayer(this List<GameObject> gameObjects, int layer)
		{
			for (int i = 0; i < gameObjects.Count; i++)
			{
				GameObject gameObject = gameObjects[i];
				if (!(gameObject == null))
				{
					gameObject.layer = layer;
				}
			}
		}

		public static void SetLayer(this List<Transform> gameObjects, int layer)
		{
			for (int i = 0; i < gameObjects.Count; i++)
			{
				Transform transform = gameObjects[i];
				if (!(transform == null))
				{
					transform.gameObject.layer = layer;
				}
			}
		}
	}
}
