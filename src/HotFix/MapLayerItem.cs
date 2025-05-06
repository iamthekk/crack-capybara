using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class MapLayerItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				SpriteRenderer component = this.items[i].GetComponent<SpriteRenderer>();
				if (component)
				{
					this.sprites.Add(component);
				}
			}
		}

		protected override void OnDeInit()
		{
			this.sprites.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				if (this.items[i] != null)
				{
					this.MoveItem(this.items[i].transform, deltaTime);
				}
			}
		}

		public void SetData(Transform playerTrans)
		{
			this.playerTransform = playerTrans;
			this.lastItem = this.GetLastItem();
		}

		public async Task SetData(string idStr, int alpha, Transform playerTrans)
		{
			this.playerTransform = playerTrans;
			string[] idArr = idStr.Split(',', StringSplitOptions.None);
			List<Sprite> list = new List<Sprite>();
			if (idArr.Length == 1 || idArr.Length == 3)
			{
				for (int i = 0; i < idArr.Length; i++)
				{
					int num;
					if (int.TryParse(idArr[i], out num) && num > 0)
					{
						ArtMap_Map elementById = GameApp.Table.GetManager().GetArtMap_MapModelInstance().GetElementById(num);
						if (elementById != null && !string.IsNullOrEmpty(elementById.path))
						{
							AsyncOperationHandle<Sprite> sprite = GameApp.Resources.LoadAssetAsync<Sprite>(elementById.path);
							await sprite.Task;
							list.Add(sprite.Result);
							sprite = default(AsyncOperationHandle<Sprite>);
						}
						else
						{
							HLog.LogError(string.Format("[ArtMap_Map] not found id or path, id={0}", num));
						}
					}
				}
			}
			if (list.Count > 0)
			{
				base.gameObject.SetActiveSafe(true);
				for (int j = 0; j < this.sprites.Count; j++)
				{
					this.sprites[j].sprite = ((j < list.Count) ? list[j] : list[0]);
					Color color = this.sprites[j].color;
					color.a = (float)alpha;
					this.sprites[j].color = color;
				}
				this.lastItem = this.GetLastItem();
			}
			else
			{
				base.gameObject.SetActiveSafe(false);
			}
		}

		public Transform GetLastItem()
		{
			return this.items[this.items.Length - 1].transform;
		}

		private void MoveItem(Transform item, float deltaTime)
		{
			if (item == null || this.playerTransform == null || this.lastItem == null)
			{
				return;
			}
			this.curDistance = this.playerTransform.position.x - item.position.x;
			if (this.curDistance > 12.8f)
			{
				item.position = new Vector3(this.lastItem.position.x + 10.8f, this.lastItem.position.y, this.lastItem.position.z);
				this.lastItem = item;
			}
		}

		public void DoFade(float alpha, float time)
		{
			Sequence sequence = DOTween.Sequence();
			for (int i = 0; i < this.sprites.Count; i++)
			{
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions43.DOFade(this.sprites[i], alpha, time));
			}
		}

		public void ResetWordPosition()
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				Vector3 position = this.items[i].transform.position;
				position.x -= 1000f;
				this.items[i].transform.position = position;
			}
		}

		public GameObject[] items;

		private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

		private Transform lastItem;

		private Transform playerTransform;

		private const float WeightInteveal = 10.8f;

		private const float FarSpeedRatio = 0.8f;

		private float curDistance;
	}
}
