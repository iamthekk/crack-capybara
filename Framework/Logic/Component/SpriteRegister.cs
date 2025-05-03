using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class SpriteRegister : MonoBehaviour
	{
		[ContextMenu("BindNames")]
		private void LoadGameObject()
		{
			if (this.m_dic != null)
			{
				return;
			}
			this.m_dic = new Dictionary<string, Sprite>(this.m_datas.Length);
			for (int i = 0; i < this.m_datas.Length; i++)
			{
				SpriteRegister.RegisterData registerData = this.m_datas[i];
				if (string.IsNullOrEmpty(registerData.m_name) && registerData.m_object != null)
				{
					registerData.m_name = registerData.m_object.name;
				}
				this.m_dic[registerData.m_name] = registerData.m_object;
			}
		}

		public Sprite GetSprite(string name)
		{
			if (this.m_dic == null)
			{
				this.LoadGameObject();
			}
			Sprite sprite;
			this.m_dic.TryGetValue(name, out sprite);
			return sprite;
		}

		public Dictionary<string, Sprite> GetDic()
		{
			if (this.m_dic == null)
			{
				this.LoadGameObject();
			}
			return this.m_dic;
		}

		[SerializeField]
		private SpriteRegister.RegisterData[] m_datas;

		private Dictionary<string, Sprite> m_dic;

		[Serializable]
		public class RegisterData
		{
			public string m_name;

			public Sprite m_object;
		}
	}
}
