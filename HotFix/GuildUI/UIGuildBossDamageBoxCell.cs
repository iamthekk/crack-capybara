using System;
using System.Collections.Generic;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildBossDamageBoxCell : GuildProxy.GuildProxy_BaseBehaviour
	{
		public UIGuildBossDamageBoxData Data
		{
			get
			{
				return this.mData;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.ButtonBox.onClick.AddListener(new UnityAction(this.OnClickBox));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.ButtonBox.onClick.RemoveListener(new UnityAction(this.OnClickBox));
		}

		public void SetData(UIGuildBossDamageBoxData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null)
			{
				return;
			}
			this.ObjBoxOpen.SetActive(this.mData.IsOpen);
			this.ObjBoxClose.SetActive(!this.mData.IsOpen);
			this.TextDamage.text = DxxTools.FormatNumber(this.mData.Damage);
		}

		private void OnClickBox()
		{
			Action<UIGuildBossDamageBoxCell> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public List<PropData> GetRewards()
		{
			if (this.mData != null)
			{
				return this.mData.Table.Reward.ToPropDataList();
			}
			return new List<PropData>();
		}

		public CustomButton ButtonBox;

		public GameObject ObjBoxOpen;

		public GameObject ObjBoxClose;

		public CustomText TextDamage;

		private UIGuildBossDamageBoxData mData;

		public Action<UIGuildBossDamageBoxCell> OnClick;
	}
}
