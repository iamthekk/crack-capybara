using System;
using System.Threading.Tasks;
using Framework.Logic.Component;
using HotFix.Client;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public abstract class RoleSpinePlayerBase : MonoBehaviour
	{
		public event Action<TrackEntry, Event> onEventCallback;

		public virtual void Init(ComponentRegister componentRegister)
		{
		}

		public virtual void DeInit()
		{
		}

		public virtual void SetSpeed(float speed)
		{
		}

		public virtual void PlayAni(string animationName, bool isLoop)
		{
		}

		public virtual void PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate complete)
		{
		}

		public virtual TrackEntry PlayAni(string aniName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			return null;
		}

		public virtual float GetAnimationDuration(string animationName)
		{
			return 0f;
		}

		public virtual TrackEntry AddAni(string animationName, bool isLoop)
		{
			return null;
		}

		public virtual bool IsHaveAni(string animationName)
		{
			return false;
		}

		public virtual void PlayAnimation(string animationName)
		{
		}

		public virtual void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent)
		{
		}

		public virtual void PlayAnimation(string animationName, AnimationState.TrackEntryDelegate complete)
		{
		}

		public virtual void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
		}

		public virtual void AddAnimation(string animationName)
		{
		}

		public virtual void SetSortingLayer(string layer)
		{
		}

		public virtual void SetOrderLayer(int layer)
		{
		}

		public virtual int GetMinOrderLayer()
		{
			return 0;
		}

		public virtual int GetMaxOrderLayer()
		{
			return 0;
		}

		public virtual void SetAnimatorSpeed(float speed)
		{
		}

		public virtual void SetSkin(string skinName)
		{
		}

		public virtual void Init_ColorRender()
		{
		}

		public virtual void DeInit_ColorRender()
		{
		}

		protected virtual void OnEvent(TrackEntry trackEntry, Event e)
		{
			Action<TrackEntry, Event> action = this.onEventCallback;
			if (action == null)
			{
				return;
			}
			action(trackEntry, e);
		}

		public virtual async Task ShowSagecraft(SagecraftType type)
		{
			await Task.CompletedTask;
		}

		public virtual void DestroySagecraft(SagecraftType type)
		{
		}

		public abstract void SetWeapon(int equipID);

		public virtual async Task PreInitMorph(int morphId)
		{
			await Task.CompletedTask;
		}

		public virtual MorphBehaviour GetMorph(int morphId)
		{
			return null;
		}

		public virtual void ActiveMorph(int morphId)
		{
		}

		public virtual void SetRoleModelShow(bool isShow)
		{
		}

		public SkeletonAnimation bodyAnimation;

		[NonSerialized]
		public ColorRender m_colorRender;
	}
}
