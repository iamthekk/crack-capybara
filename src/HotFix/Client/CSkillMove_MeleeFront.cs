using System;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CSkillMove_MeleeFront : CSkillMoveTaskBase
	{
		public CMemberBase m_target { get; set; }

		protected override Vector3 GetTargetPosition()
		{
			float num = ((this.m_owner.m_memberData.Camp == MemberCamp.Friendly) ? (-this.m_Data.offsetX) : this.m_Data.offsetX);
			float num2 = ((this.m_owner.m_memberData.Camp == MemberCamp.Friendly) ? (-this.m_Data.offsetY) : this.m_Data.offsetY);
			return new Vector3(this.m_target.m_gameObject.transform.position.x + num, this.m_target.m_gameObject.transform.position.y + num2);
		}

		protected override void OnSetParameters(string parameters)
		{
			this.m_Data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CSkillMove_MeleeFront.Data>(parameters) : new CSkillMove_MeleeFront.Data());
		}

		private CSkillMove_MeleeFront.Data m_Data;

		[Serializable]
		public class Data
		{
			public float offsetX = 1.8f;

			public float offsetY;
		}
	}
}
