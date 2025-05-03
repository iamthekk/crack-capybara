using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class SpecialChallengesViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.copyItem.SetActiveSafe(false);
			this.tapCloseCtrl.OnClose = new Action(this.OnClickClose);
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			SpecialChallengesViewModule.OpenData openData = data as SpecialChallengesViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null || this.mOpenData.monsterEntryIds == null)
			{
				this.OnClickClose();
				return;
			}
			GameApp.Sound.PlayClip(683, 1f);
			this.popAni.Clear();
			for (int i = 0; i < this.mOpenData.monsterEntryIds.Count; i++)
			{
				int num = this.mOpenData.monsterEntryIds[i];
				if (this.mOpenData.source == SpecialChallengesViewModule.Source.RogueDungeon)
				{
					RogueDungeon_monsterEntry rogueDungeon_monsterEntry = GameApp.Table.GetManager().GetRogueDungeon_monsterEntry(num);
					if (rogueDungeon_monsterEntry != null)
					{
						UISpecialChallengeItem uispecialChallengeItem;
						if (i < this.challengeItems.Count)
						{
							uispecialChallengeItem = this.challengeItems[i];
						}
						else
						{
							GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
							gameObject.SetParentNormal(this.skillParent, false);
							uispecialChallengeItem = gameObject.GetComponent<UISpecialChallengeItem>();
							uispecialChallengeItem.Init();
							this.challengeItems.Add(uispecialChallengeItem);
						}
						this.popAni.AddData(new PopAnimationSequence.Data
						{
							transform = uispecialChallengeItem.transform
						});
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(rogueDungeon_monsterEntry.nameId);
						string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(rogueDungeon_monsterEntry.desId);
						uispecialChallengeItem.SetData(infoByID, infoByID2);
						uispecialChallengeItem.gameObject.SetActiveSafe(true);
					}
				}
			}
			this.popAni.AddData(new PopAnimationSequence.Data
			{
				transform = this.tapCloseCtrl.transform
			});
			this.popAni.Play();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.tapCloseCtrl.OnClose = null;
			for (int i = 0; i < this.challengeItems.Count; i++)
			{
				this.challengeItems[i].DeInit();
			}
			this.challengeItems.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.SpecialChallengesViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_SpecialChallenge_Close, null);
		}

		public GameObject skillParent;

		public GameObject copyItem;

		public TapToCloseCtrl tapCloseCtrl;

		public PopAnimationSequence popAni;

		private List<UISpecialChallengeItem> challengeItems = new List<UISpecialChallengeItem>();

		private SpecialChallengesViewModule.OpenData mOpenData;

		public enum Source
		{
			RogueDungeon = 1
		}

		public class OpenData
		{
			public List<int> monsterEntryIds;

			public SpecialChallengesViewModule.Source source;
		}
	}
}
