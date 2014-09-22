﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
	
public class ColorCapture2D : MonoBehaviour {
	
	public Vector2 spriteInCanvasPos; // relative to canvas
	public Texture2D oriSprite;
	//public float scale; // spriteInCanvas divided by the original sprite
	public Vector2 sizeInCanvas;

	public GameObject TargetObj;
	public GameObject test;
	private Transform []ObjContents; // need to be same order as sprites
	public int SpriteNum;

	public string FileExtension;

	private Sprite[] newSprite;

	public Texture2D testReplacedTexture;

	void Start() {
		newSprite = new Sprite[SpriteNum];
		ObjContents = new Transform[SpriteNum];
		FindChildAndSetData(TargetObj.transform);

		//testing code
		if (testReplacedTexture != null) 
		{
			CombineTexture2DAndGameObject(testReplacedTexture);
		}
	}

	void FindChildAndSetData(Transform t) {
		SpriteRenderer renderer = t.GetComponent<SpriteRenderer>();
		string numbersOnly = Regex.Replace(renderer.sprite.name, ".+[^0-9]", "");
		Debug.Log (int.Parse (numbersOnly));
		ObjContents [int.Parse (numbersOnly)] = t;
		foreach (Transform pt in t) {
			FindChildAndSetData(pt);
		}
	}

	public void CombineTexture2DAndGameObject(Texture2D texture) {

		if (ObjContents == null)
			return;

		for (int i=0; i<ObjContents.Length; i++) {
			SpriteRenderer renderer = ObjContents[i].GetComponent<SpriteRenderer>();
			string numbersOnly = Regex.Replace(renderer.sprite.name, ".+[^0-9]", "");
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
			newSprite[i] = Sprite.Create (texture, renderer.sprite.rect, pivotVector);
			renderer.sprite = newSprite [i];
		}
	}

	public void Impose()
	{
		GameController gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		Texture2D canvasTexture = gameController.GetCanvasTexture ();
		Texture2D canvasBg = gameController.GetCanvasBg ();
		Texture2D newSprite = new Texture2D (oriSprite.width, oriSprite.height, TextureFormat.ARGB32, false);
		//print ("tessst : " + oriSprite.width + " " + oriSprite.height);
		//fill the color from the canvas

		float xscale = sizeInCanvas.x * 1.0f / newSprite.width;
		float yscale = sizeInCanvas.y * 1.0f / newSprite.height;
		//print ("aaaaaaaaa : " + xscale + " " + yscale);

		for (int i=0; i<newSprite.height; i++) 
		{
			for (int j=0; j<newSprite.width; j++) 
			{
				float u = (spriteInCanvasPos.x + (j * xscale)) * 1.0f / (canvasBg.width - 1);
				float v = (spriteInCanvasPos.y + (i * yscale)) * 1.0f / (canvasBg.height - 1);

				if(i==newSprite.height - 1 && j == newSprite.width - 1)
				{
					print ("lalalal : " + u + " " + v);
					print ("x comp : " + spriteInCanvasPos.x + " " + j + " " + canvasTexture.width + " " + xscale);
					print ("y comp : " + spriteInCanvasPos.y + " " + i + " " + canvasTexture.height + " " + yscale);
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

		if(test != null)
			test.renderer.material.mainTexture = newSprite;

		CombineTexture2DAndGameObject (newSprite);
	}

}