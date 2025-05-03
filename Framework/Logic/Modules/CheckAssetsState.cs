using System;
using Framework.EventSystem;
using Framework.State;
using Framework.ViewModule;

namespace Framework.Logic.Modules
{
	public class CheckAssetsState : State
	{
		public override int GetName()
		{
			return 1;
		}

		public override void OnEnter()
		{
			GameApp.View.OpenView(1, null, UILayers.First, null, null);
		}

		public override void OnExit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}
	}
}
