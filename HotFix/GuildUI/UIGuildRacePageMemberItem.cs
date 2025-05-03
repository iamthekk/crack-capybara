using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageMemberItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public GuildRaceMember Data
		{
			get
			{
				return this.mData;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.Avatar.Init();
			this.Button_Change.onClick.AddListener(new UnityAction(this.OnClickOpenChangeIndex));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button_Change = this.Button_Change;
			if (button_Change != null)
			{
				button_Change.onClick.RemoveListener(new UnityAction(this.OnClickOpenChangeIndex));
			}
			UIGuildHead avatar = this.Avatar;
			if (avatar == null)
			{
				return;
			}
			avatar.DeInit();
		}

		public void SetData(GuildRaceMember data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData.IsEmptyUser)
			{
				this.RefreshUIAsEmptyPosition();
				return;
			}
			this.RefreshUIAsUserPosition();
		}

		private void RefreshUIAsUserPosition()
		{
			this.RefreshBaseInfo();
			this.Obj_EmptyNick.SetActive(false);
			this.Text_Name.text = this.mData.UserData.GetNick();
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.mData.Power));
			int num = -1;
			if (this.mData.Position != GuildRaceBattlePosition.None)
			{
				GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(this.mData.Position);
				if (raceBaseTable != null)
				{
					num = raceBaseTable.TypeIntArray[0];
				}
			}
			string text = GuildProxy.Language.GetInfoByID1("400417", this.mData.ActivityPoint);
			if (num > 0)
			{
				text = text + "<color=#F5835CFF>" + GuildProxy.Language.GetInfoByID1("400418", num) + "</color>";
			}
			this.Text_AP.text = text;
			this.Obj_EmptyHead.SetActive(false);
			this.Avatar.SetActive(true);
			this.Avatar.Refresh(this.mData.UserData.Avatar, this.mData.UserData.AvatarFrame);
			this.Avatar.SetDefaultClick(this.mData.UserData.UserID);
		}

		private void RefreshUIAsEmptyPosition()
		{
			this.RefreshBaseInfo();
			this.Obj_EmptyNick.SetActive(true);
			this.Text_Name.text = "";
			this.Text_Power.text = "";
			int num = -1;
			if (this.mData.Position != GuildRaceBattlePosition.None)
			{
				GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(this.mData.Position);
				if (raceBaseTable != null)
				{
					num = raceBaseTable.TypeIntArray[0];
				}
			}
			string text = GuildProxy.Language.GetInfoByID1("400417", "");
			if (num > 0)
			{
				text = text + "<color=#F5835CFF>" + GuildProxy.Language.GetInfoByID1("400418", num) + "</color>";
			}
			this.Text_AP.text = text;
			this.Obj_EmptyHead.SetActive(true);
			this.Avatar.SetActive(false);
		}

		private void RefreshBaseInfo()
		{
			if (this.mData.Index <= 0)
			{
				this.Text_Index.text = GuildProxy.Language.GetInfoByID_LogError(400495);
			}
			else
			{
				this.Text_Index.text = GuildProxy.Language.GetInfoByID1_LogError(400430, this.mData.Index);
			}
			this.Obj_Flag.SetActive(this.IsInBattle());
			this.Text_Score.text = GuildProxy.Language.GetInfoByID("400415") + "\r\n+" + this.GetAddScore();
			if (this.IsHasPermission() && this.IsShowChangeButton())
			{
				this.Button_Change.gameObject.SetActive(true);
				this.Text_Score.rectTransform.anchoredPosition = new Vector2(190f, 0f);
			}
			else
			{
				this.Button_Change.gameObject.SetActive(false);
				this.Text_Score.rectTransform.anchoredPosition = new Vector2(315f, 0f);
			}
			if (this.mData.IsSuperPosition)
			{
				this.Obj_Super.SetActive(true);
				this.Text_Super.text = this.GetSpecailPosName();
				return;
			}
			this.Obj_Super.SetActive(false);
		}

		public string GetSpecailPosName()
		{
			if (this.mData == null)
			{
				return "";
			}
			return GuildProxy.Language.GetInfoByID_LogError(400490 + this.mData.Position - GuildRaceBattlePosition.Warrior);
		}

		private string GetAddScore()
		{
			if (this.mData.Position <= GuildRaceBattlePosition.None)
			{
				return "0";
			}
			GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(this.mData.Position);
			if (raceBaseTable != null)
			{
				return raceBaseTable.TypeIntArray[1].ToString();
			}
			return "0";
		}

		public void RefreshMineState()
		{
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance != null && instance.IsMeJoinRace())
			{
				return;
			}
			this.Text_Index.text = "??";
			this.Text_AP.text = GuildProxy.Language.GetInfoByID1("400417", "?");
			this.Text_Score.text = GuildProxy.Language.GetInfoByID_LogError(400450);
			this.Text_Score.rectTransform.anchoredPosition = new Vector2(330f, 0f);
		}

		private void OnClickOpenChangeIndex()
		{
			Action<UIGuildRacePageMemberItem> onClickThis = this.OnClickThis;
			if (onClickThis == null)
			{
				return;
			}
			onClickThis(this);
		}

		private bool IsInBattle()
		{
			return this.mData != null && this.mData.Position > GuildRaceBattlePosition.None;
		}

		public bool IsHasPermission()
		{
			return base.SDK.Permission.HasPermission(GuildPermissionKind.GuildActivities, null);
		}

		private bool IsShowChangeButton()
		{
			return GuildRaceBattleController.Instance != null && GuildRaceBattleController.Instance.CurrentRaceKind == GuildRaceStageKind.UserApply && this.IsInBattle();
		}

		public CustomText Text_Index;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public GameObject Obj_EmptyNick;

		public CustomText Text_AP;

		public CustomText Text_Score;

		public CustomText Text_Super;

		public GameObject Obj_Super;

		public CustomButton Button_Change;

		public GameObject Obj_Flag;

		public UIGuildHead Avatar;

		public GameObject Obj_EmptyHead;

		private GuildRaceMember mData;

		public Action<UIGuildRacePageMemberItem> OnClickThis;
	}
}
