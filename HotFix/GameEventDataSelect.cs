using System;

namespace HotFix
{
	public class GameEventDataSelect : GameEventData
	{
		public string languageId { get; private set; }

		public string info { get; private set; }

		public GameEventButtonType buttonType { get; private set; }

		public GameEventButtonColorEnum buttonColorType { get; private set; }

		public int needId { get; private set; }

		public int buttonParam { get; private set; }

		public string tipLanguageId { get; private set; }

		public string infoTip { get; private set; }

		public GameEventDataSelect(GameEventPoolData poolData, string languageId, string info, GameEventButtonType buttonType, GameEventButtonColorEnum buttonColorType, int needId, int buttonParam, string tipLanguageId, string infoTip)
		{
			this.poolData = poolData;
			this.languageId = languageId;
			this.info = info;
			this.buttonType = buttonType;
			this.buttonColorType = buttonColorType;
			this.needId = needId;
			this.buttonParam = buttonParam;
			this.tipLanguageId = tipLanguageId;
			this.infoTip = infoTip;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Select;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Option;
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count > 0)
			{
				GameEventData gameEventData = this.children[0];
				while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
				{
					gameEventData = gameEventData.GetNext(0);
				}
				return gameEventData;
			}
			return null;
		}

		public bool IsBuyButton()
		{
			return this.buttonType == GameEventButtonType.Buy;
		}

		public bool IsBattleButton()
		{
			GameEventData next = this.GetNext(0);
			if (next != null)
			{
				GameEventNodeType nodeType = next.GetNodeType();
				if (nodeType == GameEventNodeType.Battle || nodeType == GameEventNodeType.NpcBattle || nodeType == GameEventNodeType.WaveBattle)
				{
					return true;
				}
			}
			return false;
		}

		public NodeAttParam GetAttParam()
		{
			if (this.IsBuyButton())
			{
				return new NodeAttParam(GameEventAttType.Food, (double)this.buttonParam, ChapterDropSource.Event, 1);
			}
			return null;
		}

		public override string GetInfo()
		{
			return "Select_" + this.info;
		}
	}
}
