using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Platfrom;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class GuideController : Singleton<GuideController>
	{
		public GuideActionBase GetGuideAction(string name)
		{
			GuideActionBase guideActionBase;
			if (this.mAllAction.TryGetValue(name, out guideActionBase))
			{
				return guideActionBase;
			}
			guideActionBase = null;
			if (!(name == "GameIn_JoyGuide") && !(name == "GameIn_SkillGuide"))
			{
				name == "MainUI_Shop_ScrollToDiamon";
			}
			if (guideActionBase == null)
			{
				HLog.LogError("[新手引导]GuideAction is null : [HotFix.Guide.Actions." + name + "]");
				return null;
			}
			this.mAllAction.Add(name, guideActionBase);
			return guideActionBase;
		}

		public new static GuideController Instance
		{
			get
			{
				if (GuideController.mInstance == null)
				{
					HLog.LogError("[GuideController] mInstance == null");
				}
				return GuideController.mInstance;
			}
		}

		public static bool IsNull
		{
			get
			{
				return GuideController.mInstance == null;
			}
		}

		public static bool ShowGuideLog
		{
			get
			{
				return false;
			}
		}

		public GuideData CurrentGuide
		{
			get
			{
				if (this.mGuideDataModule == null)
				{
					return null;
				}
				GuideGroup curGuideGroup = this.mGuideDataModule.CurGuideGroup;
				if (curGuideGroup == null)
				{
					return null;
				}
				return curGuideGroup.CurGuide;
			}
		}

		public static void OnCreate()
		{
			if (GuideController.mInstance != null)
			{
				GuideController.mInstance.UnOnInit();
				GuideController.mInstance = null;
			}
			GuideController.mInstance = new GuideController();
			GuideController.mInstance.OnInit();
		}

		public void OnDelect()
		{
			if (GuideController.mInstance != null)
			{
				GuideController.mInstance.UnOnInit();
				GuideController.mInstance = null;
			}
		}

		public void OnInit()
		{
			this.mGuideDataModule = GameApp.Data.GetDataModule(DataName.GuideDataModule);
			this.RegisterEvents();
			this.mGuideDataModule.Init();
		}

		public void UnOnInit()
		{
			this.UnRegisterEvents();
		}

		internal void InitData(ulong mask)
		{
			this.mGuideDataModule.InitCompleteGuide(mask, true);
		}

		public void RegisterEvents()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guide_Trigger, new HandlerEvent(this.OnGuideTrigger));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guide_GuideOver, new HandlerEvent(this.OnGuideOver));
		}

		public void UnRegisterEvents()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guide_Trigger, new HandlerEvent(this.OnGuideTrigger));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guide_GuideOver, new HandlerEvent(this.OnGuideOver));
		}

		public void OpenViewTrigger(ViewName viewName)
		{
			this.DelayOpenViewTrigger(viewName);
		}

		private async void DelayOpenViewTrigger(ViewName viewName)
		{
			if (viewName != ViewName.NetLoadingViewModule && viewName != ViewName.GuideViewModule)
			{
				if (viewName != ViewName.CurrencyViewModule && viewName != ViewName.MainViewModule)
				{
					this.mHasOpenView = true;
				}
				this.GuideOverByKind(GuideCompleteKind.ViewOpen, viewName.ToString(), false);
				await TaskExpand.Delay(10);
				EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
				eventArgGuideTrigger.SetData(GuideTriggerKind.ViewOpen, viewName.ToString());
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
			}
		}

		public void CloseViewTrigger(ViewName viewName)
		{
			this.DelayCloseViewTrigger(viewName);
		}

		private async void DelayCloseViewTrigger(ViewName viewName)
		{
			if (viewName != ViewName.NetLoadingViewModule && viewName != ViewName.GuideViewModule)
			{
				this.GuideOverByKind(GuideCompleteKind.ViewClose, viewName.ToString(), false);
				await TaskExpand.Delay(10);
				EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
				EventArgGuideTrigger eventArgGuideTrigger2 = eventArgGuideTrigger;
				GuideTriggerKind guideTriggerKind = GuideTriggerKind.ViewClose;
				int num = (int)viewName;
				eventArgGuideTrigger2.SetData(guideTriggerKind, num.ToString());
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
			}
		}

		public void MainUISwitchPage(int index)
		{
			this.GuideOverByKind(GuideCompleteKind.MainDownButton, index.ToString(), false);
		}

		public void CustomizeStringTrigger(string info)
		{
			EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
			eventArgGuideTrigger.SetData(GuideTriggerKind.CustomizeString, info);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
		}

		public void CustomizeStringOver(string info, bool includehang = true)
		{
			this.GuideOverByKind(GuideCompleteKind.SpecialString, info, includehang);
		}

		public void GuideOverByKind(GuideCompleteKind kind, string info, bool includehang = false)
		{
			if (this.mGuideDataModule.CurGuideGroup == null)
			{
				if (includehang)
				{
					this.HangGuideOverByKind(kind, info);
				}
				return;
			}
			GuideGroup curGuideGroup = this.mGuideDataModule.CurGuideGroup;
			if (curGuideGroup.CurGuide == null)
			{
				if (includehang)
				{
					this.HangGuideOverByKind(kind, info);
				}
				return;
			}
			if (curGuideGroup.CurGuide.CompleteList != null)
			{
				List<GuideCompleteData> completeList = curGuideGroup.CurGuide.CompleteList;
				for (int i = 0; i < completeList.Count; i++)
				{
					if (completeList[i].GuideKind == kind && info == completeList[i].Argstr)
					{
						bool showGuideLog = GuideController.ShowGuideLog;
						EventArgGuideCompleteStep eventArgGuideCompleteStep = new EventArgGuideCompleteStep();
						eventArgGuideCompleteStep.SetData(curGuideGroup.CurGuide.Group, curGuideGroup.CurGuide.ID);
						GameApp.Event.DispatchNow(this, LocalMessageName.Guide_Special_CompleteStep, eventArgGuideCompleteStep);
						if (includehang)
						{
							this.HangGuideOverByKind(kind, info);
						}
						return;
					}
				}
			}
			if (includehang)
			{
				this.HangGuideOverByKind(kind, info);
			}
		}

		public void HangGuideOverByKind(GuideCompleteKind kind, string info)
		{
			List<GuideGroup> hangGuideList = this.mGuideDataModule.GetHangGuideList();
			List<int> list = new List<int>();
			for (int i = 0; i < hangGuideList.Count; i++)
			{
				GuideGroup guideGroup = hangGuideList[i];
				if (guideGroup.CurGuide == null)
				{
					return;
				}
				if (guideGroup.CurGuide.CompleteList != null)
				{
					List<GuideCompleteData> completeList = guideGroup.CurGuide.CompleteList;
					int j = 0;
					while (j < completeList.Count)
					{
						if (completeList[j].GuideKind == kind && info == completeList[j].Argstr)
						{
							if (guideGroup.MovetoNextGuide())
							{
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
					if (guideGroup.CompleteAll)
					{
						list.Add(guideGroup.Group);
						hangGuideList.RemoveAt(i);
						i--;
					}
				}
			}
			if (list.Count > 0)
			{
				this.mGuideDataModule.OverGuideGroups(list, true);
			}
		}

		public void HangCurGuide(GuideData guidedata)
		{
			if (guidedata == null)
			{
				return;
			}
			this.mGuideDataModule.HangGuide(guidedata.Group);
		}

		private void OnGuideTrigger(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.mIsNoGuide)
			{
				bool showGuideLog = GuideController.ShowGuideLog;
				return;
			}
			EventArgGuideTrigger eventArgGuideTrigger = null;
			EventArgGuideTrigger eventArgGuideTrigger2 = eventargs as EventArgGuideTrigger;
			if (eventArgGuideTrigger2 != null)
			{
				eventArgGuideTrigger = eventArgGuideTrigger2;
			}
			if (eventArgGuideTrigger == null)
			{
				return;
			}
			GuideTriggerKind triggerKind = eventArgGuideTrigger.triggerKind;
			string triggerParam = eventArgGuideTrigger.triggerParam;
			List<GuideGroup> hangGuideList = this.mGuideDataModule.GetHangGuideList();
			bool showGuideLog2 = GuideController.ShowGuideLog;
			GuideGroup guideGroup = this.mGuideDataModule.CurGuideGroup;
			if (guideGroup != null)
			{
				if (this.IfTrigger(guideGroup, triggerKind, triggerParam))
				{
					this.DoGuideAction(guideGroup, "继续当前进行的引导");
					return;
				}
				if (!guideGroup.IfCanBreak && triggerKind == GuideTriggerKind.OnlyMainUI && !this.mGuideDataModule.IfGuideHang(guideGroup.Group))
				{
					bool showGuideLog3 = GuideController.ShowGuideLog;
					return;
				}
				if (!guideGroup.IfCanBreak && guideGroup.IsGuideActive && !this.mGuideDataModule.IfGuideHang(guideGroup.Group))
				{
					bool showGuideLog4 = GuideController.ShowGuideLog;
					return;
				}
			}
			if (hangGuideList != null)
			{
				for (int i = 0; i < hangGuideList.Count; i++)
				{
					guideGroup = hangGuideList[i];
					if (guideGroup != null && this.IfTrigger(guideGroup, triggerKind, triggerParam) && this.mGuideDataModule.ContinueHangGuide(guideGroup))
					{
						GuideData curGuide = guideGroup.CurGuide;
						this.DoGuideAction(guideGroup, "挂起的引导继续");
						return;
					}
				}
			}
			List<GuideGroup> listByTriggerKind = this.mGuideDataModule.GetListByTriggerKind(triggerKind);
			if (listByTriggerKind == null || listByTriggerKind.Count <= 0)
			{
				return;
			}
			for (int j = 0; j < listByTriggerKind.Count; j++)
			{
				GuideGroup guideGroup2 = listByTriggerKind[j];
				if (this.IfTrigger(guideGroup2, triggerKind, triggerParam))
				{
					guideGroup2.ResetToFirst();
					this.mGuideDataModule.SetCurGuideGroup(guideGroup2);
					this.DoGuideAction(guideGroup2, "新触发引导");
					return;
				}
			}
		}

		private async void DoGuideAction(GuideGroup curgroup, string debuginfo)
		{
			if (curgroup != null)
			{
				curgroup.ActiveGuide();
				await TaskExpand.Yield();
				GuideData curGuide = curgroup.CurGuide;
				if (curGuide == null)
				{
					HLog.LogError(string.Format("[新手引导]当前为空引导:{0}  {1}", curgroup.Group, debuginfo));
				}
				else
				{
					bool showGuideLog = GuideController.ShowGuideLog;
					int curGuideIndex = curgroup.CurGuideIndex;
					if (curGuide.GuideAction == null || curGuide.GuideAction.Length == 0)
					{
						this.CheckOpenGuideView();
					}
					else
					{
						string text = curGuide.GuideAction[0];
						bool showGuideLog2 = GuideController.ShowGuideLog;
						this.GetGuideAction(text).DoAction(curGuide);
					}
				}
			}
		}

		public void CheckOpenGuideView()
		{
			if (GameApp.View.IsOpened(ViewName.GuideViewModule))
			{
				bool showGuideLog = GuideController.ShowGuideLog;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_RefreshUI, null);
				return;
			}
			bool showGuideLog2 = GuideController.ShowGuideLog;
			GameApp.View.OpenView(ViewName.GuideViewModule, null, 2, null, null);
		}

		private void OnGuideOver(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgGuideOver eventArgGuideOver = null;
			EventArgGuideOver eventArgGuideOver2 = eventargs as EventArgGuideOver;
			if (eventArgGuideOver2 != null)
			{
				eventArgGuideOver = eventArgGuideOver2;
			}
			if (eventArgGuideOver == null)
			{
				return;
			}
			int guideGroup = eventArgGuideOver.guideGroup;
			int guideId = eventArgGuideOver.guideId;
			bool isSkipGroup = eventArgGuideOver.isSkipGroup;
			if (guideGroup == 0 || guideId == 0)
			{
				if (GuideController.ShowGuideLog)
				{
					HLog.LogError("[新手引导]完成引导错误 没有指定引导数据");
				}
				return;
			}
			GuideGroup curGuideGroup = this.mGuideDataModule.CurGuideGroup;
			if (curGuideGroup == null)
			{
				bool showGuideLog = GuideController.ShowGuideLog;
				return;
			}
			if (curGuideGroup.Group != guideGroup)
			{
				if (GuideController.ShowGuideLog)
				{
					HLog.LogError(string.Concat(new string[]
					{
						"[新手引导]完成引导错误 要完成的引导不同组 当前引导",
						curGuideGroup.Group.ToString(),
						" : ",
						curGuideGroup.CurGuideIndex.ToString(),
						" 完成引导",
						guideGroup.ToString(),
						" : ",
						guideId.ToString()
					}));
				}
				return;
			}
			GuideData curGuide = curGuideGroup.CurGuide;
			if (curGuide != null && curGuide.GuideAction != null && curGuide.GuideAction.Length != 0)
			{
				string text = curGuide.GuideAction[0];
				bool showGuideLog2 = GuideController.ShowGuideLog;
				this.GetGuideAction(text).DoActionAfterGuide(curGuide);
			}
			if (curGuideGroup.IfCurAsGroupOver || isSkipGroup)
			{
				this.mGuideDataModule.OverGuideGroup(curGuideGroup.Group, isSkipGroup);
			}
			EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
			if (isSkipGroup)
			{
				this.mGuideDataModule.SetCurGuideGroup(null);
				curGuideGroup.SkipAll();
				GuideData endGuide = curGuideGroup.EndGuide;
				if (endGuide != null)
				{
					eventArgGuideTrigger.SetData(GuideTriggerKind.GuideOver, endGuide.ID.ToString());
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
				}
			}
			else
			{
				curGuideGroup.MovetoNextGuide();
				eventArgGuideTrigger.SetData(GuideTriggerKind.GuideOver, guideId.ToString());
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
			}
			if (curGuide != null)
			{
				GameApp.SDK.Analyze.Track_Guide(curGuide.ID, 1);
				GameApp.SDK.Analyze.Track_Guide(curGuide.ID, 2);
			}
		}

		internal bool IfTrigger(GuideGroup guidegroup, GuideTriggerKind kind, string args)
		{
			if (guidegroup == null)
			{
				return false;
			}
			if (this.mGuideDataModule.IsGuideOver(guidegroup.Group) && guidegroup.CompleteAll)
			{
				return false;
			}
			GuideData guideData;
			if (!guidegroup.IsGuideActive)
			{
				guideData = guidegroup.CurGuide;
			}
			else
			{
				guideData = guidegroup.NextGuide;
			}
			if (guideData == null)
			{
				return false;
			}
			List<GuideTriggerKindData> triggerKind = guideData.TriggerKind;
			if (triggerKind == null)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < triggerKind.Count; i++)
			{
				if (!string.IsNullOrEmpty(triggerKind[i].Args) && triggerKind[i].Kind == kind && triggerKind[i].Args == args)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			for (int j = 0; j < guideData.ConditionList.Count; j++)
			{
				if (!this.CheckCondition(guideData.ConditionList[j]))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		internal bool CheckCondition(GuideCondition gc)
		{
			if (gc == null)
			{
				return true;
			}
			GuideConditionKind kind = gc.Kind;
			int num3;
			int num4;
			int num5;
			if (kind <= GuideConditionKind.OnlyMainPanel)
			{
				switch (kind)
				{
				case GuideConditionKind.GuideOver:
				{
					int num;
					return gc.GetInt(1, out num) && this.mGuideDataModule.IsGuideOver(num);
				}
				case GuideConditionKind.Level:
				case GuideConditionKind.Diamond:
				case GuideConditionKind.Item:
					break;
				default:
					switch (kind)
					{
					case GuideConditionKind.FunctionOpen:
					{
						int num2;
						return gc.GetInt(1, out num2) && GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(num2);
					}
					case GuideConditionKind.HaveNoBattle:
						return !Singleton<EventRecordController>.Instance.IsHaveAnyRecord();
					case GuideConditionKind.OnlyMainPanel:
					{
						List<ViewModuleData> openList = GameApp.View.GetOpenList();
						for (int i = 0; i < openList.Count; i++)
						{
							ViewName id = (ViewName)openList[i].m_id;
							if (id <= ViewName.TipViewModule)
							{
								if (id != ViewName.NetLoadingViewModule && id != ViewName.TipViewModule)
								{
									return false;
								}
							}
							else if (id != ViewName.FlyItemViewModule && id - ViewName.MainViewModule > 1 && id != ViewName.GuideViewModule)
							{
								return false;
							}
						}
						return true;
					}
					}
					break;
				}
			}
			else if (kind != GuideConditionKind.SpecialState)
			{
				if (kind == GuideConditionKind.NotOpen)
				{
					return false;
				}
			}
			else if (gc.GetInt(1, out num3) && gc.GetInt(2, out num4) && this.mGuideDataModule.HasGuideSpecialState(num3, out num5))
			{
				return num4 == num5;
			}
			return true;
		}

		public bool IfGuideOver(int group)
		{
			return this.mGuideDataModule.IsGuideOver(group);
		}

		public bool IsGuideControllLevel(int pvelevel)
		{
			return this.mGuideDataModule.IsGuideControllLevel(pvelevel);
		}

		public bool IsGuideState(int key, int state)
		{
			int num;
			return this.mGuideDataModule.HasGuideSpecialState(key, out num) && state == num;
		}

		public bool IsAnyGuideActive()
		{
			return this.mGuideDataModule != null && this.mGuideDataModule.CurGuideGroup != null && (this.mGuideDataModule.CurGuideGroup.IsGuideActive && this.mGuideDataModule.CurGuideGroup.CurGuide != null);
		}

		public bool IsGuideGroupHang(int groupID)
		{
			return this.mGuideDataModule.IfGuideHang(groupID);
		}

		public bool IfHasTarget(string key)
		{
			return this.GetTarget(key) != null;
		}

		public bool IfNeedTarget(string key)
		{
			return this.mGuideDataModule != null && this.mGuideDataModule.IfNeedGuideTarget(key);
		}

		public void AddTarget(string key, Transform rtf)
		{
			if (string.IsNullOrEmpty(key) || rtf == null)
			{
				return;
			}
			if (!this.mGuideDataModule.IfNeedGuideTarget(key))
			{
				bool showGuideLog = GuideController.ShowGuideLog;
				return;
			}
			this.mTargetTFDic[key] = rtf;
			GuideTargetFlag.Get(rtf.gameObject, key);
			bool showGuideLog2 = GuideController.ShowGuideLog;
		}

		public void DelTarget(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			if (this.mTargetTFDic.ContainsKey(key))
			{
				this.mTargetTFDic.Remove(key);
			}
			bool showGuideLog = GuideController.ShowGuideLog;
		}

		public Transform GetTarget(string key)
		{
			Transform transform;
			if (this.mTargetTFDic.TryGetValue(key, out transform))
			{
				return transform;
			}
			return null;
		}

		private static GuideController mInstance;

		private GuideDataModule mGuideDataModule;

		private bool mIsNoGuide;

		private Dictionary<string, GuideActionBase> mAllAction = new Dictionary<string, GuideActionBase>();

		private bool mHasOpenView = true;

		private Dictionary<string, Transform> mTargetTFDic = new Dictionary<string, Transform>();

		public enum EGroup
		{
			DailyGold = 7
		}
	}
}
