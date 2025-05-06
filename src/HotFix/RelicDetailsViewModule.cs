using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Relic;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class RelicDetailsViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_relicDataModule = GameApp.Data.GetDataModule(DataName.RelicDataModule);
			this.m_attributeGroup.Init();
			this.m_starUpCostGroup.Init();
			this.m_levelUpCostGroup.Init();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as RelicDetailsViewModule.OpenData;
			this.m_relicTable = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(this.m_openData.m_relicData.m_id);
			this.m_itemStarGroup.SetData(this.m_relicTable.star);
			this.m_itemStarGroup.Init();
			this.m_starUpBt.onClick.AddListener(new UnityAction(this.OnClickStarUpBt));
			this.m_levelUpBt.onClick.AddListener(new UnityAction(this.OnClickLevelUpBt));
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			if (!this.IsShowBt())
			{
				this.uiPopCommon.SetRtfHeightByOffset(-200f);
			}
			this.OnRefreshUI();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.uiPopCommon.OnClick = null;
			this.m_itemStarGroup.DeInit();
			this.m_itemStarGroup = null;
			this.m_starUpBt.onClick.RemoveAllListeners();
			this.m_levelUpBt.onClick.RemoveAllListeners();
		}

		public override void OnDelete()
		{
			this.m_attributeGroup.DeInit();
			this.m_levelUpCostGroup.DeInit();
			this.m_starUpCostGroup.DeInit();
			this.m_relicDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBtn();
			}
		}

		private void OnClickCloseBtn()
		{
			GameApp.View.CloseView(ViewName.RelicDetailsViewModule, null);
		}

		private void OnClickLevelUpBt()
		{
			int num;
			if (!this.m_relicDataModule.IsCanUpdateLevelForLevel(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_level, out num))
			{
				return;
			}
			if (!this.m_relicDataModule.IsHaveUpdateLevelCost(this.m_openData.m_relicData))
			{
				return;
			}
			NetworkUtils.Relic.DoRelicStrengthRequest(this.m_openData.m_relicData.m_id, delegate(bool isOk, RelicStrengthResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.OnRefreshUI();
				if (this.m_levelTxt != null && this.m_levelGroup != null && this.m_levelGroup.activeSelf)
				{
					this.m_levelTxt.rectTransform.localScale = Vector3.one;
					Sequence sequence = new SequencePool().Get();
					Vector3 vector = Vector3.one * 2f;
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.m_levelTxt.transform, vector, 0.1f));
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.m_levelTxt.transform, Vector3.one, 0.1f));
				}
				if (this.m_attributeGroup != null)
				{
					this.m_attributeGroup.PlayParticleSystems(1);
				}
			});
		}

		private void OnClickStarUpBt()
		{
			int num;
			if (!this.m_relicDataModule.IsCanUpdateStarForQuality(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_quality, out num))
			{
				return;
			}
			if (!this.m_relicDataModule.IsHaveUpdateStarCost(this.m_openData.m_relicData))
			{
				return;
			}
			int num2;
			if (!this.m_relicDataModule.IsCanUpdateStarForLevelClamp(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_level, this.m_openData.m_relicData.m_quality, out num2))
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6109, new object[] { num2 }));
				return;
			}
			NetworkUtils.Relic.DoRelicStarRequest(this.m_openData.m_relicData.m_id, delegate(bool isOk, RelicStarResponse response)
			{
				if (!isOk)
				{
					return;
				}
				this.OnRefreshUI();
				if (this.m_relicTable.type != 2 && this.m_itemStarGroup != null)
				{
					this.m_itemStarGroup.PlayStar(this.m_openData.m_relicData.m_quality - 1);
				}
				if (this.m_attributeGroup != null)
				{
					this.m_attributeGroup.PlayParticleSystems(2);
				}
			});
		}

		private void OnRefreshUI()
		{
			if (this.m_openData == null || this.m_openData.m_relicData == null)
			{
				return;
			}
			if (this.m_title != null)
			{
				this.m_title.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_relicTable.NameId);
			}
			if (this.m_noteTxt != null)
			{
				this.m_noteTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_relicTable.DescId);
			}
			if (this.m_levelGroup != null)
			{
				this.m_levelGroup.SetActive(this.m_openData.m_isShowBt);
			}
			if (this.m_levelTxt != null)
			{
				this.m_levelTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6102, new object[] { this.m_openData.m_relicData.m_level });
				this.m_levelTxt.rectTransform.localScale = Vector3.one;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(this.m_relicTable.iconAtlasID);
			if (this.m_itemBaseBg != null)
			{
				this.m_itemBaseBg.SetImage(atlasPath, this.m_relicTable.baseName);
			}
			if (this.m_itemIcon != null)
			{
				this.m_itemIcon.SetImage(atlasPath, this.m_relicTable.iconName);
			}
			if (this.m_relicTable.type == 2)
			{
				if (this.m_itemStarGroup != null)
				{
					this.m_itemStarGroup.SetActive(false);
				}
			}
			else if (this.m_itemStarGroup != null)
			{
				this.m_itemStarGroup.SetCount(this.m_openData.m_relicData.m_quality);
				this.m_itemStarGroup.SetActive(true);
			}
			int num;
			bool flag = this.m_relicDataModule.IsCanUpdateLevelForLevel(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_level, out num);
			int num2;
			bool flag2 = this.m_relicDataModule.IsCanUpdateStarForQuality(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_quality, out num2);
			if (this.m_attributeGroup != null)
			{
				List<UIRelicDetailsAttributeGroup.NodeData> list = new List<UIRelicDetailsAttributeGroup.NodeData>();
				list.Add(new UIRelicDetailsAttributeGroup.NodeData
				{
					m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Title,
					m_name = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6107)
				});
				if (this.m_openData.m_isShowBt && this.m_openData.m_relicData.m_level > 0 && flag)
				{
					if (flag)
					{
						list.Add(new UIRelicDetailsAttributeGroup.NodeData
						{
							m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Note,
							m_name = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6113),
							m_layer = 1
						});
					}
					List<MergeAttributeData> updateLevelAttributes = this.m_relicDataModule.GetUpdateLevelAttributes(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_level);
					List<MergeAttributeData> updateLevelAttributes2 = this.m_relicDataModule.GetUpdateLevelAttributes(num);
					Dictionary<string, MergeAttributeData> dictionary = new Dictionary<string, MergeAttributeData>();
					if (updateLevelAttributes != null)
					{
						for (int i = 0; i < updateLevelAttributes.Count; i++)
						{
							MergeAttributeData mergeAttributeData = updateLevelAttributes[i];
							if (mergeAttributeData != null)
							{
								dictionary[mergeAttributeData.Header] = mergeAttributeData;
							}
						}
					}
					if (updateLevelAttributes2 != null)
					{
						for (int j = 0; j < updateLevelAttributes2.Count; j++)
						{
							MergeAttributeData mergeAttributeData2 = updateLevelAttributes2[j];
							if (mergeAttributeData2 != null)
							{
								Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData2.Header);
								if (elementById != null)
								{
									MergeAttributeData mergeAttributeData3;
									dictionary.TryGetValue(mergeAttributeData2.Header, out mergeAttributeData3);
									if (mergeAttributeData3 == null)
									{
										list.Add(new UIRelicDetailsAttributeGroup.NodeData
										{
											m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Attribute,
											m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId),
											m_from = this.GetAttributeString(mergeAttributeData2),
											m_layer = 1
										});
									}
									else
									{
										list.Add(new UIRelicDetailsAttributeGroup.NodeData
										{
											m_nodeType = UIRelicDetailsAttributeGroup.NodeType.AttributeNext,
											m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId),
											m_from = this.GetAttributeString(mergeAttributeData3),
											m_to = this.GetAttributeString(mergeAttributeData2),
											m_layer = 1
										});
									}
								}
							}
						}
					}
				}
				else
				{
					int num3 = ((this.m_openData.m_relicData.m_level <= 0) ? 1 : this.m_openData.m_relicData.m_level);
					List<MergeAttributeData> updateLevelAttributes3 = this.m_relicDataModule.GetUpdateLevelAttributes(this.m_openData.m_relicData.m_id, num3);
					if (updateLevelAttributes3 != null)
					{
						for (int k = 0; k < updateLevelAttributes3.Count; k++)
						{
							MergeAttributeData mergeAttributeData4 = updateLevelAttributes3[k];
							if (mergeAttributeData4 != null)
							{
								Attribute_AttrText elementById2 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData4.Header);
								if (elementById2 != null)
								{
									list.Add(new UIRelicDetailsAttributeGroup.NodeData
									{
										m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Attribute,
										m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.LanguageId),
										m_from = this.GetAttributeString(mergeAttributeData4),
										m_layer = 1
									});
								}
							}
						}
					}
				}
				list.Add(new UIRelicDetailsAttributeGroup.NodeData
				{
					m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Space
				});
				list.Add(new UIRelicDetailsAttributeGroup.NodeData
				{
					m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Title,
					m_name = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6108)
				});
				if (this.m_openData.m_isShowBt && this.m_openData.m_relicData.m_quality >= 0 && flag2)
				{
					if (flag2)
					{
						list.Add(new UIRelicDetailsAttributeGroup.NodeData
						{
							m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Note,
							m_name = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6114),
							m_layer = 2
						});
					}
					List<MergeAttributeData> updateStarAttributes = this.m_relicDataModule.GetUpdateStarAttributes(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_quality);
					List<MergeAttributeData> updateStarAttributes2 = this.m_relicDataModule.GetUpdateStarAttributes(num2);
					Dictionary<string, MergeAttributeData> dictionary2 = new Dictionary<string, MergeAttributeData>();
					if (updateStarAttributes != null)
					{
						for (int l = 0; l < updateStarAttributes.Count; l++)
						{
							MergeAttributeData mergeAttributeData5 = updateStarAttributes[l];
							if (mergeAttributeData5 != null)
							{
								dictionary2[mergeAttributeData5.Header] = mergeAttributeData5;
							}
						}
					}
					if (updateStarAttributes2 != null)
					{
						for (int m = 0; m < updateStarAttributes2.Count; m++)
						{
							MergeAttributeData mergeAttributeData6 = updateStarAttributes2[m];
							if (mergeAttributeData6 != null)
							{
								Attribute_AttrText elementById3 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData6.Header);
								if (elementById3 != null)
								{
									MergeAttributeData mergeAttributeData7;
									dictionary2.TryGetValue(mergeAttributeData6.Header, out mergeAttributeData7);
									if (mergeAttributeData7 == null)
									{
										list.Add(new UIRelicDetailsAttributeGroup.NodeData
										{
											m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Attribute,
											m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.LanguageId),
											m_from = this.GetAttributeString(mergeAttributeData6),
											m_layer = 2
										});
									}
									else
									{
										list.Add(new UIRelicDetailsAttributeGroup.NodeData
										{
											m_nodeType = UIRelicDetailsAttributeGroup.NodeType.AttributeNext,
											m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.LanguageId),
											m_from = this.GetAttributeString(mergeAttributeData7),
											m_to = this.GetAttributeString(mergeAttributeData6),
											m_layer = 2
										});
									}
								}
							}
						}
					}
				}
				else
				{
					int quality = this.m_openData.m_relicData.m_quality;
					List<MergeAttributeData> updateStarAttributes3 = this.m_relicDataModule.GetUpdateStarAttributes(this.m_openData.m_relicData.m_id, quality);
					if (updateStarAttributes3 != null)
					{
						for (int n = 0; n < updateStarAttributes3.Count; n++)
						{
							MergeAttributeData mergeAttributeData8 = updateStarAttributes3[n];
							if (mergeAttributeData8 != null)
							{
								Attribute_AttrText elementById4 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData8.Header);
								if (elementById4 != null)
								{
									list.Add(new UIRelicDetailsAttributeGroup.NodeData
									{
										m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Attribute,
										m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.LanguageId),
										m_from = this.GetAttributeString(mergeAttributeData8),
										m_layer = 2
									});
								}
							}
						}
					}
				}
				List<RelicDataModule.PermissionsData> updateStarPermissionsDatas = this.m_relicDataModule.GetUpdateStarPermissionsDatas(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_quality);
				if (updateStarPermissionsDatas != null)
				{
					for (int num4 = 0; num4 < updateStarPermissionsDatas.Count; num4++)
					{
						RelicDataModule.PermissionsData permissionsData = updateStarPermissionsDatas[num4];
						if (permissionsData != null)
						{
							list.Add(new UIRelicDetailsAttributeGroup.NodeData
							{
								m_nodeType = UIRelicDetailsAttributeGroup.NodeType.Note,
								m_name = Singleton<LanguageManager>.Instance.GetInfoByID(permissionsData.m_languageID, new object[] { permissionsData.GetValueString() }),
								m_layer = 2
							});
						}
					}
				}
				this.m_attributeGroup.RefreshData(list);
			}
			if (this.IsShowBt())
			{
				if (this.m_levelUpBt != null)
				{
					this.m_levelUpBt.gameObject.SetActive(true);
				}
				if (this.m_starUpBt != null)
				{
					this.m_starUpBt.gameObject.SetActive(true);
				}
				if (flag)
				{
					if (this.m_levelUpCostGroup != null)
					{
						this.m_levelUpCostGroup.gameObject.SetActive(true);
						List<ItemData> updateLevelCost = this.m_relicDataModule.GetUpdateLevelCost(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_level);
						this.m_levelUpCostGroup.RefreshUI(updateLevelCost);
					}
					if (this.m_levelUpBtTxt != null)
					{
						this.m_levelUpBtTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6104);
					}
					if (this.m_relicDataModule.IsHaveUpdateLevelCost(this.m_openData.m_relicData))
					{
						if (this.m_levelUpBtGray != null)
						{
							this.m_levelUpBtGray.Recovery();
						}
					}
					else if (this.m_levelUpBtGray != null)
					{
						this.m_levelUpBtGray.SetUIGray();
					}
				}
				else
				{
					if (this.m_levelUpCostGroup != null)
					{
						this.m_levelUpCostGroup.gameObject.SetActive(false);
					}
					if (this.m_levelUpBtTxt != null)
					{
						this.m_levelUpBtTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6106);
					}
					if (this.m_levelUpBtGray != null)
					{
						this.m_levelUpBtGray.SetUIGray();
					}
				}
				if (flag2)
				{
					if (this.m_starUpCostGroup != null)
					{
						this.m_starUpCostGroup.gameObject.SetActive(true);
						List<ItemData> updateStarCost = this.m_relicDataModule.GetUpdateStarCost(this.m_openData.m_relicData.m_id, this.m_openData.m_relicData.m_quality);
						this.m_starUpCostGroup.RefreshUI(updateStarCost);
					}
					if (this.m_starUpBtTxt != null)
					{
						this.m_starUpBtTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6103);
					}
					if (this.m_starUpBtGray != null)
					{
						this.m_starUpBtGray.Recovery();
					}
					if (this.m_relicDataModule.IsHaveUpdateStarCost(this.m_openData.m_relicData))
					{
						if (this.m_starUpBtGray != null)
						{
							this.m_starUpBtGray.Recovery();
							return;
						}
					}
					else if (this.m_starUpBtGray != null)
					{
						this.m_starUpBtGray.SetUIGray();
						return;
					}
				}
				else
				{
					if (this.m_starUpCostGroup != null)
					{
						this.m_starUpCostGroup.gameObject.SetActive(false);
					}
					if (this.m_starUpBtTxt != null)
					{
						this.m_starUpBtTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6105);
					}
					if (this.m_starUpBtGray != null)
					{
						this.m_starUpBtGray.SetUIGray();
						return;
					}
				}
			}
			else
			{
				if (this.m_starUpCostGroup != null)
				{
					this.m_starUpCostGroup.gameObject.SetActive(false);
				}
				if (this.m_levelUpCostGroup != null)
				{
					this.m_levelUpCostGroup.gameObject.SetActive(false);
				}
				if (this.m_levelUpBt != null)
				{
					this.m_levelUpBt.gameObject.SetActive(false);
				}
				if (this.m_starUpBt != null)
				{
					this.m_starUpBt.gameObject.SetActive(false);
				}
			}
		}

		private bool IsShowBt()
		{
			return this.m_openData.m_isShowBt && this.m_relicTable.type != 2;
		}

		private string GetAttributeString(MergeAttributeData data)
		{
			if (data.Header.Contains("%"))
			{
				return data.Value.ToString() + "%";
			}
			return DxxTools.FormatNumber(data.Value.AsLong());
		}

		public UIPopCommon uiPopCommon;

		public CustomText m_title;

		public CustomText m_noteTxt;

		[Header("LevelGroup")]
		public GameObject m_levelGroup;

		public CustomText m_levelTxt;

		[Header("Item Group")]
		public CustomImage m_itemIcon;

		public CustomImage m_itemBaseBg;

		public UIRelicStar m_itemStarGroup;

		[Header("Attributes Group")]
		public UIRelicDetailsAttributeGroup m_attributeGroup;

		[Header("Buttons")]
		public UICostCtrl m_starUpCostGroup;

		public UICostCtrl m_levelUpCostGroup;

		public CustomButton m_starUpBt;

		public CustomButton m_levelUpBt;

		public CustomText m_starUpBtTxt;

		public CustomText m_levelUpBtTxt;

		public UIGrays m_starUpBtGray;

		public UIGrays m_levelUpBtGray;

		private RelicDetailsViewModule.OpenData m_openData;

		private Relic_relic m_relicTable;

		private RelicDataModule m_relicDataModule;

		public class OpenData
		{
			public RelicData m_relicData;

			public bool m_isShowBt;
		}
	}
}
