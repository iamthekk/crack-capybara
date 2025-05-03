using System;
using Framework;
using UnityEngine;

namespace HotFix.Client
{
	[RuntimeDefaultSerializedProperty]
	public class PointRotationController
	{
		public PointRotationController(GameObject obj, PointRotationDirection pointRotationDirection, GameObject target = null)
		{
			this.gameObject = obj;
			this.m_PointRotationDirection = pointRotationDirection;
			this.m_target = target;
			this.OnRefresh();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.OnRefresh();
		}

		private void OnRefresh()
		{
			switch (this.m_PointRotationDirection)
			{
			case PointRotationDirection.Parent:
				break;
			case PointRotationDirection.Target:
				if (this.m_target != null)
				{
					this.gameObject.transform.rotation = this.m_target.transform.rotation;
					return;
				}
				break;
			case PointRotationDirection.World:
				this.gameObject.transform.rotation = Quaternion.identity;
				return;
			case PointRotationDirection.TargetRradiusScattering_Up:
				this.gameObject.transform.rotation = Quaternion.LookRotation(this.m_target.transform.position - this.gameObject.transform.position, Vector3.up);
				break;
			default:
				return;
			}
		}

		public GameObject gameObject;

		private GameObject m_target;

		private PointRotationDirection m_PointRotationDirection;
	}
}
