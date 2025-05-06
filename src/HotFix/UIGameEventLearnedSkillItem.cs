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
	public class UIGameEventLearnedSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.starItemObj.SetActiveSafe(false);
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			this.skillItem.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.emptyStarList.Clear();
			this.starList.Clear();
		}

		public void Refresh(GameEventSkillBuildData skillBuildData)
		{
			if (skillBuildData == null)
			{
				return;
			}
			this.data = skillBuildData;
			this.skillItem.Refresh(skillBuildData, false, false);
			this.RefreshStar();
		}

		private void RefreshStar()
		{
			int skillBuildGroupMaxLevel = Singleton<GameEventController>.Instance.GetSkillBuildGroupMaxLevel(this.data.groupId);
			string atlasPath = GameApp.Table.GetAtlasPath(101);
			for (int i = 0; i < skillBuildGroupMaxLevel; i++)
			{
				CustomImage customImage;
				if (i < this.emptyStarList.Count)
				{
					customImage = this.emptyStarList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.starItemObj);
					gameObject.SetParentNormal(this.emptyStarLayout, false);
					customImage = gameObject.GetComponent<CustomImage>();
					this.emptyStarList.Add(customImage);
				}
				customImage.SetImage(atlasPath, "star1");
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
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.starItemObj);
					gameObject2.SetParentNormal(this.starLayout, false);
					customImage2 = gameObject2.GetComponent<CustomImage>();
					this.starList.Add(customImage2);
				}
				string text = "star1";
				if (j < this.data.level)
				{
					if (this.data.IsComposeSkill)
					{
						text = "star3";
					}
					else
					{
						text = "star2";
					}
				}
				customImage2.SetImage(atlasPath, text);
				customImage2.gameObject.SetActiveSafe(true);
			}
		}

		public void PlayShowAnimation()
		{
			this.ClearSequence();
			this.m_seq = DOTween.Sequence();
			this.child.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(this.m_seq, ShortcutExtensions.DOScale(this.child.transform, Vector3.one * 1.1f, 0.15f));
			TweenSettingsExtensions.Append(this.m_seq, ShortcutExtensions.DOScale(this.child.transform, Vector3.one * 1f, 0.05f));
		}

		private void ClearSequence()
		{
			if (this.m_seq != null)
			{
				TweenExtensions.Kill(this.m_seq, false);
				this.m_seq = null;
			}
		}

		public void OnClickSelf()
		{
			EventArgClickSkill eventArgClickSkill = new EventArgClickSkill();
			eventArgClickSkill.SetData(this.skillItem);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ClickSkill, eventArgClickSkill);
		}

		public GameObject child;

		public GameObject emptyStarLayout;

		public GameObject starLayout;

		public GameObject starItemObj;

		public CustomButton buttonSelf;

		public UIGameEventSkillItem skillItem;

		private GameEventSkillBuildData data;

		private List<CustomImage> emptyStarList = new List<CustomImage>();

		private List<CustomImage> starList = new List<CustomImage>();

		private Sequence m_seq;
	}
}
