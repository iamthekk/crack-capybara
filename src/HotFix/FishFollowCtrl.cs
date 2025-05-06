using System;
using UnityEngine;

namespace HotFix
{
	public class FishFollowCtrl
	{
		public void Init(GameObject gameObject, Transform targetTrans)
		{
			this.transform = gameObject.transform;
			this.target = targetTrans;
			this.SetRandomDestination();
		}

		public void DeInit()
		{
		}

		public void Update(float deltaTime)
		{
			if (Vector3.Distance(this.transform.localPosition, this.randomDestination) <= 1f)
			{
				this.SetRandomDestination();
			}
			Vector3 vector = this.randomDestination - this.transform.localPosition;
			if (vector != Vector3.zero)
			{
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, num);
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, this.rotationSpeed * deltaTime);
			}
			this.transform.Translate(Vector3.right * this.moveSpeed * deltaTime);
		}

		private void SetRandomDestination()
		{
			float num = Random.Range(0f, 360f);
			float num2 = Mathf.Cos(num * 0.0174532924f) * this.horizontalRadius;
			float num3 = Mathf.Sin(num * 0.0174532924f) * this.verticalRadius;
			this.randomDestination = this.target.localPosition + new Vector3(num2, num3, 0f);
		}

		private Transform target;

		private float moveSpeed = 2f;

		private float rotationSpeed = 1.5f;

		public float horizontalRadius = 3f;

		public float verticalRadius = 2f;

		private Vector3 randomDestination;

		private Transform transform;
	}
}
