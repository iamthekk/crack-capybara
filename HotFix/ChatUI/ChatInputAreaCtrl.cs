using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace HotFix.ChatUI
{
	public class ChatInputAreaCtrl
	{
		public float CurrentHeight
		{
			get
			{
				return this.mCurrentHeight;
			}
		}

		public void OnShow()
		{
			if (ChatInputAreaCtrl.sSystemKeyboardHeight > this.FoldHeight)
			{
				this.UnFlodHeight = ChatInputAreaCtrl.sSystemKeyboardHeight;
			}
		}

		public void OnClose()
		{
			this.m_seqPool.Clear(false);
			this.m_curSeq = null;
		}

		public void PlayFold(bool fold)
		{
			this.m_seqPool.Clear(false);
			this.m_curSeq = null;
			this.IsFold = fold;
			if (this.IsFold)
			{
				this.PlayToFold();
				return;
			}
			this.PlayToUnFold();
		}

		public void SetFold(bool fold)
		{
			this.m_seqPool.Clear(false);
			this.m_curSeq = null;
			this.IsFold = fold;
			if (this.IsFold)
			{
				this.FlodHeightSetter(this.FoldHeight);
				return;
			}
			this.FlodHeightSetter(this.UnFlodHeight);
		}

		public void SetSystemKeyboardHeight(float height)
		{
			if (height <= 0f)
			{
				return;
			}
			height = height - this.UnFlodOffset + this.UnFlodInputFieldHeight;
			if (ChatInputAreaCtrl.sSystemKeyboardHeight == height)
			{
				return;
			}
			ChatInputAreaCtrl.sSystemKeyboardHeight = height;
			if (!this.IsFold && this.UnFlodHeight != ChatInputAreaCtrl.sSystemKeyboardHeight)
			{
				this.UnFlodHeight = ChatInputAreaCtrl.sSystemKeyboardHeight;
				if (!TweenExtensions.IsComplete(this.m_curSeq))
				{
					this.m_seqPool.Clear(false);
					this.m_curSeq = null;
					this.PlayToUnFold();
				}
			}
		}

		private float FlodHeightGetter()
		{
			return this.mCurrentHeight;
		}

		private void FlodHeightSetter(float h)
		{
			this.mCurrentHeight = h;
			float num = Mathf.Clamp(this.mCurrentHeight, this.FoldHeight, this.UnFlodHeight);
			if (this.RTFCenter != null)
			{
				this.RTFCenter.offsetMin = new Vector2(this.RTFCenter.offsetMin.x, num);
			}
			if (this.RTFBottom != null)
			{
				this.RTFBottom.sizeDelta = new Vector2(this.RTFBottom.sizeDelta.x, num);
			}
			Action<float> onAreaHeightChange = this.OnAreaHeightChange;
			if (onAreaHeightChange == null)
			{
				return;
			}
			onAreaHeightChange(h);
		}

		private void PlayToFold()
		{
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(new DOGetter<float>(this.FlodHeightGetter), new DOSetter<float>(this.FlodHeightSetter), this.FoldHeight, 0.12f), false), this));
			this.m_curSeq = sequence;
		}

		private void PlayToUnFold()
		{
			if (ChatInputAreaCtrl.sSystemKeyboardHeight > this.FoldHeight)
			{
				this.UnFlodHeight = ChatInputAreaCtrl.sSystemKeyboardHeight;
			}
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(new DOGetter<float>(this.FlodHeightGetter), new DOSetter<float>(this.FlodHeightSetter), this.UnFlodHeight, 0.12f), false), this));
			this.m_curSeq = sequence;
			if (this.RTFEmoji != null)
			{
				float num = Mathf.Abs(this.RTFEmoji.anchoredPosition.y) + 20f;
				this.RTFEmoji.sizeDelta = new Vector2(this.RTFEmoji.sizeDelta.x, this.UnFlodHeight - num);
			}
		}

		public RectTransform RTFCenter;

		public RectTransform RTFBottom;

		public RectTransform RTFEmoji;

		public Action<float> OnAreaHeightChange;

		public bool IsFold = true;

		public float FoldHeight = 150f;

		public float UnFlodHeight = 930f;

		public float UnFlodOffset = 170f;

		public float UnFlodInputFieldHeight = 125f;

		private static float sSystemKeyboardHeight;

		private SequencePool m_seqPool = new SequencePool();

		private Sequence m_curSeq;

		private float mCurrentHeight = 150f;
	}
}
