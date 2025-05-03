using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class MainDataModule : IDataModule
	{
		public int GetName()
		{
			return 101;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(101, new HandlerEvent(this.OnEventRefreshMainOpenData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(101, new HandlerEvent(this.OnEventRefreshMainOpenData));
		}

		public void Reset()
		{
		}

		private void OnEventRefreshMainOpenData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshMainOpenData eventArgsRefreshMainOpenData = eventargs as EventArgsRefreshMainOpenData;
			if (eventArgsRefreshMainOpenData == null)
			{
				return;
			}
			this.m_mainOpenData = eventArgsRefreshMainOpenData.m_openData;
		}

		public int ChapterMaxProcess
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule).ChapterMaxProcess;
			}
		}

		public int OldChapter
		{
			get
			{
				HLog.LogError("未完成：MainDataModule.OldChapter");
				return 1001;
			}
		}

		public int GetChapterLevelCount(int chapter = -1)
		{
			HLog.LogError("未完成：MainDataModule.GetChapterLevelCount(int chapter = -1)");
			return 10;
		}

		public string GetChapterName(int chapter = -1)
		{
			HLog.LogError("未完成：MainDataModule.GetChapterName(int chapter = -1)");
			return "UnKnow";
		}

		public string GetChatperPrefabPath(int chapter = -1)
		{
			HLog.LogError("未完成：MainDataModule.GetChatperPrefabPath(int chapter = -1)");
			return "UnKnow";
		}

		public string GetChatperPreviewPrefabPath(int chapter = -1)
		{
			HLog.LogError("未完成：MainDataModule.GetChatperPreviewPrefabPath(int chapter = -1)");
			return "UnKnow";
		}

		public int GetMissionId()
		{
			return (int)GameApp.Data.GetDataModule(DataName.LoginDataModule).userMission.MissionId;
		}

		public bool IsInChapterBattle
		{
			get
			{
				HLog.LogError("未完成：MainDataModule.IsInChapterBattle");
				return false;
			}
		}

		public bool IsMaxChapter(int chatper)
		{
			HLog.LogError("未完成：MainDataModule.IsMaxChapter(int chatper)");
			return false;
		}

		public string GetPVELevelShortName(int id)
		{
			return GameApp.Data.GetDataModule(DataName.ChapterDataModule).GetPVELevelShortName(id);
		}

		public string GetPVELevelLongName(int id)
		{
			return GameApp.Data.GetDataModule(DataName.ChapterDataModule).GetPVELevelLongName(id);
		}

		public MainOpenData m_mainOpenData;
	}
}
