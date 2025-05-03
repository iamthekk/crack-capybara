using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIEquipMergeFilterSelectNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_bt = base.gameObject.GetComponent<CustomChooseButton>();
			this.m_bt.onClick.AddListener(new UnityAction(this.OnClickBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			if (this.m_bt != null)
			{
				this.m_bt.onClick.RemoveListener(new UnityAction(this.OnClickBt));
			}
			this.m_onClickBt = null;
		}

		public void SetData(int index)
		{
			this.m_index = index;
		}

		private void OnClickBt()
		{
			if (this.m_onClickBt == null)
			{
				return;
			}
			this.m_onClickBt(this);
		}

		public void SetSelect(bool isSelect)
		{
			if (this.m_bt == null)
			{
				return;
			}
			this.m_bt.SetSelect(isSelect);
		}

		public void SetIcon(string atlasPath, string spriteName)
		{
			if (this.m_icon == null)
			{
				return;
			}
			this.m_icon.SetImage(atlasPath, spriteName);
		}

		public int m_index;

		[SerializeField]
		private CustomChooseButton m_bt;

		[SerializeField]
		private CustomImage m_icon;

		public Action<UIEquipMergeFilterSelectNode> m_onClickBt;
	}
}
