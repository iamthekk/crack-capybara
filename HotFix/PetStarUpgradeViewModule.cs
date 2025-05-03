using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PetStarUpgradeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.attributeNode.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.openData = data as PetStarUpgradeViewModule.OpenData;
			if (this.openData == null)
			{
				return;
			}
			GameApp.Sound.PlayClip(606, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.closeDelayTime -= deltaTime;
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.m_seqPool.Clear(false);
			this.m_attributeDic.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnClose.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.btnMaskClose.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnClose.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			this.btnMaskClose.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
		}

		private void OnClickCloseBt()
		{
			if (this.closeDelayTime > 0f)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.PetStarUpgradeViewModule, null);
		}

		private void PlayItemScale(RectTransform tf, float interval, float time = 1f)
		{
			tf.anchoredPosition = new Vector2(-1500f, tf.anchoredPosition.y);
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, interval);
			TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(tf, 0f, time, false)), 27);
		}

		public virtual async Task LoadSpine()
		{
			int memberId = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.openData.petData.petId)
				.memberId;
			ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
			this.uiSpineModelItem.SetScale(elementById.uiScale);
			await this.uiSpineModelItem.ShowModel(memberId, 0, "Idle", true);
		}

		public float closeDelayTime = 2f;

		public PetStarNode starNodeLeft;

		public PetStarNode starNodeRight;

		public GameObject goArrow;

		public UIEquipMergeAttributeNode attributeNode;

		public CustomButton btnClose;

		public CustomButton btnMaskClose;

		public GameObject goInfo;

		public GameObject goModel;

		public UISpineModelItem uiSpineModelItem;

		private PetStarUpgradeViewModule.OpenData openData;

		private Dictionary<int, UIEquipMergeAttributeNode> m_attributeDic = new Dictionary<int, UIEquipMergeAttributeNode>();

		private SequencePool m_seqPool = new SequencePool();

		public class OpenData
		{
			public PetData petData;
		}
	}
}
