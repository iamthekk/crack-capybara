using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class IAPBattlePassPreviewReward : CustomBehaviour
	{
		public RectTransform RTF
		{
			get
			{
				return base.rectTransform;
			}
		}

		protected override void OnInit()
		{
			this.ButtonJump.m_onClick = null;
			this.ItemUI.Init();
			this.PlayToHide();
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			if (this.ItemUI != null)
			{
				this.ItemUI.DeInit();
			}
		}

		public void RefreshUI()
		{
			if (this.mShowData == null)
			{
				return;
			}
			this.ItemUI.SetData(this.mShowData.CreateJumpShowData());
			this.ItemUI.OnRefresh();
			string text = "";
			if (this.ItemUI.m_propData != null)
			{
				string nameID = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.ItemUI.m_propData.id)
					.nameID;
				text = string.Format("{0} X {1}", Singleton<LanguageManager>.Instance.GetInfoByID(nameID), this.ItemUI.m_propData.count);
			}
			this.TextTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_level", new object[]
			{
				this.mShowData.Level,
				text
			});
		}

		public void PlayToData(IAPBattlePassData data)
		{
			if (this.mShowData == data)
			{
				return;
			}
			this.mShowData = data;
			if (this.mShowData == null)
			{
				this.PlayToHide();
				return;
			}
			this.RefreshUI();
			this.m_seqPool.Clear(false);
			this.ButtonJump.m_onClick = null;
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.RTF, -230f, 0.3f, false));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.RTF, 200f, 0.3f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.ButtonJump.m_onClick = new Action(this.OnClickJumpButton);
			});
		}

		public void PlayToHide()
		{
			this.m_seqPool.Clear(false);
			this.ButtonJump.m_onClick = null;
			TweenSettingsExtensions.Append(this.m_seqPool.Get(), ShortcutExtensions46.DOAnchorPosY(this.RTF, -230f, 0.3f, false));
		}

		private void OnClickJumpButton()
		{
			Action<IAPBattlePassData> onClickJump = this.OnClickJump;
			if (onClickJump == null)
			{
				return;
			}
			onClickJump(this.mShowData);
		}

		public CustomText TextTips;

		public CustomButton ButtonJump;

		public UIItem ItemUI;

		private IAPBattlePassData mShowData;

		public Action<IAPBattlePassData> OnClickJump;

		private SequencePool m_seqPool = new SequencePool();
	}
}
