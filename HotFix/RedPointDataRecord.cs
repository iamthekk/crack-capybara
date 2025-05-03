using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class RedPointDataRecord
	{
		public bool UseClickToHideRedPoint
		{
			get
			{
				return this.mUseClickToHideRedPoint;
			}
			set
			{
				this.mUseClickToHideRedPoint = value;
			}
		}

		public bool IsInClickHideTime
		{
			get
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (!this.mHasReadLocalTime && this.mSaveHideTimeToLocal)
				{
					string userString = Utility.PlayerPrefs.GetUserString(this.LocalSaveKey, this.mHideRedPointTime.ToString());
					if (!string.IsNullOrEmpty(userString) && !long.TryParse(userString, out this.mHideRedPointTime))
					{
						this.mHideRedPointTime = 0L;
					}
				}
				this.mHasReadLocalTime = true;
				return serverTimestamp < this.mHideRedPointTime;
			}
		}

		public int RedPointCount
		{
			get
			{
				if (this.IsInClickHideTime)
				{
					return 0;
				}
				return this.mSelfRedPointCount + this.mSubRedPointCount + this.mOtherRedPointCount;
			}
		}

		public int RedPointPriorityValue
		{
			get
			{
				return this.mSelfRedPriorityValue | this.mSubRedPriorityValue | this.mOtherRedPriorityValue;
			}
		}

		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		public bool IsFirstClick { get; private set; } = true;

		public string LocalSaveKey
		{
			get
			{
				return "RedPoint_LocalHideTime:" + this.Path;
			}
		}

		public event Action<RedNodeListenData> OnStateChange;

		public IReadOnlyList<RedPointDataRecord> Childrens
		{
			get
			{
				return this.mChildrens;
			}
		}

		public RedPointDataRecord Parent
		{
			get
			{
				return this.mParent;
			}
		}

		public string Path
		{
			get
			{
				string text = ((this.mParent == null) ? "" : this.mParent.Path);
				if (!string.IsNullOrEmpty(text))
				{
					text = text + "." + this.Name;
				}
				else
				{
					text = this.Name;
				}
				return text;
			}
		}

		private void NotifyCountChange(bool notifyparent)
		{
			try
			{
				if (notifyparent && this.mParent != null)
				{
					this.mParent.OnSubChange(this);
				}
				Action<RedNodeListenData> onStateChange = this.OnStateChange;
				if (onStateChange != null)
				{
					onStateChange(this.MakeRedNodeListenData());
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public RedNodeListenData MakeRedNodeListenData()
		{
			return new RedNodeListenData
			{
				m_count = this.RedPointCount,
				m_priority = this.RedPointPriorityValue
			};
		}

		public virtual void OnSubChange(RedPointDataRecord rpd)
		{
			this.mSubRedPointCount = 0;
			this.mSubRedPriorityValue = 0;
			for (int i = 0; i < this.mChildrens.Count; i++)
			{
				this.mSubRedPriorityValue |= this.mChildrens[i].RedPointPriorityValue;
				if (this.mChildrens[i].RedPointCount > 0)
				{
					this.mSubRedPointCount++;
				}
			}
			this.NotifyCountChange(true);
		}

		public void OnOtherRedPointCount(int count, int priorityValue)
		{
			this.mOtherRedPointCount = count;
			this.mOtherRedPriorityValue = priorityValue;
			this.NotifyCountChange(true);
		}

		public RedPointDataRecord GetRedPointRecordByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				string[] array = name.Split('.', StringSplitOptions.None);
				return this.GetRedPointRecord(array, 0);
			}
			return null;
		}

		private RedPointDataRecord GetRedPointRecord(string[] names, int index = 0)
		{
			if (names == null || index < 0 || index > names.Length)
			{
				return null;
			}
			if (index == names.Length)
			{
				return this;
			}
			for (int i = 0; i < this.mChildrens.Count; i++)
			{
				if (this.mChildrens[i].Name == names[index])
				{
					return this.mChildrens[i].GetRedPointRecord(names, index + 1);
				}
			}
			return null;
		}

		public void RemoveChild(string name)
		{
			for (int i = 0; i < this.mChildrens.Count; i++)
			{
				if (this.mChildrens[i].Name == name)
				{
					this.mChildrens.RemoveAt(i);
					return;
				}
			}
		}

		public void RemoveAllChild()
		{
			this.mChildrens.Clear();
		}

		public RedPointDataRecord AddRecordPath(string path, IRedPointRecordCalculator calculator)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			string[] array = path.Split('.', StringSplitOptions.None);
			return this.CreateRecordNew(array, calculator);
		}

		[Obsolete("可能会导致比较深层次的递归调用，不再使用")]
		private RedPointDataRecord CreateRecord(string[] names, int index, IRedPointRecordCalculator calculator)
		{
			if (names == null || names.Length == 0 || index < 0 || index >= names.Length)
			{
				HLog.LogError("[redpoint]红点记录 CreateRecord，节点错误" + this.Name);
				return null;
			}
			string text = names[index];
			if (string.IsNullOrEmpty(text))
			{
				HLog.LogError("[redpoint]红点记录 CreateRecord，节点名称错误：" + text);
				return null;
			}
			if (text.Contains('.'))
			{
				HLog.LogError("[redpoint]红点记录 CreateRecord，节点名称错误：" + text);
				return null;
			}
			RedPointDataRecord redPointDataRecord;
			if (index + 1 == names.Length)
			{
				for (int i = 0; i < this.mChildrens.Count; i++)
				{
					if (this.mChildrens[i].Name == text)
					{
						if (this.mChildrens[i].Calculator == null)
						{
							this.mChildrens[i].Calculator = calculator;
						}
						return this.mChildrens[i];
					}
				}
				redPointDataRecord = new RedPointDataRecord
				{
					Name = text,
					mParent = this
				};
				redPointDataRecord.Calculator = calculator;
				this.mChildrens.Add(redPointDataRecord);
				return redPointDataRecord;
			}
			for (int i = 0; i < this.mChildrens.Count; i++)
			{
				if (this.mChildrens[i].Name == text)
				{
					return this.mChildrens[i].CreateRecord(names, index + 1, calculator);
				}
			}
			redPointDataRecord = new RedPointDataRecord
			{
				Name = text,
				mParent = this
			};
			this.mChildrens.Add(redPointDataRecord);
			return redPointDataRecord.CreateRecord(names, index + 1, calculator);
		}

		private RedPointDataRecord CreateRecordNew(string[] names, IRedPointRecordCalculator calculator)
		{
			if (names == null || names.Length == 0)
			{
				HLog.LogError("[redpoint]红点记录 CreateRecord，节点错误" + this.Name);
				return null;
			}
			RedPointDataRecord redPointDataRecord = this;
			RedPointDataRecord redPointDataRecord2 = null;
			for (int i = 0; i < names.Length; i++)
			{
				string text = names[i];
				if (string.IsNullOrEmpty(text))
				{
					HLog.LogError(string.Format("[redpoint]红点记录 CreateRecord，节点错误 index={0} name is empty!", i));
					return null;
				}
				if (redPointDataRecord == null)
				{
					HLog.LogError(string.Format("[redpoint]红点记录 CreateRecord，节点错误 parentrecord == null at {0} of {1}", i, text));
					return null;
				}
				RedPointDataRecord redPointDataRecord3 = null;
				for (int j = 0; j < redPointDataRecord.mChildrens.Count; j++)
				{
					if (redPointDataRecord.mChildrens[j].Name == text)
					{
						redPointDataRecord3 = redPointDataRecord.mChildrens[j];
						break;
					}
				}
				if (redPointDataRecord3 == null)
				{
					redPointDataRecord3 = new RedPointDataRecord
					{
						Name = text,
						mParent = redPointDataRecord
					};
					redPointDataRecord.mChildrens.Add(redPointDataRecord3);
				}
				if (i + 1 == names.Length)
				{
					if (redPointDataRecord3.Calculator == null)
					{
						redPointDataRecord3.Calculator = calculator;
					}
					redPointDataRecord2 = redPointDataRecord3;
				}
				redPointDataRecord = redPointDataRecord3;
			}
			return redPointDataRecord2;
		}

		public void ReCalc(bool calcsub, bool notifyparent)
		{
			this.mSelfRedPointCount = 0;
			this.mSelfRedPriorityValue = 0;
			DateTime now = DateTime.Now;
			if (this.Calculator != null)
			{
				try
				{
					this.mSelfRedPointCount = this.Calculator.CalcRedPoint(this);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}
			double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
			if (calcsub)
			{
				for (int i = 0; i < this.mChildrens.Count; i++)
				{
					this.mChildrens[i].ReCalc(calcsub, false);
				}
			}
			this.mSubRedPointCount = 0;
			this.mSubRedPriorityValue = 0;
			for (int j = 0; j < this.mChildrens.Count; j++)
			{
				this.mSubRedPointCount += this.mChildrens[j].RedPointCount;
				this.mSubRedPriorityValue |= this.mChildrens[j].RedPointPriorityValue;
			}
			this.NotifyCountChange(notifyparent);
		}

		public void ClickRecord()
		{
			this.IsFirstClick = false;
			if (this.UseClickToHideRedPoint)
			{
				long serverUTC = GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC;
				if (this.OnceOneDay)
				{
					DateTime date = new DateTime(1970, 1, 1).AddSeconds((double)serverUTC).AddDays(1.0).Date;
					this.mHideRedPointTime = Convert.ToInt64((date - new DateTime(1970, 1, 1)).TotalSeconds);
				}
				else if (this.TimeForClickHide != 0L)
				{
					this.mHideRedPointTime = serverUTC + this.TimeForClickHide;
				}
				else
				{
					this.mHideRedPointTime = serverUTC + (long)Singleton<GameConfig>.Instance.RedPoint_NoTipsTimeSec;
				}
				this.SaveCurrentHideTime();
			}
			this.ReCalc(false, true);
		}

		public void RemoveHideTime()
		{
			this.mHideRedPointTime = 0L;
			this.SaveCurrentHideTime();
		}

		private void SaveCurrentHideTime()
		{
			if (!this.mSaveHideTimeToLocal)
			{
				return;
			}
			Utility.PlayerPrefs.SetUserString(this.LocalSaveKey, this.mHideRedPointTime.ToString());
		}

		public RedPointDataRecord SetUseLocalHideTime(bool savetolocal)
		{
			this.mSaveHideTimeToLocal = savetolocal;
			return this;
		}

		public void RegRecordChange(Action<RedNodeListenData> action)
		{
			if (action == null)
			{
				return;
			}
			if (action != null)
			{
				action(this.MakeRedNodeListenData());
			}
			this.OnStateChange -= action;
			this.OnStateChange += action;
		}

		public void UnRegRecordChange(Action<RedNodeListenData> action)
		{
			if (action == null)
			{
				return;
			}
			this.OnStateChange -= action;
		}

		public void UpdateSelfRedPriorityValue(int priorityValue)
		{
			this.mSelfRedPriorityValue = priorityValue;
		}

		[SerializeField]
		private int mSelfRedPointCount;

		[SerializeField]
		private int mSelfRedPriorityValue;

		[SerializeField]
		private int mSubRedPointCount;

		[SerializeField]
		private int mSubRedPriorityValue;

		[SerializeField]
		private int mOtherRedPointCount;

		[SerializeField]
		private int mOtherRedPriorityValue;

		[SerializeField]
		private bool mUseClickToHideRedPoint = true;

		[SerializeField]
		private long mHideRedPointTime;

		public long TimeForClickHide;

		public bool OnceOneDay;

		private bool mSaveHideTimeToLocal;

		private bool mHasReadLocalTime;

		[SerializeField]
		private string m_name;

		public IRedPointRecordCalculator Calculator;

		[SerializeField]
		protected List<RedPointDataRecord> mChildrens = new List<RedPointDataRecord>();

		protected RedPointDataRecord mParent;
	}
}
