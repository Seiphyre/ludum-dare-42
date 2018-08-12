using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{

    public Transform target;

    // Use this for initialization
    void Start()
    {
        if (target == null)
            Debug.LogError("No target for " + gameObject.name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
            transform.forward = target.forward;
    }
}
