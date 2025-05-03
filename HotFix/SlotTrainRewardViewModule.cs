using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class SlotTrainRewardViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 948;
		}

		public override void OnCreate(object data)
		{
			this.skillDetailItem.Init();
			this.iconObj.SetActiveSafe(false);
			this.buttonContinue.onClick.AddListener(new UnityAction(this.OnContinue));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as SlotTrainRewardViewModule.OpenData;
			if (this.openData == null)
			{
				return;
			}
			GameApp.Sound.PlayClip(59, 1f);
			if (!this.openData.IsSkill)
			{
				this.skillDetailItem.gameObject.SetActiveSafe(false);
				this.iconObj.SetActiveSafe(true);
				string atlasPath = GameApp.Table.GetAtlasPath(this.openData.atlasId);
				this.imageIcon.SetImage(atlasPath, this.openData.icon);
				this.textResult.text = this.openData.showInfo;
				this.PlayOpenAni();
				return;
			}
			this.ResetAni();
			this.skillDetailItem.Refresh(this.openData.skillBuild, new Action<GameEventSkillBuildData>(this.OnClickSkill));
			this.skillDetailItem.gameObject.SetActiveSafe(false);
			this.iconObj.SetActiveSafe(false);
			this.PlaySkillAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
			this.skillDetailItem.OnHide();
		}

		public override void OnDelete()
		{
			this.buttonContinue.onClick.RemoveListener(new UnityAction(this.OnContinue));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnContinue()
		{
			GameApp.View.CloseView(ViewName.SlotTrainRewardViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Close_SlotTrain_Reward, null);
		}

		private void OnClickSkill(GameEventSkillBuildData obj)
		{
			this.OnContinue();
		}

		private void ResetAni()
		{
			this.imageTitle.transform.localScale = new Vector3(0f, 1f, 1f);
			Color color = this.textTitle.color;
			color.a = 1f;
			this.textTitle.color = color;
			if (!this.openData.IsSkill)
			{
				this.rewardIcon.transform.localScale = Vector3.zero;
			}
			color = this.imageLight.color;
			color.a = 0f;
			this.imageLight.color = color;
			this.buttonContinue.transform.localScale = Vector3.zero;
		}

		private void PlayOpenAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.ResetAni();
			float num = 0.2f;
			if (!this.openData.IsSkill)
			{
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.rewardIcon.transform, Vector3.one, num), 22));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.imageLight, 1f, num));
			}
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(this.imageTitle.transform, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.textTitle, 1f, num));
			TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one, num), 22));
		}

		private void PlaySkillAni()
		{
			this.skillDetailItem.SetActive(true);
			this.PlayOpenAni();
		}

		public GameObject iconObj;

		public CustomImage imageIcon;

		public CustomText textResult;

		public UIGameEventSkillDetailItem skillDetailItem;

		public CustomButton buttonContinue;

		public GameObject imageTitle;

		public CustomLanguageText textTitle;

		public Image imageLight;

		public GameObject rewardIcon;

		private SequencePool sequencePool = new SequencePool();

		private SlotTrainRewardViewModule.OpenData openData;

		public class OpenData
		{
			public bool IsSkill
			{
				get
				{
					return this.skillBuild != null;
				}
			}

			public GameEventSkillBuildData skillBuild;

			public int atlasId;

			public string icon;

			public string showInfo;
		}
	}
}
