using System;

namespace HotFix
{
	public class GameEventMiniGameData
	{
		public MiniGameType GameType { get; private set; }

		public object Reward { get; private set; }

		public int TableId { get; private set; }

		public MiniGameResult Result { get; private set; }

		public void SetData(MiniGameType type, object reward, int tableId, MiniGameResult result)
		{
			this.GameType = type;
			this.Reward = reward;
			this.TableId = tableId;
			this.Result = result;
		}
	}
}
