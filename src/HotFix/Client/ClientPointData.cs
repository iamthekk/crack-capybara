using System;
using System.Threading.Tasks;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class ClientPointData
	{
		public ClientPointData(GameObject gameObject, MemberCamp camp, MemberPos posIndex)
		{
			this.m_gameObject = gameObject;
			this.m_camp = camp;
			this.m_posIndex = posIndex;
			this.m_rotation = gameObject.transform.rotation;
			this.m_scale = gameObject.transform.localScale;
		}

		public async Task OnInit()
		{
			await Task.CompletedTask;
		}

		public Vector3 GetPosition()
		{
			if (!(this.m_gameObject != null))
			{
				return Vector3.zero;
			}
			return this.m_gameObject.transform.position;
		}

		public GameObject m_gameObject;

		public MemberCamp m_camp;

		public MemberPos m_posIndex;

		public Quaternion m_rotation;

		public Vector3 m_scale;
	}
}
