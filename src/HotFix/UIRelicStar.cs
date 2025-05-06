using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRelicStar : CustomBehaviour
	{
		public void SetData(int count)
		{
			this.m_count = count;
		}

		protected override void OnInit()
		{
			for (int i = 0; i < this.m_list.Count; i++)
			{
				this.m_list[i].SetActive(i < this.m_count);
			}
			for (int j = 0; j < this.m_fglist.Count; j++)
			{
				this.m_fglist[j].SetActive(j < this.m_count);
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
		}

		protected override void OnDeInit()
		{
		}

		public void SetCount(int count)
		{
			for (int i = 0; i < this.m_count; i++)
			{
				GameObject gameObject = this.m_fglist[i];
				if (!(gameObject == null))
				{
					gameObject.SetActive(i < count);
					gameObject.transform.localScale = Vector3.one;
				}
			}
		}

		public void PlayStar(int index)
		{
			if (index >= this.m_fglist.Count || index < 0)
			{
				return;
			}
			GameObject gameObject = this.m_fglist[index];
			if (gameObject == null)
			{
				return;
			}
			Sequence sequence = new SequencePool().Get();
			gameObject.transform.localScale = Vector3.zero;
			Vector3 vector = Vector3.one * 1.5f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(gameObject.transform, vector, 0.1f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(gameObject.transform, Vector3.one, 0.1f));
		}

		private int m_count;

		[SerializeField]
		private List<GameObject> m_list = new List<GameObject>();

		[SerializeField]
		private List<GameObject> m_fglist = new List<GameObject>();
	}
}
