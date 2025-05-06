using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.Tweening;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class PopAnimationSequence : MonoBehaviour
	{
		public void AddData(PopAnimationSequence.Data data)
		{
			this.m_currentList.Add(data);
		}

		public void SetFinish(Action onFinish)
		{
			this.m_onFinish = onFinish;
		}

		private void OnEnable()
		{
			this.m_currentList = new List<PopAnimationSequence.Data>();
			this.m_currentList.AddRange(this.m_list);
			if (this.AutoPlay)
			{
				this.Play();
			}
		}

		private void OnDisable()
		{
			this.m_seqPool.Clear();
		}

		public void Play()
		{
			if (this.m_currentList.Count == 0)
			{
				return;
			}
			Sequence sequence = this.m_seqPool.Get();
			if (this.StartDelayTime > 0f)
			{
				TweenSettingsExtensions.AppendInterval(sequence, this.StartDelayTime);
			}
			for (int i = 0; i < this.m_currentList.Count; i++)
			{
				PopAnimationSequence.Data data = this.m_currentList[i];
				if (data != null && data.IsValid)
				{
					data.transform.localScale = Vector3.zero;
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						data.transform.localScale = Vector3.one * 0.5f;
					});
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(data.transform, Vector3.one, data.playTime), 27));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						Action<GameObject> itemFinish = data.itemFinish;
						if (itemFinish == null)
						{
							return;
						}
						itemFinish(data.transform.gameObject);
					});
					TweenSettingsExtensions.AppendInterval(sequence, data.delayTime);
					TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
				}
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onFinish = this.m_onFinish;
				if (onFinish == null)
				{
					return;
				}
				onFinish();
			});
		}

		public void Clear()
		{
			this.m_currentList.Clear();
		}

		[Header("初始等待时间")]
		public float StartDelayTime;

		public List<PopAnimationSequence.Data> m_list = new List<PopAnimationSequence.Data>();

		[Header("自动播放")]
		public bool AutoPlay;

		private List<PopAnimationSequence.Data> m_currentList;

		private Action m_onFinish;

		private SequencePool m_seqPool = new SequencePool();

		[Serializable]
		public class Data
		{
			public bool IsValid
			{
				get
				{
					return this.transform != null;
				}
			}

			public Transform transform;

			public float playTime = 0.15f;

			public float delayTime;

			public Action<GameObject> itemFinish;

			public GameObject mObj;
		}
	}
}
