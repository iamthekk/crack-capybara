using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using HotFix.GuildUI.GuildDetailInfoUI;
using Proto.Guild;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildDetailInfoViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Info.OnNameChanged = new Action(this.OnNameChanged);
			this.Info.Init();
			this.Slogan.OnSloganChanged = new Action(this.OnSloganChanged);
			this.Slogan.Init();
			this.Others.Init();
			this.BindCreateData();
			this.BtnCostSave.onClick.AddListener(new UnityAction(this.OnSaveGuildInfo));
			this.BtnSave.onClick.AddListener(new UnityAction(this.OnSaveGuildInfo));
		}

		protected override void OnViewOpen(object data)
		{
			this.Info.Show();
			this.Slogan.Show();
			this.Others.Show();
			this.RefreshUI();
			this.RefreshSaveButtonState();
		}

		protected override void OnViewClose()
		{
			if (this.Info != null)
			{
				this.Info.Close();
			}
			if (this.Slogan != null)
			{
				this.Slogan.Close();
			}
			if (this.Others != null)
			{
				this.Others.Close();
			}
		}

		protected override void OnViewDelete()
		{
			if (this.BtnCostSave != null)
			{
				this.BtnCostSave.onClick.RemoveListener(new UnityAction(this.OnSaveGuildInfo));
			}
			if (this.BtnSave != null)
			{
				this.BtnSave.onClick.RemoveListener(new UnityAction(this.OnSaveGuildInfo));
			}
			if (this.Info != null)
			{
				this.Info.DeInit();
			}
			if (this.Slogan != null)
			{
				this.Slogan.DeInit();
			}
			if (this.Others != null)
			{
				this.Others.DeInit();
			}
		}

		private void RefreshSaveButtonState()
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			int typeInt = GuildProxy.Table.GetGuildConstTable(102).TypeInt;
			if (!string.IsNullOrEmpty(this.mCreateData.GuildShowName) && this.mCreateData.GuildShowName != guildData.GuildShowName && typeInt > 0)
			{
				this.BtnCostSave.SetValue(typeInt);
				this.BtnCostSave.gameObject.SetActive(true);
				this.BtnSave.gameObject.SetActive(false);
				return;
			}
			this.BtnCostSave.gameObject.SetActive(false);
			this.BtnSave.gameObject.SetActive(true);
		}

		public void OnNameEditEnd(string text)
		{
			HLog.LogError("OnNameEditEnd");
			this.mCreateData.GuildShowName = text;
			this.RefreshSaveButtonState();
		}

		public void OnSloganEditEnd(string text)
		{
			this.mCreateData.GuildSlogan = text;
		}

		public void OnConditionEditEnd(string text)
		{
			int num = 0;
			if (int.TryParse(text, out num))
			{
				this.mCreateData.JoinCondition_Level = num;
			}
		}

		public void OnNameChanged()
		{
			this.RefreshSaveButtonState();
		}

		public void OnSloganChanged()
		{
			this.RefreshSaveButtonState();
		}

		private void OnSaveGuildInfo()
		{
			if (this.CheckCreateEnabled())
			{
				if (this.mCreateData.IsSameWithShareData(base.SDK.GuildInfo.GuildData))
				{
					this.CloseSelfView();
					return;
				}
				GuildNetUtil.Guild.DoRequest_ModifyGuildInfo(this.mCreateData, delegate(bool result, GuildModifyResponse resp)
				{
					if (result)
					{
						this.CloseSelfView();
					}
				});
			}
		}

		private void BindCreateData()
		{
			if (this.Info != null)
			{
				this.Info.CreateData = this.mCreateData;
			}
			if (this.Slogan != null)
			{
				this.Slogan.CreateData = this.mCreateData;
			}
			if (this.Others != null)
			{
				this.Others.CreateData = this.mCreateData;
			}
		}

		private bool CheckCreateEnabled()
		{
			if (string.IsNullOrWhiteSpace(this.mCreateData.GuildShowName))
			{
				string infoByID = GuildProxy.Language.GetInfoByID("400119");
				string infoByID2 = GuildProxy.Language.GetInfoByID1("400220", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
				return false;
			}
			if (GuildProxy.Language.CheckTextLength(this.mCreateData.GuildShowName) > GuildProxy.Table.NAME_LENGTH_MAX)
			{
				string infoByID3 = GuildProxy.Language.GetInfoByID("400119");
				string infoByID4 = GuildProxy.Language.GetInfoByID1("400064", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID3, infoByID4);
				return false;
			}
			if (GuildProxy.Language.CheckTextLength(this.mCreateData.GuildSlogan) > GuildProxy.Table.SLOGAN_LENGTH)
			{
				string infoByID5 = GuildProxy.Language.GetInfoByID("400119");
				string infoByID6 = GuildProxy.Language.GetInfoByID1("400065", GuildProxy.Table.SLOGAN_LENGTH);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID5, infoByID6);
				return false;
			}
			return true;
		}

		private void RefreshUI()
		{
			if (!base.SDK.GuildInfo.HasGuild)
			{
				HLog.LogError("错误：当前没有公会");
				this.CloseSelfView();
				return;
			}
			GuildShareDetailData guildDetailData = base.SDK.GuildInfo.GuildDetailData;
			GuildShareData shareData = guildDetailData.ShareData;
			this.mCreateData.CloneFromShareData(shareData);
			this.Info.RefreshUI(shareData, guildDetailData);
			this.Slogan.RefreshUI(shareData, guildDetailData);
			this.Others.RefreshUI(shareData, guildDetailData);
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildDetailInfoViewModule, null);
		}

		private void OnPopClick(int obj)
		{
			this.CloseSelfView();
		}

		public UIGuildPopCommon popCommon;

		public GuildDetailInfo_Info Info;

		public GuildDetailInfo_Slogan Slogan;

		public GuildDetailInfo_Others Others;

		public ButtonCurrencyCtrl BtnCostSave;

		public CustomButton BtnSave;

		private GuildCreateData mCreateData = new GuildCreateData();
	}
}
