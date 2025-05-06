using System;
using Framework.Logic.Modules;
using UnityEngine;

namespace Framework
{
	public class GameLauncher : MonoBehaviour
	{
		private void Awake()
		{
			GameLauncher.Builder = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.OnGameAppStarUp();
		}

		private void OnGameAppStarUp()
		{
			this.m_gameApp.OnStarUp();
			if (!GameApp.Config.GetBool("IsReleaseServer") && this.showDebugLog)
			{
				SRDebug.Init();
			}
		}

		private void Update()
		{
			this.m_gameApp.OnUpdate();
		}

		private void LateUpdate()
		{
			this.m_gameApp.OnLateUpdate();
		}

		private void FixedUpdate()
		{
			this.m_gameApp.OnFixedUpdate();
		}

		private void OnApplicationQuit()
		{
			this.m_gameApp.OnShutdown();
			this.m_gameApp = null;
		}

		[ContextMenu("OnShutdown")]
		public void OnShutdown()
		{
			this.m_gameApp.OnShutdown();
		}

		[ContextMenu("OnRestart")]
		public void OnRestart()
		{
			this.m_gameApp.OnRestart();
		}

		[ContextMenu("OpenPersistentDataPath")]
		public void OpenPersistentDataPath()
		{
			this.OpenFolder(Application.persistentDataPath);
		}

		private void OpenFolder(string path)
		{
		}

		[SerializeField]
		private GameApp m_gameApp;

		[Header("Language Setting")]
		public bool m_isReadyLanguage;

		public LanguageType m_languageType;

		public bool showDebugLog = true;

		public static GameLauncher Builder;
	}
}
