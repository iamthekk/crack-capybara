using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIActivitySlotTrainItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
			this.uiItem.onClick = new Action<UIItem, PropData, object>(this.onClickItem);
			this.localScale = base.transform.localScale;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
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
			this.imageSelect.gameObject.SetActiveSafe(true);
			this.imageSelect.SetAlpha(0f);
			base.transform.localScale = this.localScale;
			this.enterParticle.Stop(true, 0);
			this.selectParticle.Stop(true, 0);
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
			if (!immediately)
			{
				this.selectParticle.Play();
				Sequence sequence = this.sequencePool.Get();
				Transform parent = base.transform.parent;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					base.transform.SetParent(this.showParent.transform);
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, this.localScale * 1.2f, 0.2f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, this.localScale, 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.enterParticle.Stop(true);
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, this.localScale * 1.2f, 0.2f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, this.localScale, 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.isSelected = true;
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

		public void Refresh(PropData item, GameObject showNode)
		{
			if (item == null)
			{
				return;
			}
			this.showParent = showNode;
			this.uiItem.SetData(item);
			this.uiItem.OnRefresh();
			this.Reset();
		}

		private void onClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		public CustomText txtLimited;

		public CustomImage imageSelect;

		public Animator animator;

		public ParticleSystem enterParticle;

		public ParticleSystem selectParticle;

		public GameObject imageMask;

		private SequencePool sequencePool = new SequencePool();

		private bool isSelected;

		private GameObject showParent;

		private int soundId;

		public UIItem uiItem;

		public GameObject uiItemIconObj;

		public UISpineModelItem spineModelItem;

		public UISpineMountModelItem spineMountModelItem;

		private Vector3 localScale;
	}
}
