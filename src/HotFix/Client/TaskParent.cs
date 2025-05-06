using System;
using System.Collections.Generic;

namespace HotFix.Client
{
	public class TaskParent : BaseTask
	{
		public List<BaseTask> Children
		{
			get
			{
				return this.children;
			}
			private set
			{
				this.children = value;
			}
		}

		public int Count
		{
			get
			{
				if (this.children == null)
				{
					return 0;
				}
				return this.children.Count;
			}
		}

		public void AddChild(BaseTask child)
		{
			if (this.children == null)
			{
				this.children = new List<BaseTask>();
			}
			this.children.Add(child);
		}

		public void InsertChildToStart(BaseTask child)
		{
			if (this.children == null)
			{
				this.children = new List<BaseTask>();
			}
			this.children.Insert(0, child);
		}

		protected void InsertChildToIndex(BaseTask child, int index)
		{
			if (this.children == null)
			{
				this.children = new List<BaseTask>();
			}
			this.children.Insert(index, child);
		}

		public void AddTasks(List<BaseTask> tasks)
		{
			if (this.children == null)
			{
				this.children = new List<BaseTask>();
			}
			this.children.AddRange(tasks);
		}

		protected List<BaseTask> children;
	}
}
