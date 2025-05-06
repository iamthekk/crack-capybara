using System;

namespace HotFix
{
	public class SlotTrainNodeState : StateMachine.State
	{
		public SlotTrainNodeState(int id)
			: base(id)
		{
		}

		public override void OnEnter()
		{
		}

		public override void OnUpdate(float deltaTime)
		{
			SlotTrainNodeState.OnStateUpdate onUpdateStateAction = this.OnUpdateStateAction;
			if (onUpdateStateAction == null)
			{
				return;
			}
			onUpdateStateAction(deltaTime);
		}

		public override void OnExit()
		{
		}

		public SlotTrainNodeState.OnStateUpdate OnUpdateStateAction;

		public delegate void OnStateUpdate(float deltaTime);
	}
}
