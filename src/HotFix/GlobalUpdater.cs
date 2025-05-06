using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class GlobalUpdater : MonoBehaviour
	{
		public static GlobalUpdater Instance
		{
			get
			{
				return GlobalUpdater.instance;
			}
		}

		public void RegisterUpdater(IUpdater updater)
		{
			if (this.updaters.Contains(updater))
			{
				return;
			}
			updater.OnInit();
			this.updaters.Add(updater);
		}

		public void RegisterUpdater(Action action)
		{
			if (this.updateActions.Contains(action))
			{
				return;
			}
			this.updateActions.Add(action);
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void UnRegisterUpdater(IUpdater updater)
		{
			if (this.updaters.Contains(updater))
			{
				this.updaters.Remove(updater);
			}
		}

		public void UnRegisterUpdater(Action action)
		{
			if (this.updateActions.Contains(action))
			{
				this.updateActions.Remove(action);
			}
		}

		public static GlobalUpdater OnCreate()
		{
			if (GlobalUpdater.instance != null)
			{
				return GlobalUpdater.instance;
			}
			GameObject gameObject = new GameObject("GlobalUpdater");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.transform.parent = GameApp.View.transform;
			GlobalUpdater.instance = gameObject.AddComponent<GlobalUpdater>();
			return GlobalUpdater.instance;
		}

		public void Dispose()
		{
			Object.Destroy(base.gameObject);
		}

		private void OnDestroy()
		{
			this.ClearAll();
		}

		public void ClearAll()
		{
			for (int i = 0; i < this.updaters.Count; i++)
			{
				IUpdater updater = this.updaters[i];
				if (updater != null)
				{
					updater.OnDeInit();
				}
			}
			List<IUpdater> list = this.updaters;
			if (list == null)
			{
				return;
			}
			list.Clear();
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			try
			{
				DxxTools.Time.TryTriggerServerDayChange();
				for (int i = 0; i < this.updaters.Count; i++)
				{
					IUpdater updater = this.updaters[i];
					if (updater != null)
					{
						updater.OnUpdate(deltaTime, unscaledDeltaTime);
					}
				}
				for (int j = 0; j < this.updateActions.Count; j++)
				{
					Action action = this.updateActions[j];
					if (action != null)
					{
						action();
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		private static GlobalUpdater instance;

		private List<IUpdater> updaters = new List<IUpdater>();

		private List<Action> updateActions = new List<Action>();
	}
}
