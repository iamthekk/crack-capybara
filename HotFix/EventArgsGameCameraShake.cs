using System;
using Framework;
using Framework.EventSystem;
using LocalModels.Bean;

namespace HotFix
{
	public class EventArgsGameCameraShake : BaseEventArgs
	{
		public int shakeType
		{
			get
			{
				return this._shakeType;
			}
		}

		public float delay
		{
			get
			{
				return this._delay;
			}
		}

		public float duration
		{
			get
			{
				return this._duration;
			}
		}

		public float power
		{
			get
			{
				return this._power;
			}
		}

		public int count
		{
			get
			{
				return this._count;
			}
		}

		public void SetData(int id)
		{
			GameCamera_Shake elementById = GameApp.Table.GetManager().GetGameCamera_ShakeModelInstance().GetElementById(id);
			if (elementById != null)
			{
				this._shakeType = elementById.shakeType;
				this._delay = elementById.delay;
				this._duration = elementById.duration;
				this._power = elementById.power;
				this._count = elementById.count;
				return;
			}
			HLog.LogError(HLog.ToColor(string.Format("Table[GameCamera_ShakeModel] is Erorr. not found id = {0}", id), 3));
		}

		public override void Clear()
		{
		}

		private int _shakeType;

		private float _delay;

		private float _duration;

		private float _power;

		private int _count;
	}
}
