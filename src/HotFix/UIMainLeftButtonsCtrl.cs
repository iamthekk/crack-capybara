using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainLeftButtonsCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_isUnfold = true;
			this.m_residentButtons.Clear();
			this.m_refreshButtons.Clear();
			this.m_allButtons.Clear();
			this.OnRefresh();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPChapterGiftData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_MeetingGift_RefreshEnter, new HandlerEvent(this.OnEventRefresh));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPChapterGiftData, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_MeetingGift_RefreshEnter, new HandlerEvent(this.OnEventRefresh));
			this.ClearAllButtons();
			this.ClearRedNode();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		private void CreateGiftButtons()
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			IAP_PushPacks unBuyGift = dataModule.MeetingGift.GetUnBuyGift();
			if (unBuyGift != null)
			{
				UIMainButton_Gift uimainButton_Gift;
				this.<CreateGiftButtons>g__CreateGiftButton|19_0(unBuyGift.id, out uimainButton_Gift);
			}
			foreach (IAPChapterGift.Data data in dataModule.ChapterGift.GetEnableData())
			{
				if (data != null && data.IsEnable())
				{
					UIMainButton_Gift uimainButton_Gift;
					this.<CreateGiftButtons>g__CreateGiftButton|19_0(data.Id, out uimainButton_Gift);
				}
			}
		}

		private void ClearAllButtons()
		{
			foreach (BaseUIMainButton baseUIMainButton in this.m_refreshButtons)
			{
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.DeInit();
					Object.Destroy(baseUIMainButton.gameObject);
				}
			}
			this.m_refreshButtons.Clear();
			foreach (BaseUIMainButton baseUIMainButton2 in this.m_residentButtons)
			{
				if (!(baseUIMainButton2 == null))
				{
					baseUIMainButton2.DeInit();
				}
			}
			this.m_residentButtons.Clear();
			this.m_allButtons.Clear();
		}

		private void ClearRedNode()
		{
			this.UnRegRedChange();
			this.m_redNodePaths.Clear();
		}

		public void OnLanguageChange()
		{
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnLanguageChange();
				}
			}
		}

		public void OnRefresh()
		{
			this.ClearAllButtons();
			this.ClearRedNode();
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.m_allButtons.Add(this.m_carnival);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Carnival", this.m_carnival.transform);
			this.m_redNodePaths.Add("Main.Carnival");
			bool flag = dataModule.MeetingGift.IsAllGet();
			bool flag2 = dataModule.MeetingGift.IsAllEnd();
			if (dataModule.MeetingGift.IsUnlock || (!flag2 && !flag))
			{
				this.m_pushPackRecharge.SetShow(true);
				this.m_residentButtons.Add(this.m_pushPackRecharge);
				Singleton<GameFunctionController>.Instance.SetFunctionTarget("PushPackRecharge", this.m_pushPackRecharge.transform);
				this.m_redNodePaths.Add("IAPGift.Meeting");
				int num = 0;
				IAP_PushPacks unBuyGift = dataModule.MeetingGift.GetUnBuyGift();
				if (unBuyGift != null)
				{
					num = unBuyGift.id;
				}
				else
				{
					List<IAP_PushPacks> gifts = dataModule.MeetingGift.GetGifts();
					if (gifts.Count > 0)
					{
						List<IAP_PushPacks> list = gifts;
						num = list[list.Count - 1].id;
					}
				}
				if (num > 0)
				{
					this.m_pushPackRecharge.SetData(num);
				}
			}
			else
			{
				this.m_pushPackRecharge.SetShow(false);
			}
			this.m_residentButtons.Add(this.m_rechargeGift);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("RechargeGift", this.m_rechargeGift.transform);
			this.m_redNodePaths.Add("IAPRechargeGift");
			this.m_residentButtons.Add(this.m_privilegeCard);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("PrivilegeCard", this.m_privilegeCard.transform);
			this.m_redNodePaths.Add("IAPPrivileggeCard");
			this.m_residentButtons.Add(this.m_pushGift);
			this.m_chainPacks.PreInit();
			this.m_residentButtons.Add(this.m_chainPacks);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget(this.m_chainPacks.Function_TargetName, this.m_chainPacks.transform);
			this.m_redNodePaths.Add(this.m_chainPacks.REDPOINT_NAME);
			this.m_allButtons.AddRange(this.m_residentButtons);
			this.m_allButtons.AddRange(this.m_refreshButtons);
			this.InitAllButton();
			this.OnRefreshScroll();
			this.RegRedChange();
			this.RefreshRed();
		}

		private int GetCount()
		{
			int num = 0;
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null) && baseUIMainButton.IsShow())
				{
					num++;
				}
			}
			return num;
		}

		private void OnRefreshScroll()
		{
			int count = this.GetCount();
			if (count == 0)
			{
				if (this.m_scroll != null)
				{
					this.m_scroll.gameObject.SetActive(false);
				}
				return;
			}
			if (this.m_scroll != null)
			{
				this.m_scroll.gameObject.SetActive(true);
			}
			if (this.m_parent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_parent);
			}
			if (this.m_gridLayoutGroup == null)
			{
				return;
			}
			bool isUnfold = this.m_isUnfold;
			int num = Mathf.CeilToInt((float)count * 1f / (float)this.m_gridLayoutGroup.constraintCount);
			int num2 = ((count <= this.m_gridLayoutGroup.constraintCount) ? count : this.m_gridLayoutGroup.constraintCount);
			Vector2 zero = Vector2.zero;
			float num3 = 0f;
			if (!isUnfold)
			{
				num = 1;
				num2 = 1;
			}
			if (this.m_chooseCustomButton != null)
			{
				this.m_chooseCustomButton.SetActive(false);
			}
			if (this.m_scroll != null)
			{
				RectTransform rectTransform = this.m_scroll.transform as RectTransform;
				float num4 = (float)num * (this.m_gridLayoutGroup.cellSize.x + this.m_gridLayoutGroup.spacing.x) + (float)this.m_gridLayoutGroup.padding.horizontal - this.m_gridLayoutGroup.spacing.x;
				float num5 = (float)num2 * (this.m_gridLayoutGroup.cellSize.y + this.m_gridLayoutGroup.spacing.y) + (float)this.m_gridLayoutGroup.padding.vertical - this.m_gridLayoutGroup.spacing.y;
				rectTransform.sizeDelta = new Vector2(num4, num5) + zero;
				if (this.m_scroll.viewport != null)
				{
					Utility.SetBottom(this.m_scroll.viewport, num3);
				}
			}
		}

		private void RegRedChange()
		{
			for (int i = 0; i < this.m_redNodePaths.Count; i++)
			{
				string text = this.m_redNodePaths[i];
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.RegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
		}

		private void UnRegRedChange()
		{
			for (int i = 0; i < this.m_redNodePaths.Count; i++)
			{
				string text = this.m_redNodePaths[i];
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.UnRegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
		}

		private void RefreshRed()
		{
			foreach (string text in this.m_redNodePaths)
			{
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.ReCalc(text, true);
				}
			}
		}

		private void OnRefreshRedPointChange(RedNodeListenData obj)
		{
			if (!this.m_isUnfold)
			{
				for (int i = 0; i < this.m_redNodePaths.Count; i++)
				{
					RedPointDataRecord record = RedPointController.Instance.GetRecord(this.m_redNodePaths[i]);
					if (record != null && record.RedPointCount > 0)
					{
						break;
					}
				}
			}
			foreach (BaseUIMainButton baseUIMainButton in this.m_allButtons)
			{
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnRefreshAnimation();
				}
			}
		}

		private void InitAllButton()
		{
			foreach (BaseUIMainButton baseUIMainButton in this.m_allButtons)
			{
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.Init();
				}
			}
			this.m_allButtons.Sort(delegate(BaseUIMainButton a, BaseUIMainButton b)
			{
				int num;
				int num2;
				a.GetPriority(out num, out num2);
				int num3;
				int num4;
				b.GetPriority(out num3, out num4);
				if (num == num3)
				{
					return num4 - num2;
				}
				return num3 - num;
			});
			foreach (BaseUIMainButton baseUIMainButton2 in this.m_allButtons)
			{
				if (!(baseUIMainButton2 == null) && !(baseUIMainButton2.rectTransform == null))
				{
					baseUIMainButton2.rectTransform.SetAsFirstSibling();
					baseUIMainButton2.SetShow(baseUIMainButton2.IsShow());
				}
			}
		}

		private void OnClickChooseButton(CustomChooseButton obj)
		{
			this.m_isUnfold = !this.m_isUnfold;
			this.OnRefreshScroll();
		}

		private void OnEventRefresh(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefresh();
		}

		public void SwitchToFold()
		{
			this.m_isUnfold = false;
			this.OnRefreshScroll();
		}

		public void SwitchToUnfold()
		{
			this.m_isUnfold = true;
			this.OnRefreshScroll();
		}

		public void SwitchFoldState(int state)
		{
			if (state == 1)
			{
				this.SwitchToFold();
				return;
			}
			if (state != 2)
			{
				this.OnClickChooseButton(null);
				return;
			}
			this.SwitchToUnfold();
		}

		[CompilerGenerated]
		private bool <CreateGiftButtons>g__CreateGiftButton|19_0(int id, out UIMainButton_Gift buttonGift)
		{
			buttonGift = null;
			if (this.m_pushPackRecharge == null || this.m_parent == null)
			{
				return false;
			}
			UIMainButton_Gift uimainButton_Gift = Object.Instantiate<UIMainButton_Gift>(this.m_pushPackRecharge);
			uimainButton_Gift.transform.SetParentNormal(this.m_parent, false);
			uimainButton_Gift.SetShow(true);
			uimainButton_Gift.SetData(id);
			this.m_refreshButtons.Add(uimainButton_Gift);
			return true;
		}

		public ScrollRect m_scroll;

		public UIMainChooseButtonCtrl m_chooseCustomButton;

		public RectTransform m_parent;

		public GridLayoutGroup m_gridLayoutGroup;

		[SerializeField]
		private UIMainButton_Gift m_pushPackRecharge;

		[SerializeField]
		private UIMainButton_Sign m_sign;

		[SerializeField]
		private UIMainButton_Carnival m_carnival;

		[SerializeField]
		private UIMainButton_RechargeGift m_rechargeGift;

		[SerializeField]
		private UIMainButton_PushGift m_pushGift;

		[SerializeField]
		private UIMainButton_PrivilegeCard m_privilegeCard;

		[SerializeField]
		private UIMainButton_ChainPacks m_chainPacks;

		private List<BaseUIMainButton> m_residentButtons = new List<BaseUIMainButton>();

		private List<BaseUIMainButton> m_refreshButtons = new List<BaseUIMainButton>();

		private List<BaseUIMainButton> m_allButtons = new List<BaseUIMainButton>();

		private List<string> m_redNodePaths = new List<string>();

		private bool m_isUnfold;
	}
}
