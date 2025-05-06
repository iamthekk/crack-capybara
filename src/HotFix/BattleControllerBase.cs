using System;
using System.Threading.Tasks;

namespace HotFix
{
	public abstract class BattleControllerBase
	{
		public async Task Init()
		{
			await this.OnInit();
		}

		protected abstract Task OnInit();

		public async Task DeInit()
		{
			await this.OnDeInit();
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected abstract Task OnDeInit();

		protected abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);
	}
}
