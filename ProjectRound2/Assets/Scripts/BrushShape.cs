using UnityEngine;
using System;
using System.Collections;

public class BrushShape {

	public byte[] matrix; // flattened 2d array
	public int width;
	public int height;

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

	public static BrushShape CreateCircle(int radius)
	{
		int width = radius * 2 + 1;
		BrushShape shape = new BrushShape (new byte[width * width], width, width);
		int x0 = radius, y0 = radius;
		int x = radius;
		int y = 0;
		int radiusError = 1-x;
		
		while(x >= y)
		{
			DrawPixelInMat(shape.matrix, x + x0, y + y0, shape.width);
			DrawPixelInMat(shape.matrix, y + x0, x + y0, shape.width);
			DrawPixelInMat(shape.matrix, -x + x0, y + y0, shape.width);
			DrawPixelInMat(shape.matrix, -y + x0, x + y0, shape.width);
			DrawPixelInMat(shape.matrix, -x + x0, -y + y0, shape.width);
			DrawPixelInMat(shape.matrix, -y + x0, -x + y0, shape.width);
			DrawPixelInMat(shape.matrix, x + x0, -y + y0, shape.width);
			DrawPixelInMat(shape.matrix, y + x0, -x + y0, shape.width);
			y++;
			if (radiusError<0)
			{
				radiusError += 2 * y + 1;
			}
			else
			{
				x--;
				radiusError += 2 * (y - x + 1);
			}
		}

		//fill the circle
		int startfill_x = 0;
		int cur_y = 0;
		bool find1 = false;

		for(int i=1; i<shape.height-1; i++)
		{
			find1 = false;

			//search for last 1 in line
			for(int j=0;j<shape.width; j++)
			{
				byte value = shape.matrix[j + i * shape.width];
				if(value == 1 && find1 == false)
				{
					find1 = true;
				}
				else if(value == 0 && find1)
				{
					startfill_x = j - 1;
					break;
				}
			}

			for(int j=startfill_x + 1;j<shape.width; j++)
			{
				byte value = shape.matrix[j + i * shape.width];
				if(value == 1)
					break;
				DrawPixelInMat(shape.matrix, j, i, shape.width);
			}
		}

//		String test = "";
//		for (int i=0; i<width; i++) 
//		{
//			for(int j = 0; j < width; j++)
//			{
//				test +=  shape.matrix[j + i * width];
//			}
//			test += "\n";
//		}
//
//		print (test);

		return shape;
	}

	static void DrawPixelInMat(byte[] mat, int x, int y, int width)
	{
		int i = x + y * width;
		mat [i] = 1;
	}
}
