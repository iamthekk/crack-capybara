using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Artifact_artifactStageModelImpl : BaseLocalModelImpl<Artifact_artifactStage, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Artifact_artifactStage();
		}

		protected override int GetBeanKey(Artifact_artifactStage bean)
		{
			return bean.id;
		}
	}
}
