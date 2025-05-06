using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIGameEventSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.imagePingPong != null)
			{
				this.imagePingPong.OnOff = false;
			}
			this.starItem.SetActive(false);
			this.borderObj.SetActive(false);
			this.ActiveStar(false);
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
		}

		protected override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.starList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void Refresh(GameEventSkillBuildData data, bool isStarAni, bool isShowBorder)
		{
			if (data == null)
			{
				return;
			}
			this.skillBuildData = data;
			int skillBuildGroupMaxLevel = Singleton<GameEventController>.Instance.GetSkillBuildGroupMaxLevel(data.groupId);
			string atlasPath = GameApp.Table.GetAtlasPath(105);
			this.imageQuality.SetImage(atlasPath, GameEventSkillBuildData.GetQuality(data.quality));
			this.imageIcon.SetImage(data.skillAtlas, data.skillIcon);
			this.imageUpgrade.gameObject.SetActiveSafe(data.level > 1);
			if (string.IsNullOrEmpty(data.skillIconBadge))
			{
				this.imageIconBadge.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.imageIconBadge.gameObject.SetActiveSafe(true);
				this.imageIconBadge.SetImage(data.skillAtlas, data.skillIconBadge);
			}
			string atlasPath2 = GameApp.Table.GetAtlasPath(101);
			for (int i = 0; i < skillBuildGroupMaxLevel; i++)
			{
				CustomImage customImage;
				if (i < this.emptyStarList.Count)
				{
					customImage = this.emptyStarList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.starItem);
					gameObject.SetParentNormal(this.emptyStarLayout, false);
					customImage = gameObject.GetComponent<CustomImage>();
					this.emptyStarList.Add(customImage);
				}
				ImagePingPong component = customImage.GetComponent<ImagePingPong>();
				if (component)
				{
					component.OnOff = false;
				}
				customImage.SetImage(atlasPath2, "star1");
				customImage.gameObject.SetActiveSafe(true);
			}
			for (int j = 0; j < skillBuildGroupMaxLevel; j++)
			{
				CustomImage customImage2;
				if (j < this.starList.Count)
				{
					customImage2 = this.starList[j];
				}
				else
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.starItem);
					gameObject2.SetParentNormal(this.starLayout, false);
					customImage2 = gameObject2.GetComponent<CustomImage>();
					this.starList.Add(customImage2);
				}
				ImagePingPong component2 = customImage2.GetComponent<ImagePingPong>();
				string text = "star1";
				if (j < data.level)
				{
					if (data.IsComposeSkill)
					{
						text = "star3";
					}
					else
					{
						text = "star2";
					}
				}
				customImage2.SetImage(atlasPath2, text);
				customImage2.gameObject.SetActiveSafe(true);
				if (isStarAni)
				{
					if (component2 != null && j == data.level - 1)
					{
						component2.OnOff = true;
					}
				}
				else if (component2)
				{
					component2.OnOff = false;
				}
			}
			this.imageQualityTag.gameObject.SetActiveSafe(false);
			switch (data.quality)
			{
			case SkillBuildQuality.Gray:
				this.imageQualityTag.sprite = this.sr.GetSprite("gray");
				this.textQuality.text = Singleton<LanguageManager>.Instance.GetInfoByID("skill_quality_gray");
				return;
			case SkillBuildQuality.Gold:
				this.imageQualityTag.sprite = this.sr.GetSprite("golden");
				this.textQuality.text = Singleton<LanguageManager>.Instance.GetInfoByID("skill_quality_golden");
				return;
			case SkillBuildQuality.Red:
				this.imageQualityTag.sprite = this.sr.GetSprite("red");
				this.textQuality.text = Singleton<LanguageManager>.Instance.GetInfoByID("skill_quality_red");
				return;
			default:
				return;
			}
		}

		public void ActiveStar(bool isActive)
		{
			this.starNode.SetActiveSafe(isActive);
		}

		public GameEventSkillBuildData GetSkillBuildData()
		{
			return this.skillBuildData;
		}

		public void OnClickSelf()
		{
			EventArgClickSkill eventArgClickSkill = new EventArgClickSkill();
			eventArgClickSkill.SetData(this);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ClickSkill, eventArgClickSkill);
		}

		public Vector3 GetPosition()
		{
			return base.gameObject.transform.position;
		}

		public void PlayShowAnimation()
		{
			this.ClearSequence();
			this.m_seq = DOTween.Sequence();
			this.m_child.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(this.m_seq, ShortcutExtensions.DOScale(this.m_child.transform, Vector3.one * 1.1f, 0.15f));
			TweenSettingsExtensions.Append(this.m_seq, ShortcutExtensions.DOScale(this.m_child.transform, Vector3.one * 1f, 0.05f));
		}

		private void ClearSequence()
		{
			if (this.m_seq != null)
			{
				TweenExtensions.Kill(this.m_seq, false);
				this.m_seq = null;
			}
		}

		public void ShowQualityTag(bool isShow)
		{
			this.imageQualityTag.gameObject.SetActiveSafe(isShow);
		}

		public CustomImage imageIcon;

		public GameObject starLayout;

		public GameObject starItem;

		public GameObject borderObj;

		public CustomButton buttonSelf;

		public ImagePingPong imagePingPong;

		public GameObject emptyStarLayout;

		public CustomImage imageQuality;

		public GameObject m_child;

		public CustomImage imageIconBadge;

		public GameObject starNode;

		public CustomImage imageUpgrade;

		public SpriteRegister sr;

		public CustomImage imageQualityTag;

		public CustomText textQuality;

		private List<CustomImage> emptyStarList = new List<CustomImage>();

		private List<CustomImage> starList = new List<CustomImage>();

		private GameEventSkillBuildData skillBuildData;

		public const string EmptyStar = "star1";

		public const string YellowStar = "star2";

		public const string RedStar = "star3";

		private Sequence m_seq;
	}
}
