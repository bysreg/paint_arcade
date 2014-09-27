using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpawnAnimations : MonoBehaviour {

	public Transform bird; // transform component required
	public Transform house;
	public Transform human;

	void Start()
	{
		List<PlayerCreation> creations = GameController.GetSavedPlayerCreation ();

		if (creations == null)
			return;

		for (int i=0; i<creations.Count; i++) 
		{
			if(creations[i].type == PlayerCreation.CreationType.Bird)
			{
				SpawnCreation(bird, creations[i]);
			}
			else if(creations[i].type == PlayerCreation.CreationType.Human)
			{
				SpawnCreation(human, creations[i]);
			}
			else if(creations[i].type == PlayerCreation.CreationType.House)
			{
				SpawnCreation(house, creations[i]);
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
}
