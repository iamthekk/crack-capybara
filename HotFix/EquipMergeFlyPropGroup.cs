using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class EquipMergeFlyPropGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_propItem.Init();
			this.m_propItem.SetEnableButton(false);
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
			if (this.m_propItem != null)
			{
				this.m_propItem.gameObject.transform.position = Vector3.Lerp(this.m_from, this.m_to, this.m_time / this.m_duration);
			}
		}

		protected override void OnDeInit()
		{
			this.m_propData = null;
			this.m_propItem.DeInit();
		}

		public void Fly(int index, PropData data, Vector3 from, Vector3 to, Action finished)
		{
			this.m_index = index;
			this.m_time = 0f;
			this.m_from = from;
			this.m_to = to;
			this.m_isPlaying = true;
			this.m_propData = data;
			this.m_finished = finished;
			if (this.m_propItem != null)
			{
				this.m_propItem.SetData(this.m_propData);
				this.m_propItem.OnRefresh();
				this.m_propItem.transform.position = from;
			}
		}

		public UIItem m_propItem;

		private Action m_finished;

		private PropData m_propData;

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

		[NonSerialized]
		public int m_index;
	}
}
