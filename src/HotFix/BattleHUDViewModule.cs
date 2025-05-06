using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.State;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class BattleHUDViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.legacySkillPowerItem.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.m_comboController = new ComboController();
			this.m_comboController.SetGameObject(this.comboNode.gameObject);
			this.m_comboController.Init();
			this.textRound.text = "";
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.cacheBuffDic.Count > 0)
			{
				foreach (List<BaseHover> list in this.cacheBuffDic.Values)
				{
					if (list.Count > 0)
					{
						HoverBuffName hoverBuffName = list[0] as HoverBuffName;
						if (!hoverBuffName.IsPlayed)
						{
							hoverBuffName.Play();
						}
					}
				}
			}
		}

		private void LateUpdate()
		{
			for (int i = 0; i < this.hoverList.Count; i++)
			{
				BaseHover baseHover = this.hoverList[i];
				if (baseHover == null)
				{
					return;
				}
				baseHover.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.m_comboController.DeInit();
			foreach (LegacySkillPowerItem legacySkillPowerItem in this.m_skillPowerItemDic.Values)
			{
				Object.Destroy(legacySkillPowerItem.gameObject);
			}
			this.m_skillPowerItemDic.Clear();
		}

		public override void OnDelete()
		{
			this.RemoveAll();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_StartRun, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEventBattleEnd));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_BindCamera, new HandlerEvent(this.OnBindCamera));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_AddHover, new HandlerEvent(this.OnEventAddHover));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHover, new HandlerEvent(this.OnEventRemoveHover));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHoverByID, new HandlerEvent(this.OnEventRemoveHoverByID));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHoverByType, new HandlerEvent(this.OnEventRemoveHoverByType));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RemoveAllHover, new HandlerEvent(this.OnEventRemoveAllHover));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshCombo, new HandlerEvent(this.OnRefreshCombo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_RefreshLegacyPower, new HandlerEvent(this.OnRefreshLegacyPower));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_StartRun, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEventBattleEnd));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_BindCamera, new HandlerEvent(this.OnBindCamera));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_AddHover, new HandlerEvent(this.OnEventAddHover));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHover, new HandlerEvent(this.OnEventRemoveHover));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHoverByID, new HandlerEvent(this.OnEventRemoveHoverByID));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RemoveHoverByType, new HandlerEvent(this.OnEventRemoveHoverByType));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RemoveAllHover, new HandlerEvent(this.OnEventRemoveAllHover));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshCombo, new HandlerEvent(this.OnRefreshCombo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_RefreshLegacyPower, new HandlerEvent(this.OnRefreshLegacyPower));
		}

		private void OnEventBattleStart(object sender, int type, BaseEventArgs args)
		{
			ComboController comboController = this.m_comboController;
			if (comboController != null)
			{
				comboController.Show(true);
			}
			EventArgsBattleStart eventArgsBattleStart = args as EventArgsBattleStart;
			if (eventArgsBattleStart != null)
			{
				this.CreateLegacySkillPowerItems(eventArgsBattleStart.data);
			}
		}

		private void OnEventBattleEnd(object sender, int type, BaseEventArgs args)
		{
			ComboController comboController = this.m_comboController;
			if (comboController != null)
			{
				comboController.Show(false);
			}
			this.RemoveLegacyPowerItems();
		}

		private void RemoveLegacyPowerItems()
		{
			foreach (LegacySkillPowerItem legacySkillPowerItem in this.m_skillPowerItemDic.Values)
			{
				if (legacySkillPowerItem != null)
				{
					Object.Destroy(legacySkillPowerItem.gameObject);
				}
			}
			this.m_skillPowerItemDic.Clear();
		}

		private void CreateLegacySkillPowerItems(BattleReportData_GameStart report)
		{
			State currentState = GameApp.State.GetCurrentState();
			if (currentState != null)
			{
				if (currentState is BattleChapterState || currentState is BattleRogueDungeonState)
				{
					this.legacyPowerNode.anchoredPosition = new Vector2(5f, -127f);
				}
				else if (currentState is BattleCrossArenaState || currentState is BattleTowerState || currentState is BattleWorldBossState || currentState is BattleDungeonState || currentState is BattleTestState)
				{
					this.legacyPowerNode.anchoredPosition = new Vector2(5f, -180f);
				}
				else if (currentState is BattleGuildBossState)
				{
					this.legacyPowerNode.anchoredPosition = new Vector2(5f, -330f);
				}
				else
				{
					this.legacyPowerNode.anchoredPosition = new Vector2(5f, -180f);
					HLog.LogError("注意：此战斗类型需要做传承技能位置显示调整");
				}
			}
			if (report == null)
			{
				return;
			}
			for (int i = 0; i < report.m_members.Count; i++)
			{
				GameStartMemberData gameStartMemberData = report.m_members[i];
				if (gameStartMemberData != null && gameStartMemberData.m_instanceId == 100)
				{
					List<int> skillIds = gameStartMemberData.m_skillIds;
					Dictionary<int, FP> maxLegacyPower = gameStartMemberData.m_maxLegacyPower;
					Dictionary<int, FP> curLegacyPower = gameStartMemberData.m_curLegacyPower;
					List<int> list = maxLegacyPower.Keys.ToList<int>();
					list.Sort(delegate(int a, int b)
					{
						int num2 = skillIds.IndexOf(a);
						return skillIds.IndexOf(b) - num2;
					});
					for (int j = 0; j < list.Count; j++)
					{
						int num = list[j];
						FP fp;
						curLegacyPower.TryGetValue(num, out fp);
						FP fp2;
						maxLegacyPower.TryGetValue(num, out fp2);
						LegacySkillPowerItem legacySkillPowerItem;
						if (!this.m_skillPowerItemDic.TryGetValue(num, out legacySkillPowerItem))
						{
							legacySkillPowerItem = Object.Instantiate<LegacySkillPowerItem>(this.legacySkillPowerItem, this.legacySkillPowerItem.transform.parent, false);
							legacySkillPowerItem.SetActive(true);
							legacySkillPowerItem.Init();
							this.m_skillPowerItemDic[num] = legacySkillPowerItem;
						}
						legacySkillPowerItem.SetLegacySkill(num);
						legacySkillPowerItem.UpdateProgress(fp.AsLong(), fp2.AsLong());
					}
					return;
				}
			}
		}

		private async Task AddHover(HoverType type, HoverData hoverData, object data)
		{
			GameMember_gameHover elementById = GameApp.Table.GetManager().GetGameMember_gameHoverModelInstance().GetElementById((int)type);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Not found hover id={0}", (int)type));
			}
			else
			{
				Transform target = hoverData.target;
				ArtHover_hover artTable = GameApp.Table.GetManager().GetArtHover_hoverModelInstance().GetElementById(elementById.prefabID);
				if (artTable == null)
				{
					HLog.LogError(string.Format("$Not found hoverArt id={0}", elementById.prefabID));
				}
				else
				{
					await PoolManager.Instance.CheckPrefab(artTable.path);
					Transform parentNode = this.GetParentNode(type);
					GameObject gameObject = PoolManager.Instance.Out(artTable.path, new Vector3(100000f, 0f, 0f), Quaternion.identity, parentNode);
					gameObject.transform.SetParent(parentNode);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.rotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					BaseHover component = gameObject.GetComponent<BaseHover>();
					this.hoverList.Add(component);
					if (type == HoverType.BuffName)
					{
						if (this.cacheBuffDic.ContainsKey(hoverData.id))
						{
							this.cacheBuffDic[hoverData.id].Add(component);
						}
						else
						{
							List<BaseHover> list = new List<BaseHover>();
							list.Add(component);
							this.cacheBuffDic.Add(hoverData.id, list);
						}
					}
					component.SetHoverData(data);
					component.Init(this.camera3D, target, hoverData.id);
				}
			}
		}

		private async Task RemoveHover(BaseHover hover)
		{
			hover.DeInit();
			this.RemoveCache(hover);
			this.hoverList.Remove(hover);
			PoolManager.Instance.Put(hover.gameObject);
			await Task.CompletedTask;
		}

		private async Task RemoveHover(int ownerId)
		{
			List<BaseHover> removeList = new List<BaseHover>();
			for (int j = 0; j < this.hoverList.Count; j++)
			{
				BaseHover baseHover = this.hoverList[j];
				if (baseHover.ownerId == ownerId)
				{
					removeList.Add(baseHover);
				}
			}
			for (int i = 0; i < removeList.Count; i++)
			{
				await this.RemoveHover(removeList[i]);
			}
			removeList.Clear();
		}

		private async Task RemoveHover(HoverType type)
		{
			List<BaseHover> removeList = new List<BaseHover>();
			for (int j = 0; j < this.hoverList.Count; j++)
			{
				BaseHover baseHover = this.hoverList[j];
				if (baseHover.GetHoverType() == type)
				{
					removeList.Add(baseHover);
				}
			}
			for (int i = 0; i < removeList.Count; i++)
			{
				await this.RemoveHover(removeList[i]);
			}
			removeList.Clear();
		}

		private async Task RemoveAll()
		{
			for (int i = 0; i < this.hoverList.Count; i++)
			{
				BaseHover baseHover = this.hoverList[i];
				baseHover.DeInit();
				this.RemoveCache(baseHover);
				PoolManager.Instance.Put(baseHover.gameObject);
			}
			this.hoverList.Clear();
			this.cacheBuffDic.Clear();
			await Task.CompletedTask;
		}

		private void RemoveCache(BaseHover hover)
		{
			if (this.cacheBuffDic.ContainsKey(hover.ownerId))
			{
				this.cacheBuffDic[hover.ownerId].Remove(hover);
				if (this.cacheBuffDic[hover.ownerId].Count <= 0)
				{
					this.cacheBuffDic.Remove(hover.ownerId);
				}
			}
		}

		private Transform GetParentNode(HoverType type)
		{
			Transform transform;
			switch (type)
			{
			case HoverType.FriendlyStateBar:
			case HoverType.EnemyStateBar:
			case HoverType.BattleSign:
				transform = this.hpNode;
				break;
			case HoverType.LessHp:
			case HoverType.LessHpCrit:
			case HoverType.LessHpBigSkill:
			case HoverType.PlusHp:
			case HoverType.BattleText:
			case HoverType.BigSkillName:
			case HoverType.NormalSkillName:
			case HoverType.LessHpIce:
			case HoverType.LessHpFire:
			case HoverType.LessHpThunder:
			case HoverType.LessHpPoison:
				transform = this.damageNode;
				break;
			case HoverType.BuffName:
				transform = this.buffNode;
				break;
			default:
				transform = this.damageNode;
				break;
			}
			return transform;
		}

		private void OnBindCamera(object sender, int type, BaseEventArgs args)
		{
			EventArgsBindCamera eventArgsBindCamera = args as EventArgsBindCamera;
			if (eventArgsBindCamera == null)
			{
				return;
			}
			this.camera3D = eventArgsBindCamera.m_camera;
		}

		private void OnEventAddHover(object sender, int type, BaseEventArgs args)
		{
			EventArgsAddHover eventArgsAddHover = args as EventArgsAddHover;
			if (eventArgsAddHover == null)
			{
				return;
			}
			this.AddHover(eventArgsAddHover.type, eventArgsAddHover.targetData, eventArgsAddHover.hoverData);
		}

		private void OnEventRemoveHover(object sender, int type, BaseEventArgs args)
		{
			EventArgsRemoveHover eventArgsRemoveHover = args as EventArgsRemoveHover;
			if (eventArgsRemoveHover == null)
			{
				return;
			}
			this.RemoveHover(eventArgsRemoveHover.hover);
		}

		private void OnEventRemoveHoverByID(object sender, int type, BaseEventArgs args)
		{
			EventArgsRemoveHoverByID eventArgsRemoveHoverByID = args as EventArgsRemoveHoverByID;
			if (eventArgsRemoveHoverByID == null)
			{
				return;
			}
			this.RemoveHover(eventArgsRemoveHoverByID.instanceId);
		}

		private void OnEventRemoveHoverByType(object sender, int type, BaseEventArgs args)
		{
			EventArgsRemoveHoverByType eventArgsRemoveHoverByType = args as EventArgsRemoveHoverByType;
			if (eventArgsRemoveHoverByType == null)
			{
				return;
			}
			this.RemoveHover(eventArgsRemoveHoverByType.type);
		}

		private void OnEventRemoveAllHover(object sender, int type, BaseEventArgs args)
		{
			this.RemoveAll();
		}

		private void OnRefreshCombo(object sender, int type, BaseEventArgs args)
		{
			EventArgsInt eventArgsInt = args as EventArgsInt;
			this.m_comboController.SetCombo(eventArgsInt.Value);
		}

		private void OnRefreshLegacyPower(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshLegacyPower eventArgsRefreshLegacyPower = args as EventArgsRefreshLegacyPower;
			if (eventArgsRefreshLegacyPower == null)
			{
				return;
			}
			if (eventArgsRefreshLegacyPower.memberInstanceId != 100)
			{
				return;
			}
			int skillId = eventArgsRefreshLegacyPower.skillId;
			LegacySkillPowerItem legacySkillPowerItem;
			if (this.m_skillPowerItemDic.TryGetValue(skillId, out legacySkillPowerItem))
			{
				legacySkillPowerItem.UpdateProgress(eventArgsRefreshLegacyPower.current, eventArgsRefreshLegacyPower.max);
			}
		}

		private void OnRoundStartHandler(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			this.textRound.text = string.Format("{0}/{1}", eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound);
		}

		public void HideRoundText()
		{
			this.textRound.gameObject.SetActiveSafe(false);
		}

		[SerializeField]
		private Transform hpNode;

		[SerializeField]
		private Transform damageNode;

		[SerializeField]
		private Transform buffNode;

		[SerializeField]
		private Transform comboNode;

		[SerializeField]
		private CustomText textRound;

		[SerializeField]
		private RectTransform legacyPowerNode;

		[SerializeField]
		private LegacySkillPowerItem legacySkillPowerItem;

		private Camera camera3D;

		private List<BaseHover> hoverList = new List<BaseHover>();

		private ComboController m_comboController;

		private Dictionary<int, LegacySkillPowerItem> m_skillPowerItemDic = new Dictionary<int, LegacySkillPowerItem>();

		private Dictionary<int, List<BaseHover>> cacheBuffDic = new Dictionary<int, List<BaseHover>>();
	}
}
