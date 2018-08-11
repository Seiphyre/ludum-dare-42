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
        StartCoroutine(FallObject(item.Visual.GetObject().transform));
    }

    void Start()
    {

    }

    void Update()
    {

    }

    IEnumerator FallObject(Transform item)
    {
        Vector3 newPosition = new Vector3(0, 10f, 0);
        item.localPosition = newPosition;
        float step = 50f;
        for (int i = 0; i < step; i++)
        {
            newPosition = new Vector3(0, 10f - (float)i / (step / 10f), 0);
            item.localPosition = newPosition;
            yield return new WaitForSeconds(0.01f);
        }

        item.localPosition = Vector3.zero;

        yield break;
    }
}
