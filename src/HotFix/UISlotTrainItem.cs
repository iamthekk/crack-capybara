using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UISlotTrainItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
		}

		private void OnClickSelf()
		{
			InfoTipViewModule.InfoTipData infoTipData = new InfoTipViewModule.InfoTipData();
			if (this.slotTrainBuild.IsSkill)
			{
				infoTipData.m_name = this.slotTrainBuild.skillBuild.skillName;
				infoTipData.m_info = this.slotTrainBuild.skillBuild.skillFullDetail;
			}
			else
			{
				infoTipData.m_name = SlotTrainViewModule.GetAttributeName(this.slotTrainBuild.slotTrainType);
				infoTipData.m_info = SlotTrainViewModule.GetAttributeInfo(this.slotTrainBuild.slotTrainType, this.slotTrainBuild.param);
			}
			infoTipData.m_position = base.transform.position;
			infoTipData.m_offsetY = 280f;
			infoTipData.Open();
		}

		public void Refresh(GameEventSlotTrainFactory.SlotTrainBuild build, GameObject showNode)
		{
			if (build == null)
			{
				return;
			}
			this.slotTrainBuild = build;
			this.showParent = showNode;
			string atlasPath = GameApp.Table.GetAtlasPath(105);
			if (this.slotTrainBuild.IsSkill)
			{
				this.imageIcon.transform.localScale = Vector3.one;
				this.imageIcon.SetImage(this.slotTrainBuild.skillBuild.skillAtlas, this.slotTrainBuild.skillBuild.skillIcon);
				this.imageBg.SetImage(atlasPath, GameEventSkillBuildData.GetQuality(this.slotTrainBuild.skillBuild.quality));
				if (string.IsNullOrEmpty(this.slotTrainBuild.skillBuild.skillIconBadge))
				{
					this.imageIconBadge.gameObject.SetActiveSafe(false);
				}
				else
				{
					this.imageIconBadge.gameObject.SetActiveSafe(true);
					this.imageIconBadge.SetImage(this.slotTrainBuild.skillBuild.skillAtlas, this.slotTrainBuild.skillBuild.skillIconBadge);
				}
			}
			else
			{
				string atlasPath2 = GameApp.Table.GetAtlasPath(this.slotTrainBuild.Config.atlas);
				this.imageIcon.transform.localScale = Vector3.one * 0.8f;
				this.imageIcon.SetImage(atlasPath2, this.slotTrainBuild.Config.icon);
				this.imageBg.SetImage(atlasPath, "item_frame_att");
				this.imageIconBadge.gameObject.SetActiveSafe(false);
			}
			this.Reset();
		}

		public void Show()
		{
			this.animator.Play("Show");
		}

		public void Stop()
		{
			DOTween.Kill(this.imageSelect, false);
			this.imageSelect.gameObject.SetActive(true);
			this.imageSelect.SetAlpha(0f);
			this.enterParticle.Stop(true, 0);
			this.selectParticle.Stop(true, 0);
		}

		public void Reset()
		{
			this.isSelected = false;
			DOTween.Kill(this.imageSelect, false);
			this.animator.Play("Enter");
			this.imageMask.gameObject.SetActiveSafe(false);
			this.imageMask2.SetAlpha(0f);
			this.imageSelect.gameObject.SetActiveSafe(true);
			this.imageSelect.SetAlpha(0f);
			this.enterParticle.Stop(true, 0);
			this.selectParticle.Stop(true, 0);
		}

		public void ResetMask()
		{
		}

		public void SetSoundId(int id)
		{
			this.soundId = 70 + id;
		}

		public void SelectNodeEnter(bool showEfx = true)
		{
			if (showEfx)
			{
				this.enterParticle.Play();
				DOTween.Kill(this.imageSelect, false);
			}
			this.imageSelect.SetAlpha(1f);
			if (this.soundId > 0)
			{
				GameApp.Sound.PlayClip(this.soundId, 1f);
			}
		}

		public void SelectNodeExit(bool showEfx = true)
		{
			if (showEfx)
			{
				this.enterParticle.Stop();
				DOTween.Kill(this.imageSelect, false);
				TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions46.DOFade(this.imageSelect, 0f, 0.4f), true);
				return;
			}
			this.imageSelect.SetAlpha(0f);
		}

		public void SelectNodeSelect(bool immediately = false)
		{
			base.transform.SetAsLastSibling();
			GameApp.Sound.PlayClip(80, 1f);
			this.imageIcon.transform.localScale = Vector3.one;
			if (!immediately)
			{
				this.selectParticle.Play();
				Sequence sequence = this.sequencePool.Get();
				Transform parent = base.transform.parent;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					base.transform.SetParent(this.showParent.transform);
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, new Vector3(1.2f, 1.2f, 1.2f), 0.2f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, new Vector3(1f, 1f, 1f), 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.enterParticle.Stop();
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, new Vector3(1.2f, 1.2f, 1.2f), 0.2f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, new Vector3(1f, 1f, 1f), 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.isSelected = true;
					this.imageMask.gameObject.SetActiveSafe(true);
					this.transform.SetParent(parent);
				});
			}
		}

		public void ShowHighLight(float changeDuration, float waitDuration)
		{
			Sequence sequence = this.sequencePool.Get();
			sequence.target = this.imageSelect;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageSelect, 1f, changeDuration));
			TweenSettingsExtensions.AppendInterval(sequence, waitDuration);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageSelect, 0f, changeDuration));
		}

		public void ResultItemFly()
		{
			this.animator.Play("ItemFly");
		}

		public CustomImage imageIcon;

		public CustomImage imageBg;

		public CustomImage imageSelect;

		public Animator animator;

		public ParticleSystem enterParticle;

		public ParticleSystem selectParticle;

		public GameObject imageMask;

		public CustomImage imageMask2;

		public CustomButton buttonSelf;

		public CustomImage imageIconBadge;

		private SequencePool sequencePool = new SequencePool();

		private bool isSelected;

		private GameObject showParent;

		private GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild;

		private int soundId;
	}
}
