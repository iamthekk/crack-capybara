using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Conquer;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ConquerPlayerNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarCtrl.Init();
			this.m_avatarCtrl.SetEnableButton(true);
			this.m_avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatarCtrl);
			this.m_battleBt.onClick.AddListener(new UnityAction(this.OnClickBattleBt));
			this.m_revoltBt.onClick.AddListener(new UnityAction(this.OnClickRevoltBt));
			this.m_lootBt.onClick.AddListener(new UnityAction(this.OnClickLootBt));
			this.m_pardonBt.onClick.AddListener(new UnityAction(this.OnClickPardonBt));
		}

		protected override void OnDeInit()
		{
			this.m_battleBt.onClick.RemoveAllListeners();
			this.m_revoltBt.onClick.RemoveAllListeners();
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

		private void OnClickBattleBt()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			TicketDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			if (dataModule.IsFullSlaveCount())
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6614));
				return;
			}
			if (dataModule2.GetTicketCount(UserTicketKind.UserLife) < Singleton<GameConfig>.Instance.APBattleCost)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6613));
				return;
			}
			EventArgsAddItemTipData instance = Singleton<EventArgsAddItemTipData>.Instance;
			instance.SetDataCount(9, -Singleton<GameConfig>.Instance.APBattleCost, this.m_battleBt.gameObject.transform.position);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, instance);
			NetworkUtils.Conquer.DoConquerBattleRequest(this.m_data.Owner.UserId, delegate(bool isOk, ConquerBattleResponse resp)
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
								GameApp.Event.DispatchNow(null, LocalMessageName.CC_ConquerData_Battle, instance2);
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
			NetworkUtils.Conquer.DoConquerRevoltRequest(this.m_data.Owner.UserId, delegate(bool isOk, ConquerRevoltResponse resp)
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
			NetworkUtils.Conquer.DoConquerLootRequest(this.m_data.Owner.UserId, delegate(bool isOk, ConquerLootResponse resp)
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
			string nickName = (string.IsNullOrEmpty(this.m_data.Owner.NickName) ? DxxTools.GetDefaultNick(this.m_data.Owner.UserId) : this.m_data.Owner.NickName);
			NetworkUtils.Conquer.DoConquerPardonRequest(this.m_data.Owner.UserId, delegate(bool isOk, ConquerPardonResponse resp)
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

		public void RefreshUI(ConquerListResponse dto, long userID, bool isUser)
		{
			this.m_data = dto;
			this.m_userID = userID;
			this.m_isUser = isUser;
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.RefreshData(this.m_data.Owner.Avatar, this.m_data.Owner.AvatarFrame);
			}
			if (this.m_nickTxt != null)
			{
				this.m_nickTxt.text = (string.IsNullOrEmpty(dto.Owner.NickName) ? DxxTools.GetDefaultNick(dto.Owner.UserId) : dto.Owner.NickName);
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber((long)this.m_data.Owner.Power) });
			}
			bool flag = false;
			int num = 0;
			if (this.m_data.Slaves != null)
			{
				for (int i = 0; i < this.m_data.Slaves.Count; i++)
				{
					ConquerUserDto conquerUserDto = this.m_data.Slaves[i];
					if (conquerUserDto != null)
					{
						num++;
						if (conquerUserDto.UserId == userID)
						{
							flag = true;
						}
					}
				}
			}
			if (this.m_slaveTxt != null)
			{
				this.m_slaveTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(9302, new object[]
				{
					num,
					Singleton<GameConfig>.Instance.SlaveMaxCount
				});
			}
			if (this.m_battleBt != null)
			{
				this.m_battleBt.gameObject.SetActive(false);
			}
			if (this.m_revoltBt != null)
			{
				this.m_revoltBt.gameObject.SetActive(false);
			}
			if (this.m_lootBt != null)
			{
				this.m_lootBt.gameObject.SetActive(false);
			}
			if (this.m_pardonBt != null)
			{
				this.m_pardonBt.gameObject.SetActive(false);
			}
			if (!this.m_isUser)
			{
				if (this.m_data.Lord == null || this.m_data.Lord.UserId <= 0L)
				{
					if (flag)
					{
						if (this.m_revoltBt != null)
						{
							this.m_revoltBt.gameObject.SetActive(true);
							return;
						}
					}
					else if (this.m_battleBt != null)
					{
						this.m_battleBt.gameObject.SetActive(true);
						return;
					}
				}
				else if (this.m_data.Lord.UserId == this.m_userID)
				{
					if (this.m_pardonBt != null)
					{
						this.m_pardonBt.gameObject.SetActive(true);
						return;
					}
				}
				else if (flag)
				{
					if (this.m_revoltBt != null)
					{
						this.m_revoltBt.gameObject.SetActive(true);
						return;
					}
				}
				else if (this.m_lootBt != null)
				{
					this.m_lootBt.gameObject.SetActive(true);
				}
			}
		}

		public UIAvatarCtrl m_avatarCtrl;

		public CustomText m_nickTxt;

		public CustomText m_powerTxt;

		public CustomText m_slaveTxt;

		public CustomButton m_battleBt;

		public CustomButton m_revoltBt;

		public CustomButton m_lootBt;

		public CustomButton m_pardonBt;

		private ConquerListResponse m_data;

		private bool m_isUser;

		private long m_userID;
	}
}
