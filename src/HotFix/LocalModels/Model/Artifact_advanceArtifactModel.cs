using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Artifact_advanceArtifactModel : BaseLocalModel
	{
		public Artifact_advanceArtifact GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Artifact_advanceArtifact> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "Artifact_advanceArtifact";

		private Artifact_advanceArtifactModelImpl modelImpl = new Artifact_advanceArtifactModelImpl();
	}
}
