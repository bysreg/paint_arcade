using UnityEngine;
using System.Collections;

public class ShaderEffect : MonoBehaviour {
	private Material oldFilmMat;

	void Start() {
		oldFilmMat = Resources.Load ("Shader/OldFilmShader", typeof(Material)) as Material;
		if (oldFilmMat == null) {
			Debug.Log("Cannot find OldFilmShader");
			this.enabled = false;
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, oldFilmMat);
	}
}
