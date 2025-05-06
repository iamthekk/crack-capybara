using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace HotFix
{
	public class EventRecordUIData
	{
		public void SetIndex(int idx)
		{
			this.index = idx;
		}

		public void ToRecordData(GameEventUIData uiData)
		{
			this.resId = uiData.eventResId;
			this.stage = uiData.stage;
			this.isRoot = uiData.isRoot;
			this.languageId = uiData.languageId;
			if (uiData.languageParams != null)
			{
				this.languageParam = this.LanguageParamToString(uiData.languageParams);
			}
			List<NodeAttParam> nodeAttList = uiData.GetNodeAttList();
			List<NodeItemParam> nodeItemList = uiData.GetNodeItemList();
			List<NodeSkillParam> nodeSkillList = uiData.GetNodeSkillList();
			List<string> nodeInfoList = uiData.GetNodeInfoList();
			List<NodeScoreParam> nodeScoreList = uiData.GetNodeScoreList();
			this.attArr = new string[nodeAttList.Count];
			this.itemArr = new string[nodeItemList.Count];
			this.skillArr = new string[nodeSkillList.Count];
			this.infoArr = new string[nodeInfoList.Count];
			this.scoreArr = new string[nodeScoreList.Count];
			for (int i = 0; i < nodeAttList.Count; i++)
			{
				this.attArr[i] = JsonManager.SerializeObject(nodeAttList[i]);
			}
			for (int j = 0; j < nodeItemList.Count; j++)
			{
				this.itemArr[j] = JsonManager.SerializeObject(nodeItemList[j]);
			}
			for (int k = 0; k < nodeSkillList.Count; k++)
			{
				this.skillArr[k] = JsonManager.SerializeObject(nodeSkillList[k]);
			}
			for (int l = 0; l < nodeInfoList.Count; l++)
			{
				this.infoArr[l] = nodeInfoList[l];
			}
			for (int m = 0; m < nodeScoreList.Count; m++)
			{
				this.scoreArr[m] = JsonManager.SerializeObject(nodeScoreList[m]);
			}
		}

		public GameEventUIData ToGameEventUIData()
		{
			object[] array = this.ToLanguageParam();
			List<NodeAttParam> list = new List<NodeAttParam>();
			List<NodeItemParam> list2 = new List<NodeItemParam>();
			List<NodeSkillParam> list3 = new List<NodeSkillParam>();
			List<NodeScoreParam> list4 = new List<NodeScoreParam>();
			if (this.attArr != null)
			{
				for (int i = 0; i < this.attArr.Length; i++)
				{
					NodeAttParam nodeAttParam = JsonManager.ToObject<NodeAttParam>(this.attArr[i]);
					list.Add(nodeAttParam);
				}
			}
			if (this.itemArr != null)
			{
				for (int j = 0; j < this.itemArr.Length; j++)
				{
					NodeItemParam nodeItemParam = JsonManager.ToObject<NodeItemParam>(this.itemArr[j]);
					list2.Add(nodeItemParam);
				}
			}
			if (this.skillArr != null)
			{
				for (int k = 0; k < this.skillArr.Length; k++)
				{
					NodeSkillParam nodeSkillParam = JsonManager.ToObject<NodeSkillParam>(this.skillArr[k]);
					list3.Add(nodeSkillParam);
				}
			}
			if (this.scoreArr != null)
			{
				for (int l = 0; l < this.scoreArr.Length; l++)
				{
					NodeScoreParam nodeScoreParam = JsonManager.ToObject<NodeScoreParam>(this.scoreArr[l]);
					list4.Add(nodeScoreParam);
				}
			}
			return GameEventUIDataCreator.Create(this.resId, this.stage, this.isRoot, this.languageId, array, list, list2, list3, this.infoArr.ToList<string>(), list4);
		}

		private string LanguageParamToString(object[] param)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < param.Length; i++)
			{
				stringBuilder.Append(param[i]);
				if (i != param.Length - 1)
				{
					stringBuilder.Append('|');
				}
			}
			return stringBuilder.ToString();
		}

		private object[] ToLanguageParam()
		{
			if (this.languageParam == null)
			{
				return null;
			}
			List<string> listString = this.languageParam.GetListString('|');
			object[] array = new object[listString.Count];
			for (int i = 0; i < listString.Count; i++)
			{
				array[i] = listString[i];
			}
			return array;
		}

		public int resId;

		public int stage;

		public bool isRoot;

		public string languageId;

		public string languageParam;

		public string[] attArr;

		public string[] itemArr;

		public string[] skillArr;

		public string[] infoArr;

		public string[] scoreArr;

		public int index;
	}
}
