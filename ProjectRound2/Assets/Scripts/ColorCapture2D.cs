using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ColorCapture2D : MonoBehaviour {

	public Texture2D OriginalTexture;
	public Texture2D NewTexture;
	public Vector2 spriteInCanvasPos; // relative to canvas
	public Texture2D oriSprite;
	public float scale; // spriteInCanvas divided by the original sprite

	public GameObject TargetObj;
	public GameObject test;
	private Transform []ObjContents; // need to be same order as sprites

	private Sprite [] sprites;
	public string FileExtension;

	private Sprite[] newSprite;


	void Start() {
		UnityEngine.Object[] allSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/Sprites/"+OriginalTexture.name+"."+FileExtension);
		sprites = Array.ConvertAll(allSprites, item => item as Sprite);
		newSprite = new Sprite[sprites.Length];
		ObjContents = new Transform[sprites.Length];
		FindChildAndSetData(TargetObj.transform);
		
		//CombineTexture2DAndGameObject (NewTexture);
	}

	void FindChildAndSetData(Transform t) {
		SpriteRenderer renderer = t.GetComponent<SpriteRenderer>();
		string numbersOnly = Regex.Replace(renderer.sprite.name, "[^0-9]", "");
		Debug.Log (int.Parse (numbersOnly));
		ObjContents [int.Parse (numbersOnly)] = t;
		foreach (Transform pt in t) {
			FindChildAndSetData(pt);
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 40, 100, 35), "Done"))  {
			Impose();
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

	void Impose()
	{
		GameController gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		Texture2D canvasTexture = gameController.GetCanvasTexture ();
		Texture2D newSprite = new Texture2D (oriSprite.width, oriSprite.height, TextureFormat.ARGB32, false);
		print ("tessst : " + oriSprite.width + " " + oriSprite.height);
		//fill the color from the canvas

		for (int i=0; i<newSprite.height; i++) 
		{
			for (int j=0; j<newSprite.width; j++) 
			{
				float u = (spriteInCanvasPos.x + (j * scale)) * 1.0f / (canvasTexture.width - 1);
				float v = (spriteInCanvasPos.y + (i * scale)) * 1.0f / (canvasTexture.height - 1);

				if(i==newSprite.height - 1 && j == newSprite.width - 1)
				{
					print (u + " " + v);
				}

				Color color = canvasTexture.GetPixelBilinear(u, v);
				if(color.r == 1.0f && color.g == 1.0f && color.b == 1.0f)
				{
					color.a = 0f;
				}

				newSprite.SetPixel(j, i, color);
			}
		}

		newSprite.Apply ();

		test.renderer.material.mainTexture = newSprite;

		CombineTexture2DAndGameObject (newSprite);
	}

}
