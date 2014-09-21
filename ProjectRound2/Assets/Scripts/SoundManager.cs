﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public GameObject BGPlayer;
	public AudioSource BGPlayerAudioSouce;

	public GameObject ButtonSoundPlayer;
	public AudioSource ButtonSoundAudioSource;

	private string[] bgMusic = new string[] {"BlankBG"};
	private string bgMusicPath = "Sounds/BGMusic/";

	private string[] buttonSound = new string[] {};
	private string buttonSoundPath = "Sounds/Button/";



	static SoundManager _instance;

	static public bool isActive { 
		get { 
			return _instance != null; 
		} 
	}
	
	static public SoundManager instance {
		get {
			if (_instance == null) {
				_instance = Object.FindObjectOfType(typeof(SoundManager)) as SoundManager;
				
				if (_instance == null) {
					GameObject go = new GameObject("_soundmanager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<SoundManager>();
					_instance.Init();
				}
			}
			return _instance;
		}
	}

	private void Init() {
		BGPlayer = new GameObject("BGPlayer");
		BGPlayer.transform.parent = transform;
		BGPlayerAudioSouce = BGPlayer.AddComponent<AudioSource> ();
	}

	public void PlayBGMusic(int index) {
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(bgMusicPath,bgMusic[index]), typeof(AudioClip));
		BGPlayerAudioSouce.clip = newClip;
		BGPlayerAudioSouce.Play ();
	}
	
}
