using UnityEngine;
using System;
using System.Collections;

public class BrushShape : MonoBehaviour {

	public byte[] matrix; // flattened 2d array
	public int width;
	public int height;

	public static BrushShape Circle = new BrushShape (new byte[] {
				0,0,1,1,0,0,
				0,1,1,1,1,0,
				1,1,1,1,1,1,
				1,1,1,1,1,1,
				0,1,1,1,1,0,
				0,0,1,0,0,0
		}, 6, 6);

	public static BrushShape SuperHugeSquare = new BrushShape(new byte[] {
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,	
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
	}, 22, 22);

	public static BrushShape TShape = new BrushShape(new byte[] {
				1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,
	}, 22, 8);

	public BrushShape(byte[] matrix, 
	                  int width, 
	                  int height)
	{
		this.matrix = matrix;
		this.width = width;
		this.height = height;
	}

	public BrushShape(Texture2D texture)
	{
		int x = 0, y = 0;
		width = texture.width;
		height = texture.height;
		matrix = new byte[width * height];

		//for (int i = height - 1; i >= 0; i--) 
//		for (int i = 0; i < height; i++)
//		{
//			for(int j = 0;j < width; j++)
//			{
//				matrix[i*width + j] = Convert.ToByte(texture.GetPixel(j, i).r == 0);
//			}
//		}

		y = 0;
		for (int i = height - 1; i >= 0; i--) 
		{
			x = 0;
			
			for(int j = 0; j < width; j++)
			{	
				matrix[i*width + j] = Convert.ToByte(texture.GetPixel(x, y).r == 0);
				x++;
			}
			y++;
		}
	}

	public static BrushShape CreateSquare(int width, int height)
	{
		BrushShape shape = new BrushShape (new byte[width * height], width, height);

		for (int i=0; i<width * height; i++) 
		{
			shape.matrix [i] = 1;
		}

		return shape;
	}
}
