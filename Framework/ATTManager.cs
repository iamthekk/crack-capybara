using System;
using Framework;
using Habby;
using UnityEngine;

public class ATTManager
{
	public void OnInit()
	{
	}

	private bool CheckShowIDFA()
	{
		if (ATTControl.GetAuthorizationStatus(false) == ATTStatus.NotDetermined)
		{
			PlayerPrefs.SetInt("IDFA", 0);
		}
		if (PlayerPrefs.GetInt("IDFA") != 0)
		{
			return false;
		}
		ATTControl.RequestAdvertisingIdentifier(false, delegate(ATTStatus status, string idfa)
		{
			bool flag = status != ATTStatus.Denied;
			this.m_state = status;
			this.m_id = idfa;
			this.m_isFinished = true;
			GameAdsManager.Instance.InitUmp(flag);
		});
		return true;
	}

	public void OnUpdate()
	{
		if (!this.m_isWait)
		{
			return;
		}
		if (!this.m_isFinished)
		{
			return;
		}
		this.m_isWait = true;
		PlayerPrefs.SetInt("IDFA", (int)this.m_state);
	}

	private ATTStatus m_state;

	private string m_id = string.Empty;

	private bool m_isFinished;

	private bool m_isWait;
}
