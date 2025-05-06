using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using Proto.CrossArena;
using UnityEngine;

namespace HotFix.CrossArenaChallengeUI
{
	public class CrossArenaChallengeOppItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Avatar.Init();
			this.Avatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickShowUser);
			this.Button_Challenge.Init();
			this.Button_Challenge.SetItemIcon(10);
			this.Button_Challenge.SetCountText("x1", false);
			this.Button_Challenge.SetInfoText(Singleton<LanguageManager>.Instance.GetInfoByID("15004"));
			this.Button_Challenge.SetOnClick(new Action(this.OnChallengeMember));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.Button_Challenge.DeInit();
			UIAvatarCtrl avatar = this.Avatar;
			if (avatar == null)
			{
				return;
			}
			avatar.DeInit();
		}

		internal void SetData(CrossArenaRankMember data)
		{
			this.mData = data;
		}

		internal void RefreshUI()
		{
			if (this.mData == null)
			{
				this.RefreshAsNull();
				return;
			}
			this.Text_Name.text = this.mData.GetNick();
			this.Text_Power.text = Singleton<LanguageManager>.Instance.GetInfoByID("15031", new object[] { DxxTools.FormatNumber(this.mData.Power) });
			this.Text_Score.text = this.mData.Score.ToString();
			this.Avatar.RefreshData(this.mData.Avatar, this.mData.AvatarFrame);
		}

		private void RefreshAsNull()
		{
		}

		private void OnChallengeMember()
		{
			if (this.mData == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			if (GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.CrossArena) < 1)
			{
				CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
				openData.SetData(UserTicketKind.CrossArena);
				GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
				return;
			}
			this.Button_Challenge.SetButtonEnable(false);
			CrossArenaRankMember enemy = this.mData;
			GameApp.SDK.Analyze.Track_ArenaBattleStart(enemy);
			NetworkUtils.CrossArena.DoCrossArenaChallengeRequest(this.mData.UserID, delegate(bool result, CrossArenaChallengeResponse resp)
			{
				if (result)
				{
					GameApp.SDK.Analyze.Track_ArenaBattleEnd(enemy, resp.Record.Result, (int)resp.OwnerAfterRank, (int)resp.OwnerAfterScore);
					this.JumpToBattle(enemy, resp);
					return;
				}
				this.Button_Challenge.SetButtonEnable(true);
			});
		}

		public void JumpToBattle(CrossArenaRankMember enemy, CrossArenaChallengeResponse reportresponse)
		{
			EventArgsBattleCrossArenaEnter instance = Singleton<EventArgsBattleCrossArenaEnter>.Instance;
			instance.SetData(reportresponse.Record, false);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleCrossArena_BattleCrossArenaEnter, instance);
			PVPRecordDto record = reportresponse.Record;
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance2 = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance2.SetData(DxxTools.UI.GetCrossArenaChallengeOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance2);
					GameApp.View.CloseView(ViewName.CrossArenaChallengeViewModule, null);
					GameApp.View.CloseView(ViewName.CrossArenaViewModule, null);
					EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
					instance3.SetData(GameModel.CrossArena, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance3);
					GameApp.State.ActiveState(StateName.BattleCrossArenaState);
				});
			});
		}

		private void OnClickShowUser(UIAvatarCtrl ctrl)
		{
			if (this.mData == null)
			{
				return;
			}
			Action<long> onShowPlayerInfo = this.OnShowPlayerInfo;
			if (onShowPlayerInfo == null)
			{
				return;
			}
			onShowPlayerInfo(this.mData.UserID);
		}

		public UIAvatarCtrl Avatar;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public CustomText Text_Score;

		public UIItemInfoButton Button_Challenge;

		private CrossArenaRankMember mData;

		public Action<long> OnShowPlayerInfo;
	}
}
