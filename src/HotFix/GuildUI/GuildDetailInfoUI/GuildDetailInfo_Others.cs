using System;
using Dxx.Guild;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI.GuildDetailInfoUI
{
	public class GuildDetailInfo_Others : GuildDetailInfo_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.Input_Level.onValueChanged.AddListener(new UnityAction<string>(this.OnConditionChanged));
			this.Button_Language.onClick.AddListener(new UnityAction(this.OnOpenChangeLanguageView));
			this.JoinKindFree.OnClickButton = new Action<CustomChooseButton>(this.OnSwitchFree);
			this.JoinKindApply.OnClickButton = new Action<CustomChooseButton>(this.OnSwitchApply);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			this.Input_Level.onValueChanged.RemoveListener(new UnityAction<string>(this.OnConditionChanged));
			this.Button_Language.onClick.RemoveListener(new UnityAction(this.OnOpenChangeLanguageView));
		}

		public override void RefreshUI(GuildShareData sharedata, GuildShareDetailData detaildata)
		{
			this.mGuildData = sharedata;
			this.OnSwitchJoinKind(sharedata.JoinKind);
			this.NeedLevel = sharedata.LevelNeed;
			this.OnConditionChanged(this.NeedLevel.ToString());
			this.Text_Language.text = GuildProxy.Language.GetLanguageNameString(sharedata.GuildLanguage);
		}

		private void OnConditionChanged(string text)
		{
			int num;
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NeedLevel = 0L;
				this.Input_Level.SetTextWithoutNotify(string.Empty);
			}
			else if (int.TryParse(text, out num))
			{
				this.NeedLevel = (long)Mathf.Clamp(num, 0, int.MaxValue);
				this.Input_Level.SetTextWithoutNotify(this.NeedLevel.ToString());
			}
			else
			{
				string text2 = ((this.NeedLevel > 0L) ? this.NeedLevel.ToString() : string.Empty);
				this.Input_Level.SetTextWithoutNotify(text2);
			}
			base.CreateData.JoinCondition_Level = (int)this.NeedLevel;
		}

		private void OnOpenChangeLanguageView()
		{
			GuildProxy.UI.OpenUILanguage(base.CreateData.Language, new Action<LanguageType>(this.OnSelectedLanguage));
		}

		private void OnSelectedLanguage(LanguageType type)
		{
			GuildProxy.UI.CloseUILanguage();
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			if (base.CreateData.Language == type)
			{
				return;
			}
			base.CreateData.Language = type;
			this.Text_Language.text = GuildProxy.Language.GetLanguageNameString(type);
		}

		private void OnSwitchFree(CustomChooseButton btn)
		{
			this.OnSwitchJoinKind(GuildJoinKind.Free);
		}

		private void OnSwitchApply(CustomChooseButton btn)
		{
			this.OnSwitchJoinKind(GuildJoinKind.Conditional);
		}

		private void OnSwitchJoinKind(GuildJoinKind joinkind)
		{
			this.JoinKindJoin = joinkind;
			this.JoinKindFree.SetSelect(this.JoinKindJoin == GuildJoinKind.Free);
			this.JoinKindApply.SetSelect(this.JoinKindJoin == GuildJoinKind.Conditional);
			base.CreateData.JoinKind = this.JoinKindJoin;
		}

		[SerializeField]
		private InputField Input_Level;

		[SerializeField]
		private CustomButton Button_Language;

		[SerializeField]
		private CustomText Text_Language;

		[SerializeField]
		private CustomChooseButton JoinKindFree;

		[SerializeField]
		private CustomChooseButton JoinKindApply;

		private GuildShareData mGuildData;

		private GuildJoinKind JoinKindJoin;

		private long NeedLevel;
	}
}
