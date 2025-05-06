using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class GuardBallGroup : MonoBehaviour
	{
		private void Awake()
		{
			this.m_transform = base.transform;
		}

		private void Update()
		{
			if (this.m_isPause)
			{
				return;
			}
			if (this.debuglayout)
			{
				this.debuglayout = false;
				this.LayoutChildren();
			}
			float deltaTime = Time.deltaTime;
			float num = this.m_rotationSpeed * deltaTime;
			base.transform.Rotate(0f, num, 0f);
		}

		public void LayoutChildren()
		{
			int num = this.m_transform.childCount;
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < num; i++)
			{
				Transform child = this.m_transform.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					list.Add(child);
				}
			}
			num = list.Count;
			float num2 = 360f / (float)num;
			for (int j = 0; j < list.Count; j++)
			{
				Transform transform = list[j];
				float num3 = (float)j * num2 * 3.14159274f / 180f;
				float num4 = this.m_radius * Mathf.Cos(num3);
				float num5 = this.m_radius * Mathf.Sin(num3);
				transform.localPosition = new Vector3(num4, 0f, num5);
			}
		}

		public Transform m_transform;

		public float m_rotationSpeed;

		public bool m_isPause;

		public float m_radius = 1f;

		public bool debuglayout;
	}
}
