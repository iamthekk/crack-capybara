using System;
using Framework;
using Framework.Logic.AttributeExpansion;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	[Obsolete("废弃， 直接使用 UIItem! ", true)]
	public class UIItemCtrl
	{
		public GameObject gameObject
		{
			get
			{
				if (this.m_item != null)
				{
					return this.m_item.gameObject;
				}
				return null;
			}
		}

		public Transform transform
		{
			get
			{
				if (this.m_item != null)
				{
					return this.m_item.transform;
				}
				return null;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				if (this.m_item != null)
				{
					return this.m_item.rectTransform;
				}
				return null;
			}
		}

		public void SetGameObject(GameObject gameObj)
		{
			if (gameObj == null)
			{
				HLog.LogError("UIItemCtrl SetGameObject gameObj is null !!!");
				return;
			}
			this.RemoveLogic();
			this.RemoveItem();
			this.m_item = gameObj.GetComponent<UIItem>();
			if (this.m_item == null)
			{
				HLog.LogError("UIItemCtrl SetGameObject gameObj not have [UIItem] !!!");
				return;
			}
			this.m_item.Init();
			this.m_item.onClick = new Action<UIItem, PropData, object>(this.OnClickButton);
		}

		public void SetArgs(object args)
		{
			this.m_args = args;
		}

		public void SetDataOnly(PropData propData)
		{
			this.m_propData = propData;
		}

		public virtual void SetData(PropData propData)
		{
			this.m_propData = propData;
			if (this.m_propData == null)
			{
				return;
			}
			if (this.m_item == null)
			{
				return;
			}
			this.m_tableData = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.m_propData.id);
			this.m_itemType = this.m_tableData.GetItemType();
			this.m_propType = this.m_tableData.GetPropType();
			this.m_item.SetData(propData);
			this.CheckLogic();
			if (this.m_logic == null)
			{
				HLog.LogError("UIItemCtrl.SetData but logic is null!!!");
				return;
			}
			this.m_logicName = this.m_logic.GetType().ToString();
		}

		public void Init()
		{
			this.OnInit();
		}

		protected void OnInit()
		{
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void DeInit()
		{
			this.OnDeInit();
		}

		protected void OnDeInit()
		{
			this.RemoveLogic();
			this.RemoveItem();
		}

		private void RemoveItem()
		{
			if (this.m_item != null)
			{
				this.m_item.DeInit();
				Object.Destroy(this.m_item);
			}
			this.m_item = null;
		}

		private void RemoveLogic()
		{
			this.m_logic = null;
		}

		public void OnRefresh()
		{
			if (this.m_logic != null)
			{
				this.m_logic.OnRefresh();
			}
		}

		private void CheckLogic()
		{
			if (this.m_logic != null && this.m_logic.m_itemType == this.m_itemType)
			{
				return;
			}
			BaseUIItemLogic baseUIItemLogic = null;
			ItemType itemType = this.m_itemType;
			switch (itemType)
			{
			case ItemType.eCurrency:
				baseUIItemLogic = new UIItemLogicCurrency();
				break;
			case ItemType.eEquip:
				baseUIItemLogic = new UIItemLogicEquip();
				break;
			case ItemType.eHero:
				baseUIItemLogic = new UIItemLogicCard();
				break;
			case ItemType.eUseItem:
				baseUIItemLogic = new UIItemDefault();
				break;
			case ItemType.eRandomPack:
				baseUIItemLogic = new UIItemLogicSkill();
				break;
			case ItemType.eCustomPack:
				baseUIItemLogic = new UIItemLogicCurrency();
				break;
			case ItemType.eTimePack:
				switch (this.m_propType)
				{
				case PropType.Dust:
					baseUIItemLogic = new UIItemLogicPropDust();
					break;
				case PropType.Hourglass:
					baseUIItemLogic = new UIItemLogicPropHourglass();
					break;
				case PropType.Box:
					baseUIItemLogic = new UIItemLogicPropBox();
					break;
				case PropType.Key:
					baseUIItemLogic = new UIItemLogicPropKey();
					break;
				case PropType.Strengthen:
					baseUIItemLogic = new UIItemLogicStrengthen();
					break;
				}
				break;
			case ItemType.eDreamlandItem:
				baseUIItemLogic = new UIItemLogicCurrency();
				break;
			case ItemType.eHeroePieces:
				baseUIItemLogic = new UIItemHeroePieces();
				break;
			case ItemType.eFixedPropsPackage:
			case ItemType.eBless:
			case (ItemType)12:
				break;
			case ItemType.eMagicStone:
				baseUIItemLogic = new UIItemDefault();
				break;
			case ItemType.eRelic:
				baseUIItemLogic = new UIItemLogicRelic();
				break;
			case ItemType.eRelicFragment:
				baseUIItemLogic = new UIItemLogicRelicFragment();
				break;
			default:
				if (itemType - ItemType.eGuildGold <= 2)
				{
					baseUIItemLogic = new UIItemLogicCard();
				}
				break;
			}
			this.RemoveLogic();
			this.m_logic = baseUIItemLogic;
			if (this.m_logic == null)
			{
				HLog.LogError(string.Format("UIItemCtrl.CheckLogic m_itemType:{0} m_propType:{1} error.", this.m_itemType, this.m_propType));
				this.m_logic = new UIItemDefault();
			}
		}

		private void OnClickButton(UIItem item, PropData data, object obj)
		{
			if (this.onClick != null)
			{
				this.onClick(this, data, obj);
			}
		}

		public void SetEnableButton(bool enable)
		{
			if (this.m_item == null)
			{
				return;
			}
			this.m_item.SetEnableButton(enable);
		}

		public void SetMultiple(int count)
		{
			if (this.m_propData == null)
			{
				return;
			}
			if (this.m_item == null)
			{
				return;
			}
			this.m_item.SetMultiple(count);
		}

		public void SetActive(bool active)
		{
			if (this.m_item == null)
			{
				return;
			}
			this.m_item.SetActive(active);
		}

		public int GetInstanceID()
		{
			if (this.m_item != null)
			{
				return this.m_item.gameObject.GetInstanceID();
			}
			return 0;
		}

		public UIItem m_item;

		public BaseUIItemLogic m_logic;

		public PropData m_propData;

		public object m_args;

		[SerializeField]
		[Label]
		private string m_logicName;

		protected Item_Item m_tableData;

		protected ItemType m_itemType;

		protected PropType m_propType;

		public Action<UIItemCtrl, PropData, object> onClick;
	}
}
