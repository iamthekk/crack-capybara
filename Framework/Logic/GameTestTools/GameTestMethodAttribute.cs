using System;

namespace Framework.Logic.GameTestTools
{
	public class GameTestMethodAttribute : Attribute
	{
		public GameTestMethodAttribute()
		{
			this.Order = 99999999;
		}

		public GameTestMethodAttribute(string head = "", string name = "", string tips = "", int order = 0)
		{
			this.Order = order;
			this.Name = name;
			this.Tips = tips;
			this.Head = head;
			if (this.Order == 0)
			{
				this.Order = name.GetHashCode();
			}
		}

		public string Name;

		public string Tips;

		public string Head;

		public int Order;
	}
}
