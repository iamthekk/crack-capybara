using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class MountBasicData
	{
		public int ID { get; private set; }

		public Mount_mountStage StageConfig { get; private set; }

		public GameMember_member MemberConfig { get; private set; }

		public MergeAttributeData StageAttributeData { get; private set; }

		public int Stage
		{
			get
			{
				return this.StageConfig.stage;
			}
		}

		public string MountName
		{
			get
			{
				if (this.MemberConfig != null)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID(this.MemberConfig.nameLanguageID);
				}
				return "";
			}
		}

		public int MemberId
		{
			get
			{
				if (this.MemberConfig != null)
				{
					return this.MemberConfig.id;
				}
				return 0;
			}
		}

		public MountBasicData(Mount_mountStage stageConfig)
		{
			this.StageConfig = stageConfig;
			this.ID = stageConfig.id;
			List<MergeAttributeData> mergeAttributeData = stageConfig.attribute.GetMergeAttributeData();
			if (mergeAttributeData.Count > 0)
			{
				this.StageAttributeData = mergeAttributeData[0];
			}
			this.MemberConfig = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(stageConfig.memberId);
		}
	}
}
