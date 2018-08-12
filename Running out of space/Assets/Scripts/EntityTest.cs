using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTest : MonoBehaviour {

	// Use this for initialization
	private void Awake()
	{
		GetComponent<ItemEntity>().InitializeTest();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
