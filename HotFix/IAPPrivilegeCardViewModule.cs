using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Pay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class IAPPrivilegeCardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonGetAll.onClick.AddListener(new UnityAction(this.OnClickGetAll));
			this.freeNode.Init();
			this.rewardNode.Init();
			for (int i = 0; i < this.cardNodes.Count; i++)
			{
				this.cardNodes[i].Init();
			}
			List<int> list = new List<int>();
			list.Add(9);
			list.Add(2);
			list.Add(1);
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.DailyActivities, list);
		}

		public override void OnOpen(object data)
		{
			this.Refresh();
			base.StartCoroutine(this.WaitForLayoutUpdate());
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.freeNode.OnUpdate(deltaTime, unscaledDeltaTime);
			this.rewardNode.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.cardNodes.Count; i++)
			{
				this.cardNodes[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
		}

		public override void OnDelete()
		{
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonGetAll.onClick.RemoveListener(new UnityAction(this.OnClickGetAll));
			this.currencyCtrl.DeInit();
			this.freeNode.DeInit();
			this.rewardNode.DeInit();
			for (int i = 0; i < this.cardNodes.Count; i++)
			{
				this.cardNodes[i].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Refresh_PrivilegeCard, new HandlerEvent(this.OnEventRefreshUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Refresh_PrivilegeCard, new HandlerEvent(this.OnEventRefreshUI));
		}

		private void Refresh()
		{
			this.isReward = false;
			List<IAPMonthCardData.CardType> list = DxxTools.EnumToList<IAPMonthCardData.CardType>();
			for (int i = 0; i < list.Count; i++)
			{
				this.isReward = this.iapDataModule.MonthCard.IsCanReward(list[i]);
				if (this.isReward)
				{
					break;
				}
			}
			if (this.isReward)
			{
				this.btnGray.Recovery();
			}
			else
			{
				this.btnGray.SetUIGray();
			}
			this.freeNode.Refresh();
			this.rewardNode.Refresh();
			for (int j = 0; j < this.cardNodes.Count; j++)
			{
				this.cardNodes[j].Refresh();
			}
		}

		private IEnumerator WaitForLayoutUpdate()
		{
			yield return new WaitForEndOfFrame();
			this.PlayShowAni();
			yield break;
		}

		private void PlayShowAni()
		{
			this.nodeList.Clear();
			this.nodeList.Add(this.rewardNode);
			this.nodeList.AddRange(this.cardNodes);
			if (this.iapDataModule.MonthCard.IsCanReward(IAPMonthCardData.CardType.Free))
			{
				this.nodeList.Insert(0, this.freeNode);
			}
			else
			{
				this.nodeList.Add(this.freeNode);
			}
			float yPos = 55f;
			this.nodeList.ForEach(delegate(UIPrivilegeCardNodeCtrlBase item)
			{
				item.SetAnchorPosY(-yPos);
				yPos += item.SizeY + 20f;
			});
			RectTransform content = this.scroll.content;
			Vector2 sizeDelta = content.sizeDelta;
			sizeDelta.y = yPos;
			content.sizeDelta = sizeDelta;
			this.m_seqPool.Clear(false);
			int num = 0;
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				this.nodeList[i].PlayShowAni(this.m_seqPool, num);
				num++;
			}
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.IAPPrivilegeCardViewModule, null);
		}

		private void OnClickGetAll()
		{
			if (this.isReward)
			{
				NetworkUtils.Purchase.SendMonthCardGetRewardRequest(0, delegate(bool isOk, MonthCardGetRewardResponse resp)
				{
					if (isOk)
					{
						this.Refresh();
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					}
				});
			}
		}

		public ScrollRect scroll;

		public CustomButton buttonBack;

		public CustomButton buttonGetAll;

		public UIGrays btnGray;

		public ModuleCurrencyCtrl currencyCtrl;

		public UIPrivilegeCardFreeNodeCtrl freeNode;

		public UIPrivilegeCardActiveRewardNodeCtrl rewardNode;

		public List<UIPrivilegeCardNodeCtrl> cardNodes;

		private const float SPACE_Y = 20f;

		private List<UIPrivilegeCardNodeCtrlBase> nodeList = new List<UIPrivilegeCardNodeCtrlBase>();

		private IAPDataModule iapDataModule;

		private SequencePool m_seqPool = new SequencePool();

		private bool isReward;
	}
}
