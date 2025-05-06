using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.UI;
using HotFix.GuildUI.GuildActivitys;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildActivityViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.Button_Back.m_onClick = new Action(this.CloseThisView);
			this.CurrencyCtrl.Init();
			this.CurrencyCtrl.SetStyle(EModuleId.GuildActivity, new List<int> { 1, 2, 7000001 });
			this.RTFScrollContent = this.ScrollView.content.transform as RectTransform;
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].Init();
				this.UIList[i].SetActive(false);
			}
		}

		protected override void OnViewOpen(object data)
		{
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].SetActive(false);
			}
			this.m_seqPool.Clear(false);
			float num = 0f;
			for (int j = 0; j < this.UIList.Count; j++)
			{
				GuildActivityBase guildActivityBase = this.UIList[j];
				if (!(guildActivityBase == null))
				{
					guildActivityBase.RefreshUIOnOpen();
					guildActivityBase.RTF.anchorMin = new Vector2(0.5f, 1f);
					guildActivityBase.RTF.anchorMax = new Vector2(0.5f, 1f);
					guildActivityBase.RTF.anchoredPosition = new Vector2(0f, -num - 220f);
					guildActivityBase.CanvasGroup.alpha = 0f;
					guildActivityBase.SetActive(true);
					float num2 = (float)j * 0.1f;
					float num3 = 0.3f + (float)j * 0.05f;
					Sequence sequence = this.m_seqPool.Get();
					TweenSettingsExtensions.AppendInterval(sequence, num2);
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(guildActivityBase.RTF, -num, num3, false));
					Sequence sequence2 = this.m_seqPool.Get();
					TweenSettingsExtensions.AppendInterval(sequence2, num2);
					TweenSettingsExtensions.Append(sequence2, ShortcutExtensions46.DOFade(guildActivityBase.CanvasGroup, 1f, num3));
					num += guildActivityBase.RTF.sizeDelta.y;
				}
			}
			Vector2 sizeDelta = this.RTFScrollContent.sizeDelta;
			sizeDelta.y = num + 100f;
			this.RTFScrollContent.sizeDelta = sizeDelta;
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].DeInit();
			}
			this.UIList.Clear();
			this.CurrencyCtrl.DeInit();
		}

		private void CloseThisView()
		{
			GuildProxy.UI.CloseGuildActivity(null);
		}

		public CustomButton Button_Back;

		public ModuleCurrencyCtrl CurrencyCtrl;

		public ScrollRect ScrollView;

		public RectTransform RTFScrollContent;

		[SerializeField]
		public List<GuildActivityBase> UIList = new List<GuildActivityBase>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
