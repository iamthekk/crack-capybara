using System;
using System.Collections.Generic;

namespace HotFix
{
	public abstract class GameEventData
	{
		public bool IsRoot { get; private set; }

		public abstract GameEventNodeType GetNodeType();

		public abstract GameEventNodeOptionType GetNodeOptionType();

		public abstract string GetInfo();

		public void AddChild(GameEventData data)
		{
			this.children.Add(data);
		}

		public List<GameEventData> GetChildren()
		{
			return this.children;
		}

		public void AddFunctionData(GameEventData data)
		{
			this.functionDatas.Add(data);
		}

		public List<GameEventData> GetFunctionDatas()
		{
			return this.functionDatas;
		}

		public abstract GameEventData GetNext(int index);

		public bool IsShowButton()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i].GetNodeType() == GameEventNodeType.Select)
				{
					return true;
				}
			}
			return false;
		}

		public GameEventDataSelect GetButton(int index)
		{
			if (index < this.children.Count && this.children[index].GetNodeType() == GameEventNodeType.Select)
			{
				return this.children[index] as GameEventDataSelect;
			}
			return null;
		}

		public List<GameEventDataSelect> GetButtons()
		{
			List<GameEventDataSelect> list = new List<GameEventDataSelect>();
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i].GetNodeType() == GameEventNodeType.Select)
				{
					list.Add(this.children[i] as GameEventDataSelect);
				}
			}
			return list;
		}

		public bool IsHaveChildType(GameEventNodeType type)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i].GetNodeType() == type)
				{
					return true;
				}
				if (this.children[i].IsHaveChildType(type))
				{
					return true;
				}
			}
			return false;
		}

		public void AddFunction(GameEventFunctionBase func)
		{
			this.myFunctions.Add(func);
		}

		public List<GameEventFunctionBase> GetMyFunctions()
		{
			return this.myFunctions;
		}

		public List<GameEventFunctionBase> GetUndoMyFunctions(GameEventFunctionBase.FunctionDoType doType)
		{
			List<GameEventFunctionBase> list = new List<GameEventFunctionBase>();
			for (int i = 0; i < this.myFunctions.Count; i++)
			{
				if (this.myFunctions[i].GetDoType() == doType && !this.myFunctions[i].IsDone)
				{
					list.Add(this.myFunctions[i]);
				}
			}
			if (list.Count > 0)
			{
				list.Sort(new Comparison<GameEventFunctionBase>(GameEventData.OrderSort));
			}
			return list;
		}

		public List<NodeAttParam> GetMyFunctionAttr()
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			if (this.myFunctions != null)
			{
				for (int i = 0; i < this.myFunctions.Count; i++)
				{
					List<NodeAttParam> showAttributes = this.myFunctions[i].GetShowAttributes();
					if (showAttributes != null && showAttributes.Count > 0)
					{
						list.AddRange(showAttributes);
					}
				}
			}
			return list;
		}

		public List<NodeItemParam> GetMyFunctionItems()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			if (this.myFunctions != null)
			{
				for (int i = 0; i < this.myFunctions.Count; i++)
				{
					List<NodeItemParam> showItems = this.myFunctions[i].GetShowItems();
					if (showItems != null && showItems.Count > 0)
					{
						list.AddRange(showItems);
					}
				}
			}
			return list;
		}

		public List<NodeSkillParam> GetMyFunctionSkills()
		{
			List<NodeSkillParam> list = new List<NodeSkillParam>();
			if (this.myFunctions != null)
			{
				for (int i = 0; i < this.myFunctions.Count; i++)
				{
					List<NodeSkillParam> showSkills = this.myFunctions[i].GetShowSkills();
					if (showSkills != null && showSkills.Count > 0)
					{
						list.AddRange(showSkills);
					}
				}
			}
			return list;
		}

		public List<string> GetMyFunctionInfos()
		{
			List<string> list = new List<string>();
			if (this.myFunctions != null)
			{
				for (int i = 0; i < this.myFunctions.Count; i++)
				{
					List<string> showInfos = this.myFunctions[i].GetShowInfos();
					if (showInfos != null && showInfos.Count > 0)
					{
						list.AddRange(showInfos);
					}
				}
			}
			return list;
		}

		public List<GameEventFunctionBase> GetMyChildTriggerFunctions()
		{
			List<GameEventFunctionBase> list = new List<GameEventFunctionBase>();
			for (int i = 0; i < this.myFunctions.Count; i++)
			{
				if (this.myFunctions[i].GetDoType() == GameEventFunctionBase.FunctionDoType.ChildTrigger)
				{
					list.Add(this.myFunctions[i]);
				}
			}
			return list;
		}

		public void SetFatherFunctions(List<GameEventFunctionBase> list)
		{
			this.fatherFunctions = new List<GameEventFunctionBase>();
			for (int i = 0; i < list.Count; i++)
			{
				this.fatherFunctions.Add(list[i]);
			}
		}

		public List<GameEventFunctionBase> GetFatherFunctions()
		{
			return this.fatherFunctions;
		}

		public List<GameEventFunctionBase> GetUndoFatherFunctions()
		{
			List<GameEventFunctionBase> list = new List<GameEventFunctionBase>();
			for (int i = 0; i < this.fatherFunctions.Count; i++)
			{
				if (!this.fatherFunctions[i].IsDone)
				{
					list.Add(this.fatherFunctions[i]);
				}
			}
			if (list.Count > 0)
			{
				list.Sort(new Comparison<GameEventFunctionBase>(GameEventData.OrderSort));
			}
			return list;
		}

		public List<NodeAttParam> GetFatherFunctionAttr()
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			if (this.fatherFunctions != null)
			{
				for (int i = 0; i < this.fatherFunctions.Count; i++)
				{
					List<NodeAttParam> showAttributes = this.fatherFunctions[i].GetShowAttributes();
					if (showAttributes != null && showAttributes.Count > 0)
					{
						list.AddRange(showAttributes);
					}
				}
			}
			return list;
		}

		public List<NodeItemParam> GetFatherFunctionItems()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			if (this.fatherFunctions != null)
			{
				for (int i = 0; i < this.fatherFunctions.Count; i++)
				{
					List<NodeItemParam> showItems = this.fatherFunctions[i].GetShowItems();
					if (showItems != null && showItems.Count > 0)
					{
						list.AddRange(showItems);
					}
				}
			}
			return list;
		}

		public List<NodeSkillParam> GetFatherFunctionSkills()
		{
			List<NodeSkillParam> list = new List<NodeSkillParam>();
			if (this.fatherFunctions != null)
			{
				for (int i = 0; i < this.fatherFunctions.Count; i++)
				{
					List<NodeSkillParam> showSkills = this.fatherFunctions[i].GetShowSkills();
					if (showSkills != null && showSkills.Count > 0)
					{
						list.AddRange(showSkills);
					}
				}
			}
			return list;
		}

		public List<string> GetFatherFunctionInfos()
		{
			List<string> list = new List<string>();
			if (this.fatherFunctions != null)
			{
				for (int i = 0; i < this.fatherFunctions.Count; i++)
				{
					List<string> showInfos = this.fatherFunctions[i].GetShowInfos();
					if (showInfos != null && showInfos.Count > 0)
					{
						list.AddRange(showInfos);
					}
				}
			}
			return list;
		}

		public bool IsUndoFunction()
		{
			for (int i = 0; i < this.myFunctions.Count; i++)
			{
				if (this.myFunctions[i].IsUndo())
				{
					return true;
				}
			}
			return false;
		}

		public string GetUndoTip()
		{
			for (int i = 0; i < this.myFunctions.Count; i++)
			{
				string undoTip = this.myFunctions[i].GetUndoTip();
				if (!string.IsNullOrEmpty(undoTip))
				{
					return undoTip;
				}
			}
			return "";
		}

		public static int OrderSort(GameEventFunctionBase x, GameEventFunctionBase y)
		{
			return x.GetDoOrder().CompareTo(y.GetDoOrder());
		}

		public void SetRoot(bool isRoot)
		{
			this.IsRoot = isRoot;
		}

		public List<NodeItemParam> GetServerDrop()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			for (int i = 0; i < this.poolData.serverDrops.Count; i++)
			{
				int id = this.poolData.serverDrops[i].id;
				int count = this.poolData.serverDrops[i].count;
				NodeItemParam nodeItemParam = new NodeItemParam(NodeItemType.Item, id, (long)count, ChapterDropSource.Event, this.poolData.serverRate);
				list.Add(nodeItemParam);
			}
			return list;
		}

		public List<NodeItemParam> GetMonsterDrop()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			for (int i = 0; i < this.poolData.monsterDrops.Count; i++)
			{
				int id = this.poolData.monsterDrops[i].id;
				int count = this.poolData.monsterDrops[i].count;
				NodeItemParam nodeItemParam = new NodeItemParam(NodeItemType.Item, id, (long)count, ChapterDropSource.Battle, 1);
				list.Add(nodeItemParam);
			}
			return list;
		}

		public List<NodeItemParam> GetBattleDrop()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			for (int i = 0; i < this.poolData.battleDrops.Count; i++)
			{
				int id = this.poolData.battleDrops[i].id;
				int count = this.poolData.battleDrops[i].count;
				NodeItemParam nodeItemParam = new NodeItemParam(NodeItemType.Item, id, (long)count, ChapterDropSource.Battle, 1);
				list.Add(nodeItemParam);
			}
			return list;
		}

		public GameEventPoolData poolData;

		protected List<GameEventData> children = new List<GameEventData>();

		protected List<GameEventData> functionDatas = new List<GameEventData>();

		private List<GameEventFunctionBase> myFunctions = new List<GameEventFunctionBase>();

		private List<GameEventFunctionBase> fatherFunctions = new List<GameEventFunctionBase>();
	}
}
