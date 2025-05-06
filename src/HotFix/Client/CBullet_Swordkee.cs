using System;
using System.Threading.Tasks;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CBullet_Swordkee : CBulletBase
	{
		protected override void OnInit()
		{
			this.startPos = this.m_gameObject.transform.position;
			this.m_gameObject.transform.localScale = ((this.m_owner.m_memberData.Camp == MemberCamp.Friendly) ? Vector3.one : new Vector3(-1f, 1f, 1f));
			this.direction = (new Vector3(this.m_endPos.x, this.startPos.y, this.startPos.z) - this.startPos).normalized;
		}

		protected override Task OnDeInit()
		{
			return null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.IsPlaying)
			{
				float num = this.m_Data.moveSpeed * Time.deltaTime;
				this.m_gameObject.transform.Translate(this.direction * num);
				if (Vector3.Distance(this.m_gameObject.transform.position, this.startPos) > this.m_Data.moveDistance)
				{
					base.IsPlaying = false;
					base.BulletHit();
				}
			}
		}

		protected override void OnReadParameters(string parameters)
		{
			this.m_Data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CBullet_Swordkee.Data>(parameters) : new CBullet_Swordkee.Data());
		}

		private CBullet_Swordkee.Data m_Data;

		private Vector3 startPos = Vector3.zero;

		private Vector3 direction = Vector3.zero;

		private class Data
		{
			public float moveSpeed = 8f;

			public float moveDistance = 9f;
		}
	}
}
