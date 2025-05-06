using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainExtensionButtonsCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_redNodePaths.Clear();
			this.m_redNodePaths.Add("Main.Bag");
			this.m_redNodePaths.Add("Main.Mail");
			this.m_redNodePaths.Add("Main.Mission");
			this.m_redNodePaths.Add("Main.BlackMarket");
			this.m_redNodePaths.Add("Main.Ranking");
			this.m_redNodePaths.Add("Main.Sign");
			this.m_redNodePaths.Add("Main.TVReward");
			this.m_redNodePaths.Add("Main.Setting");
			this.m_redNodePaths.Add("Main.SelfInfo");
			this.btnExtension.onClick.AddListener(new UnityAction(this.OnBtnExtensionClick));
			this.redNode.Value = 0;
			foreach (string text in this.m_redNodePaths)
			{
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.RegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
		}

		protected override void OnDeInit()
		{
			this.btnExtension.onClick.RemoveListener(new UnityAction(this.OnBtnExtensionClick));
			for (int i = 0; i < this.m_redNodePaths.Count; i++)
			{
				string text = this.m_redNodePaths[i];
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.UnRegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
			this.m_redNodePaths.Clear();
		}

		private void OnBtnExtensionClick()
		{
			GameApp.View.OpenView(ViewName.MoreExtensionViewModule, null, 1, null, null);
		}

		public void OnLanguageChange()
		{
		}

		public void OnRefresh()
		{
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Extension, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Bag, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mail, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Task, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Ranking, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Sign, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TVReward, false) || Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Setting, false);
			if (this.btnExtension.gameObject.activeSelf != flag)
			{
				this.btnExtension.gameObject.SetActive(flag);
			}
			this.CheckRedNode();
		}

		public RectTransform GetButtonTransform()
		{
			return this.btnExtension.transform as RectTransform;
		}

		private void OnRefreshRedPointChange(RedNodeListenData obj)
		{
			bool flag = false;
			for (int i = 0; i < this.m_redNodePaths.Count; i++)
			{
				RedPointDataRecord record = RedPointController.Instance.GetRecord(this.m_redNodePaths[i]);
				if (record != null && record.RedPointCount > 0)
				{
					flag = true;
					break;
				}
			}
			this.redNode.Value = (flag ? 1 : 0);
		}

		private void CheckRedNode()
		{
			this.OnRefreshRedPointChange(null);
		}

		public CustomButton btnExtension;

		public RedNodeOneCtrl redNode;

		private List<string> m_redNodePaths = new List<string>();
	}
}
