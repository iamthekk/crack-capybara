using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPRechargeGiftViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.tagFund.onClick.AddListener(delegate
			{
				this.ShowPage(IAPRechargeGiftViewModule.EPageType.Fund);
			});
			this.tagCard.onClick.AddListener(delegate
			{
				this.ShowPage(IAPRechargeGiftViewModule.EPageType.Card);
			});
			this._tagDic = new Dictionary<IAPRechargeGiftViewModule.EPageType, CustomChooseButton>();
			this._tagDic.Add(IAPRechargeGiftViewModule.EPageType.Fund, this.tagFund);
			this._tagDic.Add(IAPRechargeGiftViewModule.EPageType.Card, this.tagCard);
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund", new Action<RedNodeListenData>(this.OnRedPointFund));
		}

		public override void OnOpen(object data)
		{
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_tag_fund");
			this.mask.SetActive(false);
			this.ShowPage(IAPRechargeGiftViewModule.EPageType.Fund);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this._pages.Length; i++)
			{
				RechargeGiftPageBase rechargeGiftPageBase = this._pages[i];
				if (!(rechargeGiftPageBase == null))
				{
					rechargeGiftPageBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
			for (int i = 0; i < this._pages.Length; i++)
			{
				RechargeGiftPageBase rechargeGiftPageBase = this._pages[i];
				if (!(rechargeGiftPageBase == null))
				{
					rechargeGiftPageBase.DeInit();
					Object.Destroy(rechargeGiftPageBase.gameObject);
					this._pages[i] = null;
				}
			}
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveAllListeners();
			this.buttonClose.onClick.RemoveAllListeners();
			this.tagFund.onClick.RemoveAllListeners();
			this.tagCard.onClick.RemoveAllListeners();
			this._tagDic = null;
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund", new Action<RedNodeListenData>(this.OnRedPointFund));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIRechargeGift_Refresh, new HandlerEvent(this.OnEventRefreshUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIRechargeGift_Refresh, new HandlerEvent(this.OnEventRefreshUI));
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			for (int i = 0; i < this._pages.Length; i++)
			{
				RechargeGiftPageBase rechargeGiftPageBase = this._pages[i];
				if (!(rechargeGiftPageBase == null))
				{
					rechargeGiftPageBase.Refresh();
				}
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.IAPRechargeGiftViewModule, null);
		}

		private async void ShowPage(IAPRechargeGiftViewModule.EPageType type)
		{
			foreach (KeyValuePair<IAPRechargeGiftViewModule.EPageType, CustomChooseButton> keyValuePair in this._tagDic)
			{
				if (keyValuePair.Key == type)
				{
					keyValuePair.Value.SetSelect(true);
				}
				else
				{
					keyValuePair.Value.SetSelect(false);
				}
			}
			await this.CreatePage(type);
			for (int i = 0; i < this._pages.Length; i++)
			{
				RechargeGiftPageBase rechargeGiftPageBase = this._pages[i];
				if (!(rechargeGiftPageBase == null))
				{
					if (rechargeGiftPageBase.PageType != type)
					{
						rechargeGiftPageBase.gameObject.SetActive(false);
					}
					else
					{
						rechargeGiftPageBase.gameObject.SetActive(true);
						rechargeGiftPageBase.OnShow();
						rechargeGiftPageBase.Refresh();
					}
				}
			}
		}

		private void OnRedPointFund(RedNodeListenData redData)
		{
			if (this.redNodeFund == null)
			{
				return;
			}
			this.redNodeFund.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointCard(RedNodeListenData redData)
		{
			if (this.redNodeCard == null)
			{
				return;
			}
			this.redNodeCard.gameObject.SetActive(redData.m_count > 0);
		}

		private Task CreatePage(IAPRechargeGiftViewModule.EPageType type)
		{
			IAPRechargeGiftViewModule.<CreatePage>d__25 <CreatePage>d__;
			<CreatePage>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreatePage>d__.<>4__this = this;
			<CreatePage>d__.type = type;
			<CreatePage>d__.<>1__state = -1;
			<CreatePage>d__.<>t__builder.Start<IAPRechargeGiftViewModule.<CreatePage>d__25>(ref <CreatePage>d__);
			return <CreatePage>d__.<>t__builder.Task;
		}

		public CustomText textTitle;

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomChooseButton tagFund;

		public CustomChooseButton tagCard;

		public RedNodeOneCtrl redNodeFund;

		public RedNodeOneCtrl redNodeCard;

		public RectTransform pageParent;

		public GameObject mask;

		private static Dictionary<IAPRechargeGiftViewModule.EPageType, string> _pathDic = new Dictionary<IAPRechargeGiftViewModule.EPageType, string>
		{
			{
				IAPRechargeGiftViewModule.EPageType.Fund,
				"Assets/_Resources/Prefab/UI/IAPRechargeGift/Fund.prefab"
			},
			{
				IAPRechargeGiftViewModule.EPageType.Card,
				"Assets/_Resources/Prefab/UI/IAPRechargeGift/Card.prefab"
			}
		};

		private RechargeGiftPageBase[] _pages = new RechargeGiftPageBase[2];

		private Dictionary<IAPRechargeGiftViewModule.EPageType, CustomChooseButton> _tagDic;

		public enum EPageType
		{
			Fund,
			Card,
			Count
		}
	}
}
