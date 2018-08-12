using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Hand Hand;

	#region -- Singleton ----------------------------

	private static Player Instance;

	public static Player GetInstance()
	{
		if (Instance == null)
		{
			Instance = FindObjectOfType<Player>();

			if (Instance == null)
			{
				GameObject GridManager = new GameObject();
				Instance = GridManager.AddComponent<Player>();
			}
		}

		return Instance;
	}

	#endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
