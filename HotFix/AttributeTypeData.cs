using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class AttributeTypeData : AttributeTypeDataBase
	{
		public bool IsImage(out string imageName)
		{
			return AttributeTypeData.imageNameDic.TryGetValue(this.m_attrType, out imageName);
		}

		public override void SetImage(CustomImage image)
		{
			string text;
			if (!this.IsImage(out text))
			{
				HLog.LogError(string.Format("AttributeTypeData SetImage error type = {0}", this.m_attrType));
				return;
			}
			Atlas_atlas elementById = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(101);
			image.SetImage(elementById.path, text);
		}

		private static Dictionary<GameEventAttType, string> imageNameDic = new Dictionary<GameEventAttType, string>
		{
			{
				GameEventAttType.AttackPercent,
				"icon_attack"
			},
			{
				GameEventAttType.Exp,
				"icon_exp"
			},
			{
				GameEventAttType.Food,
				"icon_pearl"
			},
			{
				GameEventAttType.RecoverHpRate,
				"icon_currenthp"
			},
			{
				GameEventAttType.CampHpRate,
				"icon_currenthp"
			},
			{
				GameEventAttType.HPMaxPercent,
				"icon_hp"
			},
			{
				GameEventAttType.DefencePercent,
				"icon_defense"
			},
			{
				GameEventAttType.Chips,
				"icon_chips"
			}
		};

		public GameEventAttType m_attrType;

		public float num;
	}
}
