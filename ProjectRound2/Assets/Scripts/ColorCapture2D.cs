using UnityEngine;
using System.Collections;

public class ColorCapture2D : MonoBehaviour {

	public GameObject creature;
	public Vector2 spriteInCanvasPos; // relative to canvas
	public Texture2D oriSprite;
	public float scale; // spriteInCanvas divided by the original sprite

	public GameObject test;

	void Start()
	{
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 40, 100, 35), "Done"))  {
			Impose();
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
				newSprite.SetPixel(j, i, color);
			}
		}

		newSprite.Apply ();

		test.renderer.material.mainTexture = newSprite;

		//ChangeTexture (creature, canvasTexture);
	}

	void ChangeTexture(GameObject obj, Texture2D texture)
	{

	}
}
