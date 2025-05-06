using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordBattleViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.PopCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Prefab_Item.gameObject.SetActive(false);
		}

		protected override void OnViewOpen(object data)
		{
			GuildRaceRecordBattleViewModule.OpenData openData = data as GuildRaceRecordBattleViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
				this.RefreshUI();
			}
		}

		protected override void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.ClearDeleteUIList();
		}

		private void ClearDeleteUIList()
		{
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].DeInit();
			}
			this.UIList.Clear();
			this.RTF_Content.DestroyChildren();
		}

		public void RefreshUI()
		{
			if (this.mOpenData == null || this.mOpenData.Record == null)
			{
				return;
			}
			this.m_seqPool.Clear(false);
			this.ClearDeleteUIList();
			this.mOpenData.GetHomeUserID();
			int num = 0;
			while (num < 3 && num < this.mOpenData.Record.ResultList.Count)
			{
				GuildRaceRecordBattleItem prefab_Item = this.Prefab_Item;
				GuildRaceUserVSRecordResult guildRaceUserVSRecordResult = this.mOpenData.Record.ResultList[num];
				GuildRaceRecordBattleItem guildRaceRecordBattleItem = Object.Instantiate<GuildRaceRecordBattleItem>(prefab_Item, this.RTF_Content);
				guildRaceRecordBattleItem.SetActive(true);
				RectTransform rectTransform = guildRaceRecordBattleItem.transform as RectTransform;
				guildRaceRecordBattleItem.Init();
				guildRaceRecordBattleItem.SetData(this.mOpenData.Record, num);
				guildRaceRecordBattleItem.RefreshUI();
				this.UIList.Add(guildRaceRecordBattleItem);
				rectTransform.localScale = Vector3.zero;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)num * 0.1f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.15f));
				num++;
			}
		}

		private void OnPopClick(int obj)
		{
			this.ClickCloseThis();
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceRecordBattle();
		}

		public const int BattleCount = 3;

		public UIGuildPopCommon PopCommon;

		public RectTransform RTF_Content;

		public GuildRaceRecordBattleItem Prefab_Item;

		[HideInInspector]
		public List<GuildRaceRecordBattleItem> UIList = new List<GuildRaceRecordBattleItem>();

		private GuildRaceRecordBattleViewModule.OpenData mOpenData;

		private SequencePool m_seqPool = new SequencePool();

		public class OpenData
		{
			public long GetHomeUserID()
			{
				if (this.Record == null)
				{
					return 0L;
				}
				return this.Record.User1.UserData.UserID;
			}

			public GuildRaceUserVSRecord Record;
		}
	}
}
