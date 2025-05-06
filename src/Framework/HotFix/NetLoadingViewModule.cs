using System;
using DG.Tweening;
using Framework.EventSystem;
using Framework.Logic.Tweening;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class NetLoadingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			NetLoadingViewModuleOpenData netLoadingViewModuleOpenData = data as NetLoadingViewModuleOpenData;
			if (netLoadingViewModuleOpenData != null)
			{
				this.m_delayTime = netLoadingViewModuleOpenData.m_delayTime;
				this.m_fadeTime = netLoadingViewModuleOpenData.m_fadeTime;
			}
			this.m_canvasGroup.alpha = 0f;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(this.m_seqPool.Get(), this.m_delayTime), ShortcutExtensions46.DOFade(this.m_canvasGroup, 1f, this.m_fadeTime));
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear();
		}

		public override void OnDelete()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_rotate.rotation = Quaternion.Euler(0f, 0f, this.m_rotate.eulerAngles.z + deltaTime * -180f);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public CanvasGroup m_canvasGroup;

		public RectTransform m_rotate;

		public float m_delayTime = 0.5f;

		public float m_fadeTime = 0.3f;

		private SequencePool m_seqPool = new SequencePool();
	}
}
