using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class IAPBattlePassBaseCell : CustomBehaviour
	{
		public virtual void SetData(IAPBattlePassData data, int index, Action onClickReward)
		{
		}

		public virtual void SetStatus(int curscore, bool freehaveget, bool payhaveget)
		{
		}

		public abstract void RefreshUI(SequencePool m_seqPool);

		public Action OnClickGotoBuy;

		public int Index;
	}
}
