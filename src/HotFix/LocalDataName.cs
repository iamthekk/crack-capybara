using System;
using Framework;

namespace HotFix
{
	public class LocalDataName
	{
		public static string FunctionOpen_LocalCache
		{
			get
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				return string.Format("FunctionOpen_LocalCache_{0}", dataModule.userId);
			}
		}

		public const string APPKEY = "App_key";

		public const string CURRENTLANGUAGETYPEKEY = "CurrentLanguageType_Key";

		public const string PUCHASECACHE = "PUCHASECACHE_Key";

		public const string ACCOUNT = "ACCOUNT_Key";

		public const string DEVICEID = "DEVICEID_Key";

		public const string ACCOUNT2 = "ACCOUNT2_Key";

		public const string Save_Music = "Save_Music";

		public const string Save_Sound = "Save_Sound";

		public const string RedPoint_LocalHideTime = "RedPoint_LocalHideTime";

		public const string CrossArena_LastDanCache = "CrossArena_LastDanCache";

		public const string Guild_ShowLevelUp = "Guild_ShowLevelUp";

		public const string Guild_ShowRaceDanChange = "Guild_ShowRaceDanChange";

		public const string Guild_ShowRaceMatch = "Guild_ShowRaceMatch";
	}
}
