using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIHeroEquipItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_bt.onClick.AddListener(new UnityAction(this.OnClickBt));
			this.m_rednode.Value = 0;
			this.m_putonObj.SetActive(false);
			this.m_maskObj.SetActive(false);
			this.m_lockObj.SetActive(false);
			this.m_tickObj.SetActive(false);
		}

		protected override void OnDeInit()
		{
			this.m_equipData = null;
			if (this.m_bt != null)
			{
				this.m_bt.onClick.RemoveListener(new UnityAction(this.OnClickBt));
			}
			this.m_onClick = null;
		}

		protected void OnUpdate(float deltaTime)
		{
		}

		public void SetEquipType(EquipType equipType, int equipTypeIndex)
		{
			this.equipType = equipType;
			this.equipTypeIndex = equipTypeIndex;
		}

		public void SetMergeMaterialData(EquipData matEquipData, int equipId, int composeId)
		{
			this.m_equipData = matEquipData;
			if (matEquipData != null)
			{
				this.RefreshData(matEquipData);
				this.SetMaskActive(false);
				this.SetButtonEnable(true);
				return;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(composeId);
			this.RefreshData(equipId, elementById.composeNeed2, 0);
			this.SetMaskActive(true);
			this.SetButtonEnable(false);
			if (elementById.composeNeed1 == 1)
			{
				Equip_equipCompose elementById2 = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(elementById.composeNeed2);
				Quality_equipQuality elementById3 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById2.qualityColor);
				Equip_equip elementById4 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(equipId);
				Equip_equipType elementById5 = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById(elementById4.Type);
				this.m_imgQualityS.SetActive(false);
				this.m_imgQualitySS.SetActive(false);
				string atlasPath = GameApp.Table.GetAtlasPath(elementById5.atlasID);
				if (this.m_icon != null)
				{
					this.m_icon.SetImage(atlasPath, elementById5.iconName);
				}
				string atlasPath2 = GameApp.Table.GetAtlasPath(elementById3.atlasId);
				if (this.m_qualityBg != null)
				{
					this.m_qualityBg.SetImage(atlasPath2, elementById3.bgSpriteName);
				}
				if (this.m_typeBg != null)
				{
					this.m_typeBg.SetImage(atlasPath2, elementById3.equipTypeBgSpriteName ?? "");
				}
				if (elementById2.qualityPlus > 0)
				{
					this.m_imgComposePlus.gameObject.SetActive(true);
					this.m_imgComposePlus.SetImage(atlasPath2, elementById3.composePlusSpriteName);
					this.m_txtComposePlus.text = string.Format("+{0}", elementById2.qualityPlus);
					return;
				}
				this.m_imgComposePlus.gameObject.SetActive(false);
			}
		}

		public virtual void RefreshData(EquipData equipData)
		{
			this.m_equipData = equipData;
			if (this.m_equipData == null)
			{
				return;
			}
			this.RefreshData((int)equipData.id, equipData.composeId, (int)equipData.level);
		}

		public virtual void RefreshData(int equipId, int composeId, int level)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(equipId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("UIHeroEquipUpCtrl.RefreshData equipData.id={0}, itemTable is null", equipId));
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(equipId);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("UIHeroEquipUpCtrl.RefreshData equipData.id={0}, itemTable is null", equipId));
				return;
			}
			Equip_equipType elementById3 = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById(elementById2.Type);
			if (elementById3 == null)
			{
				HLog.LogError(string.Format("UIHeroEquipUpCtrl.RefreshData equipData.id={0}, equipTypeTable={1},itemTable is null", equipId, elementById2.Type));
				return;
			}
			Equip_equipCompose elementById4 = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(composeId);
			Quality_equipQuality elementById5 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById4.qualityColor);
			string atlasPath = GameApp.Table.GetAtlasPath(elementById5.atlasId);
			if (this.m_qualityBg != null)
			{
				this.m_qualityBg.SetImage(atlasPath, elementById5.bgSpriteName);
			}
			if (elementById4.qualityPlus > 0)
			{
				this.m_imgComposePlus.gameObject.SetActive(true);
				this.m_imgComposePlus.SetImage(atlasPath, elementById5.composePlusSpriteName);
				this.m_txtComposePlus.text = string.Format("+{0}", elementById4.qualityPlus);
			}
			else
			{
				this.m_imgComposePlus.gameObject.SetActive(false);
			}
			this.m_imgQualityS.SetActive(elementById2.rank == 2);
			this.m_imgQualitySS.SetActive(elementById2.rank == 3);
			string atlasPath2 = GameApp.Table.GetAtlasPath(elementById.atlasID);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(atlasPath2, elementById.icon);
			}
			string atlasPath3 = GameApp.Table.GetAtlasPath(elementById3.atlasID);
			if (this.m_typeIcon != null)
			{
				this.m_typeIcon.SetImage(atlasPath3, elementById3.iconName);
			}
			if (this.m_typeBg != null)
			{
				this.m_typeBg.SetImage(atlasPath, elementById5.equipTypeBgSpriteName ?? "");
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = ((level > 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { level }) : "");
			}
		}

		public void SetUpLevelActive(bool active)
		{
			if (this.m_upLevelObj == null)
			{
				return;
			}
			this.m_upLevelObj.SetActive(active);
		}

		public void SetRedNodeActive(bool active)
		{
			if (this.m_rednode == null)
			{
				return;
			}
			this.m_rednode.Value = (active ? 1 : 0);
		}

		public void SetRedType(RedNodeType redNodeType)
		{
			this.m_rednode.SetType(redNodeType);
		}

		public void SetMaskActive(bool active)
		{
			if (this.m_maskObj == null)
			{
				return;
			}
			this.m_maskObj.SetActive(active);
		}

		public void SetLockActive(bool active)
		{
			if (this.m_lockObj == null)
			{
				return;
			}
			this.m_lockObj.SetActive(active);
		}

		public void SetPutOnActive(bool active)
		{
			if (this.m_putonObj == null)
			{
				return;
			}
			this.m_putonObj.SetActive(active);
		}

		public void SetTickActive(bool active)
		{
			if (this.m_tickObj == null)
			{
				return;
			}
			this.m_tickObj.SetActive(active);
		}

		public void SetButtonEnable(bool enable)
		{
			if (this.m_bt == null)
			{
				return;
			}
			this.m_bt.enabled = enable;
		}

		public void SetNameActive(bool active)
		{
			if (this.m_nameTxt == null)
			{
				return;
			}
			this.m_nameTxt.gameObject.SetActive(active);
		}

		private void OnClickBt()
		{
			if (this.m_onClick == null)
			{
				return;
			}
			this.m_onClick(this);
		}

		public EquipType equipType = EquipType.Weapon;

		public int equipTypeIndex;

		[SerializeField]
		private CustomButton m_bt;

		[SerializeField]
		private CustomImage m_qualityBg;

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private CustomImage m_typeBg;

		[SerializeField]
		private CustomImage m_typeIcon;

		[SerializeField]
		private CustomText m_nameTxt;

		[SerializeField]
		private RedNodeOneCtrl m_rednode;

		[SerializeField]
		private GameObject m_upLevelObj;

		[SerializeField]
		private GameObject m_maskObj;

		[SerializeField]
		private GameObject m_putonObj;

		[SerializeField]
		private GameObject m_tickObj;

		[SerializeField]
		private GameObject m_lockObj;

		[SerializeField]
		private GameObject m_imgQualityS;

		[SerializeField]
		private GameObject m_imgQualitySS;

		[SerializeField]
		private CustomImage m_imgComposePlus;

		[SerializeField]
		private CustomText m_txtComposePlus;

		public EquipData m_equipData;

		public Action<UIHeroEquipItem> m_onClick;
	}
}
