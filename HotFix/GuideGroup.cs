using System;
using System.Collections.Generic;

namespace HotFix
{
	public class GuideGroup
	{
		public IList<GuideData> AllGuide
		{
			get
			{
				return this.mGuideList;
			}
		}

		public int Group { get; private set; }

		public int CurGuideIndex
		{
			get
			{
				return this.mCurGuideIndex;
			}
		}

		public GuideData CurGuide
		{
			get
			{
				if (this.mCurGuideIndex >= 0 && this.mCurGuideIndex < this.mGuideList.Count)
				{
					return this.mGuideList[this.mCurGuideIndex];
				}
				return null;
			}
		}

		public GuideData NextGuide
		{
			get
			{
				if (this.mCurGuideIndex + 1 >= 0 && this.mCurGuideIndex + 1 < this.mGuideList.Count)
				{
					return this.mGuideList[this.mCurGuideIndex + 1];
				}
				return null;
			}
		}

		public bool CompleteAll
		{
			get
			{
				return this.mCurGuideIndex >= this.mGuideList.Count || (this.mCurGuideIndex + 1 == this.mGuideList.Count && this.IsGuideActive);
			}
		}

		public GuideData First
		{
			get
			{
				if (this.mGuideList.Count > 0)
				{
					return this.mGuideList[0];
				}
				return null;
			}
		}

		public GuideData EndGuide
		{
			get
			{
				if (this.mGuideList.Count > 0)
				{
					return this.mGuideList[this.mGuideList.Count - 1];
				}
				return null;
			}
		}

		public int SortValue
		{
			get
			{
				if (this.mGuideList.Count <= 0)
				{
					return int.MaxValue;
				}
				return this.mGuideList[0].SortWeight;
			}
		}

		public int GuideCount
		{
			get
			{
				return this.mGuideList.Count;
			}
		}

		public List<GuideTriggerKindData> FirstTriggerKind
		{
			get
			{
				if (this.mGuideList.Count > 0)
				{
					return this.mGuideList[0].TriggerKind;
				}
				return null;
			}
		}

		public bool IfCanBreak
		{
			get
			{
				return this.mCurGuideIndex < 0 || this.mCurGuideIndex >= this.mGuideList.Count || this.mGuideList[this.mCurGuideIndex].IfCanBreak;
			}
		}

		public bool IsGuideActive { get; private set; }

		public bool IfCurAsGroupOver
		{
			get
			{
				return this.mCurGuideIndex + 1 >= this.mGuideList.Count || (this.mCurGuideIndex >= 0 && this.mCurGuideIndex < this.mGuideList.Count && this.mGuideList[this.mCurGuideIndex].IsMarkGroupOver);
			}
		}

		public GuideGroup(int group)
		{
			this.Group = group;
		}

		public void ResetToFirst()
		{
			this.mCurGuideIndex = 0;
			this.IsGuideActive = false;
			this.mGuideList.Sort(new Comparison<GuideData>(GuideData.GroupInSort));
		}

		public void AddGuideData(GuideData guide)
		{
			if (guide == null)
			{
				return;
			}
			this.mGuideList.Add(guide);
		}

		public void SortList()
		{
			this.mGuideList.Sort(new Comparison<GuideData>(GuideData.GroupInSort));
		}

		internal void SkipAll()
		{
			this.mCurGuideIndex = this.mGuideList.Count;
		}

		public bool MovetoNextGuide()
		{
			this.mCurGuideIndex++;
			this.IsGuideActive = false;
			return this.mCurGuideIndex < this.mGuideList.Count;
		}

		internal void HangGuideCurStep()
		{
			this.IsGuideActive = false;
		}

		public void ActiveGuide()
		{
			this.IsGuideActive = true;
		}

		internal static int Sort(GuideGroup x, GuideGroup y)
		{
			return y.SortValue.CompareTo(y.SortValue);
		}

		private List<GuideData> mGuideList = new List<GuideData>();

		private int mCurGuideIndex;
	}
}
