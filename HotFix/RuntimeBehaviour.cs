using System;
using Framework;
using Framework.RunTimeManager;
using UnityEngine;

namespace HotFix
{
	[RuntimeCustomSerializedProperty("HotFix.RuntimeBehaviour")]
	public abstract class RuntimeBehaviour
	{
		public GameObject gameObject
		{
			get
			{
				return this.m_gameObject;
			}
		}

		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				return this.gameObject.transform as RectTransform;
			}
		}

		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		public int layer
		{
			get
			{
				return this.gameObject.layer;
			}
		}

		public bool activeSelf
		{
			get
			{
				return this.gameObject.activeSelf;
			}
		}

		public string tag
		{
			get
			{
				return this.gameObject.tag;
			}
		}

		public virtual void SetGameObject(GameObject gameObj)
		{
			this.m_gameObject = gameObj;
		}

		public void SetActive(bool active)
		{
			this.gameObject.SetActive(active);
		}

		public int GetInstanceID()
		{
			return this.gameObject.GetInstanceID();
		}

		public void Init()
		{
			if (this.gameObject != null)
			{
				this.m_idData = new RunTimeIDConnecterData(this.gameObject.GetInstanceID(), this, base.GetType());
				GameApp.RunTime.AddIDConnecter(this.m_idData);
			}
			this.OnInit();
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void DeInit()
		{
			if (this.gameObject != null && this.m_idData != null)
			{
				GameApp.RunTime.RemoveIDConnecter(this.m_idData);
				this.m_idData = null;
			}
			this.OnDeInit();
		}

		protected abstract void OnInit();

		protected abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		protected abstract void OnDeInit();

		[SerializeField]
		protected GameObject m_gameObject;

		private RunTimeIDConnecterData m_idData;
	}
}
