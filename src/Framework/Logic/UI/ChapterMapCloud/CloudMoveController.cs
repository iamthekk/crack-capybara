using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.ChapterMapCloud
{
	public class CloudMoveController : MonoBehaviour
	{
		private void Start()
		{
			this.m_random = new Random();
			for (int i = 0; i < this.m_count; i++)
			{
				int num = this.m_random.Next(0, this.m_prefabs.Count);
				if (!(this.m_prefabs[num] == null))
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_prefabs[num], base.gameObject.transform);
					gameObject.transform.position = this.MathfPosOnSurface(this.m_random);
					gameObject.transform.localScale = this.MathfScale(this.m_random);
					gameObject.gameObject.SetActive(true);
					this.m_clouds.Add(gameObject);
				}
			}
		}

		private void Update()
		{
			for (int i = 0; i < this.m_clouds.Count; i++)
			{
				GameObject gameObject = this.m_clouds[i];
				if (!(gameObject == null))
				{
					gameObject.transform.position += this.m_speed * Time.deltaTime / 100f;
					if (this.IsResetToLeft(gameObject.transform.position))
					{
						gameObject.transform.position = this.MathfPosOnEdge(this.m_random);
						gameObject.transform.localScale = this.MathfScale(this.m_random);
					}
				}
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < this.m_clouds.Count; i++)
			{
				GameObject gameObject = this.m_clouds[i];
				if (!(gameObject == null))
				{
					Object.Destroy(gameObject);
				}
			}
			this.m_clouds.Clear();
		}

		private Vector3 MathfPosOnSurface(Random random)
		{
			if (this.m_topLeft == null || this.m_bottomRight == null)
			{
				return base.transform.position;
			}
			Vector3 vector = default(Vector3);
			Vector3 vector2 = this.m_topLeft.position - this.m_bottomRight.position;
			vector.x = (float)random.NextDouble() * vector2.x + this.m_bottomRight.position.x;
			vector.y = (float)random.NextDouble() * vector2.y + this.m_bottomRight.position.y;
			return vector;
		}

		private Vector3 MathfPosOnEdge(Random random)
		{
			if (this.m_topLeft == null || this.m_bottomRight == null)
			{
				return base.transform.position;
			}
			Vector3 vector = default(Vector3);
			Vector3 vector2 = this.m_topLeft.position - this.m_bottomRight.position;
			vector.x = this.m_topLeft.position.x;
			vector.y = (float)random.NextDouble() * vector2.y + this.m_bottomRight.position.y;
			return vector;
		}

		private Vector3 MathfScale(Random random)
		{
			return Vector3.one * ((float)random.NextDouble() * (this.m_scaleMax - this.m_scaleMin) + this.m_scaleMin);
		}

		public bool IsResetToLeft(Vector3 pos)
		{
			return !(this.m_topLeft == null) && !(this.m_bottomRight == null) && (pos.x > this.m_bottomRight.position.x || pos.y < this.m_bottomRight.position.y);
		}

		public List<GameObject> m_prefabs = new List<GameObject>();

		public Transform m_topLeft;

		public Transform m_bottomRight;

		public float m_scaleMin = 0.5f;

		public float m_scaleMax = 1f;

		public int m_count = 5;

		public Vector3 m_speed = new Vector3(30f, -5f);

		public List<GameObject> m_clouds = new List<GameObject>();

		public Random m_random;
	}
}
