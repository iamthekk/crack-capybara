using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Framework.Logic.UI.UIParticleSystem
{
	[RequireComponent(typeof(ParticleSystem), typeof(CanvasRenderer))]
	public class UIParticleSystem : MaskableGraphic
	{
		public ParticleSystem ParticleSystem
		{
			get
			{
				return this.m_ParticleSystem;
			}
			set
			{
				if (SetPropertyUtility.SetClass<ParticleSystem>(ref this.m_ParticleSystem, value))
				{
					this.SetAllDirty();
				}
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		public UiParticleRenderMode RenderMode
		{
			get
			{
				return this.m_RenderMode;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<UiParticleRenderMode>(ref this.m_RenderMode, value))
				{
					this.SetAllDirty();
				}
			}
		}

		protected override void Awake()
		{
			ParticleSystem component = base.GetComponent<ParticleSystem>();
			ParticleSystemRenderer component2 = base.GetComponent<ParticleSystemRenderer>();
			if (this.m_Material == null)
			{
				this.m_Material = component2.sharedMaterial;
			}
			if (component2.renderMode == 1)
			{
				this.RenderMode = UiParticleRenderMode.StreachedBillboard;
			}
			base.Awake();
			this.ParticleSystem = component;
			this.m_ParticleSystemRenderer = component2;
		}

		public override void SetMaterialDirty()
		{
			base.SetMaterialDirty();
			if (this.m_ParticleSystemRenderer != null)
			{
				this.m_ParticleSystemRenderer.sharedMaterial = this.m_Material;
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (this.ParticleSystem == null)
			{
				base.OnPopulateMesh(toFill);
				return;
			}
			this.GenerateParticlesBillboards(toFill);
		}

		protected virtual void Update()
		{
			if (!this.m_IgnoreTimescale)
			{
				if (this.ParticleSystem != null && this.ParticleSystem.isPlaying)
				{
					this.SetVerticesDirty();
				}
			}
			else if (this.ParticleSystem != null)
			{
				this.ParticleSystem.Simulate(Time.unscaledDeltaTime, true, false);
				this.SetVerticesDirty();
			}
			if (this.m_ParticleSystemRenderer != null && this.m_ParticleSystemRenderer.enabled)
			{
				this.m_ParticleSystemRenderer.enabled = false;
			}
		}

		private void InitParticlesBuffer()
		{
			if (this.m_Particles == null || this.m_Particles.Length < this.ParticleSystem.main.maxParticles)
			{
				this.m_Particles = new ParticleSystem.Particle[this.ParticleSystem.main.maxParticles];
			}
		}

		private void GenerateParticlesBillboards(VertexHelper vh)
		{
			this.InitParticlesBuffer();
			int particles = this.ParticleSystem.GetParticles(this.m_Particles);
			vh.Clear();
			for (int i = 0; i < particles; i++)
			{
				this.DrawParticleBillboard(this.m_Particles[i], vh);
			}
		}

		private void DrawParticleBillboard(ParticleSystem.Particle particle, VertexHelper vh)
		{
			Vector3 vector = particle.position;
			Quaternion quaternion = Quaternion.Euler(particle.rotation3D);
			if (this.ParticleSystem.main.simulationSpace == 1)
			{
				vector = base.rectTransform.InverseTransformPoint(vector);
			}
			float num = particle.startLifetime - particle.remainingLifetime;
			float num2 = num / particle.startLifetime;
			Vector3 currentSize3D = particle.GetCurrentSize3D(this.ParticleSystem);
			if (this.m_RenderMode == UiParticleRenderMode.StreachedBillboard)
			{
				this.GetStrechedBillboardsSizeAndRotation(particle, num2, ref currentSize3D, out quaternion);
			}
			Vector3 vector2;
			vector2..ctor(-currentSize3D.x * 0.5f, currentSize3D.y * 0.5f);
			Vector3 vector3;
			vector3..ctor(currentSize3D.x * 0.5f, currentSize3D.y * 0.5f);
			Vector3 vector4;
			vector4..ctor(currentSize3D.x * 0.5f, -currentSize3D.y * 0.5f);
			Vector3 vector5;
			vector5..ctor(-currentSize3D.x * 0.5f, -currentSize3D.y * 0.5f);
			vector2 = quaternion * vector2 + vector;
			vector3 = quaternion * vector3 + vector;
			vector4 = quaternion * vector4 + vector;
			vector5 = quaternion * vector5 + vector;
			Color32 currentColor = particle.GetCurrentColor(this.ParticleSystem);
			int currentVertCount = vh.currentVertCount;
			Vector2[] array = new Vector2[4];
			if (!this.ParticleSystem.textureSheetAnimation.enabled)
			{
				this.EvaluateQuadUVs(array);
			}
			else
			{
				this.EvaluateTexturesheetUVs(particle, num, array);
			}
			vh.AddVert(vector5, currentColor, array[0]);
			vh.AddVert(vector2, currentColor, array[1]);
			vh.AddVert(vector3, currentColor, array[2]);
			vh.AddVert(vector4, currentColor, array[3]);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		private void EvaluateQuadUVs(Vector2[] uvs)
		{
			uvs[0] = new Vector2(0f, 0f);
			uvs[1] = new Vector2(0f, 1f);
			uvs[2] = new Vector2(1f, 1f);
			uvs[3] = new Vector2(1f, 0f);
		}

		private void EvaluateTexturesheetUVs(ParticleSystem.Particle particle, float timeAlive, Vector2[] uvs)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = this.ParticleSystem.textureSheetAnimation;
			float num = particle.startLifetime / (float)textureSheetAnimation.cycleCount;
			float num2 = timeAlive % num / num;
			int num3 = textureSheetAnimation.numTilesY * textureSheetAnimation.numTilesX;
			float num4 = textureSheetAnimation.frameOverTime.Evaluate(num2);
			float num5 = 0f;
			ParticleSystemAnimationType animation = textureSheetAnimation.animation;
			if (animation != null)
			{
				if (animation == 1)
				{
					num5 = Mathf.Clamp(Mathf.Floor(num4 * (float)textureSheetAnimation.numTilesX), 0f, (float)(textureSheetAnimation.numTilesX - 1));
					int num6 = textureSheetAnimation.rowIndex;
					if (textureSheetAnimation.rowMode == 1)
					{
						Random.InitState((int)particle.randomSeed);
						num6 = Random.Range(0, textureSheetAnimation.numTilesY);
					}
					num5 += (float)(num6 * textureSheetAnimation.numTilesX);
				}
			}
			else
			{
				num5 = Mathf.Clamp(Mathf.Floor(num4 * (float)num3), 0f, (float)(num3 - 1));
			}
			int num7 = (int)num5 % textureSheetAnimation.numTilesX;
			int num8 = (int)num5 / textureSheetAnimation.numTilesX;
			float num9 = 1f / (float)textureSheetAnimation.numTilesX;
			float num10 = 1f / (float)textureSheetAnimation.numTilesY;
			num8 = textureSheetAnimation.numTilesY - 1 - num8;
			float num11 = (float)num7 * num9;
			float num12 = (float)num8 * num10;
			float num13 = num11 + num9;
			float num14 = num12 + num10;
			uvs[0] = new Vector2(num11, num12);
			uvs[1] = new Vector2(num11, num14);
			uvs[2] = new Vector2(num13, num14);
			uvs[3] = new Vector2(num13, num12);
		}

		private void GetStrechedBillboardsSizeAndRotation(ParticleSystem.Particle particle, float timeAlive01, ref Vector3 size3D, out Quaternion rotation)
		{
			Vector3 zero = Vector3.zero;
			if (this.ParticleSystem.velocityOverLifetime.enabled)
			{
				zero.x = this.ParticleSystem.velocityOverLifetime.x.Evaluate(timeAlive01);
				zero.y = this.ParticleSystem.velocityOverLifetime.y.Evaluate(timeAlive01);
				zero.z = this.ParticleSystem.velocityOverLifetime.z.Evaluate(timeAlive01);
			}
			Vector3 vector = particle.velocity + zero;
			float num = Vector3.Angle(vector, Vector3.up);
			int num2 = ((vector.x < 0f) ? 1 : (-1));
			rotation = Quaternion.Euler(new Vector3(0f, 0f, num * (float)num2));
			size3D.y *= this.m_StretchedLenghScale;
			size3D += new Vector3(0f, this.m_StretchedSpeedScale * vector.magnitude);
		}

		[FormerlySerializedAs("m_ParticleSystem")]
		[SerializeField]
		private ParticleSystem m_ParticleSystem;

		[SerializeField]
		[Tooltip("Render mode of particles")]
		[FormerlySerializedAs("m_RenderMode")]
		private UiParticleRenderMode m_RenderMode;

		[FormerlySerializedAs("m_StretchedSpeedScale")]
		[Tooltip("Speed Scale for streched billboards")]
		[SerializeField]
		private float m_StretchedSpeedScale = 1f;

		[Tooltip("Speed Scale for streched billboards")]
		[SerializeField]
		[FormerlySerializedAs("m_StretchedLenghScale")]
		private float m_StretchedLenghScale = 1f;

		[SerializeField]
		[Tooltip("If true, particles ignore timescale")]
		[FormerlySerializedAs("m_IgnoreTimescale")]
		private bool m_IgnoreTimescale;

		private ParticleSystemRenderer m_ParticleSystemRenderer;

		private ParticleSystem.Particle[] m_Particles;
	}
}
