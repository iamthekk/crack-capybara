using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(80, 116, 162)]
	public class GameEventSelectNode : Node
	{
		protected override void Init()
		{
			base.Init();
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}

		public override Dictionary<string, string> GetLanguageDic()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(this.languageId) && !dictionary.ContainsKey(this.languageId))
			{
				dictionary.Add(this.languageId, this.info);
			}
			if (!string.IsNullOrEmpty(this.tipLanguageId) && !dictionary.ContainsKey(this.tipLanguageId))
			{
				dictionary.Add(this.tipLanguageId, this.infoTip);
			}
			return dictionary;
		}

		[Node.InputAttribute(1, 0, 0, false)]
		public Empty enter;

		[Node.OutputAttribute(0, 0, 0, false)]
		public Empty exit;

		[Node.OutputAttribute(0, 0, 0, false)]
		public FuncEmpty function;

		public string languageId;

		public string tipLanguageId;

		[TextArea]
		public string info;

		[TextArea]
		public string infoTip;

		public ButtonEnum buttonType;

		public ButtonColorEnum buttonColorType;

		[FormerlySerializedAs("costId")]
		public int needId;

		public int num;
	}
}
