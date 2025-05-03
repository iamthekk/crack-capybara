using System;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Social;

namespace HotFix
{
	public class ReportConquerDataModule : IDataModule
	{
		public int GetName()
		{
			return 127;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ReportConquerData_SetData, new HandlerEvent(this.OnEventSetData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ReportConquerData_SetData, new HandlerEvent(this.OnEventSetData));
		}

		public void Reset()
		{
		}

		private void OnEventSetData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetReportConquerData eventArgsSetReportConquerData = eventargs as EventArgsSetReportConquerData;
			if (eventArgsSetReportConquerData == null)
			{
				return;
			}
			this.m_socialityInteractiveData = eventArgsSetReportConquerData.m_socialityInteractiveData;
			this.m_interactDetailResponse = eventArgsSetReportConquerData.m_interactDetailResponse;
		}

		public SocialityInteractiveData m_socialityInteractiveData;

		public InteractDetailResponse m_interactDetailResponse;
	}
}
