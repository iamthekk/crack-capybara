using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix
{
	public class PetCollectionItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.petItem.Init();
			this.btnItem.onClick.AddListener(new UnityAction(this.OnItemClick));
		}

		protected override void OnDeInit()
		{
			this.btnItem.onClick.RemoveListener(new UnityAction(this.OnItemClick));
		}

		public void SetData(int petConfigId, int requireStar)
		{
			this.petConfigId = petConfigId;
			PetData petData = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId(petConfigId);
			if (petData != null)
			{
				this.petItem.RefreshData(petData);
				this.petItem.SetMaskActive(false);
			}
			else
			{
				petData = PetData.CreateFakeData(petConfigId, 1, 0);
				this.petItem.RefreshData(petData);
				this.petItem.SetMaskActive(true);
			}
			this.petItem.SetFormationTypeActive(false);
			this.txtName.text = Singleton<LanguageManager>.Instance.GetInfoByID(petData.nameId);
		}

		private void OnItemClick()
		{
			Action<PetCollectionItem> action = this.onItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton btnItem;

		public PetItem petItem;

		public CustomText txtName;

		[NonSerialized]
		public int petConfigId;

		[NonSerialized]
		public Action<PetCollectionItem> onItemClickCallback;
	}
}
