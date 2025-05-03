using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventSlotRewardViewModule : BaseViewModule
	{
		private float Speed
		{
			get
			{
				return (float)this.SkillGridHeight / 0.1f;
			}
		}

		public override void OnCreate(object data)
		{
			ComponentRegister component = base.gameObject.GetComponent<ComponentRegister>();
			this.iconObj = component.GetGameObject("IconObj");
			this.imageIcon = component.GetGameObject("ImageIcon").GetComponent<CustomImage>();
			this.textResult = component.GetGameObject("TextResult").GetComponent<CustomText>();
			this.skillDetailObj = component.GetGameObject("SkillObj");
			this.buttonContinue = component.GetGameObject("ButtonContinue").GetComponent<CustomButton>();
			this.imageTitle = component.GetGameObject("ImageTitle");
			this.textTitle = component.GetGameObject("TextTitle").GetComponent<CustomLanguageText>();
			this.imageLight = component.GetGameObject("ImageLight").GetComponent<Image>();
			this.rewardIcon = component.GetGameObject("RewardIcon");
			this.skillLayout = component.GetGameObject("SkillLayout").GetComponent<VerticalLayoutGroup>();
			this.imageIconNode = component.GetGameObject("ImageIconNode");
			this.skillSlotObj = component.GetGameObject("SkillSlotObj");
			this.spriteRegister = component.GetGameObject("SpriteRegister").GetComponent<SpriteRegister>();
			this.imageQuality = component.GetGameObject("ImageQuality").GetComponent<CustomImage>();
			this.imageMask = component.GetGameObject("ImageMask").GetComponent<Image>();
			this.textSkillInfo = component.GetGameObject("TextSkillInfo").GetComponent<CustomText>();
			this.skillDetailItem = this.skillDetailObj.GetComponent<UIGameEventSkillDetailItem>();
			this.skillDetailItem.Init();
			this.iconObj.SetActiveSafe(false);
			this.skillSlotObj.SetActiveSafe(false);
			this.imageIconNode.SetActiveSafe(false);
			this.buttonContinue.onClick.AddListener(new UnityAction(this.OnContinue));
			this.SkillGridHeight = (int)(this.imageIconNode.GetComponent<RectTransform>().sizeDelta.y + this.skillLayout.spacing);
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GameEventSlotRewardViewModule.OpenData;
			if (this.openData == null)
			{
				return;
			}
			GameApp.Sound.PlayClip(59, 1f);
			this.ResetAni();
			this.skillDetailItem.Refresh(this.openData.skillBuild, new Action<GameEventSkillBuildData>(this.OnClickSkill));
			this.skillDetailItem.gameObject.SetActiveSafe(false);
			this.iconObj.SetActiveSafe(false);
			this.textSkillInfo.text = "";
			this.skillSlotObj.SetActiveSafe(true);
			this.CreateSkillSlot();
			this.PlaySkillAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isSkillSlot)
			{
				Vector3 localPosition = this.skillLayout.transform.localPosition;
				localPosition.y -= this.currentSpeed * deltaTime;
				if (localPosition.y <= 0f)
				{
					if (!this.isResetIcon)
					{
						this.isResetIcon = true;
						this.skillIconList[this.skillIconList.Count - 1].sprite = this.skillIconList[0].sprite;
					}
					localPosition.y = (float)(this.SkillGridHeight * 3);
				}
				this.skillLayout.transform.localPosition = localPosition;
				if (this.isStopSkillSlot)
				{
					if (this.endY > localPosition.y)
					{
						return;
					}
					float num = Mathf.Abs(this.endY - localPosition.y) + (float)this.SkillGridHeight;
					float num2 = Mathf.Abs((float)this.SkillGridHeight / num);
					num2 = Mathf.Clamp01(num2);
					this.currentSpeed = (1f - num2) * this.Speed;
					if (this.currentSpeed <= 0.1f)
					{
						this.EndSkillSlot();
					}
				}
			}
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
			this.skillIconList.Clear();
			this.buttonContinue.onClick.RemoveListener(new UnityAction(this.OnContinue));
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnContinue()
		{
			GameApp.View.CloseView(ViewName.GameEventSlotRewardViewModule, null);
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
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.imageTitle.transform, Vector3.one, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textTitle, 1f, num));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.buttonContinue.transform, Vector3.one, num), 22));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
			});
		}

		private void PlaySkillAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.skillSlotObj.transform.localScale = Vector3.zero;
			Color color = this.imageMask.color;
			float a = color.a;
			color.a = 0f;
			this.imageMask.color = color;
			color = this.textSkillInfo.color;
			color.a = 0f;
			this.textSkillInfo.color = color;
			float num = 0.2f;
			float num2 = 0.1f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillSlotObj.transform, Vector3.one * 1.2f, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillSlotObj.transform, Vector3.one, num2));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.imageMask, a, num2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textSkillInfo, 1f, num2));
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.StartSkillSlot));
		}

		private void StartSkillSlot()
		{
			this.currentSpeed = this.Speed;
			this.isResetIcon = false;
			this.isStopSkillSlot = false;
			this.isSkillSlot = true;
			DelayCall.Instance.CallOnce(500, delegate
			{
				this.isStopSkillSlot = true;
			});
		}

		private void EndSkillSlot()
		{
			this.isSkillSlot = false;
			Vector3 localPosition = this.skillLayout.transform.localPosition;
			localPosition.y = this.endY;
			this.skillLayout.transform.localPosition = localPosition;
			Sequence sequence = this.sequencePool.Get();
			float num = 0.1f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textSkillInfo, 0f, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveX(this.skillSlotObj.transform, -343f, num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.skillSlotObj.SetActiveSafe(false);
				this.skillDetailItem.SetActive(true);
			});
			TweenSettingsExtensions.Append(sequence, this.skillDetailItem.PlayOpenBoxAni());
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.PlayOpenAni();
			});
		}

		private void CreateSkillSlot()
		{
			SkillBuildSourceType skillBuildSourceType = SkillBuildSourceType.SlotNormal;
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(skillBuildSourceType, 3, this.openData.randomSeed);
			bool flag = false;
			int num = 0;
			for (int i = 0; i < randomSkillList.Count; i++)
			{
				if (this.openData.skillBuild.id == randomSkillList[i].id)
				{
					num = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num2 = Random.Range(0, randomSkillList.Count);
				randomSkillList[num2] = this.openData.skillBuild;
				num = num2;
			}
			for (int j = 0; j < randomSkillList.Count; j++)
			{
				CustomImage customImage = this.CreateIcon();
				customImage.SetImage(randomSkillList[j].skillAtlas, randomSkillList[j].skillIcon);
				this.skillIconList.Add(customImage);
			}
			CustomImage customImage2 = this.CreateIcon();
			this.skillIconList.Add(customImage2);
			this.endY = (float)(this.SkillGridHeight * num);
			Vector3 localPosition = this.skillLayout.transform.localPosition;
			localPosition.y = (float)(this.SkillGridHeight * 3);
			this.skillLayout.transform.localPosition = localPosition;
		}

		private CustomImage CreateIcon()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.imageIconNode);
			gameObject.transform.SetParentNormal(this.skillLayout.transform, false);
			gameObject.gameObject.SetActiveSafe(true);
			return gameObject.GetComponent<CustomImage>();
		}

		private GameObject iconObj;

		private CustomImage imageIcon;

		private CustomText textResult;

		private GameObject skillDetailObj;

		private CustomButton buttonContinue;

		private GameObject imageTitle;

		private CustomLanguageText textTitle;

		private Image imageLight;

		private GameObject rewardIcon;

		private VerticalLayoutGroup skillLayout;

		private GameObject imageIconNode;

		private GameObject skillSlotObj;

		private SpriteRegister spriteRegister;

		private CustomImage imageQuality;

		private Image imageMask;

		private CustomText textSkillInfo;

		private GameEventSlotRewardViewModule.OpenData openData;

		private UIGameEventSkillDetailItem skillDetailItem;

		private SequencePool sequencePool = new SequencePool();

		private List<CustomImage> skillIconList = new List<CustomImage>();

		private float endY;

		private bool isSkillSlot;

		private bool isStopSkillSlot;

		private bool isResetIcon;

		private const int SHOW_SKILLS = 3;

		private const float RESET_Y = 0f;

		private const int SKILL_SLOT_STOP_TIME = 500;

		private const string NORMAL_BG = "NormalBg";

		private const string NORMAL_ICON = "NormalIcon";

		private float currentSpeed;

		private int SkillGridHeight = 190;

		public class OpenData
		{
			public GameEventSkillBuildData skillBuild;

			public int randomSeed;
		}
	}
}
