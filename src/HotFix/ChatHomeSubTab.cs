using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChatHomeSubTab : CustomBehaviour
	{
		public ChatHomeSubPanelType TabType { get; private set; }

		protected override void OnInit()
		{
			this.tabButton.onClick.AddListener(new UnityAction(this.OnClickNodeButton));
		}

		protected override void OnDeInit()
		{
			this.tabButton.onClick.RemoveListener(new UnityAction(this.OnClickNodeButton));
		}

		private void OnClickNodeButton()
		{
			Action<ChatHomeSubPanelType> action = this.onClick;
			if (action == null)
			{
				return;
			}
			action(this.TabType);
		}

		public void SetData(bool initSelect, Action<ChatHomeSubPanelType> onClickVul)
		{
			this.onClick = onClickVul;
			this.DoSelect(initSelect);
		}

		public void SetSelect(bool isSelectVal)
		{
			if (this.isSelect == isSelectVal)
			{
				return;
			}
			this.DoSelect(isSelectVal);
		}

		private void DoSelect(bool isSelectVal)
		{
			this.isSelect = isSelectVal;
			foreach (GameObject gameObject in this.selectObj)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(this.isSelect);
				}
			}
			foreach (GameObject gameObject2 in this.unSelectObj)
			{
				if (!(gameObject2 == null))
				{
					gameObject2.SetActive(!this.isSelect);
				}
			}
		}

		[SerializeField]
		private CustomButton tabButton;

		[SerializeField]
		private GameObject[] selectObj;

		[SerializeField]
		private GameObject[] unSelectObj;

		private Action<ChatHomeSubPanelType> onClick;

		private bool isSelect;
	}
}
