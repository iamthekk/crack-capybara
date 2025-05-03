using System;
using Coffee.UIExtensions;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class LegacySkillPowerItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.particle1.gameObject.SetActive(false);
			this.particle2.gameObject.SetActive(false);
			this.goFg.SetActive(false);
			this.imgIcon.onFinished.AddListener(new UnityAction<string, string>(this.OnSkillIconLoadComplete));
			if (this.txtProgress != null)
			{
				this.txtProgress.gameObject.SetActive(false);
			}
		}

		protected override void OnDeInit()
		{
			this.particle1.gameObject.SetActive(false);
			this.particle2.gameObject.SetActive(false);
			this.goFg.SetActive(false);
		}

		public void SetLegacySkill(int skillId)
		{
			this.skillId = skillId;
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(skillId);
			if (elementById == null)
			{
				return;
			}
			this.imgIcon.SetImage(elementById.iconAtlasID, elementById.icon);
		}

		private void OnSkillIconLoadComplete(string atlasPath, string spriteName)
		{
			this.isResReady = true;
			this.UpdateMask();
			if (this.maxProgress > 0L)
			{
				this.UpdateProgress(this.curProgress, this.maxProgress);
			}
			else
			{
				this.UpdateProgress(0L, 10000L);
			}
			this.goFg.SetActive(true);
		}

		public void UpdateProgress(long cur, long max)
		{
			if (max <= 0L)
			{
				HLog.LogError(string.Format("legacySkillId:{0} legacyPowerMax <= 0", this.skillId));
				GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.skillId);
				if (elementById != null && elementById.legacyPowerMax > 0)
				{
					max = (long)elementById.legacyPowerMax;
				}
				else
				{
					max = 10000L;
				}
			}
			this.curProgress = cur;
			this.maxProgress = max;
			if (!this.isResReady)
			{
				return;
			}
			if (this.txtProgress != null)
			{
				this.txtProgress.text = string.Format("{0}/{1}", cur, max);
			}
			float num = 1f - this.imgMask.fillAmount;
			float num2 = Mathf.Clamp01((float)cur / (float)max);
			float num3 = 1f - num2;
			ShortcutExtensions46.DOFillAmount(this.imgMask, num3, (num3 >= 0.999f) ? 0f : 0.2f);
			if (num2 < 1f)
			{
				if (this.particle1.gameObject.activeSelf)
				{
					this.particle1.gameObject.SetActive(false);
					this.particle2.gameObject.SetActive(false);
					return;
				}
			}
			else if (num2 > num && num2 >= 1f)
			{
				if (!this.particle1.gameObject.activeSelf)
				{
					this.particle1.gameObject.SetActive(true);
					this.particle2.gameObject.SetActive(true);
					this.particle1.Play();
					this.particle2.Play();
					return;
				}
			}
			else if (num2 >= 1f && !this.particle1.gameObject.activeSelf)
			{
				this.particle1.gameObject.SetActive(true);
				this.particle2.gameObject.SetActive(true);
				this.particle1.Play();
				this.particle2.Play();
			}
		}

		private void UpdateMask()
		{
			if (this.imgFrame.sprite == null || this.imgIcon.sprite == null)
			{
				return;
			}
			this.imgMask.sprite = this.imgIcon.sprite;
		}

		private Sprite MergeTexture(Image img1, Image img2)
		{
			Sprite sprite = img1.sprite;
			Sprite sprite2 = img2.sprite;
			Texture2D texture = img1.sprite.texture;
			Texture2D texture2 = img2.sprite.texture;
			Texture2D texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, 4, false);
			Color[] array = new Color[texture2D.width * texture2D.height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.clear;
			}
			texture2D.SetPixels(array);
			array = img1.sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
			int num = (int)sprite.textureRectOffset.x;
			int num2 = (int)sprite.textureRectOffset.y;
			texture2D.SetPixels(num, num2, (int)sprite.textureRect.width, (int)sprite.textureRect.height, array);
			texture2D.Apply();
			Texture2D texture2D2 = new Texture2D((int)sprite2.rect.width, (int)sprite2.rect.height, 4, false);
			Color[] array2 = new Color[texture2D2.width * texture2D2.height];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = Color.clear;
			}
			texture2D2.SetPixels(array2);
			array2 = img2.sprite.texture.GetPixels((int)sprite2.textureRect.x, (int)sprite2.textureRect.y, (int)sprite2.textureRect.width, (int)sprite2.textureRect.height);
			int num3 = (int)sprite2.textureRectOffset.x;
			int num4 = (int)sprite2.textureRectOffset.y;
			texture2D2.SetPixels(num3, num4, (int)sprite2.textureRect.width, (int)sprite2.textureRect.height, array2);
			texture2D2.Apply();
			int num5 = Mathf.Max((int)sprite.rect.width, (int)sprite2.rect.width);
			int num6 = Mathf.Max((int)sprite.rect.height, (int)sprite2.rect.height);
			RenderTexture temporary = RenderTexture.GetTemporary(num5, num6, 0, 0);
			Material material = new Material(this.mergeShader);
			material.SetTexture("_MainTex", texture2D);
			material.SetTexture("_OverlayTex", texture2D2);
			Graphics.Blit(null, temporary, material);
			RenderTexture.active = temporary;
			Texture2D texture2D3 = new Texture2D(temporary.width, temporary.height);
			texture2D3.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			texture2D3.Apply();
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(temporary);
			return Sprite.Create(texture2D3, new Rect(0f, 0f, (float)texture2D3.width, (float)texture2D3.height), new Vector2(0.5f, 0.5f));
		}

		public GameObject goFg;

		public CustomImage imgFrame;

		public CustomImage imgIcon;

		public CustomImage imgMask;

		public UIParticle particle1;

		public UIParticle particle2;

		public CustomText txtProgress;

		public Shader mergeShader;

		private int skillId;

		private long curProgress;

		private long maxProgress;

		private bool isResReady;
	}
}
