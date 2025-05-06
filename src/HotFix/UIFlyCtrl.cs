using System;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIFlyCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_prefab != null)
			{
				this.m_prefab.SetActive(false);
			}
			if (this.m_parent == null)
			{
				this.m_parent = base.transform;
			}
			if (this.m_startTransform == null)
			{
				this.m_startTransform = base.transform;
			}
			this.m_seqPool = new SequencePool();
			this.m_pool = LocalUnityObjctPool.Create(base.gameObject);
			this.m_pool.CreateCache("UIFlyCtrlNode", this.m_prefab);
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			this.m_pool.ClearAllCache();
		}

		public void Clear()
		{
			this.m_seqPool.Clear(false);
			this.m_pool.CollectAll();
		}

		public void Fly(int count)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				int num2 = i;
				GameObject ctrl = this.m_pool.DeQueue("UIFlyCtrlNode");
				Transform transform = ctrl.transform;
				transform.SetParent(this.m_parent);
				transform.localScale = Vector3.one;
				Vector3 position = this.m_startTransform.position;
				RectTransform endTransform = this.m_moveToTransforms[num];
				Vector3 position2 = endTransform.position;
				Vector3 vector = position + new Vector3(Utility.Math.Random(-0.2f, 0.2f), Utility.Math.Random(-0.2f, 0.2f), 0f);
				transform.position = position;
				transform.localScale = Vector3.one * 0f;
				this.m_firstTime = 0.3f;
				this.m_secondTime = 0.3f;
				num++;
				if (num >= this.m_moveToTransforms.Length)
				{
					num = 0;
				}
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, vector, this.m_firstTime, false), 1));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(transform, Vector3.one * 1.1f, this.m_firstTime));
				TweenSettingsExtensions.AppendInterval(sequence, this.m_spaceTime * (float)num2);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (!string.IsNullOrEmpty(this.m_audioPath))
					{
						GameApp.Sound.PlaySoundEffect(this.m_audioPath, 1f);
					}
				});
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, position2, this.m_secondTime, false), 11));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(transform, Vector3.one, this.m_secondTime));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.m_pool.EnQueue("UIFlyCtrlNode", ctrl);
					if (this.m_onFinished != null)
					{
						this.m_onFinished(endTransform);
					}
				});
			}
		}

		public Transform m_parent;

		public GameObject m_prefab;

		public Transform m_startTransform;

		public RectTransform[] m_moveToTransforms;

		[Header("Time Setting")]
		public float m_firstTime = 0.3f;

		public float m_secondTime = 0.3f;

		public float m_spaceTime = 0.03f;

		[Header("Audio Setting")]
		public string m_audioPath = "";

		private SequencePool m_seqPool;

		protected LocalUnityObjctPool m_pool;

		public Action<Transform> m_onFinished;
	}
}
