using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class UISpineModelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public async Task ShowModel(int modelId, string defaultAni, bool isLoopAni)
		{
			await this.ShowModel(modelId, 0, defaultAni, isLoopAni);
		}

		public void SetScale(float scale)
		{
			if (this.modelShow != null)
			{
				this.modelShow.transform.localScale = scale * Vector3.one;
			}
		}

		public Task ShowModel(int modelId, int skinId, string defaultAni, bool isLoopAni)
		{
			UISpineModelItem.<ShowModel>d__6 <ShowModel>d__;
			<ShowModel>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowModel>d__.<>4__this = this;
			<ShowModel>d__.modelId = modelId;
			<ShowModel>d__.skinId = skinId;
			<ShowModel>d__.defaultAni = defaultAni;
			<ShowModel>d__.isLoopAni = isLoopAni;
			<ShowModel>d__.<>1__state = -1;
			<ShowModel>d__.<>t__builder.Start<UISpineModelItem.<ShowModel>d__6>(ref <ShowModel>d__);
			return <ShowModel>d__.<>t__builder.Task;
		}

		public async Task ShowMemberModel(int memberId, string defaultAni, bool isLoopAni)
		{
			this.modelShow.gameObject.SetActive(false);
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(memberId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("找不到memberId={0}", memberId));
			}
			else
			{
				await this.ShowModel(elementById.modelID, elementById.initSkinID, defaultAni, isLoopAni);
			}
		}

		public Animation GetAni(string aniName)
		{
			if (this.modelShow == null || this.modelShow.AnimationState == null || this.modelShow.AnimationState.Data == null || this.modelShow.AnimationState.Data.SkeletonData == null)
			{
				return null;
			}
			return this.modelShow.AnimationState.Data.SkeletonData.FindAnimation(aniName);
		}

		public void PlayAnimation(string aniName, bool isLoop)
		{
			if (this.GetAni(aniName) != null)
			{
				this.modelShow.AnimationState.SetAnimation(0, aniName, isLoop);
			}
		}

		public void AddAnimation(string aniName, bool isLoop, float delay = 0f)
		{
			if (this.GetAni(aniName) != null)
			{
				this.modelShow.AnimationState.AddAnimation(0, aniName, isLoop, delay);
			}
		}

		public void SetAnimationTimeScale(float timeScale)
		{
			this.modelShow.timeScale = timeScale;
		}

		public bool IsShow()
		{
			return this.modelShow.gameObject.activeSelf;
		}

		public float GetAnimationDuration(string aniName)
		{
			Animation ani = this.GetAni(aniName);
			if (ani != null)
			{
				return ani.Duration;
			}
			return 0f;
		}

		public void SetSkin(int skinId)
		{
			string text = "";
			if (skinId > 0)
			{
				GameMember_skin elementById = GameApp.Table.GetManager().GetGameMember_skinModelInstance().GetElementById(skinId);
				if (elementById == null)
				{
					HLog.LogError(string.Format("找不到皮肤id={0}", skinId));
					return;
				}
				text = elementById.skin;
			}
			if (!string.IsNullOrEmpty(text) && this.modelShow.Skeleton != null)
			{
				this.modelShow.Skeleton.SetSkin(text);
				this.modelShow.Skeleton.SetSlotsToSetupPose();
			}
		}

		public void PauseAnimation(string aniName)
		{
			this.modelShow.AnimationState.SetAnimation(0, aniName, false);
			this.modelShow.AnimationState.Update(0f);
			this.modelShow.AnimationState.Apply(this.modelShow.Skeleton);
			this.modelShow.timeScale = 0f;
		}

		public void ResumeAnimation(string aniName, bool isLoop)
		{
			this.modelShow.timeScale = 1f;
			this.PlayAnimation(aniName, isLoop);
		}

		public SkeletonGraphic modelShow;
	}
}
