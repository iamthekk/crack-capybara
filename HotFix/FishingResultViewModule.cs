using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class FishingResultViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.attributeItem.Init();
			this.skillItem.Init();
			this.buttonOk.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
		}

		public override void OnOpen(object data)
		{
			this.attributeItem.ClearItems();
			float num = (float)this.NormalHeight;
			FishingResultData fishingResultData = data as FishingResultData;
			if (fishingResultData != null)
			{
				if (fishingResultData.isSuccess && fishingResultData.fishData != null)
				{
					this.successObj.SetActiveSafe(true);
					this.failObj.SetActiveSafe(false);
					this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Success");
					this.textFishName.text = this.GetQualityColor(fishingResultData.fishData.Config.type, Singleton<LanguageManager>.Instance.GetInfoByID(fishingResultData.fishData.Config.nameId));
					this.textQuality.text = this.GetQualityText(fishingResultData.fishData.Config.type);
					string atlasPath = GameApp.Table.GetAtlasPath(fishingResultData.fishData.Config.atlas);
					this.imageFishIcon.SetImage(atlasPath, fishingResultData.fishData.Config.icon);
					this.textWeight.text = ((float)fishingResultData.fishData.fishWeight / 100f).ToString() + "KG";
					int num2 = fishingResultData.fishData.Config.type - 1;
					if (num2 > 0 && num2 < this.qualityArr.Length)
					{
						this.imageQualityBg.sprite = this.qualityArr[num2];
						this.imageWeightQuality.sprite = this.weightQualityArr[num2];
					}
					List<NodeAttParam> list = fishingResultData.fishData.Config.attributes.GetMergeAttributeData().ToNodeAttParams();
					this.CalcWeightUpAttribute(list, fishingResultData.fishData.floatWeight);
					Singleton<GameEventController>.Instance.MergerAttribute(list);
					List<AttributeTypeData> attParamList = NodeAttParam.GetAttParamList(list);
					int skillBuild = fishingResultData.fishData.Config.skillBuild;
					this.skillItem.gameObject.SetActiveSafe(skillBuild > 0);
					if (skillBuild > 0)
					{
						num = (float)this.HaveSkillHeight;
						GameEventSkillBuildData specifiedSkill = Singleton<GameEventController>.Instance.GetSpecifiedSkill(skillBuild);
						Singleton<GameEventController>.Instance.SelectSkill(specifiedSkill, false);
						this.skillItem.Refresh(specifiedSkill, false, true);
					}
					if (attParamList.Count > 0)
					{
						this.attributeItem.SetData(attParamList);
					}
				}
				else
				{
					this.successObj.SetActiveSafe(false);
					this.failObj.SetActiveSafe(true);
					this.skillItem.gameObject.SetActiveSafe(false);
					this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Fail");
					if (fishingResultData.failType == 1)
					{
						this.textFailTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Fail_Tip1");
					}
					else if (fishingResultData.failType == 2)
					{
						this.textFailTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Fail_Tip2");
					}
					else
					{
						this.textFailTip.text = "";
					}
				}
				this.root.sizeDelta = new Vector2(this.root.sizeDelta.x, num);
				return;
			}
			this.OnCloseSelf();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonOk.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.attributeItem.DeInit();
			this.attributeItem = null;
			this.skillItem.DeInit();
			this.skillItem = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
		}

		private void OnCloseSelf()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIFishingResult_Close, null);
			GameApp.View.CloseView(ViewName.FishingResultViewModule, null);
		}

		private string GetQualityColor(int quality, string text)
		{
			if (quality == 1)
			{
				return "<color=#4695D1>" + text + "</color>";
			}
			if (quality == 2)
			{
				return "<color=#9F5CEE>" + text + "</color>";
			}
			if (quality == 3)
			{
				return "<color=#F4883F>" + text + "</color>";
			}
			return text;
		}

		private string GetQualityText(int quality)
		{
			if (quality == 1)
			{
				return this.GetQualityColor(quality, Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Quality_1"));
			}
			if (quality == 2)
			{
				return this.GetQualityColor(quality, Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Quality_2"));
			}
			if (quality == 3)
			{
				return this.GetQualityColor(quality, Singleton<LanguageManager>.Instance.GetInfoByID("UIFishingResult_Quality_3"));
			}
			return "";
		}

		private void CalcWeightUpAttribute(List<NodeAttParam> list, int floatWeight)
		{
			for (int i = 0; i < list.Count; i++)
			{
				double num = list[i].baseCount * (double)((float)floatWeight / 100f);
				list[i].SetNum(num);
			}
		}

		private void OnShowDetailInfo(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgClickSkill eventArgClickSkill = eventargs as EventArgClickSkill;
			if (eventArgClickSkill == null)
			{
				return;
			}
			GameEventSkillBuildData skillBuildData = eventArgClickSkill.skillItem.GetSkillBuildData();
			new InfoTipViewModule.InfoTipData
			{
				m_name = skillBuildData.skillName,
				m_info = skillBuildData.skillFullDetail,
				m_position = eventArgClickSkill.skillItem.GetPosition(),
				m_offsetY = 280f
			}.Open();
		}

		public CustomButton buttonOk;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public GameObject successObj;

		public CustomText textFishName;

		public CustomText textQuality;

		public CustomImage imageQualityBg;

		public Sprite[] qualityArr;

		public CustomImage imageFishIcon;

		public CustomImage imageWeightQuality;

		public Sprite[] weightQualityArr;

		public CustomText textWeight;

		public GameObject failObj;

		public CustomText textFailTip;

		public UIAttributeUpdateItem attributeItem;

		public UIGameEventSkillItem skillItem;

		public RectTransform root;

		public int NormalHeight = 1000;

		public int HaveSkillHeight = 1300;
	}
}
