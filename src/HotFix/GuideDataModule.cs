using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Platfrom;
using LocalModels.Bean;
using Proto.User;

namespace HotFix
{
	public class GuideDataModule : IDataModule
	{
		public GuideGroup CurGuideGroup { get; private set; }

		private void setLocalGuideMask(ulong mask)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			Utility.PlayerPrefs.SetString(string.Format("LocalGuideMask{0}", dataModule.userId), mask.ToString());
		}

		private ulong getLocalGuideMask()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return ulong.Parse(Utility.PlayerPrefs.GetString(string.Format("LocalGuideMask{0}", dataModule.userId), "0"));
		}

		public List<GuideGroup> GetListByTriggerKind(GuideTriggerKind kind)
		{
			if (kind == GuideTriggerKind.NULL)
			{
				return null;
			}
			List<GuideGroup> list;
			if (this.mTriggerDic.TryGetValue(kind, out list))
			{
				return list;
			}
			return null;
		}

		internal void Init()
		{
			int num = 0;
			this.CurGuideGroup = null;
			this.mTriggerDic.Clear();
			this.mAllGroup.Clear();
			this.mPVELevelCondition.Clear();
			string text = "";
			IList<Guide_guide> allElements = GameApp.Table.GetManager().GetGuide_guideModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				GuideData guideData = GuideData.Create(allElements[i]);
				if (guideData != null)
				{
					num++;
					GuideGroup guideGroup;
					if (this.mAllGroup.TryGetValue(guideData.Group, out guideGroup))
					{
						guideGroup.AddGuideData(guideData);
					}
					else
					{
						guideGroup = new GuideGroup(guideData.Group);
						guideGroup.AddGuideData(guideData);
						this.mAllGroup.Add(guideGroup.Group, guideGroup);
					}
					int needPVELevelID = guideData.NeedPVELevelID;
					if (needPVELevelID != 0)
					{
						GuideGroup guideGroup2;
						if (this.mPVELevelCondition.TryGetValue(needPVELevelID, out guideGroup2))
						{
							if (guideGroup2.Group != guideGroup.Group)
							{
								HLog.LogError("[新手引导]配表错误，两个引导需要相同的关卡作为条件:" + needPVELevelID.ToString());
							}
						}
						else
						{
							if (GuideController.ShowGuideLog)
							{
								text = text + needPVELevelID.ToString() + ",";
							}
							this.mPVELevelCondition.Add(needPVELevelID, guideGroup);
						}
					}
				}
			}
			bool showGuideLog = GuideController.ShowGuideLog;
			int num2 = 0;
			foreach (KeyValuePair<int, GuideGroup> keyValuePair in this.mAllGroup)
			{
				List<GuideTriggerKindData> firstTriggerKind = keyValuePair.Value.FirstTriggerKind;
				if (firstTriggerKind != null)
				{
					for (int j = 0; j < firstTriggerKind.Count; j++)
					{
						List<GuideGroup> list;
						if (!this.mTriggerDic.TryGetValue(firstTriggerKind[j].Kind, out list))
						{
							list = new List<GuideGroup>();
							this.mTriggerDic.Add(firstTriggerKind[j].Kind, list);
						}
						list.Add(keyValuePair.Value);
					}
				}
				num2++;
				keyValuePair.Value.ResetToFirst();
			}
			foreach (KeyValuePair<GuideTriggerKind, List<GuideGroup>> keyValuePair2 in this.mTriggerDic)
			{
				keyValuePair2.Value.Sort(new Comparison<GuideGroup>(GuideGroup.Sort));
			}
			this.ReCalcTargetDic();
			bool showGuideLog2 = GuideController.ShowGuideLog;
		}

		internal bool SetCurGuideGroup(GuideGroup guidegroup)
		{
			if (guidegroup == null)
			{
				this.CurGuideGroup = null;
				return true;
			}
			if (this.CurGuideGroup != null && this.CurGuideGroup != guidegroup)
			{
				if (this.CurGuideGroup.CompleteAll)
				{
					this.CurGuideGroup = null;
				}
				else if (!this.CurGuideGroup.IsGuideActive)
				{
					this.HangGuide(this.CurGuideGroup.Group);
				}
				else if (!this.CurGuideGroup.IfCanBreak)
				{
					HLog.LogError(string.Format("尝试激活新的引导，但是当前有引导并且不允许中断，{0}->{1}", this.CurGuideGroup.Group, guidegroup.Group));
				}
			}
			this.CurGuideGroup = guidegroup;
			this.CurGuideGroup.ResetToFirst();
			List<GuideTriggerKindData> firstTriggerKind = this.CurGuideGroup.FirstTriggerKind;
			if (firstTriggerKind != null)
			{
				for (int i = 0; i < firstTriggerKind.Count; i++)
				{
					List<GuideGroup> list;
					if (this.mTriggerDic.TryGetValue(firstTriggerKind[i].Kind, out list))
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].Group == this.CurGuideGroup.Group)
							{
								list.RemoveAt(j);
								break;
							}
						}
					}
				}
			}
			return true;
		}

		public bool ContinueHangGuide(GuideGroup guidegroup)
		{
			if (guidegroup == null)
			{
				HLog.LogError("[新手引导]尝试继续一个空的挂起引导！！！");
				return false;
			}
			bool flag = false;
			for (int i = 0; i < this.mHangGuideList.Count; i++)
			{
				if (this.mHangGuideList[i].Group == guidegroup.Group)
				{
					flag = true;
					this.CurGuideGroup = this.mHangGuideList[i];
					this.mHangGuideList.RemoveAt(i);
					break;
				}
			}
			if (!flag)
			{
				HLog.LogError("[新手引导]尝试继续一个非挂起的引导");
			}
			return flag;
		}

		internal void InitCompleteGuide(ulong guidemask, bool save = true)
		{
			ulong localGuideMask = this.getLocalGuideMask();
			ulong num = guidemask | localGuideMask;
			bool showGuideLog = GuideController.ShowGuideLog;
			this.mOverGuideMask = num;
			this.setLocalGuideMask(this.mOverGuideMask);
			this.InitCompletedGuideList(num);
			List<int> debugGuideList = this.DebugGuideList;
			Convert.ToString((long)num, 2) + " : ";
			foreach (KeyValuePair<GuideTriggerKind, List<GuideGroup>> keyValuePair in this.mTriggerDic)
			{
				List<GuideGroup> value = keyValuePair.Value;
				for (int i = 0; i < value.Count; i++)
				{
					if (this.mOverGuide.ContainsKey(value[i].Group))
					{
						value[i].SkipAll();
						value.RemoveAt(i);
						i--;
					}
				}
			}
			this.ReCalcTargetDic();
			bool showGuideLog2 = GuideController.ShowGuideLog;
			if (save && this.mOverGuideMask != guidemask)
			{
				this.SaveCompletedGuideList();
			}
		}

		private void InitCompletedGuideList(string str)
		{
			this.mOverGuide.Clear();
			if (!string.IsNullOrEmpty(str))
			{
				string[] array = str.Split('|', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					int num;
					if (int.TryParse(array[i], out num) && !this.mOverGuide.ContainsKey(num))
					{
						this.mOverGuide.Add(num, true);
					}
				}
			}
		}

		private void InitCompletedGuideList(ulong guidemask)
		{
			this.mOverGuide.Clear();
			for (int i = 0; i < 60; i++)
			{
				if (((guidemask >> i) & 1UL) == 1UL && !this.mOverGuide.ContainsKey(i))
				{
					this.mOverGuide.Add(i, true);
				}
			}
		}

		private async void ReCalcTargetDic()
		{
			await TaskExpand.Yield();
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (KeyValuePair<int, GuideGroup> keyValuePair in this.mAllGroup)
			{
				if (!this.mOverGuide.ContainsKey(keyValuePair.Key))
				{
					IList<GuideData> allGuide = keyValuePair.Value.AllGuide;
					for (int i = 0; i < allGuide.Count; i++)
					{
						if (allGuide[i] != null)
						{
							dictionary[allGuide[i].GuideTargetName] = true;
						}
					}
				}
			}
			if (GuideController.ShowGuideLog)
			{
				string text = "[新手引导] 尚未完成的引导目标： ";
				foreach (KeyValuePair<string, bool> keyValuePair2 in dictionary)
				{
					text = text + keyValuePair2.Key + "|";
				}
			}
			this.mGuideTargetFlag = dictionary;
		}

		public bool IfNeedGuideTarget(string target)
		{
			bool flag;
			return this.mGuideTargetFlag.TryGetValue(target, out flag) && flag;
		}

		private async void SaveCompletedGuideList()
		{
			await TaskExpand.Delay(1);
			ulong num = this.mOverGuideMask;
			foreach (KeyValuePair<int, bool> keyValuePair in this.mOverGuide)
			{
				if (keyValuePair.Key < 0 || keyValuePair.Key > 60)
				{
					HLog.LogError(string.Concat(new string[]
					{
						"[新手引导]引导组号超限",
						keyValuePair.Key.ToString(),
						"[",
						0.ToString(),
						",",
						60.ToString(),
						"]"
					}));
				}
				else
				{
					ulong num2 = 1UL;
					num2 <<= keyValuePair.Key;
					num |= num2;
				}
			}
			this.mOverGuideMask = num;
			this.setLocalGuideMask(this.mOverGuideMask);
			this.sendGuideMaskToServer();
		}

		private void sendGuideMaskToServer()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (this.mOverGuideMask != dataModule.GuideMask)
			{
				NetworkUtils.User.DoUserUpdateGuideMaskRequest(this.mOverGuideMask, delegate(bool result, UserUpdateGuideMaskResponse response)
				{
				});
			}
		}

		internal void OverGuideGroup(int group, bool skip)
		{
			this.OverGuideGroups(new List<int> { group }, skip);
		}

		internal void OverGuideGroups(List<int> groups, bool skip)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				if (!this.mOverGuide.ContainsKey(groups[i]))
				{
					this.mOverGuide.Add(groups[i], true);
				}
				if (this.CurGuideGroup != null && this.CurGuideGroup.Group == groups[i])
				{
					if (skip)
					{
						this.CurGuideGroup = null;
					}
					else if (this.CurGuideGroup.CompleteAll)
					{
						this.CurGuideGroup = null;
					}
				}
			}
			for (int j = 0; j < groups.Count; j++)
			{
				EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
				eventArgGuideTrigger.SetData(GuideTriggerKind.GuideGroupOver, groups[j].ToString());
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
			}
			this.SaveCompletedGuideList();
			this.ReCalcTargetDic();
		}

		internal bool IsGuideOver(int group)
		{
			bool flag;
			return this.mOverGuide.TryGetValue(group, out flag);
		}

		public bool IsGuideControllLevel(int pvelevel)
		{
			GuideGroup guideGroup;
			return this.mPVELevelCondition.TryGetValue(pvelevel, out guideGroup) && guideGroup != null && !this.IsGuideOver(guideGroup.Group);
		}

		public void SetGuideSpecialState(int key, int state)
		{
			this.mGuideSpecialState[key] = state;
		}

		public bool HasGuideSpecialState(int key, out int state)
		{
			int num;
			if (this.mGuideSpecialState.TryGetValue(key, out num))
			{
				state = num;
				return true;
			}
			state = 0;
			return false;
		}

		public void HangGuide(int group)
		{
			for (int i = 0; i < this.mHangGuideList.Count; i++)
			{
				if (this.mHangGuideList[i].Group == group)
				{
					HLog.LogError(string.Format("[新手引导]新手引导已经挂起了，不需要重新挂起 引导组: {0}", group));
					return;
				}
			}
			if (this.CurGuideGroup == null)
			{
				HLog.LogError(string.Format("[新手引导]当前没有新手引导，无法挂起引导 引导组: {0}", group));
				return;
			}
			if (this.CurGuideGroup.Group != group)
			{
				HLog.LogError(string.Format("[新手引导]无法挂起引导 引导组不匹配 引导组: {0} != {1}", group, this.CurGuideGroup.Group));
				return;
			}
			GuideData curGuide = this.CurGuideGroup.CurGuide;
			this.CurGuideGroup.HangGuideCurStep();
			this.mHangGuideList.Add(this.CurGuideGroup);
			this.CurGuideGroup = null;
			if (curGuide != null)
			{
				EventArgHangGuideUI eventArgHangGuideUI = new EventArgHangGuideUI();
				eventArgHangGuideUI.SetData(curGuide.Group, curGuide.ID);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_HangGuideUI, eventArgHangGuideUI);
			}
			EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
			eventArgGuideTrigger.SetData(GuideTriggerKind.GuideGroupOver, group.ToString());
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
		}

		public List<GuideGroup> GetHangGuideList()
		{
			return this.mHangGuideList;
		}

		public bool IfGuideHang(int group)
		{
			for (int i = 0; i < this.mHangGuideList.Count; i++)
			{
				if (this.mHangGuideList[i].Group == group)
				{
					return true;
				}
			}
			return false;
		}

		public int GetName()
		{
			return 150;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public List<int> DebugGuideList
		{
			get
			{
				return GuideDataModule.mEmpytDebugGuideList;
			}
		}

		private Dictionary<GuideTriggerKind, List<GuideGroup>> mTriggerDic = new Dictionary<GuideTriggerKind, List<GuideGroup>>();

		private Dictionary<int, GuideGroup> mAllGroup = new Dictionary<int, GuideGroup>();

		private Dictionary<int, bool> mOverGuide = new Dictionary<int, bool>();

		private Dictionary<int, GuideGroup> mPVELevelCondition = new Dictionary<int, GuideGroup>();

		private List<GuideGroup> mHangGuideList = new List<GuideGroup>();

		public const int MinGuideGroup = 0;

		public const int MaxGuideGroup = 60;

		private ulong mOverGuideMask;

		private Dictionary<int, int> mGuideSpecialState = new Dictionary<int, int>();

		private Dictionary<string, bool> mGuideTargetFlag = new Dictionary<string, bool>();

		private static List<int> mEmpytDebugGuideList = new List<int>();

		private GuideDataModule.GuideDebugData mDebugData;

		public class GuideDebugData
		{
			public void Init()
			{
			}

			public void Save()
			{
			}

			public List<int> DebugList = new List<int>();
		}
	}
}
