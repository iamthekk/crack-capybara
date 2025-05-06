using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Logic.Modules;
using Framework.SDKManager;
using UnityEngine;

namespace Framework.RunTimeManager
{
	public class RunTimeManager : MonoBehaviour
	{
		public void PreLoad()
		{
			if (this.m_model == null)
			{
				this.m_model = new RunTimeModel_HyBridCLR();
				this.m_model.m_onFinished = this.m_onFinished;
			}
			((RunTimeModel_HyBridCLR)this.m_model).PreLoad();
		}

		public async Task StopPreLoad()
		{
			if (this.m_model != null)
			{
				await ((RunTimeModel_HyBridCLR)this.m_model).WaitPreLoad();
				this.m_model.OnShutDown();
				this.m_model = null;
			}
		}

		public void Load()
		{
			if (this.m_model == null)
			{
				this.m_model = new RunTimeModel_HyBridCLR();
				this.m_model.m_onFinished = this.m_onFinished;
			}
			this.m_model.Load();
		}

		public void OnFixedUpdate()
		{
			if (this.m_model == null)
			{
				return;
			}
			this.m_model.OnFixedUpdate();
		}

		private void OnStarUp()
		{
			if (this.m_model == null)
			{
				return;
			}
			this.m_model.OnStarUp();
		}

		public void OnShutDown()
		{
			if (this.m_model == null)
			{
				return;
			}
			this.m_model.OnShutDown();
			this.m_model = null;
			this.m_onFinished = null;
		}

		public void OnApplicationFocus(bool hasFocus)
		{
			if (this.m_model == null)
			{
				return;
			}
			this.m_model.OnApplicationFocus(hasFocus);
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				SDKManager.SDKTGA.ResumeFromBackground = true;
			}
			if (this.m_model != null)
			{
				this.m_model.OnApplicationPause(pauseStatus);
				return;
			}
			if (!pauseStatus)
			{
				SDKManager sdk = GameApp.SDK;
				if (sdk == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze = sdk.Analyze;
				if (analyze == null)
				{
					return;
				}
				analyze.AppStart(null);
				return;
			}
			else
			{
				SDKManager sdk2 = GameApp.SDK;
				if (sdk2 == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze2 = sdk2.Analyze;
				if (analyze2 == null)
				{
					return;
				}
				analyze2.AppEnd(null);
				return;
			}
		}

		public void OnApplicationQuit()
		{
			if (this.m_model == null)
			{
				SDKManager sdk = GameApp.SDK;
				if (sdk == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze = sdk.Analyze;
				if (analyze == null)
				{
					return;
				}
				analyze.AppEnd(null);
				return;
			}
			else
			{
				if (this.m_model.IsOnApplicaitonQuitValid())
				{
					this.m_model.OnApplicationQuit();
					return;
				}
				SDKManager sdk2 = GameApp.SDK;
				if (sdk2 == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze2 = sdk2.Analyze;
				if (analyze2 == null)
				{
					return;
				}
				analyze2.AppEnd(null);
				return;
			}
		}

		public string GetInfoByID(LanguageType languageType, int id)
		{
			return this.GetInfoByID(languageType, id.ToString());
		}

		public string GetInfoByID(LanguageType languageType, string id)
		{
			if (this.m_model == null)
			{
				return string.Empty;
			}
			return this.m_model.GetLanguageInfoByID(languageType, id);
		}

		public void AddIDConnecter(RunTimeIDConnecterData data)
		{
		}

		public void RemoveIDConnecter(RunTimeIDConnecterData data)
		{
		}

		public void RemoveAllIDConnecters()
		{
		}

		public List<RunTimeIDConnecterData> GetIDConnecterDatas(int id)
		{
			return null;
		}

		public Dictionary<int, List<RunTimeIDConnecterData>> GetAllConnecterDatas()
		{
			return new Dictionary<int, List<RunTimeIDConnecterData>>();
		}

		public Action m_onFinished;

		public BaseRunTimeModel m_model;
	}
}
