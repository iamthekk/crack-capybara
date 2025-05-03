using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMiningRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClickSelf));
			this.animator.Play("Idle");
			this.petItem.Init();
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.petItem.DeInit();
		}

		public void SetData(ItemData itemData)
		{
			if (itemData == null || itemData.Data == null)
			{
				return;
			}
			if (itemData.Data.itemType == 19)
			{
				this.iconObj.SetActive(false);
				this.petItem.gameObject.SetActive(true);
				float num = 0.5f;
				int num2 = 0;
				int num3 = 0;
				Pet_pet pet_pet = GameApp.Table.GetManager().GetPet_pet(itemData.ID);
				if (pet_pet != null)
				{
					GameMember_member gameMember_member = GameApp.Table.GetManager().GetGameMember_member(pet_pet.memberId);
					if (gameMember_member != null)
					{
						num2 = gameMember_member.modelID;
						num3 = gameMember_member.initSkinID;
						ArtMember_member artMember_member = GameApp.Table.GetManager().GetArtMember_member(num2);
						if (artMember_member != null)
						{
							num = artMember_member.miningScale;
						}
					}
				}
				if (num2 > 0)
				{
					this.petItem.ShowModel(num2, num3, "Idle", true);
					this.petItem.SetScale(num);
					return;
				}
			}
			else
			{
				this.iconObj.SetActive(true);
				this.petItem.gameObject.SetActive(false);
				string atlasPath = GameApp.Table.GetAtlasPath(itemData.Data.atlasID);
				this.imageIcon.SetImage(atlasPath, itemData.Data.icon);
				this.textNum.text = string.Format("x{0}", itemData.Count);
				this.animator.Play("Show");
			}
		}

		private void OnClickSelf()
		{
			if (this.isGet)
			{
				return;
			}
			this.isGet = true;
			NetworkUtils.Mining.DoGetMiningRewardRequest(delegate(bool result, GetMiningRewardResponse response)
			{
				this.isGet = false;
				if (result)
				{
					GameApp.SDK.Analyze.Track_MiningMine_Reward(response.CommonData.Reward);
				}
			}, null);
		}

		public CustomButton button;

		public CustomImage imageIcon;

		public CustomText textNum;

		public Animator animator;

		public GameObject iconObj;

		public UISpineModelItem petItem;

		private bool isGet;
	}
}
