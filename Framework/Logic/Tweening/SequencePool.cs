using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Framework.Logic.Tweening
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

		public void Clear()
		{
			int i = 0;
			int count = this.mList.Count;
			while (i < count)
			{
				Sequence sequence = this.mList[i];
				if (sequence != null)
				{
					TweenExtensions.Kill(sequence, false);
				}
				i++;
			}
			this.mList.Clear();
		}

		private List<Sequence> mList = new List<Sequence>();
	}
}
