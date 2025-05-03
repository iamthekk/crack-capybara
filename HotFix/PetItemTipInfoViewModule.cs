using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetItemTipInfoViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_petLevelProgressItem.Init();
			this.petSkillItem.Init();
			this.petSkillItem.onItemClickCallback = new Action<PetSkillItem>(this.OnSkillItemClick);
		}

		public override void OnOpen(object data)
		{
			this.m_petPassiveNodeItem.Init();
			if (data is PetItemTipInfoViewModule.OpenData)
			{
				this.openData = data as PetItemTipInfoViewModule.OpenData;
				this.UpdateView();
				return;
			}
			this.OnPopCommonClick(UIPopCommon.UIPopCommonClickType.ButtonClose);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_petPassiveNodeItem.DeInit();
		}

		public override void OnDelete()
		{
			this.m_petLevelProgressItem.DeInit();
			this.petSkillItem.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopCommonClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = null;
		}

		private void OnPopCommonClick(UIPopCommon.UIPopCommonClickType type)
		{
			GameApp.View.CloseView(ViewName.PetItemTipInfoViewModule, null);
		}

		private void UpdateView()
		{
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)this.openData.petDto.ConfigId);
			this.ShowPlayerModel(elementById.memberId);
			Quality_petQuality elementById2 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById.quality);
			this.m_imgQualityBg.SetImage(elementById2.atlasId, elementById2.typeTxtBg);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID);
			this.m_txtQuality.text = string.Concat(new string[] { "<color=", elementById2.colorNum, ">", infoByID, "</color>" });
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.m_txtLevelValue.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { this.openData.petDto.PetLv });
			this.m_txtHpValue.text = "0";
			this.m_txtAtkValue.text = "0";
			this.m_txtDefValue.text = "0";
			List<MergeAttributeData> petLevelMergeAttributeData = elementById.GetPetLevelMergeAttributeData((int)this.openData.petDto.PetLv, GameApp.Table.GetManager());
			for (int i = 0; i < petLevelMergeAttributeData.Count; i++)
			{
				MergeAttributeData mergeAttributeData = petLevelMergeAttributeData[i];
				string header = mergeAttributeData.Header;
				if (!(header == "HPMax"))
				{
					if (!(header == "Attack"))
					{
						if (header == "Defence")
						{
							this.m_txtDefValue.text = string.Format("{0}", mergeAttributeData.Value.AsLong());
						}
					}
					else
					{
						this.m_txtAtkValue.text = string.Format("{0}", mergeAttributeData.Value.AsLong());
					}
				}
				else
				{
					this.m_txtHpValue.text = string.Format("{0}", mergeAttributeData.Value.AsLong());
				}
			}
			this.m_petLevelProgressItem.SetData((int)this.openData.petDto.ConfigId, (int)this.openData.petDto.PetLv);
			PetSkillData petSkillData = new PetSkillData().Init(elementById.battleSkill, (int)this.openData.petDto.ConfigId, (int)this.openData.petDto.PetLv);
			this.petSkillItem.SetData(petSkillData, elementById.quality);
			this.m_petPassiveNodeItem.SetData(this.openData.petDto.TrainingAttributeIds.ToList<int>(), this.openData.petDto.TrainingAttributeValues.ToList<int>());
		}

		private async void ShowPlayerModel(int memberId)
		{
			if (!this.cacheMemberId.Equals(memberId))
			{
				this.cacheMemberId = memberId;
				ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
				this.uiSpineModelItem.SetScale(elementById.uiScale);
				await this.uiSpineModelItem.ShowModel(memberId, 0, "Idle", true);
			}
		}

		private void OnSkillItemClick(PetSkillItem petSkillItem)
		{
			if (petSkillItem == null || petSkillItem.data == null)
			{
				return;
			}
			PetSkillEffectTipViewModule.OpenData openData = new PetSkillEffectTipViewModule.OpenData();
			openData.petId = (int)this.openData.petDto.ConfigId;
			openData.battleSkill = petSkillItem.data;
			openData.petLevel = (int)this.openData.petDto.PetLv;
			GameApp.View.OpenView(ViewName.PetSkillEffectTipViewModule, openData, 2, null, null);
		}

		[Header("宠物模型")]
		public UISpineModelItem uiSpineModelItem;

		public UIPopCommon popCommon;

		public CustomText txtTitle;

		public PetSkillItem petSkillItem;

		public CustomText m_txtQuality;

		public CustomImage m_imgQualityBg;

		[Header("属性")]
		public CustomText m_txtLevelValue;

		public CustomText m_txtHpValue;

		public CustomText m_txtAtkValue;

		public CustomText m_txtDefValue;

		public PetLevelProgressItem m_petLevelProgressItem;

		public PetPassiveNodeItem m_petPassiveNodeItem;

		private PetItemTipInfoViewModule.OpenData openData;

		private int cacheMemberId;

		public class OpenData
		{
			public PetDto petDto;
		}
	}
}
