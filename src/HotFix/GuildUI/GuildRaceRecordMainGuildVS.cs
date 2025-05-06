using System;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainGuildVS : GuildProxy.GuildProxy_BaseBehaviour
	{
		public bool IsFold { get; private set; }

		protected override void GuildUI_OnInit()
		{
			this.Guild1.Init();
			this.Guild2.Init();
			this.Button.onClick.AddListener(new UnityAction(this.OnClickThis));
			this.Obj_Mine.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			GuildRaceRecordMainGuildVS_GuildUI guild = this.Guild1;
			if (guild != null)
			{
				guild.DeInit();
			}
			GuildRaceRecordMainGuildVS_GuildUI guild2 = this.Guild2;
			if (guild2 != null)
			{
				guild2.DeInit();
			}
			this.Button.onClick.RemoveListener(new UnityAction(this.OnClickThis));
		}

		public void SetData(GuildRaceGuildVSRecord data)
		{
			this.Data = data;
		}

		public void RefreshUI()
		{
			if (this.Data == null || base.gameObject == null)
			{
				return;
			}
			this.Guild1.SetData(this.Data.Guild1);
			this.Guild1.RefreshUI();
			this.Guild2.SetData(this.Data.Guild2);
			this.Guild2.RefreshUI();
			string guildID = base.SDK.GuildInfo.GuildID;
			this.Obj_Mine.SetActive(this.Data.GuildID1 == guildID || this.Data.GuildID2 == guildID);
		}

		public void PlayFold(bool fold, SequencePool m_seqPool)
		{
			this.IsFold = fold;
			Vector3 eulerAngles = this.RTF_Arrow.eulerAngles;
			Vector3 vector = (fold ? new Vector3(0f, 0f, 180f) : new Vector3(0f, 0f, 0f));
			if (m_seqPool != null)
			{
				if (eulerAngles != vector)
				{
					TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions.DOLocalRotate(this.RTF_Arrow, vector, 0.3f, 0));
					return;
				}
			}
			else
			{
				this.RTF_Arrow.eulerAngles = vector;
			}
		}

		private void OnClickThis()
		{
			Action<GuildRaceRecordMainGuildVS> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public CustomButton Button;

		public GuildRaceRecordMainGuildVS_GuildUI Guild1;

		public GuildRaceRecordMainGuildVS_GuildUI Guild2;

		public RectTransform RTF_Arrow;

		public GameObject Obj_Mine;

		public GuildRaceGuildVSRecord Data;

		public Action<GuildRaceRecordMainGuildVS> OnClick;
	}
}
