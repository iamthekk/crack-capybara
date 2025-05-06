using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class ArtifactBasicData
	{
		public int ID { get; private set; }

		public Artifact_artifactStage StageConfig { get; private set; }

		public Item_Item ItemConfig { get; private set; }

		public MergeAttributeData StageAttributeData { get; private set; }

		public int Stage
		{
			get
			{
				return this.StageConfig.stage;
			}
		}

		public string ArtifactName
		{
			get
			{
				if (this.ItemConfig != null)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID(this.ItemConfig.nameID);
				}
				return "";
			}
		}

		public ArtifactBasicData(Artifact_artifactStage stageConfig)
		{
			this.StageConfig = stageConfig;
			this.ID = stageConfig.id;
			List<MergeAttributeData> mergeAttributeData = stageConfig.attribute.GetMergeAttributeData();
			if (mergeAttributeData.Count > 0)
			{
				this.StageAttributeData = mergeAttributeData[0];
			}
			this.ItemConfig = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(stageConfig.itemId);
		}
	}
}
