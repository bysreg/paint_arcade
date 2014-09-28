using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SavedCreations : MonoBehaviour {

	public static SavedCreations creations;
	List<PlayerCreation> savedPlayerCreations;

	void Awake()
	{
		if (creations == null) 
		{
			DontDestroyOnLoad(gameObject);
			creations = this;
		}
		else if(creations != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		savedPlayerCreations = new List<PlayerCreation> ();
	}

	public void SavePlayerCreation(Texture2D texture, PlayerCreation.CreationType type, int spriteNum)
	{
		savedPlayerCreations.Add(new PlayerCreation(texture, type, spriteNum));
		print ("count : " + savedPlayerCreations.Count);
	}

	public List<PlayerCreation> GetSavedPlayerCreation()
	{
		return savedPlayerCreations;
	}
}
