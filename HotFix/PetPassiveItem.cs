using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetPassiveItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnBtnItemClicked);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(int passiveId, int passiveValue)
		{
			for (int i = 0; i < this.imgBgList.Count; i++)
			{
				this.imgBgList[i].gameObject.SetActive(false);
			}
			bool flag = passiveId > 0;
			this.goLock.SetActive(!flag);
			this.goAttrNode.SetActive(flag);
			if (flag)
			{
				int num = 1;
				this.petEntryId = passiveId;
				this.petEntryValue = passiveValue;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (this.petEntryId > 0)
				{
					try
					{
						Pet_PetEntry elementById = GameApp.Table.GetManager().GetPet_PetEntryModelInstance().GetElementById(this.petEntryId);
						if (elementById != null)
						{
							num = elementById.quality;
							if (elementById.entryType == 1)
							{
								string[] passiveTextColor = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(num)
									.passiveTextColor2;
								MergeAttributeData mergeAttribute = elementById.action.GetMergeAttribute();
								string languageID = elementById.languageID;
								string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(languageID);
								string text4 = ((double)((elementById.entryType == 1) ? (mergeAttribute.Value * this.petEntryValue * 0.01f / 10000f) : mergeAttribute.Value) * 100.0).ToString("F2");
								text4 += (mergeAttribute.Header.Contains("%") ? "%" : "");
								text = infoByID ?? "";
								text2 = "+" + text4;
								if (passiveTextColor != null && passiveTextColor.Length != 0)
								{
									text3 = string.Concat(new string[]
									{
										" <color=",
										passiveTextColor[0],
										">(",
										((float)this.petEntryValue * 0.01f).ToString("F2"),
										"%)</color>"
									});
								}
								else
								{
									text3 = " (" + ((float)this.petEntryValue * 0.01f).ToString("F2") + "%)";
								}
							}
							else
							{
								string languageName = elementById.languageName;
								text = Singleton<LanguageManager>.Instance.GetInfoByID(languageName) ?? "";
							}
						}
						goto IL_025D;
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
						goto IL_025D;
					}
				}
				text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_none");
				IL_025D:
				this.textAttr.text = text;
				this.textAttrValue.text = text2 + text3;
				this.textAttrValue.transform.parent.gameObject.SetActive(!text2.IsEmpty());
				GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById((num == 0) ? 1 : num);
				this.imgBgList[num - 1].gameObject.SetActive(true);
			}
			else
			{
				this.petEntryId = 0;
				this.textAttr.text = "";
				this.textAttrValue.text = "";
				this.imgBgList[0].gameObject.SetActive(true);
				this.textUnlock.text = "";
			}
			base.gameObject.SetActiveSafe(flag && this.petEntryId > 0);
		}

		private void OnBtnItemClicked()
		{
			if (this.isLock)
			{
				return;
			}
			if (this.petEntryId > 0)
			{
				Pet_PetEntry elementById = GameApp.Table.GetManager().GetPet_PetEntryModelInstance().GetElementById(this.petEntryId);
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageName);
				string text = "";
				float num = ((elementById.entryType == 2) ? 1f : ((float)this.petEntryValue * 1f / (float)elementById.attrRange[1]));
				if (elementById.entryType == 1 || elementById.actionType == 1)
				{
					MergeAttributeData mergeAttribute = elementById.action.GetMergeAttribute();
					string text2 = Utility.Math.Round((double)(num * mergeAttribute.Value), 2).ToString() ?? "";
					text2 += (mergeAttribute.Header.Contains("%") ? "%" : "");
					text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageID) + "+" + text2;
					if (elementById.entryType == 1)
					{
						text = text + " (" + ((float)this.petEntryValue * 0.01f).ToString("F2") + "%)";
					}
				}
				else if (elementById.actionType == 2)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageID);
				}
				else if (elementById.actionType == 3)
				{
					string text3 = Utility.Math.Round((double)((float)int.Parse(elementById.action) * num), 2).ToString() ?? "";
					text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageID) + "+" + text3 + "%";
				}
				new InfoTipViewModule.InfoTipData
				{
					m_name = infoByID,
					m_info = text,
					m_position = base.transform.position,
					m_offsetY = 230f
				}.Open();
			}
		}

		public CustomButton btnItem;

		public List<CustomImage> imgBgList;

		public GameObject goAttrNode;

		public CustomText textAttr;

		public CustomText textAttrValue;

		public GameObject goLock;

		public CustomText textUnlock;

		[NonSerialized]
		public bool isLock;

		[NonSerialized]
		public int petEntryId;

		[NonSerialized]
		public int petEntryValue;
	}
}
