using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class HitFactory
	{
		public HitFactory(CMemberBase member)
		{
			this.m_owner = member;
		}

		public async Task OnInit()
		{
			await Task.CompletedTask;
		}

		public async Task OnDeInit()
		{
			await this.RemoveAllHits();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.m_hits.Values);
			foreach (HitBase hitBase in this.list)
			{
				if (hitBase != null)
				{
					hitBase.Update(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public async Task AddHit(TaskOutValue<HitBase> outHit, int id, MemberBodyPosType posType, PointRotationDirection direction, HitTargetType targetType, CMemberBase attacker, CBulletBase bullet)
		{
			if (outHit == null)
			{
				HLog.LogError(HLog.ToColor("AddHit is error. outHit = null.", 3));
			}
			else if (this.m_owner != null)
			{
				if (this.m_owner.m_memberData != null)
				{
					if (id != 0)
					{
						ArtHit_Hit elementById = GameApp.Table.GetManager().GetArtHit_HitModelInstance().GetElementById(id);
						string path = elementById.path;
						await PoolManager.Instance.CheckPrefab(path);
						Transform transform = this.m_owner.m_body.GetTransform(posType);
						GameObject gameObject = PoolManager.Instance.Out(path, transform.position, transform.rotation, transform);
						Hit_Default hit_Default = new Hit_Default();
						GameObject gameObject2;
						switch (targetType)
						{
						case HitTargetType.Owner:
							gameObject2 = this.m_owner.m_gameObject;
							break;
						case HitTargetType.Bullet:
							gameObject2 = this.m_owner.m_gameObject;
							break;
						case HitTargetType.Attacker:
							gameObject2 = attacker.m_gameObject;
							break;
						default:
							throw new ArgumentOutOfRangeException("targetType", targetType, null);
						}
						hit_Default.Init(this, gameObject, this.m_owner, gameObject2, direction);
						this.m_hits[gameObject.GetInstanceID()] = hit_Default;
						outHit.SetValue(hit_Default);
					}
				}
			}
		}

		public void ShowBulletHitEffect(int hitEffectID)
		{
			GameSkill_hitEffect elementById = GameApp.Table.GetManager().GetGameSkill_hitEffectModelInstance().GetElementById(hitEffectID);
			if (elementById == null)
			{
				HLog.LogError(HLog.ToColor(string.Format("Table[GameSkill_hitEffect] is error ..hitEffectID == {0}", hitEffectID), 3));
				return;
			}
			if (this.m_owner == null)
			{
				return;
			}
			Dictionary<MemberMeatType, HitFactory.HitAnimation> dictionary = new Dictionary<MemberMeatType, HitFactory.HitAnimation>();
			HitFactory.HitAnimation hitEffect = HitFactory.GetHitEffect(elementById.roleType_1);
			dictionary.Add(MemberMeatType.Stone, hitEffect);
			HitFactory.HitAnimation hitEffect2 = HitFactory.GetHitEffect(elementById.roleType_2);
			dictionary.Add(MemberMeatType.Jelly, hitEffect2);
			HitFactory.HitAnimation hitEffect3 = HitFactory.GetHitEffect(elementById.roleType_3);
			dictionary.Add(MemberMeatType.Plant, hitEffect3);
			HitFactory.HitAnimation hitEffect4 = HitFactory.GetHitEffect(elementById.roleType_4);
			dictionary.Add(MemberMeatType.Skeleton, hitEffect4);
			MemberMeatType meatType = this.m_owner.m_memberData.MeatType;
			if (dictionary.ContainsKey(meatType))
			{
				string modelAnimation = dictionary[meatType].ModelAnimation;
				if (!modelAnimation.Equals(string.Empty))
				{
					string text = modelAnimation + "_Die";
					if (this.m_owner.IsDeath && this.m_owner.HasShakeAnimatorAnim(text))
					{
						this.m_owner.PlayShakeAnimatorAnim(text);
					}
					else
					{
						this.m_owner.PlayShakeAnimatorAnim(modelAnimation);
					}
				}
				string modelAction = dictionary[meatType].ModelAction;
				if (!modelAction.Equals(string.Empty) && !this.m_owner.IsDeath)
				{
					this.m_owner.PlayAnimation(modelAction);
					this.m_owner.AddAnimationIdle();
					MountSpinePlayer mountSpinePlayer = this.m_owner.mountSpinePlayer;
					if (mountSpinePlayer != null)
					{
						mountSpinePlayer.PlayAnimation(modelAction);
					}
					MountSpinePlayer mountSpinePlayer2 = this.m_owner.mountSpinePlayer;
					if (mountSpinePlayer2 == null)
					{
						return;
					}
					mountSpinePlayer2.AddAnimation("Idle");
				}
			}
		}

		public static HitFactory.HitAnimation GetHitEffect(string roleType)
		{
			string[] array = roleType.Split('|', StringSplitOptions.None);
			if (!array.Length.Equals(2))
			{
				HLog.LogError(HLog.ToColor("BaseMember_ToHitted.GetHitEffect. GameBullet_hitEffectModelInstance is error ..", 3));
				return null;
			}
			return new HitFactory.HitAnimation
			{
				ModelAction = array[0],
				ModelAnimation = array[1]
			};
		}

		public async Task RemoveAllHits()
		{
			List<HitBase> hits = this.m_hits.Values.ToList<HitBase>();
			for (int i = 0; i < hits.Count; i++)
			{
				HitBase hitBase = hits[i];
				await this.RemoveHit(hitBase);
			}
		}

		public async Task RemoveHit(HitBase hit)
		{
			if (hit != null)
			{
				await hit.DeInit();
				this.m_hits.Remove(hit.m_instanceID);
				PoolManager.Instance.Put(hit.m_gameObject);
			}
		}

		public void ShowMiss()
		{
			this.m_owner.PlayShakeAnimatorAnim("Dodge");
		}

		public CMemberBase m_owner;

		private Dictionary<int, HitBase> m_hits = new Dictionary<int, HitBase>();

		private List<HitBase> list = new List<HitBase>();

		public class HitAnimation
		{
			public string ModelAction = string.Empty;

			public string ModelAnimation = string.Empty;
		}
	}
}
