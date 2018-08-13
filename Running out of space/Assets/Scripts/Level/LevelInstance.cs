using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstance : MonoBehaviour
{
    public Transform SpawnPlayer;
    public List<Transform> SpawnPoints;
    public List<ItemType> Items;

    public float LevelArmatureSpawnDelay = 0.5f;
    public int ItemSpawnDelay = 10;
    public float SpawnHeight = 5f;

    public Vector3 MapDimension;

    public List<WallStates> WallNorth, WallEast, WallSouth, WallWest;

    void Awake()
    {
        m_itemLeft = Items;

        m_Lose = false;
        LevelManager.Instance.Camera.SetWallMeshs(WallNorth, WallEast, WallSouth, WallWest);
    }

    void Start()
    {
        MainGameplayUI.Instance.SetItemCount(ItemLeftCount());
    }

    public void CleanScene()
    {
        foreach (var item in m_itemSpawned)
        {
            Destroy(item.gameObject);
        }
        m_itemSpawned.Clear();
    }

    #region GameLoop
    public IEnumerator SpawnLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "MoveIgnore")
            {
                transform.GetChild(i).localPosition += new Vector3(0, SpawnHeight, 0f);
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "MoveIgnore")
            {
                StartCoroutine(FallObject(transform.GetChild(i), transform.GetChild(i).localPosition - new Vector3(0, SpawnHeight, 0f)));
                yield return new WaitForSeconds(LevelArmatureSpawnDelay);
            }
        }
    }

    public void StartLevel()
    {
        StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop()
    {
        while (m_itemLeft.Count != 0)
        {
            if (m_Lose == true)
                break;

            SpawnItem();

            MainGameplayUI.Instance.SetItemCount(ItemLeftCount());

            yield return new WaitForSeconds(ItemSpawnDelay);
        }

        if (m_Lose)
        {
            MainGameplayUI.Instance.ShowGameOverCanvas();
        }
        else
        {
            MainGameplayUI.Instance.ShowWinCanvas();
        }

        yield break;
    }

    #endregion GameLoop

    #region Items

    public int ItemLeftCount()
    {
        return m_itemLeft.Count;
    }
    internal void SpawnItem()
    {
        Vector3 position = Vector3.zero;
        if (GetFreeSpawnPosition(out position))
        {
            ItemEntity item = Instantiate(LevelManager.Instance.ItemEntityPrefab).GetComponent<ItemEntity>();
            item.Initialize(LevelManager.Instance.Factory.GetDescription(m_itemLeft[0]));
            item.transform.position = position;
            StartCoroutine(FallObject(item.Visual.GetObject().transform, Vector3.zero));

            GridManager.GetInstance().FitObjectInGrid(item);
            GridManager.GetInstance().AddObject(item);

            m_itemLeft.RemoveAt(0);
            m_itemSpawned.Add(item);
        }
        else
        {
            m_Lose = true;
        }
    }

    IEnumerator FallObject(Transform item, Vector3 position)
    {
        Vector3 newPosition = position + new Vector3(0, SpawnHeight, 0);
        item.localPosition = newPosition;
        float step = 50f;
        for (int i = 0; i < step; i++)
        {
            newPosition = position + new Vector3(0, SpawnHeight - (float)i / (step / SpawnHeight), 0);
            item.localPosition = newPosition;
            yield return new WaitForSeconds(0.01f);
        }

        item.localPosition = position;

        yield break;
    }

    protected bool GetFreeSpawnPosition(out Vector3 position)
    {
        ItemEntity entity;
        position = Vector3.zero;
        foreach (var spawn in SpawnPoints)
        {
            if (!GridManager.GetInstance().IsCollideWithAnOtherObject(spawn.position, out entity))
            {
                position = spawn.position;
                return true;
            }
        }

        return false;
    }

    #endregion Items

    private bool m_Lose;
    private List<ItemEntity> m_itemSpawned = new List<ItemEntity>();
    private List<ItemType> m_itemLeft;
}
