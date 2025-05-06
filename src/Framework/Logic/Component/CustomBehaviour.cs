using System;
using Framework.RunTimeManager;
using UnityEngine;

namespace Framework.Logic.Component
{
	public abstract class CustomBehaviour : MonoBehaviour
	{
		public RectTransform rectTransform
		{
			get
			{
				return base.gameObject.transform as RectTransform;
			}
		}

		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		public int GetObjectInstanceID()
		{
			return base.gameObject.GetInstanceID();
		}

		public void Init()
		{
			this.OnInit();
		}

		public virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void DeInit()
		{
			this.OnDeInit();
		}

		protected abstract void OnInit();

		protected abstract void OnDeInit();

		private RunTimeIDConnecterData m_idData;
	}
}
