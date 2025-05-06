using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIGameEventSelectSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.starItemObj.SetActiveSafe(false);
			this.skillIconItemObj.SetActiveSafe(false);
			this.cloneNode.SetActiveSafe(false);
			this.effRed.gameObject.SetActiveSafe(false);
			this.effGold.gameObject.SetActiveSafe(false);
			this.skillItem.Init();
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
			GameObject gameObject = Object.Instantiate<GameObject>(this.textInfoNode);
			gameObject.transform.SetParentNormal(this.textLayout, false);
			this.textInfo = gameObject.GetComponent<CustomText>();
			this.triangleObj = Object.Instantiate<GameObject>(this.triangleNode);
			this.triangleObj.transform.SetParentNormal(this.textLayout, false);
			ComponentRegister component = this.triangleObj.GetComponent<ComponentRegister>();
			this.textUpgradeInfo = component.GetGameObject("Text").GetComponent<CustomText>();
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			this.data = null;
			this.onSelectSkill = null;
			this.skillItem.DeInit();
			this.skillItem = null;
			for (int i = 0; i < this.iconItemList.Count; i++)
			{
				this.iconItemList[i].DeInit();
			}
			this.iconItemList.Clear();
			this.emptyStarList.Clear();
			this.starList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void Refresh(GameEventSkillBuildData data, Action<GameEventSkillBuildData> onSelect)
		{
			if (data == null)
			{
				return;
			}
			this.data = data;
			this.skillItem.Refresh(data, true, false);
			this.skillItem.ShowQualityTag(true);
			string atlasPath = GameApp.Table.GetAtlasPath(101);
			this.imageBg.SetImage(atlasPath, GameEventSkillBuildData.GetQualityBg(data.quality));
			this.imageConner.SetImage(atlasPath, UIGameEventSelectSkillItem.GetConnerBG(data.quality));
			this.imageFlower.color = UIGameEventSelectSkillItem.GetFlowerColor(data.quality);
			this.textName.text = data.skillName;
			this.imageConner.gameObject.SetActiveSafe(false);
			if (data.level > 1)
			{
				this.imageFlower.gameObject.SetActiveSafe(true);
				Color color;
				if (ColorUtility.TryParseHtmlString("#D3F24E", ref color))
				{
					this.textName.color = color;
				}
			}
			else
			{
				this.imageFlower.gameObject.SetActiveSafe(false);
				this.textName.color = Color.white;
			}
			this.onSelectSkill = onSelect;
			this.newObj.SetActiveSafe(false);
			this.textInfo.text = data.skillDetailInfo;
			this.textUpgradeInfo.text = data.skillInfo;
			this.triangleObj.SetActiveSafe(!string.IsNullOrEmpty(data.skillInfo));
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.textLayout);
			this.starNode.SetActiveSafe(false);
			this.combinationNode.SetActiveSafe(false);
		}

		private void OnClickButton()
		{
			if (this.data == null)
			{
				return;
			}
			Action<GameEventSkillBuildData> action = this.onSelectSkill;
			if (action == null)
			{
				return;
			}
			action(this.data);
		}

		public static string GetConnerBG(SkillBuildQuality quality)
		{
			if (quality == SkillBuildQuality.Gold)
			{
				return "skill_bg_conner_2";
			}
			if (quality != SkillBuildQuality.Red)
			{
				return "skill_bg_conner_1";
			}
			return "skill_bg_conner_3";
		}

		public static Color GetFlowerColor(SkillBuildQuality quality)
		{
			Color white = Color.white;
			if (quality == SkillBuildQuality.Gold)
			{
				ColorUtility.TryParseHtmlString("#D7742B", ref white);
				return white;
			}
			if (quality != SkillBuildQuality.Red)
			{
				ColorUtility.TryParseHtmlString("#8e756b", ref white);
				return white;
			}
			ColorUtility.TryParseHtmlString("#B53649", ref white);
			return white;
		}

		public void PlayEffect()
		{
			if (this.data != null)
			{
				this.PlayEffect(this.data.quality);
			}
		}

		public void PlayEffect(SkillBuildQuality quality)
		{
			ParticleSystem particleSystem = null;
			if (quality != SkillBuildQuality.Gold)
			{
				if (quality == SkillBuildQuality.Red)
				{
					particleSystem = this.effRed;
				}
			}
			else
			{
				particleSystem = this.effGold;
			}
			if (particleSystem != null)
			{
				particleSystem.gameObject.SetActiveSafe(true);
				particleSystem.Play();
			}
		}

		public UIGameEventSkillItem skillItem;

		public CustomButton button;

		public CustomText textName;

		public GameObject newObj;

		public GameObject starNode;

		public GameObject starItemObj;

		public GameObject combinationNode;

		public GameObject skillIconItemObj;

		public CustomImage imageBg;

		public RectTransform textLayout;

		public GameObject cloneNode;

		public GameObject textInfoNode;

		public GameObject triangleNode;

		public Image imageFlower;

		public CustomImage imageConner;

		public ParticleSystem effRed;

		public ParticleSystem effGold;

		private GameEventSkillBuildData data;

		private Action<GameEventSkillBuildData> onSelectSkill;

		private List<UIGameEventSkillIconItem> iconItemList = new List<UIGameEventSkillIconItem>();

		private List<CustomImage> emptyStarList = new List<CustomImage>();

		private List<CustomImage> starList = new List<CustomImage>();

		private CustomText textInfo;

		private CustomText textUpgradeInfo;

		private GameObject triangleObj;

		private const string Skill_Upgrade_Color = "#D3F24E";

		private const string BG_Flower_Color_Gray = "#8e756b";

		private const string BG_Flower_Color_Gold = "#D7742B";

		private const string BG_Flower_Color_Red = "#B53649";
	}
}
