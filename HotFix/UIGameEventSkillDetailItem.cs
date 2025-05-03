using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIGameEventSkillDetailItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			ComponentRegister component = base.gameObject.GetComponent<ComponentRegister>();
			this.textName = component.GetGameObject("TextName").GetComponent<CustomText>();
			this.skillItemObj = component.GetGameObject("SkillItem");
			this.child = component.GetGameObject("Child").GetComponent<RectTransform>();
			this.animator = component.GetGameObject("Ani").GetComponent<Animator>();
			this.imageBg = component.GetGameObject("ImageBg").GetComponent<CustomImage>();
			this.fxGolden = component.GetGameObject("EffectGolden").GetComponent<ParticleSystem>();
			this.fxPurple = component.GetGameObject("EffectPurple").GetComponent<ParticleSystem>();
			this.animatorListen = component.GetGameObject("Listen").GetComponent<AnimatorListen>();
			this.starNode = component.GetGameObject("Star");
			this.emptyStarLayout = component.GetGameObject("EmptyLayout");
			this.starLayout = component.GetGameObject("StarLayout");
			this.starItemObj = component.GetGameObject("StarItem");
			this.textLayout = component.GetGameObject("TextLayout").GetComponent<RectTransform>();
			this.cloneNode = component.GetGameObject("CloneNode");
			this.textInfoNode = component.GetGameObject("TextInfoNode");
			this.triangleNode = component.GetGameObject("TriangleNode");
			this.buttonSelf = component.GetGameObject("ButtonSelf").GetComponent<CustomButton>();
			this.imageBg.transform.localScale = Vector3.zero;
			this.skillItemObj.transform.localScale = Vector3.zero;
			this.fxGolden.gameObject.SetActiveSafe(false);
			this.fxPurple.gameObject.SetActiveSafe(false);
			this.starItemObj.SetActiveSafe(false);
			this.cloneNode.SetActiveSafe(false);
			this.animatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			this.skillItem = this.skillItemObj.GetComponent<UIGameEventSkillItem>();
			this.skillItem.Init();
		}

		protected override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.currentFx.Clear();
			this.emptyStarList.Clear();
			this.starList.Clear();
		}

		public void Refresh(GameEventSkillBuildData skillBuildData, Action<GameEventSkillBuildData> onAction)
		{
			if (skillBuildData == null)
			{
				return;
			}
			this.data = skillBuildData;
			this.onClickSelf = onAction;
			string atlasPath = GameApp.Table.GetAtlasPath(101);
			this.imageBg.SetImage(atlasPath, GameEventSkillBuildData.GetQualityDetailBg(skillBuildData.quality));
			this.textName.text = GameEventSkillBuildData.GetQualityColor(skillBuildData.quality, skillBuildData.skillName);
			this.skillItem.Refresh(skillBuildData, false, true);
			if (this.textInfo == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.textInfoNode);
				gameObject.transform.SetParentNormal(this.textLayout, false);
				this.textInfo = gameObject.GetComponent<CustomText>();
				this.textInfo.text = this.data.skillDetailInfo;
				if (!string.IsNullOrEmpty(this.data.skillInfo))
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.triangleNode);
					gameObject2.transform.SetParentNormal(this.textLayout, false);
					ComponentRegister component = gameObject2.GetComponent<ComponentRegister>();
					this.textUpgradeInfo = component.GetGameObject("Text").GetComponent<CustomText>();
					this.textUpgradeInfo.text = this.data.skillInfo;
				}
			}
			else
			{
				this.textInfo.text = this.data.skillDetailInfo;
				if (this.textUpgradeInfo != null && !string.IsNullOrEmpty(this.data.skillInfo))
				{
					this.textUpgradeInfo.text = this.data.skillInfo;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.textLayout);
			SkillBuildQuality quality = skillBuildData.quality;
			if (quality != SkillBuildQuality.Gold)
			{
				if (quality != SkillBuildQuality.Red)
				{
					this.currentFx = null;
				}
				else
				{
					this.currentFx = this.fxPurple;
				}
			}
			else
			{
				this.currentFx = this.fxGolden;
			}
			this.RefreshStar();
		}

		private void RefreshStar()
		{
			int num = 1;
			string atlasPath = GameApp.Table.GetAtlasPath(101);
			for (int i = 0; i < num; i++)
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
				ImagePingPong component = customImage.GetComponent<ImagePingPong>();
				if (component)
				{
					component.OnOff = false;
				}
				customImage.SetImage(atlasPath, "star1");
				customImage.gameObject.SetActiveSafe(true);
			}
			for (int j = 0; j < num; j++)
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
				ImagePingPong component2 = customImage2.GetComponent<ImagePingPong>();
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
				if (component2 != null && j == this.data.level - 1)
				{
					component2.OnOff = true;
				}
			}
		}

		public Sequence PlayOpenBoxAni()
		{
			Sequence sequence = DOTween.Sequence();
			this.skillItemObj.transform.localScale = Vector3.zero;
			this.imageBg.transform.localScale = new Vector3(0f, 1f, 1f);
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillItemObj.transform, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScaleX(this.imageBg.transform, 1f, num));
			return sequence;
		}

		public void SetAnimatorEnable(bool isEnable)
		{
			if (this.animator != null)
			{
				this.animator.enabled = isEnable;
			}
		}

		public void PlayBGFx(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			ps.gameObject.SetActiveSafe(true);
			ps.Play();
		}

		public void StopBGFx(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			ps.gameObject.SetActiveSafe(true);
			ps.Play();
		}

		private void OnAnimationListen(GameObject obj, string param)
		{
			if (param.Equals("Finish"))
			{
				this.PlayBGFx(this.currentFx);
			}
		}

		public Vector3 GetSkillItemPos()
		{
			return this.skillItemObj.transform.position;
		}

		private void OnClickSelf()
		{
			Action<GameEventSkillBuildData> action = this.onClickSelf;
			if (action == null)
			{
				return;
			}
			action(this.data);
		}

		public void OnHide()
		{
			this.fxGolden.gameObject.SetActiveSafe(false);
			this.fxPurple.gameObject.SetActiveSafe(false);
		}

		private CustomText textName;

		private GameObject skillItemObj;

		private RectTransform child;

		private Animator animator;

		private CustomImage imageBg;

		private ParticleSystem fxGolden;

		private ParticleSystem fxPurple;

		private AnimatorListen animatorListen;

		private GameObject starNode;

		private GameObject emptyStarLayout;

		private GameObject starLayout;

		private GameObject starItemObj;

		private RectTransform textLayout;

		private GameObject cloneNode;

		private GameObject textInfoNode;

		private GameObject triangleNode;

		private CustomButton buttonSelf;

		private UIGameEventSkillItem skillItem;

		private ParticleSystem currentFx;

		private List<CustomImage> emptyStarList = new List<CustomImage>();

		private List<CustomImage> starList = new List<CustomImage>();

		private GameEventSkillBuildData data;

		private CustomText textInfo;

		private CustomText textUpgradeInfo;

		private Action<GameEventSkillBuildData> onClickSelf;
	}
}
