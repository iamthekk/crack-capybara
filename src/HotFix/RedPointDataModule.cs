using System;
using System.Collections.Generic;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class RedPointDataModule : IDataModule
	{
		public RedPointDataRecord RootRecord
		{
			get
			{
				return this.mRecordRoot;
			}
		}

		public int GetName()
		{
			return 106;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public RedPointDataModule()
		{
			this.mRecordRoot = new RedPointDataRecord();
		}

		public void OnInit()
		{
		}

		public void ReCalcAll()
		{
			this.mRecordRoot.ReCalc(true, false);
		}

		public RedPointDataRecord GetRecord(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			return this.mRecordRoot.GetRedPointRecordByName(path);
		}

		public RedPointDataRecord RegRecord(string path, IRedPointRecordCalculator calculator = null)
		{
			return this.mRecordRoot.AddRecordPath(path, calculator);
		}

		public void RemoveRecord(string path)
		{
			RedPointDataRecord record = this.GetRecord(path);
			if (record == null)
			{
				return;
			}
			if (record.Parent != null)
			{
				record.Parent.RemoveChild(record.Name);
			}
		}

		public RedPointDataRecord GetHeroRecord(long serverid)
		{
			return null;
		}

		public void LinkRecord(string listener, string dispatcher)
		{
			if (string.IsNullOrEmpty(listener))
			{
				return;
			}
			RedPointDataModule.RedPointDataLink redPointDataLink;
			if (this.mRecordLink.TryGetValue(listener, out redPointDataLink))
			{
				redPointDataLink.IsActive = false;
				this.mRecordLink.Remove(listener);
			}
			if (string.IsNullOrEmpty(dispatcher))
			{
				return;
			}
			RedPointDataRecord record = this.GetRecord(listener);
			RedPointDataRecord record2 = this.GetRecord(dispatcher);
			if (record == null || record2 == null)
			{
				return;
			}
			redPointDataLink = new RedPointDataModule.RedPointDataLink();
			redPointDataLink.Listener = record;
			redPointDataLink.Dispatcher = record2;
			redPointDataLink.IsActive = true;
			this.mRecordLink[listener] = redPointDataLink;
		}

		[SerializeField]
		private RedPointDataRecord mRecordRoot = new RedPointDataRecord();

		private Dictionary<string, RedPointDataModule.RedPointDataLink> mRecordLink = new Dictionary<string, RedPointDataModule.RedPointDataLink>();

		private class RedPointDataLink
		{
			public bool IsActive
			{
				get
				{
					return this.mIsActive;
				}
				set
				{
					this.mIsActive = value;
					if (this.mIsActive)
					{
						if (this.Dispatcher != null)
						{
							this.Dispatcher.RegRecordChange(new Action<RedNodeListenData>(this.OnDispatcherChange));
						}
					}
					else
					{
						this.Dispatcher.UnRegRecordChange(new Action<RedNodeListenData>(this.OnDispatcherChange));
					}
					this.OnDispatcherChange(null);
				}
			}

			private void OnDispatcherChange(RedNodeListenData data)
			{
				if (this.Listener == null || this.Dispatcher == null)
				{
					return;
				}
				if (!this.mIsActive)
				{
					return;
				}
				this.Listener.OnOtherRedPointCount(this.Dispatcher.RedPointCount, this.Dispatcher.RedPointPriorityValue);
			}

			private bool mIsActive;

			public RedPointDataRecord Listener;

			public RedPointDataRecord Dispatcher;
		}
	}
}
