using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIViewPlayerCamera
	{
		public GameObject GObj { get; private set; }

		public bool InitOver
		{
			get
			{
				return this.MainCamera != null && this.TFPlayerParent != null;
			}
		}

		public string KeyName { get; private set; }

		private T GetChildComponent<T>(GameObject gobj, string pathname) where T : Component
		{
			if (gobj == null)
			{
				return default(T);
			}
			Transform findChild = this.GetFindChild(gobj.transform, pathname);
			if (findChild == null)
			{
				return default(T);
			}
			T component = findChild.GetComponent<T>();
			if (component == null)
			{
				return default(T);
			}
			return component;
		}

		public Transform GetFindChild(Transform tf, string pathname)
		{
			if (tf == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(pathname))
			{
				return tf;
			}
			string[] array = pathname.Split('/', StringSplitOptions.None);
			Transform transform = tf;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform2 = transform.Find(array[i]);
				if (transform2 == null)
				{
					return null;
				}
				transform = transform2;
			}
			return transform;
		}

		public static UIViewPlayerCamera Get(string keyname, GameObject sceneobj)
		{
			if (string.IsNullOrEmpty(keyname))
			{
				return null;
			}
			UIViewPlayerCamera uiviewPlayerCamera;
			if (UIViewPlayerCamera.mDic.TryGetValue(keyname, out uiviewPlayerCamera) && uiviewPlayerCamera != null && uiviewPlayerCamera.InitOver)
			{
				if (uiviewPlayerCamera != null)
				{
					uiviewPlayerCamera.SetShow(true);
				}
				return uiviewPlayerCamera;
			}
			if (uiviewPlayerCamera == null)
			{
				uiviewPlayerCamera = new UIViewPlayerCamera();
				uiviewPlayerCamera.KeyName = keyname;
			}
			if (uiviewPlayerCamera.InitOver)
			{
				if (uiviewPlayerCamera != null)
				{
					uiviewPlayerCamera.SetShow(true);
				}
				return uiviewPlayerCamera;
			}
			sceneobj.SetActiveSafe(true);
			sceneobj.name = keyname;
			if (sceneobj != null)
			{
				uiviewPlayerCamera.SetData(sceneobj);
			}
			if (!UIViewPlayerCamera.mDic.ContainsKey(keyname))
			{
				UIViewPlayerCamera.CameraIndex++;
				uiviewPlayerCamera.mIndex = UIViewPlayerCamera.CameraIndex;
				UIViewPlayerCamera.mDic.Add(keyname, uiviewPlayerCamera);
			}
			if (uiviewPlayerCamera != null)
			{
				uiviewPlayerCamera.SetShow(true);
			}
			return uiviewPlayerCamera;
		}

		public static UIViewPlayerCamera CreateFind(string keyname, GameObject objprefab)
		{
			if (string.IsNullOrEmpty(keyname))
			{
				return null;
			}
			UIViewPlayerCamera uiviewPlayerCamera;
			if (UIViewPlayerCamera.mDic.TryGetValue(keyname, out uiviewPlayerCamera) && uiviewPlayerCamera != null && uiviewPlayerCamera.InitOver)
			{
				if (uiviewPlayerCamera != null)
				{
					uiviewPlayerCamera.SetShow(true);
				}
				return uiviewPlayerCamera;
			}
			if (uiviewPlayerCamera == null)
			{
				uiviewPlayerCamera = new UIViewPlayerCamera();
				uiviewPlayerCamera.KeyName = keyname;
			}
			if (uiviewPlayerCamera.InitOver)
			{
				if (uiviewPlayerCamera != null)
				{
					uiviewPlayerCamera.SetShow(true);
				}
				return uiviewPlayerCamera;
			}
			GameObject gameObject = GameObject.Find(keyname);
			if (gameObject == null)
			{
				gameObject = Object.Instantiate<GameObject>(objprefab);
				gameObject.SetActive(true);
				gameObject.name = keyname;
				UIViewPlayerCamera.mCameraPositionX -= 100;
				gameObject.transform.localPosition = new Vector3((float)UIViewPlayerCamera.mCameraPositionX, (float)UIViewPlayerCamera.mCameraPositionY, 0f);
			}
			if (gameObject != null)
			{
				uiviewPlayerCamera.SetData(gameObject);
			}
			if (!UIViewPlayerCamera.mDic.ContainsKey(keyname))
			{
				UIViewPlayerCamera.mDic.Add(keyname, uiviewPlayerCamera);
			}
			if (uiviewPlayerCamera != null)
			{
				uiviewPlayerCamera.SetShow(true);
			}
			return uiviewPlayerCamera;
		}

		public static void DestroyAll()
		{
			foreach (UIViewPlayerCamera uiviewPlayerCamera in UIViewPlayerCamera.mDic.Values.ToArray<UIViewPlayerCamera>())
			{
				if (uiviewPlayerCamera != null)
				{
					uiviewPlayerCamera.OnThisDestroy();
				}
			}
			UIViewPlayerCamera.mDic.Clear();
			UIViewPlayerCamera.CameraIndex = 0;
		}

		public void SetData(GameObject gobj)
		{
			if (gobj == null)
			{
				HLog.LogError("UIViewPlayerCamera init fail, the object can't be null !!!");
				return;
			}
			this.GObj = gobj;
			this.MainCamera = this.GetChildComponent<Camera>(gobj, "PlayerCamera");
			this.TFMainCamera = ((this.MainCamera != null) ? this.MainCamera.transform : null);
			this.TFPlayerParent = this.GetFindChild(gobj.transform, "Players");
		}

		public void SetCameraTarget(RawImage img, Vector2 size, int depth = 1000)
		{
			if (this.MainCamera != null && img != null)
			{
				if (this.m_renderTexture == null)
				{
					this.m_renderTexture = new RenderTexture((int)size.x, (int)size.y, depth);
				}
				this.m_renderTexture.name = "CameraTextureTarget";
				this.MainCamera.targetTexture = this.m_renderTexture;
				img.texture = this.m_renderTexture;
			}
		}

		public void SetOutlineWidth(float value)
		{
			this.m_isUpdateOutlineWidth = true;
			this.m_outlineWidth = value;
		}

		public void SetPlayerStartPosition(Vector3 pos)
		{
			this.m_startPosition = pos;
		}

		public void SetCamearLocalPosition(Vector3 pos)
		{
			if (this.TFMainCamera != null)
			{
				this.TFMainCamera.localPosition = pos;
			}
		}

		public void SetCamearEulerAngles(Vector3 eulerAngles)
		{
			if (this.TFMainCamera != null)
			{
				this.TFMainCamera.eulerAngles = eulerAngles;
			}
		}

		public void SetCameraY(float y)
		{
			if (this.TFMainCamera != null)
			{
				this.TFMainCamera.localPosition = new Vector3(this.TFMainCamera.localPosition.x, y, this.TFMainCamera.localPosition.z);
			}
		}

		public void SetCameraFov(float fov)
		{
			if (this.MainCamera != null)
			{
				this.MainCamera.fieldOfView = fov;
			}
		}

		public void Clear(bool destroy = false)
		{
			if (this.TFPlayerParent == null)
			{
				return;
			}
			int childCount = this.TFPlayerParent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = this.TFPlayerParent.GetChild(i);
				if (!(child == null))
				{
					if (destroy)
					{
						GameApp.Resources.ReleaseInstance(child.gameObject);
						Object.Destroy(child.gameObject);
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
			}
			if (destroy)
			{
				this.mPACList.Clear();
			}
			if (destroy)
			{
				this.MainCamera.targetTexture = null;
				if (this.m_renderTexture != null)
				{
					Object.Destroy(this.m_renderTexture);
					this.m_renderTexture = null;
				}
			}
		}

		public void SetShow(bool show)
		{
			if (this.GObj != null)
			{
				this.GObj.SetActiveSafe(show);
				float num = (float)(UIViewPlayerCamera.mCameraPositionX + this.mIndex * -100);
				RectTransform component = this.GObj.GetComponent<RectTransform>();
				if (component != null)
				{
					component.anchoredPosition = new Vector2(num, (float)UIViewPlayerCamera.mCameraPositionY);
					return;
				}
				this.GObj.transform.localPosition = new Vector3(num, (float)UIViewPlayerCamera.mCameraPositionY, 0f);
			}
		}

		public void Update(float time)
		{
			for (int i = 0; i < this.mPACList.Count; i++)
			{
				this.mPACList[i].Update(time);
			}
		}

		private UIViewPlayerCamera.PlayerAnimationController GetCreatePAC(GameObject obj)
		{
			if (obj == null)
			{
				HLog.LogError("[UIViewPlayerCamera]非法 创建 PAC NULL");
				return null;
			}
			for (int i = 0; i < this.mPACList.Count; i++)
			{
				if (this.mPACList[i].GObj == obj)
				{
					return this.mPACList[i];
				}
			}
			UIViewPlayerCamera.PlayerAnimationController playerAnimationController = new UIViewPlayerCamera.PlayerAnimationController();
			playerAnimationController.SetGameObject(obj);
			this.mPACList.Add(playerAnimationController);
			return playerAnimationController;
		}

		public void InitPlayAnimationList(GameObject obj, int cardid)
		{
			this.GetCreatePAC(obj).Reset(cardid);
		}

		public async Task FindCreatePlayer(TaskOutValue<GameObject> outGameObject, int mamberid, Func<GameObject, Task> actioncreated = null)
		{
			int modelID = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(mamberid)
				.modelID;
			string text = modelID.ToString();
			Transform findChild = this.GetFindChild(this.TFPlayerParent, text);
			if (findChild != null)
			{
				GameObject gameObject = findChild.gameObject;
				gameObject.SetActive(true);
				if (actioncreated != null)
				{
					actioncreated(gameObject);
				}
				outGameObject.SetValue(gameObject);
			}
			else
			{
				await this.AddPlayer(outGameObject, modelID, actioncreated);
			}
		}

		public Task AddPlayer(TaskOutValue<GameObject> outGameObject, int modelID, Func<GameObject, Task> actioncreated = null)
		{
			UIViewPlayerCamera.<AddPlayer>d__43 <AddPlayer>d__;
			<AddPlayer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AddPlayer>d__.<>4__this = this;
			<AddPlayer>d__.outGameObject = outGameObject;
			<AddPlayer>d__.modelID = modelID;
			<AddPlayer>d__.actioncreated = actioncreated;
			<AddPlayer>d__.<>1__state = -1;
			<AddPlayer>d__.<>t__builder.Start<UIViewPlayerCamera.<AddPlayer>d__43>(ref <AddPlayer>d__);
			return <AddPlayer>d__.<>t__builder.Task;
		}

		public void OnThisDestroy()
		{
			this.MainCamera = null;
			this.TFMainCamera = null;
			this.TFPlayerParent = null;
			if (UIViewPlayerCamera.mDic.ContainsKey(this.KeyName))
			{
				UIViewPlayerCamera.mDic.Remove(this.KeyName);
			}
		}

		public Camera MainCamera;

		public Transform TFMainCamera;

		public Transform TFPlayerParent;

		private Vector3 m_startPosition = Vector3.zero;

		private int mIndex;

		private static Dictionary<string, UIViewPlayerCamera> mDic = new Dictionary<string, UIViewPlayerCamera>();

		private static int CameraIndex = 0;

		private List<UIViewPlayerCamera.PlayerAnimationController> mPACList = new List<UIViewPlayerCamera.PlayerAnimationController>();

		private static int mCameraPositionX = -15000;

		private static int mCameraPositionY = -15000;

		private bool m_isUpdateOutlineWidth;

		private float m_outlineWidth = 0.03f;

		private RenderTexture m_renderTexture;

		public class PlayerAnimationController
		{
			public void Play()
			{
				if (this.GObj == null)
				{
					return;
				}
				if (this.mCurPlayIndex < 0 || this.mCurPlayList == null || this.mCurPlayIndex >= this.mCurPlayList.Length)
				{
					return;
				}
				string text = this.mCurPlayList[this.mCurPlayIndex];
				this.mCurPlayIndex++;
				this.MainAnimator.SetTrigger(text);
				if (this.WeaponLAnimator != null)
				{
					this.WeaponLAnimator.SetTrigger(text);
				}
				if (this.WeaponRAnimator != null)
				{
					this.WeaponRAnimator.SetTrigger(text);
				}
				if (this.mCurPlayIndex == 0)
				{
					this.NextPlayAnimationTime = this.PlayAnimationSpan;
				}
			}

			public void SetGameObject(GameObject gobj)
			{
				this.GObj = gobj;
				GameObject gameObject = gobj.GetComponent<ComponentRegister>().GetGameObject("Model");
				this.MainAnimator = gameObject.GetComponent<Animator>();
				this.AListener = AnimatorListen.Get(gameObject);
				this.AListener.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorEvent));
				MemberBody component = gobj.GetComponent<MemberBody>();
				if (component != null && component.m_weaponLeftBody != null)
				{
					this.WeaponLAnimator = component.m_weaponLeftBody.m_animator;
				}
				else
				{
					this.WeaponLAnimator = null;
				}
				if (component != null && component.m_weaponRightBody != null)
				{
					this.WeaponRAnimator = component.m_weaponRightBody.m_animator;
					return;
				}
				this.WeaponRAnimator = null;
			}

			private void OnAnimatorEvent(GameObject obj, string ename)
			{
				if (!(this.GObj == null))
				{
					this.MainAnimator == null;
				}
			}

			public void Update(float time)
			{
				if (this.GObj == null || !this.GObj.activeSelf)
				{
					return;
				}
				this.NextPlayAnimationTime -= time;
				if (this.NextPlayAnimationTime <= 0f)
				{
					this.NextPlayAnimationTime = this.PlayAnimationSpan;
					this.RandomPlay();
				}
			}

			public void RandomPlay()
			{
				if (this.mPlayRandomPool.Count <= 0)
				{
					return;
				}
				Random random = new Random((int)DateTime.Now.Ticks);
				int num = random.Next(this.mPlayRandomPool.Count);
				int num2 = 4;
				while (this.mLastPlayID == num)
				{
					num = random.Next(this.mPlayRandomPool.Count);
					num2--;
					if (num2 <= 0)
					{
						break;
					}
				}
				this.mLastPlayID = num;
				this.mCurPlayList = this.mPlayRandomPool[this.mLastPlayID];
				this.mCurPlayIndex = 0;
				this.Play();
			}

			internal void Reset(int cardid)
			{
			}

			public GameObject GObj;

			public AnimatorListen AListener;

			public Animator MainAnimator;

			public Animator WeaponLAnimator;

			public Animator WeaponRAnimator;

			public float PlayAnimationSpan = 4f;

			public float NextPlayAnimationTime = 0.75f;

			private int mLastPlayID = -1;

			private int mCardID;

			private List<string[]> mPlayRandomPool = new List<string[]>();

			private string[] mCurPlayList;

			private int mCurPlayIndex = -1;
		}
	}
}
