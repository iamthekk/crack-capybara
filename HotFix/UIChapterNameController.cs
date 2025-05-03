using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIChapterNameController : CustomBehaviour
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
		}

		public void SetName(string name)
		{
			this.Text_Name.text = name;
		}

		public void Show(bool value, Action onShowFinish = null)
		{
			if (value)
			{
				this.m_canvas.gameObject.SetActive(true);
				this.m_canvas.alpha = 0f;
				this.m_seqPool.Clear(false);
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.m_canvas, 1f, 0.5f));
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.m_canvas, 0f, 0.5f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					Action onShowFinish2 = onShowFinish;
					if (onShowFinish2 != null)
					{
						onShowFinish2();
					}
					this.m_canvas.gameObject.SetActive(false);
				});
				return;
			}
			this.m_canvas.gameObject.SetActive(false);
		}

		public CanvasGroup m_canvas;

		public CustomText Text_Name;

		private SequencePool m_seqPool = new SequencePool();
	}
}
