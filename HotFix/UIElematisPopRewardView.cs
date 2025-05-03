using System;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIElematisPopRewardView : MonoBehaviour
	{
		public async Task OnInit()
		{
		}

		public void OnDeInit()
		{
		}

		private async Task LoadNpcItem(ItemType itemType)
		{
		}

		public async void InitPet(int petId, int quality, bool isNew)
		{
			this.imgNew.gameObject.SetActive(isNew);
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petId);
			Quality_petQuality elementById2 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(quality);
			int memberId = elementById.memberId;
			ArtMember_member elementById3 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.textPetName.text = string.Concat(new string[] { "<color=", elementById2.colorNumDark, ">", infoByID, "</color>" });
			this.petTypeAttribute.SetData(petId, quality);
			this.spineModelItem.SetScale(elementById3.uiScale);
			this.spineModelItemAnim.SetScale(elementById3.uiScale);
			await this.spineModelItem.ShowModel(memberId, 0, "Idle", true);
			await this.spineModelItemAnim.ShowModel(memberId, 0, "Idle", true);
			this.spineModelItem.PlayAnimation("Idle", true);
			this.spineModelItemAnim.PlayAnimation("Idle", true);
		}

		private void Start()
		{
			base.gameObject.SetActive(false);
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		public UISpineModelItem spineModelItem;

		public UISpineModelItem spineModelItemAnim;

		public PetTypeAttribute petTypeAttribute;

		public CustomText textPetName;

		public CustomImage imgNew;
	}
}
