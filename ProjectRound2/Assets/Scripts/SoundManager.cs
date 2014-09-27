using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public GameObject BGPlayer;
	public AudioSource BGPlayerAudioSouce;

	public GameObject ButtonSoundPlayer;
	public AudioSource ButtonSoundAudioSource;

	public GameObject AnimationSoundPlayer;
	public AudioSource AnimationSoundAudioSource;

	private string[] bgMusic = new string[] {"IntroScreenBG", "BlankBGM", "BirdsBGM", "HouseBGM", "HumanBGM"};
	private string bgMusicPath = "Sounds/BGMusic/";

	private string[] buttonSound = new string[] {"BrushSelector", "CircleSelector", "ColourSelector", "EraserSelector"};
	private string buttonSoundPath = "Sounds/ButtonSound/";

	private string[] animationSound = new string[] {"BirdSounds"};
	private string animationSoundPath = "Sounds/AnimationSound/";

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

		ButtonSoundPlayer = new GameObject("ButtonSoundPlayer");
		ButtonSoundPlayer.transform.parent = transform;
		ButtonSoundAudioSource = ButtonSoundPlayer.AddComponent<AudioSource> ();

		AnimationSoundPlayer = new GameObject("AnimationSoundPlayer");
		AnimationSoundPlayer.transform.parent = transform;
		AnimationSoundAudioSource = AnimationSoundPlayer.AddComponent<AudioSource> ();
	}

	public void PlayBGMusic(int index, bool ifLoop) {
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(bgMusicPath,bgMusic[index]), typeof(AudioClip));
		BGPlayerAudioSouce.clip = newClip;
		BGPlayerAudioSouce.loop = ifLoop;
		BGPlayerAudioSouce.Play ();
	}

	public void PlayBGMusic(int index) {
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(bgMusicPath,bgMusic[index]), typeof(AudioClip));
		BGPlayerAudioSouce.clip = newClip;
		BGPlayerAudioSouce.loop = true;
		BGPlayerAudioSouce.Play ();
	}

	public void PlayButtonSound(int index) {
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(buttonSoundPath,buttonSound[index]), typeof(AudioClip));
		ButtonSoundAudioSource.clip = newClip;
		ButtonSoundAudioSource.Play();
	}

	public void PlayAnimationSound(int index) {
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(animationSoundPath,animationSound[index]), typeof(AudioClip));
		Debug.Log (newClip);
		AnimationSoundAudioSource.clip = newClip;
		AnimationSoundAudioSource.Play();
	}
	
}
