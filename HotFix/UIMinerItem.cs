using System;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIMinerItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.initPos = this.child.anchoredPosition;
			this.minerSpineItem.Init();
			this.petSpineItem.Init();
			this.uiItem.Init();
			this.endX = (float)Screen.width + this.width;
		}

		protected override void OnDeInit()
		{
			this.minerSpineItem.DeInit();
			this.petSpineItem.DeInit();
			this.uiItem.DeInit();
			this.sequencePool.Clear(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isRun)
			{
				this.child.anchoredPosition += Vector2.right * this.speed * deltaTime * 10f;
				if (this.isRealGet)
				{
					if (this.child.anchoredPosition.x >= this.showGetX && !this.isShowGet)
					{
						this.isShowGet = true;
						Action<int, bool> action = this.onMoveFinish;
						if (action != null)
						{
							action(this.mIndex, this.isRealGet);
						}
					}
				}
				else if (this.child.anchoredPosition.x >= this.showFailX)
				{
					Action<int, bool> action2 = this.onMoveFinish;
					if (action2 != null)
					{
						action2(this.mIndex, this.isRealGet);
					}
					this.isRun = false;
					this.PlayFlyAni();
				}
				if (this.child.anchoredPosition.x >= this.endX)
				{
					this.isRun = false;
					this.ResetPos();
				}
			}
		}

		public void SetData(ItemData itemData, int index, bool isGet)
		{
			if (itemData == null)
			{
				return;
			}
			this.mIndex = index;
			this.isRealGet = isGet;
			this.isPet = itemData.itemType == ItemType.ePet;
			this.isRun = false;
			this.uiItem.SetData(itemData.ToPropData());
			this.uiItem.OnRefresh();
			if (this.fxRedLight)
			{
				this.fxRedLight.gameObject.SetActive(itemData.Data.quality == 6);
			}
			if (this.fxOrangeLight)
			{
				this.fxOrangeLight.gameObject.SetActive(itemData.Data.quality == 5);
			}
			if (this.isPet)
			{
				this.uiItem.gameObject.SetActiveSafe(false);
				this.minerSpineItem.gameObject.SetActiveSafe(false);
				this.petSpineItem.gameObject.SetActiveSafe(true);
				int num = 0;
				int num2 = 0;
				Pet_pet pet_pet = GameApp.Table.GetManager().GetPet_pet(itemData.ID);
				if (pet_pet != null)
				{
					GameMember_member gameMember_member = GameApp.Table.GetManager().GetGameMember_member(pet_pet.memberId);
					if (gameMember_member != null)
					{
						num = gameMember_member.modelID;
						num2 = gameMember_member.initSkinID;
					}
				}
				if (num > 0)
				{
					this.petSpineItem.ShowModel(num, num2, "Run", true);
				}
			}
			else
			{
				this.uiItem.gameObject.SetActiveSafe(true);
				this.minerSpineItem.gameObject.SetActiveSafe(true);
				this.petSpineItem.gameObject.SetActiveSafe(false);
				Mining_qualityModel mining_qualityModel = GameApp.Table.GetManager().GetMining_qualityModel(itemData.Data.quality);
				if (mining_qualityModel != null)
				{
					this.minerSpineItem.SetSkin(mining_qualityModel.skinId);
					this.minerSpineItem.PlayAnimation("Idle", true);
				}
			}
			this.ResetPos();
		}

		public void PlayAni(float failX, float effX, Action<int, bool> onFinish)
		{
			this.showFailX = failX;
			this.showGetX = effX;
			this.onMoveFinish = onFinish;
			this.ResetPos();
			float num = Utility.Math.Random(this.width + 5f, this.width + 10f);
			this.child.anchoredPosition = new Vector2((float)(-(float)Screen.width) / 2f - num * (float)this.mIndex, this.initPos.y);
			this.isShowGet = false;
			this.isRun = true;
		}

		private void PlayFlyAni()
		{
			GameApp.Sound.PlayClip(642, 1f);
			Sequence sequence = this.sequencePool.Get();
			float num = 1f;
			float num2 = 0.5f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.child, (float)(-(float)Screen.width) / 2f - this.width, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, (float)Screen.height / 2f, num2, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalRotate(this.child, new Vector3(0f, 0f, 50f), num2, 0));
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.ResetPos));
		}

		private void ResetPos()
		{
			this.child.anchoredPosition = new Vector2((float)(-(float)Screen.width) / 2f - this.width, this.initPos.y);
			this.child.localEulerAngles = Vector3.zero;
			this.child.localScale = Vector3.one;
		}

		public RectTransform child;

		public UISpineModelItem minerSpineItem;

		public UISpineModelItem petSpineItem;

		public UIItem uiItem;

		public ParticleSystem fxRedLight;

		public ParticleSystem fxOrangeLight;

		private float width = 150f;

		private float speed = 30f;

		private SequencePool sequencePool = new SequencePool();

		private int mIndex;

		private bool isRealGet;

		private bool isPet;

		private Vector2 initPos;

		private bool isShowGet;

		private bool isRun;

		private float endX;

		private float showGetX;

		private float showFailX;

		private Action<int, bool> onMoveFinish;
	}
}
