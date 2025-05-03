using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using HotFix.Client;
using Server;
using Spine;

namespace HotFix
{
	public class MemberAIStateDeath : AIMemberState
	{
		public MemberAIStateDeath(CMemberBase owner, MemberAIBase ai)
			: base(owner, ai)
		{
			this.m_alphaController = new RendererFloatController();
		}

		public override int GetName()
		{
			return 4;
		}

		public override void OnEnter()
		{
			if (base.m_owner.m_memberData.Camp == MemberCamp.Enemy)
			{
				base.m_owner.m_gameObject.transform.SetParent(null);
			}
			base.m_owner.m_frozen.ForceStop();
			base.m_owner.SetDeathLayer();
			base.m_owner.ShowHpHUD(false);
			base.m_owner.PlayAnimation("Death", delegate(TrackEntry entry)
			{
				base.m_owner.PlayAlpha(delegate
				{
					base.m_owner.Destroy();
				});
				base.m_owner.RemoveHpHUD();
			});
			MountSpinePlayer mountSpinePlayer = base.m_owner.mountSpinePlayer;
			if (mountSpinePlayer != null)
			{
				mountSpinePlayer.PlayAnimation("Death", delegate(TrackEntry entry)
				{
					MountSpinePlayer mountSpinePlayer2 = base.m_owner.mountSpinePlayer;
					if (mountSpinePlayer2 == null)
					{
						return;
					}
					ColorRender colorRender = mountSpinePlayer2.colorRender;
					if (colorRender == null)
					{
						return;
					}
					colorRender.PlayAlgha(null);
				});
			}
			GameApp.Sound.PlayClip(base.m_owner.m_memberData.m_dieSoundID, 1f);
		}

		public override void OnExit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			RendererFloatController alphaController = this.m_alphaController;
			if (alphaController == null)
			{
				return;
			}
			alphaController.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private RendererFloatController m_alphaController;
	}
}
