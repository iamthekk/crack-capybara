using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainUserVSContent : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Prefab_Item.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.RemoveAllChild(true);
		}

		public void RemoveAllChild(bool destroy)
		{
			if (destroy)
			{
				for (int i = 0; i < this.UIList.Count; i++)
				{
					this.UIList[i].DeInit();
				}
				this.UIList.Clear();
				this.RTF_Content.DestroyChildren();
				return;
			}
			for (int j = 0; j < this.UIList.Count; j++)
			{
				this.UIList[j].SetActive(false);
			}
		}

		public void SetDataList(List<GuildRaceUserVSRecord> list)
		{
			this.DataList.Clear();
			this.DataList.AddRange(list);
		}

		public void RefreshUI(SequencePool m_seqPool)
		{
			List<GuildRaceUserVSRecord> dataList = this.DataList;
			float num = 10f;
			for (int i = 0; i < dataList.Count; i++)
			{
				GuildRaceRecordMainUserVS guildRaceRecordMainUserVS = null;
				if (i < this.UIList.Count)
				{
					guildRaceRecordMainUserVS = this.UIList[i];
				}
				if (guildRaceRecordMainUserVS == null)
				{
					guildRaceRecordMainUserVS = Object.Instantiate<GuildRaceRecordMainUserVS>(this.Prefab_Item, this.RTF_Content);
					guildRaceRecordMainUserVS.SetActive(true);
					guildRaceRecordMainUserVS.Init();
					if (i < this.UIList.Count)
					{
						this.UIList[i] = guildRaceRecordMainUserVS;
					}
					else
					{
						this.UIList.Add(guildRaceRecordMainUserVS);
					}
				}
				guildRaceRecordMainUserVS.SetData(dataList[i], i);
				guildRaceRecordMainUserVS.RefreshUI();
				guildRaceRecordMainUserVS.SetActive(true);
				RectTransform rectTransform = guildRaceRecordMainUserVS.gameObject.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(0f, -num);
				num += rectTransform.sizeDelta.y;
				rectTransform.localScale = Vector3.zero;
				Sequence sequence = m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)i * 0.05f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.15f));
			}
			for (int j = dataList.Count; j < this.UIList.Count; j++)
			{
				this.UIList[j].SetActive(false);
			}
			num += 10f;
			Vector2 sizeDelta = this.RTF_Content.sizeDelta;
			sizeDelta.y = num;
			this.RTF_Content.sizeDelta = sizeDelta;
		}

		public RectTransform RTF_Content;

		public GuildRaceRecordMainUserVS Prefab_Item;

		private List<GuildRaceUserVSRecord> DataList = new List<GuildRaceUserVSRecord>();

		[HideInInspector]
		public List<GuildRaceRecordMainUserVS> UIList = new List<GuildRaceRecordMainUserVS>();
	}
}
