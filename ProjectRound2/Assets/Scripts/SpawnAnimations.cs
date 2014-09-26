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
				Instantiate(bird);
				bird.position = Vector3.zero;
			}
			else if(creations[i].type == PlayerCreation.CreationType.Human)
			{
				Instantiate(human);
				human.position = Vector3.zero;
			}
			else if(creations[i].type == PlayerCreation.CreationType.House)
			{
				Instantiate(house);
				house.position = Vector3.zero;
            }
		}


	}

}
