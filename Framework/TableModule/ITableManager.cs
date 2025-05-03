using System;
using System.Collections.Generic;

namespace Framework.TableModule
{
	public interface ITableManager
	{
		void InitialiseLocalModels();

		void Load(string fileName, Action callBack);

		void LoadAll(Action callBack);

		void Loads(List<string> fileNames, Action callBack);

		void UnLoad(string fileName);

		void UnLoadALl();

		void DeInitialiseLocalModels();
	}
}
