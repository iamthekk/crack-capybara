using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class CrossArenaProgress
	{
		public void BuildProgress()
		{
			DateTime serverLocal = DxxTools.Time.ServerLocal;
			CrossArena_CrossArenaTime elementById = GameApp.Table.GetManager().GetCrossArena_CrossArenaTimeModelInstance().GetElementById(1);
			string openTime = elementById.OpenTime;
			string closeTime = elementById.CloseTime;
			TimeSpan timeSpan = TimeSpan.Parse(openTime);
			TimeSpan timeSpan2 = TimeSpan.Parse(closeTime);
			if (timeSpan2.Hours < timeSpan.Hours || (timeSpan2 == timeSpan && timeSpan2.Minutes < timeSpan.Minutes))
			{
				timeSpan2 = timeSpan2.Add(new TimeSpan(1, 0, 0, 0));
			}
			DateTime dateTime = (this.GetSeasonStartDateTime(serverLocal) + timeSpan).AddHours((double)(-(double)DxxTools.Time.Timezone));
			this.SeasonOpenTime = DxxTools.GetTotalSecend(dateTime);
			DateTime dateTime2 = (this.GetSeasonEndDateTime(serverLocal) + timeSpan2).AddHours((double)(-(double)DxxTools.Time.Timezone));
			this.SeasonCloseTime = DxxTools.GetTotalSecend(dateTime2);
			DateTime dateTime3 = new DateTime(serverLocal.Year, serverLocal.Month, serverLocal.Day, 0, 0, 0);
			DateTime dateTime4 = (dateTime3 + timeSpan).AddHours((double)(-(double)DxxTools.Time.Timezone));
			this.DailyOpenTime = DxxTools.GetTotalSecend(dateTime4);
			DateTime dateTime5 = (dateTime3 + timeSpan2).AddHours((double)(-(double)DxxTools.Time.Timezone));
			this.DailyCloseTime = DxxTools.GetTotalSecend(dateTime5);
			this.DailyCloseTime += 1L;
		}

		private DateTime GetSeasonStartDateTime(DateTime datetime)
		{
			DateTime dateTime = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
			while (dateTime.DayOfWeek != DayOfWeek.Monday)
			{
				dateTime = dateTime.AddDays(-1.0);
			}
			return dateTime;
		}

		private DateTime GetSeasonEndDateTime(DateTime datetime)
		{
			DateTime dateTime = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
			while (dateTime.DayOfWeek != DayOfWeek.Sunday)
			{
				dateTime = dateTime.AddDays(1.0);
			}
			return dateTime;
		}

		public float CalcTimeSecToSeasonEnd()
		{
			long num = this.ServerTime();
			float num2 = (float)(this.SeasonCloseTime - num);
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return num2;
		}

		public float CalcTimeSecToDailyEnd()
		{
			long num = this.ServerTime();
			float num2 = (float)(this.DailyCloseTime - num);
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return num2;
		}

		private long ServerTime()
		{
			return DxxTools.Time.ServerTimestamp;
		}

		public long DailyOpenTime;

		public long DailyCloseTime;

		public long SeasonOpenTime;

		public long SeasonCloseTime;
	}
}
