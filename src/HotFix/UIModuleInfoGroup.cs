using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using HotFix;
using LocalModels.Bean;
using UnityEngine.Events;

public class UIModuleInfoGroup : CustomBehaviour
{
	protected override void OnInit()
	{
		this.m_btnInfo.onClick.AddListener(new UnityAction(this.OnBtnInfoClick));
	}

	protected override void OnDeInit()
	{
		this.m_btnInfo.onClick.RemoveListener(new UnityAction(this.OnBtnInfoClick));
	}

	public void Refresh(int systemId)
	{
		Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(systemId);
		if (elementById != null)
		{
			this.m_txtSystemName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId);
			this.nameId = elementById.nameId;
			this.desId = elementById.infoId;
		}
	}

	private void OnBtnInfoClick()
	{
		if (!string.IsNullOrEmpty(this.nameId) && !string.IsNullOrEmpty(this.desId))
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
			{
				m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.nameId),
				m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.desId)
			};
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}
	}

	public CustomText m_txtSystemName;

	public CustomButton m_btnInfo;

	private string nameId;

	private string desId;
}
