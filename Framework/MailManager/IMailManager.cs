using System;

namespace Framework.MailManager
{
	public interface IMailManager
	{
		void SetData(string version, string url, MailServerType serverType);

		void OnInit();

		void OnDeInit();
	}
}
