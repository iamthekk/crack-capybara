using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace HotFix
{
	public static class EventPointActionUtility
	{
		public static void ParseDefaultActionData(Chapter_eventPoint table, out EventPointDefaultActionData data)
		{
			data = new EventPointDefaultActionData();
			if (table.defaultAction.Length != 0)
			{
				int.TryParse(table.defaultAction[0], out data.createActionId);
			}
			if (table.defaultAction.Length > 1)
			{
				int.TryParse(table.defaultAction[1], out data.arrivedActionId);
			}
			if (table.defaultAction.Length > 2)
			{
				int.TryParse(table.defaultAction[2], out data.leaveActionId);
			}
		}

		public static void ParseActionData(Chapter_eventPoint table, out List<EventPointActionData> list)
		{
			list = new List<EventPointActionData>();
			string[] action = table.action;
			for (int i = 0; i < action.Length; i++)
			{
				List<string> listString = action[i].GetListString(',');
				if (listString.Count >= 2)
				{
					EventPointActionData eventPointActionData = new EventPointActionData();
					int.TryParse(listString[0], out eventPointActionData.actionId);
					float.TryParse(listString[1], out eventPointActionData.duration);
					list.Add(eventPointActionData);
				}
			}
		}
	}
}
