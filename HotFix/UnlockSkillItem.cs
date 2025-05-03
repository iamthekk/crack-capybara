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
	public class UnlockSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.skillItem.Init();
			this.starLayout.SetActiveSafe(false);
			if (this.buttonClickSelf)
			{
				this.buttonClickSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.stars.Clear();
			if (this.buttonClickSelf)
			{
				this.buttonClickSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			}
		}

		public void SetData(Action onClick)
		{
			this.onClickSelf = onClick;
		}

		public void Refresh(GameEventSkillBuildData data)
		{
			if (data == null)
			{
				return;
			}
			this.skillBuildData = data;
			string atlasPath = GameApp.Table.GetAtlasPath(101);
			this.imageBg.SetImage(atlasPath, GameEventSkillBuildData.GetQualityDetailBg(this.skillBuildData.quality));
			this.imageConner.SetImage(atlasPath, UIGameEventSelectSkillItem.GetConnerBG(data.quality));
			this.imageFlower.color = UIGameEventSelectSkillItem.GetFlowerColor(data.quality);
			this.textName.text = GameEventSkillBuildData.GetQualityColor(this.skillBuildData.quality, this.skillBuildData.skillName);
			this.textInfo.text = data.skillFullDetail;
			this.skillItem.Refresh(this.skillBuildData, false, true);
			this.imageConner.gameObject.SetActiveSafe(false);
			this.imageFlower.gameObject.SetActiveSafe(data.level > 1);
		}

		private void OnClickSelf()
		{
			Action action = this.onClickSelf;
			if (action == null)
			{
				return;
			}
			action();
		}

		public CustomText textName;

		public CustomImage imageBg;

		public CustomText textInfo;

		public GameObject starLayout;

		public UIGameEventSkillItem skillItem;

		public Image imageFlower;

		public CustomImage imageConner;

		public CustomButton buttonClickSelf;

		private GameEventSkillBuildData skillBuildData;

		private List<GameObject> stars = new List<GameObject>();

		private Action onClickSelf;
	}
}
