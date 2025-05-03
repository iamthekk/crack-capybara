using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public class EventRecordEventQueueData
	{
		public void EventsToJson(List<GameEventRandomData> list)
		{
			this.eventArr = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				EventRecordEventData eventRecordEventData = new EventRecordEventData();
				eventRecordEventData.ToRecordData(list[i]);
				string text = JsonManager.SerializeObject(eventRecordEventData);
				this.eventArr[i] = text;
			}
		}

		public void ActivityToJson(List<GameEventRandomData> list)
		{
			this.activityArr = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				EventRecordEventData eventRecordEventData = new EventRecordEventData();
				eventRecordEventData.ToRecordData(list[i]);
				string text = JsonManager.SerializeObject(eventRecordEventData);
				this.activityArr[i] = text;
			}
		}

		public string[] eventArr;

		public string[] activityArr;
	}
}
