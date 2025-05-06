using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIChapterTaskController : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Show(false, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.sequencePool.Clear(false);
		}

		public void SetTask(int task)
		{
			this.textTask.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_138", new object[] { task });
		}

		public void Show(bool value, Action onShowFinish = null)
		{
			if (value)
			{
				base.gameObject.SetActive(true);
				this.canvasGroup.alpha = 0f;
				this.sequencePool.Clear(false);
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.canvasGroup, 1f, 0.5f));
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.canvasGroup, 0f, 0.5f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					Action onShowFinish2 = onShowFinish;
					if (onShowFinish2 != null)
					{
						onShowFinish2();
					}
					this.gameObject.SetActive(false);
				});
				return;
			}
			base.gameObject.SetActive(false);
		}

		public CanvasGroup canvasGroup;

		public CustomText textTask;

		private SequencePool sequencePool = new SequencePool();
	}
}
