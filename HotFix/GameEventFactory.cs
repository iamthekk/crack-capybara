using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Server;
using XNode;
using XNode.GameEvent;

namespace HotFix
{
	public class GameEventFactory
	{
		public void Init(int endStage, GameEventPoolDataFactory factory)
		{
			this.totalStage = endStage;
			this.poolDataFactory = factory;
			this.EnterCurrentStage();
		}

		public void DeInit()
		{
			if (this.eventGroup != null)
			{
				this.eventGroup.DeInit();
			}
			this.rootData = null;
			this.eventGroup = null;
			this.currentStage = 0;
		}

		public int GetCurrentStage()
		{
			return this.currentStage;
		}

		public int GetCurrentEventIndex()
		{
			return this.eventRealIndex;
		}

		public XRandom GetGroupRandom()
		{
			if (this.eventGroup != null)
			{
				return this.eventGroup.GetRandom();
			}
			return null;
		}

		public async void EnterCurrentStage()
		{
			TaskOutValue<GameEventData> outData = new TaskOutValue<GameEventData>();
			await this.GetStageData(outData);
			this.rootData = outData.Value;
			if (this.rootData != null && this.rootData.poolData != null)
			{
				if (this.lastEventType != GameEventType.Fishing && this.rootData.poolData.eventType == GameEventType.Fishing)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_Ready, null);
				}
				if (this.lastEventType == GameEventType.Fishing && this.rootData.poolData.eventType != GameEventType.Fishing)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_AllFinish, null);
				}
				this.eventGroup = new GameEventGroup();
				this.eventGroup.Init(this.rootData, this.currentStage, new Action(this.OnEventGroupEnd));
				this.lastEventType = this.rootData.poolData.eventType;
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData(this.currentStage);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EnterNewStage, eventArgsInt);
			}
		}

		private void OnEventGroupEnd()
		{
			if (this.eventGroup != null)
			{
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData(this.currentStage);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventGroup_End, eventArgsInt);
				if (this.rootData.poolData.eventType == GameEventType.Fishing)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_Exit, null);
				}
				this.eventGroup.DeInit();
				this.eventGroup = null;
				if (this.currentStage == this.totalStage && Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterSweep_End, null);
					return;
				}
				Singleton<EventRecordController>.Instance.EventGroupEnd();
				this.EnterCurrentStage();
			}
		}

		public void ContinueEvent()
		{
			if (this.isRefreshScene)
			{
				this.isRefreshScene = false;
				this.EnterCurrentStage();
			}
		}

		public void PushEvent(GameEventPushType pushType, object param)
		{
			if (this.eventGroup != null)
			{
				this.eventGroup.PushEvent(pushType, param);
			}
		}

		public void HangUp()
		{
			if (this.eventGroup != null)
			{
				this.eventGroup.HangUp();
			}
		}

		public void ResumeHangUp()
		{
			if (this.eventGroup != null)
			{
				this.eventGroup.ResumeHangUp();
			}
		}

		public bool IsCurrentEventDone()
		{
			return this.eventGroup != null && this.eventGroup.IsCurrentEventDone();
		}

		private async Task GetStageData(TaskOutValue<GameEventData> outData)
		{
			if (outData == null)
			{
				HLog.LogError("GameEventFactory.GetStageData: Task out value is null");
			}
			else
			{
				GameEventPoolData next = this.poolDataFactory.GetNext();
				if (next == null)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_NoMoreEvent, null);
				}
				else
				{
					this.currentStage = next.stage;
					this.eventRealIndex = next.queueRealIndex;
					await this.CreateData(next, outData);
				}
			}
		}

		private Task CreateData(GameEventPoolData eventPool, TaskOutValue<GameEventData> outData)
		{
			GameEventFactory.<CreateData>d__21 <CreateData>d__;
			<CreateData>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateData>d__.<>4__this = this;
			<CreateData>d__.eventPool = eventPool;
			<CreateData>d__.outData = outData;
			<CreateData>d__.<>1__state = -1;
			<CreateData>d__.<>t__builder.Start<GameEventFactory.<CreateData>d__21>(ref <CreateData>d__);
			return <CreateData>d__.<>t__builder.Task;
		}

		private void CheckChildData(Node node, GameEventData data)
		{
			if (data == null)
			{
				HLog.LogError("GameEventFactory.CheckChildData: data is null, node=" + node.name);
				return;
			}
			NodePort outputPort = node.GetOutputPort("function");
			if (outputPort != null)
			{
				List<NodePort> connections = outputPort.GetConnections();
				for (int i = 0; i < connections.Count; i++)
				{
					Node node2 = connections[i].node;
					if (!(connections[i].node == null) && node2 is GameEventFunctionNode)
					{
						GameEventFunctionNode gameEventFunctionNode = (GameEventFunctionNode)node2;
						GameEventDataFunction gameEventDataFunction = new GameEventDataFunction(data.poolData, gameEventFunctionNode.eventFunction, gameEventFunctionNode.funcParam);
						data.AddFunctionData(gameEventDataFunction);
					}
				}
			}
			List<NodePort> list;
			if (data.GetNodeType() == GameEventNodeType.IF)
			{
				NodePort outputPort2 = node.GetOutputPort("ifTrue");
				NodePort outputPort3 = node.GetOutputPort("ifFalse");
				if (outputPort2 == null || outputPort3 == null)
				{
					HLog.LogError(string.Format("条件节点必须有ture和false两个子节点, id={0}", data.poolData.tableId));
					return;
				}
				list = new List<NodePort>();
				if (outputPort2.GetConnections().Count > 0)
				{
					list.Add(outputPort2.GetConnection(0));
				}
				if (outputPort3.GetConnections().Count > 0)
				{
					list.Add(outputPort3.GetConnection(0));
				}
			}
			else
			{
				NodePort outputPort4 = node.GetOutputPort("exit");
				if (outputPort4 == null)
				{
					return;
				}
				list = outputPort4.GetConnections();
			}
			for (int j = 0; j < list.Count; j++)
			{
				Node node3 = list[j].node;
				if (!(list[j].node == null))
				{
					Node node4;
					GameEventData gameEventData;
					if (node3 is GameEventNormalNode)
					{
						GameEventNormalNode gameEventNormalNode = (GameEventNormalNode)node3;
						node4 = gameEventNormalNode;
						List<NodeAttParam> list2 = this.ToNodeAttList(gameEventNormalNode.elements, data.poolData.serverRate);
						List<NodeItemParam> list3 = this.ToNodeItemList(gameEventNormalNode.items, data.poolData.serverRate);
						gameEventData = new GameEventDataNormal(data.poolData, gameEventNormalNode.titleId, gameEventNormalNode.summaryId, gameEventNormalNode.contentId, list2, list3, gameEventNormalNode.isServerDrop);
					}
					else if (node3 is GameEventSelectNode)
					{
						GameEventSelectNode gameEventSelectNode = (GameEventSelectNode)node3;
						node4 = gameEventSelectNode;
						gameEventData = new GameEventDataSelect(data.poolData, gameEventSelectNode.languageId, gameEventSelectNode.info, gameEventSelectNode.buttonType, gameEventSelectNode.buttonColorType, gameEventSelectNode.needId, gameEventSelectNode.num, gameEventSelectNode.tipLanguageId, gameEventSelectNode.infoTip);
					}
					else if (node3 is GameEventIFNode)
					{
						GameEventIFNode gameEventIFNode = (GameEventIFNode)node3;
						node4 = gameEventIFNode;
						gameEventData = new GameEventDataIF(data.poolData, gameEventIFNode.ifType, gameEventIFNode.opType, gameEventIFNode.num);
					}
					else if (node3 is GameEventBattleNode)
					{
						GameEventBattleNode gameEventBattleNode = (GameEventBattleNode)node3;
						node4 = gameEventBattleNode;
						gameEventData = new GameEventDataBattle(data.poolData, gameEventBattleNode.groupIndex);
					}
					else if (node3 is GameEventShopNode)
					{
						GameEventShopNode gameEventShopNode = (GameEventShopNode)node3;
						node4 = gameEventShopNode;
						gameEventData = new GameEventDataShop(data.poolData, gameEventShopNode.npcId);
					}
					else if (node3 is GameEventBoxNode)
					{
						node4 = (GameEventBoxNode)node3;
						gameEventData = new GameEventDataBox(data.poolData);
					}
					else if (node3 is GameEventSkillNode)
					{
						GameEventSkillNode gameEventSkillNode = (GameEventSkillNode)node3;
						node4 = gameEventSkillNode;
						gameEventData = new GameEventDataSkill(data.poolData, gameEventSkillNode.sourceId, gameEventSkillNode.randomNum, gameEventSkillNode.selectNum, gameEventSkillNode.skillId);
					}
					else if (node3 is GameEventSurpriseNode)
					{
						node4 = (GameEventSurpriseNode)node3;
						gameEventData = new GameEventDataSurprise(data.poolData);
					}
					else if (node3 is GameEventTalentSkillNode)
					{
						node4 = (GameEventTalentSkillNode)node3;
						gameEventData = new GameEventDataTalentSkill(data.poolData);
					}
					else if (node3 is GameEventItemNode)
					{
						GameEventItemNode gameEventItemNode = (GameEventItemNode)node3;
						node4 = gameEventItemNode;
						List<NodeItemParam> list4 = this.ToNodeItemList(gameEventItemNode.elements, data.poolData.serverRate);
						gameEventData = new GameEventDataItem(data.poolData, list4);
					}
					else if (node3 is GameEventFishingNode)
					{
						GameEventFishingNode gameEventFishingNode = (GameEventFishingNode)node3;
						node4 = gameEventFishingNode;
						gameEventData = new GameEventDataFishing(data.poolData, gameEventFishingNode.npcId, gameEventFishingNode.fishType);
					}
					else if (node3 is GameEventNpcBattleNode)
					{
						node4 = (GameEventNpcBattleNode)node3;
						gameEventData = new GameEventDataNpcBattle(data.poolData);
					}
					else if (node3 is GameEventRandomNode)
					{
						node4 = (GameEventRandomNode)node3;
						gameEventData = new GameEventDataRandom(data.poolData);
					}
					else if (node3 is GameEventFishingGameNode)
					{
						node4 = (GameEventFishingGameNode)node3;
						gameEventData = new GameEventDataFishingGame(data.poolData);
					}
					else if (node3 is GameEventMiniGameNode)
					{
						node4 = (GameEventMiniGameNode)node3;
						gameEventData = new GameEventDataMiniGame(data.poolData);
					}
					else if (node3 is GameEventWaveBattleNode)
					{
						GameEventWaveBattleNode gameEventWaveBattleNode = (GameEventWaveBattleNode)node3;
						node4 = gameEventWaveBattleNode;
						gameEventData = new GameEventDataWaveBattle(data.poolData, gameEventWaveBattleNode.groupArr);
					}
					else
					{
						if (!(node3 is GameEventRoundSkillNode))
						{
							HLog.LogError("GameEventFactory.CreateGameEventData: Undefined node, node=" + node3.name);
							goto IL_0540;
						}
						GameEventRoundSkillNode gameEventRoundSkillNode = (GameEventRoundSkillNode)node3;
						node4 = gameEventRoundSkillNode;
						gameEventData = new GameEventDataRoundSkill(data.poolData, gameEventRoundSkillNode.sourceId, gameEventRoundSkillNode.round, gameEventRoundSkillNode.randomNum, gameEventRoundSkillNode.selectNum);
					}
					data.AddChild(gameEventData);
					this.CheckChildData(node4, gameEventData);
				}
				IL_0540:;
			}
		}

		private List<NodeAttParam> ToNodeAttList(GameEventNormalNode.AttStruct[] arr, int rate)
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			if (arr != null)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					NodeAttParam nodeAttParam = new NodeAttParam(arr[i].type, (double)arr[i].num, ChapterDropSource.Event, rate);
					list.Add(nodeAttParam);
				}
			}
			return list;
		}

		private List<NodeItemParam> ToNodeItemList(GameEventItemNode.ItemStruct[] arr, int rate)
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			if (arr != null)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					NodeItemParam nodeItemParam = new NodeItemParam((NodeItemType)arr[i].type, arr[i].id, (long)arr[i].num, ChapterDropSource.Event, rate);
					list.Add(nodeItemParam);
				}
			}
			return list;
		}

		private GameEventData rootData;

		private GameEventGroup eventGroup;

		private int currentStage;

		private int eventRealIndex;

		private bool isRefreshScene;

		private GameEventType lastEventType;

		private GameEventPoolDataFactory poolDataFactory;

		private int totalStage;
	}
}
