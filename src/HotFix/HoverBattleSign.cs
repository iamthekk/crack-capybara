using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverBattleSign : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.BattleSign;
		}

		protected override void OnInit()
		{
			this.nodePrefab.gameObject.SetActive(false);
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, -0.9f, 0f));
			this.nodes.Clear();
			this.typeNodes.Clear();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_SignAdd, new HandlerEvent(this.OnEventAddSign));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_SignRemove, new HandlerEvent(this.OnEventRemoveSign));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnEventShowHpHUD));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_SignAdd, new HandlerEvent(this.OnEventAddSign));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_SignRemove, new HandlerEvent(this.OnEventRemoveSign));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnEventShowHpHUD));
			this.RemoveAllNode();
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaleDeltaTime)
		{
			if (base.target != null)
			{
				this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, -0.9f, 0f));
				this.UpdateNodes(deltaTime, unscaleDeltaTime);
			}
		}

		private void AddNode(int buffID, int buffLayer, int buffRound)
		{
			GameBuff_buff elementById = GameApp.Table.GetManager().GetGameBuff_buffModelInstance().GetElementById(buffID);
			if (elementById == null || string.IsNullOrEmpty(elementById.spriteName))
			{
				return;
			}
			HoverBattleSign.Node node;
			this.typeNodes.TryGetValue(buffID, out node);
			if (node == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.nodePrefab, this.scroll.transform);
				gameObject.transform.SetParent(this.scroll);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				node = new HoverBattleSign.Node(gameObject, buffID, buffLayer, buffRound);
				node.OnInit();
				this.nodes[node.instanceId] = node;
				this.typeNodes[buffID] = node;
				return;
			}
			this.typeNodes[buffID].SetData(buffLayer, buffRound);
		}

		private void RemoveNode(int buffID)
		{
			HoverBattleSign.Node node;
			this.typeNodes.TryGetValue(buffID, out node);
			if (node == null)
			{
				return;
			}
			node.OnDeInit();
			this.nodes.Remove(node.instanceId);
			this.typeNodes.Remove(buffID);
			Object.Destroy(node.gameObject);
			node.gameObject = null;
		}

		private void UpdateNodes(float deltaTime, float unscaledDeltaTime)
		{
			List<HoverBattleSign.Node> list = this.nodes.Values.ToList<HoverBattleSign.Node>();
			for (int i = 0; i < list.Count; i++)
			{
				HoverBattleSign.Node node = list[i];
				if (node != null)
				{
					node.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		private void RemoveAllNode()
		{
			List<HoverBattleSign.Node> list = this.nodes.Values.ToList<HoverBattleSign.Node>();
			for (int i = 0; i < list.Count; i++)
			{
				HoverBattleSign.Node node = list[i];
				if (node != null)
				{
					node.OnDeInit();
					Object.Destroy(node.gameObject);
					node.gameObject = null;
				}
			}
			this.nodes.Clear();
			this.typeNodes.Clear();
		}

		private void OnEventAddSign(object sender, int eventID, BaseEventArgs args)
		{
			EventArgsAddSign eventArgsAddSign = args as EventArgsAddSign;
			if (eventArgsAddSign == null)
			{
				return;
			}
			if (base.ownerId != eventArgsAddSign.instanceId)
			{
				return;
			}
			this.AddNode(eventArgsAddSign.buffId, eventArgsAddSign.buffLayer, eventArgsAddSign.buffRound);
		}

		private void OnEventRemoveSign(object sender, int eventID, BaseEventArgs args)
		{
			EventArgsRemoveSign eventArgsRemoveSign = args as EventArgsRemoveSign;
			if (eventArgsRemoveSign == null)
			{
				return;
			}
			if (base.ownerId != eventArgsRemoveSign.instanceId)
			{
				return;
			}
			this.RemoveNode(eventArgsRemoveSign.buffId);
		}

		private void OnEventShowHpHUD(object sender, int type, BaseEventArgs args)
		{
			if (base.gameObject == null)
			{
				return;
			}
			EventArgsShowHpHUD eventArgsShowHpHUD = args as EventArgsShowHpHUD;
			if (eventArgsShowHpHUD == null)
			{
				return;
			}
			if (base.ownerId != eventArgsShowHpHUD.instanceId)
			{
				return;
			}
			this.RemoveAllNode();
		}

		public Transform scroll;

		public GameObject nodePrefab;

		public Dictionary<int, HoverBattleSign.Node> nodes = new Dictionary<int, HoverBattleSign.Node>();

		public Dictionary<int, HoverBattleSign.Node> typeNodes = new Dictionary<int, HoverBattleSign.Node>();

		private const float OffsetY = -0.9f;

		public class Node
		{
			public Node(GameObject gameObject, int buffID, int buffLayer, int buffRound)
			{
				this.instanceId = gameObject.GetInstanceID();
				this.gameObject = gameObject;
				this.buffId = buffID;
				this.buffLayer = buffLayer;
				this.buffRound = buffRound;
			}

			public async void OnInit()
			{
				ComponentRegister component = this.gameObject.GetComponent<ComponentRegister>();
				this.icon = component.GetGameObject("Icon").GetComponent<CustomImage>();
				this.textLayer = component.GetGameObject("TextLayer").GetComponent<CustomText>();
				this.textRound = component.GetGameObject("TextRound").GetComponent<CustomText>();
				this.icon.enabled = false;
				this.icon.onFinished.RemoveAllListeners();
				this.icon.onFinished.AddListener(new UnityAction<string, string>(this.OnSetImageFinished));
				GameBuff_buff elementById = GameApp.Table.GetManager().GetGameBuff_buffModelInstance().GetElementById(this.buffId);
				if (elementById.atlasID > 0 && !string.IsNullOrEmpty(elementById.spriteName))
				{
					string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
					this.icon.SetImage(atlasPath, elementById.spriteName);
				}
				this.Refresh(this.buffLayer, this.buffRound);
				await Task.CompletedTask;
			}

			public void OnUpdate(float deltaTime, float unscaledDeltaTime)
			{
			}

			public void OnDeInit()
			{
				if (this.icon != null)
				{
					this.icon.onFinished.RemoveListener(new UnityAction<string, string>(this.OnSetImageFinished));
				}
			}

			private void OnSetImageFinished(string atlasPath, string spriteName)
			{
				if (this.icon != null)
				{
					this.icon.enabled = true;
				}
			}

			public void SetData(int buffLayer, int buffRound)
			{
				this.buffLayer = buffLayer;
				this.buffRound = buffRound;
				this.Refresh(buffLayer, buffRound);
			}

			private void Refresh(int layer, int round)
			{
				if (this.textLayer == null)
				{
					return;
				}
				if (this.textRound == null)
				{
					return;
				}
				if (layer > 1)
				{
					this.textLayer.text = layer.ToString();
				}
				else
				{
					this.textLayer.text = "";
				}
				if (round > 99)
				{
					this.textRound.text = "";
					return;
				}
				this.textRound.text = round.ToString();
			}

			public int instanceId;

			public GameObject gameObject;

			public int buffId;

			public int buffLayer = 1;

			public int buffRound = 1;

			public bool isForever;

			public CustomImage icon;

			public CustomText textLayer;

			public CustomText textRound;
		}
	}
}
