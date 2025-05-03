using System;
using System.Reflection;

namespace Framework.Logic.GameTestTools
{
	public class GameTestMethodVO
	{
		public int Order
		{
			get
			{
				if (this.TestAttribute == null)
				{
					return 0;
				}
				return this.TestAttribute.Order;
			}
		}

		public string Name
		{
			get
			{
				if (this.TestAttribute == null)
				{
					return "";
				}
				return this.TestAttribute.Name;
			}
		}

		public string Tips
		{
			get
			{
				if (this.TestAttribute == null)
				{
					return "";
				}
				return this.TestAttribute.Tips;
			}
		}

		public string TypeName
		{
			get
			{
				if (this.ClassType == null)
				{
					return "";
				}
				return this.ClassType.FullName;
			}
		}

		public string FuncName
		{
			get
			{
				if (this.MInfo == null)
				{
					return "";
				}
				return this.MInfo.Name;
			}
		}

		public GameTestMethodVO(Type type, MethodInfo mi, GameTestMethodAttribute testattribute)
		{
			this.ClassType = type;
			this.MInfo = mi;
			this.TestAttribute = testattribute;
			this.Info = new GameTestGUIInfo
			{
				TypeName = this.ClassType.FullName,
				FuncName = this.MInfo.Name,
				ShowName = this.Name,
				ShowTips = this.Tips,
				Head = testattribute.Head
			};
			if (string.IsNullOrEmpty(this.Info.Head))
			{
				this.Info.Head = "Others";
			}
		}

		public void DoTest(GameTestGUIInfo info)
		{
			if (this.MInfo == null)
			{
				return;
			}
			try
			{
				this.MInfo.Invoke(null, null);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public GameTestGUIInfo MakeInfo()
		{
			return this.Info;
		}

		public bool TryDoTest(GameTestGUIInfo info)
		{
			if (info == null)
			{
				return false;
			}
			if (info.TypeName != this.TypeName || info.FuncName != this.FuncName)
			{
				return false;
			}
			this.DoTest(info);
			return true;
		}

		public static int Sort(GameTestMethodVO x, GameTestMethodVO y)
		{
			return x.Order.CompareTo(y.Order);
		}

		private GameTestMethodAttribute TestAttribute;

		private Type ClassType;

		private MethodInfo MInfo;

		private GameTestGUIInfo Info;
	}
}
