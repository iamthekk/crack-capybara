using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class BattleRaftCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ShowNormalWave(true);
			this.ShowLeftTailing();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetOrderLayer(int layer)
		{
			this.raft.sortingOrder = layer;
		}

		public void ShowNormalWave(bool isNormal)
		{
			this.smallWave.SetActiveSafe(isNormal);
			this.largeWave.SetActiveSafe(!isNormal);
		}

		public void HideWave()
		{
			this.smallWave.SetActiveSafe(false);
			this.largeWave.SetActiveSafe(false);
		}

		public void ShowLeftTailing()
		{
			this.leftTailing.SetActiveSafe(true);
			this.rightTailing.SetActiveSafe(false);
		}

		public void ShowRightTailing()
		{
			this.leftTailing.SetActiveSafe(false);
			this.rightTailing.SetActiveSafe(true);
		}

		public void HideTailing()
		{
			this.leftTailing.SetActiveSafe(false);
			this.rightTailing.SetActiveSafe(false);
		}

		public void ShowShadow(bool isShow)
		{
			this.shadow.SetActiveSafe(isShow);
		}

		public void HideAni(Action onFinish)
		{
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions43.DOFade(this.raft, 0f, 0.5f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		public void ResetAlpha()
		{
			Color color = this.raft.color;
			color.a = 1f;
			this.raft.color = color;
		}

		public SpriteRenderer raft;

		public GameObject smallWave;

		public GameObject largeWave;

		public GameObject shadow;

		public GameObject leftTailing;

		public GameObject rightTailing;

		private SequencePool sequencePool = new SequencePool();
	}
}
