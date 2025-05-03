using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIGameEventButtonItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
			this.mAttributeUpdateItem.Init();
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			this.data = null;
			if (this.mAttributeUpdateItem != null)
			{
				this.mAttributeUpdateItem.DeInit();
				this.mAttributeUpdateItem = null;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void Refresh(GameEventUIButtonData data)
		{
			if (data == null)
			{
				return;
			}
			this.data = data;
			this.textInfo.text = data.GetInfo();
			string infoTip = data.GetInfoTip();
			if (!string.IsNullOrEmpty(infoTip))
			{
				this.tipNode.gameObject.SetActiveSafe(true);
				this.textTip.text = infoTip;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.tipLayoutRT);
			}
			else
			{
				this.tipNode.gameObject.SetActiveSafe(false);
			}
			this.useGoldObj.SetActiveSafe(false);
			this.textNeedInfo.text = "";
			this.uiGray.Recovery();
			this.mAttributeUpdateItem.ClearItems();
			this.mAttributeUpdateItem.gameObject.SetActiveSafe(false);
			switch (data.buttonType)
			{
			case GameEventButtonType.Normal:
			{
				List<AttributeTypeData> attributeList = data.GetAttributeList();
				List<ItemTypeData> itemList = data.GetItemList();
				List<SkillTypeData> skillList = data.GetSkillList();
				List<InfoTypeData> infoList = data.GetInfoList();
				if (attributeList.Count > 0 || itemList.Count > 0 || skillList.Count > 0 || infoList.Count > 0)
				{
					this.mAttributeUpdateItem.gameObject.SetActiveSafe(true);
					this.mAttributeUpdateItem.SetData(attributeList, itemList, skillList, infoList, null);
					this.tipNode.gameObject.SetActiveSafe(false);
				}
				break;
			}
			case GameEventButtonType.Buy:
				this.useGoldObj.SetActiveSafe(true);
				this.textBuyInfo.text = data.GetInfo();
				this.textPrice.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_CostFood", new object[] { data.param });
				if (Singleton<GameEventController>.Instance.IsBuyEnabled(data.param))
				{
					this.textPrice.color = Color.white;
				}
				else
				{
					Color color;
					if (ColorUtility.TryParseHtmlString("#FF604D", ref color))
					{
						this.textPrice.color = color;
					}
					this.uiGray.SetUIGray();
				}
				break;
			case GameEventButtonType.EventItemBuy:
			{
				Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(data.needId);
				if (elementById != null)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageId);
					this.textNeedInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_162", new object[] { infoByID });
				}
				else
				{
					HLog.LogError(string.Format("[Chapter_eventItem]表未找到id={0}", data.needId));
				}
				if (!Singleton<GameEventController>.Instance.IsEventItemBuyEnabled(data.needId, data.param))
				{
					this.uiGray.SetUIGray();
				}
				break;
			}
			case GameEventButtonType.NeedEventItem:
			{
				Chapter_eventItem elementById2 = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(data.needId);
				if (elementById2 != null)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.languageId);
					this.textNeedInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_163", new object[] { infoByID2 });
				}
				else
				{
					HLog.LogError(string.Format("[Chapter_eventItem]表未找到id={0}", data.needId));
				}
				if (!Singleton<GameEventController>.Instance.IsHaveEventItem(data.needId))
				{
					this.uiGray.SetUIGray();
				}
				break;
			}
			}
			if (data.isUndoFunction)
			{
				this.uiGray.SetUIGray();
			}
			Sprite sprite = this.m_spriteRegister.GetSprite(data.ButtonColorType.ToString());
			if (sprite == null)
			{
				HLog.LogError(string.Format("UISpriteRegister.GetSprite is null, name = {0}", data.ButtonColorType));
				sprite = this.m_spriteRegister.GetSprite(GameEventButtonColorEnum.Green.ToString());
			}
			this.m_buttonImage.sprite = sprite;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.costTrans);
		}

		private void OnClickButton()
		{
			if (this.isMoveToNpc)
			{
				return;
			}
			if (this.data != null)
			{
				if (this.data.isUndoFunction)
				{
					EventArgsString instance = Singleton<EventArgsString>.Instance;
					instance.SetData(this.data.undoTip);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
					return;
				}
				switch (this.data.buttonType)
				{
				case GameEventButtonType.Buy:
					if (!Singleton<GameEventController>.Instance.IsBuyEnabled(this.data.param))
					{
						EventArgsString instance2 = Singleton<EventArgsString>.Instance;
						instance2.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_40"));
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance2);
						return;
					}
					Singleton<GameEventController>.Instance.Buy(this.data.param);
					break;
				case GameEventButtonType.EventItemBuy:
					if (!Singleton<GameEventController>.Instance.IsEventItemBuyEnabled(this.data.needId, this.data.param))
					{
						string text = "";
						Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(this.data.needId);
						if (elementById != null)
						{
							text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageId);
						}
						EventArgsString instance3 = Singleton<EventArgsString>.Instance;
						instance3.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_164", new object[] { text }));
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance3);
						return;
					}
					Singleton<GameEventController>.Instance.EventItemBuy(this.data.needId, this.data.param);
					break;
				case GameEventButtonType.NeedEventItem:
					if (!Singleton<GameEventController>.Instance.IsHaveEventItem(this.data.needId))
					{
						string text2 = "";
						Chapter_eventItem elementById2 = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(this.data.needId);
						if (elementById2 != null)
						{
							text2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.languageId);
						}
						EventArgsString instance4 = Singleton<EventArgsString>.Instance;
						instance4.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_163", new object[] { text2 }));
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance4);
						return;
					}
					break;
				}
			}
			if (this.data != null)
			{
				Action<int> onClick = this.m_onClick;
				if (onClick == null)
				{
					return;
				}
				onClick(this.data.index);
			}
		}

		public void ShakeButton()
		{
			if (base.gameObject.activeSelf)
			{
				this.animator.Play("Shake");
			}
		}

		public void SetButtonActive(bool isActive)
		{
			this.isShakeEnable = isActive;
			this.isClickEnable = isActive;
		}

		public void SetMoveToNpc(bool isMoveTo)
		{
			if (this.data == null)
			{
				return;
			}
			this.isMoveToNpc = isMoveTo;
			this.textInfo.text = this.data.GetInfo();
			if (this.isMoveToNpc)
			{
				this.useGoldObj.SetActiveSafe(false);
				return;
			}
			if (this.data.buttonType == GameEventButtonType.Buy)
			{
				this.useGoldObj.SetActiveSafe(true);
				return;
			}
			this.useGoldObj.SetActiveSafe(false);
		}

		public void Rebuild()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tipLayoutRT);
			this.mAttributeUpdateItem.Rebuild();
		}

		public CustomButton button;

		public CustomText textInfo;

		public GameObject useGoldObj;

		public CustomText textBuyInfo;

		public CustomText textPrice;

		public Animator animator;

		public Image m_buttonImage;

		public SpriteRegister m_spriteRegister;

		public CustomText textNeedInfo;

		public UIGray uiGray;

		public UIAttributeUpdateItem mAttributeUpdateItem;

		public RectTransform costTrans;

		public CustomText textTip;

		public GameObject tipNode;

		public RectTransform tipLayoutRT;

		private GameEventUIButtonData data;

		public Action<int> m_onClick;

		private bool isMoveToNpc;

		private bool isShakeEnable;

		private bool isClickEnable;
	}
}
