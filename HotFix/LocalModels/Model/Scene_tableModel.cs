using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Scene_tableModel : BaseLocalModel
	{
		public Scene_table GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Scene_table> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "Scene_table";

		private Scene_tableModelImpl modelImpl = new Scene_tableModelImpl();
	}
}
