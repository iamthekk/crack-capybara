using System;
using Framework;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIItem : CustomBehaviour
	{
		public ItemType m_itemType { get; set; }

		public ItemIconSizeType m_iconSizeType { get; set; }

		public PropType m_propType { get; set; }

		public bool versionMatch { get; private set; } = true;

		public void SetArgs(object args)
		{
			this.m_args = args;
		}

		private void CheckLogic()
		{
			Type type = null;
			switch (this.m_itemType)
			{
			case ItemType.eCurrency:
				type = typeof(UIItemLogicCurrency);
				break;
			case ItemType.eEquip:
				type = typeof(UIItemLogicEquip);
				break;
			case ItemType.eHero:
				type = typeof(UIItemLogicCard);
				break;
			case ItemType.eUseItem:
			case ItemType.eMagicStone:
			case ItemType.eTowerTicket:
			case ItemType.eSevenDayCarnival:
			case ItemType.DungeonDragonLair:
			case ItemType.DungeonAstralTree:
			case ItemType.SwordIsland:
			case ItemType.DeepSeaRuins:
			case ItemType.eSpecialAvatarFrame:
			case ItemType.eSpecialClothes:
			case ItemType.eIAPRefreshData:
			case ItemType.eManaCrystal:
				type = typeof(UIItemDefault);
				break;
			case ItemType.eRandomPack:
				type = typeof(UIItemLogicSkill);
				break;
			case ItemType.eCustomPack:
				type = typeof(UIItemLogicCurrency);
				break;
			case ItemType.eTimePack:
				switch (this.m_propType)
				{
				case PropType.Dust:
					type = typeof(UIItemLogicPropDust);
					break;
				case PropType.Hourglass:
					type = typeof(UIItemLogicPropHourglass);
					break;
				case PropType.Box:
					type = typeof(UIItemLogicPropBox);
					break;
				case PropType.Key:
					type = typeof(UIItemLogicPropKey);
					break;
				case PropType.Strengthen:
					type = typeof(UIItemLogicStrengthen);
					break;
				}
				break;
			case ItemType.eDreamlandItem:
				type = typeof(UIItemLogicCurrency);
				break;
			case ItemType.eHeroePieces:
				type = typeof(UIItemHeroePieces);
				break;
			case ItemType.eFixedPropsPackage:
				type = typeof(UIItemLogicCurrency);
				break;
			case ItemType.eRelic:
				type = typeof(UIItemLogicRelic);
				break;
			case ItemType.eRelicFragment:
				type = typeof(UIItemLogicRelicFragment);
				break;
			case ItemType.eCrossArena:
				type = typeof(UIItemLogicCrossArena);
				break;
			case ItemType.ePet:
				type = typeof(UIItemLogicPet);
				break;
			case ItemType.ePetFragment:
				type = typeof(UIItemLogicPetFragment);
				break;
			case ItemType.eCollection:
				type = typeof(UIItemLogicCollection);
				break;
			case ItemType.eCollectionFragment:
			case ItemType.eCollectionShareFragment:
				type = typeof(UIItemLogicCollectionFragment);
				break;
			case ItemType.Artifact:
				type = typeof(UIItemLogicArtifact);
				break;
			case ItemType.Mount:
				type = typeof(UIItemLogicMount);
				break;
			case ItemType.eGuildGold:
			case ItemType.eGuildExp:
			case ItemType.eGuildDust:
				type = typeof(UIItemLogicCard);
				break;
			}
			if (type == null)
			{
				type = typeof(UIItemDefault);
				HLog.LogError(string.Format("UIItemCtrl.CheckLogic m_itemType:{0} m_propType:{1} error.", this.m_itemType, this.m_propType));
				return;
			}
			if (this.m_logic != null && this.m_logic.GetType() == type)
			{
				return;
			}
			BaseUIItemLogic baseUIItemLogic = Activator.CreateInstance(type) as BaseUIItemLogic;
			this.RemoveLogic();
			this.m_logic = baseUIItemLogic;
		}

		private void RemoveLogic()
		{
			this.m_logic = null;
		}

		public virtual void SetData(PropData propData)
		{
			this.m_propData = propData;
			if (this.m_propData == null || this.m_propData.id == 0U)
			{
				HLog.LogError("UIItem SetData(PropData), but propData is null or id = 0 !");
				return;
			}
			this.versionMatch = DxxTools.Game.TryVersionMatch(this.m_propData);
			if (!this.versionMatch)
			{
				return;
			}
			this.m_tableData = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.m_propData.id);
			if (this.m_tableData == null)
			{
				HLog.LogError(string.Format("Not found item id ={0}", this.m_propData.id));
				return;
			}
			this.m_itemType = this.m_tableData.GetItemType();
			this.m_propType = this.m_tableData.GetPropType();
			this.m_iconSizeType = this.m_tableData.GetIconSizeType();
			this.SetIconSize();
			this.CheckLogic();
			if (this.m_logic == null)
			{
				HLog.LogError("UIItemCtrl.SetData but logic is null!!!");
				return;
			}
			this.m_logicName = this.m_logic.GetType().ToString();
			this.m_logic.SetItem(this);
		}

		private void SetIconSize()
		{
			RectTransform rectTransform = this.m_imgItemIcon.rectTransform;
			switch (this.m_iconSizeType)
			{
			case ItemIconSizeType.Size_128x128:
				rectTransform.sizeDelta = new Vector2(128f, 128f);
				return;
			case ItemIconSizeType.Size_160x160:
				rectTransform.sizeDelta = new Vector2(160f, 160f);
				return;
			case ItemIconSizeType.Size_200x200:
				rectTransform.sizeDelta = new Vector2(200f, 200f);
				return;
			case ItemIconSizeType.Size_198x204:
				rectTransform.sizeDelta = new Vector2(198f, 204f);
				return;
			case ItemIconSizeType.Size_220x220:
				rectTransform.sizeDelta = new Vector2(220f, 220f);
				return;
			default:
				return;
			}
		}

		protected override void OnInit()
		{
			this.m_qualityS.SetActive(false);
			this.m_qualitySS.SetActive(false);
			this.onClick = new Action<UIItem, PropData, object>(this.OnBtnItemClick);
			if (this.m_button != null)
			{
				this.m_button.m_onClick = new Action(this.OnClickButton);
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_button != null)
			{
				this.m_button.m_onClick = null;
			}
			this.DestroyHeader();
			this.DestroySlider();
			this.DestroyStar();
		}

		public void OnRefresh()
		{
			if (this.versionMatch)
			{
				if (this.m_logic != null)
				{
					this.m_logic.OnRefresh();
					return;
				}
			}
			else
			{
				this.CloseOther();
				DxxTools.Game.VersionMatchNotExist_SetImage_Item_Icon(this.m_imgItemIcon);
				DxxTools.Game.VersionMatchNotExist_SetImage_Item_Quality(this.m_bgImage);
				this.OnRefreshCount();
				this.SetQualityTxtImageActive(false);
				this.SetTypeBgImageActive(false);
				this.SetQualityRank(1);
				this.m_imgFragmentFlag.gameObject.SetActive(false);
			}
		}

		public void RefreshAsNull()
		{
		}

		public void SetBg(string atlasName, string spriteName)
		{
			if (this.m_bgImage == null)
			{
				return;
			}
			this.m_bgImage.SetImage(atlasName, spriteName);
		}

		public void SetIcon(string atlasPath, string spriteName)
		{
			this.m_imgFragmentFlag.gameObject.SetActive(this.m_itemType == ItemType.eCollectionFragment || this.m_itemType == ItemType.eCollectionShareFragment);
			if (this.m_imgItemIcon != null)
			{
				this.m_imgItemIcon.SetImage(atlasPath, spriteName);
			}
		}

		public void SetFragmentFrame(int quality)
		{
		}

		public void SetMultiple(int count)
		{
			this.m_countMultiple = count;
			this.OnRefreshCount();
		}

		public virtual void OnRefreshCount()
		{
			if (this.m_countMultiple < 0)
			{
				this.m_countMultiple = 0;
			}
			if (this.m_propData != null)
			{
				this.SetCount((int)this.m_propData.count * this.m_countMultiple);
			}
		}

		public void SetCountShowType(UIItem.CountShowType countShowType)
		{
			this.m_countShowType = countShowType;
		}

		private void SetCount(int count)
		{
			if (this.m_countTxt == null)
			{
				return;
			}
			switch (this.m_countShowType)
			{
			case UIItem.CountShowType.ShowAll:
				this.m_countTxt.text = "x" + DxxTools.FormatNumber((long)count);
				return;
			case UIItem.CountShowType.MissOne:
				if (count <= 1)
				{
					this.m_countTxt.text = string.Empty;
					return;
				}
				this.m_countTxt.text = "x" + DxxTools.FormatNumber((long)count);
				return;
			case UIItem.CountShowType.MissZero:
				if (count <= 0)
				{
					this.m_countTxt.text = string.Empty;
					return;
				}
				this.m_countTxt.text = "x" + DxxTools.FormatNumber((long)count);
				return;
			case UIItem.CountShowType.MissAll:
				this.m_countTxt.text = string.Empty;
				return;
			default:
				return;
			}
		}

		public void SetCountText(string text)
		{
			if (this.m_countTxt == null)
			{
				return;
			}
			this.m_countTxt.text = text;
		}

		public void OnBtnItemClick(UIItem item, PropData data, object openData)
		{
			if (!DxxTools.Game.TryVersionMatchOperate(this.versionMatch))
			{
				return;
			}
			float num = base.rectTransform.rect.height * base.rectTransform.localScale.y * 0.5f + 10f;
			DxxTools.UI.OnItemClick(item, data, openData, item.transform.position, num);
		}

		public void SetClickDisabled(bool value)
		{
			this.m_button.enabled = !value;
		}

		public void CreateSlider(Action onFinish)
		{
			if (this.m_sliderParent == null)
			{
				return;
			}
			if (this.m_slider != null)
			{
				return;
			}
			GameObject globalGameObject = GameApp.UnityGlobal.GetGlobalGameObject("Assets/_Resources/Prefab/UI/Common/Item/Item_Slider.prefab");
			if (globalGameObject == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(globalGameObject);
			gameObject.transform.SetParentNormal(this.m_sliderParent, false);
			this.m_slider = gameObject.GetComponent<UIItemSlider>();
			this.m_slider.Init();
		}

		public void DestroySlider()
		{
			if (this.m_slider == null)
			{
				return;
			}
			this.m_slider.DeInit();
			this.m_slider = null;
		}

		public void CreateHeader()
		{
			if (this.m_headerParent == null)
			{
				return;
			}
			if (this.m_header != null)
			{
				return;
			}
			GameObject globalGameObject = GameApp.UnityGlobal.GetGlobalGameObject("Assets/_Resources/Prefab/UI/Common/Item/Item_Header.prefab");
			if (globalGameObject == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(globalGameObject);
			gameObject.transform.SetParentNormal(this.m_headerParent, false);
			this.m_header = gameObject.GetComponent<UIItemHeader>();
			this.m_header.Init();
		}

		public void DestroyHeader()
		{
			if (this.m_header == null)
			{
				return;
			}
			this.m_header.DeInit();
			this.m_header = null;
		}

		public void CreateStar()
		{
			if (this.m_starParent == null)
			{
				return;
			}
			if (this.m_star != null)
			{
				return;
			}
			GameObject globalGameObject = GameApp.UnityGlobal.GetGlobalGameObject("Assets/_Resources/Prefab/UI/Common/Item/Item_Star.prefab");
			if (globalGameObject == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(globalGameObject);
			gameObject.transform.SetParentNormal(this.m_starParent, false);
			this.m_star = gameObject.GetComponent<UIItemStar>();
			this.m_star.Init();
		}

		public void DestroyStar()
		{
			if (this.m_star == null)
			{
				return;
			}
			this.m_star.DeInit();
			this.m_star = null;
		}

		public void CloseOther()
		{
			if (this.m_headerParent != null)
			{
				this.m_headerParent.gameObject.SetActive(false);
			}
			if (this.m_starParent != null)
			{
				this.m_starParent.gameObject.SetActive(false);
			}
			if (this.m_sliderParent != null)
			{
				this.m_sliderParent.gameObject.SetActive(false);
			}
			this.SetQualityTxtImageActive(false);
			this.SetTypeBgImageActive(false);
		}

		public void SetTypeBgImage(string atlasName, string spriteName)
		{
			if (this.m_typeBgImage == null)
			{
				return;
			}
			this.m_typeBgImage.SetImage(atlasName, spriteName);
		}

		public void SetTypeImage(string atlasName, string spriteName)
		{
			if (this.m_typeImage == null)
			{
				return;
			}
			this.m_typeImage.SetImage(atlasName, spriteName);
		}

		public void SetTypeBgImageActive(bool active)
		{
			if (this.m_typeBgImage == null)
			{
				return;
			}
			this.m_typeBgImage.gameObject.SetActive(active);
		}

		public void SetQualityRank(int rank)
		{
			if (this.m_qualityS != null)
			{
				this.m_qualityS.SetActive(rank == 2);
			}
			if (this.m_qualitySS != null)
			{
				this.m_qualitySS.SetActive(rank == 3);
			}
		}

		public void SetQualityTxtImageActive(bool active)
		{
		}

		private void OnClickButton()
		{
			if (!DxxTools.Game.TryVersionMatchOperate(this.versionMatch))
			{
				return;
			}
			if (this.onClick != null)
			{
				this.onClick(this, this.m_propData, this.m_args);
			}
		}

		public void SetEnableButton(bool enable)
		{
			if (this.m_button == null)
			{
				return;
			}
			this.m_button.enabled = enable;
		}

		public void SetGrayState(bool isGray)
		{
			if (isGray)
			{
				this.uiGrays.m_targets = base.GetComponentsInChildren<Graphic>();
				this.uiGrays.SetUIGray();
				return;
			}
			this.uiGrays.Recovery();
		}

		public void SetTextCountScale(Vector3 scale)
		{
			this.m_countTxt.transform.localScale = scale;
		}

		public void SetMaskActive(bool active)
		{
			if (this.m_maskObj != null)
			{
				this.m_maskObj.SetActive(active);
			}
		}

		public void SetTickActive(bool active)
		{
			if (this.m_tickObj != null)
			{
				this.m_tickObj.SetActive(active);
			}
		}

		public void SetLockActive(bool active)
		{
			if (this.m_lockObj != null)
			{
				this.m_lockObj.SetActive(active);
			}
		}

		public const string StarPrefabPath = "Assets/_Resources/Prefab/UI/Common/Item/Item_Star.prefab";

		public const string HeaderPrefabPath = "Assets/_Resources/Prefab/UI/Common/Item/Item_Header.prefab";

		public const string SliderPrefabPath = "Assets/_Resources/Prefab/UI/Common/Item/Item_Slider.prefab";

		[Header("Data Setting")]
		[SerializeField]
		public PropData m_propData;

		[SerializeField]
		public Item_Item m_tableData;

		public object m_args;

		[Header("Click Setting")]
		public CustomButton m_button;

		[Header("Base Setting")]
		public CustomImage m_bgImage;

		public CustomImage m_imgItemIcon;

		public CustomImage m_imgFragmentFlag;

		public CustomText m_countTxt;

		public UIGrays uiGrays;

		[Label]
		public int m_countMultiple = 1;

		[Header("Slider")]
		public Transform m_sliderParent;

		public UIItemSlider m_slider;

		[Header("Header Txt")]
		public Transform m_headerParent;

		public UIItemHeader m_header;

		[Header("Star Setting")]
		public Transform m_starParent;

		public UIItemStar m_star;

		[Header("Equit Setting")]
		public CustomImage m_typeBgImage;

		public CustomImage m_typeImage;

		public GameObject m_qualityS;

		public GameObject m_qualitySS;

		[Header("ShowCount Setting")]
		public UIItem.CountShowType m_countShowType = UIItem.CountShowType.MissOne;

		[Header("Logic Ctrl")]
		[Label]
		public string m_logicName;

		public BaseUIItemLogic m_logic;

		public GameObject m_maskObj;

		public GameObject m_tickObj;

		public GameObject m_lockObj;

		public Action<UIItem, PropData, object> onClick;

		public enum CountShowType
		{
			ShowAll,
			MissOne,
			MissZero,
			MissAll
		}
	}
}
