using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	[Serializable]
	public class ActivitySlotTrainSelectNodeCtrl
	{
		public bool IsPlaying { get; private set; }

		public void Init(List<UIActivitySlotTrainItem> list, Action onEndFunction, Action<int> onSelectFunction = null, Action<int> onMoveFunction = null)
		{
			this.itemList = list;
			this.itemCount = this.itemList.Count;
			this.onEnd = onEndFunction;
			this.onSelectIndex = onSelectFunction;
			this.onMoveToIndex = onMoveFunction;
			SlotTrainNodeState slotTrainNodeState = new SlotTrainNodeState(0);
			SlotTrainNodeState slotTrainNodeState2 = new SlotTrainNodeState(1);
			SlotTrainNodeState slotTrainNodeState3 = new SlotTrainNodeState(2);
			this.stateMachine.RegisterState(slotTrainNodeState);
			this.stateMachine.RegisterState(slotTrainNodeState2);
			this.stateMachine.RegisterState(slotTrainNodeState3);
			slotTrainNodeState.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				if (this.timer >= this.interval_BaseMove / this.speedRatio)
				{
					this.timer = 0f;
					this.GoToNextIndex();
					if (!this.CanNextStep)
					{
						return;
					}
					if (this.moveCount >= this.itemCount * this.slowAfterRound && this.index == this.slowIndex)
					{
						this.stateMachine.ActiveState(1);
					}
				}
			};
			slotTrainNodeState2.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				float num = this.interval_BaseMove / this.speedRatio;
				if (this.needSlowStop)
				{
					num = Mathf.Lerp(this.interval_BaseMove, this.interval_Slow_Max, (float)this.slowedCount / (float)this.slowCount);
				}
				if (this.timer >= num)
				{
					this.timer = 0f;
					this.GoToNextIndex();
					this.slowedCount++;
					if (!this.CanNextStep)
					{
						return;
					}
					if (this.slowedCount >= this.slowCount)
					{
						this.selectedCount = 1;
						Action<int> action = this.onSelectIndex;
						if (action != null)
						{
							action(this.index);
						}
						this.stateMachine.ActiveState(2);
						this.CheckEnd();
					}
				}
			};
			slotTrainNodeState3.OnUpdateStateAction = delegate(float deltaTime)
			{
				this.timer += deltaTime;
				if (this.timer >= this.interval_Select && this.selectedCount < this.selectCount)
				{
					this.timer = 0f;
					this.GoToNextIndex();
					this.selectedCount++;
					Action<int> action2 = this.onSelectIndex;
					if (action2 != null)
					{
						action2(this.index);
					}
					this.CheckEnd();
				}
			};
		}

		private void GoToNextIndex()
		{
			this.moveCount++;
			this.itemList[this.index].SelectNodeExit(this.showSlotEfx);
			this.index = this.GetItemIndex(this.index + 1);
			this.itemList[this.index].SelectNodeEnter(this.showSlotEfx);
			Action<int> action = this.onMoveToIndex;
			if (action == null)
			{
				return;
			}
			action(this.index);
		}

		private void CheckEnd()
		{
			if (this.selectedCount >= this.selectCount)
			{
				this.IsPlaying = false;
				Action action = this.onEnd;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		public void StartAtIndex(int startIdx, int stopStartIdx, int selectLength = 1)
		{
			this.timer = 0f;
			this.index = startIdx;
			this.stopStartIndex = stopStartIdx;
			this.selectCount = selectLength;
			this.slowIndex = this.GetItemIndex(this.stopStartIndex - this.slowCount);
			this.moveCount = 0;
			this.slowedCount = 0;
			this.selectedCount = 0;
			this.itemList[this.index].SelectNodeEnter(this.showSlotEfx);
			this.stateMachine.ActiveState(0);
			this.IsPlaying = true;
		}

		public void Stop()
		{
			this.IsPlaying = false;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.IsPlaying)
			{
				return;
			}
			this.stateMachine.OnUpdate(deltaTime);
		}

		private int GetItemIndex(int idx)
		{
			if (this.itemCount <= 0)
			{
				return 0;
			}
			while (idx < 0)
			{
				idx += this.itemCount;
			}
			return idx % this.itemCount;
		}

		private const int STATE_SPIN = 0;

		private const int STATE_WAIT_STOP = 1;

		private const int STATE_STOP = 2;

		public float speedRatio = 1f;

		public float interval_BaseMove = 0.06f;

		public bool needSlowStop = true;

		public int slowAfterRound = 2;

		public int slowCount = 5;

		public float interval_Slow_Max = 0.33f;

		public int selectCount = 1;

		public float interval_Select = 0.4f;

		public bool showSlotEfx = true;

		private int itemCount;

		private int index;

		private int slowIndex;

		private int stopStartIndex;

		private int moveCount;

		private int slowedCount;

		private int selectedCount;

		private float timer;

		private Action onEnd;

		private Action<int> onSelectIndex;

		private Action<int> onMoveToIndex;

		public bool CanNextStep;

		private List<UIActivitySlotTrainItem> itemList;

		private StateMachine stateMachine = new StateMachine();
	}
}
