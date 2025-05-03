using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HotFix
{
	public class DelayCall : MonoBehaviour
	{
		private long curTime
		{
			get
			{
				return (long)(Time.time * 1000f);
			}
		}

		[HideInInspector]
		public static DelayCall Instance
		{
			get
			{
				if (DelayCall._instance == null)
				{
					GameObject gameObject = new GameObject("DelayCall");
					Object.DontDestroyOnLoad(gameObject);
					DelayCall._instance = gameObject.AddComponent<DelayCall>();
				}
				return DelayCall._instance;
			}
		}

		public void Update()
		{
			if (this.isPause)
			{
				return;
			}
			while (this.removeList.Count > 0)
			{
				this.callList.Remove(this.removeList[0]);
				this.removeList.RemoveAt(0);
			}
			for (int i = 0; i < this.callList.Count; i++)
			{
				DelayCall.DelayCallHandle delayCallHandle = this.callList[i];
				long curTime = this.curTime;
				if (curTime >= delayCallHandle.exeTime)
				{
					Delegate method = delayCallHandle.method;
					object[] args = delayCallHandle.args;
					if (delayCallHandle.isRepeat)
					{
						while (curTime >= delayCallHandle.exeTime)
						{
							delayCallHandle.exeTime += (long)delayCallHandle.delay;
							method.DynamicInvoke(args);
						}
					}
					else
					{
						method.DynamicInvoke(args);
						this.WaitClear(delayCallHandle);
					}
				}
			}
		}

		public void SetPause(bool pause)
		{
			this.isPause = pause;
			if (this.isPause)
			{
				this.pauseStartTime = Time.time;
				return;
			}
			int num = (int)((Time.time - this.pauseStartTime) * 1000f);
			for (int i = 0; i < this.callList.Count; i++)
			{
				this.callList[i].exeTime += (long)num;
			}
		}

		private void AddCall(bool isRepeat, int delay, Delegate method, params object[] args)
		{
			if (method == null)
			{
				HLog.LogError("call method is null");
				return;
			}
			if (delay < 1)
			{
				method.DynamicInvoke(args);
				return;
			}
			DelayCall.DelayCallHandle delayCallHandle;
			if (this.pool.Count > 0)
			{
				delayCallHandle = this.pool[this.pool.Count - 1];
				this.pool.Remove(delayCallHandle);
			}
			else
			{
				delayCallHandle = new DelayCall.DelayCallHandle();
			}
			delayCallHandle.isRepeat = isRepeat;
			delayCallHandle.delay = delay;
			delayCallHandle.exeTime = (long)delay + this.curTime;
			delayCallHandle.method = method;
			delayCallHandle.args = args;
			this.callList.Add(delayCallHandle);
		}

		public void CallOnce(int delay, DelayCall.CallAction act)
		{
			this.AddCall(false, delay, act, Array.Empty<object>());
		}

		public void CallOnce<T1>(int delay, DelayCall.CallAction<T1> act, params object[] args)
		{
			this.AddCall(false, delay, act, args);
		}

		public void CallLoop(int delay, DelayCall.CallAction act)
		{
			this.AddCall(true, delay, act, Array.Empty<object>());
		}

		public void CallLoop<T1>(int delay, DelayCall.CallAction<T1> act, params object[] args)
		{
			this.AddCall(true, delay, act, args);
		}

		public void ClearCall(DelayCall.CallAction act)
		{
			this.WaitClear(act);
		}

		public void ClearCall<T1>(DelayCall.CallAction<T1> act)
		{
			this.WaitClear(act);
		}

		private void WaitClear(Delegate method)
		{
			DelayCall.DelayCallHandle delayCallHandle = this.callList.FirstOrDefault((DelayCall.DelayCallHandle t) => t.method == method);
			if (delayCallHandle != null)
			{
				this.WaitClear(delayCallHandle);
			}
		}

		private void WaitClear(DelayCall.DelayCallHandle call)
		{
			if (!this.removeList.Contains(call))
			{
				call.Clear();
				this.removeList.Add(call);
				this.pool.Add(call);
			}
		}

		public void ClearAll(bool isClearPool)
		{
			for (int i = 0; i < this.callList.Count; i++)
			{
				this.callList[i].Clear();
				this.callList[i] = null;
			}
			this.callList.Clear();
			this.removeList.Clear();
			if (isClearPool)
			{
				for (int j = 0; j < this.pool.Count; j++)
				{
					this.pool[j].Clear();
					this.pool[j] = null;
				}
				this.pool.Clear();
			}
		}

		private List<DelayCall.DelayCallHandle> callList = new List<DelayCall.DelayCallHandle>();

		private List<DelayCall.DelayCallHandle> removeList = new List<DelayCall.DelayCallHandle>();

		private List<DelayCall.DelayCallHandle> pool = new List<DelayCall.DelayCallHandle>();

		private bool isPause;

		private float pauseStartTime;

		private static DelayCall _instance;

		public delegate void CallAction();

		public delegate void CallAction<T1>(T1 param1);

		public class DelayCallHandle
		{
			public void Clear()
			{
				this.method = null;
				this.args = null;
			}

			public int delay;

			public bool isRepeat;

			public long exeTime;

			public Delegate method;

			public object[] args;
		}
	}
}
