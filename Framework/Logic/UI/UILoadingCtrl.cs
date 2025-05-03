using System;
using DG.Tweening;
using Framework.Logic.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Logic.UI
{
	[Obsolete("Use UINetLoading instead")]
	public class UILoadingCtrl : MonoBehaviour
	{
		public void OnInit()
		{
			this.buttonRetry.onClick.AddListener(new UnityAction(this.OnRetry));
		}

		public void OnDeInit()
		{
			if (this.buttonRetry != null)
			{
				this.buttonRetry.onClick.RemoveListener(new UnityAction(this.OnRetry));
			}
		}

		private void OnRetry()
		{
			if (base.gameObject.activeSelf)
			{
				Action action = this.retryCallback;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		public void OnShow()
		{
			base.gameObject.SetActive(true);
			this.wifiObj.SetActive(true);
			this.retryObj.SetActive(false);
			TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.Append(this.mSeqPool.Get(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this.rotate, new Vector3(0f, 0f, 360f), 2f, 2), 1)), true), -1);
		}

		public void OnClose()
		{
			this.mSeqPool.Clear();
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void ShowRetry(Action callback)
		{
			this.retryCallback = callback;
			this.wifiObj.SetActive(false);
			this.retryObj.SetActive(true);
		}

		public void SetRetryText(string retrystr)
		{
			this.textRetry.text = retrystr;
		}

		public void SetText(string retrystr, string loadingstr)
		{
			this.textRetry.text = retrystr;
		}

		public const string Path = "UI/GuildUI/UI_Loading";

		public RectTransform rotate;

		public GameObject wifiObj;

		public GameObject retryObj;

		public CustomButton buttonRetry;

		public CustomText textRetry;

		public float inteval = 0.5f;

		private SequencePool mSeqPool = new SequencePool();

		private Action retryCallback;
	}
}
