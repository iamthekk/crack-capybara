using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Artifact_advanceArtifactModelImpl : BaseLocalModelImpl<Artifact_advanceArtifact, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Artifact_advanceArtifact();
		}

		protected override int GetBeanKey(Artifact_advanceArtifact bean)
		{
			return bean.id;
		}
	}
}
