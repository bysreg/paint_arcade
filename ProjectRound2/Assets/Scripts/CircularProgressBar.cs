using UnityEngine;
using System.Collections;

public class CircularProgressBar : MonoBehaviour {
	public float currentProgress;
	void Start() {
		gameObject.renderer.material.SetFloat ("_Cutoff", 1);
	}

	public void Activate() {
		Deactivate();
		StartCoroutine(RadialProgress(1));
	}

	public void Deactivate() {
		StopAllCoroutines();
		currentProgress = 0;
		gameObject.renderer.material.SetFloat("_Cutoff", 0.99f);

	}


	IEnumerator RadialProgress(float time) {
		float rate = 1 / time;
		while (currentProgress < 0.99f) {
			currentProgress += Time.deltaTime * rate;
			gameObject.renderer.material.SetFloat("_Cutoff", 1-currentProgress);
			yield return 0;
		}
		gameObject.renderer.material.SetFloat("_Cutoff", 0.99f);
	}
}
