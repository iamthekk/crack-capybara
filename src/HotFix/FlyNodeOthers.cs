using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Platfrom;
using UnityEngine;

namespace HotFix
{
	public class FlyNodeOthers : BaseFlyNode
	{
		protected override void OnInit()
		{
			this.m_seqPool = new SequencePool();
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			this.m_onItemFinished = null;
		}

		public override void Fly()
		{
			if (this.m_pool == null)
			{
				return;
			}
			this.m_finishedCount = 0;
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				this.OnFly(i);
			}
		}

		private void OnFly(int index)
		{
			FlyItemViewModule.Data flyData = this.m_flyDatas[index];
			FlyNodeOthers.Data data = this.m_datas[index];
			int count = ((data.m_count <= 20L) ? ((int)data.m_count) : 20);
			int num = 0;
			float num2 = 0.3f;
			float num3 = 0.3f;
			float num4 = 0.03f;
			for (int i = 0; i < flyData.m_endNodes.Count; i++)
			{
				BaseFlyEndNode baseFlyEndNode = flyData.m_endNodes[i];
				if (!(baseFlyEndNode == null))
				{
					baseFlyEndNode.SetCount(data.m_from, data.m_to, data.m_count);
					baseFlyEndNode.Init();
				}
			}
			int finisheCount = 0;
			TweenCallback <>9__0;
			for (int j = 0; j < count; j++)
			{
				int num5 = j;
				GameObject ctrl = this.m_pool.DeQueue(flyData.m_item.gameObject.name);
				BaseFlyItem component = ctrl.GetComponent<BaseFlyItem>();
				if (component)
				{
					component.SetData(data.m_param);
				}
				Transform transform = ctrl.transform;
				transform.SetParent(this.m_nodeParent);
				transform.localScale = Vector3.one;
				Vector3 position = flyData.m_startTrans.position;
				int endNodeIndex = num;
				BaseFlyEndNode endNode = flyData.m_endNodes[num];
				Transform transform2 = endNode.transform;
				Vector3 vector = position + new Vector3(Utility.Math.Random(-0.2f, 0.2f), Utility.Math.Random(-0.2f, 0.2f), 0f);
				transform.position = position;
				transform.localScale = Vector3.one * 0f;
				num++;
				if (num >= flyData.m_endNodes.Count)
				{
					num = 0;
				}
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, vector, num2, false), 1));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(transform, Vector3.one * 1.1f, num2));
				TweenSettingsExtensions.AppendInterval(sequence, num4 * (float)num5);
				Sequence sequence2 = sequence;
				TweenCallback tweenCallback;
				if ((tweenCallback = <>9__0) == null)
				{
					tweenCallback = (<>9__0 = delegate
					{
						if (!string.IsNullOrEmpty(flyData.m_audioClipPath))
						{
							GameApp.Sound.PlaySoundEffect(flyData.m_audioClipPath, 1f);
							return;
						}
						if (flyData.m_audioId > 0)
						{
							GameApp.Sound.PlayClip(flyData.m_audioId, 1f);
						}
					});
				}
				TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback);
				if (endNode.m_transform != null)
				{
					Tweener tweener = null;
					tweener = TweenSettingsExtensions.OnUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, transform2.position, num3, false), 11), delegate
					{
						tweener.ChangeEndValue(endNode.m_transform.position, true);
					});
					TweenSettingsExtensions.Append(sequence, tweener);
				}
				else
				{
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, transform2.position, num3, false), 11));
				}
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(transform, Vector3.one, num3));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.m_pool.EnQueue(flyData.m_item.gameObject.name, ctrl);
					int finisheCount2 = finisheCount;
					finisheCount = finisheCount2 + 1;
					bool flag = false;
					int num6 = finisheCount;
					if (finisheCount >= count)
					{
						num6 = count;
						flag = true;
					}
					if (endNode != null)
					{
						endNode.OnItemFinished(num6, count);
					}
					if (this.m_onItemFinished != null)
					{
						this.m_onItemFinished(this, index, endNodeIndex, (float)num6 * 1f / (float)count);
					}
					if (flag)
					{
						this.m_finishedCount++;
						if (this.m_finishedCount >= this.m_datas.Count)
						{
							this.OnFinished();
						}
					}
				});
			}
		}

		private async void OnFinished()
		{
			await TaskExpand.Delay(200);
			for (int i = 0; i < this.m_flyDatas.Count; i++)
			{
				FlyItemViewModule.Data data = this.m_flyDatas[i];
				if (data != null)
				{
					for (int j = 0; j < data.m_endNodes.Count; j++)
					{
						BaseFlyEndNode baseFlyEndNode = data.m_endNodes[j];
						if (!(baseFlyEndNode == null))
						{
							baseFlyEndNode.OnFinished();
							baseFlyEndNode.DeInit();
						}
					}
				}
			}
			if (this.m_onFinished != null)
			{
				this.m_onFinished(this);
			}
		}

		public FlyItemModel m_model;

		public List<FlyItemData> m_itemDatas = new List<FlyItemData>();

		public List<FlyItemViewModule.Data> m_flyDatas = new List<FlyItemViewModule.Data>();

		public List<FlyNodeOthers.Data> m_datas = new List<FlyNodeOthers.Data>();

		public LocalUnityObjctPool m_pool;

		public Transform m_nodeParent;

		private SequencePool m_seqPool;

		public OnFlyNodeFlyNodeOthersItemFinished m_onItemFinished;

		private int m_finishedCount;

		public class Data
		{
			public long m_from;

			public long m_to;

			public long m_count;

			public object m_param;
		}
	}
}
