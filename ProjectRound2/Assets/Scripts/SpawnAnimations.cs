using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpawnAnimations : MonoBehaviour {

	public Transform bird; // transform component required
	public Transform house;
	public Transform human;

	public Transform[] targetBirds;
	public Transform[] targetHouses;
	public Transform[] targetHumans;

	void Start()
	{
		List<PlayerCreation> creations = GameController.GetSavedPlayerCreation ();

		if (creations == null)
			return;

		for (int i=0; i<creations.Count; i++) 
		{
			if(creations[i].type == PlayerCreation.CreationType.Bird)
			{
				ReplaceTexture(targetBirds, creations[i]);
			}
			else if(creations[i].type == PlayerCreation.CreationType.Human)
			{
				ReplaceTexture(targetHumans, creations[i]);
			}
			else if(creations[i].type == PlayerCreation.CreationType.House)
			{
				ReplaceTexture(targetHouses, creations[i]);
            }
		}
	}

	void SpawnCreation(Transform prefab, PlayerCreation creation)
	{
		Transform clone = Instantiate(prefab) as Transform;
		clone.position = Vector3.zero;
		Transform[] objContents = new Transform[creation.spriteNum];

		ColorCapture2D.FindChildAndSetData(objContents, clone);
		ColorCapture2D.CombineTexture2DAndGameObject (creation.spriteNum, objContents, creation.texture);
	}

	void ReplaceTexture(Transform[] targets, PlayerCreation creation)
	{
		for(int i=0;i<targets.Length;i++)
		{
			Transform[] objContents = new Transform[creation.spriteNum];
			
			ColorCapture2D.FindChildAndSetData(objContents, targets[i]);
			ColorCapture2D.CombineTexture2DAndGameObject (creation.spriteNum, objContents, creation.texture);
		}
	}
}
