using System;
using System.Collections.Generic;
using DG.Tweening;

namespace HotFix
{
	public class SequencePool
	{
		public void Add(Sequence seq)
		{
			this.mList.Add(seq);
		}

		public Sequence Get()
		{
			Sequence sequence = DOTween.Sequence();
			this.mList.Add(sequence);
			return sequence;
		}

		public void Clear(bool complete = false)
		{
			int i = 0;
			int count = this.mList.Count;
			while (i < count)
			{
				Sequence sequence = this.mList[i];
				if (sequence != null)
				{
					TweenExtensions.Kill(sequence, complete);
				}
				i++;
			}
			this.mList.Clear();
		}

		private List<Sequence> mList = new List<Sequence>();
	}
}
