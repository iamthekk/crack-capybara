using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Item;
using UnityEngine;

namespace HotFix
{
	public class ItemInfoViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.ItemUI.Init();
			this.UseCtrl.Init();
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				HLog.LogError("ItemInfoViewModule.OnOpen data == null");
				return;
			}
			this.m_infoOpenData = (ItemInfoOpenData)data;
			if (this.m_infoOpenData == null)
			{
				HLog.LogError("ItemInfoViewModule.OnOpen item == null");
				return;
			}
			this.m_onItemInfoMathVolume = this.m_infoOpenData.m_onItemInfoMathVolume;
			this.ItemUI.SetData(this.m_infoOpenData.m_propData);
			this.ItemUI.SetCountShowType(UIItem.CountShowType.MissOne);
			this.OnRefreshUI();
		}

		public void OnRefreshUI()
		{
			if (this.ItemUI != null)
			{
				this.ItemUI.SetData(this.m_infoOpenData.m_propData);
				this.ItemUI.OnRefresh();
			}
			this.UpdateUI();
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.m_infoOpenData.m_propData.id);
			ItemType itemType = elementById.GetItemType();
			this.UpdateOpenType(itemType, this.m_infoOpenData.m_openDataType);
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.Text_Type.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.typeDescribeID);
			this.Text_Info.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.describeID);
		}

		private void UpdateUI()
		{
			this.UseCtrl.SetData(this.m_infoOpenData.m_propData);
			if (this.UseCtrl.IsCanShow)
			{
				this.UseCtrl.Open(new Action(this.OnClickUseBt));
			}
			string text = this.m_infoOpenData.m_propData.count.ToString();
			if (this.m_onItemInfoMathVolume != null)
			{
				text = this.m_onItemInfoMathVolume(this.m_infoOpenData.m_propData).ToString();
			}
			this.Text_InfoCount.text = text;
		}

		private int GetShowType(ItemType itemType)
		{
			int num = 0;
			if (itemType != ItemType.eTimePack)
			{
				if (itemType == ItemType.eHeroePieces)
				{
					num = 1;
				}
			}
			else
			{
				num = 1;
			}
			return num;
		}

		private void UpdateOpenType(ItemType itemType, ItemInfoOpenDataType openDataType)
		{
			int showType = this.GetShowType(itemType);
			switch (openDataType)
			{
			case ItemInfoOpenDataType.eBag:
				if (showType == 0)
				{
					this.UseCtrl.SetActive(false);
				}
				else if (showType == 1)
				{
					this.UseCtrl.SetActive(true);
				}
				else
				{
					this.UseCtrl.SetActive(false);
				}
				break;
			case ItemInfoOpenDataType.eHero:
				this.UseCtrl.SetActive(false);
				break;
			case ItemInfoOpenDataType.eShow:
				this.UseCtrl.SetActive(false);
				break;
			default:
				this.UseCtrl.SetActive(false);
				break;
			}
			float num = 0f;
			if (!this.UseCtrl.gameObject.activeSelf)
			{
				num -= 472f;
			}
			this.uiPopCommon.SetRtfHeightByOffset(num);
		}

		public override void OnClose()
		{
			if (this.UseCtrl.IsOpen)
			{
				this.UseCtrl.Close();
			}
		}

		public override void OnDelete()
		{
			if (this.UseCtrl != null)
			{
				this.UseCtrl.DeInit();
			}
			if (this.ItemUI != null)
			{
				this.ItemUI.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseView();
			}
		}

		private void OnClickCloseView()
		{
			GameApp.View.CloseView(ViewName.ItemInfoViewModule, null);
		}

		private void OnClickUseBt()
		{
			uint level = GameApp.Data.GetDataModule(DataName.LoginDataModule).userLevel.Level;
			uint id = this.m_infoOpenData.m_propData.id;
			int useCount = this.UseCtrl.GetUseCount();
			NetworkUtils.Item.SendItemUseRequest(this.m_infoOpenData.m_propData.rowId, (uint)useCount, (uint)this.UseCtrl.GetUseIndex(), delegate(bool isOk, ItemUseResponse resp)
			{
				if (!isOk)
				{
					return;
				}
				DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				ItemDto itemDataByServerID = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataByServerID(this.m_infoOpenData.m_propData.rowId);
				if (itemDataByServerID != null && itemDataByServerID.Count > 0UL)
				{
					this.m_infoOpenData.m_propData = itemDataByServerID.ToPropData();
					this.OnRefreshUI();
					GameApp.Event.DispatchNow(this, 118, null);
					return;
				}
				this.OnClickCloseView();
				GameApp.Event.DispatchNow(this, 118, null);
			});
		}

		[Header("常规通用组件")]
		public UIPopCommon uiPopCommon;

		public CustomText Text_Title;

		[Header("物品基础信息")]
		public CustomText Text_Type;

		public UIItem ItemUI;

		public CustomText Text_InfoCount;

		[Header("物品道具信息")]
		public CustomText Text_Info;

		[Header("物品使用")]
		public ItemInfoViewUseCtrl UseCtrl;

		private ItemInfoOpenData m_infoOpenData;

		public OnItemInfoMathVolume m_onItemInfoMathVolume;
	}
}
