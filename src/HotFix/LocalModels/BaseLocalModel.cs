using System;

namespace LocalModels
{
	public abstract class BaseLocalModel
	{
		public byte[] Data
		{
			get
			{
				return this.m_assetBytes;
			}
		}

		public virtual void Initialise(string name, byte[] assetBytes)
		{
			this.m_assetBytes = assetBytes;
		}

		public virtual void DeInitialise()
		{
			this.m_assetBytes = null;
		}

		private byte[] m_assetBytes;
	}
}
