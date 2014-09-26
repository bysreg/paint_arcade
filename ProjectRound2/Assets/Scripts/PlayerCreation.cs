using UnityEngine;
using System.Collections;

public class PlayerCreation : MonoBehaviour {

	public readonly Texture2D texture;
	public readonly CreationType type;

	public enum CreationType
	{
		Bird = 0, 
		Human, 
		House,
	}

	public PlayerCreation(Texture2D texture, CreationType type)
	{
		this.texture = texture;
		this.type = type;
	}
}
