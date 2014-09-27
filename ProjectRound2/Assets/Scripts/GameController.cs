using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Kinect;
using PaintArcade.Generic;

public class GameController : MonoBehaviour {

	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;
	public int maxHands;
	public PlayerHand KinectRightHand;
	public Texture2D canvasBg; // mandatory
	public Texture2D canvasDrawableArea;
	public int brushRadius = 10;
	public int eraserRadius = 20;

	Texture2D canvasTexture;
	GameObject canvasObject;
	GameObject canvasBgObject;
	BrushShape brushShape;
	BrushShape eraserShape;
	PlayerHandData[] hands;
	Color[] canvasDrawableAreaColors;
	static List<PlayerCreation> savedPlayerCreations;

	void Awake()
	{
		canvasObject = GameObject.Find ("Canvas");
		canvasBgObject = GameObject.Find ("CanvasBg");
		hands = new PlayerHandData[maxHands];
		savedPlayerCreations = new List<PlayerCreation> ();
	}

	void Start()
	{
		InitCanvasTexture ();
		InitPlayerHands ();

		canvasObject.renderer.material.mainTexture = canvasTexture;
//		brushShape = CustomBrushShapes.brushShapes [0];
//		brushShape = BrushShape.CreateSquare (30, 30);
		brushShape = BrushShape.CreateCircle (Consts.BrushSizeSmall);
		eraserShape = BrushShape.CreateCircle (eraserRadius);
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 40, 100, 35), "Done"))  {
			ColorCapture2D[] colorCapture2Ds = GetComponents<ColorCapture2D>();
			for(int i=0; i<colorCapture2Ds.Length;i++)
			{
				colorCapture2Ds[i].Impose();
				//activate that object
				colorCapture2Ds[i].TargetObj.SetActive(true);
			}
		}

		if(GUI.Button(new Rect(140, 40, 100, 35), "Next"))  {
			Application.LoadLevel("Final");
		}
	}

	void Update()
	{
		HandleKeyPress ();

		if (simulateWithMouse) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			int layerMask = (1 << LayerMask.NameToLayer ("Canvas"));
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.yellow);

			bool inCanvas = Physics.Raycast (ray, out hitInfo, 100, layerMask);

			hands[0].prevIsHandDown = hands [0].isHandDown;
			hands[0].prevPos = hands[0].pos;
			hands[0].pos.x = (int) (hitInfo.textureCoord.x * canvasWidth);
			hands[0].pos.y = (int) (hitInfo.textureCoord.y * canvasHeight);
			hands[0].uvpos.x = hitInfo.textureCoord.x;
			hands[0].uvpos.y = hitInfo.textureCoord.y;
		
			//print (hitInfo.textureCoord.x + " " + hitInfo.textureCoord.y);
			//print (hands[0].pos.x + " " + hands[0].pos.y);

			if (inCanvas)
				hands [0].isHandDown = Input.GetMouseButton (0);
			else
				hands [0].isHandDown = false;
		}
		else
		{
			//Right Hand
			{
				hands[0].prevIsHandDown = KinectRightHand.isHandDown;
				hands[0].prevPos = hands[0].pos;
				hands[0].isHandDown = KinectRightHand.isHandDown;
				hands[0].color = KinectRightHand.color;
				hands[0].tool = KinectRightHand.tool;
				
				float x = KinectRightHand.transform.position.x - KinectRightHand.collider.bounds.size.x*.5f;
				float y = KinectRightHand.transform.position.y + KinectRightHand.collider.bounds.size.y*1.5f;
				float width = canvasObject.collider.bounds.size.x;
				float height = canvasObject.collider.bounds.size.y;
				int px = (int)((width*.5f+x)/width*canvasWidth+canvasObject.transform.position.x);
				int py = (int)((height*.5f+y)/height*canvasHeight+canvasObject.transform.position.y);
				
				hands[0].pos = new Vector2(px - brushShape.width*.5f, py + brushShape.height*.5f);
			}
			
		}

		for (int i=0; i<maxHands; i++) 
		{
			if (hands [i].isHandDown) 
			{
				if(hands[i].tool == ETool.Brush)
				{
					ConnectBrushPoint(hands[i], DrawBrush, brushShape);
				}
				else if(hands[i].tool == ETool.Eraser)
				{
					ConnectBrushPoint(hands[i], Erase, eraserShape);
				}
			}
		}
	}

	void Erase(Vector2 pos, Color color, BrushShape brushShape)
	{
		int k = 0;
		int x, y;
		y = (int) (pos.y - brushShape.height/ 2.0f);
		for (int i = brushShape.height - 1; i >= 0; i--) 
		{
			x = (int) (pos.x - brushShape.width / 2.0f);
			
			for(int j = 0;j < brushShape.width; j++)
			{
				if (x < 0 || x >= canvasWidth)
					continue;
				if (y < 0 || y >= canvasHeight)
					continue;
				
				//print (x + " " + y + " " + i + " " + j);
				float u = x * 1.0f / (canvasWidth - 1);
				float v = y * 1.0f / (canvasHeight - 1);
				Color oriColor = canvasBg.GetPixelBilinear(u, v);
				oriColor.a = 0f; // the part of the image will become transparent if erase. if this is equal to one, it will result in drawing the oiginal canvasBg

                if(brushShape.matrix[i*brushShape.width + j] == 1)
                    canvasTexture.SetPixel (x, y, oriColor);
                x++;
            }
            y++;
        }
        
        //intentionally not using canvasTexture.Apply, it will be done in ConnectBrushPoint
	}

	void DrawBrush(Vector2 pos, Color color, BrushShape brushShape)
	{
		int k = 0;
		int x, y;
		float u, v;

		y = (int) (pos.y - brushShape.height/ 2.0f);
		for (int i = brushShape.height - 1; i >= 0; i--) 
		{
			x = (int) (pos.x - brushShape.width / 2.0f);
			v = y * 1.0f / canvasHeight;

			for(int j = 0;j < brushShape.width; j++)
			{
				u = x * 1.0f / canvasWidth;

				if (x < 0 || x >= canvasWidth)
					continue;
				if (y < 0 || y >= canvasHeight)
					continue;

				//print (x + " " + y + " " + i + " " + j);
				if(brushShape.matrix[i*brushShape.width + j] == 1 && CheckIsDrawable(u, v))
				//if(brushShape.matrix[i*brushShape.width + j] == 1 && CheckIsDrawable(x, y))
					canvasTexture.SetPixel (x, y, color);
				x++;
			}
			y++;
		}

		//intentionally not using canvasTexture.Apply, it will be done in ConnectBrushPoint
	}

	bool CheckIsDrawable(float u, float v)
	{
		//fIXME
		int x, y;
		x = (int) (u * canvasBg.width);
		y = (int) (v * canvasBg.height);
		//x = (int) u;
		//y = (int) v;
		//return canvasDrawableAreaColors [u + v * canvasWidth].r <= 0.1;
		//return canvasDrawableAreaColors [x + y * canvasWidth].r <= 0.1;
		//print (x + " " + y);
		//return canvasDrawableAreaColors [x + y * canvasBg.width].r <= 0.1; // 2
		//return canvasDrawableArea.GetPixelBilinear (u, v).r > 0.;
		return canvasDrawableArea.GetPixel (x, y).r == 0;
//		return true;
	}

	void ConnectBrushPoint(PlayerHandData hand, Action<Vector2, Color, BrushShape> drawf, BrushShape brushShape)
	{
		if (!hand.prevIsHandDown || !hand.isHandDown)
			return;

		if (hand.pos.x < 0 || hand.pos.x >= canvasWidth)
			return;
		if (hand.pos.y < 0 || hand.pos.y >= canvasHeight)
            return;
        
        //print (pos0.x + " " + pos0.y + " " + pos1.x + " " + pos1.y + " " + hand.pos);
			 
 		//connect current pos and prev pos
		DrawLine (hand, drawf, brushShape);
	}

	// NOTE : pos0.x, pos0.y, pos1.x, pos1.y, must be integer
	void DrawLine(PlayerHandData hand, Action<Vector2, Color, BrushShape> drawf, BrushShape brushShape)
	{
		Vector2 pos0 = hand.prevPos;
		Vector2 pos1 = hand.pos;
		Color color = hand.color;

		float sx, sy, err, e2;
		float dx = Mathf.Abs(pos1.x - pos0.x);
		float dy = Mathf.Abs(pos1.y - pos0.y); 
		if (pos0.x < pos1.x)
			sx = 1; 
		else 
			sx = -1;
		if (pos0.y < pos1.y)  
			sy = 1; 
		else 
			sy = -1;
		err = dx - dy;
		
		while(true)
		{
			//DrawBrush(pos0, color, brushShape);
			drawf(pos0, color, brushShape);
			if (pos0.x == pos1.x && pos0.y == pos1.y) 
				break;
			e2 = 2 * err;
			if (e2 > -dy) 
			{
				err = err - dy;
				pos0.x = pos0.x + sx;
			}
			if (e2 < dx)
			{
				err = err + dx;
				pos0.y = pos0.y + sy ;
			}
		}

		canvasTexture.Apply ();
	}

	void HandleKeyPress()
	{
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			hands[0].tool = ETool.Eraser;
		}
		if (Input.GetKeyDown (KeyCode.X)) 
		{
			hands[0].tool = ETool.Brush;
		}
		if(Input.GetKeyDown (KeyCode.N))
		{
			hands[0].color = Color.blue;
		}
		if(Input.GetKeyDown (KeyCode.M))
		{
			hands[0].color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			hands[0].color = Color.black;
		}
		if (Input.GetKeyDown (KeyCode.W)) 
		{
			hands[0].color = Color.white;
		}
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			hands[0].color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			hands[0].color = Color.magenta;
		}
		if (Input.GetKeyDown (KeyCode.T)) 
		{
			hands[0].color = Color.yellow;
		}
		if (Input.GetKeyDown (KeyCode.Y)) 
		{
			hands[0].color = Color.green;
		}
		if (Input.GetKeyDown (KeyCode.U)) 
		{
			hands[0].color = Color.blue;
		}
		if (Input.GetKeyDown (KeyCode.I)) 
		{
			hands[0].color = Color.cyan;
		}
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			SceneManager.instance.asyncLoadNextSceneWithDelay (0f);
		}
	}

	void InitCanvasTexture()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.ARGB32, false);
		for (int i=0; i<canvasHeight; i++) 
		{
			for(int j=0;j<canvasWidth;j++)
			{
				Color color = Color.white;
				color.a = 0f;

				canvasTexture.SetPixel(j, i, color);
			}
		}
		canvasTexture.Apply ();

		if(canvasBgObject != null)
		{
			canvasBgObject.renderer.material.mainTexture = canvasBg;
		}

		canvasDrawableAreaColors = canvasDrawableArea.GetPixels ();
	}
	
	void InitPlayerHands()
	{
		for (int i = 0; i<maxHands; i++) 
		{
			hands[i] = new PlayerHandData();
		}
	}

	public Texture2D GetCanvasTexture()
	{
		return canvasTexture;
	}

	public Texture2D GetCanvasBg()
	{
		return canvasBg;
	}

	public PlayerHandData[] GetPlayerHands()
	{
		return hands;
	}

	public void ChangeBrushRadius(int value)
	{
		brushRadius = value;
		brushShape = BrushShape.CreateCircle (value);
	}

	public void ChangeEraserRadius(int value) {
		eraserRadius = value;
		eraserShape = BrushShape.CreateCircle (eraserRadius);
	}

	public void SavePlayerCreation(Texture2D texture, PlayerCreation.CreationType type, int spriteNum)
	{
		savedPlayerCreations.Add(new PlayerCreation(texture, type, spriteNum));
	}

	public static List<PlayerCreation> GetSavedPlayerCreation()
	{
		return savedPlayerCreations;
	}
	
}
