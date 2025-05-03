using System;

namespace HotFix
{
	public class MiningTreasureGridData
	{
		public TreasurePos GetTreasurePos()
		{
			if (this.treasurePos == 1)
			{
				return TreasurePos.Left_TOP_CONNER;
			}
			if (this.treasurePos == 3)
			{
				return TreasurePos.RIGHT_TOP_CONNER;
			}
			if (this.treasurePos == 7)
			{
				return TreasurePos.LEFT_BOTTOM_CONNER;
			}
			if (this.treasurePos == 9)
			{
				return TreasurePos.RIGHT_BOTTOM_CONNER;
			}
			if (this.treasurePos > 1 && this.treasurePos < 3)
			{
				return TreasurePos.TOP_EDGE;
			}
			if (this.treasurePos > 7 && this.treasurePos < 9)
			{
				return TreasurePos.BOTTOM_EDGE;
			}
			if (this.treasurePos % 3 == 1 && this.treasurePos != 1 && this.treasurePos != 7)
			{
				return TreasurePos.LEFT_EDGE;
			}
			if (this.treasurePos % 3 == 0 && this.treasurePos != 3 && this.treasurePos != 9)
			{
				return TreasurePos.RIGHT_EDGE;
			}
			return TreasurePos.MIDDLE;
		}

		public bool IsTop()
		{
			TreasurePos treasurePos = this.GetTreasurePos();
			return treasurePos == TreasurePos.TOP_EDGE || treasurePos == TreasurePos.Left_TOP_CONNER || treasurePos == TreasurePos.RIGHT_TOP_CONNER;
		}

		public bool IsBottom()
		{
			TreasurePos treasurePos = this.GetTreasurePos();
			return treasurePos == TreasurePos.BOTTOM_EDGE || treasurePos == TreasurePos.LEFT_BOTTOM_CONNER || treasurePos == TreasurePos.RIGHT_BOTTOM_CONNER;
		}

		public bool IsLeft()
		{
			TreasurePos treasurePos = this.GetTreasurePos();
			return treasurePos == TreasurePos.LEFT_EDGE || treasurePos == TreasurePos.LEFT_BOTTOM_CONNER || treasurePos == TreasurePos.Left_TOP_CONNER;
		}

		public bool IsRight()
		{
			TreasurePos treasurePos = this.GetTreasurePos();
			return treasurePos == TreasurePos.RIGHT_EDGE || treasurePos == TreasurePos.RIGHT_BOTTOM_CONNER || treasurePos == TreasurePos.RIGHT_TOP_CONNER;
		}

		public int Up
		{
			get
			{
				int num = this.treasurePos - 3;
				if (num > 0)
				{
					return num;
				}
				return 0;
			}
		}

		public int Down
		{
			get
			{
				int num = this.treasurePos + 3;
				if (num <= 9)
				{
					return num;
				}
				return 0;
			}
		}

		public int Left
		{
			get
			{
				if ((this.treasurePos - 1) % 3 != 0)
				{
					return this.treasurePos - 1;
				}
				return 0;
			}
		}

		public int Right
		{
			get
			{
				if (this.treasurePos % 3 != 0)
				{
					return this.treasurePos + 1;
				}
				return 0;
			}
		}

		public const int TREASURE_MAX_GRID = 3;

		public int serverPos;

		public int treasurePos;
	}
}
