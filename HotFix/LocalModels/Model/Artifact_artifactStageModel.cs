using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Artifact_artifactStageModel : BaseLocalModel
	{
		public Artifact_artifactStage GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Artifact_artifactStage> GetAllElements()
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

		public static readonly string fileName = "Artifact_artifactStage";

		private Artifact_artifactStageModelImpl modelImpl = new Artifact_artifactStageModelImpl();
	}
}
