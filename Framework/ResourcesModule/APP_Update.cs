using System;
using UnityEngine.Networking;

namespace Framework.ResourcesModule
{
	public class APP_Update : Singleton<APP_Update>
	{
		public void CheckUpdate(string url, string curVersion, Action<string> success, Action<string, string> fail)
		{
			this.m_url = url;
			this.m_curVersion = curVersion;
			this.m_success = success;
			this.m_fail = fail;
			this.Check();
		}

		private void Check()
		{
			if (GameApp.Config.GetBool("IsBuildIn"))
			{
				Action<string> success = this.m_success;
				if (success == null)
				{
					return;
				}
				success(this.m_curVersion);
				return;
			}
			else
			{
				if (!(this.m_url == string.Empty))
				{
					GameApp.Http.Get(this.m_url, delegate(UnityWebRequest x)
					{
						if (x.result != 1)
						{
							Action<string, string> fail2 = this.m_fail;
							if (fail2 == null)
							{
								return;
							}
							fail2("-410", "-410");
							return;
						}
						else
						{
							Action<string> success2 = this.m_success;
							if (success2 == null)
							{
								return;
							}
							success2(x.downloadHandler.text);
							return;
						}
					});
					return;
				}
				Action<string, string> fail = this.m_fail;
				if (fail == null)
				{
					return;
				}
				fail("-409", "-409");
				return;
			}
		}

		private string m_url = string.Empty;

		private string m_curVersion = string.Empty;

		private Action<string> m_success;

		private Action<string, string> m_fail;
	}
}
