using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class ComponentRegisterScriptable : MonoBehaviour
	{
		private void LoadGameObject()
		{
			if (this.m_gameObjectDatas == null)
			{
				return;
			}
			this.m_gameObjectDic = new Dictionary<string, ScriptableObject>(this.m_gameObjectDatas.Length);
			for (int i = 0; i < this.m_gameObjectDatas.Length; i++)
			{
				ComponentRegisterScriptable.GameObjectRegisterData gameObjectRegisterData = this.m_gameObjectDatas[i];
				this.m_gameObjectDic[gameObjectRegisterData.m_name] = gameObjectRegisterData.m_object;
			}
		}

		public ScriptableObject GetGameObject(string name)
		{
			if (this.m_gameObjectDic == null)
			{
				this.LoadGameObject();
			}
			ScriptableObject scriptableObject;
			this.m_gameObjectDic.TryGetValue(name, out scriptableObject);
			return scriptableObject;
		}

		public Dictionary<string, ScriptableObject> GetDic()
		{
			if (this.m_gameObjectDic == null)
			{
				this.LoadGameObject();
			}
			return this.m_gameObjectDic;
		}

		[SerializeField]
		private ComponentRegisterScriptable.GameObjectRegisterData[] m_gameObjectDatas;

		private Dictionary<string, ScriptableObject> m_gameObjectDic;

		[Serializable]
		public class GameObjectRegisterData
		{
			public string m_name;

			public ScriptableObject m_object;
		}
	}
}
