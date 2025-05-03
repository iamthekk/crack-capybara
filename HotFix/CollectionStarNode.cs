using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class CollectionStarNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetStar(int collectionStar)
		{
			int num = 0;
			string text = "";
			int num2 = 105;
			if (collectionStar > 0)
			{
				Collection_starColor elementById = GameApp.Table.GetManager().GetCollection_starColorModelInstance().GetElementById(collectionStar);
				if (elementById != null)
				{
					num = elementById.starNumber;
					text = elementById.iconStar;
					num2 = elementById.atlasID;
				}
				Atlas_atlas elementById2 = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(num2);
				string text2 = ((elementById2 != null) ? elementById2.path : "");
				for (int i = 0; i < this.m_imgStarList.Count; i++)
				{
					if (num > 0)
					{
						this.m_imgStarList[i].SetImage(text2, text);
					}
					this.m_imgStarList[i].gameObject.SetActive(num >= i + 1);
				}
				return;
			}
			for (int j = 0; j < this.m_imgStarList.Count; j++)
			{
				this.m_imgStarList[j].gameObject.SetActive(num >= j + 1);
			}
		}

		public List<CustomImage> m_imgStarList = new List<CustomImage>();
	}
}
