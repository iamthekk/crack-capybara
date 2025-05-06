using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Spine.Unity;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class EventPointBase : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.ctrlList.Count; i++)
			{
				this.ctrlList[i].DeInit();
			}
			this.ctrlList.Clear();
			this.spriteList.Clear();
		}

		public virtual void SetData(Chapter_eventPoint table)
		{
			this.config = table;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.ctrlList.Count; i++)
			{
				this.ctrlList[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected async Task Load()
		{
			int mapId = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.Config.mapId;
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			string matPrefix = "";
			int num = 0;
			string text = "";
			if (elementById != null)
			{
				matPrefix = elementById.matPrefix;
				num = elementById.bottomType;
				text = elementById.bottomPointName;
			}
			else
			{
				HLog.LogError(string.Format("Not found map id={0}", mapId));
			}
			Dictionary<string, GameObject> dic = ViewTools.CollectAllGameObjects(base.gameObject);
			if (this.config.bottomId > 0 && num > 0 && !string.IsNullOrEmpty(text))
			{
				GameObject point;
				if (dic.TryGetValue(text, out point))
				{
					int num2 = this.config.bottomId + num;
					Map_EventPointBottom elementById2 = GameApp.Table.GetManager().GetMap_EventPointBottomModelInstance().GetElementById(num2);
					if (elementById2 != null)
					{
						AsyncOperationHandle<GameObject> handler = GameApp.Resources.LoadAssetAsync<GameObject>(elementById2.path);
						await handler.Task;
						if (handler.Result != null)
						{
							Object.Instantiate<GameObject>(handler.Result).SetParentNormal(point, false);
						}
						handler = default(AsyncOperationHandle<GameObject>);
					}
				}
				point = null;
			}
			foreach (GameObject gameObject in dic.Values)
			{
				SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
				if (sr)
				{
					this.spriteList.Add(sr);
					if (!string.IsNullOrEmpty(matPrefix) && sr.material != null)
					{
						int num3 = sr.material.name.IndexOf('_');
						if (num3 >= 0)
						{
							string text2 = matPrefix + sr.material.name.Substring(num3).Replace(" (Instance)", "");
							AsyncOperationHandle<Material> handler2 = GameApp.Resources.LoadAssetAsync<Material>("Assets/_Resources/Prefab/Game/EventMaterial/Sprite/" + text2 + ".mat");
							await handler2.Task;
							if (handler2.Result != null)
							{
								sr.material = Object.Instantiate<Material>(handler2.Result);
							}
							handler2 = default(AsyncOperationHandle<Material>);
						}
					}
				}
				else
				{
					SkeletonAnimation spine = gameObject.GetComponent<SkeletonAnimation>();
					if (spine)
					{
						if (!this.GetGameObjectPath(gameObject).ToUpper().Contains("NPC"))
						{
							Renderer spineRenderer = gameObject.GetComponent<Renderer>();
							if (spineRenderer != null)
							{
								for (int i = 0; i < spineRenderer.sharedMaterials.Length; i++)
								{
									Material material = spineRenderer.sharedMaterials[i];
									if (!string.IsNullOrEmpty(matPrefix) && material != null)
									{
										string text3 = material.name.Replace(" (Instance)", "") + "_" + matPrefix;
										AsyncOperationHandle<Material> handler2 = GameApp.Resources.LoadAssetAsync<Material>("Assets/_Resources/Prefab/Game/EventMaterial/Spine/" + text3 + ".mat");
										await handler2.Task;
										if (handler2.Result != null)
										{
											Material material2 = Object.Instantiate<Material>(handler2.Result);
											spineRenderer.sharedMaterials[i].CopyPropertiesFromMaterial(material2);
										}
										handler2 = default(AsyncOperationHandle<Material>);
									}
								}
							}
							spineRenderer = null;
						}
						ColorRenderCtrl colorRenderCtrl = spine.gameObject.AddComponent<ColorRenderCtrl>();
						colorRenderCtrl.SetData(spine);
						colorRenderCtrl.Init();
						this.ctrlList.Add(colorRenderCtrl);
					}
					sr = null;
					spine = null;
				}
			}
			Dictionary<string, GameObject>.ValueCollection.Enumerator enumerator = default(Dictionary<string, GameObject>.ValueCollection.Enumerator);
		}

		public void SetVColor(float to, float duration)
		{
			for (int i = 0; i < this.ctrlList.Count; i++)
			{
				this.ctrlList[i].PlayVColor(to, duration, null);
			}
		}

		public void SetSpriteColor(float to, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			Color color;
			color..ctor(to, to, to);
			for (int i = 0; i < this.spriteList.Count; i++)
			{
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions43.DOColor(this.spriteList[i], color, duration));
			}
		}

		private string GetGameObjectPath(GameObject obj)
		{
			string text = obj.transform.name;
			Transform transform = obj.transform.parent;
			string text2 = "";
			while (!text2.Equals(base.gameObject.name) && transform != null)
			{
				text2 = transform.name;
				if (text2.Equals(base.gameObject.name))
				{
					break;
				}
				transform = transform.parent;
				text = text2 + "/" + text;
			}
			return text;
		}

		private const string SPRITE_MAT_PATH = "Assets/_Resources/Prefab/Game/EventMaterial/Sprite/";

		private const string SPINE_MAT_PATH = "Assets/_Resources/Prefab/Game/EventMaterial/Spine/";

		protected List<ColorRenderCtrl> ctrlList = new List<ColorRenderCtrl>();

		protected List<SpriteRenderer> spriteList = new List<SpriteRenderer>();

		protected Chapter_eventPoint config;
	}
}
