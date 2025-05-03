using System;
using System.Collections.Generic;
using System.Linq;
using Proto.Common;

namespace HotFix
{
	public class SocialityInteractiveData
	{
		public void RefreshData(InteractDto dto)
		{
			this.m_params.Clear();
			this.m_rowID = dto.RowId;
			this.m_id = (int)dto.Id;
			this.m_time = dto.Time;
			this.m_status = dto.Status;
			this.m_params = dto.Params.ToList<string>();
		}

		public void RefreshData(InteractDto dto, long currentTime)
		{
			this.RefreshData(dto);
			this.m_duration = currentTime - this.m_time;
		}

		public long m_rowID;

		public int m_id;

		public long m_time;

		public long m_duration;

		public bool m_status;

		public List<string> m_params = new List<string>();
	}
}
