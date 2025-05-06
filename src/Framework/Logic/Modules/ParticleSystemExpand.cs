using System;
using UnityEngine;

namespace Framework.Logic.Modules
{
	public static class ParticleSystemExpand
	{
		public static void ParticlePlay(this ParticleSystem particleSystem, bool withChildren = true)
		{
			particleSystem.Play(withChildren);
		}

		public static void ParticleStop(this ParticleSystem particleSystem, bool withChildren = true, ParticleSystemStopBehavior particleSystemStopBehavior = 1)
		{
			particleSystem.Stop(withChildren, particleSystemStopBehavior);
		}

		public static void ParticlePause(this ParticleSystem particleSystem, bool withChildren = true)
		{
			if (!particleSystem.isPlaying)
			{
				return;
			}
			particleSystem.Pause(withChildren);
		}

		public static void ParticleUnPause(this ParticleSystem particleSystem, bool withChildren = true)
		{
			if (particleSystem.isStopped)
			{
				return;
			}
			if (particleSystem.isPlaying)
			{
				return;
			}
			particleSystem.ParticlePlay(withChildren);
		}

		public static void ParticlePlay(this ParticleSystem[] particleSystems, bool withChildren = true)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].ParticlePlay(withChildren);
			}
		}

		public static void ParticleStop(this ParticleSystem[] particleSystems, bool withChildren = true, ParticleSystemStopBehavior particleSystemStopBehavior = 1)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].ParticleStop(withChildren, particleSystemStopBehavior);
			}
		}

		public static void ParticlePause(this ParticleSystem[] particleSystems, bool withChildren = true)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].ParticlePause(withChildren);
			}
		}

		public static void ParticleUnPause(this ParticleSystem[] particleSystems, bool withChildren = true)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].ParticleUnPause(withChildren);
			}
		}
	}
}
