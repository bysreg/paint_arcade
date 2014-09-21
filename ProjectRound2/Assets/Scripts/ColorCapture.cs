using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorCapture : MonoBehaviour {

	class Mapping
	{
		public Vector2 canvasUV;
		public Vector2 destUV;
	}

	public Texture2D lineTexture;
	public Texture2D destTexture; // the original uv texture of the model to be replaced
	public Texture2D destReplaceTexture; // texture that contains only black and white and correspond to the dest texture
	public float scale;
	public Vector2 startBoxPos;

	//private List<Vector2> uvs; // from the lineTexture
	private List<Mapping> mappings;

	void Start()
	{
		int lineTexWidth = lineTexture.width;
		int lineTexHeight = lineTexture.height;
		int	uvWidth = (int) (scale * destTexture.width); // uv width in lineTexture
		int uvHeight = (int) (scale * destTexture.height); // uv height in lineTexture
//		uvs = new List<Vector2> ();
		mappings = new List<Mapping> ();

		for (int i=0; i < lineTexture.height; i++) 
		{
			for(int j=0; j < lineTexture.width; j++)
			{
				if(lineTexture.GetPixel(j, i).r == 0) // black ?
				{
					Mapping mapping = new Mapping();
					mapping.canvasUV = new Vector2(j  * 1.0f / (lineTexWidth - 1), i * 1.0f / (lineTexHeight - 1));
					mapping.destUV = new Vector2((j - startBoxPos.x) * 1.0f / (uvWidth - 1), (i - startBoxPos.y) * 1.0f / (uvHeight - 1));
					mappings.Add(mapping);
				}
			}
		}

		print ("test " + mappings.Count + " " + mappings[0].canvasUV.x + " " + mappings[0].canvasUV.y);
	}

	void Update()
	{

	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 40, 100, 35), "Done"))  {
			//FinishFillingColor();
			Impose();
		}
	}


	void Impose()
	{
		GameObject skeleton = GameObject.Find("Skeleton/Skeleton");
		int matwidth = destTexture.width;
		int matheight = destTexture.height;
		Texture2D oriTexture = destTexture;
		Texture2D exportTexture = new Texture2D (matwidth, matheight, TextureFormat.ARGB32, false);
		Texture2D canvasTexture = GameObject.Find ("GameController").GetComponent<GameController> ().GetCanvasTexture ();

		for (int i=0; i<matheight; i++) {
			for (int j=0; j<matwidth; j++) {
				exportTexture.SetPixel (j, i, oriTexture.GetPixel (j, i));
			}
		}

		for(int i=0;i<mappings.Count;i++)
		{
			Color color = canvasTexture.GetPixelBilinear(mappings[i].canvasUV.x, mappings[i].canvasUV.y);
			exportTexture.SetPixel((int) (mappings[i].destUV.x * matwidth), (int) (mappings[i].destUV.y * matheight), color);
		}

		exportTexture.Apply ();
		ApplyToObject (exportTexture, skeleton);
	}

//	void FinishFillingColor()
//	{
//		int width = lineTexture.width;
//		int height = lineTexture.height;
//		Texture2D exportTexture = new Texture2D (width, height, TextureFormat.ARGB32, false);
//		Texture2D canvasTexture = GameObject.Find ("GameController").GetComponent<GameController> ().GetCanvasTexture ();
//		GameObject earthObj = GameObject.Find ("EarthCube");
//
//		for (int i=0; i<height; i++) 
//		{
//			for(int j=0;j<width;j++)
//			{
//				float u = j * 1.0f / (width - 1);
//				float v = i * 1.0f / (height - 1);
//
//				exportTexture.SetPixel(j, i, canvasTexture.GetPixelBilinear(u, v));
//
//				if(i<=0)
//					print (u + " " + v + " " + exportTexture.GetPixelBilinear(u, v));
//				//exportTexture.SetPixel(j, i, Color.cyan);
//			}
//		}
//
//		exportTexture.Apply ();
//
//
//		ApplyToObject (exportTexture, earthObj);
//	}

	void ApplyToObject(Texture2D texture, GameObject obj)
	{
		print (obj.renderer.material.mainTexture);
		obj.renderer.material.mainTexture = texture;
	}
}
