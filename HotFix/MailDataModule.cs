using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Framework.DataModule;
using Framework.EventSystem;
using Habby.Mail.Data;

namespace HotFix
{
	public class MailDataModule : IDataModule
	{
		public int GetName()
		{
			return 131;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_MailData_GetListData, new HandlerEvent(this.MailListDataSet));
			manager.RegisterEvent(LocalMessageName.CC_MailData_ReceiveAwardsData, new HandlerEvent(this.MailReceiveDataSet));
			manager.RegisterEvent(LocalMessageName.CC_MailData_DeleteData, new HandlerEvent(this.MailDeleteDataSet));
			manager.RegisterEvent(LocalMessageName.CC_MailData_ReadMail, new HandlerEvent(this.MailReadDataSet));
			manager.RegisterEvent(LocalMessageName.CC_MailData_SetIsShowRedPoint, new HandlerEvent(this.OnEventSetIsShowRedPoint));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_GetListData, new HandlerEvent(this.MailListDataSet));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_ReceiveAwardsData, new HandlerEvent(this.MailReceiveDataSet));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_DeleteData, new HandlerEvent(this.MailDeleteDataSet));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_ReadMail, new HandlerEvent(this.MailReadDataSet));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_SetIsShowRedPoint, new HandlerEvent(this.OnEventSetIsShowRedPoint));
		}

		public void Reset()
		{
		}

		private void MailListDataSet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventMailList eventMailList = eventArgs as EventMailList;
			if (eventMailList == null)
			{
				return;
			}
			this.RefreshMailDatas(eventMailList.m_mailDatas);
		}

		private void MailReceiveDataSet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventMailReceiveAwards eventMailReceiveAwards = eventArgs as EventMailReceiveAwards;
			if (eventMailReceiveAwards == null)
			{
				return;
			}
			this.SetMailReceiveAwards(eventMailReceiveAwards.mailReceiveAwardsResponse);
		}

		private void MailDeleteDataSet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventMailDelete eventMailDelete = eventArgs as EventMailDelete;
			if (eventMailDelete == null)
			{
				return;
			}
			this.SetMailDelet(eventMailDelete.idList);
		}

		private void MailReadDataSet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsReadMail eventArgsReadMail = eventArgs as EventArgsReadMail;
			if (eventArgsReadMail == null)
			{
				return;
			}
			this.SetMailData(eventArgsReadMail.mailData);
		}

		private void OnEventSetIsShowRedPoint(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBool eventArgsBool = eventargs as EventArgsBool;
			if (eventArgsBool == null)
			{
				return;
			}
			this.m_isCanShowRed = eventArgsBool.Value;
		}

		private void RefreshMailDatas(List<MailData> mailDatas)
		{
			List<string> list = this.m_mails.Keys.ToList<string>();
			this.m_mails.Clear();
			for (int i = 0; i < mailDatas.Count; i++)
			{
				MailData mailData = mailDatas[i];
				if (mailData != null)
				{
					this.m_mails[mailData.mailId] = mailData;
					if (list.Count == 0)
					{
						if (!mailData.IsReadShow())
						{
							this.m_isCanShowRed = true;
						}
					}
					else if (!list.Contains(mailData.mailId))
					{
						this.m_isCanShowRed = true;
					}
				}
			}
		}

		public Dictionary<string, MailData> GetMailDatas()
		{
			return this.m_mails;
		}

		public void SetMailData(MailData mailData)
		{
			if (mailData == null)
			{
				return;
			}
			this.m_mails[mailData.mailId].readed = true;
		}

		public MailData GetMailData(string mailId)
		{
			MailData mailData;
			this.m_mails.TryGetValue(mailId, out mailData);
			return mailData;
		}

		public void SetMailReceiveAwards(MailRewardObject mailRewardObject)
		{
			if (mailRewardObject.mail == null)
			{
				return;
			}
			MailData mail;
			if (this.m_mails.TryGetValue(mailRewardObject.mail.mailId, out mail))
			{
				mail = mailRewardObject.mail;
			}
		}

		public void SetMailDelet(List<string> idlist)
		{
			for (int i = 0; i < idlist.Count; i++)
			{
				MailData mailData;
				if (this.m_mails.TryGetValue(idlist[i], out mailData))
				{
					this.m_mails.Remove(idlist[i]);
				}
			}
		}

		public bool GetIsCanShowRed()
		{
			bool flag = false;
			foreach (KeyValuePair<string, MailData> keyValuePair in this.m_mails)
			{
				if (!keyValuePair.Value.isReward && keyValuePair.Value.rewards.Length != 0)
				{
					flag = true;
					return flag;
				}
			}
			return flag;
		}

		public static string GetFinalContent(string mailContent)
		{
			string text;
			try
			{
				text = Regex.Replace(mailContent, "\\[mail_(\\d+):([^\\]]*)\\]", new MatchEvaluator(MailDataModule.MailEvaluator));
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				text = mailContent;
			}
			return text;
		}

		private static string MailEvaluator(Match match)
		{
			int num = int.Parse(match.Groups[1].Value);
			string value = match.Groups[2].Value;
			if (string.IsNullOrEmpty(value))
			{
				HLog.LogError(string.Format("邮件替换格式无参数：[mail_{0}:]", num));
				return match.Value;
			}
			if (num == 1)
			{
				return WorldBossDataModule.GetRankLevelShow(int.Parse(value));
			}
			HLog.LogError(string.Format("邮件替换格式Id未定义：[mail_{0}:]", num));
			return match.Value;
		}

		private Dictionary<string, MailData> m_mails = new Dictionary<string, MailData>();

		private bool m_isCanShowRed;

		private const string regexStr = "\\[mail_(\\d+):([^\\]]*)\\]";
	}
}
