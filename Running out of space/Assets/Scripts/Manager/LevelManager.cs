using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<LevelInstance> Levels;

    public ItemFactory Factory;

    public GameObject ItemEntityPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Levels[m_currentLevel].SpawnItem();
        }
    }

    int m_currentLevel;
}
