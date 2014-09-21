using UnityEngine;
using System.Collections;

public class CustomBrushShapes : MonoBehaviour {

	public Texture2D[] brushTextures;

	public static BrushShape[] brushShapes;

	void Awake()
	{
		brushShapes = new BrushShape[brushTextures.Length];

		for (int i=0; i<brushTextures.Length; i++) 
		{
			brushShapes[i] = new BrushShape(brushTextures[i]);
			//print(brushShapes[i].width + " " + brushShapes[i].height);
		}
	}

}
