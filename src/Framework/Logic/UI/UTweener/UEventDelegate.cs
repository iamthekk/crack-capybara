using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[Serializable]
	public class UEventDelegate
	{
		public MonoBehaviour target
		{
			get
			{
				return this.mTarget;
			}
			set
			{
				this.mTarget = value;
				this.mCachedCallback = null;
				this.mRawDelegate = false;
				this.mCached = false;
				this.mMethod = null;
				this.mParameterInfos = null;
				this.mParameters = null;
			}
		}

		public string methodName
		{
			get
			{
				return this.mMethodName;
			}
			set
			{
				this.mMethodName = value;
				this.mCachedCallback = null;
				this.mRawDelegate = false;
				this.mCached = false;
				this.mMethod = null;
				this.mParameterInfos = null;
				this.mParameters = null;
			}
		}

		public UEventDelegate.Parameter[] parameters
		{
			get
			{
				if (!this.mCached)
				{
					this.Cache();
				}
				return this.mParameters;
			}
		}

		public bool isValid
		{
			get
			{
				if (!this.mCached)
				{
					this.Cache();
				}
				return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
			}
		}

		public bool isEnabled
		{
			get
			{
				if (!this.mCached)
				{
					this.Cache();
				}
				if (this.mRawDelegate && this.mCachedCallback != null)
				{
					return true;
				}
				if (this.mTarget == null)
				{
					return false;
				}
				MonoBehaviour monoBehaviour = this.mTarget;
				return monoBehaviour == null || monoBehaviour.enabled;
			}
		}

		public UEventDelegate()
		{
		}

		public UEventDelegate(UEventDelegate.Callback call)
		{
			this.Set(call);
		}

		public UEventDelegate(MonoBehaviour target, string methodName)
		{
			this.Set(target, methodName);
		}

		private static string GetMethodName(UEventDelegate.Callback callback)
		{
			return callback.Method.Name;
		}

		private static bool IsValid(UEventDelegate.Callback callback)
		{
			return callback != null && callback.Method != null;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return !this.isValid;
			}
			if (obj is UEventDelegate.Callback)
			{
				UEventDelegate.Callback callback = obj as UEventDelegate.Callback;
				if (callback.Equals(this.mCachedCallback))
				{
					return true;
				}
				MonoBehaviour monoBehaviour = callback.Target as MonoBehaviour;
				return this.mTarget == monoBehaviour && string.Equals(this.mMethodName, UEventDelegate.GetMethodName(callback));
			}
			else
			{
				if (obj is UEventDelegate)
				{
					UEventDelegate ueventDelegate = obj as UEventDelegate;
					return this.mTarget == ueventDelegate.mTarget && string.Equals(this.mMethodName, ueventDelegate.mMethodName);
				}
				return false;
			}
		}

		public override int GetHashCode()
		{
			return UEventDelegate.s_Hash;
		}

		private void Set(UEventDelegate.Callback call)
		{
			this.Clear();
			if (call != null && UEventDelegate.IsValid(call))
			{
				this.mTarget = call.Target as MonoBehaviour;
				if (this.mTarget == null)
				{
					this.mRawDelegate = true;
					this.mCachedCallback = call;
					this.mMethodName = null;
					return;
				}
				this.mMethodName = UEventDelegate.GetMethodName(call);
				this.mRawDelegate = false;
			}
		}

		public void Set(MonoBehaviour target, string methodName)
		{
			this.Clear();
			this.mTarget = target;
			this.mMethodName = methodName;
		}

		private void Cache()
		{
			this.mCached = true;
			if (this.mRawDelegate)
			{
				return;
			}
			if ((this.mCachedCallback == null || this.mCachedCallback.Target as MonoBehaviour != this.mTarget || UEventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
			{
				Type type = this.mTarget.GetType();
				this.mMethod = null;
				while (type != null)
				{
					try
					{
						this.mMethod = type.GetMethod(this.mMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (this.mMethod != null)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
					}
					type = type.BaseType;
				}
				if (this.mMethod == null)
				{
					string text = "Could not find method '";
					string text2 = this.mMethodName;
					string text3 = "' on ";
					Type type2 = this.mTarget.GetType();
					HLog.LogError(text + text2 + text3 + ((type2 != null) ? type2.ToString() : null), this.mTarget.ToString());
					return;
				}
				if (this.mMethod.ReturnType != typeof(void))
				{
					Type type3 = this.mTarget.GetType();
					HLog.LogError(((type3 != null) ? type3.ToString() : null) + "." + this.mMethodName + " must have a 'void' return type.", this.mTarget.ToString());
					return;
				}
				this.mParameterInfos = this.mMethod.GetParameters();
				if (this.mParameterInfos.Length == 0)
				{
					this.mCachedCallback = (UEventDelegate.Callback)Delegate.CreateDelegate(typeof(UEventDelegate.Callback), this.mTarget, this.mMethodName);
					this.mArgs = null;
					this.mParameters = null;
					return;
				}
				this.mCachedCallback = null;
				if (this.mParameters == null || this.mParameters.Length != this.mParameterInfos.Length)
				{
					this.mParameters = new UEventDelegate.Parameter[this.mParameterInfos.Length];
					int i = 0;
					int num = this.mParameters.Length;
					while (i < num)
					{
						this.mParameters[i] = new UEventDelegate.Parameter();
						i++;
					}
				}
				int j = 0;
				int num2 = this.mParameters.Length;
				while (j < num2)
				{
					this.mParameters[j].expectedType = this.mParameterInfos[j].ParameterType;
					j++;
				}
			}
		}

		public bool Execute()
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mCachedCallback != null)
			{
				this.mCachedCallback();
				return true;
			}
			if (this.mMethod != null)
			{
				if (this.mParameters == null || this.mParameters.Length == 0)
				{
					this.mMethod.Invoke(this.mTarget, null);
				}
				else
				{
					if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
					{
						this.mArgs = new object[this.mParameters.Length];
					}
					int i = 0;
					int num = this.mParameters.Length;
					while (i < num)
					{
						this.mArgs[i] = this.mParameters[i].value;
						i++;
					}
					try
					{
						this.mMethod.Invoke(this.mTarget, this.mArgs);
					}
					catch (ArgumentException ex)
					{
						HLog.LogException(ex);
					}
					int j = 0;
					int num2 = this.mArgs.Length;
					while (j < num2)
					{
						if (this.mParameterInfos[j].IsIn || this.mParameterInfos[j].IsOut)
						{
							this.mParameters[j].value = this.mArgs[j];
						}
						this.mArgs[j] = null;
						j++;
					}
				}
				return true;
			}
			return false;
		}

		public void Clear()
		{
			this.mTarget = null;
			this.mMethodName = null;
			this.mRawDelegate = false;
			this.mCachedCallback = null;
			this.mParameters = null;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mArgs = null;
		}

		public override string ToString()
		{
			if (this.mTarget != null)
			{
				string text = this.mTarget.GetType().ToString();
				int num = text.LastIndexOf('.');
				if (num > 0)
				{
					text = text.Substring(num + 1);
				}
				if (!string.IsNullOrEmpty(this.methodName))
				{
					return text + "/" + this.methodName;
				}
				return text + "/[delegate]";
			}
			else
			{
				if (!this.mRawDelegate)
				{
					return null;
				}
				return "[delegate]";
			}
		}

		public static void Execute(List<UEventDelegate> list)
		{
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null)
					{
						try
						{
							ueventDelegate.Execute();
						}
						catch (Exception ex)
						{
							if (ex.InnerException != null)
							{
								HLog.LogException(ex.InnerException);
							}
							else
							{
								HLog.LogException(ex);
							}
						}
						if (i >= list.Count)
						{
							break;
						}
						if (list[i] != ueventDelegate)
						{
							continue;
						}
						if (ueventDelegate.oneShot)
						{
							list.RemoveAt(i);
							continue;
						}
					}
				}
			}
		}

		public static bool IsValid(List<UEventDelegate> list)
		{
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null && ueventDelegate.isValid)
					{
						return true;
					}
					i++;
				}
			}
			return false;
		}

		public static UEventDelegate Set(List<UEventDelegate> list, UEventDelegate.Callback callback)
		{
			if (list != null)
			{
				UEventDelegate ueventDelegate = new UEventDelegate(callback);
				list.Clear();
				list.Add(ueventDelegate);
				return ueventDelegate;
			}
			return null;
		}

		public static void Set(List<UEventDelegate> list, UEventDelegate del)
		{
			if (list != null)
			{
				list.Clear();
				list.Add(del);
			}
		}

		public static UEventDelegate Add(List<UEventDelegate> list, UEventDelegate.Callback callback)
		{
			return UEventDelegate.Add(list, callback, false);
		}

		public static UEventDelegate Add(List<UEventDelegate> list, UEventDelegate.Callback callback, bool oneShot)
		{
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null && ueventDelegate.Equals(callback))
					{
						return ueventDelegate;
					}
					i++;
				}
				UEventDelegate ueventDelegate2 = new UEventDelegate(callback);
				ueventDelegate2.oneShot = oneShot;
				list.Add(ueventDelegate2);
				return ueventDelegate2;
			}
			return null;
		}

		public static void Add(List<UEventDelegate> list, UEventDelegate ev)
		{
			UEventDelegate.Add(list, ev, ev.oneShot);
		}

		public static void Add(List<UEventDelegate> list, UEventDelegate ev, bool oneShot)
		{
			if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
			{
				UEventDelegate.Add(list, ev.mCachedCallback, oneShot);
				return;
			}
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null && ueventDelegate.Equals(ev))
					{
						return;
					}
					i++;
				}
				UEventDelegate ueventDelegate2 = new UEventDelegate(ev.target, ev.methodName);
				ueventDelegate2.oneShot = oneShot;
				if (ev.mParameters != null && ev.mParameters.Length != 0)
				{
					ueventDelegate2.mParameters = new UEventDelegate.Parameter[ev.mParameters.Length];
					for (int j = 0; j < ev.mParameters.Length; j++)
					{
						ueventDelegate2.mParameters[j] = ev.mParameters[j];
					}
				}
				list.Add(ueventDelegate2);
			}
		}

		public static bool Remove(List<UEventDelegate> list, UEventDelegate.Callback callback)
		{
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null && ueventDelegate.Equals(callback))
					{
						list.RemoveAt(i);
						return true;
					}
					i++;
				}
			}
			return false;
		}

		public static bool Remove(List<UEventDelegate> list, UEventDelegate ev)
		{
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UEventDelegate ueventDelegate = list[i];
					if (ueventDelegate != null && ueventDelegate.Equals(ev))
					{
						list.RemoveAt(i);
						return true;
					}
					i++;
				}
			}
			return false;
		}

		[SerializeField]
		private MonoBehaviour mTarget;

		[SerializeField]
		private string mMethodName;

		[SerializeField]
		private UEventDelegate.Parameter[] mParameters;

		public bool oneShot;

		[NonSerialized]
		private UEventDelegate.Callback mCachedCallback;

		[NonSerialized]
		private bool mRawDelegate;

		[NonSerialized]
		private bool mCached;

		[NonSerialized]
		private MethodInfo mMethod;

		[NonSerialized]
		private ParameterInfo[] mParameterInfos;

		[NonSerialized]
		private object[] mArgs;

		private static int s_Hash = "UEventDelegate".GetHashCode();

		[Serializable]
		public class Parameter
		{
			public Parameter()
			{
			}

			public Parameter(Object obj, string field)
			{
				this.obj = obj;
				this.field = field;
			}

			public Parameter(object val)
			{
				this.mValue = val;
			}

			public object value
			{
				get
				{
					if (this.mValue != null)
					{
						return this.mValue;
					}
					if (!this.cached)
					{
						this.cached = true;
						this.fieldInfo = null;
						this.propInfo = null;
						if (this.obj != null && !string.IsNullOrEmpty(this.field))
						{
							Type type = this.obj.GetType();
							this.propInfo = type.GetProperty(this.field);
							if (this.propInfo == null)
							{
								this.fieldInfo = type.GetField(this.field);
							}
						}
					}
					if (this.propInfo != null)
					{
						return this.propInfo.GetValue(this.obj, null);
					}
					if (this.fieldInfo != null)
					{
						return this.fieldInfo.GetValue(this.obj);
					}
					if (this.obj != null)
					{
						return this.obj;
					}
					if (this.expectedType != null && this.expectedType.IsValueType)
					{
						return null;
					}
					return Convert.ChangeType(null, this.expectedType);
				}
				set
				{
					this.mValue = value;
				}
			}

			public Type type
			{
				get
				{
					if (this.mValue != null)
					{
						return this.mValue.GetType();
					}
					if (this.obj == null)
					{
						return typeof(void);
					}
					return this.obj.GetType();
				}
			}

			public Object obj;

			public string field;

			[NonSerialized]
			private object mValue;

			[NonSerialized]
			public Type expectedType = typeof(void);

			[NonSerialized]
			public bool cached;

			[NonSerialized]
			public PropertyInfo propInfo;

			[NonSerialized]
			public FieldInfo fieldInfo;
		}

		public delegate void Callback();
	}
}
