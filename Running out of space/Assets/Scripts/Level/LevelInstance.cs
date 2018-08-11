using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstance : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public List<ItemType> Items;
    public int SpawnDelay = 10;

    internal void SpawnItem()
    {
        ItemEntity item = Instantiate(LevelManager.Instance.ItemEntityPrefab).GetComponent<ItemEntity>();
        item.Initialize(LevelManager.Instance.Factory.GetDescription(Items[0]));
        item.transform.position = SpawnPoints[0].position;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
