using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVisual
{
    public ItemVisual(ItemDescription description)
    {
        m_description = description;
    }

    internal GameObject GetItem()
    {
        if (HasItem())
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
        if (HasItem())
        {
            int dir = right ? -90 : 90;
            m_visualGameObject.transform.localRotation *= Quaternion.Euler(0, dir, 0);
        }
    }

    internal bool HasItem()
    {
        if (m_visualGameObject != null)
        {
            return true;
        }
        else
        {
            return CreateItem();
        }
    }

    protected bool CreateItem()
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
}
