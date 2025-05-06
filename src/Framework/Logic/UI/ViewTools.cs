using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace Framework.Logic.UI
{
	public class ViewTools
	{
		public static Dictionary<string, GameObject> CollectAllGameObjects(GameObject rootGameObject)
		{
			Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
			ViewTools.CollectAllGameObject(dictionary, rootGameObject);
			return dictionary;
		}

		private static void CollectAllGameObject(Dictionary<string, GameObject> objectMap, GameObject gameObject)
		{
			if (objectMap.ContainsKey(gameObject.name))
			{
				objectMap[gameObject.name] = gameObject;
			}
			else
			{
				objectMap.Add(gameObject.name, gameObject);
			}
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				ViewTools.CollectAllGameObject(objectMap, gameObject.transform.GetChild(i).gameObject);
			}
		}

		public static List<GameObject> GetAllGameObjects(GameObject rootGameObject)
		{
			List<GameObject> list = new List<GameObject>();
			ViewTools.CollectAllGameObject(list, rootGameObject);
			return list;
		}

		private static void CollectAllGameObject(List<GameObject> objectMap, GameObject gameObject)
		{
			objectMap.Add(gameObject);
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				ViewTools.CollectAllGameObject(objectMap, gameObject.transform.GetChild(i).gameObject);
			}
		}

		public static T GetRegComponent<T>(ComponentRegister cr, string regedname) where T : Component
		{
			if (cr == null || string.IsNullOrEmpty(regedname))
			{
				HLog.LogError("HeroCombineViewModule GetRegComponent Error == " + regedname);
				return default(T);
			}
			GameObject gameObject = cr.GetGameObject(regedname);
			if (gameObject == null)
			{
				return default(T);
			}
			return gameObject.GetComponent<T>();
		}

		public static GameObject GetRegedGameObject(ComponentRegister cr, string regedname)
		{
			if (cr == null || string.IsNullOrEmpty(regedname))
			{
				HLog.LogError("GetComponent Error == " + regedname);
				return null;
			}
			return cr.GetGameObject(regedname);
		}

		public static T GetChildComponent<T>(GameObject gobj, string pathname) where T : Component
		{
			if (gobj == null)
			{
				return default(T);
			}
			Transform findChild = ViewTools.GetFindChild(gobj.transform, pathname);
			if (findChild == null)
			{
				return default(T);
			}
			T component = findChild.GetComponent<T>();
			if (component == null)
			{
				return default(T);
			}
			return component;
		}

		public static Transform GetFindChild(Transform tf, string pathname)
		{
			if (tf == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(pathname))
			{
				return tf;
			}
			string[] array = pathname.Split('/', StringSplitOptions.None);
			Transform transform = tf;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform2 = transform.Find(array[i]);
				if (transform2 == null)
				{
					return null;
				}
				transform = transform2;
			}
			return transform;
		}
	}
}
