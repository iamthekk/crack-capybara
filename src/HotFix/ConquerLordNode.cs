using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Conquer;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ConquerLordNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatarCtrl);
			this.m_avatarCtrl.Init();
			this.m_revoltBt.onClick.AddListener(new UnityAction(this.OnClickRevoltBt));
		}

		protected override void OnDeInit()
		{
			this.m_avatarCtrl.DeInit();
			this.m_revoltBt.onClick.RemoveAllListeners();
			this.m_data = null;
		}

		private void OnClickAvatarCtrl(UIAvatarCtrl obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_data.Lord.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnClickRevoltBt()
		{
			if (GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.UserLife) < Singleton<GameConfig>.Instance.APRevoltCost)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6613));
				return;
			}
			EventArgsAddItemTipData instance = Singleton<EventArgsAddItemTipData>.Instance;
			instance.SetDataCount(9, -Singleton<GameConfig>.Instance.APBattleCost, this.m_revoltBt.gameObject.transform.position);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, instance);
			NetworkUtils.Conquer.DoConquerRevoltRequest(this.m_data.Lord.UserId, delegate(bool isOk, ConquerRevoltResponse resp)
			{
				if (!isOk)
				{
					return;
				}
				Action <>9__2;
				GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
				{
					LoadingViewModule viewModule = GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule);
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							if (resp.Record.Result != 0)
							{
								EventArgsConquerTypeData instance2 = Singleton<EventArgsConquerTypeData>.Instance;
								instance2.SetData(resp.UserId);
								GameApp.Event.DispatchNow(null, LocalMessageName.CC_ConquerData_Revolt, instance2);
							}
							EventArgsRefreshMainOpenData instance3 = Singleton<EventArgsRefreshMainOpenData>.Instance;
							instance3.SetData(DxxTools.UI.GetConquerForMainOpenData());
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance3);
							GameApp.View.CloseView(ViewName.ConquerViewModule, null);
							EventArgsGameDataEnter instance4 = Singleton<EventArgsGameDataEnter>.Instance;
							instance4.SetData(GameModel.Conquer, null);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance4);
							GameApp.State.ActiveState(StateName.BattleConquerState);
						});
					}
					viewModule.PlayShow(action);
				});
			});
		}

		public void RefreshUI(ConquerListResponse dto, long userID, bool isUser)
		{
			this.m_data = dto;
			this.m_userID = userID;
			this.m_isUser = isUser;
			if (dto.Lord == null)
			{
				if (this.m_have != null)
				{
					this.m_have.SetActive(false);
				}
				if (this.m_unHave != null)
				{
					this.m_unHave.SetActive(true);
				}
				return;
			}
			if (this.m_have != null)
			{
				this.m_have.SetActive(true);
			}
			if (this.m_unHave != null)
			{
				this.m_unHave.SetActive(false);
			}
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.RefreshData(this.m_data.Lord.Avatar, this.m_data.Lord.AvatarFrame);
				this.m_avatarCtrl.SetEnableButton(true);
			}
			if (this.m_nickTxt != null)
			{
				string text = (string.IsNullOrEmpty(dto.Lord.NickName) ? DxxTools.GetDefaultNick(dto.Lord.UserId) : dto.Lord.NickName);
				if (text.Length >= 7)
				{
					text = text.Substring(0, 7) + "...";
				}
				this.m_nickTxt.text = text;
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber((long)this.m_data.Lord.Power) });
			}
			if (this.m_coinTxt != null)
			{
				this.m_coinTxt.text = DxxTools.FormatNumber((long)((ulong)this.m_data.Owner.Coin));
			}
			if (this.m_revoltBt != null)
			{
				this.m_revoltBt.gameObject.SetActive(this.m_isUser);
			}
		}

		public GameObject m_have;

		public UIAvatarCtrl m_avatarCtrl;

		public CustomText m_nickTxt;

		public CustomText m_powerTxt;

		public CustomText m_coinTxt;

		public CustomButton m_revoltBt;

		public GameObject m_unHave;

		private ConquerListResponse m_data;

		private bool m_isUser;

		private long m_userID;

		private const int NickNameMaxLength = 7;
	}
}
