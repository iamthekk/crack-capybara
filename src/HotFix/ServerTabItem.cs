using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class ServerTabItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.imgSelect.gameObject.SetActive(false);
			this.btnItem.m_onClick = new Action(this.OnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void OnItemClick()
		{
			Action<ServerTabItem> action = this.onClickAction;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void SetData(int index, SelectServerViewModule.ServerTabNodeData data)
		{
			this.index = index;
			this.data = data;
			this.UpdateView();
		}

		private void UpdateView()
		{
			string text;
			if (this.data.zoneId > 0U)
			{
				ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)this.data.zoneId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId);
			}
			else
			{
				ServerList_serverGrop elementById2 = GameApp.Table.GetManager().GetServerList_serverGropModelInstance().GetElementById((int)this.data.groupData.groupId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.groupName);
			}
			this.textUnselect.text = text;
			this.textSelect.text = text;
		}

		public void UpdateSelect(int selectIndex)
		{
			this.imgSelect.gameObject.SetActive(selectIndex == this.index);
			this.textUnselect.enabled = selectIndex != this.index;
			this.textSelect.enabled = selectIndex == this.index;
		}

		public CustomButton btnItem;

		public CustomImage imgSelect;

		public CustomText textUnselect;

		public CustomText textSelect;

		[NonSerialized]
		public Action<ServerTabItem> onClickAction;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public SelectServerViewModule.ServerTabNodeData data;
	}
}
