using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI.GuildCheckPopUI
{
	public class GuildCheckPop_JoinState : GuildCheckPop_Base
	{
		public bool ShowJoinState { get; private set; }

		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.Text_JoinState.text = "";
			this.Button_Join.onClick.AddListener(new UnityAction(this.OnJoinClick));
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			if (this.Button_Join != null)
			{
				this.Button_Join.onClick.RemoveListener(new UnityAction(this.OnJoinClick));
			}
		}

		public override void RefreshUI(GuildShareData sharedata)
		{
			this.mGuildData = sharedata;
			this.RefreshState();
			Action onRefreshJoinState = this.OnRefreshJoinState;
			if (onRefreshJoinState == null)
			{
				return;
			}
			onRefreshJoinState();
		}

		private void RefreshState()
		{
			if (base.SDK.GuildInfo.HasGuild)
			{
				this.Text_JoinState.text = "";
				this.Button_Join.gameObject.SetActive(false);
				this.ShowJoinState = false;
				return;
			}
			this.ShowJoinState = true;
			if (this.mGuildData.IsApply)
			{
				this.Text_JoinState.text = GuildProxy.Language.GetInfoByID("400031");
				this.Button_Join.gameObject.SetActiveSafe(false);
				return;
			}
			if (this.mGuildData.GuildMemberCount >= this.mGuildData.GuildMemberMaxCount)
			{
				this.Text_JoinState.text = GuildProxy.Language.GetInfoByID("400163");
				this.Button_Join.gameObject.SetActiveSafe(false);
				return;
			}
			if ((long)GuildProxy.GameUser.MyLevel() < this.mGuildData.LevelNeed)
			{
				this.Text_JoinState.text = GuildProxy.Language.GetInfoByID("400224");
				this.Button_Join.gameObject.SetActiveSafe(false);
				return;
			}
			this.Text_JoinState.text = "";
			this.Button_Join.gameObject.SetActiveSafe(true);
		}

		public void SwitchToAlreadyApply()
		{
			this.Text_JoinState.text = GuildProxy.Language.GetInfoByID("400031");
			this.Button_Join.gameObject.SetActiveSafe(false);
		}

		private void OnJoinClick()
		{
			Action onApplyJoin = this.OnApplyJoin;
			if (onApplyJoin == null)
			{
				return;
			}
			onApplyJoin();
		}

		[SerializeField]
		private CustomText Text_JoinState;

		[SerializeField]
		private CustomButton Button_Join;

		private GuildShareData mGuildData;

		public Action OnApplyJoin;

		public Action OnRefreshJoinState;
	}
}
