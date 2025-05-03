using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class SkillUpgradePreviewViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.copyStarItem.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			SkillUpgradePreviewViewModule.OpenData openData = data as SkillUpgradePreviewViewModule.OpenData;
			if (openData != null)
			{
				this.openData = openData;
			}
			if (this.openData == null)
			{
				return;
			}
			this.textCurrentTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiupgradepreview_current");
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.openData.currentSkillId);
			GameSkill_skill elementById2 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.openData.nextSkillId);
			if (elementById != null)
			{
				this.textCurrentSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
				this.textCurrentSkillDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.fullDetailID);
			}
			if (elementById2 != null)
			{
				this.textNextSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID);
				this.textNextSkillDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.fullDetailID);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uiupgradepreview_next");
			List<string> listString = infoByID.GetListString('@');
			if (listString.Count >= 2)
			{
				this.textUnlock1.text = listString[0];
				this.textUnlock2.text = listString[1];
			}
			else if (listString.Count == 1)
			{
				if (infoByID.ToCharArray()[0].Equals('@'))
				{
					this.textUnlock1.text = "";
					this.textUnlock2.text = listString[0];
				}
				else
				{
					this.textUnlock1.text = listString[0];
					this.textUnlock2.text = "";
				}
			}
			for (int i = 0; i < this.openData.star; i++)
			{
				UIStarItem uistarItem;
				if (i < this.starItems.Count)
				{
					uistarItem = this.starItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyStarItem);
					gameObject.SetParentNormal(this.starLayout, false);
					uistarItem = gameObject.GetComponent<UIStarItem>();
					uistarItem.Init();
				}
				uistarItem.gameObject.SetActiveSafe(true);
				uistarItem.SetData(true);
				this.starItems.Add(uistarItem);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			for (int i = 0; i < this.starItems.Count; i++)
			{
				this.starItems[i].gameObject.SetActiveSafe(false);
			}
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.starItems.Count; i++)
			{
				this.starItems[i].DeInit();
			}
			this.starItems.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.SkillUpgradePreviewViewModule, null);
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textCurrentTitle;

		public CustomText textCurrentSkillName;

		public CustomText textCurrentSkillDes;

		public CustomText textUnlock1;

		public CustomText textUnlock2;

		public GameObject starLayout;

		public GameObject copyStarItem;

		public CustomText textNextSkillName;

		public CustomText textNextSkillDes;

		private SkillUpgradePreviewViewModule.OpenData openData;

		private List<UIStarItem> starItems = new List<UIStarItem>();

		public class OpenData
		{
			public int currentSkillId;

			public int nextSkillId;

			public int star;
		}
	}
}
