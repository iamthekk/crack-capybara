using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix.Client
{
	public class CBulletFactory
	{
		public CBulletFactory(CMemberBase member)
		{
			this.m_owner = member;
		}

		public async Task OnInit()
		{
			await Task.CompletedTask;
		}

		public async Task OnDeInit()
		{
			await this.RemoveAllBullet();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.m_bullets.Values);
			foreach (CBulletBase cbulletBase in this.list)
			{
				if (cbulletBase != null)
				{
					cbulletBase.Update(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public async Task AddBullet(CFireBulletData data)
		{
			GameBullet_bullet elementById = GameApp.Table.GetManager().GetGameBullet_bulletModelInstance().GetElementById(data.m_bulletID);
			GameBullet_bulletType elementById2 = GameApp.Table.GetManager().GetGameBullet_bulletTypeModelInstance().GetElementById(elementById.bulletType);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("Tabla[GameBullet_bulletType] is Error.bulletTable.bulletType = {0}", elementById.bulletType));
			}
			else
			{
				CBulletBase cbulletBase = Activator.CreateInstance(Type.GetType(elementById2.cClassName)) as CBulletBase;
				if (cbulletBase == null)
				{
					HLog.LogError("CBulletFactory.AddBullet   CBulletBase == null   cClassName = " + elementById2.cClassName);
				}
				else
				{
					cbulletBase.ReadParameters(elementById.parameters);
					cbulletBase.Init(this, data, elementById, this.m_owner);
					if (!this.m_bullets.ContainsKey(cbulletBase.m_instanceID))
					{
						this.m_bullets[cbulletBase.m_instanceID] = cbulletBase;
					}
					GameApp.Sound.PlayClip(data.m_startSoundID, 1f);
				}
			}
		}

		public async Task RemoveBullet(CBulletBase bullet)
		{
			if (bullet != null)
			{
				this.m_bullets.Remove(bullet.m_instanceID);
				await bullet.DeInit();
			}
		}

		private async Task RemoveAllBullet()
		{
			List<CBulletBase> list = this.m_bullets.Values.ToList<CBulletBase>();
			List<Task> list2 = new List<Task>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(this.RemoveBullet(list[i]));
			}
			await Task.WhenAll(list2);
		}

		public void OnShake(int bulletID)
		{
			GameBullet_bullet elementById = GameApp.Table.GetManager().GetGameBullet_bulletModelInstance().GetElementById(bulletID);
			if (elementById == null)
			{
				return;
			}
			if (elementById.shakeID.Equals(0))
			{
				return;
			}
			EventArgsGameCameraShake instance = Singleton<EventArgsGameCameraShake>.Instance;
			instance.SetData(elementById.shakeID);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Shake, instance);
		}

		public CMemberBase m_owner;

		private Dictionary<int, CBulletBase> m_bullets = new Dictionary<int, CBulletBase>();

		private List<CBulletBase> list = new List<CBulletBase>();

		private float startTime;
	}
}
