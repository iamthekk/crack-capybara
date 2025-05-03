using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class EquipMergeFlyEquipGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_equipItem.Init();
			this.m_equipItem.SetButtonEnable(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration)
			{
				this.m_time = this.m_duration;
				this.m_isPlaying = false;
				Action finished = this.m_finished;
				if (finished != null)
				{
					finished();
				}
			}
			if (this.m_equipItem != null)
			{
				this.m_equipItem.gameObject.transform.position = Vector3.Lerp(this.m_from, this.m_to, this.m_time / this.m_duration);
			}
		}

		protected override void OnDeInit()
		{
			this.m_equipData = null;
			this.m_equipItem.DeInit();
		}

		public void Fly(EquipData data, Vector3 from, Vector3 to, Action finished)
		{
			this.m_time = 0f;
			this.m_from = from;
			this.m_to = to;
			this.m_isPlaying = true;
			this.m_equipData = data;
			this.m_finished = finished;
			if (this.m_equipItem != null)
			{
				this.m_equipItem.RefreshData(this.m_equipData);
				this.m_equipItem.transform.position = from;
			}
		}

		public UIHeroEquipItem m_equipItem;

		private Action m_finished;

		private EquipData m_equipData;

		[SerializeField]
		private Vector3 m_from;

		[SerializeField]
		private Vector3 m_to;

		[SerializeField]
		private float m_time;

		[SerializeField]
		private float m_duration = 0.25f;

		[SerializeField]
		private bool m_isPlaying;
	}
}
