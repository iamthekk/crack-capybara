using System;
using System.Text.RegularExpressions;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIEquipDetailsSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void SetData(int equipSkillID, int composeId, bool isLock, int totalCount, int index = 1)
		{
			this.m_index = index;
			if (totalCount < 5 && index == totalCount)
			{
				this.m_imageMask.SetActiveSafe(true);
			}
			else
			{
				this.m_imageMask.SetActiveSafe(false);
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(composeId);
			if (elementById == null)
			{
				return;
			}
			Quality_equipQuality elementById2 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById.qualityColor);
			if (elementById2 == null)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(elementById2.atlasId);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(atlasPath, elementById2.pointSpriteName);
			}
			if (this.m_lock != null)
			{
				this.m_lock.SetImage(atlasPath, elementById2.lockSpriteName);
			}
			if (this.m_imageBg != null)
			{
				string text;
				if (elementById.qualityColor == 1 || elementById.qualityColor == 3 || elementById.qualityColor == 5)
				{
					text = "img_lightcolour_di";
				}
				else
				{
					text = "img_drakcolour_di";
				}
				this.m_imageBg.SetImage("Assets/_Resources/AtlasRaft/UIMainEquip/UIMainEquip.spriteatlas", text);
			}
			this.m_icon.gameObject.SetActive(!isLock);
			this.m_lock.gameObject.SetActive(isLock);
			Equip_skill elementById3 = GameApp.Table.GetManager().GetEquip_skillModelInstance().GetElementById(equipSkillID);
			if (this.m_txt != null && elementById3 != null)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.descId);
				string text2 = "<color=#[0-9a-fA-F]{6}>|<color=[a-zA-Z]+>|</color>";
				string text3 = Regex.Replace(infoByID, text2, string.Empty);
				this.m_txt.text = text3;
				Color black = Color.black;
				string text4;
				if (isLock)
				{
					text4 = "#a49082";
				}
				else if (elementById.qualityColor == 1 || elementById.qualityColor == 3 || elementById.qualityColor == 5)
				{
					text4 = elementById2.colorNumLight;
				}
				else
				{
					text4 = elementById2.colorNumLight;
				}
				ColorUtility.TryParseHtmlString(text4, ref black);
				this.m_txt.color = black;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		public CustomImage m_icon;

		public CustomImage m_lock;

		public CustomText m_txt;

		public CustomImage m_imageBg;

		public GameObject m_imageMask;

		private int m_index = 1;
	}
}
