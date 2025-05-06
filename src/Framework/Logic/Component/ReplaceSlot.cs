using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class ReplaceSlot : MonoBehaviour
	{
		private void Start()
		{
			this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
			if (this.skeletonAnimation != null)
			{
				this.sourceMaterial = this.skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial;
			}
			this.skeleton = this.skeletonAnimation.skeleton;
		}

		private void Update()
		{
		}

		public void Apply()
		{
			this.Apply(this.SlotsInfos);
		}

		public void Apply(List<SlotsInfo> slots)
		{
			this.templateSkin = this.skeleton.Data.FindSkin(this.templateAttachmentsSkin);
			this.customSkin = this.customSkin ?? new Skin("custom skin");
			this.customSkin.CopySkin(this.templateSkin);
			for (int i = 0; i < slots.Count; i++)
			{
				int index = this.skeleton.Data.FindSlot(this.SlotsInfos[i].SlotName).Index;
				Attachment remappedClone = AttachmentCloneExtensions.GetRemappedClone(this.templateSkin.GetAttachment(index, this.SlotsInfos[i].SkinKey), this.SlotsInfos[i].图片, this.sourceMaterial, true, true, false, false, false);
				this.customSkin.SetAttachment(index, this.SlotsInfos[i].SkinKey, remappedClone);
			}
			if (this.repack)
			{
				Skin skin = new Skin("repacked skin");
				skin.AddSkin(this.skeleton.Data.DefaultSkin);
				skin.AddSkin(this.customSkin);
				if (this.runtimeMaterial)
				{
					Object.Destroy(this.runtimeMaterial);
				}
				if (this.runtimeAtlas)
				{
					Object.Destroy(this.runtimeAtlas);
				}
				skin = AtlasUtilities.GetRepackedSkin(skin, "repacked skin", this.sourceMaterial, ref this.runtimeMaterial, ref this.runtimeAtlas, 1024, 2, 4, false, true, false, null, null, null, null);
				this.skeleton.SetSkin(skin);
				if (this.bbFollower != null)
				{
					this.bbFollower.Initialize(true);
				}
			}
			else
			{
				this.skeleton.SetSkin(this.customSkin);
			}
			this.skeleton.SetSlotsToSetupPose();
			this.skeletonAnimation.Update(0f);
			AtlasUtilities.ClearCache();
			Resources.UnloadUnusedAssets();
		}

		public void SetSkinName(string skinName)
		{
			this.templateAttachmentsSkin = skinName;
		}

		public void ClearSkin()
		{
			if (this.templateAttachmentsSkin == null)
			{
				return;
			}
			this.customSkin.Clear();
			this.skeleton.SetSkin(this.templateAttachmentsSkin);
			this.skeleton.SetSlotsToSetupPose();
			this.skeletonAnimation.Update(0f);
			Resources.UnloadUnusedAssets();
		}

		[SpineSkin("", "", true, false, false)]
		public string templateAttachmentsSkin = "skin_1";

		public Material sourceMaterial;

		[Header("Runtime Repack")]
		public bool repack = true;

		public BoundingBoxFollower bbFollower;

		[Header("Do not assign")]
		public Texture2D runtimeAtlas;

		public Material runtimeMaterial;

		[Header("换装部位")]
		public List<SlotsInfo> SlotsInfos = new List<SlotsInfo>();

		private SkeletonAnimation skeletonAnimation;

		private Skeleton skeleton;

		private Skin templateSkin;

		private Skin customSkin;
	}
}
