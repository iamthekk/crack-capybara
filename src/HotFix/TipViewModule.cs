using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class TipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.textTipNodePrefab.SetActive(false);
			this.rewardTipNodePrefab.SetActive(false);
			this.combatTipNodePrefab.SetActive(false);
			this.attributeNodePrefab.SetActive(false);
			this.tipNodeParent.SetActive(true);
			this.m_combatTipParent.SetActive(true);
			this.m_attributeTipParent.SetActive(true);
			this.tipNodePool = LocalUnityObjctPool.Create(base.gameObject);
			this.tipNodePool.CreateCache("textTipNodePoolKey", this.textTipNodePrefab);
			this.tipNodePool.CreateCache("rewardTipNodePoolKey", this.rewardTipNodePrefab);
			this.tipNodePool.CreateCache("combatTipNodePoolKey", this.combatTipNodePrefab);
			this.tipNodePool.CreateCache("attributeTipNodePoolKey", this.attributeNodePrefab);
		}

		public override void OnDelete()
		{
			foreach (TextTipNode textTipNode in this.textTipNodes)
			{
				textTipNode.DeInit();
			}
			this.textTipNodes.Clear();
			foreach (ItemTipNode itemTipNode in this.itemTipNodes)
			{
				itemTipNode.DeInit();
			}
			this.itemTipNodes.Clear();
			foreach (CombatTipNode combatTipNode in this.combatTipNodes)
			{
				if (!(combatTipNode == null))
				{
					combatTipNode.DeInit();
				}
			}
			this.combatTipNodes.Clear();
			foreach (AttributeTipNode attributeTipNode in this.attributeTipNodes)
			{
				if (!(attributeTipNode == null))
				{
					attributeTipNode.DeInit();
				}
			}
			this.attributeTipNodes.Clear();
			this.tipNodePool.CollectAll();
		}

		public override void OnOpen(object data)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.combatTipNodes);
			foreach (CombatTipNode combatTipNode in this.list)
			{
				if (!(combatTipNode == null))
				{
					combatTipNode.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(3, new HandlerEvent(this.OnAddTextTipNode));
			manager.RegisterEvent(4, new HandlerEvent(this.OnAddItemTipNode));
			manager.RegisterEvent(5, new HandlerEvent(this.OnAddCombatTipNode));
			manager.RegisterEvent(6, new HandlerEvent(this.OnAddAttributeTipNode));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(3, new HandlerEvent(this.OnAddTextTipNode));
			manager.UnRegisterEvent(4, new HandlerEvent(this.OnAddItemTipNode));
			manager.UnRegisterEvent(5, new HandlerEvent(this.OnAddCombatTipNode));
			manager.UnRegisterEvent(6, new HandlerEvent(this.OnAddAttributeTipNode));
		}

		private void OnAddTextTipNode(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsString eventArgsString = eventArgs as EventArgsString;
			if (eventArgsString == null || eventArgsString.Value.Equals(string.Empty))
			{
				return;
			}
			TextTipNode component = this.ActiveTipNodeObj("textTipNodePoolKey").GetComponent<TextTipNode>();
			component.m_onFinished = delegate(TextTipNode tipNode)
			{
				tipNode.DeInit();
				this.textTipNodes.Remove(tipNode);
				this.tipNodePool.EnQueue("textTipNodePoolKey", tipNode.gameObject);
			};
			this.textTipNodes.Add(component);
			component.Init();
			component.SetInfo(eventArgsString.Value);
		}

		private void OnAddItemTipNode(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsAddItemTipData eventArgsAddItemTipData = eventArgs as EventArgsAddItemTipData;
			if (eventArgsAddItemTipData == null)
			{
				return;
			}
			GameObject gameObject = this.ActiveTipNodeObj("rewardTipNodePoolKey");
			if (eventArgsAddItemTipData.IsSetPosition)
			{
				gameObject.transform.position = eventArgsAddItemTipData.Position;
			}
			ItemTipNode component = gameObject.GetComponent<ItemTipNode>();
			component.OnFinished = delegate(ItemTipNode tipNode)
			{
				tipNode.DeInit();
				this.itemTipNodes.Remove(tipNode);
				this.tipNodePool.EnQueue("rewardTipNodePoolKey", tipNode.gameObject);
			};
			this.itemTipNodes.Add(component);
			component.Init();
			component.SetInfo(eventArgsAddItemTipData.ItemId, eventArgsAddItemTipData.Tip);
		}

		private void OnAddCombatTipNode(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsAddCombatTipNode eventArgsAddCombatTipNode = eventargs as EventArgsAddCombatTipNode;
			if (eventArgsAddCombatTipNode == null)
			{
				return;
			}
			CombatTipNode component = this.ActiveTipNodeObj("combatTipNodePoolKey").GetComponent<CombatTipNode>();
			component.m_onFinished = delegate(CombatTipNode tipNode)
			{
				tipNode.DeInit();
				this.combatTipNodes.Remove(tipNode);
				this.tipNodePool.EnQueue("combatTipNodePoolKey", tipNode.gameObject);
			};
			this.combatTipNodes.Add(component);
			component.Init();
			component.SetData(eventArgsAddCombatTipNode.m_from, eventArgsAddCombatTipNode.m_to);
			component.Play();
			if (eventArgsAddCombatTipNode.m_to > eventArgsAddCombatTipNode.m_from)
			{
				GameApp.Sound.PlayClip(604, 1f);
				return;
			}
			if (eventArgsAddCombatTipNode.m_to < eventArgsAddCombatTipNode.m_from)
			{
				GameApp.Sound.PlayClip(605, 1f);
			}
		}

		private void OnAddAttributeTipNode(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsAddAttributeTipNode eventArgsAddAttributeTipNode = eventargs as EventArgsAddAttributeTipNode;
			if (eventArgsAddAttributeTipNode == null)
			{
				return;
			}
			if (eventArgsAddAttributeTipNode.data != null && eventArgsAddAttributeTipNode.data.Count > 0)
			{
				for (int i = 0; i < eventArgsAddAttributeTipNode.data.Count; i++)
				{
					KeyValuePair<string, long> keyValuePair = eventArgsAddAttributeTipNode.data[i];
					GameObject gameObject = this.ActiveTipNodeObj("attributeTipNodePoolKey");
					AttributeTipNode component = gameObject.GetComponent<AttributeTipNode>();
					Vector3 zero = Vector3.zero;
					zero.y = -component.flySpace * (float)i;
					gameObject.transform.localPosition = zero;
					component.m_onFinished = delegate(AttributeTipNode tipNode)
					{
						tipNode.DeInit();
						this.attributeTipNodes.Remove(tipNode);
						this.tipNodePool.EnQueue("attributeTipNodePoolKey", tipNode.gameObject);
					};
					this.attributeTipNodes.Add(component);
					component.Init();
					component.SetData(keyValuePair.Key, keyValuePair.Value);
					component.Play((float)i * component.intervalTime);
				}
			}
		}

		private GameObject ActiveTipNodeObj(string cacheName)
		{
			GameObject gameObject = this.tipNodePool.DeQueue(cacheName);
			if (cacheName == "attributeTipNodePoolKey")
			{
				gameObject.transform.SetParent(this.m_attributeTipParent.transform);
			}
			else
			{
				gameObject.transform.SetParent(this.m_combatTipParent.transform);
			}
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		[GameTestMethod("Tip", "AddCombatTipNode", "", 0)]
		private static void OnAddCombatTipNode()
		{
			EventArgsAddCombatTipNode eventArgsAddCombatTipNode = new EventArgsAddCombatTipNode();
			eventArgsAddCombatTipNode.m_from = 1000L;
			eventArgsAddCombatTipNode.m_to = 10000L;
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, eventArgsAddCombatTipNode);
		}

		[GameTestMethod("Tip", "AddAttributeTipNode", "", 0)]
		private static void OnAddAttributeTipNode()
		{
			EventArgsAddAttributeTipNode eventArgsAddAttributeTipNode = new EventArgsAddAttributeTipNode();
			eventArgsAddAttributeTipNode.AddData("Attack", 50L);
			eventArgsAddAttributeTipNode.AddData("Defence", 100L);
			eventArgsAddAttributeTipNode.AddData("HPMax", 200L);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddAttributeTipNode, eventArgsAddAttributeTipNode);
		}

		public GameObject tipNodeParent;

		public GameObject m_combatTipParent;

		public GameObject m_attributeTipParent;

		public GameObject textTipNodePrefab;

		public GameObject rewardTipNodePrefab;

		public GameObject combatTipNodePrefab;

		public GameObject attributeNodePrefab;

		private LocalUnityObjctPool tipNodePool;

		private readonly List<TextTipNode> textTipNodes = new List<TextTipNode>();

		private readonly List<ItemTipNode> itemTipNodes = new List<ItemTipNode>();

		private readonly List<CombatTipNode> combatTipNodes = new List<CombatTipNode>();

		private readonly List<AttributeTipNode> attributeTipNodes = new List<AttributeTipNode>();

		private const string TextTipNodePoolKey = "textTipNodePoolKey";

		private const string RewardTipNodePoolKey = "rewardTipNodePoolKey";

		private const string CombatTipNodePoolKey = "combatTipNodePoolKey";

		private const string AttributeTipNodePoolKey = "attributeTipNodePoolKey";

		private List<CombatTipNode> list = new List<CombatTipNode>();
	}
}
