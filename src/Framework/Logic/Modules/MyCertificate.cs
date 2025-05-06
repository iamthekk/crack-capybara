using System;
using Framework.NetWork;
using UnityEngine.Networking;

namespace Framework.Logic.Modules
{
	public class MyCertificate : CertificateHandler
	{
		public MyCertificate(NetWorkUsingType usingType, string[] pubKeys, string[] certHashs)
		{
			this.m_pubKeys = pubKeys;
			this.m_certHashs = certHashs;
			this.m_usingType = usingType;
		}

		protected override bool ValidateCertificate(byte[] certificateData)
		{
			return true;
		}

		private NetWorkUsingType m_usingType;

		private string[] m_pubKeys;

		private string[] m_certHashs;
	}
}
