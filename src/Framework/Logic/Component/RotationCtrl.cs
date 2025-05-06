using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class RotationCtrl : MonoBehaviour
	{
		private void Update()
		{
			base.gameObject.transform.Rotate(this.m_diration * this.m_speed * Time.deltaTime * -10f);
		}

		public Vector3 m_diration = Vector3.forward;

		public float m_speed = 10f;
	}
}
