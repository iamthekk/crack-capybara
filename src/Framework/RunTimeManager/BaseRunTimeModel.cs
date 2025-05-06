using System;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Modules;
using UnityEngine;

namespace Framework.RunTimeManager
{
	public abstract class BaseRunTimeModel
	{
		public abstract bool IsOnApplicaitonQuitValid();

		public abstract void Load();

		public abstract void OnFixedUpdate();

		public abstract void OnStarUp();

		public abstract void OnShutDown();

		public abstract void OnApplicationFocus(bool hasFocus);

		public abstract void OnApplicationPause(bool pauseStatus);

		public abstract void OnApplicationQuit();

		public abstract string GetLanguageInfoByID(LanguageType languageType, int id);

		public abstract string GetLanguageInfoByID(LanguageType languageType, string id);

		public Action m_onFinished;

		[SerializeField]
		[Label]
		protected bool m_isLoadingFinished;
	}
}
