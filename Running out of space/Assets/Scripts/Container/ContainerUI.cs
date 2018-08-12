using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    public GameObject LockPanel;
    public Transform Select;

    public List<ContentUI> Content;

    void Initialize(List<ItemEntity> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Content[i].Item = items[i];
            Content[i].UpdateSprite();
        }
        //Check if player has item in hand
        LockPanel.SetActive(CheckEmptySlot());
    }

    public void SetSelectPosition(Transform contentCase)
    {
        Select.position = contentCase.position;
    }

    public bool CheckEmptySlot()
    {
        foreach (ContentUI contentCase in Content)
        {
            if (!contentCase.HasItem())
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class ContentUI
{
    public Image Image;
    public ItemEntity Item;

    public void UpdateSprite()
    {
        Image.enabled = Item != null;

        if (Image.enabled)
        {
            Image.sprite = LevelManager.Instance.Factory.GetItemSprite(Item.Description.ItemType);
        }
    }

    public bool HasItem()
    {
        return Item != null;
    }
}