using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    public ItemDescription Description;
    public ItemVisual Visual;

    public void Initialize(ItemDescription desc)
    {
        Description = desc;
        Visual = new ItemVisual(Description);
        Visual.GetItem().transform.parent = transform;
        Visual.GetItem().transform.position = Vector3.zero;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Visual.Rotate(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Visual.Rotate(false);
        }
    }

    protected void CanContain()
    {

    }
}
