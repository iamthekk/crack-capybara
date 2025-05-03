using System;
using System.Runtime.CompilerServices;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventItemData
	{
		public int id { get; private set; }

		public int startStage { get; private set; }

		public int lifetime { get; private set; }

		public EventItemType itemType { get; private set; }

		public string param { get; private set; }

		public string languageId { get; private set; }

		public int atlas { get; private set; }

		public string icon { get; private set; }

		public bool isShowUI { get; private set; }

		public int itemNum { get; private set; }

		public int costFood { get; private set; }

		public bool isOverlay { get; private set; }

		public int endStage
		{
			get
			{
				return this.startStage + this.lifetime;
			}
		}

		public GameEventItemData(int id, int startStage, int num = 1)
		{
			this.id = id;
			this.startStage = startStage;
			Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(id);
			this.lifetime = elementById.stage;
			this.itemType = (EventItemType)elementById.function;
			this.param = elementById.param;
			this.languageId = elementById.languageId;
			this.atlas = elementById.atlas;
			this.icon = elementById.icon;
			this.isShowUI = elementById.showUI > 0;
			this.itemNum = num;
			this.costFood = elementById.costFood;
			this.isOverlay = elementById.isOverlay > 0;
		}

		public void ResetStage(int stage)
		{
			this.startStage = stage;
		}

		[return: TupleElementNames(new string[] { "weatherType", "showDark", "changeWaves" })]
		public static ValueTuple<int, bool, bool>? ToWeatherData(string param)
		{
			string[] array = param.Split('|', StringSplitOptions.None);
			int num;
			int num2;
			int num3;
			if (array.Length >= 3 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2) && int.TryParse(array[2], out num3))
			{
				return new ValueTuple<int, bool, bool>?(new ValueTuple<int, bool, bool>(num, num2 > 0, num3 > 0));
			}
			return null;
		}

		public void AddNum(int num)
		{
			this.itemNum += num;
		}
	}
}
