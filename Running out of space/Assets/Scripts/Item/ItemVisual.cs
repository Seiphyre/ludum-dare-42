using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVisual
{
    public ItemVisual(ItemDescription description, Vector3 position, Transform parent = null)
    {
        m_description = description;
        if (HasVisual())
        {
            if (parent != null)
            {
                m_visualGameObject.transform.parent = parent;
                m_visualGameObject.transform.localPosition = position;
            }
            else
            {
                m_visualGameObject.transform.position = position;
            }

            if (m_description.ContainerType == ItemContainerType.Containing)
            {
                m_content = new ItemContent();
                if (m_description.HideContent)
                {
                    for (int i = 0; i < m_description.ContainSize; i++)
                    {
                        m_content.AddSlot(m_visualGameObject.transform);
                    }
                }
                else
                {
                    for (int i = 0; i < m_visualGameObject.transform.childCount; i++)
                    {
                        if (m_visualGameObject.transform.GetChild(i).tag == "Slot")
                        {
                            m_content.AddSlot(m_visualGameObject.transform.GetChild(i));
                        }
                    }
                }
            }
        }
    }

    internal GameObject GetObject()
    {
        if (HasVisual())
        {
            return m_visualGameObject;
        }
        else
        {
            return null;
        }
    }

    internal void SetPosition(Vector3 position)
    {
        m_visualGameObject.transform.position = position;
    }

    internal void Rotate(bool right)
    {
        if (HasVisual())
        {
            int dir = right ? -90 : 90;
            m_visualGameObject.transform.localRotation *= Quaternion.Euler(0, dir, 0);
        }
    }

    internal void AddItemToContent(ItemEntity item)
    {
        if (item == null)
        {
            Debug.LogError("The Item is null");
            return;
        }
        else if (m_description.ContainerType != ItemContainerType.Containing)
        {
            Debug.LogError("The Item is not a container");
            return;
        }
        else if (item.Description.ContainerType != ItemContainerType.Content)
        {
            Debug.LogError("The item cannot be contained");
            return;
        }

        if (m_content.CanFit(item.Description.ContentSize))
        {
            m_content.AddItem(item, m_description);
        }
    }

    internal bool HasItemInContent()
    {
        foreach (var contentItem in m_content.Items)
        {
            if (contentItem != null)
            {
                return true;
            }
        }

        return false;
    }

    internal List<ItemEntity> GetContent()
    {
        return m_content.Items;
    }

    internal bool RemoveContent(ItemEntity content)
    {
        return m_content.RemoveItem(content);
    }

    protected bool HasVisual()
    {
        if (m_visualGameObject != null)
        {
            return true;
        }
        else
        {
            return CreateVisual();
        }
    }

    protected bool CreateVisual()
    {
        if (m_description.Prefab != null)
        {
            m_visualGameObject = GameObject.Instantiate(m_description.Prefab);
            return true;
        }
        else
        {
            Debug.LogError("Item description doesn't have a Prefab");
            return false;
        }
    }

    internal int GetEmptySlotsCount()
    {
        return m_content.GetEmptySlotsCount();
    }

    private GameObject m_visualGameObject;
    private ItemDescription m_description;
    private ItemContent m_content;

    private class ItemContent
    {
        internal ItemContent()
        {
            Slots = new List<Transform>();
            Items = new List<ItemEntity>();
            SlotsAvailable = new List<bool>();
            ItemSlotIndex = new Dictionary<ItemEntity, int>();
            EmptySlots = 0;
        }

        internal bool CanFit(int size)
        {
            return EmptySlots >= size;
        }

        internal int GetEmptySlotsCount()
        {
            return EmptySlots;
        }

        internal void AddSlot(Transform slot)
        {
            Slots.Add(slot);
            SlotsAvailable.Add(true);
            EmptySlots++;
        }

        internal void AddItem(ItemEntity item, ItemDescription container)
        {
            Items.Add(item);

            if (container.HideContent)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.transform.position = Slots[GetFreeSlot()].transform.position;
                ItemSlotIndex.Add(item, GetFreeSlot());
                SlotsAvailable[ItemSlotIndex[item]] = false;
            }

            EmptySlots -= item.Description.ContentSize;
        }

        internal bool RemoveItem(ItemEntity item)
        {
            if (!Items.Contains(item))
            {
                Debug.LogError("The given item is not a content of this container");
                return false;
            }

            SlotsAvailable[ItemSlotIndex[item]] = true;

            Items.Remove(item);

            ItemSlotIndex.Remove(item);

            item.gameObject.SetActive(true);

            EmptySlots += item.Description.ContentSize;

            return true;
        }

        protected int GetFreeSlot()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (SlotsAvailable[i] == true)
                    return i;
            }

            return -1;
        }

        internal List<Transform> Slots;

        internal List<bool> SlotsAvailable;

        internal List<ItemEntity> Items;

        internal Dictionary<ItemEntity, int> ItemSlotIndex;

        protected int EmptySlots;
    }
}
