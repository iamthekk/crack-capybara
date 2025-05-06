using System;

namespace HotFix
{
	public abstract class BaseFlyNode
	{
		public abstract void Fly();

		public void Init()
		{
			this.OnInit();
		}

		public void DeInit()
		{
			this.OnDeInit();
			this.m_onFinished = null;
		}

		protected abstract void OnInit();

		protected abstract void OnDeInit();

		public Action<BaseFlyNode> m_onFinished;
	}
}
