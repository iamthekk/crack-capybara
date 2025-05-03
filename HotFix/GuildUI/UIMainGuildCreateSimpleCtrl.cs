using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIMainGuildCreateSimpleCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.InputGuildName.characterLimit = GuildProxy.Table.NAME_LENGTH_MAX;
			this.InputGuildName.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
			this.InputGuildName.onEndEdit.AddListener(new UnityAction<string>(this.OnEditGuildNameEnd));
			this.ButtonCancel.onClick.AddListener(new UnityAction(this.OnClickCancel));
			this.ButtonCreate.onClick.AddListener(new UnityAction(this.OnClickCreate));
			this.InputGuildName.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(101);
			this.Text_Price.text = guildConstTable.TypeInt.ToString();
		}

		protected override void GuildUI_OnUnInit()
		{
			this.InputGuildName.onValidateInput = null;
			this.InputGuildName.onEndEdit.RemoveListener(new UnityAction<string>(this.OnEditGuildNameEnd));
			this.ButtonCancel.onClick.RemoveListener(new UnityAction(this.OnClickCancel));
			this.ButtonCreate.onClick.RemoveListener(new UnityAction(this.OnClickCreate));
			this.InputGuildName.onValueChanged.RemoveListener(new UnityAction<string>(this.OnValueChanged));
		}

		private void CalcNameLength()
		{
			string text = this.InputGuildName.text;
			int num = GuildProxy.Language.CheckTextNameLength(text);
			if (num > GuildProxy.Table.NAME_LENGTH_MAX)
			{
				text = text.Substring(0, text.Length - 1);
				num = GuildProxy.Table.NAME_LENGTH_MAX;
				this.InputGuildName.text = text;
			}
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", GuildProxy.Table.NAME_LENGTH_MAX - num);
			this.Text_NameLength.text = infoByID;
			if (GuildProxy.Table.NAME_LENGTH_MAX - num <= 0)
			{
				this.Text_NameLength.color = Color.red;
				return;
			}
			this.Text_NameLength.color = Color.gray;
		}

		private void OnValueChanged(string text)
		{
			this.CalcNameLength();
		}

		protected override void GuildUI_OnShow()
		{
			if (this.InputGuildName.placeholder != null)
			{
				Text component = this.InputGuildName.placeholder.GetComponent<Text>();
				if (component != null)
				{
					component.text = GuildProxy.Language.GetInfoByID("4063");
				}
				this.CalcNameLength();
			}
		}

		protected override void GuildUI_OnClose()
		{
		}

		private char OnValidateInput(string text, int charIndex, char addedChar)
		{
			if (!DxxTools.Char.CheckLength(text, addedChar, Singleton<GameConfig>.Instance.NickName_MaxLength))
			{
				return '\0';
			}
			if (DxxTools.Char.CheckEmoji(addedChar))
			{
				return '\0';
			}
			return addedChar;
		}

		private void OnEditGuildNameEnd(string str)
		{
			this._inputGuildName = str;
		}

		private void OnClickCancel()
		{
			base.Close();
		}

		private void OnClickCreate()
		{
			if (!GuildSDKManager.Instance.CheckQuitGuildTime())
			{
				return;
			}
			if (this.CheckCreateEnabled())
			{
				Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(101);
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(2UL) < (long)guildConstTable.TypeInt)
				{
					GameApp.View.ShowItemNotEnoughTip(2, false);
					return;
				}
				GuildCreateData guildCreateData = new GuildCreateData();
				guildCreateData.GuildShowName = this._inputGuildName;
				guildCreateData.GuildSlogan = "";
				guildCreateData.GuildNotice = "";
				GuildNetUtil.Guild.DoRequest_CreateGuild(guildCreateData, delegate(bool result, GuildCreateResponse response)
				{
					if (result)
					{
						GuildProxy.UI.CloseMainGuildInfo();
						GuildSDKManager.Instance.OpenGuild();
					}
				});
			}
		}

		private bool CheckCreateEnabled()
		{
			string infoByID = GuildProxy.Language.GetInfoByID("400119");
			if (string.IsNullOrWhiteSpace(this._inputGuildName))
			{
				string infoByID2 = GuildProxy.Language.GetInfoByID1("400220", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
				return false;
			}
			if (GuildProxy.Language.CheckTextLength(this._inputGuildName) > GuildProxy.Table.NAME_LENGTH_MAX)
			{
				string infoByID3 = GuildProxy.Language.GetInfoByID1("400064", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID3);
				return false;
			}
			return true;
		}

		public CustomButton ButtonCancel;

		public CustomButton ButtonCreate;

		public CustomImage Image_Currency;

		public CustomText Text_Price;

		public InputField InputGuildName;

		public CustomText Text_NameLength;

		private string _inputGuildName;
	}
}
