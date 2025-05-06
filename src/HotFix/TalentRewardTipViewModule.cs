using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class TalentRewardTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			TalentRewardTipViewModule.OpenData openData = data as TalentRewardTipViewModule.OpenData;
			this.txtTitle.text = openData.title;
			this.txtDesc.text = openData.desc;
			Vector3 vector = openData.clickPos;
			vector.z = base.transform.position.z;
			this.node.position = vector;
			vector = this.node.localPosition;
			vector.y += openData.offsetY;
			this.node.localPosition = vector;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnMask.m_onClick = new Action(this.OnBtnMaskClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnMask.m_onClick = null;
		}

		private void OnBtnMaskClick()
		{
			GameApp.View.CloseView(ViewName.TalentRewardTipViewModule, null);
		}

		public Transform node;

		public CustomButton btnMask;

		public CustomText txtTitle;

		public CustomText txtDesc;

		public class OpenData
		{
			public string title;

			public string desc;

			public Vector3 clickPos;

			public float offsetY;
		}
	}
}
