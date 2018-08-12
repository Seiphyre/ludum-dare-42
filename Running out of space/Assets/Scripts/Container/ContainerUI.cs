using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    public GameObject LockPanel;
    public Transform Select;

    public Selectable ObjectSelected;

    public List<ContentUI> Content;

    public bool Locked
    {
        get { return LockPanel.activeSelf; }
    }

    public void Initialize(ItemVisual itemVisual)
    {
        List<ItemEntity> items = itemVisual.GetContent();
        m_itemVisual = itemVisual;

        for (int i = 0; i < Content.Count; i++)
        {
            if (items.Count > i)
                Content[i].Item = items[i];
            else
                Content[i].Item = null;

            Content[i].UpdateSprite();
        }

        if (Player.GetInstance().Hand.CurrentObject != null && !CheckEmptySlot())
        {
            LockPanel.SetActive(true);
        }
        else
        {
            LockPanel.SetActive(false);
        }
    }

    public void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        ObjectSelected.Select();
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
        if (Player.GetInstance().Hand.CurrentObject != null)
        {
            if (!Locked)
            {
                AddContent(index);
            }
        }
        else
        {
            RemoveContent(index);
        }
    }

    internal void AddContent(int index)
    {
        index = GetNextAvailableIndex();
        ItemEntity item = Player.GetInstance().Hand.GetAndRemoveCurrentObject();
        m_itemVisual.AddItemToContent(item);
        Initialize(m_itemVisual);
    }

    internal void RemoveContent(int index)
    {
        if (m_itemVisual.RemoveContent(Content[index].Item))
        {
            //Function to set item in player hand
            Player.GetInstance().Hand.TakeObject(Content[index].Item);
            Initialize(m_itemVisual);
        }
    }

    internal int GetNextAvailableIndex()
    {
        for (int i = 0; i < Content.Count; i++)
        {
            if (!Content[i].HasItem())
            {
                return i;
            }
        }

        return Content.Count - 1;
    }

    internal bool CheckEmptySlot()
    {
        int index = 0;
        foreach (ContentUI contentCase in Content)
        {
            if (index > m_itemVisual.GetEmptySlotsCount())
            {
                return false;
            }
            if (!contentCase.HasItem())
            {
                return true;
            }
            index++;
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