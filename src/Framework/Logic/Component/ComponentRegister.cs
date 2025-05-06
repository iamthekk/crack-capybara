using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class ComponentRegister : MonoBehaviour
	{
		private void LoadGameObject()
		{
			if (this.m_gameObjectDatas == null)
			{
				return;
			}
			this.m_gameObjectDic = new Dictionary<string, GameObject>(this.m_gameObjectDatas.Length);
			for (int i = 0; i < this.m_gameObjectDatas.Length; i++)
			{
				ComponentRegister.GameObjectRegisterData gameObjectRegisterData = this.m_gameObjectDatas[i];
				this.m_gameObjectDic[gameObjectRegisterData.m_name] = gameObjectRegisterData.m_object;
			}
		}

		public GameObject GetGameObject(string name)
		{
			if (this.m_gameObjectDic == null)
			{
				this.LoadGameObject();
			}
			GameObject gameObject;
			this.m_gameObjectDic.TryGetValue(name, out gameObject);
			return gameObject;
		}

		public Dictionary<string, GameObject> GetDic()
		{
			if (this.m_gameObjectDic == null)
			{
				this.LoadGameObject();
			}
			return this.m_gameObjectDic;
		}

		public bool Contains(string name)
		{
			if (this.m_gameObjectDic == null)
			{
				this.LoadGameObject();
			}
			return this.m_gameObjectDic.ContainsKey(name);
		}

		[SerializeField]
		private ComponentRegister.GameObjectRegisterData[] m_gameObjectDatas;

		private Dictionary<string, GameObject> m_gameObjectDic;

		[Serializable]
		public class GameObjectRegisterData
		{
			public string m_name;

			public GameObject m_object;
		}
	}
}
