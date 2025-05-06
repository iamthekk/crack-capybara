using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TalentLegacyStudyFinishAttrItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void OnShow()
		{
		}

		public void OnClose()
		{
		}

		public void SetData(MergeAttributeData attributeData)
		{
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(attributeData.Header);
			if (elementById != null)
			{
				string text = "";
				if (attributeData.Header.Contains("%"))
				{
					text = "%";
				}
				this.Text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
				this.Text_Value.text = HLog.StringBuilder("+", DxxTools.FormatNumber((long)attributeData.Value), text);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
		}

		public CustomText Text_Name;

		public CustomText Text_Value;
	}
}
