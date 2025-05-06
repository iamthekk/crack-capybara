using System;
using Framework.Logic.AttributeExpansion;
using UnityEngine;

namespace Framework.MailManager
{
	public class MailManager : MonoBehaviour
	{
		public string Url
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_url))
				{
					this.m_url = ((this.m_serverType == MailServerType.Release) ? this.m_serverUrl : this.m_testServerUrl);
				}
				return this.m_url;
			}
		}

		public string Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				this.m_version = value;
			}
		}

		public bool IsEnable
		{
			get
			{
				return this.m_isEnable;
			}
		}

		public void SetManager(IMailManager manager)
		{
			this.m_manager = manager;
			if (this.m_manager == null)
			{
				return;
			}
			this.m_manager.SetData(this.m_version, this.Url, this.m_serverType);
		}

		public IMailManager Manager
		{
			get
			{
				return this.m_manager;
			}
		}

		public void OnInit()
		{
			if (this.m_manager != null)
			{
				this.m_manager.OnInit();
			}
		}

		public void OnDeInit()
		{
			if (this.m_manager != null)
			{
				this.m_manager.OnDeInit();
			}
			this.m_manager = null;
		}

		[SerializeField]
		private bool m_isEnable;

		[Header("正式版")]
		[SerializeField]
		private string m_serverUrl;

		[Header("测试版")]
		[SerializeField]
		private string m_testServerUrl;

		[Header("模式")]
		public MailServerType m_serverType;

		[Label]
		[Header("版本")]
		public string m_version = string.Empty;

		[SerializeField]
		[Label]
		private string m_url = string.Empty;

		private IMailManager m_manager;
	}
}
