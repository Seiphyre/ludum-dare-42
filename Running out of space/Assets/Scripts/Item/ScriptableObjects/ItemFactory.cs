using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "ItemData/Factory", order = 1)]
public class ItemFactory : ScriptableObject
{
    public List<ItemDescription> Descriptions;

    public ItemDescription GetDescription(ItemType type)
    {
        foreach (ItemDescription desc in Descriptions)
        {
            if (desc.ItemType == type)
            {
                return desc;
            }
        }

        Debug.LogError("No item corresponding to type in ItemFactory");
        return null;
    }

    public Sprite GetItemSprite(ItemType type)
    {
        return GetDescription(type).Sprite;
    }
}