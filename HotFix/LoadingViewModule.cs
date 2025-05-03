using System;
using Framework.EventSystem;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class LoadingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.m_isLoading = false;
			this.m_data = data as LoadingViewModuleData;
			if (this.m_data == null)
			{
				this.m_data = new LoadingViewModuleData();
				this.m_data.m_loadingType = LoadingViewModule.LoadingType.Opened;
			}
			LoadingViewModule.LoadingType loadingType = this.m_data.m_loadingType;
			if (loadingType != LoadingViewModule.LoadingType.Closed)
			{
				if (loadingType == LoadingViewModule.LoadingType.Opened)
				{
					this.OnOpendStart();
				}
			}
			else
			{
				this.OnClosedStart();
			}
			this.m_time = 0f;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			LoadingViewModule.LoadingType loadingType = this.m_data.m_loadingType;
			if (loadingType == LoadingViewModule.LoadingType.Closed)
			{
				this.OnClosedUpdate(unscaledDeltaTime);
				return;
			}
			if (loadingType != LoadingViewModule.LoadingType.Opened)
			{
				return;
			}
			this.OnOpendUpdate(unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Play(LoadingViewModule.LoadingType type, Action finished, bool isFading = true)
		{
			this.m_data = new LoadingViewModuleData();
			this.m_data.m_loadingType = type;
			this.m_data.m_onLoading = finished;
			this.m_data.m_onLoadFinish = null;
			this.m_isLoading = false;
			if (type == LoadingViewModule.LoadingType.Closed)
			{
				this.OnClosedTrigger();
				return;
			}
			if (type != LoadingViewModule.LoadingType.Opened)
			{
				return;
			}
			this.OnOpenedTrigger();
		}

		public void PlayHide(Action finished)
		{
			this.Play(LoadingViewModule.LoadingType.Opened, finished, true);
		}

		public void PlayShow(Action finished)
		{
			this.Play(LoadingViewModule.LoadingType.Closed, finished, true);
		}

		private void OnOpendStart()
		{
			this.Animator.Play("Hide", -1, 0f);
		}

		private void OnOpenedTrigger()
		{
			this.m_time = 0f;
			this.Animator.Play("Hide", -1, 0f);
		}

		private void OnOpendUpdate(float deltaTime)
		{
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration && !this.m_isLoading)
			{
				this.m_isLoading = true;
				Action onLoading = this.m_data.m_onLoading;
				if (onLoading == null)
				{
					return;
				}
				onLoading();
			}
		}

		private void OnClosedStart()
		{
			this.Animator.Play("Show", -1, 0f);
		}

		private void OnClosedTrigger()
		{
			this.m_time = 0f;
			this.Animator.Play("Show", -1, 0f);
		}

		private void OnClosedUpdate(float deltaTime)
		{
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration && !this.m_isLoading)
			{
				this.m_isLoading = true;
				Action onLoading = this.m_data.m_onLoading;
				if (onLoading == null)
				{
					return;
				}
				onLoading();
			}
		}

		public AudioSource m_cloudSound;

		public Animator Animator;

		private float m_time;

		private float m_duration = 0.5f;

		private bool m_isLoading;

		private LoadingViewModuleData m_data;

		public enum LoadingType
		{
			Null,
			Closed,
			Opened
		}
	}
}
