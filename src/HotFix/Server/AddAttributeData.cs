using System;
using System.Collections.Generic;

namespace Server
{
	public class AddAttributeData
	{
		public void Merge(AddAttributeData data)
		{
			if (data == null)
			{
				return;
			}
			if (data.m_attributeDatas != null)
			{
				this.m_attributeDatas.AddRange(data.m_attributeDatas);
			}
			if (data.m_skillIDs != null)
			{
				this.m_skillIDs.AddRange(data.m_skillIDs);
			}
		}

		public void AddChapterEventData(string attrs, List<int> ids)
		{
			this.AddAttrs(attrs);
			this.AddSkills(ids);
		}

		private void AddAttrs(string attr)
		{
			List<MergeAttributeData> mergeAttributeData = attr.GetMergeAttributeData();
			this.m_attributeDatas.AddRange(mergeAttributeData);
		}

		private void AddSkills(List<int> ids)
		{
			this.m_skillIDs.AddRange(ids);
		}

		public void Clear()
		{
			this.m_attributeDatas.Clear();
			this.m_skillIDs.Clear();
		}

		public List<MergeAttributeData> m_attributeDatas = new List<MergeAttributeData>();

		public List<int> m_skillIDs = new List<int>();
	}
}
