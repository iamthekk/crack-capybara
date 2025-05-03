using System;
using LocalModels;

namespace Server
{
	public class BattleServer
	{
		public OutBattleData DoBattle(InBattleData inBattleData, LocalModelManager localModelManager)
		{
			BattleServerController battleServerController = new BattleServerController();
			battleServerController.SetInData(inBattleData);
			battleServerController.SetLocalModelManager(localModelManager);
			battleServerController.Init();
			try
			{
				battleServerController.OnBattleStart();
				for (int i = 0; i < inBattleData.m_durationRound; i++)
				{
					battleServerController.OnRoundBattle();
					battleServerController.DebugRoundReport();
					if (battleServerController.IsGameOver)
					{
						break;
					}
				}
			}
			catch (StackOverflowException ex)
			{
				HLog.LogException(ex);
			}
			catch (Exception ex2)
			{
				HLog.LogException(ex2);
				throw;
			}
			OutBattleData outData = battleServerController.OutData;
			battleServerController.DeInit();
			return outData;
		}
	}
}
