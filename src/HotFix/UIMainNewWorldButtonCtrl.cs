using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainNewWorldButtonCtrl : CustomBehaviour
	{
		private NewWorldDataModule dataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
			}
		}

		protected override void OnInit()
		{
			this.buttonNewWorld.onClick.AddListener(new UnityAction(this.OnClickNewWorld));
			RedPointController.Instance.RegRecordChange("Main.NewWorld", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			this.buttonNewWorld.onClick.RemoveListener(new UnityAction(this.OnClickNewWorld));
			RedPointController.Instance.UnRegRecordChange("Main.NewWorld", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			long num = this.dataModule.NewWorldOpenTime - DxxTools.Time.ServerTimestamp;
			if (num > 0L)
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(num);
				return;
			}
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_open_short");
		}

		public void OnRefresh()
		{
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.NewWorld, false);
			base.gameObject.SetActiveSafe(flag && !this.dataModule.IsEnterNewWorld);
		}

		private void OnClickNewWorld()
		{
			GameApp.View.OpenView(ViewName.NewWorldViewModule, null, 1, null, null);
		}

		private void OnRedPointChange(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		public CustomButton buttonNewWorld;

		public CustomText textTime;

		public RedNodeOneCtrl redNode;

		private string timeStr;
	}
}
