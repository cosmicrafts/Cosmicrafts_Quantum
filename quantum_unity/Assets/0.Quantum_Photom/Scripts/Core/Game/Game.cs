namespace TowerRush
{
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using System.Collections;
	using System.Collections.Generic;
	using TowerRush.Core;
	using Quantum.Services;

	using Scene = TowerRush.Core.Scene;

	public class Game : MonoBehaviour
	{
		// CONSTANTS
		
		private const   string INTRO_SCENE_NAME   = "IntroScene";
		private const   string MAIN_SCENE_NAME    = "Menu_Cosmic";
		private const   string MenuCosmic_SCENE_NAME    = "Menu_Cosmic";
		private const   string AutoStart_SCENE_NAME   = "Auto_MatchScene";
		private const   string LOADING_SCENE_NAME = "LoadingScene";
		
		// PUBLIC MEMBERS
		
		public static   Game             Instance         { get; private set; }
		public static   GameplayInfo     GameplayInfo     { get; set; }
		public static   Scene            CurrentScene     { get { return Instance.m_CurrentScene; } }
		public static   Services         QuantumServices  { get { return Instance.m_QuantumServices; } }

		public static   bool             IsPaused         { get { return Time.deltaTime < 0.00001f; } }
		public static   bool             IsQuitting       { get; private set; }
		public static   bool             IsLoading        { get; private set; }
		public static   bool             IsError          { get; private set; }
		public static   bool             IsRestarting     { get; private set; }

		public          AudioService     AudioService     { get; private set; }
		public          MainCamera       MainCamera       { get; private set; }

		// PRIVATE MEMBERS

		private         Services         m_QuantumServices;
		private         Coroutines       m_Coroutines;
		private         InputService     m_Input;
		private         Scene            m_CurrentScene;
		private         LoadingScene     m_LoadingScene;
		private         List<GameObject> m_GameObjectCache = new List<GameObject>(4);
		private         SceneContext     m_SceneContext    = new SceneContext();

		#if ENABLE_CHEAT_MANAGER
		[SerializeField, HideInInspector]
		private         string           m_CheatManagerJson;
		#endif

		// PUBLIC METHODS

		public void Restart()
		{
			IsError      = false;
			IsRestarting = true;

			m_CurrentScene.FinishScene();
		}

		public void BreakGame()
		{
			IsError = true;
		}

		public static void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		// MONOBEHAVIOUR INTERFACE

		private void Awake()
		{
			if(Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			m_SceneContext.Game = this;

#if UNITY_EDITOR
		Application.targetFrameRate = 60;
#else
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount  = 1;
#endif
		}
		
		private IEnumerator Start()
		{
			#if ENABLE_CHEAT_MANAGER
			CheatManager.Load(m_CheatManagerJson);
			#endif

			Random.InitState(System.DateTime.Now.Millisecond);
			Application.targetFrameRate = 60;
			Time.timeScale              = 1f;
			Screen.sleepTimeout         = SleepTimeout.NeverSleep;

			GameOptions.Load();

			Initialize();

			MainCamera   = GetComponentInChildren<MainCamera>(true);
			AudioService = GetComponentInChildren<AudioService>(true);

			AudioService.Initialize();
			AudioService.SetReferenceTransform(MainCamera.transform);

			AssignCurrentScene();

			if (m_CurrentScene == null)
				yield break;

			yield return m_CurrentScene.Initialize();
			m_CurrentScene.Activate();
		}

		private void OnDestroy()
		{
			if (Instance != this)
				return;
		
			Instance = null;
			Deinitialize();
		}
		
		private void OnApplicationQuit()
		{
			IsQuitting = true;

			AudioService.Deinitialize();
			m_Coroutines.Deinitialize();
		}

		private void Update()
		{
			if (QuantumRunner.Default != null)
			{
				m_SceneContext.QuantumGame = QuantumRunner.Default.Game;

				if (m_SceneContext.QuantumGame != null)
				{
					m_SceneContext.Frame = m_SceneContext.QuantumGame.Frames.Predicted;
				}
				else
				{
					m_SceneContext.Frame = null;
				}
			}
			else
			{
				m_SceneContext.QuantumGame = null;
				m_SceneContext.Frame       = null;
			}

			m_QuantumServices.Tick();
			m_Input.Update_Internal(m_SceneContext);

			if (m_CurrentScene != null)
			{
				m_CurrentScene.OnUpdate_Internal(m_SceneContext);

				if (m_CurrentScene.Finished == true)
				{
					LoadNextScene();
				}
			}

			if (m_LoadingScene != null)
			{
				m_LoadingScene.OnUpdate_Internal(m_SceneContext);
			}

			if (AudioService != null)
			{
				AudioService.OnUpdate();
			}

			m_Coroutines.Update_Internal();

#if UNITY_EDITOR
			UpdateTime_Debug();
#endif
		}

		private void LateUpdate()
		{
			if (m_CurrentScene != null)
			{
				m_CurrentScene.OnLateUpdate_Internal(m_SceneContext);
			}
		}

		// PRIVATE METHODS

		private void Initialize()
		{
			m_Coroutines = gameObject.AddComponent<Coroutines>();
			m_Coroutines.Initialize();

			m_Input = gameObject.AddComponent<InputService>();
			m_Input.Initialize();

			m_QuantumServices = new Services();
			m_QuantumServices.Initialize();
		}

		private void Deinitialize()
		{
			m_Coroutines.Deinitialize();
			m_Coroutines = null;

			m_Input.Deinitialize();
			m_Input = null;

			m_QuantumServices.Deinitialize();
			m_QuantumServices = null;
		}

		private void AssignCurrentScene()
		{
			var unityScene = SceneManager.GetActiveScene();
			unityScene.GetRootGameObjects(m_GameObjectCache);

			foreach (var go in m_GameObjectCache)
			{
				m_SceneContext.ActiveScene = m_CurrentScene = go.GetComponent<Scene>();
				if (m_CurrentScene != null)
					break;
			}
		}

		private void LoadNextScene()
		{
			if (IsLoading == true && IsRestarting == false)
				return;

			if (IsRestarting == true)
			{
				IsRestarting = false;
				StartCoroutine(LoadScene_Coroutine(MAIN_SCENE_NAME));
				return;
			}

			Debug.Log(m_CurrentScene.name);

			if (m_CurrentScene is StartScene)
			{
				if (IntroScene.CanShowIntro() == true)
				{
					StartCoroutine(LoadScene_Coroutine(INTRO_SCENE_NAME));
				}
				else
				{
					StartCoroutine(LoadScene_Coroutine(MAIN_SCENE_NAME));
				}
			}
			else if (m_CurrentScene is IntroScene)
			{
				StartCoroutine(LoadScene_Coroutine(MAIN_SCENE_NAME));
			}
			else if (m_CurrentScene is LoginCosmic)
			{
				StartCoroutine(LoadScene_Coroutine(MAIN_SCENE_NAME));
			}
			else if (m_CurrentScene is MenuCosmic)
			{
				StartCoroutine(LoadScene_Coroutine(AutoStart_SCENE_NAME));
			}
			else if (m_CurrentScene is MainScene)
			{
				StartCoroutine(LoadScene_Coroutine(GameplayInfo.SceneName));
			}
			else if (m_CurrentScene is GameCosmic)
			{
				StartCoroutine(LoadScene_Coroutine(MenuCosmic_SCENE_NAME));
			}
		}

		private IEnumerator LoadScene_Coroutine(string sceneName)
		{
			IsLoading = true;

			yield return ShowLoadingScene_Coroutine();

			QuantumServices.Network.Pause();

			if (IsError == true)
				yield break;

			if (m_CurrentScene != null)
			{
				m_CurrentScene.Deactivate();
				m_CurrentScene.Deinitialize();
				m_CurrentScene = null;
			}

			yield return Resources.UnloadUnusedAssets();

			var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
			while (asyncOperation.isDone == false)
				yield return null;

			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

			AssignCurrentScene();

			yield return m_CurrentScene.Initialize();

			yield return null;
			yield return null;

			QuantumServices.Network.Unpause();
			
			m_CurrentScene.Activate();
			while (m_CurrentScene.Active == false)
				yield return null;

			yield return Resources.UnloadUnusedAssets();

			if (IsError == true)
				yield break;

			yield return HideLoadingScene_Coroutine();

			IsLoading = false;
		}

		private IEnumerator ShowLoadingScene_Coroutine()
		{
			if (IsError == true)
				yield break;

			var asyncOperation = SceneManager.LoadSceneAsync(LOADING_SCENE_NAME, LoadSceneMode.Additive);
			while (asyncOperation.isDone == false)
				yield return null;

			var unityScene = SceneManager.GetSceneByName(LOADING_SCENE_NAME);
			unityScene.GetRootGameObjects(m_GameObjectCache);
			foreach (var go in m_GameObjectCache)
			{
				m_LoadingScene = go.GetComponent<Scene>() as LoadingScene;
				if (m_LoadingScene != null)
				{
					yield return m_LoadingScene.Initialize();
					m_LoadingScene.Activate();

					while (m_LoadingScene.Active == false)
						yield return null;

					yield break;
				}
			}
		}

		private IEnumerator HideLoadingScene_Coroutine()
		{
			if (m_LoadingScene == null)
				yield break;

			m_LoadingScene.Deactivate();

			while (m_LoadingScene.Finished == false)
				yield return null;

			m_LoadingScene.Deinitialize();
			m_LoadingScene = null;
		}

#if UNITY_EDITOR
		private int m_ScaleStep;
		private void UpdateTime_Debug()
		{
			if (Input.GetKey(KeyCode.LeftControl) == true && Input.GetKey(KeyCode.LeftShift) == true)
			{
				var newScaleStep = m_ScaleStep;

				if (Input.GetKeyDown(KeyCode.Mouse2) == true)
				{
					newScaleStep = 0;
				}
				else if (Input.mouseScrollDelta.y > 0f)
				{
					newScaleStep = Mathf.Clamp(m_ScaleStep + 1, -9, 9);
				}
				else if (Input.mouseScrollDelta.y < 0f)
				{
					newScaleStep = Mathf.Clamp(m_ScaleStep - 1, -9, 9);
				}

				if (m_ScaleStep != newScaleStep)
				{
					if (newScaleStep > 0)
					{
						Time.timeScale = 1f + newScaleStep;
					}
					else
					{
						Time.timeScale = 1f + (newScaleStep * 0.1f);
					}

					Debug.Log("New time scale: " + Time.timeScale);
					m_ScaleStep = newScaleStep;
				}
			}
		}
#endif
	}
}
