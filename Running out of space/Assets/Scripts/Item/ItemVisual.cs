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
        if (m_content.CanFit(item.Description.ContentSize))
        {
            m_content.AddItem(item, m_description);
        }
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

    private GameObject m_visualGameObject;
    private ItemDescription m_description;
    private ItemContent m_content;

    private class ItemContent
    {
        internal ItemContent()
        {
            Slots = new List<Transform>();
            Item = new List<ItemEntity>();
            EmptySlots = 0;
        }

        internal bool CanFit(int size)
        {
            return EmptySlots >= size;
        }

        internal void AddSlot(Transform slot)
        {
            Slots.Add(slot);
            EmptySlots++;
        }

        internal void AddItem(ItemEntity item, ItemDescription container)
        {
            Item.Add(item);

            if (container.HideContent)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.transform.position = Slots[Item.IndexOf(item)].transform.position;
            }

            EmptySlots -= item.Description.ContentSize;
        }

        internal List<Transform> Slots;

        internal List<ItemEntity> Item;

        protected int EmptySlots;
    }
}
