using UnityEngine;
using System.Collections;

public class PlayerCreation : MonoBehaviour {

	public readonly Texture2D texture;
	public readonly CreationType type;
	public readonly int spriteNum;

	public enum CreationType
	{
		Bird = 0, 
		Human, 
		House,
	}

	public PlayerCreation(Texture2D texture, CreationType type, int spriteNum)
	{
		this.texture = texture;
		this.type = type;
		this.spriteNum = spriteNum;
	}
}
