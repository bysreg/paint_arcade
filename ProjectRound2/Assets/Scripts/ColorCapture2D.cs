using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ColorCapture2D : MonoBehaviour {

	class Mapping
	{
		public Vector2 canvasUV;
		public Vector2 destUV;
	}

	public Vector2 spriteInCanvasPos; // relative to canvas
	public Texture2D oriSprite;
	public Vector2 sizeInCanvas; // compared to the canvasBg
	public Texture2D drawableArea; // black and white texture that acts as a mask. black means that the player can draw in that area.
	public Texture2D lineTexture;
	public GameObject TargetObj;
	public GameObject test;
	public int SpriteNum;
	public string FileExtension;
	public Texture2D testReplacedTexture;
	public PlayerCreation.CreationType creationType;

	private Transform[] ObjContents; // need to be same order as sprites
	private List<Mapping> mappings;

	void Start() 
	{
		ObjContents = new Transform[SpriteNum];
		FindChildAndSetData(ObjContents, TargetObj.transform);

		//testing code
		if (testReplacedTexture != null) 
		{
			CombineTexture2DAndGameObject(SpriteNum, ObjContents, testReplacedTexture);
		}

		mappings = new List<Mapping>();
		for (int i=0; i < drawableArea.height; i++) 
		{
			for(int j=0; j < drawableArea.width; j++)
			{
				Color color = drawableArea.GetPixel(j, i);
				if(color.r <= 0.1) // black ?
				{
					//print ("a : " + j + " " + i + " " + color.r);
					Mapping mapping = new Mapping();
					mapping.canvasUV = new Vector2(j  * 1.0f / (drawableArea.width - 1), i * 1.0f / (drawableArea.height - 1)); // intentionally use lineTexWidth and height rather than canvas to make it more accurate
					mapping.destUV = new Vector2((j - spriteInCanvasPos.x) * 1.0f / (sizeInCanvas.x - 1), (i - spriteInCanvasPos.y) * 1.0f / (sizeInCanvas.y - 1));
                    mappings.Add(mapping);
                }
            }
        }
		//print ("count " + mappings.Count);
	}

	public static void FindChildAndSetData(Transform[] contents, Transform t) 
	{
		SpriteRenderer renderer = t.GetComponent<SpriteRenderer>();
		string numbersOnly = Regex.Replace(renderer.sprite.name, ".+[^0-9]", "");
		//Debug.Log (int.Parse (numbersOnly));
		contents [int.Parse (numbersOnly)] = t;
		foreach (Transform pt in t) {
			FindChildAndSetData(contents, pt);
		}
	}

	public static void CombineTexture2DAndGameObject(int spriteNum, Transform[] contents, Texture2D texture) 
	{
		if (contents == null)
			return;

		Sprite[] newSprite = new Sprite[spriteNum];

		for (int i=0; i<contents.Length; i++) {
			SpriteRenderer renderer = contents[i].GetComponent<SpriteRenderer>();
			string numbersOnly = Regex.Replace(renderer.sprite.name, ".+[^0-9]", "");
			Bounds bounds =renderer.bounds;
			Vector2 position = contents[i].transform.position;
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

		//initialize with white color
		for(int i=0;i<newSprite.height;i++)
		{
			for(int j=0;j<newSprite.width;j++)
			{
				newSprite.SetPixel(j, i, new Color(1, 1, 1, 0));
			}
		}

		for(int i=0;i<mappings.Count;i++)
		{
			Color color = canvasTexture.GetPixelBilinear(mappings[i].canvasUV.x, mappings[i].canvasUV.y);
			newSprite.SetPixel((int) (mappings[i].destUV.x * newSprite.width), (int) (mappings[i].destUV.y * newSprite.height), color);
		}
		//apply lines
		Color[] linesColor = lineTexture.GetPixels ();

		int k = 0;
		for (int i=0; i<lineTexture.height; i++) 
		{
			for (int j=0; j < lineTexture.width; j++, k++) 
			{
				if(linesColor[k].a < 0.04)
				{
					continue;
				}

				newSprite.SetPixel(j, i, linesColor[k]);
			}
		}

		newSprite.Apply ();

		if(test != null)
			test.renderer.material.mainTexture = newSprite;

		gameController.SavePlayerCreation (newSprite, creationType, SpriteNum);

		CombineTexture2DAndGameObject (SpriteNum, ObjContents, newSprite);

		//deactivate the drawable
		GameObject.Find ("Drawable").SetActive (false);
	}

}
