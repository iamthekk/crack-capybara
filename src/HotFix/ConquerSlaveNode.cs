using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Conquer;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ConquerSlaveNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatarCtrl);
			this.m_avatarCtrl.Init();
			this.m_lootBt.onClick.AddListener(new UnityAction(this.OnClickLootBt));
			this.m_pardonBt.onClick.AddListener(new UnityAction(this.OnClickPardonBt));
		}

		protected override void OnDeInit()
		{
			this.m_lootBt.onClick.RemoveAllListeners();
			this.m_pardonBt.onClick.RemoveAllListeners();
			this.m_avatarCtrl.DeInit();
			this.m_data = null;
		}

		private void OnClickAvatarCtrl(UIAvatarCtrl obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_data.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnClickLootBt()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			TicketDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			if (dataModule.IsFullSlaveCount())
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6614));
				return;
			}
			if (dataModule2.GetTicketCount(UserTicketKind.UserLife) < Singleton<GameConfig>.Instance.APLootCost)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6613));
				return;
			}
			EventArgsAddItemTipData instance = Singleton<EventArgsAddItemTipData>.Instance;
			instance.SetDataCount(9, -Singleton<GameConfig>.Instance.APBattleCost, this.m_lootBt.gameObject.transform.position);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, instance);
			NetworkUtils.Conquer.DoConquerLootRequest(this.m_data.UserId, delegate(bool isOk, ConquerLootResponse resp)
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
								GameApp.Event.DispatchNow(null, LocalMessageName.CC_ConquerData_Loot, instance2);
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

		private void OnClickPardonBt()
		{
			string nickName = (string.IsNullOrEmpty(this.m_data.NickName) ? DxxTools.GetDefaultNick(this.m_data.UserId) : this.m_data.NickName);
			NetworkUtils.Conquer.DoConquerPardonRequest(this.m_data.UserId, delegate(bool isOk, ConquerPardonResponse resp)
			{
				if (!isOk)
				{
					return;
				}
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6615, new object[] { nickName }));
				EventArgsConquerTypeData instance = Singleton<EventArgsConquerTypeData>.Instance;
				instance.SetData(resp.UserId);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ConquerData_Pardon, instance);
			});
		}

		public void RefreshUI(ConquerUserDto dto, long userID, bool isUser)
		{
			this.m_data = dto;
			this.m_userID = userID;
			this.m_isUser = isUser;
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.RefreshData(this.m_data.Avatar, this.m_data.AvatarFrame);
				this.m_avatarCtrl.SetEnableButton(true);
			}
			if (this.m_nickTxt != null)
			{
				string text = (string.IsNullOrEmpty(dto.NickName) ? DxxTools.GetDefaultNick(dto.UserId) : dto.NickName);
				if (text.Length >= 7)
				{
					text = text.Substring(0, 7) + "...";
				}
				this.m_nickTxt.text = text;
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber((long)this.m_data.Power) });
			}
			if (this.m_coinTxt != null)
			{
				this.m_coinTxt.text = DxxTools.FormatNumber((long)((ulong)this.m_data.Coin));
			}
			if (dto.UserId == userID)
			{
				if (this.m_lootBt != null)
				{
					this.m_lootBt.gameObject.SetActive(false);
				}
				if (this.m_pardonBt != null)
				{
					this.m_pardonBt.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				if (this.m_lootBt != null)
				{
					this.m_lootBt.gameObject.SetActive(!this.m_isUser);
				}
				if (this.m_pardonBt != null)
				{
					this.m_pardonBt.gameObject.SetActive(this.m_isUser);
				}
			}
		}

		public RectTransform m_root;

		public UIAvatarCtrl m_avatarCtrl;

		public CustomText m_nickTxt;

		public CustomText m_powerTxt;

		public CustomText m_coinTxt;

		public CustomButton m_lootBt;

		public CustomButton m_pardonBt;

		public ConquerUserDto m_data;

		private bool m_isUser;

		private long m_userID;

		private const int NickNameMaxLength = 7;
	}
}
