using System;
using Framework;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class BaseHover : CustomBehaviour
	{
		public Transform target { get; private set; }

		public int ownerId { get; private set; }

		public abstract HoverType GetHoverType();

		public bool isPlaying { get; private set; }

		public Camera Camera3D
		{
			get
			{
				return this.camera3D;
			}
		}

		public Camera UICamera
		{
			get
			{
				return GameApp.View.UICamera;
			}
		}

		public void Init(Camera camera3D, Transform target, int ownerId)
		{
			this.camera3D = camera3D;
			this.target = target;
			this.ownerId = ownerId;
			this.isPlaying = true;
			this.OnInit();
		}

		protected override void OnDeInit()
		{
			this.isPlaying = false;
		}

		protected abstract void OnUpdateImpl(float deltaTime, float unscaledDeltaTime);

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.isPlaying)
			{
				return;
			}
			this.OnUpdateImpl(deltaTime, unscaledDeltaTime);
		}

		public void SetHoverData(object data)
		{
			this.hoverData = data;
		}

		public virtual void RefreshTargetPos(Vector3 target)
		{
			Vector3 vector = this.Camera3D.WorldToScreenPoint(target);
			Vector3 vector2 = this.UICamera.ScreenToWorldPoint(vector);
			vector2.z = base.transform.parent.position.z;
			base.gameObject.transform.position = vector2;
		}

		public void RefreshTargetPositionLessHp(Vector3 target)
		{
			target.x += Random.Range(-0.5f, 0.5f);
			target.y += Random.Range(0f, 0.9f);
			Vector3 vector = this.Camera3D.WorldToScreenPoint(target);
			Vector3 vector2 = this.UICamera.ScreenToWorldPoint(vector);
			vector2.z = base.transform.parent.position.z;
			base.gameObject.transform.position = vector2;
		}

		protected void SetShow(bool isShow)
		{
			base.gameObject.SetActive(isShow);
		}

		protected void RemoveHover()
		{
			EventArgsRemoveHover instance = Singleton<EventArgsRemoveHover>.Instance;
			instance.hover = this;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RemoveHover, instance);
		}

		protected object hoverData;

		private Camera camera3D;
	}
}
