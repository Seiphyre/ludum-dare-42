using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    public GameObject LockPanel;
    public Transform Select;

    public List<ContentUI> Content;

    public void Initialize(ItemVisual itemVisual)
    {
        List<ItemEntity> items = itemVisual.GetContent();

        for (int i = 0; i < Content.Count; i++)
        {
            if (items.Count > i)
                Content[i].Item = items[i];
            else
                Content[i].Item = null;

            Content[i].UpdateSprite();
        }
        //Check if player has item in hand
        LockPanel.SetActive(!CheckEmptySlot());

        m_itemVisual = itemVisual;
    }

    public void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSelectPosition(Transform contentCase)
    {
        Select.position = contentCase.position;
    }

    public void AddOrRemoveContent(int index)
    {
        if (Content[index].Item == null)
            AddContent(index);
        else
            RemoveContent(index);
    }

    internal void AddContent(int index)
    {
        //GetItemFromPlayer
        //m_itemVisual.AddItemToContent();
        Initialize(m_itemVisual);
    }

    internal void RemoveContent(int index)
    {
        if (m_itemVisual.RemoveContent(Content[index].Item))
        {
            //Function to set item in player hand
            Initialize(m_itemVisual);
        }
    }

    internal bool CheckEmptySlot()
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

    private ItemVisual m_itemVisual;
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