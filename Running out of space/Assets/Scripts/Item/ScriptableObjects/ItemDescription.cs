using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDescription", menuName = "ItemData/Description", order = 1)]
public class ItemDescription : ScriptableObject
{
    public GameObject Prefab;

    public ItemType ItemType;
    public Sprite Sprite;
    public ItemContainerType ContainerType;
    public bool HideContent;
    public int ContainSize;
    public int ContentSize;
    public Vector3 Size;
}