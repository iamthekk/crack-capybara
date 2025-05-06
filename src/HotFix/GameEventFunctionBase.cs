using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNode.GameEvent;

namespace HotFix
{
	public abstract class GameEventFunctionBase
	{
		public bool IsDone { get; protected set; }

		protected GameEventFunctionBase(GameEventDataFunction data)
		{
			this.functionData = data;
		}

		public abstract GameEventFunctionBase.FunctionDoType GetDoType();

		public abstract int GetDoOrder();

		public abstract void Create();

		public abstract Task DoFunction();

		protected virtual void MarkDone()
		{
			this.IsDone = true;
		}

		public virtual List<NodeAttParam> GetShowAttributes()
		{
			return null;
		}

		public virtual List<NodeItemParam> GetShowItems()
		{
			return null;
		}

		public virtual List<NodeSkillParam> GetShowSkills()
		{
			return null;
		}

		public virtual List<string> GetShowInfos()
		{
			return null;
		}

		public virtual bool IsUndo()
		{
			return false;
		}

		public virtual string GetUndoTip()
		{
			return "";
		}

		public List<NodeAttParam> ToAttributeParamList(string param)
		{
			string[] array = param.Split('|', StringSplitOptions.None);
			List<NodeAttParam> list = new List<NodeAttParam>();
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',', StringSplitOptions.None);
				AttEnum attEnum;
				int num;
				if (array2.Length >= 2 && Enum.TryParse<AttEnum>(array2[0], out attEnum) && int.TryParse(array2[1], out num))
				{
					NodeAttParam nodeAttParam = new NodeAttParam(attEnum, (double)num, ChapterDropSource.Event, 1);
					list.Add(nodeAttParam);
				}
				else
				{
					HLog.LogError(string.Format("属性参数解析错误：eventId={0}, param={1}", this.functionData.poolData.tableId, param));
				}
			}
			return list;
		}

		public List<NodeItemParam> ToItemParamList(string param)
		{
			string[] array = param.Split('|', StringSplitOptions.None);
			List<NodeItemParam> list = new List<NodeItemParam>();
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',', StringSplitOptions.None);
				int num;
				int num2;
				int num3;
				if (array2.Length >= 3 && int.TryParse(array2[0], out num) && int.TryParse(array2[1], out num2) && int.TryParse(array2[2], out num3))
				{
					NodeItemParam nodeItemParam = new NodeItemParam((NodeItemType)num, num2, (long)num3, ChapterDropSource.Event, this.functionData.poolData.serverRate);
					list.Add(nodeItemParam);
				}
				else
				{
					HLog.LogError(string.Format("物品参数解析错误：eventId={0}, param={1}", this.functionData.poolData.tableId, param));
				}
			}
			return list;
		}

		protected GameEventDataFunction functionData;

		public enum FunctionDoType
		{
			Immediately,
			ChildTrigger
		}

		protected enum FunctionDoOrder
		{
			A = 1,
			B = 10,
			C = 100,
			D = 1000,
			E = 10000,
			F = 100000
		}
	}
}
