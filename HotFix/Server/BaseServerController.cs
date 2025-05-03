using System;
using LocalModels;

namespace Server
{
	public abstract class BaseServerController<In, Out>
	{
		public In InData
		{
			get
			{
				return this.m_inData;
			}
		}

		public Out OutData
		{
			get
			{
				return this.m_outData;
			}
		}

		public int CurFrame
		{
			get
			{
				return this.m_curFrame;
			}
		}

		public LocalModelManager Table
		{
			get
			{
				return this.m_localModelManager;
			}
		}

		public virtual void Init()
		{
			this.m_curFrame = 0;
			this.m_onUpdate = null;
			this.OnInit();
		}

		protected abstract void OnInit();

		public virtual void Update(float deltaTime)
		{
			this.m_curFrame++;
			Action<float> onUpdate = this.m_onUpdate;
			if (onUpdate != null)
			{
				onUpdate(deltaTime);
			}
			this.OnUpdate(deltaTime);
		}

		protected abstract void OnUpdate(float deltaTime);

		public virtual void DeInit()
		{
			this.m_curFrame = 0;
			this.RemoveAllUpdate();
			this.OnDeInit();
		}

		protected abstract void OnDeInit();

		public void SetInData(In inData)
		{
			this.m_inData = inData;
		}

		public void AddUpdate(Action<float> onUpdate)
		{
			if (onUpdate == null)
			{
				return;
			}
			this.m_onUpdate = (Action<float>)Delegate.Combine(this.m_onUpdate, onUpdate);
		}

		public void RemoveUpdate(Action<float> onUpdate)
		{
			if (onUpdate == null)
			{
				return;
			}
			this.m_onUpdate = (Action<float>)Delegate.Remove(this.m_onUpdate, onUpdate);
		}

		public void RemoveAllUpdate()
		{
			this.m_onUpdate = null;
		}

		public void SetLocalModelManager(LocalModelManager localModelManager)
		{
			this.m_localModelManager = localModelManager;
		}

		protected In m_inData;

		protected Out m_outData;

		protected int m_curFrame;

		protected LocalModelManager m_localModelManager;

		private Action<float> m_onUpdate;
	}
}
