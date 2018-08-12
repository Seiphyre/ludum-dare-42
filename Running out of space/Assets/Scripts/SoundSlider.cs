using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Slider>().value = SoundManager.GetInstance().Volume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
