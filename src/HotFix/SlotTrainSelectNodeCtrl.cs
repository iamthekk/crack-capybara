using System;
using System.Collections.Generic;
using Framework.Logic;

namespace HotFix
{
	public class SlotTrainSelectNodeCtrl
	{
		public SlotTrainSelectNodeCtrl(List<UISlotTrainItem> list, Action<int> callback)
		{
			this.itemList = list;
			this.onSelectIndex = callback;
			SlotTrainNodeState slotTrainNodeState = new SlotTrainNodeState(0);
			SlotTrainNodeState slotTrainNodeState2 = new SlotTrainNodeState(1);
			SlotTrainNodeState slotTrainNodeState3 = new SlotTrainNodeState(2);
			this.stateMachine.RegisterState(slotTrainNodeState);
			this.stateMachine.RegisterState(slotTrainNodeState2);
			this.stateMachine.RegisterState(slotTrainNodeState3);
			slotTrainNodeState.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				if (this.timer >= 0.06f / this.SpeedRatio)
				{
					this.timer = 0f;
					if (this.stopWaitIndex >= 0 && this.index == this.stopWaitIndex && this.index == this.stopWaitIndex)
					{
						this.stateMachine.ActiveState(1);
					}
					this.GoToNextIndex();
				}
			};
			slotTrainNodeState2.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				float num = 0.06f / this.SpeedRatio;
				if (this.IsSpeedDecayStop)
				{
					num = 0.06f / (this.SpeedRatio * ((float)this.stopWaitIndexCounter / 36f));
					num = Utility.Math.Clamp(num, 0f, 0.33f);
				}
				if (this.timer >= num)
				{
					this.timer = 0f;
					this.GoToNextIndex();
					this.stopWaitIndexCounter--;
					if (this.stopWaitIndexCounter == 0)
					{
						int num2 = this.SelectCurrentIndex();
						Action<int> action = this.onSelectIndex;
						if (action != null)
						{
							action(num2);
						}
						this.stateMachine.ActiveState(2);
					}
				}
			};
			slotTrainNodeState3.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				if (this.selectCount < this.TargetLength && this.timer >= 0.4f)
				{
					this.timer = 0f;
					int num3 = this.SelectCurrentIndex();
					Action<int> action2 = this.onSelectIndex;
					if (action2 == null)
					{
						return;
					}
					action2(num3);
				}
			};
		}

		private void GoToNextIndex()
		{
			int itemIndex = SlotTrainViewModule.GetItemIndex(this.index - (this.TargetLength - 1), this.itemList.Count);
			this.itemList[itemIndex].SelectNodeExit(this.IsShowSlotEfx);
			this.index++;
			if (this.index >= this.itemList.Count)
			{
				this.index = 0;
			}
			this.itemList[this.index].SelectNodeEnter(this.IsShowSlotEfx);
		}

		public void StartAtIndex(int startIndex, int targetLength = 1)
		{
			this.timer = 0f;
			this.index = startIndex;
			this.stopWaitIndex = -1;
			this.TargetLength = targetLength;
			this.selectCount = 0;
			for (int i = 0; i < this.TargetLength; i++)
			{
				this.itemList[startIndex - i].SelectNodeEnter(this.IsShowSlotEfx);
			}
			this.stateMachine.ActiveState(0);
		}

		public void StopAtIndex(int stopIndex)
		{
			this.selectCount = 0;
			if (this.stateMachine.GetCurrentStateName() == 2)
			{
				this.stateMachine.ActiveState(0);
			}
			this.stopWaitIndex = SlotTrainViewModule.GetItemIndex(stopIndex - 36 - 1, this.itemList.Count);
			this.stopWaitIndexCounter = 36;
		}

		public void Remove()
		{
			this.itemList[this.index].SelectNodeExit(this.IsShowSlotEfx);
		}

		public int SelectCurrentIndex()
		{
			int itemIndex = SlotTrainViewModule.GetItemIndex(this.index - this.selectCount, this.itemList.Count);
			this.itemList[itemIndex].SelectNodeSelect(false);
			this.selectCount++;
			return itemIndex;
		}

		public void OnUpdate(float deltaTime)
		{
			this.stateMachine.OnUpdate(deltaTime);
		}

		private const int STATE_SPIN = 0;

		private const int STATE_WAIT_STOP = 1;

		private const int STATE_STOP = 2;

		private const int STOP_COUNT = 36;

		private const float INTERVAL = 0.06f;

		private int index;

		private int stopWaitIndex = -1;

		private int stopWaitIndexCounter;

		public bool IsSpeedDecayStop = true;

		public bool IsShowSlotEfx = true;

		public int TargetLength = 1;

		private int selectCount;

		public float SpeedRatio = 1f;

		private float timer;

		private Action<int> onSelectIndex;

		private List<UISlotTrainItem> itemList;

		private StateMachine stateMachine = new StateMachine();
	}
}
