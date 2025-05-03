using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainChooseButtonCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_chooseBt.onClick.AddListener(new UnityAction(this.OnClickChooseBt));
		}

		protected override void OnDeInit()
		{
			if (this.m_chooseBt != null)
			{
				this.m_chooseBt.onClick = null;
			}
			this.m_chooseBt = null;
			this.m_redNode = null;
		}

		public void SetUnfold(bool isUnfold)
		{
			if (this.m_chooseBt == null)
			{
				return;
			}
			this.m_chooseBt.SetSelect(!isUnfold);
		}

		public void SetRedPoit(bool enable)
		{
			if (this.m_redNode == null)
			{
				return;
			}
			if (this.m_redNode.gameObject == null)
			{
				return;
			}
			this.m_redNode.gameObject.SetActive(enable);
		}

		private void OnClickChooseBt()
		{
			if (this.m_onClick != null)
			{
				this.m_onClick(this.m_chooseBt);
			}
		}

		[SerializeField]
		private CustomChooseButton m_chooseBt;

		[SerializeField]
		private RedNodeOneCtrl m_redNode;

		public Action<CustomChooseButton> m_onClick;
	}
}
