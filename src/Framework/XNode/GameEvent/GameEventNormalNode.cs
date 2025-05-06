using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace XNode.GameEvent
{
	public class GameEventNormalNode : Node
	{
		protected override void Init()
		{
			base.Init();
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}

		public string GetAtt()
		{
			if (this.elements == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.elements.Length; i++)
			{
				string chinese = this.GetChinese(this.elements[i].type);
				string text = (this.IsRate(this.elements[i].type) ? "%" : "");
				stringBuilder.Append(chinese);
				if (this.elements[i].num > 0f)
				{
					stringBuilder.Append("+" + this.elements[i].num.ToString());
				}
				else
				{
					stringBuilder.Append(this.elements[i].num.ToString());
				}
				stringBuilder.Append(text);
				if (i < this.elements.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Insert(0, "(");
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		private string GetChinese(AttEnum type)
		{
			switch (type)
			{
			case AttEnum.Attack:
				return "攻击";
			case AttEnum.HP:
				return "血量";
			case AttEnum.HPMax:
				return "血上限";
			case AttEnum.Exp:
				return "经验";
			case AttEnum.Food:
				return "食物";
			case AttEnum.Defense:
				return "防御";
			case AttEnum.CampHpRate:
				return "营地回复血量";
			case AttEnum.Chips:
				return "筹码";
			}
			return "";
		}

		private bool IsRate(AttEnum type)
		{
			return type == AttEnum.Attack || type == AttEnum.HPMax || type == AttEnum.HP || type == AttEnum.Defense;
		}

		public override Dictionary<string, string> GetLanguageDic()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(this.titleId) && !dictionary.ContainsKey(this.titleId))
			{
				dictionary.Add(this.titleId, this.title);
			}
			if (!string.IsNullOrEmpty(this.summaryId) && !dictionary.ContainsKey(this.summaryId))
			{
				dictionary.Add(this.summaryId, this.summary);
			}
			if (!string.IsNullOrEmpty(this.contentId) && !dictionary.ContainsKey(this.contentId))
			{
				dictionary.Add(this.contentId, this.content);
			}
			return dictionary;
		}

		public void SetElements(GameEventNormalNode.AttStruct[] arr)
		{
			this.elements = arr;
		}

		public void SetItems(GameEventItemNode.ItemStruct[] arr)
		{
			this.items = arr;
		}

		[Node.InputAttribute(1, 0, 0, false)]
		public Empty enter;

		[Node.OutputAttribute(0, 0, 0, false)]
		public Empty exit;

		[Node.OutputAttribute(0, 0, 0, false)]
		public FuncEmpty function;

		public string titleId;

		[TextArea]
		public string title;

		public string summaryId;

		[TextArea]
		public string summary;

		public string contentId;

		[TextArea]
		public string content;

		[Node.OutputAttribute(0, 0, 0, false, dynamicPortList = true)]
		public GameEventNormalNode.AttStruct[] elements;

		[Node.OutputAttribute(0, 0, 0, false, dynamicPortList = true)]
		public GameEventItemNode.ItemStruct[] items;

		public bool isServerDrop;

		[Serializable]
		public struct AttStruct
		{
			public AttEnum type;

			public float num;
		}
	}
}
