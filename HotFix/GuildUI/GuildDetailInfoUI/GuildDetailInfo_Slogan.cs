using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI.GuildDetailInfoUI
{
	public class GuildDetailInfo_Slogan : GuildDetailInfo_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.Input_Slogan.characterLimit = GuildProxy.Table.SLOGAN_LENGTH;
			this.Input_Slogan.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Slogan.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			this.Input_Slogan.onValueChanged.RemoveListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Slogan.onValidateInput = null;
		}

		public override void RefreshUI(GuildShareData sharedata, GuildShareDetailData detaildata)
		{
			this.mGuildData = sharedata;
			this.Input_Slogan.text = base.CreateData.GuildSlogan;
			this.CalcSloganLength();
		}

		private char OnValidateInput(string text, int charindex, char addedchar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charindex, addedchar, GuildProxy.Table.SLOGAN_LENGTH);
		}

		private void OnValueChanged(string text)
		{
			base.CreateData.GuildSlogan = text;
			this.Input_Slogan.text = base.CreateData.GuildSlogan;
			this.CalcSloganLength();
			Action onSloganChanged = this.OnSloganChanged;
			if (onSloganChanged == null)
			{
				return;
			}
			onSloganChanged();
		}

		private void CalcSloganLength()
		{
			int num = GuildProxy.Language.CheckTextLength(base.CreateData.GuildSlogan);
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", GuildProxy.Table.SLOGAN_LENGTH - num);
			this.Text_SloganLength.text = infoByID;
			if (GuildProxy.Table.SLOGAN_LENGTH - num <= 0)
			{
				this.Text_SloganLength.color = Color.red;
				return;
			}
			this.Text_SloganLength.color = Color.gray;
		}

		[SerializeField]
		private InputField Input_Slogan;

		[SerializeField]
		private CustomText Text_SloganLength;

		public Action OnSloganChanged;

		private GuildShareData mGuildData;
	}
}
