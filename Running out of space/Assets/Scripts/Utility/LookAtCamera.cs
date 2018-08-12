using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	Transform target;

    // Use this for initialization
    void Start()
    {
        target = LevelManager.Instance.Camera.transform.GetChild(0).GetChild(0);
		if(target == null)
		Debug.LogError("No target for " + gameObject.name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
            transform.forward = target.forward;
    }
}
