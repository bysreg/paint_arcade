using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TheBoxWorld {
	public class SceneManager : MonoBehaviour {

		public float loadingProgress;
		public List<GameObject> SceneTransferEffects;
		private GameObject sceneEntry;
		private bool isLoading = false;
		private string targetLoadingSceneName;
		private AsyncOperation async;

		static SceneManager _instance;
		
		static public bool isActive { 
			get { 
				return _instance != null; 
			} 
		}
		
		static public SceneManager instance {
			get {
				if (_instance == null) {
					_instance = Object.FindObjectOfType(typeof(SceneManager)) as SceneManager;
					if (_instance == null) {
						GameObject go = new GameObject("_scenemanager");
						DontDestroyOnLoad(go);
						_instance = go.AddComponent<SceneManager>();
						_instance.init();
					}
				}
				return _instance;
			}
		}
		
		void init() {
			loadingProgress = 0f;
			SceneTransferEffects = new List<GameObject>();
			loadAllEffects();
		}

		void loadAllEffects() {
			SceneTransferEffects.Add(Resources.Load("Prefabs/Fadein", typeof(GameObject)) as GameObject);
		}

		void Update() {

			if(isLoading && async != null) {
				loadingProgress = async.progress;
			}

			if(isLoading && Application.loadedLevelName == targetLoadingSceneName) {
				isLoading = false;
				applyEffect();
			}
		
		}

		public void loadScene(string name) {
			targetLoadingSceneName = name;
			isLoading = true;
			Application.LoadLevel(name);
		}

		void applyEffect() {
			sceneEntry = GameObject.FindGameObjectWithTag("scene_entry");
			if(sceneEntry) {
				GameObject effect = Instantiate(SceneTransferEffects[0], Vector3.zero, Quaternion.identity) as GameObject;
				effect.transform.parent = sceneEntry.transform;
			} else {
				Debug.Log("Cannot find scene entry in Scene " + Application.loadedLevelName);
			}

		}

		public void asyncLoadScene(string name) {
			isLoading = true;
			targetLoadingSceneName = name;
			StartCoroutine (AsyncLoadLevel (name));
		}

		public void asyncLoadSceneWithLoadingScreen(string name) {
			loadScene("Loading");
			targetLoadingSceneName = name;
			isLoading = true;
			loadingProgress = 0f;
			StartCoroutine (AsyncLoadLevel (name));
		}

		IEnumerator AsyncLoadLevel (string name) {
			async = Application.LoadLevelAsync (name);
			while (!async.isDone) {
				yield return 0;
			}
		}
	}
}