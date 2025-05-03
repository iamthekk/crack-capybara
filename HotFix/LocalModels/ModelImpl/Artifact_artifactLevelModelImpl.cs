using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Artifact_artifactLevelModelImpl : BaseLocalModelImpl<Artifact_artifactLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Artifact_artifactLevel();
		}

		protected override int GetBeanKey(Artifact_artifactLevel bean)
		{
			return bean.id;
		}
	}
}
