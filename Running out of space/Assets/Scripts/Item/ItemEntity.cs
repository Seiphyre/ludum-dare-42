using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    public ItemDescription Description;
    public ItemVisual Visual;

    public ItemEntity ItemToContent;



    [ContextMenu("Initialize")]
    public void InitializeTest()
    {
        Visual = new ItemVisual(Description, Vector3.zero, transform);
    }

    public void Initialize(ItemDescription desc)
    {
        Description = desc;
        Visual = new ItemVisual(Description, Vector3.zero, transform);
    }

    [ContextMenu("ShowContent")]
    public void ShowContentCanvas()
    {
        ContainerUI containerUI = LevelManager.Instance.GetContainerCanvas();
        containerUI.Show(transform.position + Vector3.up * 1.5f);
        containerUI.Initialize(Visual);
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

    [ContextMenu("TestSetToContent")]
    public void Test()
    {
        AddItemToContainer(ItemToContent);
    }

    internal void AddItemToContainer(ItemEntity item)
    {
        if (Description.ContainerType != ItemContainerType.Containing)
        {
            Debug.LogError("The Item is not a container");
            return;
        }
        else if (item.Description.ContainerType != ItemContainerType.Content)
        {
            Debug.LogError("The item cannot be contained");
            return;
        }

        Visual.AddItemToContent(item);
    }
}
