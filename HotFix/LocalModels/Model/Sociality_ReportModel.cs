using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Sociality_ReportModel : BaseLocalModel
	{
		public Sociality_Report GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Sociality_Report> GetAllElements()
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

		public static readonly string fileName = "Sociality_Report";

		private Sociality_ReportModelImpl modelImpl = new Sociality_ReportModelImpl();
	}
}
