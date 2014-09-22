using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ColorCapture2D : MonoBehaviour {

	public Texture2D OriginalTexture;
	public Texture2D NewTexture;

	public GameObject TargetObj;
	public Transform []ObjContents; // need to be same order as sprites

	public Sprite [] sprites;
	public string FileExtension;
	public Vector2 OriginalPivot;

	private Sprite[] newSprite;


	void Start() {
		UnityEngine.Object[] allSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/Sprites/"+OriginalTexture.name+"."+FileExtension);
		sprites = Array.ConvertAll(allSprites, item => item as Sprite);
		newSprite = new Sprite[sprites.Length];
		CombineTexture2DAndGameObject (NewTexture);

		Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
		Debug.Log ("test:" + ts.Length);

		//ObjContents = new Transform[sprites.Length];
		foreach (Transform t in TargetObj.transform) {
			
		}


	}

	public void CombineTexture2DAndGameObject(Texture2D texture) {

		for (int i=0; i<ObjContents.Length; i++) {
			SpriteRenderer renderer = ObjContents[i].GetComponent<SpriteRenderer>();
			string numbersOnly = Regex.Replace(renderer.sprite.name, "[^0-9]", "");
			Bounds bounds =renderer.bounds;
			Vector2 position = ObjContents[i].transform.position;
			Vector2 min = bounds.min;
			Vector2 size = bounds.size;
			Vector2 offsetOfAbsolutePositionRelativelyToMinOfBounds = position - min;
			Vector2 pivotVector =
				new Vector2(
					offsetOfAbsolutePositionRelativelyToMinOfBounds.x/size.x,
					offsetOfAbsolutePositionRelativelyToMinOfBounds.y/size.y
					);
			Sprite s = sprites [i];
			newSprite[i] = Sprite.Create (texture, s.rect, pivotVector);
			renderer.sprite = newSprite [i];
		}
	}
}
