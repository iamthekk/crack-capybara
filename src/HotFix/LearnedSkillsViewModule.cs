using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class LearnedSkillsViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 946;
		}

		public override void OnCreate(object data)
		{
			this.skillObj.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnEventClickSkill));
			this.m_tapToCloseCtrl.OnClose = delegate
			{
				GameApp.View.CloseView(ViewName.LearnedSkillsViewModule, null);
			};
			List<GameEventSkillBuildData> list = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkillBuildList();
			this.textNoSkill.SetActive(list.Count == 0);
			for (int i = 0; i < this.skillItemList.Count; i++)
			{
				this.skillItemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				int index = j;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)j * 0.05f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					UIGameEventLearnedSkillItem uigameEventLearnedSkillItem;
					if (index < this.skillItemList.Count)
					{
						uigameEventLearnedSkillItem = this.skillItemList[index];
						uigameEventLearnedSkillItem.DeInit();
						uigameEventLearnedSkillItem.Init();
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.skillObj);
						gameObject.transform.SetParentNormal(this.gridLayoutGroup.transform, false);
						uigameEventLearnedSkillItem = gameObject.GetComponent<UIGameEventLearnedSkillItem>();
						uigameEventLearnedSkillItem.Init();
						this.skillItemList.Add(uigameEventLearnedSkillItem);
					}
					uigameEventLearnedSkillItem.gameObject.SetActiveSafe(true);
					uigameEventLearnedSkillItem.Refresh(list[index]);
					uigameEventLearnedSkillItem.PlayShowAnimation();
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnEventClickSkill));
			this.m_seqPool.Clear(false);
			for (int i = 0; i < this.skillItemList.Count; i++)
			{
				this.skillItemList[i].DeInit();
			}
		}

		public override void OnDelete()
		{
			this.skillItemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnEventClickSkill(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgClickSkill eventArgClickSkill = eventargs as EventArgClickSkill;
			if (eventArgClickSkill == null)
			{
				return;
			}
			GameEventSkillBuildData skillBuildData = eventArgClickSkill.skillItem.GetSkillBuildData();
			new InfoTipViewModule.InfoTipData
			{
				m_name = skillBuildData.skillName,
				m_info = skillBuildData.skillFullDetail,
				m_position = eventArgClickSkill.skillItem.GetPosition(),
				m_offsetY = 280f
			}.Open();
		}

		public GridLayoutGroup gridLayoutGroup;

		public GameObject skillObj;

		public GameObject textNoSkill;

		public TapToCloseCtrl m_tapToCloseCtrl;

		private List<UIGameEventLearnedSkillItem> skillItemList = new List<UIGameEventLearnedSkillItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
