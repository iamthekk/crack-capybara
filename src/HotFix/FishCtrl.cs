using System;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class FishCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this._startPos = base.transform.localPosition;
			this._startPosY = this._startPos.y;
		}

		protected override void OnDeInit()
		{
		}

		public void StartBite(FishData data)
		{
			if (data != null)
			{
				int num = Random.Range(0, data.Config.actionList.Length);
				this._moveRow = GameApp.Table.GetManager().GetFishing_fishMoveModelInstance().GetElementById(data.Config.actionList[num]);
				this._isBite = true;
				this._curStateIndex = 0;
			}
			this.UpdateState();
		}

		public void EndBite()
		{
			this._isBite = false;
			this._timer = 0f;
		}

		public void RodStateLeadToUpdate(float deltaTime)
		{
			if (this._isBite)
			{
				this._timer -= deltaTime;
				if (this._timer <= 0f)
				{
					this.UpdateState();
				}
			}
		}

		public void FixedUpdate()
		{
			if (this._isBite && this._fishState == FishCtrl.FishState.Tired)
			{
				this._timer -= Time.fixedDeltaTime;
				if (this._timer <= 0f)
				{
					this.UpdateState();
				}
			}
		}

		private void UpdateState()
		{
			int curState = this.GetCurState();
			if (curState == 1)
			{
				this._fishState = FishCtrl.FishState.StruggleViolently;
			}
			else if (curState == 2)
			{
				this._fishState = FishCtrl.FishState.Struggle;
			}
			else
			{
				this._fishState = FishCtrl.FishState.Tired;
			}
			this.violent.gameObject.SetActive(this._fishState == FishCtrl.FishState.StruggleViolently);
			this.struggle.gameObject.SetActive(this._fishState == FishCtrl.FishState.Struggle);
			this.tired.gameObject.SetActive(this._fishState == FishCtrl.FishState.Tired);
		}

		private int GetCurState()
		{
			this._curStateIndex %= this._moveRow.action.Length;
			int num = this._moveRow.action[this._curStateIndex];
			this.GetTime(num, this._curStateIndex);
			this._curStateIndex++;
			return num;
		}

		private void GetTime(int state, int index)
		{
			if (state == 1)
			{
				this._timer = (float)Utility.Math.Random(this._moveRow.strongTime[0], this._moveRow.strongTime[1]) * this.rate * (float)this._moveRow.actionTime[index] * this.rate;
				return;
			}
			if (state == 2)
			{
				this._timer = (float)Utility.Math.Random(this._moveRow.struggleTime[0], this._moveRow.struggleTime[1]) * this.rate * (float)this._moveRow.actionTime[index] * this.rate;
				return;
			}
			this._timer = (float)Utility.Math.Random(this._moveRow.tireTime[0], this._moveRow.tireTime[1]) * this.rate * (float)this._moveRow.actionTime[index] * this.rate;
		}

		public FishCtrl.FishState GetFishState()
		{
			return this._fishState;
		}

		public void SetPos(float distance)
		{
			float num = this.moveCurve.Evaluate(distance);
			this._startPos.y = this._startPosY + num * this.moveRate;
			base.transform.localPosition = this._startPos;
			this._scale.x = this.baseScale - num * this.scaleRate;
			this._scale.y = this.baseScale - num * this.scaleRate;
			this.zoomGo.localScale = this._scale;
		}

		public void SetDistance(double distance)
		{
			this.textDistance.text = string.Format("{0}M", distance);
		}

		public CustomText textDistance;

		public float rate = 0.01f;

		public float strongSpeedRate = 1.2f;

		public float strongStrengthRate = 1.2f;

		public float tiredSpeedRate = 0.2f;

		public float tiredStrengthRate = 0.2f;

		public GameObject violent;

		public GameObject struggle;

		public GameObject tired;

		public Transform zoomGo;

		public float moveRate = 2f;

		public float baseScale = 2f;

		public float scaleRate = 1f;

		public AnimationCurve moveCurve;

		private Vector3 _startPos;

		private float _startPosY;

		private Fishing_fishMove _moveRow;

		private float _timer;

		private bool _isBite;

		private int _curStateIndex;

		private FishCtrl.FishState _fishState;

		private Vector3 _scale = Vector3.one;

		public enum FishState
		{
			StruggleViolently,
			Struggle,
			Tired
		}
	}
}
