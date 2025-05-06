using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.Platfrom;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMiningGridItem : CustomBehaviour
	{
		public GridDto GridData
		{
			get
			{
				return this.gridData;
			}
		}

		public int ServerPos
		{
			get
			{
				if (this.gridData != null)
				{
					return this.gridData.Pos;
				}
				return 0;
			}
		}

		protected override void OnInit()
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.buttonNormal.onClick.AddListener(new UnityAction(this.OnClickNormal));
			this.buttonOre.onClick.AddListener(new UnityAction(this.OnClickOre));
			this.buttonBomb.onClick.AddListener(new UnityAction(this.OnClickBomb));
			this.rewardItem.Init();
			this.spineEff.Init();
			this.spineBomb.Init();
			this.spineLight.Init();
			this.imageOreType.gameObject.SetActiveSafe(false);
			this.imageQuality.gameObject.SetActiveSafe(false);
			this.rewardItem.gameObject.SetActiveSafe(false);
			this.spineEff.gameObject.SetActiveSafe(false);
			this.spineBomb.gameObject.SetActiveSafe(false);
			this.spineLight.gameObject.SetActiveSafe(false);
			this.fxBoom.gameObject.SetActiveSafe(false);
			this.textKnockNum.text = "";
			Sound_sound elementById = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(632);
			if (elementById != null)
			{
				this.buttonNormal.m_audioPath = elementById.path;
			}
			Sound_sound elementById2 = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(633);
			if (elementById2 != null)
			{
				this.buttonOre.m_audioPath = elementById2.path;
			}
			this.buttonBomb.m_audioPath = string.Empty;
		}

		protected override void OnDeInit()
		{
			this.buttonNormal.onClick.RemoveListener(new UnityAction(this.OnClickNormal));
			this.buttonOre.onClick.RemoveListener(new UnityAction(this.OnClickOre));
			this.buttonBomb.onClick.RemoveListener(new UnityAction(this.OnClickBomb));
			this.rewardItem.DeInit();
			this.spineEff.DeInit();
			this.spineBomb.DeInit();
			this.spineLight.DeInit();
		}

		public void SetData(GridDto gridDto)
		{
			if (gridDto == null)
			{
				return;
			}
			this.gridData = gridDto;
			this.Refresh();
		}

		public async void Refresh()
		{
			if (this.gridData != null)
			{
				this.oreBuild = GameApp.Table.GetManager().GetMining_oreBuild(this.gridData.OreBuildId);
				if (this.oreBuild == null)
				{
					HLog.LogError(string.Format("Table Mining_oreBuild not found id= {0}", this.gridData.OreBuildId));
				}
				else
				{
					this.oreRes = GameApp.Table.GetManager().GetMining_oreRes(this.oreBuild.oreResId);
					if (this.oreRes == null)
					{
						HLog.LogError(string.Format("Table Mining_oreRes not found id= {0}", this.oreBuild.oreResId));
					}
					else
					{
						string atlasPath = GameApp.Table.GetAtlasPath(this.oreRes.atlas);
						if (!string.IsNullOrEmpty(atlasPath))
						{
							this.imageOreType.SetImage(atlasPath, this.oreRes.icon);
						}
						this.normalOreObj.SetActiveSafe(this.gridData.Status > 0);
						MiningOreType oreType = (MiningOreType)this.oreBuild.oreType;
						switch (oreType)
						{
						case MiningOreType.Door:
						case MiningOreType.Key:
							break;
						case MiningOreType.Ore_Normal:
							this.imageOreType.gameObject.SetActiveSafe(false);
							this.imageQuality.gameObject.SetActiveSafe(false);
							this.spineBomb.gameObject.SetActiveSafe(false);
							this.textKnockNum.text = "";
							if (this.gridData.Item != null && this.gridData.Item.Count > 0)
							{
								this.rewardItem.gameObject.SetActiveSafe(true);
								this.rewardItem.SetData(this.gridData.Item[0].ToItemData());
								goto IL_04DA;
							}
							this.rewardItem.gameObject.SetActiveSafe(false);
							goto IL_04DA;
						case MiningOreType.Ore_Special_1:
						case MiningOreType.Ore_Special_2:
						case MiningOreType.Ore_Special_3:
							this.spineBomb.gameObject.SetActiveSafe(false);
							if (this.gridData.Floors > 0)
							{
								this.imageOreType.gameObject.SetActiveSafe(true);
								this.textKnockNum.text = ((this.gridData.Floors > 1) ? this.gridData.Floors.ToString() : "");
								this.rewardItem.gameObject.SetActiveSafe(false);
								this.RefreshGrade(this.gridData.Grade);
								goto IL_04DA;
							}
							this.imageOreType.gameObject.SetActiveSafe(false);
							this.imageQuality.gameObject.SetActiveSafe(false);
							this.textKnockNum.text = "";
							if (this.gridData.Item != null && this.gridData.Item.Count > 0)
							{
								this.rewardItem.gameObject.SetActiveSafe(true);
								this.rewardItem.SetData(this.gridData.Item[0].ToItemData());
								goto IL_04DA;
							}
							this.rewardItem.gameObject.SetActiveSafe(false);
							goto IL_04DA;
						default:
							if (oreType != MiningOreType.Bomb)
							{
								if (oreType != MiningOreType.Treasure)
								{
									goto IL_04DA;
								}
								this.spineBomb.gameObject.SetActiveSafe(false);
								this.imageOreType.gameObject.SetActiveSafe(false);
								this.imageQuality.gameObject.SetActiveSafe(false);
								this.rewardItem.gameObject.SetActiveSafe(false);
								this.textKnockNum.text = "";
								goto IL_04DA;
							}
							break;
						}
						this.imageQuality.gameObject.SetActiveSafe(false);
						this.rewardItem.gameObject.SetActiveSafe(false);
						this.textKnockNum.text = "";
						if (this.oreBuild.oreType == 101)
						{
							this.imageOreType.gameObject.SetActiveSafe(false);
							await this.spineBomb.ShowModel(this.oreRes.model, "ZhaDanA_2", true);
							if (this.gridData.Status > 0)
							{
								this.spineBomb.gameObject.SetActiveSafe(true);
								this.spineBomb.PauseAnimation("ZhaDanA_2");
							}
							else
							{
								this.spineBomb.gameObject.SetActiveSafe(this.gridData.BombStatus == 1);
								if (this.spineBomb.gameObject.activeSelf)
								{
									this.spineBomb.ResumeAnimation("ZhaDanA_2", true);
								}
							}
						}
						else
						{
							this.imageOreType.gameObject.SetActiveSafe(true);
							this.spineBomb.gameObject.SetActiveSafe(false);
						}
						IL_04DA:;
					}
				}
			}
		}

		private void RefreshGrade(int grade)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(130);
			this.imageQuality.SetImage(atlasPath, this.GetQuality(grade));
			this.imageQuality.gameObject.SetActiveSafe(true);
		}

		private void PlayMiningAni(bool isGrade)
		{
			EventArgsMining eventArgsMining = new EventArgsMining();
			eventArgsMining.SetData(this, isGrade);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_PlayMining, eventArgsMining);
		}

		public void ShowMiningEffect()
		{
			float animationDuration = this.spineEff.GetAnimationDuration("SuiShi");
			this.spineEff.gameObject.SetActiveSafe(true);
			this.spineEff.PlayAnimation("SuiShi", false);
			this.normalOreObj.SetActiveSafe(false);
			DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
			{
				if (this.spineEff)
				{
					this.spineEff.gameObject.SetActiveSafe(false);
				}
			});
		}

		public void ShowGradeEffect()
		{
			string text;
			switch (this.gridData.Grade)
			{
			case 2:
				text = "Guang_Lv";
				break;
			case 3:
				text = "Guang_Lan";
				break;
			case 4:
				text = "Guang_Zi";
				break;
			case 5:
				text = "Guang_Jin";
				break;
			case 6:
				text = "Guang_Hong";
				break;
			default:
				text = "Guang_Bai";
				break;
			}
			float animationDuration = this.spineLight.GetAnimationDuration(text);
			this.spineLight.gameObject.SetActiveSafe(true);
			this.spineLight.PlayAnimation(text, false);
			DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
			{
				if (this.spineLight)
				{
					this.spineLight.gameObject.SetActiveSafe(false);
				}
			});
		}

		public void PlayBombAni(RepeatedField<GridDto> boomGrids)
		{
			if (this.spineBomb == null)
			{
				return;
			}
			this.spineBomb.gameObject.SetActiveSafe(true);
			this.spineBomb.PlayAnimation("ZhaDanA_3", true);
			int num = 1000;
			DelayCall.Instance.CallOnce(num, delegate
			{
				if (this.spineBomb)
				{
					this.spineBomb.PlayAnimation("ZhaDanA_5", false);
				}
			});
			float animationDuration = this.spineBomb.GetAnimationDuration("ZhaDanA_3");
			int num2 = num + (int)(animationDuration * 1000f);
			DelayCall.Instance.CallOnce(num2, delegate
			{
				if (boomGrids != null && boomGrids.Count > 0)
				{
					EventArgsMiningBomb eventArgsMiningBomb = new EventArgsMiningBomb();
					eventArgsMiningBomb.SetData(boomGrids);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Mining_Bomb, eventArgsMiningBomb);
					this.buttonBomb.enabled = true;
				}
				EventArgsBool eventArgsBool = new EventArgsBool();
				eventArgsBool.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_ShowClickMask, eventArgsBool);
			});
		}

		public void ShowBoomEffect()
		{
			this.fxBoom.gameObject.SetActiveSafe(true);
			this.fxBoom.Play();
			DelayCall.Instance.CallOnce(1000, delegate
			{
				if (this.fxBoom != null)
				{
					this.fxBoom.gameObject.SetActiveSafe(false);
					this.Refresh();
				}
			});
		}

		private string GetQuality(int grade)
		{
			switch (grade)
			{
			case 2:
				return "mining_light_green";
			case 3:
				return "mining_light_blue";
			case 4:
				return "mining_light_purple";
			case 5:
				return "mining_light_orange";
			case 6:
				return "mining_light_red";
			default:
				return "mining_light_white";
			}
		}

		private bool UseCacheTicket()
		{
			if (this.miningDataModule.cacheTicket.IsDataValid() && this.miningDataModule.cacheTicket.mVariable > 0)
			{
				this.miningDataModule.UseCacheTicket();
				return true;
			}
			return false;
		}

		private void OnClickNormal()
		{
			if (this.miningDataModule.IsMiningDisabled())
			{
				string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("mining_network_tip"), Array.Empty<object>());
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("mining_network_tip_ok");
				DxxTools.UI.OpenPopCommon(text, null, string.Empty, infoByID, string.Empty, false, 2);
				return;
			}
			if (this.gridData.Status > 0)
			{
				if (this.UseCacheTicket())
				{
					this.miningDataModule.CacheMiningPos(this.gridData.Pos);
					this.ShowMiningEffect();
					this.SendMiningMsg(delegate
					{
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_RefreshInfo, null);
						this.Refresh();
					});
					return;
				}
				this.ShowNotEnoughTip();
			}
		}

		private void OnClickBomb()
		{
			if (this.gridData.BombStatus == 1 && this.oreBuild.oreType == 101)
			{
				EventArgsBool eventArgsBool = new EventArgsBool();
				eventArgsBool.SetData(true);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_ShowClickMask, eventArgsBool);
				this.buttonBomb.enabled = false;
				NetworkUtils.Mining.DoOpenBombRequest(this.gridData.Pos, delegate(bool result, OpenBombResponse response)
				{
					if (result)
					{
						GameApp.Sound.PlayClip(633, 1f);
						this.PlayBombAni(response.Grids);
						return;
					}
					this.buttonBomb.enabled = true;
					EventArgsBool eventArgsBool2 = new EventArgsBool();
					eventArgsBool2.SetData(false);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_ShowClickMask, eventArgsBool2);
				});
			}
		}

		private void OnClickOre()
		{
			if (this.gridData.Status == 0)
			{
				switch (this.oreBuild.oreType)
				{
				case 1:
				{
					MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
					if (!dataModule.IsHaveKey)
					{
						this.ShowInfoTip(this.oreRes.languageId, this.oreRes.desId, base.transform.position, 200f);
						return;
					}
					if (dataModule.IsNotGetReward())
					{
						NetworkUtils.Mining.DoGetMiningRewardRequest(delegate(bool result, GetMiningRewardResponse response)
						{
							if (result)
							{
								GameApp.SDK.Analyze.Track_MiningMine_Reward(response.CommonData.Reward);
							}
						}, new Action(this.OnClickOre));
						return;
					}
					this.buttonOre.enabled = false;
					NetworkUtils.Mining.DoOpenNextDoorRequest(delegate(bool result, OpenNextDoorResponse response)
					{
						this.buttonOre.enabled = true;
						if (result)
						{
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_NextFloor, null);
						}
					});
					return;
				}
				case 2:
					this.ShowInfoTip(this.oreRes.languageId, this.oreRes.desId, base.transform.position, 200f);
					return;
				case 3:
					break;
				case 4:
				case 5:
				case 6:
					if (this.UseCacheTicket())
					{
						if (this.gridData.Floors > 0)
						{
							this.buttonOre.enabled = false;
							this.PlayMiningAni(true);
							this.SendMiningMsg(delegate
							{
								this.buttonOre.enabled = true;
								GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_RefreshInfo, null);
								this.ShowGradeEffect();
								this.RefreshGrade(this.gridData.Grade);
								this.Refresh();
							});
							return;
						}
					}
					else
					{
						this.ShowNotEnoughTip();
					}
					break;
				default:
					return;
				}
			}
		}

		private void SendMiningMsg(Action onResult)
		{
			NetworkUtils.Mining.DoMiningRequest(0U, new List<int> { this.gridData.Pos }, delegate(bool result, DoMiningResponse response)
			{
				if (result)
				{
					int i = 0;
					while (i < response.MiningInfoDto.Grids.Count)
					{
						GridDto gridDto = response.MiningInfoDto.Grids[i];
						if (gridDto.Pos == this.gridData.Pos)
						{
							this.gridData = gridDto;
							Action onResult2 = onResult;
							if (onResult2 == null)
							{
								return;
							}
							onResult2();
							return;
						}
						else
						{
							i++;
						}
					}
				}
			});
		}

		private void ShowNotEnoughTip()
		{
			string text = "";
			int mining_Ticket_ID = GameConfig.Mining_Ticket_ID;
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(mining_Ticket_ID);
			if (item_Item != null)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(item_Item.nameID);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_mining_disabled", new object[] { text });
			GameApp.View.ShowStringTip(infoByID);
		}

		private void ShowInfoTip(string nameLanguageId, string infoLanguageId, Vector3 position, float offsetY)
		{
			new InfoTipViewModule.InfoTipData
			{
				m_name = Singleton<LanguageManager>.Instance.GetInfoByID(nameLanguageId),
				m_info = Singleton<LanguageManager>.Instance.GetInfoByID(infoLanguageId),
				m_position = position,
				m_offsetY = offsetY
			}.Open();
		}

		public async Task AutoMining(GridDto dto)
		{
			if (dto != null)
			{
				this.gridData = dto;
				if (dto.Status == 0 && dto.Grade > 0)
				{
					this.ShowGradeEffect();
				}
				else
				{
					float animationDuration = this.spineEff.GetAnimationDuration("SuiShi");
					this.spineEff.gameObject.SetActiveSafe(true);
					this.spineEff.PlayAnimation("SuiShi", false);
					this.normalOreObj.SetActiveSafe(false);
					await TaskExpand.Delay((int)(animationDuration * 1000f));
					if (this.spineEff != null)
					{
						this.spineEff.gameObject.SetActiveSafe(false);
					}
				}
				this.Refresh();
			}
		}

		public void AutoBombAni()
		{
			this.spineBomb.PlayAnimation("ZhaDanA_5", false);
		}

		public async Task AutoBombEffect(GridDto dto)
		{
			if (dto != null)
			{
				this.gridData = dto;
			}
			this.fxBoom.gameObject.SetActiveSafe(true);
			this.fxBoom.Play();
			await TaskExpand.Delay(1000);
			this.fxBoom.gameObject.SetActiveSafe(false);
			this.Refresh();
		}

		public CustomImage imageOreType;

		public GameObject normalOreObj;

		public CustomImage imageQuality;

		public CustomText textKnockNum;

		public UIMiningRewardItem rewardItem;

		public CustomButton buttonNormal;

		public CustomButton buttonOre;

		public CustomButton buttonBomb;

		public UISpineModelItem spineEff;

		public UISpineModelItem spineLight;

		public UISpineModelItem spineBomb;

		public ParticleSystem fxBoom;

		private GridDto gridData;

		private Mining_oreBuild oreBuild;

		private Mining_oreRes oreRes;

		private MiningDataModule miningDataModule;
	}
}
